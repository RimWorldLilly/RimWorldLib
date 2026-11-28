using System;

namespace Verse.AI.Group
{
	// Token: 0x0200093B RID: 2363
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x060048B3 RID: 18611 RVA: 0x001C7F40 File Offset: 0x001C6140
		public override bool IsCaravanSendable
		{
			get
			{
				return this.isCaravanSendable;
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x060048B4 RID: 18612 RVA: 0x001C7F48 File Offset: 0x001C6148
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x001C7F50 File Offset: 0x001C6150
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x001C7F58 File Offset: 0x001C6158
		public LordJob_DefendPoint(IntVec3 point, float? wanderRadius = null, float? defendRadius = null, bool isCaravanSendable = false, bool addFleeToil = true)
		{
			this.point = point;
			this.wanderRadius = wanderRadius;
			this.defendRadius = defendRadius;
			this.isCaravanSendable = isCaravanSendable;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x001C7F88 File Offset: 0x001C6188
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			IntVec3 defendPoint = this.point;
			float? num = this.wanderRadius;
			stateGraph.AddToil(new LordToil_DefendPoint(defendPoint, this.defendRadius, num));
			return stateGraph;
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x001C7FBC File Offset: 0x001C61BC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<float?>(ref this.defendRadius, "defendRadius", null, false);
			Scribe_Values.Look<bool>(ref this.isCaravanSendable, "isCaravanSendable", false, false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04002ED3 RID: 11987
		private IntVec3 point;

		// Token: 0x04002ED4 RID: 11988
		private float? wanderRadius;

		// Token: 0x04002ED5 RID: 11989
		private float? defendRadius;

		// Token: 0x04002ED6 RID: 11990
		private bool isCaravanSendable;

		// Token: 0x04002ED7 RID: 11991
		private bool addFleeToil;
	}
}
