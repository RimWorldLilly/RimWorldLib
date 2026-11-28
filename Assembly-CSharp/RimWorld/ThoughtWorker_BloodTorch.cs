using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E86 RID: 3718
	public class ThoughtWorker_BloodTorch : ThoughtWorker
	{
		// Token: 0x0600624F RID: 25167 RVA: 0x002356F0 File Offset: 0x002338F0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (p.Suspended)
			{
				return false;
			}
			Room pawnRoom = p.GetRoom(RegionType.Set_All);
			if (GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(ThingDefOf.SanguphageMeetingTorch), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 8f, delegate(Thing b)
			{
				Room room = b.GetRoom(RegionType.Set_All);
				if (room != null && room != pawnRoom)
				{
					return false;
				}
				CompGlower compGlower = b.TryGetComp<CompGlower>();
				return compGlower != null && compGlower.Glows;
			}, null, 0, -1, false, RegionType.Set_Passable, false, false) == null)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(0);
		}

		// Token: 0x04003623 RID: 13859
		private const float MaxDist = 8f;
	}
}
