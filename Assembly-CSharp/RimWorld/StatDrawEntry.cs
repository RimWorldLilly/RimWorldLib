using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CEF RID: 7407
	public class StatDrawEntry
	{
		// Token: 0x170022AD RID: 8877
		// (get) Token: 0x0600BEBE RID: 48830 RVA: 0x00451413 File Offset: 0x0044F613
		public string LabelCap
		{
			get
			{
				if (this.labelInt != null)
				{
					return this.labelInt.CapitalizeFirst();
				}
				return this.stat.LabelCap;
			}
		}

		// Token: 0x170022AE RID: 8878
		// (get) Token: 0x0600BEBF RID: 48831 RVA: 0x0045143C File Offset: 0x0044F63C
		public string ValueString
		{
			get
			{
				if (this.numberSense == ToStringNumberSense.Factor)
				{
					return this.value.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute);
				}
				if (this.valueStringInt == null)
				{
					return this.stat.Worker.GetStatDrawEntryLabel(this.stat, this.value, this.numberSense, this.optionalReq, !this.forceUnfinalizedMode);
				}
				return this.valueStringInt;
			}
		}

		// Token: 0x170022AF RID: 8879
		// (get) Token: 0x0600BEC0 RID: 48832 RVA: 0x004514A0 File Offset: 0x0044F6A0
		public int DisplayPriorityWithinCategory
		{
			get
			{
				return this.displayOrderWithinCategory;
			}
		}

		// Token: 0x0600BEC1 RID: 48833 RVA: 0x004514A8 File Offset: 0x0044F6A8
		public StatDrawEntry(StatCategoryDef category, StatDef stat, float value, StatRequest optionalReq, ToStringNumberSense numberSense = ToStringNumberSense.Undefined, int? overrideDisplayPriorityWithinCategory = null, bool forceUnfinalizedMode = false)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = value;
			this.valueStringInt = null;
			this.displayOrderWithinCategory = ((overrideDisplayPriorityWithinCategory != null) ? overrideDisplayPriorityWithinCategory.Value : stat.displayPriorityInCategory);
			this.optionalReq = optionalReq;
			this.forceUnfinalizedMode = forceUnfinalizedMode;
			this.overridesHideStats = stat.overridesHideStats;
			this.hasOptionalReq = true;
			if (numberSense == ToStringNumberSense.Undefined)
			{
				this.numberSense = stat.toStringNumberSense;
				return;
			}
			this.numberSense = numberSense;
		}

		// Token: 0x0600BEC2 RID: 48834 RVA: 0x00451538 File Offset: 0x0044F738
		public StatDrawEntry(StatCategoryDef category, StatDef stat, string value)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.valueStringInt = value;
			this.overridesHideStats = stat.overridesHideStats;
			this.hasOptionalReq = false;
			this.numberSense = ToStringNumberSense.Undefined;
		}

		// Token: 0x0600BEC3 RID: 48835 RVA: 0x00451578 File Offset: 0x0044F778
		public StatDrawEntry(StatCategoryDef category, string label, string valueString, string reportText, int displayPriorityWithinCategory, string overrideReportTitle = null, IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks = null, bool forceUnfinalizedMode = false, bool overridesHideStats = false)
		{
			this.category = category;
			this.stat = null;
			this.labelInt = label;
			this.value = 0f;
			this.valueStringInt = valueString;
			this.displayOrderWithinCategory = displayPriorityWithinCategory;
			this.numberSense = ToStringNumberSense.Absolute;
			this.overrideReportText = reportText;
			this.overrideReportTitle = overrideReportTitle;
			this.hyperlinks = hyperlinks;
			this.forceUnfinalizedMode = forceUnfinalizedMode;
			this.overridesHideStats = overridesHideStats;
		}

		// Token: 0x0600BEC4 RID: 48836 RVA: 0x004515EC File Offset: 0x0044F7EC
		public StatDrawEntry(StatCategoryDef category, StatDef stat)
		{
			this.category = category;
			this.stat = stat;
			this.labelInt = null;
			this.value = 0f;
			this.valueStringInt = "-";
			this.displayOrderWithinCategory = stat.displayPriorityInCategory;
			this.numberSense = ToStringNumberSense.Undefined;
		}

		// Token: 0x0600BEC5 RID: 48837 RVA: 0x00451640 File Offset: 0x0044F840
		public bool ShouldDisplay([CanBeNull] Thing thing = null)
		{
			return (thing == null || !thing.def.hideStats || this.overridesHideStats || DebugSettings.showHiddenInfo) && (this.stat == null || !Mathf.Approximately(this.value, this.stat.hideAtValue));
		}

		// Token: 0x0600BEC6 RID: 48838 RVA: 0x00451691 File Offset: 0x0044F891
		public IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks(StatRequest req)
		{
			if (this.hyperlinks != null)
			{
				return this.hyperlinks;
			}
			if (this.stat != null)
			{
				return this.stat.Worker.GetInfoCardHyperlinks(req);
			}
			return null;
		}

		// Token: 0x0600BEC7 RID: 48839 RVA: 0x004516C0 File Offset: 0x0044F8C0
		public string GetExplanationText(StatRequest optionalReq)
		{
			if (this.explanationText == null)
			{
				this.WriteExplanationTextInt();
			}
			string result;
			if (optionalReq.Empty || this.stat == null)
			{
				result = this.explanationText;
			}
			else
			{
				result = string.Format("{0}\n\n{1}", this.explanationText, this.stat.Worker.GetExplanationFull(optionalReq, this.numberSense, this.value));
			}
			return result;
		}

		// Token: 0x0600BEC8 RID: 48840 RVA: 0x00451728 File Offset: 0x0044F928
		private void WriteExplanationTextInt()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.overrideReportTitle.NullOrEmpty())
			{
				stringBuilder.AppendLine(this.overrideReportTitle);
			}
			if (!this.overrideReportText.NullOrEmpty())
			{
				stringBuilder.AppendLine(this.overrideReportText);
			}
			else if (this.stat != null)
			{
				stringBuilder.AppendLine(this.stat.description);
			}
			stringBuilder.AppendLine();
			this.explanationText = stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600BEC9 RID: 48841 RVA: 0x004517A3 File Offset: 0x0044F9A3
		public StatDrawEntry SetReportText(string reportText)
		{
			this.overrideReportText = reportText;
			return this;
		}

		// Token: 0x0600BECA RID: 48842 RVA: 0x004517B0 File Offset: 0x0044F9B0
		public float Draw(float x, float y, float width, bool selected, bool highlightLabel, bool lowlightLabel, Action clickedCallback, Action mousedOverCallback, Vector2 scrollPosition, Rect scrollOutRect, string valueCached = null)
		{
			float num = width * 0.45f;
			string text = valueCached ?? this.ValueString;
			Rect rect = new Rect(8f, y, width, Text.CalcHeight(text, num));
			if (y - scrollPosition.y + rect.height >= 0f && y - scrollPosition.y <= scrollOutRect.height)
			{
				GUI.color = Color.white;
				if (selected)
				{
					Widgets.DrawHighlightSelected(rect);
				}
				else if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				if (highlightLabel)
				{
					Widgets.DrawTextHighlight(rect, 4f, null);
				}
				if (lowlightLabel)
				{
					GUI.color = Color.grey;
				}
				Rect rect2 = rect;
				rect2.width -= num;
				Widgets.Label(rect2, this.LabelCap);
				Rect rect3 = rect;
				rect3.x = rect2.xMax;
				rect3.width = num;
				Widgets.Label(rect3, text);
				GUI.color = Color.white;
				if (this.stat != null && Mouse.IsOver(rect))
				{
					StatDef localStat = this.stat;
					TooltipHandler.TipRegion(rect, new TipSignal(() => localStat.LabelCap + ": " + localStat.description, this.stat.GetHashCode()));
				}
				if (Widgets.ButtonInvisible(rect, true))
				{
					clickedCallback();
				}
				if (Mouse.IsOver(rect))
				{
					mousedOverCallback();
				}
			}
			return rect.height;
		}

		// Token: 0x0600BECB RID: 48843 RVA: 0x00451919 File Offset: 0x0044FB19
		public bool Same(StatDrawEntry other)
		{
			return other != null && (this == other || (this.stat == other.stat && this.labelInt == other.labelInt));
		}

		// Token: 0x0600BECC RID: 48844 RVA: 0x00451947 File Offset: 0x0044FB47
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.LabelCap,
				": ",
				this.ValueString,
				")"
			});
		}

		// Token: 0x04006D88 RID: 28040
		public StatCategoryDef category;

		// Token: 0x04006D89 RID: 28041
		private int displayOrderWithinCategory;

		// Token: 0x04006D8A RID: 28042
		public StatDef stat;

		// Token: 0x04006D8B RID: 28043
		private float value;

		// Token: 0x04006D8C RID: 28044
		public StatRequest optionalReq;

		// Token: 0x04006D8D RID: 28045
		public bool hasOptionalReq;

		// Token: 0x04006D8E RID: 28046
		public bool forceUnfinalizedMode;

		// Token: 0x04006D8F RID: 28047
		public bool overridesHideStats;

		// Token: 0x04006D90 RID: 28048
		private IEnumerable<Dialog_InfoCard.Hyperlink> hyperlinks;

		// Token: 0x04006D91 RID: 28049
		private string labelInt;

		// Token: 0x04006D92 RID: 28050
		private string valueStringInt;

		// Token: 0x04006D93 RID: 28051
		private string overrideReportText;

		// Token: 0x04006D94 RID: 28052
		private string overrideReportTitle;

		// Token: 0x04006D95 RID: 28053
		private string explanationText;

		// Token: 0x04006D96 RID: 28054
		private ToStringNumberSense numberSense;
	}
}
