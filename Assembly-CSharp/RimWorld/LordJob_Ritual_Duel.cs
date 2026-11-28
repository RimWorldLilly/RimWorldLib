using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CAE RID: 3246
	public class LordJob_Ritual_Duel : LordJob_Ritual
	{
		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06005C15 RID: 23573 RVA: 0x0021E0E5 File Offset: 0x0021C2E5
		public DuelBehaviorStage CurrentDuelStage
		{
			get
			{
				if (this.attacksThisStage <= 0)
				{
					return DuelBehaviorStage.Move;
				}
				return DuelBehaviorStage.Attack;
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06005C16 RID: 23574 RVA: 0x0021E0F3 File Offset: 0x0021C2F3
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005C17 RID: 23575 RVA: 0x0021E0F6 File Offset: 0x0021C2F6
		public LordJob_Ritual_Duel()
		{
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x0021E10C File Offset: 0x0021C30C
		public LordJob_Ritual_Duel(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null) : base(selectedTarget, ritual, obligation, allStages, assignments, organizer, null)
		{
			foreach (RitualRole ritualRole in assignments.AllRolesForReading)
			{
				if (ritualRole != null && ritualRole.id.Contains("duelist"))
				{
					Pawn item = assignments.FirstAssignedPawn(ritualRole);
					this.duelists.Add(item);
					this.pawnsDeathIgnored.Add(item);
				}
			}
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x0021E1B4 File Offset: 0x0021C3B4
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped && base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x0021E1C4 File Offset: 0x0021C3C4
		public void StartDuelIfNotStartedYet()
		{
			if (!this.duelStarted)
			{
				this.duelStarted = true;
				this.StartDuel();
			}
		}

		// Token: 0x06005C1B RID: 23579 RVA: 0x0021E1DB File Offset: 0x0021C3DB
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (this.duelists.Contains(p))
			{
				return 1f;
			}
			return base.VoluntaryJoinPriorityFor(p);
		}

		// Token: 0x06005C1C RID: 23580 RVA: 0x0021E1F8 File Offset: 0x0021C3F8
		private void InterruptDuelistJobs()
		{
			foreach (Pawn pawn in this.duelists)
			{
				Pawn_JobTracker jobs = pawn.jobs;
				if (jobs != null)
				{
					jobs.CheckForJobOverride(0f, true);
				}
			}
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x0021E25C File Offset: 0x0021C45C
		private void StartDuel()
		{
			this.StartMoving();
		}

		// Token: 0x06005C1E RID: 23582 RVA: 0x0021E264 File Offset: 0x0021C464
		private void StartMoving()
		{
			this.attacksThisStage = 0;
			this.movingTicks = LordJob_Ritual_Duel.MovingTicksPerStage.RandomInRange;
			this.InterruptDuelistJobs();
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x0021E294 File Offset: 0x0021C494
		private void StartAttacking()
		{
			this.movingTicks = 0;
			this.attacksThisStage = LordJob_Ritual_Duel.AttacksPerStage.RandomInRange;
			this.InterruptDuelistJobs();
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x0021E2C1 File Offset: 0x0021C4C1
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.movingTicks > 0)
			{
				this.movingTicks--;
				if (this.movingTicks <= 0)
				{
					this.StartAttacking();
				}
			}
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x0021E2F0 File Offset: 0x0021C4F0
		public override void ApplyOutcome(float progress, bool showFinishedMessage = true, bool showFailedMessage = true, bool cancelled = false)
		{
			foreach (Pawn pawn in this.duelists)
			{
				if (pawn.equipment.Primary != null)
				{
					ThingWithComps thingWithComps;
					pawn.equipment.TryDropEquipment(pawn.equipment.Primary, out thingWithComps, pawn.PositionHeld, true);
				}
			}
			base.ApplyOutcome(progress, showFinishedMessage, showFailedMessage, cancelled);
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x0021E374 File Offset: 0x0021C574
		public void Notify_MeleeAttack(Pawn duelist, Thing victim)
		{
			this.attacksThisStage--;
			if (this.attacksThisStage <= 0)
			{
				this.StartMoving();
			}
			if (this.usedWeapon)
			{
				return;
			}
			Verb verb = duelist.meleeVerbs.TryGetMeleeVerb(victim);
			if (verb != null && verb.EquipmentSource != null && verb.EquipmentSource.def.IsMeleeWeapon)
			{
				this.usedWeapon = true;
			}
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x0021E3D8 File Offset: 0x0021C5D8
		protected override bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
		{
			int num = 0;
			using (List<Pawn>.Enumerator enumerator = this.duelists.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.DeadOrDowned)
					{
						num++;
					}
				}
			}
			return num <= 1;
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x0021E438 File Offset: 0x0021C638
		public Pawn Opponent(Pawn duelist)
		{
			return this.duelists[(this.duelists.IndexOf(duelist) == 0) ? 1 : 0];
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x0021E454 File Offset: 0x0021C654
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (!p.DeadOrDowned)
			{
				base.Cancel();
			}
			if (p.Dead && this.duelists.Contains(p))
			{
				p.health.killedByRitual = true;
			}
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x0021E486 File Offset: 0x0021C686
		public override bool BlocksSocialInteraction(Pawn pawn)
		{
			return this.duelists.Contains(pawn);
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x0021E494 File Offset: 0x0021C694
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.duelists, "duelists", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.usedWeapon, "usedWeapon", false, false);
			Scribe_Values.Look<int>(ref this.movingTicks, "movingTicks", 0, false);
			Scribe_Values.Look<int>(ref this.attacksThisStage, "attacksThisStage", 0, false);
			Scribe_Values.Look<bool>(ref this.duelStarted, "duelStarted", false, false);
		}

		// Token: 0x040034B1 RID: 13489
		public List<Pawn> duelists = new List<Pawn>();

		// Token: 0x040034B2 RID: 13490
		public bool usedWeapon;

		// Token: 0x040034B3 RID: 13491
		private bool duelStarted;

		// Token: 0x040034B4 RID: 13492
		private int attacksThisStage;

		// Token: 0x040034B5 RID: 13493
		private int movingTicks;

		// Token: 0x040034B6 RID: 13494
		private static readonly IntRange AttacksPerStage = new IntRange(4, 8);

		// Token: 0x040034B7 RID: 13495
		private static readonly IntRange MovingTicksPerStage = new IntRange(360, 600);
	}
}
