using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CFB RID: 3323
	public class LordToilData_Stage : LordToilData
	{
		// Token: 0x06005DB4 RID: 23988 RVA: 0x00225470 File Offset: 0x00223670
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.stagingPoint, "stagingPoint", default(IntVec3), false);
		}

		// Token: 0x04003531 RID: 13617
		public IntVec3 stagingPoint;
	}
}
