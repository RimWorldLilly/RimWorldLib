using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C66 RID: 3174
	public class LordJob_DefendBase : LordJob
	{
		// Token: 0x06005A13 RID: 23059 RVA: 0x0021247F File Offset: 0x0021067F
		public LordJob_DefendBase()
		{
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x00212487 File Offset: 0x00210687
		public LordJob_DefendBase(Faction faction, IntVec3 baseCenter, int delayBeforeAssault, bool attackWhenPlayerBecameEnemy = false)
		{
			this.faction = faction;
			this.baseCenter = baseCenter;
			this.attackWhenPlayerBecameEnemy = attackWhenPlayerBecameEnemy;
			this.delayBeforeAssault = delayBeforeAssault;
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x002124AC File Offset: 0x002106AC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendBase lordToil_DefendBase = new LordToil_DefendBase(this.baseCenter);
			stateGraph.StartingToil = lordToil_DefendBase;
			LordToil_DefendBase lordToil_DefendBase2 = new LordToil_DefendBase(this.baseCenter);
			stateGraph.AddToil(lordToil_DefendBase2);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(true, false);
			lordToil_AssaultColony.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition = new Transition(lordToil_DefendBase, lordToil_DefendBase2, false, true);
			transition.AddSource(lordToil_AssaultColony);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendBase2, this.attackWhenPlayerBecameEnemy ? lordToil_AssaultColony : lordToil_DefendBase, false, true);
			if (this.attackWhenPlayerBecameEnemy)
			{
				transition2.AddSource(lordToil_DefendBase);
			}
			transition2.AddTrigger(new Trigger_BecamePlayerEnemy());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendBase, lordToil_AssaultColony, false, true);
			transition3.AddTrigger(new Trigger_FractionPawnsLost(0.2f));
			transition3.AddTrigger(new Trigger_PawnHarmed(0.4f, false, null, null, null));
			transition3.AddTrigger(new Trigger_ChanceOnTickInterval(2500, 0.03f));
			transition3.AddTrigger(new Trigger_TicksPassed(this.delayBeforeAssault));
			transition3.AddTrigger(new Trigger_UrgentlyHungry());
			transition3.AddTrigger(new Trigger_ChanceOnPlayerHarmNPCBuilding(0.4f));
			transition3.AddTrigger(new Trigger_OnClamor(ClamorDefOf.Ability));
			transition3.AddPostAction(new TransitionAction_WakeAll());
			TaggedString taggedString = this.faction.def.messageDefendersAttacking.Formatted(this.faction.def.pawnsPlural, this.faction.Name, Faction.OfPlayer.def.pawnsPlural).CapitalizeFirst();
			transition3.AddPreAction(new TransitionAction_Message(taggedString, MessageTypeDefOf.ThreatBig, null, 1f, null));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x00212678 File Offset: 0x00210878
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.attackWhenPlayerBecameEnemy, "attackWhenPlayerBecameEnemy", false, false);
			Scribe_Values.Look<int>(ref this.delayBeforeAssault, "delayBeforeAssault", 25000, false);
		}

		// Token: 0x040033A2 RID: 13218
		private Faction faction;

		// Token: 0x040033A3 RID: 13219
		private IntVec3 baseCenter;

		// Token: 0x040033A4 RID: 13220
		private bool attackWhenPlayerBecameEnemy;

		// Token: 0x040033A5 RID: 13221
		private int delayBeforeAssault;

		// Token: 0x040033A6 RID: 13222
		private List<Pawn> tmpPawns;

		// Token: 0x040033A7 RID: 13223
		private List<IntVec3> tmpCells;
	}
}
