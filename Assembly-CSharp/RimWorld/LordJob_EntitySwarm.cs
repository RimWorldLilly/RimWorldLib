using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C6A RID: 3178
	public abstract class LordJob_EntitySwarm : LordJob
	{
		// Token: 0x06005A22 RID: 23074 RVA: 0x002127AF File Offset: 0x002109AF
		public LordJob_EntitySwarm()
		{
		}

		// Token: 0x06005A23 RID: 23075 RVA: 0x002127B7 File Offset: 0x002109B7
		public LordJob_EntitySwarm(IntVec3 startPos, IntVec3 destPos)
		{
			this.startPos = startPos;
			this.destPos = destPos;
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x002127D0 File Offset: 0x002109D0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil lordToil = this.CreateTravelingToil(this.startPos, this.destPos);
			stateGraph.StartingToil = lordToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			stateGraph.AddTransition(new Transition(lordToil, lordToil_ExitMap, false, true)
			{
				triggers = 
				{
					new Trigger_Memo("TravelArrived")
				}
			}, false);
			return stateGraph;
		}

		// Token: 0x06005A25 RID: 23077
		protected abstract LordToil CreateTravelingToil(IntVec3 start, IntVec3 dest);

		// Token: 0x06005A26 RID: 23078 RVA: 0x00212833 File Offset: 0x00210A33
		public override bool DutyActiveWhenDown(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x00212838 File Offset: 0x00210A38
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.startPos, "startPos", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.destPos, "destPos", default(IntVec3), false);
		}

		// Token: 0x040033AA RID: 13226
		private IntVec3 startPos;

		// Token: 0x040033AB RID: 13227
		private IntVec3 destPos;
	}
}
