using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015C8 RID: 5576
	public class DirectPawnRelation : IExposable
	{
		// Token: 0x06008A3F RID: 35391 RVA: 0x00320DF1 File Offset: 0x0031EFF1
		public DirectPawnRelation()
		{
		}

		// Token: 0x06008A40 RID: 35392 RVA: 0x00320DF9 File Offset: 0x0031EFF9
		public DirectPawnRelation(PawnRelationDef def, Pawn otherPawn, int startTicks)
		{
			this.def = def;
			this.otherPawn = otherPawn;
			this.startTicks = startTicks;
		}

		// Token: 0x06008A41 RID: 35393 RVA: 0x00320E16 File Offset: 0x0031F016
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", true);
		}

		// Token: 0x04005540 RID: 21824
		public PawnRelationDef def;

		// Token: 0x04005541 RID: 21825
		public Pawn otherPawn;

		// Token: 0x04005542 RID: 21826
		public int startTicks;
	}
}
