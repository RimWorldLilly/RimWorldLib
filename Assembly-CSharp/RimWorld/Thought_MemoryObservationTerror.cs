using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015DD RID: 5597
	public class Thought_MemoryObservationTerror : Thought_MemoryObservation
	{
		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x06008B14 RID: 35604 RVA: 0x00326B00 File Offset: 0x00324D00
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.target.Named("THING"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x06008B15 RID: 35605 RVA: 0x00326B37 File Offset: 0x00324D37
		public override bool Save
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001813 RID: 6163
		// (set) Token: 0x06008B16 RID: 35606 RVA: 0x00326B3A File Offset: 0x00324D3A
		public override Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
				this.target = value;
			}
		}

		// Token: 0x0400558B RID: 21899
		public int intensity;

		// Token: 0x0400558C RID: 21900
		private Thing target;
	}
}
