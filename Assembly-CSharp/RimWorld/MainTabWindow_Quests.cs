using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D9F RID: 7583
	[StaticConstructorOnStartup]
	public class MainTabWindow_Quests : MainTabWindow
	{
		// Token: 0x1700249D RID: 9373
		// (get) Token: 0x0600C4D5 RID: 50389 RVA: 0x00480D83 File Offset: 0x0047EF83
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 640f);
			}
		}

		// Token: 0x0600C4D6 RID: 50390 RVA: 0x00480D94 File Offset: 0x0047EF94
		public override void PreOpen()
		{
			base.PreOpen();
			this.quickSearchWidget.Reset();
			if (MainTabWindow_Quests.RatingIcon == null)
			{
				MainTabWindow_Quests.RatingIcon = ContentFinder<Texture2D>.Get("UI/Icons/ChallengeRatingIcon", true);
			}
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("AvailableQuests".Translate(), delegate()
			{
				this.curTab = MainTabWindow_Quests.QuestsTab.Available;
				this.selected = null;
			}, () => this.curTab == MainTabWindow_Quests.QuestsTab.Available));
			this.tabs.Add(new TabRecord("ActiveQuests".Translate(), delegate()
			{
				this.curTab = MainTabWindow_Quests.QuestsTab.Active;
				this.selected = null;
			}, () => this.curTab == MainTabWindow_Quests.QuestsTab.Active));
			this.tabs.Add(new TabRecord("HistoricalQuests".Translate(), delegate()
			{
				this.curTab = MainTabWindow_Quests.QuestsTab.Historical;
				this.selected = null;
			}, () => this.curTab == MainTabWindow_Quests.QuestsTab.Historical));
			this.Select(this.selected);
		}

		// Token: 0x0600C4D7 RID: 50391 RVA: 0x00480E8C File Offset: 0x0047F08C
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMin += 4f;
			rect2.xMax = rect2.width * 0.36f;
			rect2.yMax -= this.DoRewardsPrefsButton(rect2).height + 4f;
			this.DoQuestsList(rect2);
			Rect rect3 = rect;
			rect3.yMin += 4f;
			rect3.xMin = rect2.xMax + 17f;
			this.DoSelectedQuestInfo(rect3);
		}

		// Token: 0x0600C4D8 RID: 50392 RVA: 0x00480F1C File Offset: 0x0047F11C
		public Rect DoRewardsPrefsButton(Rect rect)
		{
			rect.yMin = rect.yMax - 40f;
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(rect, "ChooseRewards".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_RewardPrefsConfig());
			}
			return rect;
		}

		// Token: 0x0600C4D9 RID: 50393 RVA: 0x00480F78 File Offset: 0x0047F178
		public void Select(Quest quest)
		{
			if (quest != this.selected)
			{
				this.selected = quest;
				this.selectedQuestScrollPosition = default(Vector2);
				this.selectedQuestLastHeight = 300f;
			}
			if (quest != null)
			{
				if (quest.dismissed)
				{
					this.curTab = MainTabWindow_Quests.QuestsTab.Historical;
					return;
				}
				if (quest.State == QuestState.NotYetAccepted)
				{
					this.curTab = MainTabWindow_Quests.QuestsTab.Available;
					return;
				}
				if (quest.State == QuestState.Ongoing)
				{
					this.curTab = MainTabWindow_Quests.QuestsTab.Active;
					return;
				}
				this.curTab = MainTabWindow_Quests.QuestsTab.Historical;
			}
		}

		// Token: 0x0600C4DA RID: 50394 RVA: 0x00480FE8 File Offset: 0x0047F1E8
		private void DoQuestsList(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMin += 32f;
			Rect rect3 = new Rect(0f, rect2.yMax - 24f, rect2.width, 24f);
			this.quickSearchWidget.OnGUI(rect3, null, null);
			rect2.yMax -= 28f;
			Widgets.DrawMenuSection(rect2);
			TabDrawer.DrawTabs<TabRecord>(rect2, this.tabs, 200f);
			if (DebugSettings.godMode)
			{
				this.showAll = true;
			}
			else if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(rect.width - 135f, rect2.yMax - 24f, 120f, 24f), "DEV: Show all", ref this.showAll, false, null, null, false, false);
			}
			else
			{
				this.showAll = false;
			}
			this.SortQuestsByTab();
			if (MainTabWindow_Quests.tmpQuestsToShow.Count != 0)
			{
				MainTabWindow_Quests.<>c__DisplayClass49_0 CS$<>8__locals1;
				CS$<>8__locals1.scrollOutRect = rect2;
				CS$<>8__locals1.scrollOutRect = CS$<>8__locals1.scrollOutRect.ContractedBy(10f);
				CS$<>8__locals1.scrollOutRect.xMax = CS$<>8__locals1.scrollOutRect.xMax + 6f;
				CS$<>8__locals1.scrollViewRect = new Rect(0f, 0f, CS$<>8__locals1.scrollOutRect.width - 16f, (float)MainTabWindow_Quests.tmpQuestsToShow.Count * 32f);
				CS$<>8__locals1.scrollPosition = default(Vector2);
				switch (this.curTab)
				{
				case MainTabWindow_Quests.QuestsTab.Available:
					Widgets.BeginScrollView(CS$<>8__locals1.scrollOutRect, ref this.scrollPosition_available, CS$<>8__locals1.scrollViewRect, true);
					CS$<>8__locals1.scrollPosition = this.scrollPosition_available;
					break;
				case MainTabWindow_Quests.QuestsTab.Active:
					Widgets.BeginScrollView(CS$<>8__locals1.scrollOutRect, ref this.scrollPosition_active, CS$<>8__locals1.scrollViewRect, true);
					CS$<>8__locals1.scrollPosition = this.scrollPosition_active;
					break;
				case MainTabWindow_Quests.QuestsTab.Historical:
					Widgets.BeginScrollView(CS$<>8__locals1.scrollOutRect, ref this.scrollPosition_historical, CS$<>8__locals1.scrollViewRect, true);
					CS$<>8__locals1.scrollPosition = this.scrollPosition_historical;
					break;
				}
				CS$<>8__locals1.curY = 0f;
				foreach (Quest quest in MainTabWindow_Quests.tmpQuestsToShow)
				{
					this.<DoQuestsList>g__DrawQuest|49_0(quest, 0, ref CS$<>8__locals1);
				}
				MainTabWindow_Quests.tmpQuestsVisited.Clear();
				MainTabWindow_Quests.tmpQuestsToShow.Clear();
				Widgets.EndScrollView();
				return;
			}
			Widgets.NoneLabel(rect2.y + 17f, rect2.width, null);
		}

		// Token: 0x0600C4DB RID: 50395 RVA: 0x00481274 File Offset: 0x0047F474
		private void SortQuestsByTab()
		{
			List<Quest> questsInDisplayOrder = Find.QuestManager.questsInDisplayOrder;
			MainTabWindow_Quests.tmpQuestsToShow.Clear();
			for (int i = 0; i < questsInDisplayOrder.Count; i++)
			{
				if (this.ShouldListNow(questsInDisplayOrder[i]) && this.quickSearchWidget.filter.Matches(questsInDisplayOrder[i].name))
				{
					MainTabWindow_Quests.tmpQuestsToShow.Add(questsInDisplayOrder[i]);
				}
			}
			switch (this.curTab)
			{
			case MainTabWindow_Quests.QuestsTab.Available:
				MainTabWindow_Quests.tmpQuestsToShow.SortBy((Quest q) => q.TicksUntilExpiry);
				return;
			case MainTabWindow_Quests.QuestsTab.Active:
				MainTabWindow_Quests.tmpQuestsToShow.SortBy((Quest q) => q.TicksSinceAccepted);
				return;
			case MainTabWindow_Quests.QuestsTab.Historical:
				MainTabWindow_Quests.tmpQuestsToShow.SortBy((Quest q) => q.TicksSinceCleanup);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600C4DC RID: 50396 RVA: 0x0048137C File Offset: 0x0047F57C
		private void DoRow(Rect rect, Quest quest)
		{
			Rect rect2 = rect;
			rect2.width -= 95f;
			Rect rect3 = rect;
			rect3.xMax -= 4f;
			rect3.xMin = rect3.xMax - 35f;
			Rect rect4 = rect;
			rect4.xMax = rect3.xMin;
			rect4.xMin = rect4.xMax - 60f;
			if (quest.Historical)
			{
				Rect position = rect.ContractedBy(2f);
				QuestState state = quest.State;
				if (state != QuestState.EndedSuccess)
				{
					if (state != QuestState.EndedFailed)
					{
						Widgets.DrawRectFast(position, MainTabWindow_Quests.QuestExpiredColor, null);
					}
					else
					{
						Widgets.DrawRectFast(position, MainTabWindow_Quests.QuestFailedColor, null);
					}
				}
				else
				{
					Widgets.DrawRectFast(position, MainTabWindow_Quests.QuestCompletedColor, null);
				}
			}
			if (this.selected == quest)
			{
				Widgets.DrawHighlightSelected(rect);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect5 = new Rect(rect2.x + 4f, rect2.y, rect2.width - 4f, rect2.height);
			Widgets.Label(rect5, quest.name.Truncate(rect5.width, null));
			string timeTip;
			Color color;
			string shortTimeInfo = this.GetShortTimeInfo(quest, out timeTip, out color);
			if (!shortTimeInfo.NullOrEmpty())
			{
				GUI.color = color;
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect3, shortTimeInfo);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, () => quest.name + (timeTip.NullOrEmpty() ? "" : ("\n" + timeTip)), quest.id ^ 875632098);
					Widgets.DrawHighlight(rect3);
				}
			}
			if (quest.dismissed && !quest.Historical)
			{
				rect4.x -= 25f;
				Rect rect6 = new Rect(rect4.xMax + 5f, rect4.y + rect4.height / 2f - 7f, 15f, 15f);
				GUI.DrawTexture(rect6, MainTabWindow_Quests.QuestDismissedIcon);
				rect6.height = rect5.height;
				rect6.y = rect5.y;
				if (Mouse.IsOver(rect6))
				{
					TooltipHandler.TipRegion(rect6, "QuestDismissed".Translate());
					Widgets.DrawHighlight(rect6);
				}
			}
			if (ModsConfig.IdeologyActive && quest.charity && !quest.Historical && !quest.dismissed)
			{
				Rect rect7 = new Rect(rect4.x - 15f, rect4.y + rect4.height / 2f - 7f, 15f, 15f);
				GUI.DrawTexture(rect7, MainTabWindow_Quests.CharityQuestIcon);
				rect7.height = rect5.height;
				rect7.y = rect5.y;
				if (Mouse.IsOver(rect7))
				{
					TooltipHandler.TipRegion(rect7, "CharityQuestTip".Translate());
					Widgets.DrawHighlight(rect7);
				}
			}
			int num = Mathf.Max(quest.challengeRating, 1);
			for (int i = 0; i < num; i++)
			{
				GUI.DrawTexture(new Rect(rect4.xMax - (float)(15 * (i + 1)), rect4.y + rect4.height / 2f - 7f, 15f, 15f), MainTabWindow_Quests.RatingIcon);
			}
			if (Mouse.IsOver(rect4))
			{
				TooltipHandler.TipRegion(rect4, "QuestChallengeRatingTip".Translate());
				Widgets.DrawHighlight(rect4);
			}
			GenUI.ResetLabelAlign();
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.Select(quest);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600C4DD RID: 50397 RVA: 0x00481758 File Offset: 0x0047F958
		private string GetShortTimeInfo(Quest quest, out string tip, out Color color)
		{
			color = Color.gray;
			if (quest.State == QuestState.NotYetAccepted)
			{
				if (quest.TicksUntilExpiry >= 0)
				{
					color = ColorLibrary.RedReadable;
					tip = "QuestExpiresIn".Translate(quest.TicksUntilExpiry.ToStringTicksToPeriod(true, false, true, true, false));
					return quest.TicksUntilExpiry.ToStringTicksToPeriod(true, true, true, true, false);
				}
			}
			else
			{
				if (quest.Historical)
				{
					tip = "QuestFinishedAgo".Translate(quest.TicksSinceCleanup.ToStringTicksToPeriod(true, false, true, true, false));
					return quest.TicksSinceCleanup.ToStringTicksToPeriod(false, true, true, true, false);
				}
				if (quest.EverAccepted)
				{
					foreach (QuestPart questPart in quest.PartsListForReading)
					{
						QuestPart_Delay questPart_Delay = questPart as QuestPart_Delay;
						if (questPart_Delay != null && questPart_Delay.State == QuestPartState.Enabled && questPart_Delay.isBad && !questPart_Delay.expiryInfoPart.NullOrEmpty())
						{
							color = ColorLibrary.RedReadable;
							tip = "QuestExpiresIn".Translate(questPart_Delay.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
							return questPart_Delay.TicksLeft.ToStringTicksToPeriod(false, true, false, true, false);
						}
					}
					tip = this.GetAcceptedAgoByString(quest);
					return quest.TicksSinceAccepted.ToStringTicksToPeriod(false, true, true, true, false);
				}
			}
			tip = null;
			return null;
		}

		// Token: 0x0600C4DE RID: 50398 RVA: 0x004818DC File Offset: 0x0047FADC
		private void DoSelectedQuestInfo(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			if (this.selected == null)
			{
				Widgets.NoneLabelCenteredVertically(rect, "(" + "NoQuestSelected".Translate() + ")");
				return;
			}
			Rect rect2 = rect.ContractedBy(17f);
			Rect outRect = rect2;
			Rect innerRect = new Rect(0f, 0f, outRect.width, this.selectedQuestLastHeight);
			Rect rect3 = new Rect(0f, 0f, outRect.width - 16f, this.selectedQuestLastHeight);
			Rect rect4 = rect3;
			bool flag = rect3.height > rect2.height;
			if (flag)
			{
				rect3.width -= 4f;
				rect4.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref this.selectedQuestScrollPosition, rect3, true);
			float num = 0f;
			this.DoTitle(rect3, ref num);
			this.DoDismissButton(rect3, ref num);
			this.DoCharityIcon(rect3);
			if (this.selected != null)
			{
				float curYBeforeAcceptButton = num;
				this.DoAcceptButton(rect3, ref num);
				this.DoRightAlignedInfo(rect3, ref num, curYBeforeAcceptButton);
				this.DoOutcomeInfo(rect3, ref num);
				this.DoDescription(rect3, ref num);
				this.DoAcceptanceRequirementInfo(innerRect, flag, ref num);
				this.DoIdeoCharityInfo(innerRect, flag, ref num);
				this.DoRewards(rect3, ref num);
				this.DoLookTargets(rect3, ref num);
				this.DoSelectTargets(rect3, ref num);
				float num2 = num;
				this.DoDefHyperlinks(rect3, ref num);
				float num3 = num;
				num = num2;
				if (this.selected.root != null && !this.selected.root.hideInvolvedFactionsInfo)
				{
					this.DoFactionInfo(rect4, ref num);
				}
				this.DoDebugInfoToggle(rect3, ref num);
				if (num3 > num)
				{
					num = num3;
				}
				this.DoDebugInfo(rect3, ref num);
				this.selectedQuestLastHeight = num;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600C4DF RID: 50399 RVA: 0x00481AA8 File Offset: 0x0047FCA8
		private void DoTitle(Rect innerRect, ref float curY)
		{
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(innerRect.x, curY, innerRect.width, 100f);
			Widgets.Label(rect, this.selected.name.Truncate(rect.width, null));
			Text.Font = GameFont.Small;
			curY += Text.LineHeight;
			curY += 17f;
		}

		// Token: 0x0600C4E0 RID: 50400 RVA: 0x00481B10 File Offset: 0x0047FD10
		private void DoDismissButton(Rect innerRect, ref float curY)
		{
			Rect rect = new Rect(innerRect.xMax - 32f - 4f, innerRect.y, 32f, 32f);
			Texture2D tex = (!this.selected.Historical && this.selected.dismissed) ? MainTabWindow_Quests.ResumeQuestIcon : MainTabWindow_Quests.DismissIcon;
			if (Widgets.ButtonImage(rect, tex, true, null))
			{
				if (this.selected.Historical)
				{
					this.selected.hiddenInUI = true;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.Select(null);
					return;
				}
				this.selected.dismissed = !this.selected.dismissed;
				foreach (Quest quest in this.selected.GetSubquests(null))
				{
					quest.dismissed = this.selected.dismissed;
				}
				if (this.selected.dismissed)
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					this.SortQuestsByTab();
					this.selected = MainTabWindow_Quests.tmpQuestsToShow.FirstOrDefault((Quest x) => this.ShouldListNow(x));
					MainTabWindow_Quests.tmpQuestsToShow.Clear();
					return;
				}
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				this.Select(this.selected);
			}
			if (Mouse.IsOver(rect))
			{
				string key = this.selected.Historical ? "DeleteQuest" : (this.selected.dismissed ? "UnDismissQuest" : "DismissQuest");
				TooltipHandler.TipRegion(rect, key.Translate());
			}
		}

		// Token: 0x0600C4E1 RID: 50401 RVA: 0x00481CC0 File Offset: 0x0047FEC0
		private void DoCharityIcon(Rect innerRect)
		{
			if (this.selected != null && this.selected.charity && ModsConfig.IdeologyActive)
			{
				Rect rect = new Rect(innerRect.xMax - 32f - 26f - 32f - 4f, innerRect.y, 32f, 32f);
				GUI.DrawTexture(rect, MainTabWindow_Quests.CharityQuestIcon);
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, "CharityQuestTip".Translate());
				}
			}
		}

		// Token: 0x0600C4E2 RID: 50402 RVA: 0x00481D48 File Offset: 0x0047FF48
		private void DoDebugInfoToggle(Rect innerRect, ref float curY)
		{
			if (!Prefs.DevMode)
			{
				this.showDebugInfo = false;
				return;
			}
			if (DebugSettings.godMode)
			{
				this.showDebugInfo = true;
				return;
			}
			Widgets.CheckboxLabeled(new Rect(innerRect.xMax - 110f, curY, 110f, 30f), "DEV: Show debug info", ref this.showDebugInfo, false, null, null, false, false);
			curY += 30f;
		}

		// Token: 0x0600C4E3 RID: 50403 RVA: 0x00481DB0 File Offset: 0x0047FFB0
		private void DoAcceptButton(Rect innerRect, ref float curY)
		{
			QuestPart_Choice questPart_Choice = null;
			List<QuestPart> partsListForReading = this.selected.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				questPart_Choice = (partsListForReading[i] as QuestPart_Choice);
				if (questPart_Choice != null)
				{
					break;
				}
			}
			if (questPart_Choice != null && !Prefs.DevMode)
			{
				return;
			}
			curY += 17f;
			if (this.selected.State == QuestState.NotYetAccepted)
			{
				float num = innerRect.x;
				if (questPart_Choice == null)
				{
					Rect rect = new Rect(num, curY, 180f, 40f);
					AcceptanceReport acceptanceReport = QuestUtility.CanAcceptQuest(this.selected);
					if (!acceptanceReport.Accepted)
					{
						GUI.color = Color.grey;
						TooltipHandler.TipRegion(rect, acceptanceReport.Reason.Colorize(ColoredText.WarningColor));
					}
					if (Widgets.ButtonText(rect, "AcceptQuest".Translate(), true, true, true, null))
					{
						this.AcceptQuestByInterface(null, this.selected.RequiresAccepter);
					}
					num += rect.width + 10f;
					GUI.color = Color.white;
				}
				if (Prefs.DevMode && Widgets.ButtonText(new Rect(num, curY, 180f, 40f), "DEV: Accept instantly", true, true, true, null))
				{
					SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
					if (questPart_Choice != null && questPart_Choice.choices.Any<QuestPart_Choice.Choice>())
					{
						questPart_Choice.Choose(questPart_Choice.choices.RandomElement<QuestPart_Choice.Choice>());
					}
					this.selected.Accept((from p in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended
					where QuestUtility.CanPawnAcceptQuest(p, this.selected)
					select p).RandomElementWithFallback(null));
					this.selected.dismissed = false;
					this.Select(this.selected);
				}
				curY += 44f;
			}
		}

		// Token: 0x0600C4E4 RID: 50404 RVA: 0x00481F6C File Offset: 0x0048016C
		private void DoRightAlignedInfo(Rect innerRect, ref float curY, float curYBeforeAcceptButton)
		{
			bool flag = false;
			Vector2 locForDates = QuestUtility.GetLocForDates();
			float num = curYBeforeAcceptButton;
			if (!this.selected.initiallyAccepted && this.selected.EverAccepted)
			{
				if (!flag)
				{
					num += 17f;
					flag = true;
				}
				Rect rect = new Rect(innerRect.x, num, innerRect.width, 25f);
				GUI.color = MainTabWindow_Quests.TimeLimitColor;
				Text.Anchor = TextAnchor.MiddleRight;
				string text = this.selected.Historical ? this.GetAcceptedOnByString(this.selected) : this.GetAcceptedAgoByString(this.selected);
				Widgets.Label(rect, text);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				rect.xMin = rect.xMax - Text.CalcSize(text).x;
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, this.selected.Historical ? this.GetAcceptedAgoByString(this.selected) : this.GetAcceptedOnByString(this.selected));
				}
				if (this.selected.AccepterPawn != null && CameraJumper.CanJump(this.selected.AccepterPawn))
				{
					Widgets.DrawHighlightIfMouseover(rect);
					if (Widgets.ButtonInvisible(rect, true))
					{
						CameraJumper.TryJumpAndSelect(this.selected.AccepterPawn, CameraJumper.MovementMode.Pan);
						Find.MainTabsRoot.EscapeCurrentTab(true);
					}
				}
				num += Text.LineHeight;
			}
			else if (this.selected.Historical)
			{
				if (!flag)
				{
					num += 17f;
					flag = true;
				}
				Rect rect2 = new Rect(innerRect.x, num, innerRect.width, 25f);
				GUI.color = MainTabWindow_Quests.TimeLimitColor;
				Text.Anchor = TextAnchor.MiddleRight;
				TaggedString taggedString = "AppearedOn".Translate(GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(this.selected.appearanceTick), locForDates));
				Widgets.Label(rect2, taggedString);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				rect2.xMin = rect2.xMax - Text.CalcSize(taggedString).x;
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, "AppearedDaysAgo".Translate(((float)this.selected.TicksSinceAppeared / 60000f).ToString("0.#")));
				}
				num += Text.LineHeight;
			}
			if (this.selected.State == QuestState.NotYetAccepted && this.selected.TicksUntilExpiry > 0)
			{
				if (!flag)
				{
					num += 17f;
					flag = true;
				}
				Rect rect3 = new Rect(innerRect.x, num, innerRect.width, 25f);
				GUI.color = MainTabWindow_Quests.TimeLimitColor;
				Text.Anchor = TextAnchor.MiddleRight;
				string text2 = "QuestExpiresIn".Translate(this.selected.TicksUntilExpiry.ToStringTicksToPeriod(true, false, true, true, false));
				Widgets.Label(rect3, text2);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				rect3.xMin = rect3.xMax - Text.CalcSize(text2).x;
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, "QuestExpiresOn".Translate(GenDate.DateFullStringWithHourAt((long)(Find.TickManager.TicksAbs + this.selected.TicksUntilExpiry), locForDates)));
				}
				num += Text.LineHeight;
			}
			if (this.selected.State == QuestState.Ongoing)
			{
				this.tmpQuestParts.Clear();
				this.tmpQuestParts.AddRange(this.selected.PartsListForReading);
				this.tmpQuestParts.SortBy(delegate(QuestPart x)
				{
					if (!(x is QuestPartActivable))
					{
						return 0;
					}
					return ((QuestPartActivable)x).EnableTick;
				});
				for (int i = 0; i < this.tmpQuestParts.Count; i++)
				{
					QuestPartActivable questPartActivable = this.tmpQuestParts[i] as QuestPartActivable;
					if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
					{
						string expiryInfoPart = questPartActivable.ExpiryInfoPart;
						if (!expiryInfoPart.NullOrEmpty())
						{
							if (!flag)
							{
								num += 17f;
								flag = true;
							}
							Rect rect4 = new Rect(innerRect.x, num, innerRect.width, 25f);
							GUI.color = MainTabWindow_Quests.TimeLimitColor;
							Text.Anchor = TextAnchor.MiddleRight;
							Widgets.Label(rect4, expiryInfoPart);
							GUI.color = Color.white;
							Text.Anchor = TextAnchor.UpperLeft;
							rect4.xMin = rect4.xMax - Text.CalcSize(expiryInfoPart).x;
							if (Mouse.IsOver(rect4))
							{
								string expiryInfoPartTip = questPartActivable.ExpiryInfoPartTip;
								if (!expiryInfoPartTip.NullOrEmpty())
								{
									TooltipHandler.TipRegion(rect4, expiryInfoPartTip);
								}
							}
							num += Text.LineHeight;
						}
					}
				}
				this.tmpQuestParts.Clear();
			}
			curY = Mathf.Max(curY, num);
		}

		// Token: 0x0600C4E5 RID: 50405 RVA: 0x00482434 File Offset: 0x00480634
		private void DoAcceptanceRequirementInfo(Rect innerRect, bool scrollBarVisible, ref float curY)
		{
			if (this.selected.EverAccepted || this.selected.Historical)
			{
				return;
			}
			IEnumerable<string> enumerable = this.ListUnmetAcceptRequirements();
			int num = enumerable.Count<string>();
			if (num == 0)
			{
				return;
			}
			bool flag = num > 1;
			string text = "QuestAcceptanceRequirementsDescription".Translate() + (flag ? ": " : " ") + (flag ? ("\n" + enumerable.ToLineList("  - ", true)) : (enumerable.First<string>() + "."));
			curY += 17f;
			this.DrawInfoBox(innerRect, scrollBarVisible, ref curY, text, MainTabWindow_Quests.acceptanceRequirementsBoxBgColor, MainTabWindow_Quests.AcceptanceRequirementsBoxColor, MainTabWindow_Quests.AcceptanceRequirementsColor);
			new LookTargets(this.ListUnmetAcceptRequirementCulprits()).TryHighlight(true, true, true);
		}

		// Token: 0x0600C4E6 RID: 50406 RVA: 0x00482500 File Offset: 0x00480700
		private void DoIdeoCharityInfo(Rect innerRect, bool scrollBarVisible, ref float curY)
		{
			if (!this.selected.charity || !ModsConfig.IdeologyActive)
			{
				return;
			}
			List<Pawn> allMaps_FreeColonistsSpawned = PawnsFinder.AllMaps_FreeColonistsSpawned;
			List<Ideo> ideosListForReading = Find.IdeoManager.IdeosListForReading;
			string text = "";
			for (int i = 0; i < ideosListForReading.Count; i++)
			{
				Ideo ideo = ideosListForReading[i];
				List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
				bool flag = false;
				for (int j = 0; j < preceptsListForReading.Count; j++)
				{
					if (preceptsListForReading[j].def.issue == IssueDefOf.Charity)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					MainTabWindow_Quests.tmpColonistsForIdeo.Clear();
					for (int k = 0; k < allMaps_FreeColonistsSpawned.Count; k++)
					{
						Pawn pawn = allMaps_FreeColonistsSpawned[k];
						if (pawn != null && pawn.Ideo == ideo && !pawn.IsQuestReward(this.selected))
						{
							MainTabWindow_Quests.tmpColonistsForIdeo.Add(pawn);
						}
					}
					if (MainTabWindow_Quests.tmpColonistsForIdeo.Count != 0)
					{
						if (!text.NullOrEmpty())
						{
							text += "\n\n";
						}
						text += "IdeoCharityQuestInfo".Translate(ideo.name, GenThing.ThingsToCommaList(MainTabWindow_Quests.tmpColonistsForIdeo, false, true, -1));
					}
				}
			}
			if (text.NullOrEmpty())
			{
				return;
			}
			curY += 17f;
			this.DrawInfoBox(innerRect, scrollBarVisible, ref curY, text, MainTabWindow_Quests.IdeoCharityBoxBackgroundColor, MainTabWindow_Quests.IdeoCharityBoxBorderColor, MainTabWindow_Quests.IdeoCharityTextColor);
		}

		// Token: 0x0600C4E7 RID: 50407 RVA: 0x00482674 File Offset: 0x00480874
		private void DrawInfoBox(Rect innerRect, bool scrollBarVisible, ref float curY, string text, Color boxBackground, Color boxBorder, Color textColor)
		{
			float num = 0f;
			float x = innerRect.x + 8f;
			float num2 = innerRect.width - 16f;
			if (scrollBarVisible)
			{
				num2 -= 31f;
			}
			Rect rect = new Rect(x, curY, num2, 10000f);
			num += Text.CalcHeight(text, rect.width);
			Rect rect2 = new Rect(x, curY, num2, num).ExpandedBy(8f);
			Widgets.DrawBoxSolid(rect2, boxBackground);
			GUI.color = textColor;
			Widgets.Label(rect, text);
			GUI.color = boxBorder;
			Widgets.DrawBox(rect2, 2, null);
			curY += num;
			GUI.color = Color.white;
		}

		// Token: 0x0600C4E8 RID: 50408 RVA: 0x00482718 File Offset: 0x00480918
		private IEnumerable<string> ListUnmetAcceptRequirements()
		{
			MainTabWindow_Quests.<ListUnmetAcceptRequirements>d__65 <ListUnmetAcceptRequirements>d__ = new MainTabWindow_Quests.<ListUnmetAcceptRequirements>d__65(-2);
			<ListUnmetAcceptRequirements>d__.<>4__this = this;
			return <ListUnmetAcceptRequirements>d__;
		}

		// Token: 0x0600C4E9 RID: 50409 RVA: 0x00482728 File Offset: 0x00480928
		private IEnumerable<GlobalTargetInfo> ListUnmetAcceptRequirementCulprits()
		{
			MainTabWindow_Quests.<ListUnmetAcceptRequirementCulprits>d__66 <ListUnmetAcceptRequirementCulprits>d__ = new MainTabWindow_Quests.<ListUnmetAcceptRequirementCulprits>d__66(-2);
			<ListUnmetAcceptRequirementCulprits>d__.<>4__this = this;
			return <ListUnmetAcceptRequirementCulprits>d__;
		}

		// Token: 0x0600C4EA RID: 50410 RVA: 0x00482738 File Offset: 0x00480938
		private void DoOutcomeInfo(Rect innerRect, ref float curY)
		{
			if (!this.selected.Historical)
			{
				return;
			}
			string text;
			if (this.selected.State == QuestState.EndedOfferExpired)
			{
				text = "QuestOutcomeInfo_OfferExpired".Translate();
			}
			else if (this.selected.State == QuestState.EndedUnknownOutcome || this.selected.State == QuestState.EndedSuccess)
			{
				text = "QuestOutcomeInfo_UnknownOrSuccess".Translate();
			}
			else if (this.selected.State == QuestState.EndedFailed)
			{
				text = "QuestOutcomeInfo_Failed".Translate();
			}
			else if (this.selected.State == QuestState.EndedInvalid)
			{
				text = "QuestOutcomeInfo_Invalid".Translate();
			}
			else
			{
				text = null;
			}
			if (!text.NullOrEmpty())
			{
				curY += 17f;
				Widgets.Label(new Rect(innerRect.x, curY, innerRect.width, 25f), text);
				curY += Text.LineHeight;
			}
		}

		// Token: 0x0600C4EB RID: 50411 RVA: 0x00482820 File Offset: 0x00480A20
		private void DoDescription(Rect innerRect, ref float curY)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.selected.description.RawText.NullOrEmpty())
			{
				string value = this.selected.description.Resolve();
				stringBuilder.Append(value);
			}
			this.tmpQuestParts.Clear();
			this.tmpQuestParts.AddRange(this.selected.PartsListForReading);
			this.tmpQuestParts.SortBy(delegate(QuestPart x)
			{
				if (!(x is QuestPartActivable))
				{
					return 0;
				}
				return ((QuestPartActivable)x).EnableTick;
			});
			for (int i = 0; i < this.tmpQuestParts.Count; i++)
			{
				QuestPartActivable questPartActivable = this.tmpQuestParts[i] as QuestPartActivable;
				if (questPartActivable == null || questPartActivable.State == QuestPartState.Enabled)
				{
					string descriptionPart = this.tmpQuestParts[i].DescriptionPart;
					if (!descriptionPart.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(descriptionPart);
					}
				}
			}
			this.tmpQuestParts.Clear();
			if (stringBuilder.Length == 0)
			{
				return;
			}
			curY += 17f;
			Rect rect = new Rect(innerRect.x, curY, innerRect.width, 10000f);
			Widgets.Label(rect, stringBuilder.ToString());
			curY += Text.CalcHeight(stringBuilder.ToString(), rect.width);
		}

		// Token: 0x0600C4EC RID: 50412 RVA: 0x00482980 File Offset: 0x00480B80
		private void DoRewards(Rect innerRect, ref float curY)
		{
			QuestPart_Choice choice = null;
			List<QuestPart> partsListForReading = this.selected.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				choice = (partsListForReading[i] as QuestPart_Choice);
				if (choice != null)
				{
					break;
				}
			}
			if (choice == null)
			{
				return;
			}
			bool flag = this.selected.State == QuestState.NotYetAccepted;
			bool flag2 = true;
			for (int j = 0; j < choice.choices.Count; j++)
			{
				MainTabWindow_Quests.tmpStackElements.Clear();
				float num = 0f;
				for (int k = 0; k < choice.choices[j].rewards.Count; k++)
				{
					MainTabWindow_Quests.tmpStackElements.AddRange(choice.choices[j].rewards[k].StackElements);
					num += choice.choices[j].rewards[k].TotalMarketValue;
				}
				if (MainTabWindow_Quests.tmpStackElements.Any<GenUI.AnonymousStackElement>())
				{
					if (num > 0f)
					{
						if (choice.choices[j].rewards.Count != 1)
						{
							goto IL_195;
						}
						Reward_Items reward_Items = choice.choices[j].rewards[0] as Reward_Items;
						if (reward_Items == null || reward_Items.items == null || reward_Items.items.Count != 1)
						{
							goto IL_195;
						}
						bool flag3 = !(reward_Items.items[0].StyleSourcePrecept is Precept_Relic);
						IL_196:
						if (flag3)
						{
							TaggedString totalValueStr = "TotalValue".Translate(num.ToStringMoney("F0"));
							MainTabWindow_Quests.tmpStackElements.Add(new GenUI.AnonymousStackElement
							{
								drawer = delegate(Rect r)
								{
									GUI.color = new Color(0.7f, 0.7f, 0.7f);
									Widgets.Label(new Rect(r.x + 5f, r.y, r.width - 10f, r.height), totalValueStr);
									GUI.color = Color.white;
								},
								width = Text.CalcSize(totalValueStr).x + 10f
							});
							goto IL_205;
						}
						goto IL_205;
						IL_195:
						flag3 = true;
						goto IL_196;
					}
					IL_205:
					if (flag2)
					{
						curY += 17f;
						flag2 = false;
					}
					else
					{
						curY += 10f;
					}
					Rect rect = new Rect(innerRect.x, curY, innerRect.width, 10000f);
					Rect rect2 = rect.ContractedBy(10f);
					if (flag)
					{
						rect2.xMin += 100f;
					}
					rect.height = GenUI.DrawElementStack<GenUI.AnonymousStackElement>(rect2, 24f, MainTabWindow_Quests.tmpStackElements, null, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, false).height + 20f;
					Widgets.DrawBoxSolid(rect, new Color(0.13f, 0.13f, 0.13f));
					GUI.color = new Color(1f, 1f, 1f, 0.3f);
					Widgets.DrawHighlightIfMouseover(rect);
					GUI.color = Color.white;
					GenUI.DrawElementStack<GenUI.AnonymousStackElement>(rect2, 24f, MainTabWindow_Quests.tmpStackElements, delegate(Rect r, GenUI.AnonymousStackElement obj)
					{
						obj.drawer(r);
					}, (GenUI.AnonymousStackElement obj) => obj.width, 4f, 5f, false);
					if (flag)
					{
						AcceptanceReport report = QuestUtility.CanAcceptQuest(this.selected);
						if (!report)
						{
							GUI.color = Color.grey;
						}
						Rect rect3 = new Rect(rect.x, rect.y, 100f, rect.height);
						if (Widgets.ButtonText(rect3, "AcceptQuestFor".Translate() + ":", true, true, true, null))
						{
							MainTabWindow_Quests.tmpRemainingQuestParts.Clear();
							MainTabWindow_Quests.tmpRemainingQuestParts.AddRange(this.selected.PartsListForReading);
							for (int l = 0; l < choice.choices.Count; l++)
							{
								if (j != l)
								{
									for (int m = 0; m < choice.choices[l].questParts.Count; m++)
									{
										QuestPart item = choice.choices[l].questParts[m];
										if (!choice.choices[j].questParts.Contains(item))
										{
											MainTabWindow_Quests.tmpRemainingQuestParts.Remove(item);
										}
									}
								}
							}
							bool requiresAccepter = false;
							for (int n = 0; n < MainTabWindow_Quests.tmpRemainingQuestParts.Count; n++)
							{
								if (MainTabWindow_Quests.tmpRemainingQuestParts[n].RequiresAccepter)
								{
									requiresAccepter = true;
									break;
								}
							}
							MainTabWindow_Quests.tmpRemainingQuestParts.Clear();
							QuestPart_Choice.Choice localChoice = choice.choices[j];
							this.AcceptQuestByInterface(delegate
							{
								choice.Choose(localChoice);
							}, requiresAccepter);
						}
						TaggedString taggedString = "AcceptQuestForTip".Translate();
						if (!report && !report.Reason.NullOrEmpty())
						{
							taggedString += "\n\n" + report.Reason.Colorize(ColorLibrary.RedReadable);
						}
						TooltipHandler.TipRegion(rect3, taggedString);
						GUI.color = Color.white;
					}
					curY += rect.height;
				}
			}
			MainTabWindow_Quests.tmpStackElements.Clear();
		}

		// Token: 0x0600C4ED RID: 50413 RVA: 0x00482F50 File Offset: 0x00481150
		private void DoLookTargets(Rect innerRect, ref float curY)
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num++;
				}
			}
			MainTabWindow_Quests.tmpLookTargets.Clear();
			MainTabWindow_Quests.tmpLookTargets.AddRange(this.selected.QuestLookTargets);
			MainTabWindow_Quests.tmpLookTargets.SortBy(delegate(GlobalTargetInfo x)
			{
				if (x.Thing is Pawn)
				{
					return 0;
				}
				if (x.HasThing)
				{
					return 1;
				}
				if (!x.IsWorldTarget)
				{
					return 2;
				}
				if (!(x.WorldObject is Settlement) || ((Settlement)x.WorldObject).Faction != Faction.OfPlayer)
				{
					return 3;
				}
				return 4;
			}, (GlobalTargetInfo x) => x.Label);
			bool flag = false;
			for (int j = 0; j < MainTabWindow_Quests.tmpLookTargets.Count; j++)
			{
				GlobalTargetInfo globalTargetInfo = MainTabWindow_Quests.tmpLookTargets[j];
				if (globalTargetInfo.HasWorldObject)
				{
					MapParent mapParent = globalTargetInfo.WorldObject as MapParent;
					if (mapParent != null && (!mapParent.HasMap || !mapParent.Map.IsPlayerHome))
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = false;
			for (int k = 0; k < MainTabWindow_Quests.tmpLookTargets.Count; k++)
			{
				GlobalTargetInfo globalTargetInfo2 = MainTabWindow_Quests.tmpLookTargets[k];
				if (CameraJumper.CanJump(globalTargetInfo2))
				{
					Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
					if (num != 1 || anyPlayerHomeMap == null || !(globalTargetInfo2 == anyPlayerHomeMap.Parent) || flag)
					{
						if (!flag2)
						{
							flag2 = true;
							curY += 17f;
						}
						if (Widgets.ButtonText(new Rect(innerRect.x, curY, innerRect.width, 25f), "JumpToTargetCustom".Translate(globalTargetInfo2.Label), false, true, true, null))
						{
							CameraJumper.TryJumpAndSelect(globalTargetInfo2, CameraJumper.MovementMode.Pan);
							Find.MainTabsRoot.EscapeCurrentTab(true);
						}
						curY += 25f;
					}
				}
			}
		}

		// Token: 0x0600C4EE RID: 50414 RVA: 0x0048312C File Offset: 0x0048132C
		private void DoSelectTargets(Rect innerRect, ref float curY)
		{
			bool flag = false;
			for (int i = 0; i < this.selected.PartsListForReading.Count; i++)
			{
				QuestPart questPart = this.selected.PartsListForReading[i];
				MainTabWindow_Quests.tmpSelectTargets.Clear();
				MainTabWindow_Quests.tmpSelectTargets.AddRange(questPart.QuestSelectTargets);
				if (MainTabWindow_Quests.tmpSelectTargets.Count != 0)
				{
					if (!flag)
					{
						flag = true;
						curY += 4f;
					}
					if (Widgets.ButtonText(new Rect(innerRect.x, curY, innerRect.width, 25f), questPart.QuestSelectTargetsLabel, false, true, true, null))
					{
						Map map = null;
						int num = 0;
						Vector3 a = Vector3.zero;
						Find.Selector.ClearSelection();
						for (int j = 0; j < MainTabWindow_Quests.tmpSelectTargets.Count; j++)
						{
							GlobalTargetInfo target = MainTabWindow_Quests.tmpSelectTargets[j];
							if (CameraJumper.CanJump(target) && target.HasThing)
							{
								Find.Selector.Select(target.Thing, true, true);
								if (map == null)
								{
									map = target.Map;
								}
								else if (target.Map != map)
								{
									num = 0;
									break;
								}
								a += target.Cell.ToVector3();
								num++;
							}
						}
						if (num > 0)
						{
							CameraJumper.TryJump(new IntVec3(a / (float)num), map, CameraJumper.MovementMode.Pan);
						}
						Find.MainTabsRoot.EscapeCurrentTab(true);
					}
					curY += 25f;
				}
			}
		}

		// Token: 0x0600C4EF RID: 50415 RVA: 0x004832B4 File Offset: 0x004814B4
		private void DoFactionInfo(Rect rect, ref float curY)
		{
			curY += 15f;
			foreach (Faction faction in this.selected.InvolvedFactions)
			{
				if (faction != null && !faction.Hidden && !faction.IsPlayer)
				{
					FactionUIUtility.DrawRelatedFactionInfo(rect, faction, ref curY);
				}
			}
		}

		// Token: 0x0600C4F0 RID: 50416 RVA: 0x00483324 File Offset: 0x00481524
		private void DoDefHyperlinks(Rect rect, ref float curY)
		{
			curY += 25f;
			foreach (Dialog_InfoCard.Hyperlink hyperlink in this.selected.Hyperlinks)
			{
				float num = Text.CalcHeight(hyperlink.Label, rect.width);
				float width = rect.width / 2f;
				Rect rect2 = new Rect(rect.x, curY, width, num);
				Color value = Widgets.NormalOptionColor;
				if (hyperlink.quest != null && (hyperlink.quest.IsSubquestOf(this.selected) || this.selected.IsSubquestOf(hyperlink.quest)))
				{
					if (!this.selected.hidden && !hyperlink.quest.hidden)
					{
						string text = "";
						if (hyperlink.quest.Historical)
						{
							text += "(" + "Finished".Translate().ToLower() + ") ";
							value = Color.gray;
						}
						text += (hyperlink.quest.IsSubquestOf(this.selected) ? "HasSubquest".Translate() : "SubquestOf".Translate());
						text = text + ": " + hyperlink.Label;
						Widgets.HyperlinkWithIcon(rect2, hyperlink, text, 2f, 6f, new Color?(value), true, null);
					}
				}
				else
				{
					Widgets.HyperlinkWithIcon(rect2, hyperlink, "ViewHyperlink".Translate(hyperlink.Label), 2f, 6f, new Color?(value), false, null);
				}
				curY += num;
			}
		}

		// Token: 0x0600C4F1 RID: 50417 RVA: 0x00483514 File Offset: 0x00481714
		private void DoDebugInfo(Rect innerRect, ref float curY)
		{
			if (!this.showDebugInfo)
			{
				return;
			}
			curY += 17f;
			List<QuestPart> partsListForReading = this.selected.PartsListForReading;
			if (this.selected.State == QuestState.Ongoing)
			{
				for (int i = 0; i < partsListForReading.Count; i++)
				{
					partsListForReading[i].DoDebugWindowContents(innerRect, ref curY);
				}
			}
			if (this.selected.State == QuestState.Ongoing || this.selected.State == QuestState.NotYetAccepted)
			{
				Rect rect = new Rect(innerRect.x, curY, 210f, 25f);
				this.debugSendSignalTextField = Widgets.TextField(rect, this.debugSendSignalTextField);
				Rect rect2 = new Rect(innerRect.x + rect.width + 4f, curY, 117f, 25f);
				if (Widgets.ButtonText(rect2, "Send signal", true, true, true, null))
				{
					Find.SignalManager.SendSignal(new Signal(this.debugSendSignalTextField, false));
					this.debugSendSignalTextField = "";
				}
				if (Widgets.ButtonText(new Rect(rect2.xMax + 4f, curY, 165f, 25f), "Send defined signal...", true, true, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (string signalLocal2 in from x in this.DebugPossibleSignals(this.selected).Distinct<string>()
					orderby x
					select x)
					{
						string signalLocal = signalLocal2;
						list.Add(new FloatMenuOption(signalLocal, delegate()
						{
							Find.SignalManager.SendSignal(new Signal(signalLocal, false));
							this.debugSendSignalTextField = "";
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				curY += rect.height + 4f;
			}
			string text = "-----------------";
			text = text + "\nId: " + this.selected.id.ToString();
			text = text + "\nState: " + this.selected.State.ToString();
			text += "\nData:";
			text = text + "\n" + Scribe.saver.DebugOutputFor(this.selected);
			text += "\n";
			text += "\nActive QuestParts:";
			bool flag = false;
			for (int j = 0; j < partsListForReading.Count; j++)
			{
				QuestPartActivable questPartActivable = partsListForReading[j] as QuestPartActivable;
				if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
				{
					text = text + "\n" + questPartActivable.ToString();
					flag = true;
				}
			}
			if (!flag)
			{
				text += "\nNone";
			}
			Rect rect3 = new Rect(innerRect.x, curY, 180f, 40f);
			if (Widgets.ButtonText(rect3, "Copy debug to clipboard", true, true, true, null))
			{
				GUIUtility.systemCopyBuffer = text;
			}
			curY += rect3.height + 4f;
			Widgets.LongLabel(innerRect.x, innerRect.width, text, ref curY, true);
		}

		// Token: 0x0600C4F2 RID: 50418 RVA: 0x0048387C File Offset: 0x00481A7C
		private bool ShouldListNow(Quest quest)
		{
			if (quest.hidden && !this.showAll)
			{
				return false;
			}
			switch (this.curTab)
			{
			case MainTabWindow_Quests.QuestsTab.Available:
				return quest.State == QuestState.NotYetAccepted && !quest.dismissed && !quest.hiddenInUI;
			case MainTabWindow_Quests.QuestsTab.Active:
				return quest.State == QuestState.Ongoing && !quest.dismissed && !quest.hiddenInUI;
			case MainTabWindow_Quests.QuestsTab.Historical:
				return !quest.hiddenInUI && (quest.Historical || quest.dismissed);
			default:
				return false;
			}
		}

		// Token: 0x0600C4F3 RID: 50419 RVA: 0x0048390B File Offset: 0x00481B0B
		private IEnumerable<string> DebugPossibleSignals(Quest quest)
		{
			MainTabWindow_Quests.<DebugPossibleSignals>d__80 <DebugPossibleSignals>d__ = new MainTabWindow_Quests.<DebugPossibleSignals>d__80(-2);
			<DebugPossibleSignals>d__.<>4__this = this;
			<DebugPossibleSignals>d__.<>3__quest = quest;
			return <DebugPossibleSignals>d__;
		}

		// Token: 0x0600C4F4 RID: 50420 RVA: 0x00483924 File Offset: 0x00481B24
		private string GetAcceptedAgoByString(Quest quest)
		{
			string value = quest.TicksSinceAccepted.ToStringTicksToPeriod(true, false, true, true, false);
			if (!quest.AccepterPawnLabelCap.NullOrEmpty())
			{
				return "AcceptedAgoBy".Translate(value, quest.AccepterPawnLabelCap);
			}
			return "AcceptedAgo".Translate(value);
		}

		// Token: 0x0600C4F5 RID: 50421 RVA: 0x00483988 File Offset: 0x00481B88
		private string GetAcceptedOnByString(Quest quest)
		{
			Vector2 locForDates = QuestUtility.GetLocForDates();
			string value = GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(quest.acceptanceTick), locForDates);
			if (!quest.AccepterPawnLabelCap.NullOrEmpty())
			{
				return "AcceptedOnBy".Translate(value, quest.AccepterPawnLabelCap);
			}
			return "AcceptedOn".Translate(value);
		}

		// Token: 0x0600C4F6 RID: 50422 RVA: 0x004839F4 File Offset: 0x00481BF4
		private void AcceptQuestByInterface(Action preAcceptAction = null, bool requiresAccepter = false)
		{
			if (!QuestUtility.CanAcceptQuest(this.selected))
			{
				Messages.Message("MessageCannotAcceptQuest".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (!requiresAccepter)
			{
				SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
				if (preAcceptAction != null)
				{
					preAcceptAction();
				}
				this.selected.Accept(null);
				this.Select(this.selected);
				return;
			}
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn p = enumerator.Current;
					if (QuestUtility.CanPawnAcceptQuest(p, this.selected))
					{
						Pawn pLocal = p;
						string text = "AcceptWith".Translate(p);
						if (p.royalty != null && p.royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
						{
							text = text + " (" + p.royalty.MostSeniorTitle.def.GetLabelFor(pLocal) + ")";
						}
						list.Add(new FloatMenuOption(text, delegate()
						{
							if (!QuestUtility.CanPawnAcceptQuest(pLocal, this.selected))
							{
								return;
							}
							QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = this.selected.PartsListForReading.OfType<QuestPart_GiveRoyalFavor>().FirstOrDefault<QuestPart_GiveRoyalFavor>();
							if (questPart_GiveRoyalFavor == null || !questPart_GiveRoyalFavor.giveToAccepter)
							{
								base.<AcceptQuestByInterface>g__AcceptAction|1();
								return;
							}
							IEnumerable<Trait> conceitedTraits = RoyalTitleUtility.GetConceitedTraits(p);
							IEnumerable<Trait> traitsAffectingPsylinkNegatively = RoyalTitleUtility.GetTraitsAffectingPsylinkNegatively(p);
							bool totallyDisabled = p.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
							bool flag = conceitedTraits.Any<Trait>();
							bool flag2 = !p.HasPsylink && traitsAffectingPsylinkNegatively.Any<Trait>();
							if (totallyDisabled || flag || flag2)
							{
								NamedArgument arg = p.Named("PAWN");
								NamedArgument arg2 = questPart_GiveRoyalFavor.faction.Named("FACTION");
								TaggedString t4 = null;
								if (totallyDisabled)
								{
									t4 = "RoyalIncapableOfSocial".Translate(arg, arg2);
								}
								TaggedString t2 = null;
								if (flag)
								{
									t2 = "RoyalWithConceitedTrait".Translate(arg, arg2, (from t in conceitedTraits
									select t.Label).ToCommaList(true, false));
								}
								TaggedString t3 = null;
								if (flag2)
								{
									t3 = "RoyalWithTraitAffectingPsylinkNegatively".Translate(arg, arg2, (from t in traitsAffectingPsylinkNegatively
									select t.Label).ToCommaList(true, false));
								}
								TaggedString taggedString = "QuestGivesRoyalFavor".Translate(arg, arg2);
								if (totallyDisabled)
								{
									taggedString += "\n\n" + t4;
								}
								if (flag)
								{
									taggedString += "\n\n" + t2;
								}
								if (flag2)
								{
									taggedString += "\n\n" + t3;
								}
								taggedString += "\n\n" + "WantToContinue".Translate();
								Find.WindowStack.Add(new Dialog_MessageBox(taggedString, "Confirm".Translate(), new Action(base.<AcceptQuestByInterface>g__AcceptAction|1), "GoBack".Translate(), null, null, false, null, null, WindowLayer.Dialog));
								return;
							}
							base.<AcceptQuestByInterface>g__AcceptAction|1();
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
			}
			if (list.Count > 0)
			{
				Find.WindowStack.Add(new FloatMenu(list));
				return;
			}
			Messages.Message("MessageNoColonistCanAcceptQuest".Translate(Faction.OfPlayer.def.pawnsPlural), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0600C4FF RID: 50431 RVA: 0x00483DEC File Offset: 0x00481FEC
		[CompilerGenerated]
		private void <DoQuestsList>g__DrawQuest|49_0(Quest quest, int indent = 0, ref MainTabWindow_Quests.<>c__DisplayClass49_0 A_3)
		{
			if (MainTabWindow_Quests.tmpQuestsVisited.Contains(quest))
			{
				return;
			}
			if (quest.parent != null && MainTabWindow_Quests.tmpQuestsToShow.Contains(quest.parent) && !MainTabWindow_Quests.tmpQuestsVisited.Contains(quest.parent))
			{
				return;
			}
			float num = A_3.scrollPosition.y - 32f;
			float num2 = A_3.scrollPosition.y + A_3.scrollOutRect.height;
			if (A_3.curY > num && A_3.curY < num2)
			{
				float num3 = (float)indent * 10f;
				this.DoRow(new Rect(num3, A_3.curY, A_3.scrollViewRect.width - 4f - num3, 32f), quest);
			}
			A_3.curY += 32f;
			MainTabWindow_Quests.tmpQuestsVisited.Add(quest);
			indent++;
			foreach (Quest quest2 in quest.GetSubquests(null))
			{
				if (MainTabWindow_Quests.tmpQuestsToShow.Contains(quest2))
				{
					this.<DoQuestsList>g__DrawQuest|49_0(quest2, indent, ref A_3);
				}
			}
		}

		// Token: 0x04007069 RID: 28777
		private Quest selected;

		// Token: 0x0400706A RID: 28778
		private MainTabWindow_Quests.QuestsTab curTab;

		// Token: 0x0400706B RID: 28779
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x0400706C RID: 28780
		private Vector2 scrollPosition_available;

		// Token: 0x0400706D RID: 28781
		private Vector2 scrollPosition_active;

		// Token: 0x0400706E RID: 28782
		private Vector2 scrollPosition_historical;

		// Token: 0x0400706F RID: 28783
		private Vector2 selectedQuestScrollPosition;

		// Token: 0x04007070 RID: 28784
		private float selectedQuestLastHeight;

		// Token: 0x04007071 RID: 28785
		private bool showDebugInfo;

		// Token: 0x04007072 RID: 28786
		private List<QuestPart> tmpQuestParts = new List<QuestPart>();

		// Token: 0x04007073 RID: 28787
		private string debugSendSignalTextField;

		// Token: 0x04007074 RID: 28788
		private bool showAll;

		// Token: 0x04007075 RID: 28789
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x04007076 RID: 28790
		private const float LeftRectWidthFraction = 0.36f;

		// Token: 0x04007077 RID: 28791
		private const float RowHeight = 32f;

		// Token: 0x04007078 RID: 28792
		private const float CheckBoxHeight = 24f;

		// Token: 0x04007079 RID: 28793
		private const float ShowDebugInfoToggleWidth = 110f;

		// Token: 0x0400707A RID: 28794
		private const float DismisIconWidth = 32f;

		// Token: 0x0400707B RID: 28795
		private const float TimeInfoWidth = 35f;

		// Token: 0x0400707C RID: 28796
		private const float CharityIconWidth = 32f;

		// Token: 0x0400707D RID: 28797
		private static readonly Color TimeLimitColor = new Color(1f, 1f, 1f, 0.7f);

		// Token: 0x0400707E RID: 28798
		private static readonly Color AcceptanceRequirementsColor = new Color(1f, 0.25f, 0.25f);

		// Token: 0x0400707F RID: 28799
		private static readonly Color AcceptanceRequirementsBoxColor = new Color(0.62f, 0.18f, 0.18f);

		// Token: 0x04007080 RID: 28800
		private static readonly Color acceptanceRequirementsBoxBgColor = new Color(0.13f, 0.13f, 0.13f);

		// Token: 0x04007081 RID: 28801
		private static readonly Color IdeoCharityTextColor = new Color32(byte.MaxValue, 237, 38, byte.MaxValue);

		// Token: 0x04007082 RID: 28802
		private static readonly Color IdeoCharityBoxBorderColor = new Color32(205, 207, 18, byte.MaxValue);

		// Token: 0x04007083 RID: 28803
		private static readonly Color IdeoCharityBoxBackgroundColor = new Color(0.13f, 0.13f, 0.13f);

		// Token: 0x04007084 RID: 28804
		private static readonly Color QuestCompletedColor = GenColor.FromHex("1e591a");

		// Token: 0x04007085 RID: 28805
		private static readonly Color QuestFailedColor = GenColor.FromHex("5e2f2f");

		// Token: 0x04007086 RID: 28806
		private static readonly Color QuestExpiredColor = GenColor.FromHex("916e2d");

		// Token: 0x04007087 RID: 28807
		private const int RowIconSize = 15;

		// Token: 0x04007088 RID: 28808
		private const float RatingWidth = 60f;

		// Token: 0x04007089 RID: 28809
		private const float RewardsConfigButtonHeight = 40f;

		// Token: 0x0400708A RID: 28810
		private static Texture2D RatingIcon = null;

		// Token: 0x0400708B RID: 28811
		private static readonly Texture2D DismissIcon = ContentFinder<Texture2D>.Get("UI/Buttons/Dismiss", true);

		// Token: 0x0400708C RID: 28812
		private static readonly Texture2D ResumeQuestIcon = ContentFinder<Texture2D>.Get("UI/Buttons/UnDismiss", true);

		// Token: 0x0400708D RID: 28813
		private static readonly Texture2D QuestDismissedIcon = ContentFinder<Texture2D>.Get("UI/Icons/DismissedQuestIcon", true);

		// Token: 0x0400708E RID: 28814
		private static readonly Texture2D CharityQuestIcon = ContentFinder<Texture2D>.Get("UI/Icons/CharityQuestIcon", true);

		// Token: 0x0400708F RID: 28815
		private const float IndentWidth = 10f;

		// Token: 0x04007090 RID: 28816
		private const float SearchBoxHeight = 24f;

		// Token: 0x04007091 RID: 28817
		private static List<Quest> tmpQuestsToShow = new List<Quest>();

		// Token: 0x04007092 RID: 28818
		private static HashSet<Quest> tmpQuestsVisited = new HashSet<Quest>();

		// Token: 0x04007093 RID: 28819
		private static List<Thing> tmpColonistsForIdeo = new List<Thing>();

		// Token: 0x04007094 RID: 28820
		private static readonly HashSet<string> uniqueEntries = new HashSet<string>();

		// Token: 0x04007095 RID: 28821
		private static List<GenUI.AnonymousStackElement> tmpStackElements = new List<GenUI.AnonymousStackElement>();

		// Token: 0x04007096 RID: 28822
		private static List<QuestPart> tmpRemainingQuestParts = new List<QuestPart>();

		// Token: 0x04007097 RID: 28823
		private static List<GlobalTargetInfo> tmpLookTargets = new List<GlobalTargetInfo>();

		// Token: 0x04007098 RID: 28824
		private static List<GlobalTargetInfo> tmpSelectTargets = new List<GlobalTargetInfo>();

		// Token: 0x02003AF5 RID: 15093
		private enum QuestsTab : byte
		{
			// Token: 0x0400EA2A RID: 59946
			Available,
			// Token: 0x0400EA2B RID: 59947
			Active,
			// Token: 0x0400EA2C RID: 59948
			Historical
		}
	}
}
