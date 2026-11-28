using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200109D RID: 4253
	public class QuestPart_SagnuophageMeeting : QuestPart_MakeLord
	{
		// Token: 0x06006A78 RID: 27256 RVA: 0x0025703C File Offset: 0x0025523C
		protected override Lord MakeLord()
		{
			Lord lord = LordMaker.MakeNewLord(this.faction, new LordJob_SanguophageMeeting(this.targetCell, this.enemyTargets, this.meetingDurationTicks, this.outSignalMeetingStarted, this.outSignalMeetingCompleted, this.outSignalAllSanguophagesGone, this.inSignalDefend, this.inSignalQuestSuccess, this.inSignalQuestFail), base.Map, null);
			QuestUtility.AddQuestTag(ref lord.questTags, this.questTag);
			return lord;
		}

		// Token: 0x06006A79 RID: 27257 RVA: 0x002570A8 File Offset: 0x002552A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.targetCell, "targetCell", default(IntVec3), false);
			Scribe_Collections.Look<Thing>(ref this.enemyTargets, "enemyTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.meetingDurationTicks, "meetingDurationTicks", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalDefend, "inSignalDefend", null, false);
			Scribe_Values.Look<string>(ref this.outSignalMeetingStarted, "outSignalMeetingStarted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalMeetingCompleted, "outSignalMeetingCompleted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalAllSanguophagesGone, "outSignalAllSanguophagesGone", null, false);
			Scribe_Values.Look<string>(ref this.inSignalQuestSuccess, "inSignalQuestSuccess", null, false);
			Scribe_Values.Look<string>(ref this.inSignalQuestFail, "inSignalQuestFail", null, false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
		}

		// Token: 0x040043A5 RID: 17317
		public IntVec3 targetCell;

		// Token: 0x040043A6 RID: 17318
		public List<Thing> enemyTargets = new List<Thing>();

		// Token: 0x040043A7 RID: 17319
		public int meetingDurationTicks;

		// Token: 0x040043A8 RID: 17320
		public string outSignalMeetingStarted;

		// Token: 0x040043A9 RID: 17321
		public string outSignalMeetingCompleted;

		// Token: 0x040043AA RID: 17322
		public string outSignalAllSanguophagesGone;

		// Token: 0x040043AB RID: 17323
		public string inSignalDefend;

		// Token: 0x040043AC RID: 17324
		public string inSignalQuestSuccess;

		// Token: 0x040043AD RID: 17325
		public string inSignalQuestFail;

		// Token: 0x040043AE RID: 17326
		public string questTag;
	}
}
