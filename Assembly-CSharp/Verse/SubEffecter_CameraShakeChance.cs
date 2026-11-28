using System;

namespace Verse
{
	// Token: 0x020006CA RID: 1738
	public class SubEffecter_CameraShakeChance : SubEffecter_CameraShake
	{
		// Token: 0x06003708 RID: 14088 RVA: 0x00160B4A File Offset: 0x0015ED4A
		public SubEffecter_CameraShakeChance(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
			this.ticksUntilEffect = this.def.initialDelayTicks;
			this.lifespanMaxTicks = Find.TickManager.TicksGame + this.def.lifespanMaxTicks + this.ticksUntilEffect;
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x00160B88 File Offset: 0x0015ED88
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilEffect--;
			if (Find.TickManager.TicksGame >= this.lastEffectTicks + this.def.chancePeriodTicks && Find.TickManager.TicksGame < this.lifespanMaxTicks && this.ticksUntilEffect <= 0 && Rand.Chance(base.EffectiveChancePerTick))
			{
				this.lastEffectTicks = Find.TickManager.TicksGame;
				base.DoShake(A);
			}
		}

		// Token: 0x0400242E RID: 9262
		private int lastEffectTicks;

		// Token: 0x0400242F RID: 9263
		private int ticksUntilEffect;

		// Token: 0x04002430 RID: 9264
		private int lifespanMaxTicks;
	}
}
