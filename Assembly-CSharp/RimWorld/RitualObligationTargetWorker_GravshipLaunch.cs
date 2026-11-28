using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016A9 RID: 5801
	public class RitualObligationTargetWorker_GravshipLaunch : RitualObligationTargetFilter
	{
		// Token: 0x06009158 RID: 37208 RVA: 0x003536DF File Offset: 0x003518DF
		public RitualObligationTargetWorker_GravshipLaunch()
		{
		}

		// Token: 0x06009159 RID: 37209 RVA: 0x003536E7 File Offset: 0x003518E7
		public RitualObligationTargetWorker_GravshipLaunch(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600915A RID: 37210 RVA: 0x003536F0 File Offset: 0x003518F0
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_GravshipLaunch.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_GravshipLaunch.<GetTargets>d__2(-2);
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x0600915B RID: 37211 RVA: 0x00353700 File Offset: 0x00351900
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			CompPilotConsole compPilotConsole;
			if (!target.Thing.TryGetComp(out compPilotConsole))
			{
				return false;
			}
			AcceptanceReport acceptanceReport = compPilotConsole.CanUseNow();
			if (!acceptanceReport.Accepted)
			{
				return acceptanceReport.Reason;
			}
			return true;
		}

		// Token: 0x0600915C RID: 37212 RVA: 0x00353757 File Offset: 0x00351957
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			return new RitualObligationTargetWorker_GravshipLaunch.<GetTargetInfos>d__4(-2);
		}
	}
}
