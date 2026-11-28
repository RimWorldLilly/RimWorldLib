using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A2C RID: 6700
	public class CompRitualTargetEffecterSpawner : CompRitualEffectSpawner
	{
		// Token: 0x17001D81 RID: 7553
		// (get) Token: 0x0600A832 RID: 43058 RVA: 0x003BC961 File Offset: 0x003BAB61
		private CompProperties_RitualTargetEffecterSpawner Props
		{
			get
			{
				return (CompProperties_RitualTargetEffecterSpawner)this.props;
			}
		}

		// Token: 0x0600A833 RID: 43059 RVA: 0x003BC96E File Offset: 0x003BAB6E
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x0600A834 RID: 43060 RVA: 0x003BC990 File Offset: 0x003BAB90
		protected override void Tick_InRitual(LordJob_Ritual ritual)
		{
		}

		// Token: 0x0600A835 RID: 43061 RVA: 0x003BC994 File Offset: 0x003BAB94
		protected override void Tick_InRitualInterval(LordJob_Ritual ritual)
		{
			if (this.Props.minRitualProgress > 0f && ritual.Progress < this.Props.minRitualProgress)
			{
				return;
			}
			if (this.effecter == null)
			{
				this.effecter = this.Props.effecter.Spawn();
				this.effecter.Trigger(this.parent, this.parent, -1);
			}
			this.effecter.EffectTick(this.parent, this.parent);
		}

		// Token: 0x0600A836 RID: 43062 RVA: 0x003BCA29 File Offset: 0x003BAC29
		protected override void Tick_OutOfRitualInterval()
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x040062D9 RID: 25305
		private Effecter effecter;
	}
}
