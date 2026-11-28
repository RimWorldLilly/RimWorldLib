using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016A6 RID: 5798
	public class RitualObligationTargetWorker_UnconnectedGauranlenTree : RitualObligationTargetFilter
	{
		// Token: 0x06009147 RID: 37191 RVA: 0x00353380 File Offset: 0x00351580
		public RitualObligationTargetWorker_UnconnectedGauranlenTree()
		{
		}

		// Token: 0x06009148 RID: 37192 RVA: 0x00353388 File Offset: 0x00351588
		public RitualObligationTargetWorker_UnconnectedGauranlenTree(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009149 RID: 37193 RVA: 0x00353391 File Offset: 0x00351591
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_UnconnectedGauranlenTree.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_UnconnectedGauranlenTree.<GetTargets>d__2(-2);
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x0600914A RID: 37194 RVA: 0x003533A4 File Offset: 0x003515A4
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			Thing thing = target.Thing;
			CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
			if (compTreeConnection == null)
			{
				return false;
			}
			if (compTreeConnection.Connected)
			{
				return "RitualTargetConnectedGauranlenTree".Translate(thing.Named("TREE"), compTreeConnection.ConnectedPawn.Named("PAWN"));
			}
			if (compTreeConnection.ConnectionTorn)
			{
				return "RitualTargetConnectionTornGauranlenTree".Translate(thing.Named("TREE"), compTreeConnection.UntornInDurationTicks.ToStringTicksToPeriod(true, false, true, true, false)).Resolve().CapitalizeFirst();
			}
			return true;
		}

		// Token: 0x0600914B RID: 37195 RVA: 0x00353448 File Offset: 0x00351648
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			return new RitualObligationTargetWorker_UnconnectedGauranlenTree.<GetTargetInfos>d__4(-2);
		}
	}
}
