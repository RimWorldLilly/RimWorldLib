using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000EA RID: 234
	public static class WildManUtility
	{
		// Token: 0x0600087E RID: 2174 RVA: 0x000306F6 File Offset: 0x0002E8F6
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan && !p.IsSubhuman;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00030710 File Offset: 0x0002E910
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.IsAnimal || p.IsWildMan();
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00030722 File Offset: 0x0002E922
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00030739 File Offset: 0x0002E939
		public static bool WildManShouldReachOutsideNow(Pawn p)
		{
			return p.IsWildMan() && !p.mindState.WildManEverReachedOutside && (!p.IsPrisoner || p.guest.Released);
		}
	}
}
