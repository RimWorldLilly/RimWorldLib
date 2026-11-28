using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C77 RID: 3191
	public class LordJob_Metalhorror : LordJob
	{
		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06005A81 RID: 23169 RVA: 0x00214C66 File Offset: 0x00212E66
		public override bool CanAutoAddPawns
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005A82 RID: 23170 RVA: 0x00214C6C File Offset: 0x00212E6C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_MetalhorrorActive lordToil_MetalhorrorActive = new LordToil_MetalhorrorActive();
			stateGraph.AddToil(lordToil_MetalhorrorActive);
			stateGraph.StartingToil = lordToil_MetalhorrorActive;
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.AddToil(lordToil_Sleep);
			Transition transition = new Transition(lordToil_MetalhorrorActive, lordToil_Sleep, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal _) => this.lord.ownedPawns.Count != 0 && GenTicks.IsTickInterval(120) && this.ShouldBeHibernating()));
			transition.AddPostAction(new TransitionAction_Message("MessageMetalHorrorDormant".Translate(), MessageTypeDefOf.NeutralEvent, null, 1f, null));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Sleep, lordToil_MetalhorrorActive, false, true);
			transition2.AddTrigger(new Trigger_DormancyWakeup());
			transition2.AddTrigger(new Trigger_Custom((TriggerSignal _) => this.lord.ownedPawns.Count != 0 && GenTicks.IsTickInterval(120) && !this.ShouldBeHibernating()));
			transition2.AddPostAction(new TransitionAction_WakeAll());
			transition2.AddPostAction(new TransitionAction_Letter("LetterMetalhorrorReawakeningLabel".Translate(), "LetterMetalhorrorReawakening".Translate(), LetterDefOf.ThreatBig));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x06005A83 RID: 23171 RVA: 0x00214D5C File Offset: 0x00212F5C
		private bool ShouldBeHibernating()
		{
			bool result = false;
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				if (pawn.SpawnedOrAnyParentSpawned)
				{
					result = true;
					if (GenTicks.TicksGame < pawn.TickSpawned + 15000 || pawn.mindState.WasRecentlyCombatantTicks(15000) || GenTicks.TicksGame < pawn.mindState.hibernationEndedTick + 7500 || pawn.IsOnHoldingPlatform)
					{
						return false;
					}
				}
			}
			return result;
		}

		// Token: 0x06005A84 RID: 23172 RVA: 0x00214E08 File Offset: 0x00213008
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
		}

		// Token: 0x040033E8 RID: 13288
		public int biosignature;

		// Token: 0x040033E9 RID: 13289
		private const int TicksToDormant = 15000;

		// Token: 0x040033EA RID: 13290
		private const int TicksToDormantHibernation = 7500;

		// Token: 0x040033EB RID: 13291
		private const int TransitionCheckRateTicks = 120;
	}
}
