using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016A0 RID: 5792
	public class RitualObligationTargetWorker_IdeoBuildingOrRitualSpot : RitualObligationTargetFilter
	{
		// Token: 0x0600912B RID: 37163 RVA: 0x00352CF9 File Offset: 0x00350EF9
		public RitualObligationTargetWorker_IdeoBuildingOrRitualSpot()
		{
		}

		// Token: 0x0600912C RID: 37164 RVA: 0x00352D01 File Offset: 0x00350F01
		public RitualObligationTargetWorker_IdeoBuildingOrRitualSpot(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600912D RID: 37165 RVA: 0x00352D0A File Offset: 0x00350F0A
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			return Enumerable.Empty<TargetInfo>();
		}

		// Token: 0x0600912E RID: 37166 RVA: 0x00352D14 File Offset: 0x00350F14
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!ModLister.CheckIdeology("Ideo building target"))
			{
				return false;
			}
			Building building = target.Thing as Building;
			if (building == null || building.Faction == null || !building.Faction.IsPlayer)
			{
				return false;
			}
			if (building.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			for (int i = 0; i < this.parent.ideo.PreceptsListForReading.Count; i++)
			{
				Precept_Building precept_Building = this.parent.ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null && building.def == precept_Building.ThingDef)
				{
					return true;
				}
			}
			if (building.TryGetComp<CompGatherSpot>() != null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600912F RID: 37167 RVA: 0x00352DDD File Offset: 0x00350FDD
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			RitualObligationTargetWorker_IdeoBuildingOrRitualSpot.<GetTargetInfos>d__4 <GetTargetInfos>d__ = new RitualObligationTargetWorker_IdeoBuildingOrRitualSpot.<GetTargetInfos>d__4(-2);
			<GetTargetInfos>d__.<>4__this = this;
			return <GetTargetInfos>d__;
		}

		// Token: 0x06009130 RID: 37168 RVA: 0x00352DED File Offset: 0x00350FED
		public override List<string> MissingTargetBuilding(Ideo ideo)
		{
			return null;
		}
	}
}
