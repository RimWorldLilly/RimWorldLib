using System;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A47 RID: 6727
	public class CompUseEffect_PlayWarmupSound : CompUseEffect
	{
		// Token: 0x17001D90 RID: 7568
		// (get) Token: 0x0600A88E RID: 43150 RVA: 0x003BDECB File Offset: 0x003BC0CB
		private CompProperties_UseEffectPlayWarmupSound Props
		{
			get
			{
				return (CompProperties_UseEffectPlayWarmupSound)this.props;
			}
		}

		// Token: 0x0600A88F RID: 43151 RVA: 0x003BDED8 File Offset: 0x003BC0D8
		public override void PrepareTick()
		{
			if (this.Props.warmupSound != null && this.sustainer == null)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.PerTick);
				this.sustainer = this.Props.warmupSound.TrySpawnSustainer(info);
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
		}

		// Token: 0x04006301 RID: 25345
		private Sustainer sustainer;
	}
}
