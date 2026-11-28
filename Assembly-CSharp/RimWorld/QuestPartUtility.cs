using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110D RID: 4365
	public static class QuestPartUtility
	{
		// Token: 0x06006D12 RID: 27922 RVA: 0x00262CC8 File Offset: 0x00260EC8
		public static T MakeAndAddEndCondition<T>(Quest quest, string inSignalActivate, QuestEndOutcome outcome, Letter letter = null) where T : QuestPartActivable, new()
		{
			T t = Activator.CreateInstance<T>();
			t.inSignalEnable = inSignalActivate;
			quest.AddPart(t);
			if (letter != null)
			{
				quest.AddPart(new QuestPart_Letter
				{
					letter = letter,
					inSignal = t.OutSignalCompleted
				});
			}
			quest.AddPart(new QuestPart_QuestEnd
			{
				inSignal = t.OutSignalCompleted,
				outcome = new QuestEndOutcome?(outcome)
			});
			return t;
		}

		// Token: 0x06006D13 RID: 27923 RVA: 0x00262D48 File Offset: 0x00260F48
		public static QuestPart_QuestEnd MakeAndAddEndNodeWithLetter(Quest quest, string inSignalActivate, QuestEndOutcome outcome, Letter letter)
		{
			quest.AddPart(new QuestPart_Letter
			{
				letter = letter,
				inSignal = inSignalActivate
			});
			QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
			questPart_QuestEnd.inSignal = inSignalActivate;
			questPart_QuestEnd.outcome = new QuestEndOutcome?(outcome);
			quest.AddPart(questPart_QuestEnd);
			return questPart_QuestEnd;
		}

		// Token: 0x06006D14 RID: 27924 RVA: 0x00262D94 File Offset: 0x00260F94
		public static QuestPart_Delay MakeAndAddQuestTimeoutDelay(Quest quest, int delayTicks, WorldObject worldObject)
		{
			QuestPart_WorldObjectTimeout questPart_WorldObjectTimeout = new QuestPart_WorldObjectTimeout();
			questPart_WorldObjectTimeout.delayTicks = delayTicks;
			questPart_WorldObjectTimeout.expiryInfoPart = "QuestExpiresIn".Translate();
			questPart_WorldObjectTimeout.expiryInfoPartTip = "QuestExpiresOn".Translate();
			questPart_WorldObjectTimeout.isBad = true;
			questPart_WorldObjectTimeout.outcomeCompletedSignalArg = QuestEndOutcome.Fail;
			questPart_WorldObjectTimeout.inSignalEnable = quest.InitiateSignal;
			quest.AddPart(questPart_WorldObjectTimeout);
			string text = "Quest" + quest.id.ToString() + ".DelayingWorldObject";
			QuestUtility.AddQuestTag(ref worldObject.questTags, text);
			questPart_WorldObjectTimeout.inSignalDisable = text + ".MapGenerated";
			quest.AddPart(new QuestPart_QuestEnd
			{
				inSignal = questPart_WorldObjectTimeout.OutSignalCompleted
			});
			return questPart_WorldObjectTimeout;
		}

		// Token: 0x06006D15 RID: 27925 RVA: 0x00262E4B File Offset: 0x0026104B
		public static IEnumerable<GenUI.AnonymousStackElement> GetRewardStackElementsForThings(IEnumerable<Thing> things, bool detailsHidden = false)
		{
			QuestPartUtility.<GetRewardStackElementsForThings>d__8 <GetRewardStackElementsForThings>d__ = new QuestPartUtility.<GetRewardStackElementsForThings>d__8(-2);
			<GetRewardStackElementsForThings>d__.<>3__things = things;
			<GetRewardStackElementsForThings>d__.<>3__detailsHidden = detailsHidden;
			return <GetRewardStackElementsForThings>d__;
		}

		// Token: 0x06006D16 RID: 27926 RVA: 0x00262E62 File Offset: 0x00261062
		public static IEnumerable<GenUI.AnonymousStackElement> GetRewardStackElementsForThings(IEnumerable<Reward_Items.RememberedItem> thingDefs)
		{
			QuestPartUtility.<GetRewardStackElementsForThings>d__10 <GetRewardStackElementsForThings>d__ = new QuestPartUtility.<GetRewardStackElementsForThings>d__10(-2);
			<GetRewardStackElementsForThings>d__.<>3__thingDefs = thingDefs;
			return <GetRewardStackElementsForThings>d__;
		}

		// Token: 0x06006D17 RID: 27927 RVA: 0x00262E74 File Offset: 0x00261074
		public static GenUI.AnonymousStackElement GetStandardRewardStackElement(string label, Texture2D icon, Func<string> tipGetter, Action onClick = null)
		{
			return QuestPartUtility.GetStandardRewardStackElement(label, delegate(Rect r)
			{
				GUI.DrawTexture(r, icon);
			}, tipGetter, onClick);
		}

		// Token: 0x06006D18 RID: 27928 RVA: 0x00262EA4 File Offset: 0x002610A4
		public static GenUI.AnonymousStackElement GetStandardRewardStackElement(string label, Action<Rect> iconDrawer, Func<string> tipGetter, Action onClick = null)
		{
			return new GenUI.AnonymousStackElement
			{
				drawer = delegate(Rect rect)
				{
					Widgets.DrawHighlight(rect);
					Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
						if (tipGetter != null)
						{
							TooltipHandler.TipRegion(rect, new TipSignal(tipGetter, 90876542 ^ (int)rect.x ^ (int)rect.y));
						}
					}
					Rect obj = new Rect(rect2.x, rect2.y, 22f, 22f);
					iconDrawer(obj);
					Rect rect3 = rect2;
					rect3.xMin += 24f;
					Widgets.Label(rect3, label);
					if (onClick != null && Widgets.ButtonInvisible(rect, true))
					{
						onClick();
					}
				},
				width = Text.CalcSize(label).x + 12f + 22f + 2f
			};
		}

		// Token: 0x04004559 RID: 17753
		public const int RewardStackElementIconSize = 22;

		// Token: 0x0400455A RID: 17754
		public const int RewardStackElementMarginHorizontal = 6;

		// Token: 0x0400455B RID: 17755
		public const int RewardStackElementMarginVertical = 1;

		// Token: 0x0400455C RID: 17756
		public const int RewardStackElementIconGap = 2;

		// Token: 0x0400455D RID: 17757
		private static List<Pair<Thing, int>> tmpThings = new List<Pair<Thing, int>>();

		// Token: 0x0400455E RID: 17758
		private static List<Reward_Items.RememberedItem> tmpThingDefs = new List<Reward_Items.RememberedItem>();
	}
}
