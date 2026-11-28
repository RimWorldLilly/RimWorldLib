using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C85 RID: 3205
	public class LordJob_SlaveRebellion : LordJob
	{
		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06005AC9 RID: 23241 RVA: 0x002163DC File Offset: 0x002145DC
		public override bool NeverInRestraints
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x06005ACA RID: 23242 RVA: 0x002163DF File Offset: 0x002145DF
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06005ACB RID: 23243 RVA: 0x002163E2 File Offset: 0x002145E2
		public bool IsAggressiveRebellion
		{
			get
			{
				return !this.passive;
			}
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x002163ED File Offset: 0x002145ED
		public LordJob_SlaveRebellion()
		{
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x002163FC File Offset: 0x002145FC
		public LordJob_SlaveRebellion(IntVec3 groupUpLoc, IntVec3 exitPoint, int sapperThingID, bool passive = true)
		{
			this.groupUpLoc = groupUpLoc;
			this.exitPoint = exitPoint;
			this.sapperThingID = sapperThingID;
			this.passive = passive;
		}

		// Token: 0x06005ACE RID: 23246 RVA: 0x00216428 File Offset: 0x00214628
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckIdeology("Slave rebellion"))
			{
				return stateGraph;
			}
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.groupUpLoc);
			lordToil_Travel.maxDanger = Danger.Deadly;
			lordToil_Travel.useAvoidGrid = true;
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_AssaultColonyPrisoners lordToil_AssaultColonyPrisoners = new LordToil_AssaultColonyPrisoners();
			lordToil_AssaultColonyPrisoners.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_AssaultColonyPrisoners);
			LordToil_PrisonerEscape lordToil_PrisonerEscape = new LordToil_PrisonerEscape(this.exitPoint, this.sapperThingID);
			lordToil_PrisonerEscape.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_PrisonerEscape);
			LordToil_ExitMapFighting lordToil_ExitMapFighting = new LordToil_ExitMapFighting(LocomotionUrgency.Jog, true, false);
			lordToil_ExitMapFighting.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMapFighting);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, true, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog, false, false);
			lordToil_ExitMap2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap2);
			if (!this.passive)
			{
				Transition transition = new Transition(lordToil_Travel, lordToil_AssaultColonyPrisoners, false, true);
				transition.AddTrigger(new Trigger_Memo("TravelArrived"));
				transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
				stateGraph.AddTransition(transition, false);
				Transition transition2 = new Transition(lordToil_AssaultColonyPrisoners, lordToil_ExitMapFighting, false, true);
				transition2.AddTrigger(new Trigger_FractionColonyDamageTaken(LordJob_SlaveRebellion.DesiredDamageRange.RandomInRange, 900f));
				transition2.AddTrigger(new Trigger_Memo("TravelArrived"));
				stateGraph.AddTransition(transition2, false);
				Transition transition3 = new Transition(lordToil_AssaultColonyPrisoners, lordToil_ExitMapFighting, false, true);
				transition3.AddTrigger(new Trigger_TicksPassed(LordJob_SlaveRebellion.AssaultTimeBeforeGiveUp.RandomInRange));
				transition3.AddTrigger(new Trigger_Memo("TravelArrived"));
				stateGraph.AddTransition(transition3, false);
			}
			else
			{
				Transition transition4 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
				transition4.AddTrigger(new Trigger_Memo("TravelArrived"));
				stateGraph.AddTransition(transition4, false);
				Transition transition5 = new Transition(lordToil_Travel, lordToil_PrisonerEscape, false, true);
				transition5.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
				stateGraph.AddTransition(transition5, false);
				Transition transition6 = new Transition(lordToil_Travel, lordToil_ExitMap, false, true);
				transition6.AddSources(new LordToil[]
				{
					lordToil_AssaultColonyPrisoners,
					lordToil_PrisonerEscape,
					lordToil_ExitMapFighting
				});
				transition6.AddTrigger(new Trigger_PawnCannotReachMapEdge());
				stateGraph.AddTransition(transition6, false);
				Transition transition7 = new Transition(lordToil_ExitMap, lordToil_PrisonerEscape, false, true);
				transition7.AddTrigger(new Trigger_PawnCanReachMapEdge());
				transition7.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition7, false);
			}
			Transition transition8 = new Transition(lordToil_PrisonerEscape, lordToil_ExitMap2, false, true);
			transition8.AddSource(lordToil_ExitMapFighting);
			transition8.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition8, false);
			return stateGraph;
		}

		// Token: 0x06005ACF RID: 23247 RVA: 0x00216698 File Offset: 0x00214898
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.groupUpLoc, "groupUpLoc", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.exitPoint, "exitPoint", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.sapperThingID, "sapperThingID", -1, false);
			Scribe_Values.Look<bool>(ref this.passive, "passive", false, false);
		}

		// Token: 0x06005AD0 RID: 23248 RVA: 0x00216703 File Offset: 0x00214903
		public override void Notify_PawnAdded(Pawn p)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06005AD1 RID: 23249 RVA: 0x0021670B File Offset: 0x0021490B
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			ReachabilityUtility.ClearCacheFor(p);
		}

		// Token: 0x06005AD2 RID: 23250 RVA: 0x00216713 File Offset: 0x00214913
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return true;
		}

		// Token: 0x06005AD3 RID: 23251 RVA: 0x00216718 File Offset: 0x00214918
		public override bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			Pawn pawn = target as Pawn;
			if (pawn == null)
			{
				return true;
			}
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			return mentalStateDef == null || !mentalStateDef.escapingPrisonersIgnore;
		}

		// Token: 0x04003410 RID: 13328
		private IntVec3 groupUpLoc;

		// Token: 0x04003411 RID: 13329
		private IntVec3 exitPoint;

		// Token: 0x04003412 RID: 13330
		private bool passive;

		// Token: 0x04003413 RID: 13331
		private int sapperThingID = -1;

		// Token: 0x04003414 RID: 13332
		private static FloatRange DesiredDamageRange = new FloatRange(0.25f, 0.35f);

		// Token: 0x04003415 RID: 13333
		private const float MinDamage = 900f;

		// Token: 0x04003416 RID: 13334
		private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);
	}
}
