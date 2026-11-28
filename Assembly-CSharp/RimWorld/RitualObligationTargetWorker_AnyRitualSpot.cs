using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200169C RID: 5788
	public class RitualObligationTargetWorker_AnyRitualSpot : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x06009118 RID: 37144 RVA: 0x00352AF6 File Offset: 0x00350CF6
		public RitualObligationTargetWorker_AnyRitualSpot()
		{
		}

		// Token: 0x06009119 RID: 37145 RVA: 0x00352AFE File Offset: 0x00350CFE
		public RitualObligationTargetWorker_AnyRitualSpot(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600911A RID: 37146 RVA: 0x00352B07 File Offset: 0x00350D07
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_AnyRitualSpot.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_AnyRitualSpot.<GetTargets>d__2(-2);
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x0600911B RID: 37147 RVA: 0x00352B17 File Offset: 0x00350D17
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			return target.Thing.def == ThingDefOf.RitualSpot;
		}

		// Token: 0x0600911C RID: 37148 RVA: 0x00352B41 File Offset: 0x00350D41
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			return new RitualObligationTargetWorker_AnyRitualSpot.<GetTargetInfos>d__4(-2);
		}
	}
}
