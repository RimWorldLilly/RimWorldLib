using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200209D RID: 8349
	[StaticConstructorOnStartup]
	public class Gizmo_GrowthTier : Gizmo
	{
		// Token: 0x170025CA RID: 9674
		// (get) Token: 0x0600D264 RID: 53860 RVA: 0x004D45C5 File Offset: 0x004D27C5
		private float Width
		{
			get
			{
				return 190f;
			}
		}

		// Token: 0x170025CB RID: 9675
		// (get) Token: 0x0600D265 RID: 53861 RVA: 0x004D45CC File Offset: 0x004D27CC
		private int GrowthTier
		{
			get
			{
				return this.child.ageTracker.GrowthTier;
			}
		}

		// Token: 0x170025CC RID: 9676
		// (get) Token: 0x0600D266 RID: 53862 RVA: 0x004D45DE File Offset: 0x004D27DE
		public override bool Visible
		{
			get
			{
				return this.child.IsColonistPlayerControlled || this.child.IsPrisonerOfColony || this.child.IsSlaveOfColony;
			}
		}

		// Token: 0x0600D267 RID: 53863 RVA: 0x004D4607 File Offset: 0x004D2807
		public override float GetWidth(float maxWidth)
		{
			return this.Width;
		}

		// Token: 0x0600D268 RID: 53864 RVA: 0x004D460F File Offset: 0x004D280F
		public Gizmo_GrowthTier(Pawn child)
		{
			this.child = child;
			this.Order = -100f;
		}

		// Token: 0x0600D269 RID: 53865 RVA: 0x004D462C File Offset: 0x004D282C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(8f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width, rect2.height / 2f);
			Rect rect4 = new Rect(rect2.x, rect3.yMax, rect2.width, rect3.height);
			rect3.yMax -= 2f;
			rect4.yMin += 2f;
			this.DrawGrowthTier(rect3);
			this.DrawLearning(rect4);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0600D26A RID: 53866 RVA: 0x004D46F0 File Offset: 0x004D28F0
		private string GrowthTierTooltip(int tier)
		{
			TaggedString taggedString = string.Concat(new string[]
			{
				("StatsReport_GrowthTier".Translate() + ": ").AsTipTitle(),
				tier.ToString(),
				"\n",
				"StatsReport_GrowthTierDesc".Translate().Colorize(ColoredText.SubtleGrayColor),
				"\n\n"
			});
			if (this.child.ageTracker.AtMaxGrowthTier)
			{
				taggedString += ("MaxTier".Translate() + ": ").AsTipTitle() + "MaxTierDesc".Translate(this.child.Named("PAWN"));
			}
			else
			{
				taggedString += ("ProgressToNextGrowthTier".Translate() + ": ").AsTipTitle() + Mathf.FloorToInt(this.child.ageTracker.growthPoints).ToString() + " / " + GrowthUtility.GrowthTiers[tier + 1].pointsRequirement.ToString();
				if (this.child.ageTracker.canGainGrowthPoints)
				{
					TaggedString t = taggedString;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
					defaultInterpolatedStringHandler.AppendLiteral(" (+");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PerDay".Translate(this.child.ageTracker.GrowthPointsPerDay.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute)));
					defaultInterpolatedStringHandler.AppendLiteral(")");
					taggedString = t + defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			if (this.child.ageTracker.AgeBiologicalYears < 13)
			{
				int num = 0;
				for (int i = this.child.ageTracker.AgeBiologicalYears + 1; i <= 13; i++)
				{
					if (GrowthUtility.IsGrowthBirthday(i))
					{
						num = i;
						break;
					}
				}
				taggedString += "\n\n" + ("NextGrowthMomentAt".Translate() + ": ").AsTipTitle() + num.ToString();
			}
			GrowthUtility.GrowthTier growthTier = GrowthUtility.GrowthTiers[tier];
			taggedString += "\n\n" + ("ThisGrowthTier".Translate(tier) + ":").AsTipTitle();
			if (growthTier.passionGainsRange.TrueMax > 0)
			{
				taggedString += "\n  - " + "NumPassionsFromOptions".Translate(growthTier.passionGainsRange.ToString(), growthTier.passionChoices);
			}
			taggedString += "\n  - " + "NumTraitsFromOptions".Translate(growthTier.traitGains, growthTier.traitChoices);
			if (!this.child.ageTracker.AtMaxGrowthTier)
			{
				GrowthUtility.GrowthTier growthTier2 = GrowthUtility.GrowthTiers[tier + 1];
				taggedString += "\n\n" + ("NextGrowthTier".Translate(tier + 1) + ":").AsTipTitle();
				if (growthTier2.passionGainsRange.TrueMax > 0)
				{
					taggedString += "\n  - " + "NumPassionsFromOptions".Translate(growthTier2.passionGainsRange.ToString(), growthTier2.passionChoices);
				}
				taggedString += "\n  - " + "NumTraitsFromOptions".Translate(growthTier2.traitGains, growthTier2.traitChoices);
			}
			return taggedString.Resolve();
		}

		// Token: 0x0600D26B RID: 53867 RVA: 0x004D4A9C File Offset: 0x004D2C9C
		private void DrawGrowthTier(Rect rect)
		{
			int growthTier = this.GrowthTier;
			Rect rect2 = rect;
			rect2.xMax = rect.x + rect.width * 0.55f;
			string label = "StatsReport_GrowthTier".Translate() + ": " + growthTier.ToString();
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, label);
			Text.Anchor = TextAnchor.UpperLeft;
			float percentToNextGrowthTier = this.child.ageTracker.PercentToNextGrowthTier;
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			rect3.yMin += 2f;
			rect3.yMax -= 2f;
			Widgets.FillableBar(rect3, percentToNextGrowthTier, Gizmo_GrowthTier.BarTex, Gizmo_GrowthTier.EmptyBarTex, true);
			Text.Anchor = TextAnchor.MiddleCenter;
			float pointsRequirement = GrowthUtility.GrowthTiers[GrowthUtility.GrowthTiers.Length - 1].pointsRequirement;
			string label2 = this.child.ageTracker.AtMaxGrowthTier ? (pointsRequirement.ToString() + " / " + pointsRequirement.ToString()) : (Mathf.FloorToInt(this.child.ageTracker.growthPoints).ToString() + " / " + GrowthUtility.GrowthTiers[growthTier + 1].pointsRequirement.ToString());
			Widgets.Label(rect3, label2);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, new TipSignal(this.GrowthTierTooltip(growthTier), this.child.thingIDNumber ^ 837825001));
			}
		}

		// Token: 0x0600D26C RID: 53868 RVA: 0x004D4C38 File Offset: 0x004D2E38
		private void DrawLearning(Rect rect)
		{
			if (this.child.needs.learning == null)
			{
				return;
			}
			Rect rect2 = rect;
			rect2.xMax = rect.x + rect.width * 0.55f;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect2, NeedDefOf.Learning.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			rect3.yMin += 2f;
			rect3.yMax -= 2f;
			Widgets.FillableBar(rect3, this.child.needs.learning.CurLevelPercentage, Widgets.BarFullTexHor, Gizmo_GrowthTier.EmptyBarTex, true);
			Text.Anchor = TextAnchor.MiddleCenter;
			string label = this.child.needs.learning.CurLevelPercentage.ToStringPercent();
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, this.child.needs.learning.GetTipString());
			}
		}

		// Token: 0x04008548 RID: 34120
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(GenUI.FillableBar_Empty);

		// Token: 0x04008549 RID: 34121
		private static readonly Texture2D BarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1254902f, 0.5411765f, 0f));

		// Token: 0x0400854A RID: 34122
		private const float Spacing = 8f;

		// Token: 0x0400854B RID: 34123
		private const float LabelWidthPercent = 0.55f;

		// Token: 0x0400854C RID: 34124
		private const float BarMarginY = 2f;

		// Token: 0x0400854D RID: 34125
		private const int GrowthTierTooltipId = 837825001;

		// Token: 0x0400854E RID: 34126
		private Pawn child;
	}
}
