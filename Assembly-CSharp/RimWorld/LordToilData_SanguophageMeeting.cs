using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C7D RID: 3197
	public class LordToilData_SanguophageMeeting : LordToilData
	{
		// Token: 0x06005AAA RID: 23210 RVA: 0x00215BB0 File Offset: 0x00213DB0
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksInMeeting, "ticksInMeeting", 0, false);
		}

		// Token: 0x04003402 RID: 13314
		public int ticksInMeeting;
	}
}
