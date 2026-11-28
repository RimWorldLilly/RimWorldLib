using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D3A RID: 3386
	public static class SocialProperness
	{
		// Token: 0x06005EF0 RID: 24304 RVA: 0x0022B5B5 File Offset: 0x002297B5
		public static bool IsSociallyProper(this Thing t, Pawn p)
		{
			return t.IsSociallyProper(p, p.IsPrisonerOfColony, false);
		}

		// Token: 0x06005EF1 RID: 24305 RVA: 0x0022B5C8 File Offset: 0x002297C8
		public static bool IsSociallyProper(this Thing t, Pawn p, bool forPrisoner, bool animalsCare = false)
		{
			if (!animalsCare && p != null && !p.RaceProps.Humanlike)
			{
				return true;
			}
			if (!t.def.socialPropernessMatters)
			{
				return true;
			}
			if (!t.Spawned)
			{
				return true;
			}
			IntVec3 intVec = t.def.hasInteractionCell ? t.InteractionCell : t.Position;
			if (forPrisoner)
			{
				return p == null || intVec.GetRoom(t.Map) == p.GetRoom(RegionType.Set_All);
			}
			if (ModsConfig.BiotechActive && t.def == ThingDefOf.HemogenPack)
			{
				return !SocialProperness.BloodfeedingPrisonerInRoom(t.GetRoom(RegionType.Set_All));
			}
			return !intVec.IsInPrisonCell(t.Map);
		}

		// Token: 0x06005EF2 RID: 24306 RVA: 0x0022B674 File Offset: 0x00229874
		public static bool BloodfeedingPrisonerInRoom(Room r)
		{
			if (r == null || !r.IsPrisonCell)
			{
				return false;
			}
			using (IEnumerator<Pawn> enumerator = r.Owners.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsBloodfeeder())
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
