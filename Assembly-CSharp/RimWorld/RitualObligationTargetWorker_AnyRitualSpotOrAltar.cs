using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200169D RID: 5789
	public class RitualObligationTargetWorker_AnyRitualSpotOrAltar : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x0600911D RID: 37149 RVA: 0x00352B4A File Offset: 0x00350D4A
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar()
		{
		}

		// Token: 0x0600911E RID: 37150 RVA: 0x00352B52 File Offset: 0x00350D52
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600911F RID: 37151 RVA: 0x00352B5B File Offset: 0x00350D5B
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_AnyRitualSpotOrAltar.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_AnyRitualSpotOrAltar.<GetTargets>d__2(-2);
			<GetTargets>d__.<>4__this = this;
			<GetTargets>d__.<>3__obligation = obligation;
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x06009120 RID: 37152 RVA: 0x00352B7C File Offset: 0x00350D7C
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			if (target.Thing.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x06009121 RID: 37153 RVA: 0x00352BCA File Offset: 0x00350DCA
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			RitualObligationTargetWorker_AnyRitualSpotOrAltar.<GetTargetInfos>d__4 <GetTargetInfos>d__ = new RitualObligationTargetWorker_AnyRitualSpotOrAltar.<GetTargetInfos>d__4(-2);
			<GetTargetInfos>d__.<>4__this = this;
			return <GetTargetInfos>d__;
		}
	}
}
