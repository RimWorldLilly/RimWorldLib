using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AC RID: 6316
	internal class FleshTendril : IExposable
	{
		// Token: 0x06009E2D RID: 40493 RVA: 0x0038C6D8 File Offset: 0x0038A8D8
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sourceNode, "sourceNode", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.currentPos, "currentPos", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.length, "length", 0, false);
		}

		// Token: 0x04005D8F RID: 23951
		public IntVec3 sourceNode;

		// Token: 0x04005D90 RID: 23952
		public IntVec3 currentPos;

		// Token: 0x04005D91 RID: 23953
		public int length;
	}
}
