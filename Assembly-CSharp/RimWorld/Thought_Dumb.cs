using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D3 RID: 5587
	public class Thought_Dumb : Thought
	{
		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x06008AD4 RID: 35540 RVA: 0x00325D30 File Offset: 0x00323F30
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x06008AD5 RID: 35541 RVA: 0x00325D38 File Offset: 0x00323F38
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x06008AD6 RID: 35542 RVA: 0x00325D41 File Offset: 0x00323F41
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x04005577 RID: 21879
		private int forcedStage;
	}
}
