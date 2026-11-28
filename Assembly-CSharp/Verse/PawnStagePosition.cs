using System;

namespace Verse
{
	// Token: 0x020000B3 RID: 179
	public class PawnStagePosition : IExposable
	{
		// Token: 0x06000752 RID: 1874 RVA: 0x00029F68 File Offset: 0x00028168
		public PawnStagePosition()
		{
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00029F7B File Offset: 0x0002817B
		public PawnStagePosition(IntVec3 cell, Thing thing, Rot4 orientation, bool highlight)
		{
			this.cell = cell;
			this.thing = thing;
			this.orientation = orientation;
			this.highlight = highlight;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00029FAC File Offset: 0x000281AC
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<Rot4>(ref this.orientation, "orientation", Rot4.Invalid, false);
			Scribe_Values.Look<bool>(ref this.highlight, "highlight", false, false);
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
		}

		// Token: 0x040002C0 RID: 704
		public IntVec3 cell;

		// Token: 0x040002C1 RID: 705
		public Thing thing;

		// Token: 0x040002C2 RID: 706
		public Rot4 orientation = Rot4.Invalid;

		// Token: 0x040002C3 RID: 707
		public bool highlight;
	}
}
