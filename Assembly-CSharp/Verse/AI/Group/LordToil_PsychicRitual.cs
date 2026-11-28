using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000916 RID: 2326
	public class LordToil_PsychicRitual : LordToil
	{
		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x060046E3 RID: 18147 RVA: 0x001BD795 File Offset: 0x001BB995
		public PsychicRitualLordToilData RitualData
		{
			get
			{
				return (PsychicRitualLordToilData)this.data;
			}
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x001BD7A2 File Offset: 0x001BB9A2
		public LordToil_PsychicRitual(PsychicRitualDef def, PsychicRitualRoleAssignments assignments)
		{
			this.def = def;
			this.assignments = assignments;
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x001BD7BF File Offset: 0x001BB9BF
		public LordToil_PsychicRitual(Func<PsychicRitualToil> psychicRitualToilGenerator)
		{
			this.psychicRitualToilGenerator = psychicRitualToilGenerator;
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x001BD7D8 File Offset: 0x001BB9D8
		protected virtual PsychicRitualToil GeneratePsychicRitualToil()
		{
			PsychicRitualToil psychicRitualToil;
			if (this.def != null)
			{
				psychicRitualToil = this.def.CreateGraph();
				if (psychicRitualToil == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
					defaultInterpolatedStringHandler.AppendLiteral("PsychicRitualDef ");
					defaultInterpolatedStringHandler.AppendFormatted<PsychicRitualDef>(this.def);
					defaultInterpolatedStringHandler.AppendLiteral(" returned a null graph during CreateGraph().");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			else
			{
				Func<PsychicRitualToil> func = this.psychicRitualToilGenerator;
				psychicRitualToil = ((func != null) ? func() : null);
				if (psychicRitualToil == null)
				{
					throw new InvalidOperationException("PsychicRitualToil generator " + this.psychicRitualToilGenerator.ToStringSafe<Func<PsychicRitualToil>>() + " returned a null toil when invoked.");
				}
			}
			return psychicRitualToil;
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x001BD870 File Offset: 0x001BBA70
		public override void Init()
		{
			this.data = new PsychicRitualLordToilData();
			this.RitualData.psychicRitual = new PsychicRitual
			{
				lord = this.lord,
				def = this.def,
				assignments = this.assignments
			};
			this.RitualData.iteration = this.iteration;
			this.RitualData.psychicRitualToil = this.GeneratePsychicRitualToil();
			this.RitualData.playerRitual = (this.RitualData.psychicRitual.lord.faction == Faction.OfPlayer);
			this.RitualData.removeLordOnCancel = this.removeLordOnCancel;
			float value;
			this.def.CalculateMaxPower(this.assignments, null, out value);
			this.RitualData.psychicRitual.maxPower = new float?(value);
			this.RitualData.psychicRitual.Start();
			this.RitualData.psychicRitualToil.Start(this.RitualData.psychicRitual, null);
			if (this.RitualData.playerRitual)
			{
				Messages.Message(this.def.PsychicRitualBegunMessage(this.assignments).CapitalizeFirst(), this.assignments.Target, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x001BD9B2 File Offset: 0x001BBBB2
		public override void UpdateAllDuties()
		{
			this.RitualData.psychicRitualToil.UpdateAllDuties(this.RitualData.psychicRitual, null);
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x001BD9D0 File Offset: 0x001BBBD0
		public override void LordToilTick()
		{
			if (this.RitualData.done)
			{
				return;
			}
			if (!this.RitualData.psychicRitual.succeeded)
			{
				this.RitualData.psychicRitual.def.RemoveIncapablePawns(this.RitualData.psychicRitual);
			}
			this.RitualData.psychicRitual.def.CheckPsychicRitualCancelConditions(this.RitualData.psychicRitual);
			if (this.RitualData.done)
			{
				return;
			}
			this.RitualData.done = this.RitualData.psychicRitualToil.Tick(this.RitualData.psychicRitual, null);
			if (this.RitualData.psychicRitual.canceled)
			{
				this.lord.ReceiveMemo("PsychicRitualCanceled");
				return;
			}
			if (this.RitualData.done)
			{
				this.RitualCompleted();
			}
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x001BDAA8 File Offset: 0x001BBCA8
		private void RitualCompleted()
		{
			if (this.RitualData.playerRitual)
			{
				Find.IdeoManager.lastPsychicRitualPerformedTick = Find.TickManager.TicksGame;
				Messages.Message(this.RitualData.psychicRitual.def.PsychicRitualCompletedMessage().CapitalizeFirst(), this.assignments.Target, MessageTypeDefOf.NeutralEvent, true);
			}
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.PsychicRitualPerformed), true);
			foreach (Pawn arg in this.assignments.AllAssignedPawns)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InvolvedInPsychicRitual, arg.Named(HistoryEventArgsNames.Doer)), true);
			}
			this.lord.ReceiveMemo("PsychicRitualCompleted" + this.RitualData.iteration.ToString());
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x001BDBAC File Offset: 0x001BBDAC
		public override void Cleanup()
		{
			this.RitualData.psychicRitualToil.End(this.RitualData.psychicRitual, null, false);
			this.RitualData.done = true;
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x060046EC RID: 18156 RVA: 0x001BDBD7 File Offset: 0x001BBDD7
		public override bool ShouldFail
		{
			get
			{
				return this.RitualData.done || base.ShouldFail;
			}
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x001BDBEE File Offset: 0x001BBDEE
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn pawn)
		{
			return this.RitualData.psychicRitualToil.GetPawnGizmos(this.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x001BDC0D File Offset: 0x001BBE0D
		public override IEnumerable<Gizmo> GetBuildingGizmos(Building building)
		{
			return this.RitualData.psychicRitualToil.GetBuildingGizmos(this.RitualData.psychicRitual, null, building);
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x001BDC2C File Offset: 0x001BBE2C
		public override void Notify_PawnLost(Pawn victim, PawnLostCondition condition)
		{
			PsychicRitual psychicRitual = this.RitualData.psychicRitual;
			psychicRitual.Notify_PawnLost(victim, condition);
			this.RitualData.psychicRitualToil.Notify_PawnLost(psychicRitual, null, victim, condition);
			PsychicRitualRoleDef psychicRitualRoleDef = this.assignments.RoleForPawn(victim, true);
			if (psychicRitualRoleDef != null && !psychicRitualRoleDef.removeOnLost)
			{
				return;
			}
			psychicRitual.assignments.RemoveParticipant(victim);
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x001BDC88 File Offset: 0x001BBE88
		public override void Notify_PawnJobDone(Pawn pawn, JobCondition condition)
		{
			if (pawn.CurJob.lord != this.lord)
			{
				return;
			}
			this.RitualData.psychicRitualToil.Notify_PawnJobDone(this.RitualData.psychicRitual, null, pawn, pawn.CurJob, condition);
			PsychicRitualRoleAssignments psychicRitualRoleAssignments = this.RitualData.psychicRitual.assignments;
			PsychicRitualRoleDef psychicRitualRoleDef = (psychicRitualRoleAssignments != null) ? psychicRitualRoleAssignments.RoleForPawn(pawn, true) : null;
			if (psychicRitualRoleDef != null)
			{
				psychicRitualRoleDef.Notify_PawnJobDone(this.RitualData.psychicRitual, pawn, condition);
			}
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x001BDD02 File Offset: 0x001BBF02
		public override void Notify_BuildingLost(Building building)
		{
			this.RitualData.psychicRitualToil.Notify_BuildingLost(this.RitualData.psychicRitual, null, building);
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x001BDD21 File Offset: 0x001BBF21
		public override void Notify_CorpseLost(Corpse corpse)
		{
			this.RitualData.psychicRitualToil.Notify_CorpseLost(this.RitualData.psychicRitual, null, corpse);
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x001BDD40 File Offset: 0x001BBF40
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.RitualData.psychicRitualToil.Notify_ReachedDutyLocation(this.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x001BDD5F File Offset: 0x001BBF5F
		public override void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			this.RitualData.psychicRitualToil.Notify_ConstructionFailed(this.RitualData.psychicRitual, null, pawn, frame, newBlueprint);
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x001BDD80 File Offset: 0x001BBF80
		public override void Notify_BuildingSpawnedOnMap(Building building)
		{
			this.RitualData.psychicRitualToil.Notify_BuildingSpawnedOnMap(this.RitualData.psychicRitual, null, building);
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x001BDD9F File Offset: 0x001BBF9F
		public override void Notify_BuildingDespawnedOnMap(Building building)
		{
			this.RitualData.psychicRitualToil.Notify_BuildingDespawnedOnMap(this.RitualData.psychicRitual, null, building);
		}

		// Token: 0x04002E1C RID: 11804
		protected PsychicRitualDef def;

		// Token: 0x04002E1D RID: 11805
		protected PsychicRitualRoleAssignments assignments;

		// Token: 0x04002E1E RID: 11806
		protected Func<PsychicRitualToil> psychicRitualToilGenerator;

		// Token: 0x04002E1F RID: 11807
		public int iteration;

		// Token: 0x04002E20 RID: 11808
		public bool removeLordOnCancel = true;
	}
}
