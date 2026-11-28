using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002093 RID: 8339
	public class Hediff_HeartAttack : HediffWithComps
	{
		// Token: 0x0600D231 RID: 53809 RVA: 0x004D3535 File Offset: 0x004D1735
		public override void PostMake()
		{
			base.PostMake();
			this.intervalFactor = Rand.Range(0.1f, 2f);
		}

		// Token: 0x0600D232 RID: 53810 RVA: 0x004D3552 File Offset: 0x004D1752
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalFactor, "intervalFactor", 0f, false);
		}

		// Token: 0x0600D233 RID: 53811 RVA: 0x004D3570 File Offset: 0x004D1770
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.pawn.IsHashIntervalTick((int)(5000f * this.intervalFactor), delta))
			{
				this.Severity += Rand.Range(-0.4f, 0.6f);
			}
		}

		// Token: 0x0600D234 RID: 53812 RVA: 0x004D35B0 File Offset: 0x004D17B0
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.Tended(quality, maxQuality, 0);
			float num = 0.65f * quality;
			if (Rand.Value < num)
			{
				if (batchPosition == 0 && this.pawn.Spawned)
				{
					MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatSuccess".Translate(num.ToStringPercent()), 6.5f);
				}
				this.Severity -= 0.3f;
				return;
			}
			if (batchPosition == 0 && this.pawn.Spawned)
			{
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatFailed".Translate(num.ToStringPercent()), 6.5f);
			}
		}

		// Token: 0x04008527 RID: 34087
		private float intervalFactor;

		// Token: 0x04008528 RID: 34088
		private const int SeverityChangeInterval = 5000;

		// Token: 0x04008529 RID: 34089
		private const float TendSuccessChanceFactor = 0.65f;

		// Token: 0x0400852A RID: 34090
		private const float TendSeverityReduction = 0.3f;
	}
}
