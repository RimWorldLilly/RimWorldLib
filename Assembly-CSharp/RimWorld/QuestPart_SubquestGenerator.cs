using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FE RID: 4350
	public abstract class QuestPart_SubquestGenerator : QuestPartActivable
	{
		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06006C8B RID: 27787 RVA: 0x0026100E File Offset: 0x0025F20E
		protected virtual bool CanGenerateSubquest
		{
			get
			{
				return this.PendingSubquestCount < this.maxActiveSubquests && (this.maxSuccessfulSubquests < 0 || this.SuccessfulSubquestCount + this.PendingSubquestCount < this.maxSuccessfulSubquests);
			}
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06006C8C RID: 27788 RVA: 0x00261040 File Offset: 0x0025F240
		protected int SuccessfulSubquestCount
		{
			get
			{
				return this.quest.GetSubquests(new QuestState?(QuestState.EndedSuccess)).Count<Quest>();
			}
		}

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06006C8D RID: 27789 RVA: 0x00261058 File Offset: 0x0025F258
		private int PendingSubquestCount
		{
			get
			{
				return this.quest.GetSubquests(null).Count((Quest q) => q.State == QuestState.Ongoing || q.State == QuestState.NotYetAccepted);
			}
		}

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06006C8E RID: 27790 RVA: 0x0026109D File Offset: 0x0025F29D
		protected bool Paused
		{
			get
			{
				return Find.AnyPlayerHomeMap == null;
			}
		}

		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06006C8F RID: 27791 RVA: 0x002610A8 File Offset: 0x0025F2A8
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.expiryInfoPartKey.NullOrEmpty())
				{
					return null;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(this.expiryInfoPartKey.Translate());
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.SuccessfulSubquestCount);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.maxSuccessfulSubquests);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x06006C90 RID: 27792 RVA: 0x00261118 File Offset: 0x0025F318
		public override void QuestPartTick()
		{
			if (this.subquestDefs.Count == 0)
			{
				return;
			}
			if (this.maxSuccessfulSubquests > 0 && this.SuccessfulSubquestCount >= this.maxSuccessfulSubquests)
			{
				base.Complete();
				return;
			}
			if (this.Paused)
			{
				return;
			}
			if (this.currentInterval != null)
			{
				this.currentInterval--;
				int? num = this.currentInterval;
				int num2 = 0;
				if (num.GetValueOrDefault() < num2 & num != null)
				{
					if (!this.TryGenerateSubquest())
					{
						Log.Warning("Failed to generate subquest for quest " + this.quest.root.defName);
					}
					this.currentInterval = null;
				}
			}
			if (this.currentInterval == null && this.CanGenerateSubquest)
			{
				this.currentInterval = new int?(this.interval.RandomInRange);
			}
		}

		// Token: 0x06006C91 RID: 27793
		protected abstract QuestScriptDef GetNextSubquestDef();

		// Token: 0x06006C92 RID: 27794
		protected abstract Slate InitSlate();

		// Token: 0x06006C93 RID: 27795 RVA: 0x00261214 File Offset: 0x0025F414
		protected virtual bool TryGenerateSubquest()
		{
			QuestScriptDef nextSubquestDef = this.GetNextSubquestDef();
			if (nextSubquestDef == null)
			{
				return false;
			}
			Slate vars = this.InitSlate();
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(nextSubquestDef, vars);
			quest.parent = this.quest;
			if (!quest.hidden && quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
			return true;
		}

		// Token: 0x06006C94 RID: 27796 RVA: 0x00261268 File Offset: 0x0025F468
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Generate random subquest", true, true, true, null) && this.CanGenerateSubquest)
			{
				this.TryGenerateSubquest();
			}
			curY += rect.height + 4f;
			Rect rect2 = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect2, "Generate specific subquest", true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (List<QuestScriptDef>.Enumerator enumerator = this.subquestDefs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						QuestScriptDef questDef = enumerator.Current;
						list.Add(new FloatMenuOption(questDef.defName, delegate()
						{
							Slate vars = this.InitSlate();
							Quest quest2 = QuestUtility.GenerateQuestAndMakeAvailable(questDef, vars);
							quest2.parent = this.quest;
							if (!quest2.hidden && quest2.root.sendAvailableLetter)
							{
								QuestUtility.SendLetterQuestAvailable(quest2, null);
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				if (list.Any<FloatMenuOption>())
				{
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			curY += rect2.height + 4f;
			Rect rect3 = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect3, "Remove active subquests", true, true, true, null))
			{
				foreach (Quest quest in this.quest.GetSubquests(null))
				{
					Find.QuestManager.Remove(quest);
				}
			}
			curY += rect3.height + 4f;
			Rect rect4 = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect4, "Complete quest", true, true, true, null))
			{
				base.Complete();
			}
			curY += rect4.height + 4f;
		}

		// Token: 0x06006C95 RID: 27797 RVA: 0x002614A8 File Offset: 0x0025F6A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int?>(ref this.currentInterval, "currentInterval", null, false);
			Scribe_Collections.Look<QuestScriptDef>(ref this.subquestDefs, "subquestDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<IntRange>(ref this.interval, "interval", default(IntRange), false);
			Scribe_Values.Look<int>(ref this.maxActiveSubquests, "maxActiveSubquests", 0, false);
			Scribe_Values.Look<int>(ref this.maxSuccessfulSubquests, "maxSuccessfulSubquests", -1, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartKey, "expiryInfoPartKey", null, false);
		}

		// Token: 0x04004519 RID: 17689
		public List<QuestScriptDef> subquestDefs = new List<QuestScriptDef>();

		// Token: 0x0400451A RID: 17690
		public IntRange interval;

		// Token: 0x0400451B RID: 17691
		public int maxActiveSubquests = 2;

		// Token: 0x0400451C RID: 17692
		public string expiryInfoPartKey;

		// Token: 0x0400451D RID: 17693
		public int maxSuccessfulSubquests = -1;

		// Token: 0x0400451E RID: 17694
		private int? currentInterval;
	}
}
