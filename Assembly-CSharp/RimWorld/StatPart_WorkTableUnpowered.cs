using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB5 RID: 8117
	public class StatPart_WorkTableUnpowered : StatPart
	{
		// Token: 0x0600CE04 RID: 52740 RVA: 0x004BA9F6 File Offset: 0x004B8BF6
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				val *= req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
			}
		}

		// Token: 0x0600CE05 RID: 52741 RVA: 0x004BAA2C File Offset: 0x004B8C2C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				float unpoweredWorkTableWorkSpeedFactor = req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
				return "NoPower".Translate().CapitalizeFirst() + ": x" + unpoweredWorkTableWorkSpeedFactor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CE06 RID: 52742 RVA: 0x004BAA98 File Offset: 0x004B8C98
		public static bool Applies(Thing th)
		{
			if (th.def.building.unpoweredWorkTableWorkSpeedFactor == 0f)
			{
				return false;
			}
			CompPowerTrader compPowerTrader = th.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && !compPowerTrader.PowerOn;
		}
	}
}
