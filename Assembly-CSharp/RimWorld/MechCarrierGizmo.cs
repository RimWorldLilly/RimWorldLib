using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EB RID: 6379
	[StaticConstructorOnStartup]
	public class MechCarrierGizmo : Gizmo
	{
		// Token: 0x06009FEF RID: 40943 RVA: 0x00394C10 File Offset: 0x00392E10
		public MechCarrierGizmo(CompMechCarrier carrier)
		{
			this.carrier = carrier;
			this.targetValue = (float)carrier.maxToFill / (float)carrier.Props.maxIngredientCount;
			if (MechCarrierGizmo.bandPercentages == null)
			{
				MechCarrierGizmo.bandPercentages = new List<float>();
				int num = 12;
				for (int i = 0; i <= num; i++)
				{
					float item = 1f / (float)num * (float)i;
					MechCarrierGizmo.bandPercentages.Add(item);
				}
			}
		}

		// Token: 0x06009FF0 RID: 40944 RVA: 0x00394C7B File Offset: 0x00392E7B
		public override float GetWidth(float maxWidth)
		{
			return 160f;
		}

		// Token: 0x06009FF1 RID: 40945 RVA: 0x00394C84 File Offset: 0x00392E84
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(10f);
			Widgets.DrawWindowBackground(rect);
			Text.Font = GameFont.Small;
			TaggedString labelCap = this.carrier.Props.fixedIngredient.LabelCap;
			float height = Text.CalcHeight(labelCap, rect2.width);
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width, height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, labelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			this.lastTargetValue = this.targetValue;
			float num = rect2.height - rect3.height;
			float num2 = num - 4f;
			float num3 = (num - num2) / 2f;
			Rect rect4 = new Rect(rect2.x, rect3.yMax + num3, rect2.width, num2);
			Widgets.DraggableBar(rect4, MechCarrierGizmo.BarTex, MechCarrierGizmo.BarHighlightTex, MechCarrierGizmo.EmptyBarTex, MechCarrierGizmo.DragBarTex, ref MechCarrierGizmo.draggingBar, this.carrier.PercentageFull, ref this.targetValue, MechCarrierGizmo.bandPercentages, 24, 0f, 1f);
			Text.Anchor = TextAnchor.MiddleCenter;
			rect4.y -= 2f;
			Widgets.Label(rect4, this.carrier.IngredientCount.ToString() + " / " + this.carrier.Props.maxIngredientCount.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect4, () => this.GetResourceBarTip(), Gen.HashCombineInt(this.carrier.GetHashCode(), 34242369));
			if (this.lastTargetValue != this.targetValue)
			{
				this.carrier.maxToFill = Mathf.RoundToInt(this.targetValue * (float)this.carrier.Props.maxIngredientCount);
			}
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06009FF2 RID: 40946 RVA: 0x00394E70 File Offset: 0x00393070
		private string GetResourceBarTip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MechCarrierAutofillResources".Translate() + " " + this.carrier.Props.fixedIngredient.label + ": " + this.carrier.maxToFill.ToString());
			stringBuilder.AppendInNewLine("MechCarrierClickToSetAutofillAmount".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendInNewLine("MechCarrierAutofillDesc".Translate(this.carrier.parent.def.label, this.carrier.Props.spawnPawnKind.labelPlural));
			return stringBuilder.ToString();
		}

		// Token: 0x04005E8F RID: 24207
		private CompMechCarrier carrier;

		// Token: 0x04005E90 RID: 24208
		private const float Width = 160f;

		// Token: 0x04005E91 RID: 24209
		private static readonly Texture2D BarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

		// Token: 0x04005E92 RID: 24210
		private static readonly Texture2D BarHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

		// Token: 0x04005E93 RID: 24211
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x04005E94 RID: 24212
		private static readonly Texture2D DragBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));

		// Token: 0x04005E95 RID: 24213
		private const int Increments = 24;

		// Token: 0x04005E96 RID: 24214
		private static bool draggingBar;

		// Token: 0x04005E97 RID: 24215
		private float lastTargetValue;

		// Token: 0x04005E98 RID: 24216
		private float targetValue;

		// Token: 0x04005E99 RID: 24217
		private static List<float> bandPercentages;
	}
}
