using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001696 RID: 5782
	public class RitualObligationTargetWorker_Thing : RitualObligationTargetFilter
	{
		// Token: 0x060090F2 RID: 37106 RVA: 0x0035260E File Offset: 0x0035080E
		public RitualObligationTargetWorker_Thing()
		{
		}

		// Token: 0x060090F3 RID: 37107 RVA: 0x00352616 File Offset: 0x00350816
		public RitualObligationTargetWorker_Thing(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x060090F4 RID: 37108 RVA: 0x0035261F File Offset: 0x0035081F
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_Thing.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_Thing.<GetTargets>d__2(-2);
			<GetTargets>d__.<>3__obligation = obligation;
			return <GetTargets>d__;
		}

		// Token: 0x060090F5 RID: 37109 RVA: 0x0035262F File Offset: 0x0035082F
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			return target.HasThing && target.Thing == obligation.targetA.Thing;
		}

		// Token: 0x060090F6 RID: 37110 RVA: 0x00352656 File Offset: 0x00350856
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			RitualObligationTargetWorker_Thing.<GetTargetInfos>d__4 <GetTargetInfos>d__ = new RitualObligationTargetWorker_Thing.<GetTargetInfos>d__4(-2);
			<GetTargetInfos>d__.<>3__obligation = obligation;
			return <GetTargetInfos>d__;
		}
	}
}
