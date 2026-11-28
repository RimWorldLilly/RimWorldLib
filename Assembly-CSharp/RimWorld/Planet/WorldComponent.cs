using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200233E RID: 9022
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x0600DF01 RID: 57089 RVA: 0x0052F44E File Offset: 0x0052D64E
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x0600DF02 RID: 57090 RVA: 0x0052F45D File Offset: 0x0052D65D
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x0600DF03 RID: 57091 RVA: 0x0052F45F File Offset: 0x0052D65F
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x0600DF04 RID: 57092 RVA: 0x0052F461 File Offset: 0x0052D661
		public virtual void WorldComponentOnGUI()
		{
		}

		// Token: 0x0600DF05 RID: 57093 RVA: 0x0052F463 File Offset: 0x0052D663
		public virtual void ExposeData()
		{
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600DF06 RID: 57094 RVA: 0x0052F46B File Offset: 0x0052D66B
		public virtual void FinalizeInit(bool fromLoad)
		{
		}

		// Token: 0x04008F41 RID: 36673
		public World world;
	}
}
