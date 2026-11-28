using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C59 RID: 3161
	public class LordJob_AssaultThings : LordJob
	{
		// Token: 0x060059B9 RID: 22969 RVA: 0x00210086 File Offset: 0x0020E286
		public LordJob_AssaultThings()
		{
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x0021008E File Offset: 0x0020E28E
		public LordJob_AssaultThings(Faction assaulterFaction, List<Thing> things, float damageFraction = 1f, bool useAvoidGridSmart = false)
		{
			this.assaulterFaction = assaulterFaction;
			this.things = things;
			this.useAvoidGridSmart = useAvoidGridSmart;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x002100B4 File Offset: 0x0020E2B4
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil lordToil = new LordToil_AssaultThings(this.things);
			if (this.useAvoidGridSmart)
			{
				lordToil.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil);
			LordToil_ExitMapAndDefendSelf lordToil_ExitMapAndDefendSelf = new LordToil_ExitMapAndDefendSelf();
			lordToil_ExitMapAndDefendSelf.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMapAndDefendSelf);
			Transition transition = new Transition(lordToil, lordToil_ExitMapAndDefendSelf, false, true);
			transition.AddTrigger(new Trigger_ThingsDamageTaken(this.things, this.damageFraction));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x00210128 File Offset: 0x0020E328
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
			Scribe_Values.Look<float>(ref this.damageFraction, "damageFraction", 0f, false);
		}

		// Token: 0x04003367 RID: 13159
		private Faction assaulterFaction;

		// Token: 0x04003368 RID: 13160
		private List<Thing> things;

		// Token: 0x04003369 RID: 13161
		private bool useAvoidGridSmart;

		// Token: 0x0400336A RID: 13162
		private float damageFraction;
	}
}
