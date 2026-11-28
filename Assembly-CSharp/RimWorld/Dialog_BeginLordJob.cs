using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C95 RID: 7317
	[StaticConstructorOnStartup]
	public abstract class Dialog_BeginLordJob : Window
	{
		// Token: 0x170021E6 RID: 8678
		// (get) Token: 0x0600BAC4 RID: 47812 RVA: 0x0041F4B4 File Offset: 0x0041D6B4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(845f, 740f);
			}
		}

		// Token: 0x170021E7 RID: 8679
		// (get) Token: 0x0600BAC5 RID: 47813 RVA: 0x0041F4C5 File Offset: 0x0041D6C5
		protected virtual Vector2 ButtonSize
		{
			get
			{
				return new Vector2(200f, 40f);
			}
		}

		// Token: 0x0600BAC6 RID: 47814 RVA: 0x0041F4D6 File Offset: 0x0041D6D6
		protected virtual IEnumerable<string> BlockingIssues()
		{
			return null;
		}

		// Token: 0x170021E8 RID: 8680
		// (get) Token: 0x0600BAC7 RID: 47815 RVA: 0x0041F4DC File Offset: 0x0041D6DC
		protected string WarningText
		{
			get
			{
				if (this.BlockingIssues() == null)
				{
					return "";
				}
				string result = "";
				using (IEnumerator<string> enumerator = this.BlockingIssues().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						result = enumerator.Current;
					}
				}
				return result;
			}
		}

		// Token: 0x170021E9 RID: 8681
		// (get) Token: 0x0600BAC8 RID: 47816 RVA: 0x0041F53C File Offset: 0x0041D73C
		public virtual bool CanBegin
		{
			get
			{
				IEnumerable<string> enumerable = this.BlockingIssues();
				return enumerable == null || !enumerable.Any<string>();
			}
		}

		// Token: 0x170021EA RID: 8682
		// (get) Token: 0x0600BAC9 RID: 47817 RVA: 0x0041F55E File Offset: 0x0041D75E
		public virtual TaggedString HeaderLabel
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170021EB RID: 8683
		// (get) Token: 0x0600BACA RID: 47818 RVA: 0x0041F56A File Offset: 0x0041D76A
		public virtual TaggedString DescriptionLabel
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170021EC RID: 8684
		// (get) Token: 0x0600BACB RID: 47819 RVA: 0x0041F576 File Offset: 0x0041D776
		public virtual TaggedString ExtraExplanationLabel
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170021ED RID: 8685
		// (get) Token: 0x0600BACC RID: 47820 RVA: 0x0041F582 File Offset: 0x0041D782
		public virtual TaggedString ExpectedQualityLabel
		{
			get
			{
				return "ExpectedLordJobQuality".Translate();
			}
		}

		// Token: 0x170021EE RID: 8686
		// (get) Token: 0x0600BACD RID: 47821 RVA: 0x0041F58E File Offset: 0x0041D78E
		public virtual TaggedString OkButtonLabel
		{
			get
			{
				return "OK".Translate();
			}
		}

		// Token: 0x170021EF RID: 8687
		// (get) Token: 0x0600BACE RID: 47822 RVA: 0x0041F59A File Offset: 0x0041D79A
		public virtual TaggedString CancelButtonLabel
		{
			get
			{
				return "CancelButton".Translate();
			}
		}

		// Token: 0x170021F0 RID: 8688
		// (get) Token: 0x0600BACF RID: 47823 RVA: 0x0041F5A6 File Offset: 0x0041D7A6
		public virtual TaggedString QualityFactorsLabel
		{
			get
			{
				return "QualityFactors".Translate();
			}
		}

		// Token: 0x170021F1 RID: 8689
		// (get) Token: 0x0600BAD0 RID: 47824 RVA: 0x0041F5B2 File Offset: 0x0041D7B2
		public virtual Texture2D Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600BAD1 RID: 47825 RVA: 0x0041F5B5 File Offset: 0x0041D7B5
		public virtual TaggedString ExpectedDurationLabel(FloatRange qualityRange)
		{
			return TaggedString.Empty;
		}

		// Token: 0x0600BAD2 RID: 47826 RVA: 0x0041F5BC File Offset: 0x0041D7BC
		public virtual string OutcomeChancesLabel(string qualityNumber)
		{
			return "LordJobOutcomeChances".Translate(qualityNumber);
		}

		// Token: 0x0600BAD3 RID: 47827 RVA: 0x0041F5D3 File Offset: 0x0041D7D3
		public virtual string OutcomeToolTip(ILordJobOutcomePossibility possibility)
		{
			return possibility.ToolTip;
		}

		// Token: 0x0600BAD4 RID: 47828 RVA: 0x0041F5E0 File Offset: 0x0041D7E0
		public virtual Color QualitySummaryColor(FloatRange qualityRange)
		{
			return Color.white;
		}

		// Token: 0x0600BAD5 RID: 47829 RVA: 0x0041F5E7 File Offset: 0x0041D7E7
		protected virtual void Start()
		{
			this.Close(true);
		}

		// Token: 0x0600BAD6 RID: 47830 RVA: 0x0041F5F0 File Offset: 0x0041D7F0
		protected virtual void Cancel()
		{
			this.Close(true);
		}

		// Token: 0x0600BAD7 RID: 47831 RVA: 0x0041F5F9 File Offset: 0x0041D7F9
		public virtual void DoExtraHeaderInfo(ref RectDivider layout, ref RectDivider headerLabelRow)
		{
		}

		// Token: 0x0600BAD8 RID: 47832 RVA: 0x0041F5FB File Offset: 0x0041D7FB
		public virtual void DrawExtraOutcomeDescriptions(Rect viewRect, FloatRange qualityRange, string qualityNumber, ref float curY, ref float totalInfoHeight)
		{
		}

		// Token: 0x0600BAD9 RID: 47833 RVA: 0x0041F5FD File Offset: 0x0041D7FD
		public Dialog_BeginLordJob(IPawnRoleSelectionWidget participantsDrawer) : base(null)
		{
			if (!ModLister.CheckAnyExpansion("Ritual"))
			{
				return;
			}
			this.participantsDrawer = participantsDrawer;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.forcePause = true;
		}

		// Token: 0x0600BADA RID: 47834 RVA: 0x0041F63C File Offset: 0x0041D83C
		public override void DoWindowContents(Rect inRect)
		{
			using (TextBlock.Default())
			{
				RectDivider rectDivider = new RectDivider(inRect, 798775645, null);
				rectDivider.NewRow(0f, VerticalJustification.Bottom, new float?((float)1));
				this.DoHeader(ref rectDivider);
				rectDivider.NewRow(0f, VerticalJustification.Top, null);
				this.DoDescription(ref rectDivider);
				this.DoButtonRow(ref rectDivider, this.CanBegin);
				rectDivider.NewRow(0f, VerticalJustification.Top, new float?(20f));
				rectDivider.NewCol(20f, HorizontalJustification.Left, new float?(0f));
				RectDivider rectDivider2 = rectDivider.NewCol(320f, HorizontalJustification.Left, new float?(24f));
				RectDivider rectDivider3 = rectDivider;
				this.DoLeftColumn(ref rectDivider2);
				this.DoRightColumn(ref rectDivider3);
			}
		}

		// Token: 0x0600BADB RID: 47835 RVA: 0x0041F72C File Offset: 0x0041D92C
		public virtual void DoHeader(ref RectDivider layout)
		{
			RectDivider rectDivider;
			using (new TextBlock(GameFont.Medium))
			{
				Vector2 vector = Text.CalcSize(this.HeaderLabel);
				rectDivider = layout.NewRow(vector.y, VerticalJustification.Top, null);
				if (this.Icon != null)
				{
					Widgets.DrawTextureFitted(rectDivider.NewCol(vector.y, HorizontalJustification.Left, new float?(5f)), this.Icon, 1f, 1f);
				}
				Widgets.Label(rectDivider.NewCol(vector.x, HorizontalJustification.Left, null), this.HeaderLabel);
			}
			this.DoExtraHeaderInfo(ref layout, ref rectDivider);
		}

		// Token: 0x0600BADC RID: 47836 RVA: 0x0041F7FC File Offset: 0x0041D9FC
		public virtual void DoDescription(ref RectDivider layout)
		{
			using (new ProfilerBlock("DoDescription"))
			{
				string text = this.DescriptionLabel;
				if (!text.NullOrEmpty())
				{
					float num = Text.CalcHeight(text, layout.Rect.width - 30f);
					RectDivider rect = layout.NewRow(num + 17f, VerticalJustification.Top, new float?(10f));
					rect.NewRow(0f, VerticalJustification.Top, new float?(10f));
					rect.NewCol(10f, HorizontalJustification.Left, new float?(0f));
					rect.NewCol(20f, HorizontalJustification.Right, new float?(0f));
					Widgets.Label(rect, text);
				}
				string text2 = this.ExtraExplanationLabel;
				if (!text2.NullOrEmpty() || !text2.NullOrEmpty())
				{
					float num2 = Text.CalcHeight(text2, layout.Rect.width - 30f);
					RectDivider rect2 = layout.NewRow(num2 + 17f, VerticalJustification.Top, new float?(0f));
					rect2.NewRow(0f, VerticalJustification.Top, new float?(10f));
					rect2.NewCol(10f, HorizontalJustification.Left, new float?(0f));
					rect2.NewCol(20f, HorizontalJustification.Right, new float?(0f));
					Widgets.Label(rect2, text2);
				}
			}
		}

		// Token: 0x0600BADD RID: 47837 RVA: 0x0041F990 File Offset: 0x0041DB90
		public virtual void DoButtonRow(ref RectDivider layout, bool canBegin)
		{
			RectDivider rectDivider = layout.NewRow(this.ButtonSize.y, VerticalJustification.Bottom, new float?(0f));
			RectDivider rect = rectDivider.NewCol(this.ButtonSize.x, HorizontalJustification.Right, new float?(10f));
			RectDivider rect2 = rectDivider.NewCol(this.ButtonSize.x, HorizontalJustification.Left, null);
			RectDivider rect3 = rectDivider.NewCol(rectDivider.Rect.width, HorizontalJustification.Right, null);
			using (new TextBlock(canBegin ? Color.white : Color.gray))
			{
				if (Widgets.ButtonText(rect, this.OkButtonLabel, true, true, canBegin, null))
				{
					this.Start();
				}
			}
			if (Widgets.ButtonText(rect2, this.CancelButtonLabel, true, true, true, null))
			{
				this.Cancel();
			}
			using (new TextBlock(TextAnchor.MiddleRight, ColorLibrary.RedReadable))
			{
				Widgets.Label(rect3, this.WarningText);
			}
		}

		// Token: 0x0600BADE RID: 47838 RVA: 0x0041FAE0 File Offset: 0x0041DCE0
		public virtual void DoLeftColumn(ref RectDivider layout)
		{
			layout.NewRow(10f, VerticalJustification.Bottom, new float?(0f));
			using (new ProfilerBlock("DrawPawnList"))
			{
				this.participantsDrawer.DrawPawnList(layout);
			}
		}

		// Token: 0x0600BADF RID: 47839 RVA: 0x0041FB48 File Offset: 0x0041DD48
		public virtual void DoRightColumn(ref RectDivider layout)
		{
			this.DrawQualityFactors(layout.NewCol(402f, HorizontalJustification.Left, null));
		}

		// Token: 0x0600BAE0 RID: 47840 RVA: 0x0041FB78 File Offset: 0x0041DD78
		private float DrawQualityFactor(Rect viewRect, bool even, QualityFactor qualityFactor, float y)
		{
			if (qualityFactor == null)
			{
				return 0f;
			}
			Rect rect = new Rect(viewRect.x, y, viewRect.width, 25f);
			Rect rect2 = new Rect
			{
				x = viewRect.x,
				width = viewRect.width + 10f,
				y = y - 3f,
				height = 28f
			};
			if (even)
			{
				Widgets.DrawLightHighlight(rect2);
			}
			GUI.color = (qualityFactor.uncertainOutcome ? ColorLibrary.Yellow : (qualityFactor.positive ? ColorLibrary.Green : ColorLibrary.RedReadable));
			Rect rect3 = rect;
			rect3.width = 205f;
			Widgets.LabelEllipses(rect3, "  " + qualityFactor.label);
			using (new TextBlock(TextAnchor.UpperRight))
			{
				Widgets.Label(rect, qualityFactor.qualityChange);
			}
			if (!qualityFactor.noMiddleColumnInfo)
			{
				if (!qualityFactor.count.NullOrEmpty())
				{
					float x = Text.CalcSize(qualityFactor.count).x;
					Rect rect4 = new Rect(rect);
					rect4.xMin += 220f - x / 2f;
					rect4.width = x;
					Widgets.Label(rect4, qualityFactor.count);
				}
				else
				{
					GUI.color = Color.white;
					Texture2D image;
					if (qualityFactor.uncertainOutcome)
					{
						image = Dialog_BeginLordJob.questionMark;
					}
					else if (qualityFactor.present)
					{
						image = Dialog_BeginLordJob.QualityOffsetCheckOn;
					}
					else
					{
						image = Dialog_BeginLordJob.QualityOffsetCheckOff;
					}
					Rect rect5 = new Rect(rect);
					rect5.x += 208f;
					rect5.y -= 1f;
					rect5.width = 24f;
					rect5.height = 24f;
					if (!qualityFactor.present)
					{
						if (qualityFactor.uncertainOutcome)
						{
							TooltipHandler.TipRegion(rect5, () => "QualityFactorTooltipUncertain".Translate(), 238934347);
						}
						else
						{
							TooltipHandler.TipRegion(rect5, () => "QualityFactorTooltipNotFulfilled".Translate(), 238934347);
						}
					}
					GUI.DrawTexture(rect5, image);
				}
			}
			GUI.color = Color.white;
			if (qualityFactor.toolTip != null && Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect, () => qualityFactor.toolTip, 976091152);
			}
			return 28f;
		}

		// Token: 0x0600BAE1 RID: 47841 RVA: 0x0041FE60 File Offset: 0x0041E060
		public static string QualityNumberToString(FloatRange qualityRange)
		{
			if (qualityRange.min == qualityRange.max)
			{
				return qualityRange.min.ToStringPercent("F0");
			}
			return qualityRange.min.ToStringPercent("F0") + "-" + qualityRange.max.ToStringPercent("F0");
		}

		// Token: 0x0600BAE2 RID: 47842 RVA: 0x0041FEB6 File Offset: 0x0041E0B6
		protected virtual List<QualityFactor> PopulateQualityFactors(out FloatRange qualityRange)
		{
			this.tmpExpectedOutcomeEffects.Clear();
			qualityRange = new FloatRange(0f, 0f);
			return this.tmpExpectedOutcomeEffects;
		}

		// Token: 0x0600BAE3 RID: 47843 RVA: 0x0041FEDE File Offset: 0x0041E0DE
		protected virtual List<ILordJobOutcomePossibility> PopulateOutcomePossibilities()
		{
			Dialog_BeginLordJob.tmpOutcomes.Clear();
			return Dialog_BeginLordJob.tmpOutcomes;
		}

		// Token: 0x0600BAE4 RID: 47844 RVA: 0x0041FEF0 File Offset: 0x0041E0F0
		public virtual void DrawOutcomeChances(Rect viewRect, FloatRange qualityRange, string qualityNumber, ref float curY, ref float totalInfoHeight)
		{
			List<ILordJobOutcomePossibility> list = this.PopulateOutcomePossibilities();
			if (list.NullOrEmpty<ILordJobOutcomePossibility>())
			{
				return;
			}
			Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, 32f), this.OutcomeChancesLabel(qualityNumber) + ": ");
			curY += 28f;
			totalInfoHeight += 28f;
			float num = 0f;
			foreach (ILordJobOutcomePossibility lordJobOutcomePossibility in list)
			{
				num += lordJobOutcomePossibility.Weight(qualityRange);
			}
			foreach (ILordJobOutcomePossibility lordJobOutcomePossibility2 in list)
			{
				float f = lordJobOutcomePossibility2.Weight(qualityRange) / num;
				TaggedString taggedString = "  - " + lordJobOutcomePossibility2.Label + ": " + f.ToStringPercent();
				Rect rect = new Rect(viewRect.x, curY, Text.CalcSize(taggedString).x + 4f, 32f);
				Rect rect2 = new Rect(rect)
				{
					width = rect.width + 8f,
					height = 22f
				};
				if (Mouse.IsOver(rect2))
				{
					string desc = this.OutcomeToolTip(lordJobOutcomePossibility2);
					Widgets.DrawLightHighlight(rect2);
					if (!desc.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, () => desc, 231134347);
					}
				}
				Widgets.Label(rect, taggedString);
				curY += Text.LineHeight;
				totalInfoHeight += Text.LineHeight;
			}
		}

		// Token: 0x0600BAE5 RID: 47845 RVA: 0x004200F4 File Offset: 0x0041E2F4
		protected virtual void DrawQualityDescription(Rect outRectQualityDesc, FloatRange qualityRange, string qualityNumber, float totalInfoHeight)
		{
			Rect viewRect = new Rect(0f, 0f, outRectQualityDesc.width, this.qualityDescHeight);
			bool flag = this.qualityDescHeight > outRectQualityDesc.height;
			if (flag)
			{
				viewRect.width -= 16f;
			}
			float num = 0f;
			Widgets.BeginScrollView(outRectQualityDesc, ref this.scrollPositionQualityDesc, viewRect, flag);
			TaggedString taggedString = this.ExpectedDurationLabel(qualityRange);
			if (!taggedString.NullOrEmpty())
			{
				float num2 = Text.CalcHeight(taggedString, viewRect.width);
				Widgets.Label(new Rect(viewRect.x, num - 4f, viewRect.width, num2), taggedString);
				num += 17f + (num2 - 22f);
				totalInfoHeight += 17f + (num2 - 22f);
			}
			this.DrawOutcomeChances(viewRect, qualityRange, qualityNumber, ref num, ref totalInfoHeight);
			this.DrawExtraOutcomeDescriptions(viewRect, qualityRange, qualityNumber, ref num, ref totalInfoHeight);
			GUI.color = Color.white;
			this.qualityDescHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x0600BAE6 RID: 47846 RVA: 0x004201F8 File Offset: 0x0041E3F8
		protected virtual void DrawQualityFactors(Rect viewRect)
		{
			float num = viewRect.y;
			float num2 = 0f;
			bool flag = true;
			FloatRange qualityRange;
			List<QualityFactor> list = this.PopulateQualityFactors(out qualityRange);
			if (list.NullOrEmpty<QualityFactor>())
			{
				return;
			}
			Widgets.Label(new Rect(viewRect.x, num + 3f, viewRect.width, 32f), this.QualityFactorsLabel);
			num += 32f;
			num2 += 32f;
			foreach (QualityFactor qualityFactor in from e in list
			orderby e.priority descending
			select e)
			{
				float num3 = this.DrawQualityFactor(viewRect, flag, qualityFactor, num);
				num += num3;
				num2 += num3;
				flag = !flag;
			}
			num += 2f;
			string text = Dialog_BeginLordJob.QualityNumberToString(qualityRange);
			using (new TextBlock(this.QualitySummaryColor(qualityRange)))
			{
				Widgets.Label(new Rect(viewRect.x, num + 4f, viewRect.width, 25f), this.ExpectedQualityLabel + ":");
				using (new TextBlock(GameFont.Medium))
				{
					float x = Text.CalcSize(text).x;
					Widgets.Label(new Rect(viewRect.xMax - x, num - 2f, viewRect.width, 32f), text);
				}
				num += 28f;
				num2 += 28f;
			}
			Rect rect = viewRect;
			rect.width += 10f;
			rect.height = num2;
			rect = rect.ExpandedBy(9f);
			using (new TextBlock(new Color(0.25f, 0.25f, 0.25f)))
			{
				Widgets.DrawBox(rect, 2, null);
			}
			num += 10f;
			num2 += 10f;
			Rect outRectQualityDesc = new Rect(viewRect.x, num, viewRect.width, viewRect.height - num2);
			this.DrawQualityDescription(outRectQualityDesc, qualityRange, text, num2);
		}

		// Token: 0x0600BAE7 RID: 47847 RVA: 0x00420460 File Offset: 0x0041E660
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			this.participantsDrawer.WindowUpdate();
		}

		// Token: 0x0600BAE8 RID: 47848 RVA: 0x00420473 File Offset: 0x0041E673
		public override void OnAcceptKeyPressed()
		{
			this.Start();
		}

		// Token: 0x04006A30 RID: 27184
		protected IPawnRoleSelectionWidget participantsDrawer;

		// Token: 0x04006A31 RID: 27185
		private Vector2 scrollPositionQualityDesc;

		// Token: 0x04006A32 RID: 27186
		private float qualityDescHeight;

		// Token: 0x04006A33 RID: 27187
		private static readonly Texture2D questionMark = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);

		// Token: 0x04006A34 RID: 27188
		private static readonly Texture2D QualityOffsetCheckOn = Resources.Load<Texture2D>("Textures/UI/Widgets/RitualQualityCheck_On");

		// Token: 0x04006A35 RID: 27189
		private static readonly Texture2D QualityOffsetCheckOff = Resources.Load<Texture2D>("Textures/UI/Widgets/RitualQualityCheck_Off");

		// Token: 0x04006A36 RID: 27190
		protected const float CategoryCaptionHeight = 32f;

		// Token: 0x04006A37 RID: 27191
		protected const float EntryHeight = 28f;

		// Token: 0x04006A38 RID: 27192
		protected const float ListWidth = 320f;

		// Token: 0x04006A39 RID: 27193
		protected const float QualityOffsetListWidth = 402f;

		// Token: 0x04006A3A RID: 27194
		private const int ContextHash = 798775645;

		// Token: 0x04006A3B RID: 27195
		private List<QualityFactor> tmpExpectedOutcomeEffects = new List<QualityFactor>();

		// Token: 0x04006A3C RID: 27196
		private static List<ILordJobOutcomePossibility> tmpOutcomes = new List<ILordJobOutcomePossibility>();
	}
}
