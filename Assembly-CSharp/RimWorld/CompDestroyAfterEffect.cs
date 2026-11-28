using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ECE RID: 3790
	public class CompDestroyAfterEffect : ThingComp
	{
		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06006383 RID: 25475 RVA: 0x0023BA76 File Offset: 0x00239C76
		public CompProperties_DestroyAfterEffect Props
		{
			get
			{
				return (CompProperties_DestroyAfterEffect)this.props;
			}
		}

		// Token: 0x06006384 RID: 25476 RVA: 0x0023BA84 File Offset: 0x00239C84
		public override void CompTick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.effecter == null)
			{
				this.effecter = this.Props.effecterDef.SpawnMaintained(this.parent.PositionHeld, this.parent.Map, 1f);
				if (this.destructionTick < 0)
				{
					this.destructionTick = ticksGame + this.Props.effecterDef.maintainTicks;
				}
			}
			if (ticksGame >= this.destructionTick)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006385 RID: 25477 RVA: 0x0023BB0B File Offset: 0x00239D0B
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.destructionTick, "destructionTick", 0, false);
		}

		// Token: 0x040037A8 RID: 14248
		public int destructionTick = int.MinValue;

		// Token: 0x040037A9 RID: 14249
		private Effecter effecter;
	}
}
