using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D3 RID: 8659
	public class QuestNode_GetPopIntentForQuest : QuestNode
	{
		// Token: 0x0600D77D RID: 55165 RVA: 0x004FFD42 File Offset: 0x004FDF42
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D77E RID: 55166 RVA: 0x004FFD4C File Offset: 0x004FDF4C
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D77F RID: 55167 RVA: 0x004FFD5C File Offset: 0x004FDF5C
		private void SetVars(Slate slate)
		{
			float populationIntentForQuest = StorytellerUtilityPopulation.PopulationIntentForQuest;
			slate.Set<float>(this.storeAs.GetValue(slate), populationIntentForQuest, false);
		}

		// Token: 0x040088E6 RID: 35046
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
