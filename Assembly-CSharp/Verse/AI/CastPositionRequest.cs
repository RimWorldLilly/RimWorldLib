using System;

namespace Verse.AI
{
	// Token: 0x0200090A RID: 2314
	public struct CastPositionRequest
	{
		// Token: 0x04002DBA RID: 11706
		public Pawn caster;

		// Token: 0x04002DBB RID: 11707
		public Thing target;

		// Token: 0x04002DBC RID: 11708
		public Verb verb;

		// Token: 0x04002DBD RID: 11709
		public float maxRangeFromCaster;

		// Token: 0x04002DBE RID: 11710
		public float maxRangeFromTarget;

		// Token: 0x04002DBF RID: 11711
		public IntVec3 locus;

		// Token: 0x04002DC0 RID: 11712
		public float maxRangeFromLocus;

		// Token: 0x04002DC1 RID: 11713
		public bool wantCoverFromTarget;

		// Token: 0x04002DC2 RID: 11714
		public IntVec3? preferredCastPosition;

		// Token: 0x04002DC3 RID: 11715
		public Func<IntVec3, bool> validator;

		// Token: 0x04002DC4 RID: 11716
		public int maxRegions;
	}
}
