using System;

namespace Verse
{
	// Token: 0x020004B6 RID: 1206
	public class Hediff_LightExposure : Hediff
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x000C7022 File Offset: 0x000C5222
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000C7025 File Offset: 0x000C5225
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Light exposure"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000C704C File Offset: 0x000C524C
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.pawn.SpawnedOrAnyParentSpawned && this.pawn.IsHashIntervalTick(60, delta) && !this.pawn.Dead)
			{
				bool flag = this.pawn.MapHeld.glowGrid.PsychGlowAt(this.pawn.PositionHeld) > PsychGlow.Dark;
				this.Severity += (flag ? 0.4f : -0.25f);
			}
		}

		// Token: 0x0400196E RID: 6510
		private const float ExposurePerSecond_Lit = 0.4f;

		// Token: 0x0400196F RID: 6511
		private const float ExposurePerSecond_Unlit = -0.25f;
	}
}
