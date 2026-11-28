using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200091C RID: 2332
	public class PsychicRitualGraph : PsychicRitualToil
	{
		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06004713 RID: 18195 RVA: 0x001BE7E4 File Offset: 0x001BC9E4
		public PsychicRitualGraph Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06004714 RID: 18196 RVA: 0x001BE7EC File Offset: 0x001BC9EC
		public PsychicRitualGraph RootGraph
		{
			get
			{
				PsychicRitualGraph psychicRitualGraph = this;
				for (int i = 0; i < 1000; i++)
				{
					if (psychicRitualGraph.parent == null)
					{
						return psychicRitualGraph;
					}
					psychicRitualGraph = psychicRitualGraph.parent;
				}
				throw new InvalidOperationException("Max depth exceeded while trying to find root for PsychicRitualGraph " + this.ToString() + ". Is there a cycle?");
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06004715 RID: 18197 RVA: 0x001BE838 File Offset: 0x001BCA38
		public int ToilCount
		{
			get
			{
				return this.toils.Count;
			}
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06004716 RID: 18198 RVA: 0x001BE845 File Offset: 0x001BCA45
		public PsychicRitualToil CurrentToil
		{
			get
			{
				return this.GetToil(this.currentToilIndex);
			}
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x001BE853 File Offset: 0x001BCA53
		public PsychicRitualGraph()
		{
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x001BE862 File Offset: 0x001BCA62
		public PsychicRitualGraph(IEnumerable<PsychicRitualToil> toils)
		{
			this.toils = new List<PsychicRitualToil>(toils);
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x001BE87D File Offset: 0x001BCA7D
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			this.parent = parent;
			this.toils = (this.toils ?? psychicRitual.def.CreateToils(psychicRitual, this));
			PsychicRitualToilRunner.Start(this.GetToil(this.currentToilIndex), psychicRitual, this);
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x001BE8B6 File Offset: 0x001BCAB6
		public override void End(PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
			PsychicRitualToilRunner.End(this.GetToil(this.currentToilIndex), psychicRitual, this, success);
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x001BE8CC File Offset: 0x001BCACC
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			int num = this.currentToilIndex;
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			for (int i = 0; i < 1000; i++)
			{
				if (toil == null)
				{
					return true;
				}
				bool flag = PsychicRitualToilRunner.Tick(toil, psychicRitual, this);
				if (!flag)
				{
					return false;
				}
				if (flag && !this.willAdvancePastLastToil && this.currentToilIndex + 1 == this.toils.Count)
				{
					return true;
				}
				if (DebugSettings.logPsychicRitualTransitions)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Toil ");
					defaultInterpolatedStringHandler.AppendFormatted<PsychicRitualToil>(toil);
					defaultInterpolatedStringHandler.AppendLiteral(" has finished during tick ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(Find.TickManager.TicksGame);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				PsychicRitualToilRunner.End(toil, psychicRitual, this, true);
				this.currentToilIndex = (this.nextToilIndexOverride ?? (this.currentToilIndex + 1));
				this.nextToilIndexOverride = null;
				toil = this.GetToil(this.currentToilIndex);
				PsychicRitualToilRunner.Start(toil, psychicRitual, this);
				PsychicRitualToilRunner.UpdateAllDuties(toil, psychicRitual, this);
				if (toil != null)
				{
					foreach (Pawn pawn in psychicRitual.assignments.AllAssignedPawns)
					{
						if (toil.ClearJobOnStart(psychicRitual, parent, pawn) && pawn != null)
						{
							Pawn_JobTracker jobs = pawn.jobs;
							if (jobs != null)
							{
								jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							}
						}
					}
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(68, 4);
			defaultInterpolatedStringHandler2.AppendLiteral("Max iterations exceeded, starting from toil ");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler2.AppendLiteral(": ");
			defaultInterpolatedStringHandler2.AppendFormatted(this.GetToil(num).ToStringSafe<PsychicRitualToil>());
			defaultInterpolatedStringHandler2.AppendLiteral(" ");
			defaultInterpolatedStringHandler2.AppendLiteral("and ending on toil ");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(this.currentToilIndex);
			defaultInterpolatedStringHandler2.AppendLiteral(": ");
			defaultInterpolatedStringHandler2.AppendFormatted(this.GetToil(num).ToStringSafe<PsychicRitualToil>());
			throw new InvalidOperationException(defaultInterpolatedStringHandler2.ToStringAndClear());
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x001BEAE8 File Offset: 0x001BCCE8
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			PsychicRitualToilRunner.UpdateAllDuties(this.GetToil(this.currentToilIndex), psychicRitual, this);
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x001BEAFD File Offset: 0x001BCCFD
		public override void Notify_PawnLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn victim, PawnLostCondition condition)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_PawnLost(psychicRitual, this, victim, condition);
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x001BEB1A File Offset: 0x001BCD1A
		public override void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_PawnJobDone(psychicRitual, this, pawn, job, condition);
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x001BEB39 File Offset: 0x001BCD39
		public override void Notify_BuildingLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_BuildingLost(psychicRitual, this, building);
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x001BEB54 File Offset: 0x001BCD54
		public override void Notify_CorpseLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Corpse corpse)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_CorpseLost(psychicRitual, this, corpse);
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x001BEB6F File Offset: 0x001BCD6F
		public override void Notify_ReachedDutyLocation(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_ReachedDutyLocation(psychicRitual, this, pawn);
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x001BEB8A File Offset: 0x001BCD8A
		public override void Notify_ConstructionFailed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_ConstructionFailed(psychicRitual, this, pawn, frame, newBlueprint);
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x001BEBA9 File Offset: 0x001BCDA9
		public override void Notify_BuildingSpawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_BuildingSpawnedOnMap(psychicRitual, this, building);
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x001BEBC4 File Offset: 0x001BCDC4
		public override void Notify_BuildingDespawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return;
			}
			toil.Notify_BuildingDespawnedOnMap(psychicRitual, this, building);
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x001BEBDF File Offset: 0x001BCDDF
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return result;
			}
			return toil.Notify_DutyResult(psychicRitual, this, result, pawn, issueParams);
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x001BEBFF File Offset: 0x001BCDFF
		public override ThinkResult Notify_DutyConstantResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return result;
			}
			return toil.Notify_DutyConstantResult(psychicRitual, this, result, pawn, issueParams);
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x001BEC1F File Offset: 0x001BCE1F
		public override string GetPawnReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return null;
			}
			return toil.GetPawnReport(psychicRitual, this, pawn);
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x001BEC3B File Offset: 0x001BCE3B
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return null;
			}
			return toil.GetJobReport(psychicRitual, this, pawn);
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x001BEC57 File Offset: 0x001BCE57
		public override string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return null;
			}
			return toil.GetReport(psychicRitual, this);
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x001BEC74 File Offset: 0x001BCE74
		public override AcceptanceReport AbilityAllowed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Ability ability)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.AbilityAllowed(psychicRitual, parent, ability);
			}
			return toil.AbilityAllowed(psychicRitual, this, ability);
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x001BECA4 File Offset: 0x001BCEA4
		public override AcceptanceReport AllowsFloatMenu(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.AllowsFloatMenu(psychicRitual, parent, pawn);
			}
			return toil.AllowsFloatMenu(psychicRitual, this, pawn);
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x001BECD4 File Offset: 0x001BCED4
		public override bool BlocksSocialInteraction(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.BlocksSocialInteraction(psychicRitual, parent, pawn);
			}
			return toil.BlocksSocialInteraction(psychicRitual, this, pawn);
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x001BED04 File Offset: 0x001BCF04
		public override bool DutyActiveWhenDown(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.DutyActiveWhenDown(psychicRitual, parent, pawn);
			}
			return toil.DutyActiveWhenDown(psychicRitual, this, pawn);
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x001BED34 File Offset: 0x001BCF34
		public override IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.GetPawnGizmos(psychicRitual, parent, pawn);
			}
			return toil.GetPawnGizmos(psychicRitual, this, pawn);
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x001BED64 File Offset: 0x001BCF64
		public override IEnumerable<Gizmo> GetBuildingGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.GetBuildingGizmos(psychicRitual, parent, building);
			}
			return toil.GetBuildingGizmos(psychicRitual, this, building);
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x001BED94 File Offset: 0x001BCF94
		public override void SetPawnDuty(Pawn pawn, PsychicRitual psychicRitual, PsychicRitualGraph parent, DutyDef def, LocalTargetInfo? focus = null, LocalTargetInfo? focusSecond = null, LocalTargetInfo? focusThird = null, string tag = null, float wanderRadius = 0f, Rot4? overrideFacing = null)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				base.SetPawnDuty(pawn, psychicRitual, parent, def, focus, focusSecond, focusThird, tag, wanderRadius, null);
				return;
			}
			toil.SetPawnDuty(pawn, psychicRitual, this, def, focus, focusSecond, focusThird, tag, wanderRadius, null);
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x001BEDF0 File Offset: 0x001BCFF0
		public override bool ShouldRemovePawn(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition condition)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.ShouldRemovePawn(psychicRitual, parent, pawn, condition);
			}
			return toil.ShouldRemovePawn(psychicRitual, this, pawn, condition);
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x001BEE24 File Offset: 0x001BD024
		public override bool ClearJobOnStart(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil toil = this.GetToil(this.currentToilIndex);
			if (toil == null)
			{
				return base.ClearJobOnStart(psychicRitual, parent, pawn);
			}
			return toil.ClearJobOnStart(psychicRitual, this, pawn);
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x001BEE54 File Offset: 0x001BD054
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int?>(ref this.nextToilIndexOverride, "nextToilIndexOverride", null, false);
			Scribe_Values.Look<int>(ref this.currentToilIndex, "currentToilIndex", 0, false);
			Scribe_Collections.Look<PsychicRitualToil>(ref this.toils, "toils", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<PsychicRitualGraph>(ref this.parent, "parent", false);
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x001BEEBC File Offset: 0x001BD0BC
		public PsychicRitualToil GetToil(int toilIndex)
		{
			if (this.toils == null)
			{
				return null;
			}
			if (toilIndex == -1)
			{
				return null;
			}
			if (toilIndex == this.toils.Count)
			{
				return null;
			}
			if (this.currentToilIndex < 0 || this.currentToilIndex > this.toils.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(74, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot start toil at index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.currentToilIndex);
				defaultInterpolatedStringHandler.AppendLiteral("; ");
				defaultInterpolatedStringHandler.AppendLiteral("it is outside the range of valid toils [0, $");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.toils.Count);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this.toils[toilIndex];
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x001BEF78 File Offset: 0x001BD178
		public bool SetNextToil(PsychicRitualToil toil)
		{
			if (this.toils == null)
			{
				throw new InvalidOperationException("Cannot set next toil to " + toil.ToStringSafe<PsychicRitualToil>() + "; there are no toils set.");
			}
			int num;
			if (toil == null)
			{
				num = -1;
			}
			else
			{
				num = this.toils.IndexOf(toil);
				if (num < 0)
				{
					throw new ArgumentException("Cannot set next toil to " + toil.ToStringSafe<PsychicRitualToil>() + "; it is not in the list of toils.");
				}
			}
			this.nextToilIndexOverride = new int?(num);
			return true;
		}

		// Token: 0x04002E32 RID: 11826
		public const int EndPsychicRitualIndex = -1;

		// Token: 0x04002E33 RID: 11827
		protected const int MaxJumpsPerTick = 1000;

		// Token: 0x04002E34 RID: 11828
		protected const int MaxGraphDepth = 1000;

		// Token: 0x04002E35 RID: 11829
		protected int? nextToilIndexOverride;

		// Token: 0x04002E36 RID: 11830
		protected int currentToilIndex;

		// Token: 0x04002E37 RID: 11831
		protected List<PsychicRitualToil> toils;

		// Token: 0x04002E38 RID: 11832
		protected PsychicRitualGraph parent;

		// Token: 0x04002E39 RID: 11833
		public bool willAdvancePastLastToil = true;
	}
}
