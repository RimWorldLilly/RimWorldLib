using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A3C RID: 6716
	public class CompUseEffect_GiveQuest : CompUseEffect
	{
		// Token: 0x17001D8B RID: 7563
		// (get) Token: 0x0600A871 RID: 43121 RVA: 0x003BD7B1 File Offset: 0x003BB9B1
		public CompProperties_UseEffectGiveQuest Props
		{
			get
			{
				return (CompProperties_UseEffectGiveQuest)this.props;
			}
		}

		// Token: 0x0600A872 RID: 43122 RVA: 0x003BD7C0 File Offset: 0x003BB9C0
		public override void DoEffect(Pawn user)
		{
			if (this.Props.skipIfMechlinkAlreadySentMechs)
			{
				Mechlink mechlink = this.parent as Mechlink;
				if (mechlink != null && mechlink.sentMechsToPlayer)
				{
					return;
				}
			}
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(user.Map), false);
			slate.Set<Pawn>("asker", user, false);
			slate.Set<Map>("map", user.Map, false);
			if (this.Props.discovered != null)
			{
				slate.Set<bool>("discovered", this.Props.discovered.Value, false);
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(this.Props.quest, slate);
			if (!quest.hidden && quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
		}
	}
}
