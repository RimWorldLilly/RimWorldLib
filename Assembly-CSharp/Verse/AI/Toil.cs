using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200083B RID: 2107
	public sealed class Toil : IJobEndable
	{
		// Token: 0x0600423A RID: 16954 RVA: 0x001A27AC File Offset: 0x001A09AC
		public void Clear()
		{
			this.initAction = null;
			this.tickAction = null;
			this.tickIntervalAction = null;
			this.endConditions.Clear();
			this.preInitActions = null;
			this.preTickActions = null;
			this.preTickIntervalActions = null;
			this.finishActions = null;
			this.atomicWithPrevious = false;
			this.socialMode = RandomSocialMode.Normal;
			this.activeSkill = null;
			this.defaultCompleteMode = ToilCompleteMode.Instant;
			this.defaultDuration = 0;
			this.handlingFacing = false;
			this.debugName = null;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x001A2828 File Offset: 0x001A0A28
		public void Cleanup(int myIndex, JobDriver jobDriver)
		{
			if (this.finishActions != null)
			{
				for (int i = 0; i < this.finishActions.Count; i++)
				{
					try
					{
						this.finishActions[i]();
					}
					catch (Exception ex)
					{
						string[] array = new string[9];
						int num = 0;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Pawn ");
						defaultInterpolatedStringHandler.AppendFormatted(this.actor.ToStringSafe<Pawn>());
						defaultInterpolatedStringHandler.AppendLiteral(" threw exception while executing toil ");
						defaultInterpolatedStringHandler.AppendFormatted<Toil>(this);
						defaultInterpolatedStringHandler.AppendLiteral("'s finish action (");
						defaultInterpolatedStringHandler.AppendFormatted<int>(i);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
						array[1] = ", jobDriver=";
						array[2] = jobDriver.ToStringSafe<JobDriver>();
						array[3] = ", job=";
						array[4] = jobDriver.job.ToStringSafe<Job>();
						array[5] = ", toilIndex=";
						array[6] = myIndex.ToString();
						array[7] = ": ";
						int num2 = 8;
						Exception ex2 = ex;
						array[num2] = ((ex2 != null) ? ex2.ToString() : null);
						Log.Error(string.Concat(array));
					}
				}
			}
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x001A2950 File Offset: 0x001A0B50
		public Pawn GetActor()
		{
			return this.actor;
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x001A2958 File Offset: 0x001A0B58
		public void AddFailCondition(Func<bool> newFailCondition)
		{
			this.endConditions.Add(delegate
			{
				if (newFailCondition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x001A2989 File Offset: 0x001A0B89
		public void AddEndCondition(Func<JobCondition> newEndCondition)
		{
			this.endConditions.Add(newEndCondition);
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x001A2997 File Offset: 0x001A0B97
		public void AddPreInitAction(Action newAct)
		{
			if (this.preInitActions == null)
			{
				this.preInitActions = new List<Action>();
			}
			this.preInitActions.Add(newAct);
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x001A29B8 File Offset: 0x001A0BB8
		public void AddPreTickAction(Action newAct)
		{
			if (this.preTickActions == null)
			{
				this.preTickActions = new List<Action>();
			}
			this.preTickActions.Add(newAct);
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x001A29D9 File Offset: 0x001A0BD9
		public void AddPreTickIntervalAction(Action<int> newAct)
		{
			if (this.preTickIntervalActions == null)
			{
				this.preTickIntervalActions = new List<Action<int>>();
			}
			this.preTickIntervalActions.Add(newAct);
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x001A29FA File Offset: 0x001A0BFA
		public void AddFinishAction(Action newAct)
		{
			if (this.finishActions == null)
			{
				this.finishActions = new List<Action>();
			}
			this.finishActions.Add(newAct);
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x001A2A1B File Offset: 0x001A0C1B
		public override string ToString()
		{
			return this.debugName ?? "unnamed";
		}

		// Token: 0x04002B58 RID: 11096
		public Pawn actor;

		// Token: 0x04002B59 RID: 11097
		public Action initAction;

		// Token: 0x04002B5A RID: 11098
		public Action tickAction;

		// Token: 0x04002B5B RID: 11099
		public Action<int> tickIntervalAction;

		// Token: 0x04002B5C RID: 11100
		public List<Func<JobCondition>> endConditions = new List<Func<JobCondition>>();

		// Token: 0x04002B5D RID: 11101
		public List<Action> preInitActions;

		// Token: 0x04002B5E RID: 11102
		public List<Action> preTickActions;

		// Token: 0x04002B5F RID: 11103
		public List<Action<int>> preTickIntervalActions;

		// Token: 0x04002B60 RID: 11104
		public List<Action> finishActions;

		// Token: 0x04002B61 RID: 11105
		public bool atomicWithPrevious;

		// Token: 0x04002B62 RID: 11106
		public RandomSocialMode socialMode = RandomSocialMode.Normal;

		// Token: 0x04002B63 RID: 11107
		public Func<SkillDef> activeSkill;

		// Token: 0x04002B64 RID: 11108
		public ToilCompleteMode defaultCompleteMode = ToilCompleteMode.Instant;

		// Token: 0x04002B65 RID: 11109
		public int defaultDuration;

		// Token: 0x04002B66 RID: 11110
		public bool handlingFacing;

		// Token: 0x04002B67 RID: 11111
		public bool inPool = true;

		// Token: 0x04002B68 RID: 11112
		public string debugName;
	}
}
