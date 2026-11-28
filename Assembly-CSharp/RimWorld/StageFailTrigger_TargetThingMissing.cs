using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001670 RID: 5744
	public class StageFailTrigger_TargetThingMissing : StageFailTrigger
	{
		// Token: 0x0600902A RID: 36906 RVA: 0x0034F2C0 File Offset: 0x0034D4C0
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
			return ritual.selectedTarget.Thing == null || ritual.selectedTarget.ThingDestroyed;
		}

		// Token: 0x0600902B RID: 36907 RVA: 0x0034F310 File Offset: 0x0034D510
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x040057FB RID: 22523
		public ThingDef onlyIfTargetIsOfDef;
	}
}
