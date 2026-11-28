using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002217 RID: 8727
	public class QuestNode_GiveRoyalFavorAndDevelopmentPoints : QuestNode_GiveRoyalFavor
	{
		// Token: 0x0600D875 RID: 55413 RVA: 0x00503DCC File Offset: 0x00501FCC
		protected override void PostProcessRewardChoice(QuestPart_Choice rewardChoice)
		{
			if (ModsConfig.IdeologyActive && Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				for (int i = 0; i < rewardChoice.choices.Count; i++)
				{
					rewardChoice.choices[i].rewards.Add(new Reward_DevelopmentPoints(QuestGen.quest));
				}
			}
		}
	}
}
