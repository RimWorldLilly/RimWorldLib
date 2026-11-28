using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021AF RID: 8623
	public class QuestNode_GeneratePawnRandDevelopmentStage : QuestNode_GeneratePawn
	{
		// Token: 0x0600D6E8 RID: 55016 RVA: 0x004FC74C File Offset: 0x004FA94C
		protected override DevelopmentalStage GetDevelopmentalStage(Slate slate)
		{
			if (!Find.Storyteller.difficulty.ChildrenAllowed)
			{
				return DevelopmentalStage.Adult;
			}
			float value = this.childChance.GetValue(slate);
			float value2 = this.adultChance.GetValue(slate);
			float num = value / (value + value2);
			if (Rand.Value > num)
			{
				return DevelopmentalStage.Adult;
			}
			return DevelopmentalStage.Child;
		}

		// Token: 0x04008836 RID: 34870
		public SlateRef<float> childChance;

		// Token: 0x04008837 RID: 34871
		public SlateRef<float> adultChance;
	}
}
