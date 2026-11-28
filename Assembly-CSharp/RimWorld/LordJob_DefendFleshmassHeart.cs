using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C68 RID: 3176
	public class LordJob_DefendFleshmassHeart : LordJob
	{
		// Token: 0x06005A1C RID: 23068 RVA: 0x00212745 File Offset: 0x00210945
		public LordJob_DefendFleshmassHeart()
		{
		}

		// Token: 0x06005A1D RID: 23069 RVA: 0x0021274D File Offset: 0x0021094D
		public LordJob_DefendFleshmassHeart(Building_FleshmassHeart heart)
		{
			this.heart = heart;
		}

		// Token: 0x06005A1E RID: 23070 RVA: 0x0021275C File Offset: 0x0021095C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendFleshmassHeart(this.heart));
			return stateGraph;
		}

		// Token: 0x06005A1F RID: 23071 RVA: 0x00212774 File Offset: 0x00210974
		public override void ExposeData()
		{
			Scribe_References.Look<Building_FleshmassHeart>(ref this.heart, "heart", false);
		}

		// Token: 0x040033A9 RID: 13225
		private Building_FleshmassHeart heart;
	}
}
