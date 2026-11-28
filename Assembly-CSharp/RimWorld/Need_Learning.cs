using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200157A RID: 5498
	public class Need_Learning : Need
	{
		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x060086CF RID: 34511 RVA: 0x0030B80B File Offset: 0x00309A0B
		protected override bool IsFrozen
		{
			get
			{
				return base.IsFrozen || this.pawn.Deathresting;
			}
		}

		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x060086D0 RID: 34512 RVA: 0x0030B822 File Offset: 0x00309A22
		public bool Suspended
		{
			get
			{
				return this.IsFrozen;
			}
		}

		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x060086D1 RID: 34513 RVA: 0x0030B82C File Offset: 0x00309A2C
		public LearningCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return LearningCategory.Empty;
				}
				if (this.CurLevel < 0.15f)
				{
					return LearningCategory.VeryLow;
				}
				if (this.CurLevel < 0.3f)
				{
					return LearningCategory.Low;
				}
				if (this.CurLevel < 0.7f)
				{
					return LearningCategory.Satisfied;
				}
				if (this.CurLevel < 0.85f)
				{
					return LearningCategory.High;
				}
				return LearningCategory.Extreme;
			}
		}

		// Token: 0x060086D2 RID: 34514 RVA: 0x0030B885 File Offset: 0x00309A85
		public Need_Learning(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>
			{
				0.15f,
				0.3f,
				0.7f,
				0.85f
			};
		}

		// Token: 0x060086D3 RID: 34515 RVA: 0x0030B8C5 File Offset: 0x00309AC5
		public void Learn(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			amount = Mathf.Min(amount, 1f - this.CurLevel);
			this.curLevelInt += amount;
		}

		// Token: 0x060086D4 RID: 34516 RVA: 0x0030B8F4 File Offset: 0x00309AF4
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null, bool drawLabel = true)
		{
			if (this.pawn.learning == null)
			{
				return;
			}
			List<LearningDesireDef> activeLearningDesires = this.pawn.learning.ActiveLearningDesires;
			float num = (float)activeLearningDesires.Count * 30f;
			float num2 = rect.xMax - num;
			for (int i = 0; i < activeLearningDesires.Count; i++)
			{
				Rect rect2 = new Rect(num2 + (float)i * 30f, rect.y, 30f, 30f);
				LearningDesireDef learningDesire = activeLearningDesires[i];
				GUI.DrawTexture(rect2.ContractedBy(5f), activeLearningDesires[i].Icon);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, () => string.Concat(new string[]
					{
						learningDesire.LabelCap.Colorize(ColoredText.TipSectionTitleColor),
						"\n\n",
						learningDesire.description,
						"\n\n",
						"LearningDesireTooltip".Translate().Colorize(ColoredText.SubtleGrayColor)
					}).ResolveTags(), learningDesire.GetHashCode());
					doTooltip = false;
				}
			}
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip, drawLabel);
		}

		// Token: 0x060086D5 RID: 34517 RVA: 0x0030B9E8 File Offset: 0x00309BE8
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= 0.00045000002f;
			}
			if (this.pawn.ageTracker.canGainGrowthPoints)
			{
				this.pawn.ageTracker.growthPoints += this.pawn.ageTracker.GrowthPointsPerDay * 0.0025f;
			}
		}

		// Token: 0x060086D6 RID: 34518 RVA: 0x0030BA50 File Offset: 0x00309C50
		public override string GetTipString()
		{
			if (Need_Learning.learningActivitiesLineList == null)
			{
				Need_Learning.learningActivitiesLineList = (from d in DefDatabase<LearningDesireDef>.AllDefsListForReading
				select d.label).ToList<string>().ToLineList("  - ", true);
			}
			return (base.LabelCap + ": " + base.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor) + "\n" + this.def.description.Formatted(Need_Learning.learningActivitiesLineList.Named("ACTIVITIES"), this.pawn.Named("PAWN")).Resolve();
		}

		// Token: 0x04005332 RID: 21298
		private static string learningActivitiesLineList;

		// Token: 0x04005333 RID: 21299
		public const float BaseFallPerTick = 3E-06f;

		// Token: 0x04005334 RID: 21300
		public const float BaseFallPerInterval = 0.00045000002f;

		// Token: 0x04005335 RID: 21301
		public const float ThresholdEmpty = 0.01f;

		// Token: 0x04005336 RID: 21302
		public const float ThresholdLow = 0.15f;

		// Token: 0x04005337 RID: 21303
		public const float ThresholdSatisfied = 0.3f;

		// Token: 0x04005338 RID: 21304
		public const float ThresholdHigh = 0.7f;

		// Token: 0x04005339 RID: 21305
		public const float ThresholdVeryHigh = 0.85f;

		// Token: 0x0400533A RID: 21306
		public const float IconSize = 30f;

		// Token: 0x0400533B RID: 21307
		public const float IconPad = 5f;
	}
}
