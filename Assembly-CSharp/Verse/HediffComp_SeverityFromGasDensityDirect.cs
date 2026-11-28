using System;

namespace Verse
{
	// Token: 0x0200047C RID: 1148
	public class HediffComp_SeverityFromGasDensityDirect : HediffComp
	{
		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600200F RID: 8207 RVA: 0x000C1634 File Offset: 0x000BF834
		public HediffCompProperties_SeverityFromGasDensityDirect Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromGasDensityDirect)this.props;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06002010 RID: 8208 RVA: 0x000C1644 File Offset: 0x000BF844
		public override bool CompShouldRemove
		{
			get
			{
				return base.Pawn.Dead || (!base.Pawn.Spawned && base.Pawn.CarriedBy == null) || !base.Pawn.PositionHeld.AnyGas(base.Pawn.MapHeld, this.Props.gasType);
			}
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x000C16A3 File Offset: 0x000BF8A3
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.UpdateSeverity();
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x000C16AB File Offset: 0x000BF8AB
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(this.Props.intervalTicks))
			{
				this.UpdateSeverity();
			}
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x000C16CC File Offset: 0x000BF8CC
		private void UpdateSeverity()
		{
			if (!base.Pawn.Spawned)
			{
				return;
			}
			if (this.Props.gasType == GasType.ToxGas && !ModsConfig.BiotechActive)
			{
				return;
			}
			float num = base.Pawn.MapHeld.gasGrid.DensityPercentAt(base.Pawn.PositionHeld, this.Props.gasType);
			for (int i = 0; i <= this.Props.densityStages.Count; i++)
			{
				if (num <= this.Props.densityStages[i])
				{
					this.parent.Severity = (float)(i + 1);
					return;
				}
			}
		}
	}
}
