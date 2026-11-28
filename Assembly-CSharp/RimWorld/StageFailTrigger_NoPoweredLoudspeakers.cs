using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001673 RID: 5747
	public class StageFailTrigger_NoPoweredLoudspeakers : StageFailTrigger
	{
		// Token: 0x06009033 RID: 36915 RVA: 0x0034F428 File Offset: 0x0034D628
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
			if (thing2 == null || thing2.Destroyed)
			{
				return true;
			}
			foreach (Thing thing3 in thing2.Map.listerBuldingOfDefInProximity.GetForCell(thing2.Position, 12f, ThingDefOf.Loudspeaker, null))
			{
				CompPowerTrader compPowerTrader = thing3.TryGetComp<CompPowerTrader>();
				if (thing3.GetRoom(RegionType.Set_All) == thing2.GetRoom(RegionType.Set_All) && compPowerTrader.PowerNet != null && compPowerTrader.PowerNet.HasActivePowerSource)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009034 RID: 36916 RVA: 0x0034F504 File Offset: 0x0034D704
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
		}

		// Token: 0x040057FE RID: 22526
		public ThingDef onlyIfTargetIsOfDef;

		// Token: 0x040057FF RID: 22527
		public const int maxDistance = 12;
	}
}
