using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D78 RID: 7544
	public class ITab_Pawn_Log : ITab
	{
		// Token: 0x1700243E RID: 9278
		// (get) Token: 0x0600C38F RID: 50063 RVA: 0x00478564 File Offset: 0x00476764
		private Pawn SelPawnForCombatInfo
		{
			get
			{
				if (this.SelPawn != null)
				{
					return this.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				string str = "Social tab on non-pawn non-corpse ";
				Thing selThing = base.SelThing;
				throw new InvalidOperationException(str + ((selThing != null) ? selThing.ToString() : null));
			}
		}

		// Token: 0x0600C390 RID: 50064 RVA: 0x004785B8 File Offset: 0x004767B8
		public ITab_Pawn_Log()
		{
			this.size = new Vector2(630f, 510f);
			this.labelKey = "TabLog";
		}

		// Token: 0x0600C391 RID: 50065 RVA: 0x00478614 File Offset: 0x00476814
		protected override void FillTab()
		{
			Pawn selPawnForCombatInfo = this.SelPawnForCombatInfo;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(ITab_Pawn_Log.ShowAllX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowAllWidth, 24f);
			bool flag = this.showAll;
			Widgets.CheckboxLabeled(rect2, "ShowAll".Translate(), ref this.showAll, false, null, null, false, false);
			if (flag != this.showAll)
			{
				this.cachedLogDisplay = null;
			}
			Rect rect3 = new Rect(ITab_Pawn_Log.ShowCombatX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowCombatWidth, 24f);
			bool flag2 = this.showCombat;
			Widgets.CheckboxLabeled(rect3, "ShowCombat".Translate(), ref this.showCombat, false, null, null, false, false);
			if (flag2 != this.showCombat)
			{
				this.cachedLogDisplay = null;
			}
			Rect rect4 = new Rect(ITab_Pawn_Log.ShowSocialX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowSocialWidth, 24f);
			bool flag3 = this.showSocial;
			Widgets.CheckboxLabeled(rect4, "ShowSocial".Translate(), ref this.showSocial, false, null, null, false, false);
			if (flag3 != this.showSocial)
			{
				this.cachedLogDisplay = null;
			}
			Text.Font = font;
			if (this.cachedLogDisplay == null || this.cachedLogDisplayLastTick != selPawnForCombatInfo.records.LastBattleTick || this.cachedLogPlayLastTick != Find.PlayLog.LastTick || this.cachedLogForPawn != selPawnForCombatInfo)
			{
				this.cachedLogDisplay = ITab_Pawn_Log_Utility.GenerateLogLinesFor(selPawnForCombatInfo, this.showAll, this.showCombat, this.showSocial, 300);
				this.cachedLogDisplayLastTick = selPawnForCombatInfo.records.LastBattleTick;
				this.cachedLogPlayLastTick = Find.PlayLog.LastTick;
				this.cachedLogForPawn = selPawnForCombatInfo;
			}
			Rect rect5 = new Rect(rect.width - ITab_Pawn_Log.ButtonOffset, 0f, 18f, 24f);
			if (Widgets.ButtonImage(rect5, TexButton.Copy, true, null))
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable in this.cachedLogDisplay)
				{
					logLineDisplayable.AppendTo(stringBuilder);
				}
				GUIUtility.systemCopyBuffer = stringBuilder.ToString();
			}
			TooltipHandler.TipRegionByKey(rect5, "CopyLogTip");
			rect.yMin = 24f;
			rect = rect.ContractedBy(10f);
			float width = rect.width - 16f - 10f;
			float num = 0f;
			foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable2 in this.cachedLogDisplay)
			{
				if (logLineDisplayable2.Matches(this.logSeek))
				{
					this.scrollPosition.y = num - (logLineDisplayable2.GetHeight(width) + rect.height) / 2f;
				}
				num += logLineDisplayable2.GetHeight(width);
			}
			this.logSeek = null;
			if (num > 0f)
			{
				Rect rect6 = new Rect(0f, 0f, rect.width - 16f, num);
				this.data.StartNewDraw();
				Rect rect7 = rect6;
				rect7.yMin += this.scrollPosition.y;
				rect7.height = rect.height;
				Widgets.BeginScrollView(rect, ref this.scrollPosition, rect6, true);
				float num2 = 0f;
				foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable3 in this.cachedLogDisplay)
				{
					float height = logLineDisplayable3.GetHeight(width);
					if (rect7.Overlaps(new Rect(0f, num2, width, height)))
					{
						logLineDisplayable3.Draw(num2, width, this.data);
					}
					else
					{
						this.data.alternatingBackground = !this.data.alternatingBackground;
					}
					num2 += height;
				}
				Widgets.EndScrollView();
				return;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = Color.grey;
			Widgets.Label(new Rect(0f, 0f, this.size.x, this.size.y), "(" + "NoRecentEntries".Translate() + ")");
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x0600C392 RID: 50066 RVA: 0x00478A94 File Offset: 0x00476C94
		public void SeekTo(LogEntry entry)
		{
			this.logSeek = entry;
		}

		// Token: 0x0600C393 RID: 50067 RVA: 0x00478A9D File Offset: 0x00476C9D
		public void Highlight(LogEntry entry)
		{
			this.data.highlightEntry = entry;
			this.data.highlightIntensity = 1f;
		}

		// Token: 0x0600C394 RID: 50068 RVA: 0x00478ABB File Offset: 0x00476CBB
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.cachedLogForPawn = null;
		}

		// Token: 0x04006FB9 RID: 28601
		public const float Width = 630f;

		// Token: 0x04006FBA RID: 28602
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllX = 60f;

		// Token: 0x04006FBB RID: 28603
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllWidth = 100f;

		// Token: 0x04006FBC RID: 28604
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatX = 445f;

		// Token: 0x04006FBD RID: 28605
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatWidth = 115f;

		// Token: 0x04006FBE RID: 28606
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialX = 330f;

		// Token: 0x04006FBF RID: 28607
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialWidth = 105f;

		// Token: 0x04006FC0 RID: 28608
		[TweakValue("Interface", 0f, 20f)]
		private static float ToolbarHeight = 2f;

		// Token: 0x04006FC1 RID: 28609
		[TweakValue("Interface", 0f, 100f)]
		private static float ButtonOffset = 60f;

		// Token: 0x04006FC2 RID: 28610
		private const int MaxLogLines = 300;

		// Token: 0x04006FC3 RID: 28611
		public bool showAll;

		// Token: 0x04006FC4 RID: 28612
		public bool showCombat = true;

		// Token: 0x04006FC5 RID: 28613
		public bool showSocial = true;

		// Token: 0x04006FC6 RID: 28614
		public LogEntry logSeek;

		// Token: 0x04006FC7 RID: 28615
		public ITab_Pawn_Log_Utility.LogDrawData data = new ITab_Pawn_Log_Utility.LogDrawData();

		// Token: 0x04006FC8 RID: 28616
		public List<ITab_Pawn_Log_Utility.LogLineDisplayable> cachedLogDisplay;

		// Token: 0x04006FC9 RID: 28617
		public int cachedLogDisplayLastTick = -1;

		// Token: 0x04006FCA RID: 28618
		public int cachedLogPlayLastTick = -1;

		// Token: 0x04006FCB RID: 28619
		private Pawn cachedLogForPawn;

		// Token: 0x04006FCC RID: 28620
		private Vector2 scrollPosition;
	}
}
