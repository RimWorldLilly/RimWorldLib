using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200172C RID: 5932
	public class RitualTargetFilter_AltarOrIdeogram : RitualTargetFilter_Altar
	{
		// Token: 0x060093ED RID: 37869 RVA: 0x0035EE7C File Offset: 0x0035D07C
		public RitualTargetFilter_AltarOrIdeogram()
		{
		}

		// Token: 0x060093EE RID: 37870 RVA: 0x0035EE84 File Offset: 0x0035D084
		public RitualTargetFilter_AltarOrIdeogram(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x060093EF RID: 37871 RVA: 0x0035EE90 File Offset: 0x0035D090
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			TargetInfo targetInfo = this.BestTarget(initiator, selectedTarget);
			rejectionReason = "";
			if (!targetInfo.IsValid)
			{
				rejectionReason = "AbilityDisabledNoAltarIdeogramOrRitualsSpot".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x060093F0 RID: 37872 RVA: 0x0035EECA File Offset: 0x0035D0CA
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			return new RitualTargetFilter_AltarOrIdeogram.<GetTargetInfos>d__3(-2);
		}

		// Token: 0x060093F1 RID: 37873 RVA: 0x0035EED3 File Offset: 0x0035D0D3
		protected override IEnumerable<Building> CandidateBuildings(Ideo ideo, Map map)
		{
			RitualTargetFilter_AltarOrIdeogram.<CandidateBuildings>d__4 <CandidateBuildings>d__ = new RitualTargetFilter_AltarOrIdeogram.<CandidateBuildings>d__4(-2);
			<CandidateBuildings>d__.<>3__ideo = ideo;
			<CandidateBuildings>d__.<>3__map = map;
			return <CandidateBuildings>d__;
		}
	}
}
