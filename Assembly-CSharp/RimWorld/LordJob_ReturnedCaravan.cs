using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C7B RID: 3195
	public class LordJob_ReturnedCaravan : LordJob
	{
		// Token: 0x06005A9B RID: 23195 RVA: 0x00215543 File Offset: 0x00213743
		public LordJob_ReturnedCaravan(IntVec3 entryPoint)
		{
			this.entryPoint = entryPoint;
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x00215552 File Offset: 0x00213752
		public LordJob_ReturnedCaravan()
		{
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x0021555C File Offset: 0x0021375C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ReturnedCaravan_PenAnimals lordToil_ReturnedCaravan_PenAnimals = new LordToil_ReturnedCaravan_PenAnimals(this.entryPoint);
			stateGraph.AddToil(lordToil_ReturnedCaravan_PenAnimals);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_ReturnedCaravan_PenAnimals, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_Memo("RepenningFinished"));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x002155B0 File Offset: 0x002137B0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.entryPoint, "entryPoint", default(IntVec3), false);
		}

		// Token: 0x040033F7 RID: 13303
		private IntVec3 entryPoint;
	}
}
