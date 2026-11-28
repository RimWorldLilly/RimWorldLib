using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001672 RID: 5746
	public class StageFailTrigger_TargetNotLit : StageFailTrigger
	{
		// Token: 0x06009030 RID: 36912 RVA: 0x0034F3AC File Offset: 0x0034D5AC
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
			CompRefuelable compRefuelable = (thing2 != null) ? thing2.TryGetComp<CompRefuelable>() : null;
			return compRefuelable == null || !compRefuelable.HasFuel;
		}

		// Token: 0x06009031 RID: 36913 RVA: 0x0034F408 File Offset: 0x0034D608
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x040057FD RID: 22525
		public ThingDef onlyIfTargetIsOfDef;
	}
}
