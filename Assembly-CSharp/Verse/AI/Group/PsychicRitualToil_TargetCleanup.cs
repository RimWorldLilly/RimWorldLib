using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000936 RID: 2358
	public class PsychicRitualToil_TargetCleanup : PsychicRitualToil
	{
		// Token: 0x06004831 RID: 18481 RVA: 0x001C561B File Offset: 0x001C381B
		protected PsychicRitualToil_TargetCleanup()
		{
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x001C5623 File Offset: 0x001C3823
		public PsychicRitualToil_TargetCleanup(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x001C563C File Offset: 0x001C383C
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			this.invoker = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			this.target = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
			Pawn pawn = this.invoker;
			if (pawn != null)
			{
				Pawn_JobTracker jobs = pawn.jobs;
				if (jobs != null)
				{
					jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			if (this.target != null)
			{
				if (!this.target.Dead && this.target.IsPrisonerOfColony)
				{
					WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(this.target, this.invoker);
					PawnUtility.ForceWait(this.target, 2500, null, true, false);
				}
			}
			else
			{
				Log.Warning("Tried to cleanup prisoner target for ritual " + psychicRitual.def.defName + ", but no target pawn was found.");
			}
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (this.invoker.IsPlayerControlled && this.target.Faction != null && this.target.HomeFaction != null && this.target.HomeFaction != Faction.OfPlayer && this.target.HomeFaction.def.humanlikeFaction && !this.target.Faction.def.PermanentlyHostileTo(FactionDefOf.PlayerColony))
			{
				this.target.HomeFaction.TryAffectGoodwillWith(Faction.OfPlayer, -25, true, true, HistoryEventDefOf.WasPsychicRitualTarget, null);
			}
			QuestUtility.SendQuestTargetSignals(this.target.questTags, "PsychicRitualTarget", this.target.Named("SUBJECT"));
		}

		// Token: 0x06004834 RID: 18484 RVA: 0x001C57C8 File Offset: 0x001C39C8
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			if (this.target == null || this.target.Dead || !this.target.IsPrisonerOfColony)
			{
				psychicRitual.ReleaseAllPawnsAndBuildings();
				return true;
			}
			if (this.invoker.IsCarryingPawn(this.target))
			{
				psychicRitual.ReleaseAllPawnsAndBuildings();
				return true;
			}
			return false;
		}

		// Token: 0x06004835 RID: 18485 RVA: 0x001C581B File Offset: 0x001C3A1B
		public override bool ClearJobOnStart(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return false;
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x001C5820 File Offset: 0x001C3A20
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_References.Look<Pawn>(ref this.invoker, "invoker", false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x04002EA4 RID: 11940
		private const int TargetFactionGoodwillChange = -25;

		// Token: 0x04002EA5 RID: 11941
		private PsychicRitualRoleDef targetRole;

		// Token: 0x04002EA6 RID: 11942
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x04002EA7 RID: 11943
		private Pawn invoker;

		// Token: 0x04002EA8 RID: 11944
		private Pawn target;
	}
}
