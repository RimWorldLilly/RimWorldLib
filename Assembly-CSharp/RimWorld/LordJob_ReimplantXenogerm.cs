using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C7A RID: 3194
	public class LordJob_ReimplantXenogerm : LordJob
	{
		// Token: 0x06005A97 RID: 23191 RVA: 0x002153A0 File Offset: 0x002135A0
		public LordJob_ReimplantXenogerm()
		{
		}

		// Token: 0x06005A98 RID: 23192 RVA: 0x002153A8 File Offset: 0x002135A8
		public LordJob_ReimplantXenogerm(IntVec3 idleSpot, int waitDurationTicks, string inSignalReimplanted)
		{
			this.idleSpot = idleSpot;
			this.waitDurationTicks = waitDurationTicks;
			this.inSignalReimplanted = inSignalReimplanted;
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x002153C8 File Offset: 0x002135C8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckBiotech("Xenogerm reimplantation reward"))
			{
				return stateGraph;
			}
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.idleSpot);
			stateGraph.AddToil(lordToil_Travel);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_ReimplantXenogerm lordToil_ReimplantXenogerm = new LordToil_ReimplantXenogerm(this.idleSpot);
			stateGraph.AddToil(lordToil_ReimplantXenogerm);
			LordToil_ExitMapRandom lordToil_ExitMapRandom = new LordToil_ExitMapRandom();
			stateGraph.AddToil(lordToil_ExitMapRandom);
			LordToil_ExitMapAndDefendSelf lordToil_ExitMapAndDefendSelf = new LordToil_ExitMapAndDefendSelf();
			stateGraph.AddToil(lordToil_ExitMapAndDefendSelf);
			Transition transition = new Transition(lordToil_Travel, lordToil_ReimplantXenogerm, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			transition.canMoveToSameState = true;
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_ReimplantXenogerm, lordToil_ExitMapRandom, false, true);
			transition2.AddSource(lordToil_Travel);
			transition2.AddTrigger(new Trigger_Signal(this.inSignalReimplanted));
			transition2.AddTrigger(new Trigger_TicksPassed(this.waitDurationTicks));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_ReimplantXenogerm, lordToil_ExitMapAndDefendSelf, false, true);
			transition3.AddSource(lordToil_Travel);
			transition3.AddSource(lordToil_ExitMapRandom);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition3.AddTrigger(new Trigger_PawnKilled());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x002154F8 File Offset: 0x002136F8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.idleSpot, "idleSpot", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.inSignalReimplanted, "inSignalReimplanted", null, false);
			Scribe_Values.Look<int>(ref this.waitDurationTicks, "waitDurationTicks", 0, false);
		}

		// Token: 0x040033F4 RID: 13300
		private IntVec3 idleSpot;

		// Token: 0x040033F5 RID: 13301
		private string inSignalReimplanted;

		// Token: 0x040033F6 RID: 13302
		private int waitDurationTicks;
	}
}
