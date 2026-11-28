using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000EC RID: 236
	public class PriorityWork : IExposable
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x00030871 File Offset: 0x0002EA71
		public bool IsPrioritized
		{
			get
			{
				if (this.prioritizedCell.IsValid)
				{
					if (Find.TickManager.TicksGame < this.prioritizeTick + 30000)
					{
						return true;
					}
					this.Clear();
				}
				return false;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x000308A1 File Offset: 0x0002EAA1
		public IntVec3 Cell
		{
			get
			{
				return this.prioritizedCell;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x000308A9 File Offset: 0x0002EAA9
		public WorkGiverDef WorkGiver
		{
			get
			{
				return this.prioritizedWorkGiver;
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000308B1 File Offset: 0x0002EAB1
		public PriorityWork()
		{
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x000308D4 File Offset: 0x0002EAD4
		public PriorityWork(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00030900 File Offset: 0x0002EB00
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.prioritizedCell, "prioritizedCell", default(IntVec3), false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.prioritizedWorkGiver, "prioritizedWorkGiver");
			Scribe_Values.Look<int>(ref this.prioritizeTick, "prioritizeTick", 0, false);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00030949 File Offset: 0x0002EB49
		public void Set(IntVec3 prioritizedCell, WorkGiverDef prioritizedWorkGiver)
		{
			this.prioritizedCell = prioritizedCell;
			this.prioritizedWorkGiver = prioritizedWorkGiver;
			this.prioritizeTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00030969 File Offset: 0x0002EB69
		public void Clear()
		{
			this.prioritizedCell = IntVec3.Invalid;
			this.prioritizedWorkGiver = null;
			this.prioritizeTick = 0;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00030984 File Offset: 0x0002EB84
		public void ClearPrioritizedWorkAndJobQueue()
		{
			this.Clear();
			this.pawn.jobs.ClearQueuedJobs(true);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0003099D File Offset: 0x0002EB9D
		public IEnumerable<Gizmo> GetGizmos()
		{
			PriorityWork.<GetGizmos>d__17 <GetGizmos>d__ = new PriorityWork.<GetGizmos>d__17(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x040003CE RID: 974
		private Pawn pawn;

		// Token: 0x040003CF RID: 975
		private IntVec3 prioritizedCell = IntVec3.Invalid;

		// Token: 0x040003D0 RID: 976
		private WorkGiverDef prioritizedWorkGiver;

		// Token: 0x040003D1 RID: 977
		private int prioritizeTick = Find.TickManager.TicksGame;

		// Token: 0x040003D2 RID: 978
		private const int Timeout = 30000;
	}
}
