using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C91 RID: 3217
	public class LordJob_WaitForEscort : LordJob
	{
		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06005B12 RID: 23314 RVA: 0x002183D3 File Offset: 0x002165D3
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x06005B13 RID: 23315 RVA: 0x002183DB File Offset: 0x002165DB
		public LordJob_WaitForEscort()
		{
		}

		// Token: 0x06005B14 RID: 23316 RVA: 0x002183EA File Offset: 0x002165EA
		public LordJob_WaitForEscort(IntVec3 point, bool addFleeToil = true)
		{
			this.point = point;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x06005B15 RID: 23317 RVA: 0x00218408 File Offset: 0x00216608
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckRoyalty("Shuttle crash rescue"))
			{
				return stateGraph;
			}
			LordToil_WanderClose lordToil_WanderClose = new LordToil_WanderClose(this.point);
			stateGraph.AddToil(lordToil_WanderClose);
			stateGraph.StartingToil = lordToil_WanderClose;
			return stateGraph;
		}

		// Token: 0x06005B16 RID: 23318 RVA: 0x00218444 File Offset: 0x00216644
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04003435 RID: 13365
		public IntVec3 point;

		// Token: 0x04003436 RID: 13366
		private bool addFleeToil = true;
	}
}
