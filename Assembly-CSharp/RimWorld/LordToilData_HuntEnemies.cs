using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CE4 RID: 3300
	public class LordToilData_HuntEnemies : LordToilData
	{
		// Token: 0x06005D41 RID: 23873 RVA: 0x00223430 File Offset: 0x00221630
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", IntVec3.Invalid, false);
		}

		// Token: 0x04003507 RID: 13575
		public IntVec3 fallbackLocation = IntVec3.Invalid;
	}
}
