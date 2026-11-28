using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001671 RID: 5745
	public class StageFailTrigger_TargetUnpowered : StageFailTrigger
	{
		// Token: 0x0600902D RID: 36909 RVA: 0x0034F330 File Offset: 0x0034D530
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			if (this.onlyIfTargetIsOfDef != null)
			{
				Thing thing = ritual.selectedTarget.Thing;
				if (((thing != null) ? thing.def : null) != this.onlyIfTargetIsOfDef)
				{
					return false;
				}
			}
			Thing thing2 = ritual.selectedTarget.Thing;
			CompPowerTrader compPowerTrader = (thing2 != null) ? thing2.TryGetComp<CompPowerTrader>() : null;
			return compPowerTrader == null || !compPowerTrader.PowerOn;
		}

		// Token: 0x0600902E RID: 36910 RVA: 0x0034F38C File Offset: 0x0034D58C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x040057FC RID: 22524
		public ThingDef onlyIfTargetIsOfDef;
	}
}
