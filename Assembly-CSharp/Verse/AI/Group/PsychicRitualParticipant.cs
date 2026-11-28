using System;

namespace Verse.AI.Group
{
	// Token: 0x0200091D RID: 2333
	public class PsychicRitualParticipant : IExposable
	{
		// Token: 0x06004736 RID: 18230 RVA: 0x001BEFE8 File Offset: 0x001BD1E8
		public PsychicRitualParticipant()
		{
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x001BEFF0 File Offset: 0x001BD1F0
		public PsychicRitualParticipant(ValueTuple<Pawn, IntVec3> pair)
		{
			this.pawn = pair.Item1;
			this.location = pair.Item2;
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x001BF01D File Offset: 0x001BD21D
		public void Deconstruct(out Pawn pawn, out IntVec3 location)
		{
			pawn = this.pawn;
			location = this.location;
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x001BF034 File Offset: 0x001BD234
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<IntVec3>(ref this.location, "location", default(IntVec3), false);
		}

		// Token: 0x04002E3A RID: 11834
		public Pawn pawn;

		// Token: 0x04002E3B RID: 11835
		public IntVec3 location;
	}
}
