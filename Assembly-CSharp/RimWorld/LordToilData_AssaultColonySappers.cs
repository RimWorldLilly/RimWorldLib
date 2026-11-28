using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC7 RID: 3271
	public class LordToilData_AssaultColonySappers : LordToilData
	{
		// Token: 0x06005CCB RID: 23755 RVA: 0x002210C0 File Offset: 0x0021F2C0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sapperDest, "sapperDest", default(IntVec3), false);
		}

		// Token: 0x040034E4 RID: 13540
		public IntVec3 sapperDest = IntVec3.Invalid;
	}
}
