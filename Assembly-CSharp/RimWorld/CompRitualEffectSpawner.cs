using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A28 RID: 6696
	public abstract class CompRitualEffectSpawner : ThingComp
	{
		// Token: 0x0600A826 RID: 43046 RVA: 0x003BC834 File Offset: 0x003BAA34
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(30))
			{
				this.ritual = this.parent.TargetOfRitual();
			}
			if (this.ritual != null)
			{
				this.Tick_InRitualInterval(this.ritual);
				return;
			}
			this.Tick_OutOfRitualInterval();
		}

		// Token: 0x0600A827 RID: 43047
		protected abstract void Tick_InRitual(LordJob_Ritual ritual);

		// Token: 0x0600A828 RID: 43048
		protected abstract void Tick_InRitualInterval(LordJob_Ritual ritual);

		// Token: 0x0600A829 RID: 43049
		protected abstract void Tick_OutOfRitualInterval();

		// Token: 0x040062D3 RID: 25299
		protected LordJob_Ritual ritual;

		// Token: 0x040062D4 RID: 25300
		private const int RitualCheckInterval = 30;
	}
}
