using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C58 RID: 3160
	public class LordJob_AssaultColony : LordJob, ILordAvoidTraps
	{
		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x060059B1 RID: 22961 RVA: 0x0020FA24 File Offset: 0x0020DC24
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x060059B2 RID: 22962 RVA: 0x0020FA27 File Offset: 0x0020DC27
		public float AvoidTrapRatio
		{
			get
			{
				if (!this.useAvoidGridSmart)
				{
					return 0f;
				}
				return 0.3f;
			}
		}

		// Token: 0x060059B3 RID: 22963 RVA: 0x0020FA3C File Offset: 0x0020DC3C
		public LordJob_AssaultColony()
		{
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x0020FA5C File Offset: 0x0020DC5C
		public LordJob_AssaultColony(SpawnedPawnParams parms)
		{
			this.assaulterFaction = parms.spawnerThing.Faction;
			this.canKidnap = false;
			this.canTimeoutOrFlee = false;
			this.canSteal = false;
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x0020FAAC File Offset: 0x0020DCAC
		public LordJob_AssaultColony(Faction assaulterFaction, bool canKidnap = true, bool canTimeoutOrFlee = true, bool sappers = false, bool useAvoidGridSmart = false, bool canSteal = true, bool breachers = false, bool canPickUpOpportunisticWeapons = false)
		{
			this.assaulterFaction = assaulterFaction;
			this.canKidnap = canKidnap;
			this.canTimeoutOrFlee = canTimeoutOrFlee;
			this.sappers = sappers;
			this.useAvoidGridSmart = useAvoidGridSmart;
			this.canSteal = canSteal;
			this.breachers = breachers;
			this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x0020FB14 File Offset: 0x0020DD14
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			List<LordToil> list = new List<LordToil>();
			LordToil lordToil = null;
			if (this.sappers)
			{
				lordToil = new LordToil_AssaultColonySappers();
				if (this.useAvoidGridSmart)
				{
					lordToil.useAvoidGrid = true;
				}
				stateGraph.AddToil(lordToil);
				list.Add(lordToil);
				Transition transition = new Transition(lordToil, lordToil, true, true);
				transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
				stateGraph.AddTransition(transition, false);
			}
			if (this.breachers)
			{
				LordToil lordToil2 = new LordToil_AssaultColonyBreaching();
				if (this.useAvoidGridSmart)
				{
					lordToil2.useAvoidGrid = this.useAvoidGridSmart;
				}
				stateGraph.AddToil(lordToil2);
				list.Add(lordToil2);
			}
			LordToil lordToil3 = new LordToil_AssaultColony(false, this.canPickUpOpportunisticWeapons);
			if (this.useAvoidGridSmart)
			{
				lordToil3.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil3);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, true);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.sappers)
			{
				Transition transition2 = new Transition(lordToil, lordToil3, false, true);
				transition2.AddTrigger(new Trigger_NoFightingSappers());
				stateGraph.AddTransition(transition2, false);
			}
			if (this.assaulterFaction != null && this.assaulterFaction.def.humanlikeFaction)
			{
				if (this.canTimeoutOrFlee)
				{
					Transition transition3 = new Transition(lordToil3, lordToil_ExitMap, false, true);
					transition3.AddSources(list);
					IntRange intRange;
					if (this.sappers)
					{
						intRange = LordJob_AssaultColony.SapTimeBeforeGiveUp;
					}
					else if (this.breachers)
					{
						intRange = LordJob_AssaultColony.BreachTimeBeforeGiveUp;
					}
					else
					{
						intRange = LordJob_AssaultColony.AssaultTimeBeforeGiveUp;
					}
					Trigger_TicksPassed trigger_TicksPassed = new Trigger_TicksPassed(intRange.RandomInRange);
					trigger_TicksPassed.WithFilter(new TriggerFilter_MapExitable());
					transition3.AddTrigger(trigger_TicksPassed);
					transition3.AddPreAction(new TransitionAction_Message("MessageRaidersGivenUpLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					stateGraph.AddTransition(transition3, false);
					Transition transition4 = new Transition(lordToil3, lordToil_ExitMap, false, true);
					transition4.AddSources(list);
					FloatRange floatRange = new FloatRange(0.25f, 0.35f);
					Trigger_FractionColonyDamageTaken trigger_FractionColonyDamageTaken = new Trigger_FractionColonyDamageTaken(floatRange.RandomInRange, 900f);
					trigger_FractionColonyDamageTaken.WithFilter(new TriggerFilter_MapExitable());
					transition4.AddTrigger(trigger_FractionColonyDamageTaken);
					transition4.AddPreAction(new TransitionAction_Message("MessageRaidersSatisfiedLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					stateGraph.AddTransition(transition4, false);
				}
				if (this.canKidnap)
				{
					LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Kidnap().CreateGraph()).StartingToil;
					Transition transition5 = new Transition(lordToil3, startingToil, false, true);
					transition5.AddSources(list);
					transition5.AddPreAction(new TransitionAction_Message("MessageRaidersKidnapping".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					transition5.AddTrigger(new Trigger_KidnapVictimPresent());
					stateGraph.AddTransition(transition5, false);
				}
				if (this.canSteal)
				{
					LordToil startingToil2 = stateGraph.AttachSubgraph(new LordJob_Steal().CreateGraph()).StartingToil;
					Transition transition6 = new Transition(lordToil3, startingToil2, false, true);
					transition6.AddSources(list);
					transition6.AddPreAction(new TransitionAction_Message("MessageRaidersStealing".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					transition6.AddTrigger(new Trigger_HighValueThingsAround());
					stateGraph.AddTransition(transition6, false);
				}
			}
			if (this.assaulterFaction != null && this.assaulterFaction == Faction.OfMechanoids)
			{
				Transition transition7 = new Transition(lordToil3, lordToil_ExitMap, false, true);
				transition7.AddSources(list);
				transition7.AddTrigger(new Trigger_GameEnding(2500));
				transition7.AddPreAction(new TransitionAction_Message("MechanoidsMovingOn".Translate(), null, 1f));
				stateGraph.AddTransition(transition7, false);
			}
			if (this.assaulterFaction != null)
			{
				Transition transition8 = new Transition(lordToil3, lordToil_ExitMap, false, true);
				transition8.AddSources(list);
				transition8.AddTrigger(new Trigger_BecameNonHostileToPlayer());
				transition8.AddPreAction(new TransitionAction_Message("MessageRaidersLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
				stateGraph.AddTransition(transition8, false);
			}
			return stateGraph;
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x0020FFAC File Offset: 0x0020E1AC
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<bool>(ref this.sappers, "sappers", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
			Scribe_Values.Look<bool>(ref this.breachers, "breaching", false, false);
			Scribe_Values.Look<bool>(ref this.canPickUpOpportunisticWeapons, "canPickUpOpportunisticWeapons", false, false);
		}

		// Token: 0x0400335C RID: 13148
		private Faction assaulterFaction;

		// Token: 0x0400335D RID: 13149
		private bool canKidnap = true;

		// Token: 0x0400335E RID: 13150
		private bool canTimeoutOrFlee = true;

		// Token: 0x0400335F RID: 13151
		private bool sappers;

		// Token: 0x04003360 RID: 13152
		private bool useAvoidGridSmart;

		// Token: 0x04003361 RID: 13153
		private bool canSteal = true;

		// Token: 0x04003362 RID: 13154
		private bool breachers;

		// Token: 0x04003363 RID: 13155
		private bool canPickUpOpportunisticWeapons;

		// Token: 0x04003364 RID: 13156
		private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);

		// Token: 0x04003365 RID: 13157
		private static readonly IntRange SapTimeBeforeGiveUp = new IntRange(33000, 38000);

		// Token: 0x04003366 RID: 13158
		private static readonly IntRange BreachTimeBeforeGiveUp = new IntRange(33000, 38000);
	}
}
