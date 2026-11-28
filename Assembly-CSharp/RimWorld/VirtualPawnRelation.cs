using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015C7 RID: 5575
	public class VirtualPawnRelation : IExposable
	{
		// Token: 0x06008A3C RID: 35388 RVA: 0x00320D97 File Offset: 0x0031EF97
		public VirtualPawnRelation()
		{
		}

		// Token: 0x06008A3D RID: 35389 RVA: 0x00320D9F File Offset: 0x0031EF9F
		public VirtualPawnRelation(PawnRelationDef def, RelationshipRecord record, int startTicks)
		{
			this.def = def;
			this.record = record;
			this.startTicks = startTicks;
		}

		// Token: 0x06008A3E RID: 35390 RVA: 0x00320DBC File Offset: 0x0031EFBC
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_References.Look<RelationshipRecord>(ref this.record, "record", false);
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
		}

		// Token: 0x0400553D RID: 21821
		public PawnRelationDef def;

		// Token: 0x0400553E RID: 21822
		public RelationshipRecord record;

		// Token: 0x0400553F RID: 21823
		public int startTicks;
	}
}
