using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200084C RID: 2124
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x060042BB RID: 17083 RVA: 0x001A4D0C File Offset: 0x001A2F0C
		public int Count
		{
			get
			{
				return this.jobs.Count;
			}
		}

		// Token: 0x17000CB5 RID: 3253
		public QueuedJob this[int index]
		{
			get
			{
				return this.jobs[index];
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x060042BD RID: 17085 RVA: 0x001A4D28 File Offset: 0x001A2F28
		public bool AnyPlayerForced
		{
			get
			{
				for (int i = 0; i < this.jobs.Count; i++)
				{
					if (this.jobs[i].job.playerForced)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x001A4D68 File Offset: 0x001A2F68
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.jobs.RemoveAll(delegate(QueuedJob j)
				{
					Job job = j.job;
					return ((job != null) ? job.def : null) == null;
				});
			}
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x001A4DBE File Offset: 0x001A2FBE
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x001A4DD3 File Offset: 0x001A2FD3
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x001A4DE8 File Offset: 0x001A2FE8
		public void Clear(Pawn pawn, bool canReturnToPool)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				this.jobs[i] = null;
				queuedJob.Cleanup(pawn, canReturnToPool);
			}
			this.jobs.Clear();
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x001A4E38 File Offset: 0x001A3038
		public bool Contains(Job j)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				if (((queuedJob != null) ? queuedJob.job : null) == j)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x001A4E7C File Offset: 0x001A307C
		public QueuedJob Extract(Job j)
		{
			int num = this.jobs.FindIndex((QueuedJob qj) => qj.job == j);
			if (num >= 0)
			{
				QueuedJob result = this.jobs[num];
				this.jobs.RemoveAt(num);
				return result;
			}
			return null;
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x001A4ECC File Offset: 0x001A30CC
		public QueuedJob Dequeue()
		{
			if (this.jobs.NullOrEmpty<QueuedJob>())
			{
				return null;
			}
			QueuedJob result = this.jobs[0];
			this.jobs.RemoveAt(0);
			return result;
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x001A4EF5 File Offset: 0x001A30F5
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x001A4F04 File Offset: 0x001A3104
		public bool AnyCanBeginNow(Pawn pawn, bool whileLyingDown)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				if (this.jobs[i].job.CanBeginNow(pawn, whileLyingDown))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x001A4F44 File Offset: 0x001A3144
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x001A4F56 File Offset: 0x001A3156
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x001A4F68 File Offset: 0x001A3168
		public JobQueue Capture()
		{
			JobQueue jobQueue = new JobQueue();
			foreach (QueuedJob other in this.jobs)
			{
				jobQueue.jobs.Add(new QueuedJob(other));
			}
			return jobQueue;
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x001A4FCC File Offset: 0x001A31CC
		public void RemoveAll(Pawn pawn, Predicate<Job> filter)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				if (filter(queuedJob.job))
				{
					this.jobs[i] = null;
					queuedJob.Cleanup(pawn, true);
				}
			}
			this.jobs.RemoveAll((QueuedJob job) => job == null);
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x001A504C File Offset: 0x001A324C
		public void RemoveAllWorkType(Pawn pawn, WorkTypeDef wType, bool hardDisable)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				if (queuedJob.job.workGiverDef != null && queuedJob.job.workGiverDef.workType == wType && (hardDisable || !queuedJob.job.playerForced))
				{
					this.jobs[i] = null;
					queuedJob.Cleanup(pawn, true);
				}
			}
			this.jobs.RemoveAll((QueuedJob job) => job == null);
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x001A50EC File Offset: 0x001A32EC
		public void RemoveAllJoyKind(Pawn pawn, JoyKindDef joyKind)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				QueuedJob queuedJob = this.jobs[i];
				if (queuedJob.job.def.joyKind == joyKind)
				{
					this.jobs[i] = null;
					queuedJob.Cleanup(pawn, true);
				}
			}
			this.jobs.RemoveAll((QueuedJob job) => job == null);
		}

		// Token: 0x04002B71 RID: 11121
		private List<QueuedJob> jobs = new List<QueuedJob>();
	}
}
