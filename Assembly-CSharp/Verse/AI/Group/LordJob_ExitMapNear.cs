using System;

namespace Verse.AI.Group
{
	// Token: 0x0200093D RID: 2365
	public class LordJob_ExitMapNear : LordJob
	{
		// Token: 0x060048BD RID: 18621 RVA: 0x001C813F File Offset: 0x001C633F
		public LordJob_ExitMapNear()
		{
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x001C814E File Offset: 0x001C634E
		public LordJob_ExitMapNear(IntVec3 near, LocomotionUrgency locomotion, float radius = 12f, bool canDig = false, bool useAvoidGridSmart = false)
		{
			this.near = near;
			this.locomotion = locomotion;
			this.radius = radius;
			this.canDig = canDig;
			this.useAvoidGridSmart = useAvoidGridSmart;
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x001C8184 File Offset: 0x001C6384
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMapNear lordToil_ExitMapNear = new LordToil_ExitMapNear(this.near, this.radius, this.locomotion, this.canDig);
			if (this.useAvoidGridSmart)
			{
				lordToil_ExitMapNear.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil_ExitMapNear);
			return stateGraph;
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x001C81CC File Offset: 0x001C63CC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.near, "near", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
		}

		// Token: 0x04002EDB RID: 11995
		private IntVec3 near;

		// Token: 0x04002EDC RID: 11996
		private float radius;

		// Token: 0x04002EDD RID: 11997
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002EDE RID: 11998
		private bool canDig;

		// Token: 0x04002EDF RID: 11999
		private bool useAvoidGridSmart;

		// Token: 0x04002EE0 RID: 12000
		public const float DefaultRadius = 12f;
	}
}
