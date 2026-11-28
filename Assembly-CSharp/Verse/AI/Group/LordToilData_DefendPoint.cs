using System;

namespace Verse.AI.Group
{
	// Token: 0x02000945 RID: 2373
	public class LordToilData_DefendPoint : LordToilData
	{
		// Token: 0x06004906 RID: 18694 RVA: 0x001C904C File Offset: 0x001C724C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendPoint, "defendPoint", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 28f, false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
		}

		// Token: 0x04002EEB RID: 12011
		public IntVec3 defendPoint = IntVec3.Invalid;

		// Token: 0x04002EEC RID: 12012
		public float defendRadius = 28f;

		// Token: 0x04002EED RID: 12013
		public float? wanderRadius;
	}
}
