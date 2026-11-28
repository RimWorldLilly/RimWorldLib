using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000F60 RID: 3936
	public class GatheringDef : Def
	{
		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x06006536 RID: 25910 RVA: 0x002415A7 File Offset: 0x0023F7A7
		public bool IsRandomSelectable
		{
			get
			{
				return this.randomSelectionWeight > 0f;
			}
		}

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x06006537 RID: 25911 RVA: 0x002415B6 File Offset: 0x0023F7B6
		public GatheringWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (GatheringWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x002415E8 File Offset: 0x0023F7E8
		public bool CanExecute(Map map, Pawn organizer = null, bool ignoreGameConditions = false)
		{
			return (ignoreGameConditions || GatheringsUtility.AcceptableGameConditionsToStartGathering(map, this)) && this.Worker.CanExecute(map, organizer);
		}

		// Token: 0x04003B90 RID: 15248
		public Type workerClass = typeof(GatheringWorker);

		// Token: 0x04003B91 RID: 15249
		public DutyDef duty;

		// Token: 0x04003B92 RID: 15250
		public float randomSelectionWeight;

		// Token: 0x04003B93 RID: 15251
		public bool respectTimetable = true;

		// Token: 0x04003B94 RID: 15252
		public List<ThingDef> gatherSpotDefs;

		// Token: 0x04003B95 RID: 15253
		[MustTranslate]
		public string letterTitle;

		// Token: 0x04003B96 RID: 15254
		[MustTranslate]
		public string letterText;

		// Token: 0x04003B97 RID: 15255
		[MustTranslate]
		public string calledOffMessage;

		// Token: 0x04003B98 RID: 15256
		[MustTranslate]
		public string finishedMessage;

		// Token: 0x04003B99 RID: 15257
		public List<RoyalTitleDef> requiredTitleAny = new List<RoyalTitleDef>();

		// Token: 0x04003B9A RID: 15258
		private GatheringWorker worker;
	}
}
