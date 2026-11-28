using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200172B RID: 5931
	public class RitualTargetFilter_Altar : RitualTargetFilter
	{
		// Token: 0x060093E7 RID: 37863 RVA: 0x0035EC8C File Offset: 0x0035CE8C
		public RitualTargetFilter_Altar()
		{
		}

		// Token: 0x060093E8 RID: 37864 RVA: 0x0035EC94 File Offset: 0x0035CE94
		public RitualTargetFilter_Altar(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x060093E9 RID: 37865 RVA: 0x0035ECA0 File Offset: 0x0035CEA0
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			TargetInfo targetInfo = this.BestTarget(initiator, selectedTarget);
			rejectionReason = "";
			if (!targetInfo.IsValid)
			{
				rejectionReason = "AbilityDisabledNoAltarOrRitualsSpot".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x060093EA RID: 37866 RVA: 0x0035ECDC File Offset: 0x0035CEDC
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			Pawn pawn = initiator.Thing as Pawn;
			if (pawn == null)
			{
				return null;
			}
			Thing thing = null;
			float num = 99999f;
			foreach (Building building in this.CandidateBuildings(pawn.Ideo, initiator.Thing.Map))
			{
				if (building.def.isAltar && pawn.CanReach(building, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
				{
					int lengthHorizontalSquared = (pawn.Position - building.Position).LengthHorizontalSquared;
					if ((float)lengthHorizontalSquared < num)
					{
						thing = building;
						num = (float)lengthHorizontalSquared;
					}
				}
			}
			if (thing == null && this.def.fallbackToRitualSpot && pawn.Map != null)
			{
				foreach (Thing thing2 in pawn.Map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot))
				{
					if (pawn.CanReach(thing2, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						int lengthHorizontalSquared2 = (pawn.Position - thing2.Position).LengthHorizontalSquared;
						if ((float)lengthHorizontalSquared2 < num)
						{
							thing = thing2;
							num = (float)lengthHorizontalSquared2;
						}
					}
				}
			}
			return thing;
		}

		// Token: 0x060093EB RID: 37867 RVA: 0x0035EE5C File Offset: 0x0035D05C
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			return new RitualTargetFilter_Altar.<GetTargetInfos>d__4(-2);
		}

		// Token: 0x060093EC RID: 37868 RVA: 0x0035EE65 File Offset: 0x0035D065
		protected virtual IEnumerable<Building> CandidateBuildings(Ideo ideo, Map map)
		{
			RitualTargetFilter_Altar.<CandidateBuildings>d__5 <CandidateBuildings>d__ = new RitualTargetFilter_Altar.<CandidateBuildings>d__5(-2);
			<CandidateBuildings>d__.<>3__ideo = ideo;
			<CandidateBuildings>d__.<>3__map = map;
			return <CandidateBuildings>d__;
		}
	}
}
