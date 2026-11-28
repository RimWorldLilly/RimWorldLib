using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000815 RID: 2069
	public class ThingCountTracker : IExposable
	{
		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06004119 RID: 16665 RVA: 0x0019DA30 File Offset: 0x0019BC30
		public IReadOnlyDictionary<ThingDef, List<ThingCountTracker.PawnCount>> ReadOnlyPairs
		{
			get
			{
				return this.stuffCount;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x0600411A RID: 16666 RVA: 0x0019DA38 File Offset: 0x0019BC38
		public Thing ParentThing
		{
			get
			{
				return (Thing)this.parent;
			}
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x0019DA45 File Offset: 0x0019BC45
		public ThingCountTracker()
		{
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x0019DA58 File Offset: 0x0019BC58
		public ThingCountTracker(IHaulEnroute parent)
		{
			this.parent = parent;
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x0019DA74 File Offset: 0x0019BC74
		public void Add(Pawn pawn, ThingDef stuff, int amount)
		{
			if (amount <= 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 4);
				defaultInterpolatedStringHandler.AppendFormatted(pawn.LabelShort);
				defaultInterpolatedStringHandler.AppendLiteral(" tried to add a negative value to the tracker on ");
				defaultInterpolatedStringHandler.AppendFormatted(((Thing)this.parent).Label);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted(stuff.label);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(amount);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			List<ThingCountTracker.PawnCount> list;
			if (this.stuffCount.TryGetValue(stuff, out list))
			{
				foreach (ThingCountTracker.PawnCount pawnCount in list)
				{
					if (pawnCount.pawn == pawn)
					{
						pawnCount.count += amount;
						return;
					}
				}
				this.stuffCount[stuff].Add(new ThingCountTracker.PawnCount
				{
					pawn = pawn,
					count = amount
				});
				return;
			}
			this.stuffCount[stuff] = new List<ThingCountTracker.PawnCount>
			{
				new ThingCountTracker.PawnCount
				{
					pawn = pawn,
					count = amount
				}
			};
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x0019DBBC File Offset: 0x0019BDBC
		public void CopyReservations(ThingCountTracker other)
		{
			foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in other.stuffCount)
			{
				ThingDef thingDef;
				List<ThingCountTracker.PawnCount> list;
				keyValuePair.Deconstruct(out thingDef, out list);
				ThingDef stuff = thingDef;
				foreach (ThingCountTracker.PawnCount pawnCount in list)
				{
					this.Add(pawnCount.pawn, stuff, pawnCount.count);
				}
			}
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x0019DC64 File Offset: 0x0019BE64
		public int Get(ThingDef stuff, Pawn excludePawn = null)
		{
			List<ThingCountTracker.PawnCount> list;
			if (this.stuffCount.TryGetValue(stuff, out list))
			{
				int num = 0;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					ThingCountTracker.PawnCount pawnCount = list[i];
					if (pawnCount.pawn != excludePawn)
					{
						num += pawnCount.count;
					}
				}
				return num;
			}
			return 0;
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x0019DCB4 File Offset: 0x0019BEB4
		public void ReleaseFor(Pawn pawn)
		{
			foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in this.stuffCount)
			{
				ThingDef thingDef;
				List<ThingCountTracker.PawnCount> list;
				keyValuePair.Deconstruct(out thingDef, out list);
				List<ThingCountTracker.PawnCount> list2 = list;
				for (int i = list2.Count - 1; i >= 0; i--)
				{
					if (list2[i].pawn == pawn)
					{
						list2.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x0019DD40 File Offset: 0x0019BF40
		public bool CanCleanup()
		{
			foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in this.stuffCount)
			{
				ThingDef thingDef;
				List<ThingCountTracker.PawnCount> list;
				keyValuePair.Deconstruct(out thingDef, out list);
				using (List<ThingCountTracker.PawnCount>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.count > 0)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x0019DDE0 File Offset: 0x0019BFE0
		public void InterruptEnroutePawns(Pawn exclude)
		{
			foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in this.stuffCount)
			{
				ThingDef thingDef;
				List<ThingCountTracker.PawnCount> list;
				keyValuePair.Deconstruct(out thingDef, out list);
				List<ThingCountTracker.PawnCount> list2 = list;
				for (int i = 0; i < list2.Count; i++)
				{
					Pawn pawn = list2[i].pawn;
					if (exclude != pawn)
					{
						for (int j = pawn.jobs.jobQueue.Count - 1; j >= 0; j--)
						{
							Job job = pawn.jobs.jobQueue[j].job;
							if (job.targetB == this.ParentThing && !job.targetQueueB.NullOrEmpty<LocalTargetInfo>())
							{
								if (pawn.mindState.priorityWork.WorkGiver != null && this.ParentThing.OccupiedRect().Contains(pawn.mindState.priorityWork.Cell))
								{
									pawn.mindState.priorityWork.Clear();
								}
								pawn.jobs.jobQueue.Extract(job);
							}
							else if (job.targetB == this.ParentThing)
							{
								job.SetTarget(TargetIndex.B, null);
							}
							else if (job.targetQueueB != null && job.targetQueueB.Contains(this.ParentThing))
							{
								job.targetQueueB.Remove(this.ParentThing);
							}
						}
						if (pawn.CurJob.targetB == this.ParentThing && !pawn.CurJob.targetQueueB.NullOrEmpty<LocalTargetInfo>())
						{
							if (pawn.mindState.priorityWork.WorkGiver != null && this.ParentThing.OccupiedRect().Contains(pawn.mindState.priorityWork.Cell))
							{
								pawn.mindState.priorityWork.Clear();
							}
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
						}
						else if (pawn.CurJob.targetB == this.ParentThing)
						{
							pawn.CurJob.SetTarget(TargetIndex.B, null);
						}
						else if (pawn.CurJob.targetQueueB != null && pawn.CurJob.targetQueueB.Contains(this.ParentThing))
						{
							pawn.CurJob.targetQueueB.Remove(this.ParentThing);
						}
					}
				}
				list2.Clear();
			}
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x0019E0BC File Offset: 0x0019C2BC
		public void ExposeData()
		{
			Scribe_References.Look<IHaulEnroute>(ref this.parent, "parent", false);
			Scribe_Collections.Look<ThingDef, List<ThingCountTracker.PawnCount>>(ref this.stuffCount, "stuffCount", LookMode.Def, LookMode.Deep, ref this.tmpThings, ref this.tmpCount, true, false, false);
		}

		// Token: 0x04002B11 RID: 11025
		private Dictionary<ThingDef, List<ThingCountTracker.PawnCount>> stuffCount = new Dictionary<ThingDef, List<ThingCountTracker.PawnCount>>();

		// Token: 0x04002B12 RID: 11026
		public IHaulEnroute parent;

		// Token: 0x04002B13 RID: 11027
		private List<ThingDef> tmpThings;

		// Token: 0x04002B14 RID: 11028
		private List<List<ThingCountTracker.PawnCount>> tmpCount;

		// Token: 0x02002AE3 RID: 10979
		public class PawnCount : IExposable
		{
			// Token: 0x06010864 RID: 67684 RVA: 0x005CCEFD File Offset: 0x005CB0FD
			public void ExposeData()
			{
				Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
				Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			}

			// Token: 0x0400AFF8 RID: 45048
			public Pawn pawn;

			// Token: 0x0400AFF9 RID: 45049
			public int count;
		}
	}
}
