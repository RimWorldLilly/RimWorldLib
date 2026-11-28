using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF6 RID: 8182
	public class TreeSighting : IExposable
	{
		// Token: 0x17002556 RID: 9558
		// (get) Token: 0x0600CF4F RID: 53071 RVA: 0x004C59C8 File Offset: 0x004C3BC8
		public Thing Tree
		{
			get
			{
				return this.tree;
			}
		}

		// Token: 0x17002557 RID: 9559
		// (get) Token: 0x0600CF50 RID: 53072 RVA: 0x004C59D0 File Offset: 0x004C3BD0
		public int TicksSinceSighting
		{
			get
			{
				return Find.TickManager.TicksGame - this.tickSighted;
			}
		}

		// Token: 0x0600CF51 RID: 53073 RVA: 0x004C59E3 File Offset: 0x004C3BE3
		public TreeSighting()
		{
		}

		// Token: 0x0600CF52 RID: 53074 RVA: 0x004C59EB File Offset: 0x004C3BEB
		public TreeSighting(Thing tree, int tickSighted)
		{
			this.tree = tree;
			this.tickSighted = tickSighted;
		}

		// Token: 0x0600CF53 RID: 53075 RVA: 0x004C5A01 File Offset: 0x004C3C01
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.tickSighted, "tickSighted", 0, false);
			Scribe_References.Look<Thing>(ref this.tree, "tree", false);
		}

		// Token: 0x040083DA RID: 33754
		private int tickSighted;

		// Token: 0x040083DB RID: 33755
		public Thing tree;
	}
}
