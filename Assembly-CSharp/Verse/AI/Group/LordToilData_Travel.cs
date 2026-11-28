using System;

namespace Verse.AI.Group
{
	// Token: 0x0200094F RID: 2383
	public class LordToilData_Travel : LordToilData
	{
		// Token: 0x0600492A RID: 18730 RVA: 0x001C95C0 File Offset: 0x001C77C0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}

		// Token: 0x04002EF6 RID: 12022
		public IntVec3 dest;
	}
}
