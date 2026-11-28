using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CFF RID: 3327
	public class LordToilData_Venerate : LordToilData
	{
		// Token: 0x06005DC5 RID: 24005 RVA: 0x002258A1 File Offset: 0x00223AA1
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentNearVeneratorTicks, "currentNearVeneratorTicks", 0, false);
			Scribe_Values.Look<int>(ref this.lastNearVeneratorIndex, "lastNearVeneratorIndex", 0, false);
		}

		// Token: 0x04003536 RID: 13622
		public int currentNearVeneratorTicks = -1;

		// Token: 0x04003537 RID: 13623
		public int lastNearVeneratorIndex = -1;
	}
}
