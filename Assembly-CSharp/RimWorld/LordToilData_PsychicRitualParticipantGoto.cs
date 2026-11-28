using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CEF RID: 3311
	public class LordToilData_PsychicRitualParticipantGoto : LordToilData
	{
		// Token: 0x06005D7A RID: 23930 RVA: 0x00223E14 File Offset: 0x00222014
		public override void ExposeData()
		{
			Scribe_Deep.Look<JobSyncTracker>(ref this.finalGoto, "finalGoto", Array.Empty<object>());
			Scribe_Collections.Look<PsychicRitualParticipant>(ref this.participants, "participants", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksElapsed, "ticksElapsed", 0, false);
			Scribe_Values.Look<int>(ref this.timeoutTicks, "timeoutTicks", 0, false);
		}

		// Token: 0x04003511 RID: 13585
		public List<PsychicRitualParticipant> participants;

		// Token: 0x04003512 RID: 13586
		public JobSyncTracker finalGoto;

		// Token: 0x04003513 RID: 13587
		public int ticksElapsed;

		// Token: 0x04003514 RID: 13588
		public int timeoutTicks;
	}
}
