using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C8F RID: 3215
	public class LordJob_VoidAwakeningDefendStructure : LordJob
	{
		// Token: 0x06005B0C RID: 23308 RVA: 0x002182B0 File Offset: 0x002164B0
		public LordJob_VoidAwakeningDefendStructure()
		{
		}

		// Token: 0x06005B0D RID: 23309 RVA: 0x002182B8 File Offset: 0x002164B8
		public LordJob_VoidAwakeningDefendStructure(IntVec3 defendSpot, float defendRadius)
		{
			this.defendSpot = defendSpot;
			this.defendRadius = defendRadius;
		}

		// Token: 0x06005B0E RID: 23310 RVA: 0x002182D0 File Offset: 0x002164D0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendSpot, "defendSpot", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 0f, false);
		}

		// Token: 0x06005B0F RID: 23311 RVA: 0x00218310 File Offset: 0x00216510
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.defendSpot, new float?(this.defendRadius), null);
			stateGraph.AddToil(lordToil_DefendPoint);
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(Faction.OfEntities, true, true, false, false, true, false, false).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_DefendPoint, startingToil, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(0.33f, false, null, null, null));
			transition.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x04003433 RID: 13363
		private IntVec3 defendSpot;

		// Token: 0x04003434 RID: 13364
		private float defendRadius;
	}
}
