using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200047A RID: 1146
	public class HediffComp_SeverityFromGas : HediffComp
	{
		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x000C14F9 File Offset: 0x000BF6F9
		public HediffCompProperties_SeverityFromGas Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromGas)this.props;
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000C1508 File Offset: 0x000BF708
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Props.gasType == GasType.ToxGas && !ModsConfig.BiotechActive)
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			if (!pawn.Spawned || !pawn.IsHashIntervalTick(this.Props.intervalTicks))
			{
				return;
			}
			if (pawn.Position.AnyGas(pawn.Map, this.Props.gasType))
			{
				float num = (float)pawn.Position.GasDensity(pawn.Map, this.Props.gasType) / 255f * this.Props.severityGasDensityFactor;
				if (this.Props.exposureStatFactor != null)
				{
					num *= 1f - pawn.GetStatValue(this.Props.exposureStatFactor, true, -1);
				}
				severityAdjustment += num;
				return;
			}
			severityAdjustment += this.Props.severityNotExposed;
		}
	}
}
