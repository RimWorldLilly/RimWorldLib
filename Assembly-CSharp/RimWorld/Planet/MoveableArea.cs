using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F4 RID: 8948
	public class MoveableArea : IExposable
	{
		// Token: 0x0600DD34 RID: 56628 RVA: 0x00525D92 File Offset: 0x00523F92
		public MoveableArea()
		{
		}

		// Token: 0x0600DD35 RID: 56629 RVA: 0x00525DA5 File Offset: 0x00523FA5
		public MoveableArea(Gravship gravship, string label, string renamableLabel, Color color, int id)
		{
			this.gravship = gravship;
			this.label = label;
			this.renamableLabel = renamableLabel;
			this.color = color;
			this.id = id;
		}

		// Token: 0x0600DD36 RID: 56630 RVA: 0x00525DE0 File Offset: 0x00523FE0
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<IntVec3>(ref this.relativeCells, "relativeCells", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.renamableLabel, "renamableLabel", null, false);
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_References.Look<Gravship>(ref this.gravship, "gravship", false);
		}

		// Token: 0x0600DD37 RID: 56631 RVA: 0x00525E64 File Offset: 0x00524064
		public void Add(IntVec3 relativeCell)
		{
			this.relativeCells.Add(relativeCell);
		}

		// Token: 0x170026A3 RID: 9891
		// (get) Token: 0x0600DD38 RID: 56632 RVA: 0x00525E72 File Offset: 0x00524072
		public IEnumerable<IntVec3> RelativeCells
		{
			get
			{
				MoveableArea.<get_RelativeCells>d__11 <get_RelativeCells>d__ = new MoveableArea.<get_RelativeCells>d__11(-2);
				<get_RelativeCells>d__.<>4__this = this;
				return <get_RelativeCells>d__;
			}
		}

		// Token: 0x04008DC7 RID: 36295
		protected List<IntVec3> relativeCells = new List<IntVec3>();

		// Token: 0x04008DC8 RID: 36296
		protected string label;

		// Token: 0x04008DC9 RID: 36297
		protected string renamableLabel;

		// Token: 0x04008DCA RID: 36298
		protected Color color;

		// Token: 0x04008DCB RID: 36299
		protected int id;

		// Token: 0x04008DCC RID: 36300
		private Gravship gravship;
	}
}
