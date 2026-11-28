using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC2 RID: 7106
	public class Alert_QuestExpiresSoon : Alert
	{
		// Token: 0x170020F2 RID: 8434
		// (get) Token: 0x0600B62F RID: 46639 RVA: 0x00402210 File Offset: 0x00400410
		private Quest QuestExpiring
		{
			get
			{
				int num = int.MaxValue;
				this.questExpiring = null;
				foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
				{
					if (!quest.dismissed && !quest.Historical && !quest.initiallyAccepted && quest.State == QuestState.NotYetAccepted && quest.TicksUntilExpiry > 0 && quest.TicksUntilExpiry < 60000 && quest.TicksUntilExpiry < num)
					{
						this.questExpiring = quest;
						num = quest.TicksUntilExpiry;
					}
				}
				return this.questExpiring;
			}
		}

		// Token: 0x0600B630 RID: 46640 RVA: 0x004022C0 File Offset: 0x004004C0
		public Alert_QuestExpiresSoon()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B631 RID: 46641 RVA: 0x004022CF File Offset: 0x004004CF
		protected override void OnClick()
		{
			if (this.questExpiring != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.questExpiring);
			}
		}

		// Token: 0x0600B632 RID: 46642 RVA: 0x00402303 File Offset: 0x00400503
		public override string GetLabel()
		{
			if (this.questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoon".Translate(this.questExpiring.TicksUntilExpiry.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x0600B633 RID: 46643 RVA: 0x0040233C File Offset: 0x0040053C
		public override TaggedString GetExplanation()
		{
			if (this.questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoonDesc".Translate(this.questExpiring.name, this.questExpiring.TicksUntilExpiry.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor));
		}

		// Token: 0x0600B634 RID: 46644 RVA: 0x00402397 File Offset: 0x00400597
		public override AlertReport GetReport()
		{
			return this.QuestExpiring != null;
		}

		// Token: 0x040068AA RID: 26794
		private Quest questExpiring;

		// Token: 0x040068AB RID: 26795
		private const int TicksToAlert = 60000;
	}
}
