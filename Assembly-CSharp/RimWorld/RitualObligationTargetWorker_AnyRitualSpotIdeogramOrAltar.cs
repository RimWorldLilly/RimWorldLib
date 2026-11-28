using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200169E RID: 5790
	public class RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x06009122 RID: 37154 RVA: 0x00352BDA File Offset: 0x00350DDA
		public RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar()
		{
		}

		// Token: 0x06009123 RID: 37155 RVA: 0x00352BE2 File Offset: 0x00350DE2
		public RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009124 RID: 37156 RVA: 0x00352BEB File Offset: 0x00350DEB
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar.<GetTargets>d__2(-2);
			<GetTargets>d__.<>4__this = this;
			<GetTargets>d__.<>3__obligation = obligation;
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x06009125 RID: 37157 RVA: 0x00352C0C File Offset: 0x00350E0C
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Thing thing = target.Thing;
			if (thing.def == ThingDefOf.RitualSpot || thing.def == ThingDefOf.Ideogram)
			{
				return true;
			}
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x06009126 RID: 37158 RVA: 0x00352C69 File Offset: 0x00350E69
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar.<GetTargetInfos>d__4 <GetTargetInfos>d__ = new RitualObligationTargetWorker_AnyRitualSpotIdeogramOrAltar.<GetTargetInfos>d__4(-2);
			<GetTargetInfos>d__.<>4__this = this;
			return <GetTargetInfos>d__;
		}
	}
}
