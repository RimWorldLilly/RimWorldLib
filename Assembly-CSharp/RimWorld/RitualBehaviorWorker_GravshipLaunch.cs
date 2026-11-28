using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200168E RID: 5774
	public class RitualBehaviorWorker_GravshipLaunch : RitualBehaviorWorker
	{
		// Token: 0x060090BB RID: 37051 RVA: 0x003517A8 File Offset: 0x0034F9A8
		public RitualBehaviorWorker_GravshipLaunch()
		{
		}

		// Token: 0x060090BC RID: 37052 RVA: 0x003517F8 File Offset: 0x0034F9F8
		public RitualBehaviorWorker_GravshipLaunch(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x170018FD RID: 6397
		// (get) Token: 0x060090BD RID: 37053 RVA: 0x00351849 File Offset: 0x0034FA49
		public override bool ChecksReservations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060090BE RID: 37054 RVA: 0x0035184C File Offset: 0x0034FA4C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forceVisitorsToLeave, "forceVisitorsToLeave", true, false);
			Scribe_Values.Look<bool>(ref this.boardColonyAnimals, "boardColonyAnimals", true, false);
			Scribe_Values.Look<bool>(ref this.boardColonyMechs, "boardColonyMechs", true, false);
		}

		// Token: 0x060090BF RID: 37055 RVA: 0x0035188A File Offset: 0x0034FA8A
		public override bool TargetStillAllowed(TargetInfo selectedTarget, LordJob_Ritual ritual)
		{
			return base.TargetStillAllowed(selectedTarget, ritual);
		}

		// Token: 0x060090C0 RID: 37056 RVA: 0x00351899 File Offset: 0x0034FA99
		public override string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return null;
		}

		// Token: 0x060090C1 RID: 37057 RVA: 0x0035189C File Offset: 0x0034FA9C
		private bool CanReachGravship(Pawn pawn, Building_GravEngine engine)
		{
			if (Find.TickManager.TicksGame != this.cacheTick)
			{
				this.cacheTick = Find.TickManager.TicksGame;
				this.cachedCanReachGravship.Clear();
			}
			bool result;
			if (this.cachedCanReachGravship.TryGetValue(pawn, out result))
			{
				return result;
			}
			IntVec3 intVec;
			bool flag = GravshipUtility.TryFindSpotOnGravship(pawn, engine, out intVec);
			this.cachedCanReachGravship[pawn] = flag;
			return flag;
		}

		// Token: 0x060090C2 RID: 37058 RVA: 0x00351900 File Offset: 0x0034FB00
		public override bool PawnCanFillRole(Pawn pawn, RitualRole role, out string reason, TargetInfo ritualTarget)
		{
			reason = null;
			CompPilotConsole compPilotConsole = ritualTarget.Thing.TryGetComp<CompPilotConsole>();
			Building_GravEngine building_GravEngine = (compPilotConsole != null) ? compPilotConsole.engine : null;
			if (building_GravEngine == null)
			{
				Log.ErrorOnce("Could not find engine for gravship launch", 23184679);
				return false;
			}
			if (role == null)
			{
				if (!this.CanReachGravship(pawn, building_GravEngine))
				{
					reason = "NoPathToGravship".Translate();
					return false;
				}
			}
			else if (!pawn.IsPrisoner && !pawn.CanReach(ritualTarget.Thing, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				reason = "NoPathToPilotConsole".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x060090C3 RID: 37059 RVA: 0x00351994 File Offset: 0x0034FB94
		public override void TryExecuteOn(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments, bool playerForced = false)
		{
			CompPilotConsole compPilotConsole = target.Thing.TryGetComp<CompPilotConsole>();
			Building_GravEngine building_GravEngine = (compPilotConsole != null) ? compPilotConsole.engine : null;
			building_GravEngine.pawnsToBoard = new HashSet<Pawn>();
			building_GravEngine.pawnsToLeave = new HashSet<Pawn>();
			this.tmpPawns.Clear();
			this.tmpPawns.AddRange(target.Map.mapPawns.AllPawnsSpawned);
			foreach (Pawn pawn in this.tmpPawns)
			{
				if (!pawn.Downed)
				{
					if (this.forceVisitorsToLeave && pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer))
					{
						building_GravEngine.pawnsToLeave.Add(pawn);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					else if (this.boardColonyAnimals && pawn.IsColonyAnimal)
					{
						building_GravEngine.pawnsToBoard.Add(pawn);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					else if (ModsConfig.BiotechActive && this.boardColonyMechs && pawn.IsColonyMech)
					{
						building_GravEngine.pawnsToBoard.Add(pawn);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
			base.TryExecuteOn(target, organizer, ritual, obligation, assignments, playerForced);
		}

		// Token: 0x060090C4 RID: 37060 RVA: 0x00351B04 File Offset: 0x0034FD04
		public override void Cleanup(LordJob_Ritual ritual)
		{
			CompPilotConsole compPilotConsole = ritual.selectedTarget.Thing.TryGetComp<CompPilotConsole>();
			Building_GravEngine building_GravEngine = (compPilotConsole != null) ? compPilotConsole.engine : null;
			if (building_GravEngine != null)
			{
				if (building_GravEngine.pawnsToBoard != null)
				{
					this.tmpPawnToEndJob.AddRange(building_GravEngine.pawnsToBoard);
				}
				if (building_GravEngine.pawnsToLeave != null)
				{
					this.tmpPawnToEndJob.AddRange(building_GravEngine.pawnsToLeave);
				}
				building_GravEngine.pawnsToBoard = null;
				building_GravEngine.pawnsToLeave = null;
			}
			foreach (Pawn pawn in this.tmpPawnToEndJob)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			this.tmpPawnToEndJob.Clear();
			base.Cleanup(ritual);
		}

		// Token: 0x0400583A RID: 22586
		public bool forceVisitorsToLeave = true;

		// Token: 0x0400583B RID: 22587
		public bool boardColonyAnimals = true;

		// Token: 0x0400583C RID: 22588
		public bool boardColonyMechs = true;

		// Token: 0x0400583D RID: 22589
		private Dictionary<Pawn, bool> cachedCanReachGravship = new Dictionary<Pawn, bool>();

		// Token: 0x0400583E RID: 22590
		private int cacheTick = -1;

		// Token: 0x0400583F RID: 22591
		private List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04005840 RID: 22592
		private List<Pawn> tmpPawnToEndJob = new List<Pawn>();
	}
}
