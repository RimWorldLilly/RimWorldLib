using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C64 RID: 3172
	public class LordJob_DefendAndExpandHive : LordJob
	{
		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x06005A07 RID: 23047 RVA: 0x00212124 File Offset: 0x00210324
		public override bool CanBlockHostileVisitors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06005A08 RID: 23048 RVA: 0x00212127 File Offset: 0x00210327
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x0021212A File Offset: 0x0021032A
		public LordJob_DefendAndExpandHive()
		{
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x00212132 File Offset: 0x00210332
		public LordJob_DefendAndExpandHive(SpawnedPawnParams parms)
		{
			this.aggressive = parms.aggressive;
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x00212148 File Offset: 0x00210348
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendAndExpandHive lordToil_DefendAndExpandHive = new LordToil_DefendAndExpandHive();
			lordToil_DefendAndExpandHive.distToHiveToAttack = 10f;
			stateGraph.StartingToil = lordToil_DefendAndExpandHive;
			LordToil_DefendHiveAggressively lordToil_DefendHiveAggressively = new LordToil_DefendHiveAggressively();
			lordToil_DefendHiveAggressively.distToHiveToAttack = 40f;
			stateGraph.AddToil(lordToil_DefendHiveAggressively);
			LordToil lordToil = new LordToil_AssaultColony(false, false);
			if (base.Map.generatorDef == MapGeneratorDefOf.InsectLair)
			{
				lordToil = new LordToil_NestAssault(false, false);
			}
			stateGraph.AddToil(lordToil);
			Transition transition = new Transition(lordToil_DefendAndExpandHive, this.aggressive ? lordToil : lordToil_DefendHiveAggressively, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null, null, null));
			transition.AddTrigger(new Trigger_PawnLostViolently(false));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoAttackedByEnemy));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoBurnedBadly));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoDestroyedNonRoofCollapse));
			transition.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendAndExpandHive, lordToil, false, true);
			transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction, null, null));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendHiveAggressively, lordToil, false, true);
			transition3.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction, null, null));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_DefendAndExpandHive, lordToil_DefendAndExpandHive, true, true);
			transition4.AddTrigger(new Trigger_Memo(Hive.MemoDeSpawned));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_DefendHiveAggressively, lordToil_DefendHiveAggressively, true, true);
			transition5.AddTrigger(new Trigger_Memo(Hive.MemoDeSpawned));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil, lordToil_DefendAndExpandHive, false, true);
			transition6.AddSource(lordToil_DefendHiveAggressively);
			transition6.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1200, new string[]
			{
				Hive.MemoAttackedByEnemy,
				Hive.MemoBurnedBadly,
				Hive.MemoDestroyedNonRoofCollapse,
				Hive.MemoDeSpawned,
				HediffGiver_Heat.MemoPawnBurnedByAir
			}));
			transition6.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x00212390 File Offset: 0x00210590
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}

		// Token: 0x06005A0D RID: 23053 RVA: 0x0021239C File Offset: 0x0021059C
		public override bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			if (!base.ValidateAttackTarget(searcher, target))
			{
				return false;
			}
			MentalStateDef mentalStateDef = searcher.MentalStateDef;
			return mentalStateDef == null || !mentalStateDef.blocksDefendAndExpandHive;
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x002123CA File Offset: 0x002105CA
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
		}

		// Token: 0x040033A0 RID: 13216
		private bool aggressive;
	}
}
