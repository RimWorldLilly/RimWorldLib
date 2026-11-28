using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000915 RID: 2325
	public class JobSyncTracker : IExposable
	{
		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x060046D7 RID: 18135 RVA: 0x001BD5B7 File Offset: 0x001BB7B7
		public bool AllPawnsDone
		{
			get
			{
				return this.pawns.Count == 0;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x060046D8 RID: 18136 RVA: 0x001BD5C7 File Offset: 0x001BB7C7
		public bool AnyPawnsDone
		{
			get
			{
				return this.initialCount != this.pawns.Count;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x060046D9 RID: 18137 RVA: 0x001BD5DF File Offset: 0x001BB7DF
		public int PawnDoneCount
		{
			get
			{
				return this.initialCount - this.pawns.Count;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x060046DA RID: 18138 RVA: 0x001BD5F3 File Offset: 0x001BB7F3
		public IEnumerable<Pawn> BlockingPawns
		{
			get
			{
				return this.pawns;
			}
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x001BD5FB File Offset: 0x001BB7FB
		public bool WaitingOnPawn(Pawn pawn)
		{
			return this.pawns.Contains(pawn);
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x001BD609 File Offset: 0x001BB809
		protected JobSyncTracker()
		{
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x001BD614 File Offset: 0x001BB814
		public JobSyncTracker(IEnumerable<Pawn> pawns, ILoadReferenceable source, string dutyTag = null, string thinkNodeTag = null, JobCondition allowedTerminalStates = JobCondition.Succeeded)
		{
			this.pawns = new HashSet<Pawn>(pawns);
			this.allowedTerminalStates = allowedTerminalStates;
			this.dutyTag = dutyTag;
			this.initialCount = this.pawns.Count;
			this.source = source;
			this.thinkNodeTag = thinkNodeTag;
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x001BD662 File Offset: 0x001BB862
		public void ForcePawnFinished(Pawn pawn)
		{
			this.pawns.Remove(pawn);
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x001BD674 File Offset: 0x001BB874
		public void Notify_PawnJobDone(Pawn pawn, string dutyTag, string thinkNodeTag, ILoadReferenceable source, JobCondition condition)
		{
			if (!this.pawns.Contains(pawn))
			{
				return;
			}
			if ((this.allowedTerminalStates & condition) == JobCondition.None)
			{
				return;
			}
			if (this.dutyTag != null && this.dutyTag != dutyTag)
			{
				return;
			}
			if (this.source != null && this.source != source)
			{
				return;
			}
			if (this.thinkNodeTag != null && this.thinkNodeTag != thinkNodeTag)
			{
				return;
			}
			this.pawns.Remove(pawn);
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x001BD6EA File Offset: 0x001BB8EA
		public void Notify_PawnJobDone(Pawn pawn, Job job, JobCondition condition)
		{
			string text = job.dutyTag;
			ThinkNode jobGiver = job.jobGiver;
			this.Notify_PawnJobDone(pawn, text, (jobGiver != null) ? jobGiver.tag : null, job.source, condition);
		}

		// Token: 0x060046E1 RID: 18145 RVA: 0x001BD712 File Offset: 0x001BB912
		public void Notify_PawnLost(Pawn pawn)
		{
			if (this.pawns.Remove(pawn))
			{
				this.initialCount--;
			}
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x001BD730 File Offset: 0x001BB930
		public void ExposeData()
		{
			Scribe_Values.Look<JobCondition>(ref this.allowedTerminalStates, "allowedTerminalStates", JobCondition.None, false);
			Scribe_Values.Look<string>(ref this.dutyTag, "dutyTag", null, false);
			Scribe_Values.Look<int>(ref this.initialCount, "initialCount", 0, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference);
			Scribe_References.Look<ILoadReferenceable>(ref this.source, "source", false);
		}

		// Token: 0x04002E16 RID: 11798
		public JobCondition allowedTerminalStates;

		// Token: 0x04002E17 RID: 11799
		public string dutyTag;

		// Token: 0x04002E18 RID: 11800
		public string thinkNodeTag;

		// Token: 0x04002E19 RID: 11801
		private int initialCount;

		// Token: 0x04002E1A RID: 11802
		private HashSet<Pawn> pawns;

		// Token: 0x04002E1B RID: 11803
		private ILoadReferenceable source;
	}
}
