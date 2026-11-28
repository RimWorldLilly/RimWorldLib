using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C6C RID: 3180
	public class LordJob_ExitOnShuttle : LordJob
	{
		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06005A2D RID: 23085 RVA: 0x00212AB2 File Offset: 0x00210CB2
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x00212ABA File Offset: 0x00210CBA
		public LordJob_ExitOnShuttle()
		{
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x00212AC9 File Offset: 0x00210CC9
		public LordJob_ExitOnShuttle(Thing shuttle, bool addFleeToil = true)
		{
			this.shuttle = shuttle;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x00212AE6 File Offset: 0x00210CE6
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x00212AF0 File Offset: 0x00210CF0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckRoyalty("Shuttle crash rescue"))
			{
				return stateGraph;
			}
			LordToil_Wait lordToil_Wait = new LordToil_Wait(true);
			stateGraph.AddToil(lordToil_Wait);
			stateGraph.StartingToil = lordToil_Wait;
			LordToil_EnterShuttleOrLeave lordToil_EnterShuttleOrLeave = new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.Sprint, false, false);
			stateGraph.AddToil(lordToil_EnterShuttleOrLeave);
			Transition transition = new Transition(lordToil_Wait, lordToil_EnterShuttleOrLeave, false, true);
			transition.AddPreAction(new TransitionAction_Custom(new Action(this.InitializeLoading)));
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.shuttle.Spawned));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x00212B7D File Offset: 0x00210D7D
		private void InitializeLoading()
		{
			if (!this.shuttle.TryGetComp<CompTransporter>().LoadingInProgressOrReadyToLaunch)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.shuttle.TryGetComp<CompTransporter>()));
			}
		}

		// Token: 0x06005A33 RID: 23091 RVA: 0x00212BA7 File Offset: 0x00210DA7
		public override void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x040033B0 RID: 13232
		public Thing shuttle;

		// Token: 0x040033B1 RID: 13233
		private bool addFleeToil = true;
	}
}
