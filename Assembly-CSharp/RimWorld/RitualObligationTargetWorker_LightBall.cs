using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016A2 RID: 5794
	public class RitualObligationTargetWorker_LightBall : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x06009134 RID: 37172 RVA: 0x00352E69 File Offset: 0x00351069
		public RitualObligationTargetWorker_LightBall()
		{
		}

		// Token: 0x06009135 RID: 37173 RVA: 0x00352E71 File Offset: 0x00351071
		public RitualObligationTargetWorker_LightBall(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009136 RID: 37174 RVA: 0x00352E7C File Offset: 0x0035107C
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!ModLister.CheckIdeology("Lightball target"))
			{
				return false;
			}
			if (!base.CanUseTargetInternal(target, obligation).canUse)
			{
				return false;
			}
			Thing thing = target.Thing;
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			if (compPowerTrader != null)
			{
				if (compPowerTrader.PowerNet == null || !compPowerTrader.PowerNet.HasActivePowerSource)
				{
					return "RitualTargetLightBallIsNotPowered".Translate();
				}
				List<Thing> forCell = target.Map.listerBuldingOfDefInProximity.GetForCell(target.Cell, (float)this.def.maxSpeakerDistance, ThingDefOf.Loudspeaker, null);
				bool flag = false;
				foreach (Thing thing2 in forCell)
				{
					CompPowerTrader compPowerTrader2 = thing2.TryGetComp<CompPowerTrader>();
					if (thing2.GetRoom(RegionType.Set_All) == thing.GetRoom(RegionType.Set_All) && compPowerTrader2.PowerNet != null && compPowerTrader2.PowerNet.HasActivePowerSource)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return "RitualTargetNoPoweredSpeakers".Translate();
				}
			}
			return true;
		}

		// Token: 0x06009137 RID: 37175 RVA: 0x00352FA0 File Offset: 0x003511A0
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			return new RitualObligationTargetWorker_LightBall.<GetTargetInfos>d__3(-2);
		}
	}
}
