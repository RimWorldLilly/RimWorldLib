using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C81 RID: 3201
	public class LordJob_Siege : LordJob
	{
		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x06005ABB RID: 23227 RVA: 0x00215F57 File Offset: 0x00214157
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x00215F5A File Offset: 0x0021415A
		public LordJob_Siege()
		{
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x00215F62 File Offset: 0x00214162
		public LordJob_Siege(Faction faction, IntVec3 siegeSpot, float blueprintPoints)
		{
			this.faction = faction;
			this.siegeSpot = siegeSpot;
			this.blueprintPoints = blueprintPoints;
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x00215F80 File Offset: 0x00214180
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.siegeSpot).CreateGraph()).StartingToil;
			LordToil_Siege lordToil_Siege = new LordToil_Siege(this.siegeSpot, this.blueprintPoints);
			stateGraph.AddToil(lordToil_Siege);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, true);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil startingToil2 = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, false).CreateGraph()).StartingToil;
			Transition transition = new Transition(startingToil, lordToil_Siege, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			transition.AddTrigger(new Trigger_TicksPassed(5000));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Siege, startingToil2, false, true);
			transition2.AddTrigger(new Trigger_Memo("NoBuilders"));
			transition2.AddTrigger(new Trigger_Memo("NoArtillery"));
			transition2.AddTrigger(new Trigger_PawnHarmed(0.08f, false, null, null, null));
			transition2.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			transition2.AddTrigger(new Trigger_TicksPassed((int)(60000f * Rand.Range(1.5f, 3f))));
			transition2.AddPreAction(new TransitionAction_Message("MessageSiegersAssaulting".Translate(this.faction.def.pawnsPlural, this.faction), MessageTypeDefOf.ThreatBig, null, 1f, null));
			transition2.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Siege, lordToil_ExitMap, false, true);
			transition3.AddSource(startingToil2);
			transition3.AddSource(startingToil);
			transition3.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition3.AddPreAction(new TransitionAction_Message("MessageRaidersLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x0021618C File Offset: 0x0021438C
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.siegeSpot, "siegeSpot", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.blueprintPoints, "blueprintPoints", 0f, false);
		}

		// Token: 0x04003408 RID: 13320
		private Faction faction;

		// Token: 0x04003409 RID: 13321
		private IntVec3 siegeSpot;

		// Token: 0x0400340A RID: 13322
		private float blueprintPoints;
	}
}
