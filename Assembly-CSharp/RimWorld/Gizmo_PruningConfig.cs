using System;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019CC RID: 6604
	[StaticConstructorOnStartup]
	public class Gizmo_PruningConfig : Gizmo
	{
		// Token: 0x17001CFD RID: 7421
		// (get) Token: 0x0600A60E RID: 42510 RVA: 0x003B39C4 File Offset: 0x003B1BC4
		private float DesiredConnectionStrength
		{
			get
			{
				if (!this.draggingBar)
				{
					return this.connection.DesiredConnectionStrength;
				}
				return this.selectedStrengthTarget;
			}
		}

		// Token: 0x17001CFE RID: 7422
		// (get) Token: 0x0600A60F RID: 42511 RVA: 0x003B39E0 File Offset: 0x003B1BE0
		private float OverrideHeight
		{
			get
			{
				return 75f + this.ExtraHeight;
			}
		}

		// Token: 0x0600A610 RID: 42512 RVA: 0x003B39EE File Offset: 0x003B1BEE
		public Gizmo_PruningConfig(CompTreeConnection connection)
		{
			this.connection = connection;
			this.Order = -100f;
		}

		// Token: 0x0600A611 RID: 42513 RVA: 0x003B3A24 File Offset: 0x003B1C24
		public override float GetWidth(float maxWidth)
		{
			return 212f;
		}

		// Token: 0x0600A612 RID: 42514 RVA: 0x003B3A2C File Offset: 0x003B1C2C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return new GizmoResult(GizmoState.Clear);
			}
			Rect rect = new Rect(topLeft.x, topLeft.y - this.ExtraHeight, this.GetWidth(maxWidth), this.OverrideHeight);
			Rect rect2 = rect.ContractedBy(6f);
			Widgets.DrawWindowBackground(rect);
			Rect rect3 = rect2;
			float yMin = rect3.yMin;
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3.x, ref yMin, rect3.width, "ConnectionStrength".Translate(), default(TipSignal));
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect3.x, ref yMin, rect3.width, "DesiredConnectionStrength".Translate() + ": " + this.DesiredConnectionStrength.ToStringPercent(), default(TipSignal));
			Widgets.Label(rect3.x, ref yMin, rect3.width, "PruningHoursToMaintain".Translate() + ": " + this.connection.PruningHoursToMaintain(this.DesiredConnectionStrength).ToString("F1"), default(TipSignal));
			Text.Font = GameFont.Small;
			if (Mouse.IsOver(rect2) && !this.draggingBar)
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect2, () => this.GetTip(), 9493937);
			}
			this.DrawBar(rect2, yMin);
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0600A613 RID: 42515 RVA: 0x003B3BB0 File Offset: 0x003B1DB0
		private string GetTip()
		{
			string text = "DesiredConnectionStrengthDesc".Translate(this.connection.parent.Named("TREE"), this.connection.ConnectedPawn.Named("CONNECTEDPAWN"), this.connection.ConnectionStrengthLossPerDay.ToStringPercent().Named("FALL")).Resolve();
			string text2 = this.connection.AffectingBuildingsDescription("CurrentlyAffectedBy");
			if (!text2.NullOrEmpty())
			{
				text = text + "\n\n" + text2;
			}
			return text;
		}

		// Token: 0x0600A614 RID: 42516 RVA: 0x003B3C3C File Offset: 0x003B1E3C
		private void DrawThreshold(Rect rect, float percent, float strValue)
		{
			Rect position = new Rect
			{
				x = rect.x + 3f + (rect.width - 8f) * percent,
				y = rect.y + rect.height - 9f,
				width = 2f,
				height = 6f
			};
			if (strValue < percent)
			{
				GUI.DrawTexture(position, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(position, BaseContent.BlackTex);
		}

		// Token: 0x0600A615 RID: 42517 RVA: 0x003B3CC8 File Offset: 0x003B1EC8
		private void DrawStrengthTarget(Rect rect, float percent)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			GUI.DrawTexture(new Rect(rect.x + 3f + num, rect.y, 2f, rect.height), Gizmo_PruningConfig.StrengthTargetTex);
			float num2 = UIScaling.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = UIScaling.AdjustCoordToUIScalingCeil(num2 + 4f);
			Rect rect2 = new Rect
			{
				y = rect.y - 3f,
				height = 5f,
				xMin = num2,
				xMax = xMax
			};
			GUI.DrawTexture(rect2, Gizmo_PruningConfig.StrengthTargetTex);
			Rect position = rect2;
			position.y = rect.yMax - 2f;
			GUI.DrawTexture(position, Gizmo_PruningConfig.StrengthTargetTex);
		}

		// Token: 0x0600A616 RID: 42518 RVA: 0x003B3DA4 File Offset: 0x003B1FA4
		private void DrawBar(Rect inRect, float curY)
		{
			Rect rect = inRect;
			rect.xMin += 10f;
			rect.xMax -= 10f;
			rect.yMax = inRect.yMax - 4f;
			rect.yMin = curY + 10f;
			bool flag = Mouse.IsOver(rect);
			float connectionStrength = this.connection.ConnectionStrength;
			Widgets.FillableBar(rect, connectionStrength, flag ? Gizmo_PruningConfig.StrengthHighlightTex : Gizmo_PruningConfig.StrengthTex, Gizmo_PruningConfig.EmptyBarTex, true);
			foreach (CurvePoint curvePoint in this.connection.Props.maxDryadsPerConnectionStrengthCurve.Points)
			{
				if (curvePoint.x > 0f)
				{
					this.DrawThreshold(rect, curvePoint.x, connectionStrength);
				}
			}
			float num = Mathf.Clamp(Mathf.Round((Event.current.mousePosition.x - (rect.x + 3f)) / (rect.width - 8f) * 20f) / 20f, 0f, 1f);
			Event current = Event.current;
			if (current.type == EventType.mouseDown && current.button == 0 && flag)
			{
				this.selectedStrengthTarget = num;
				this.draggingBar = true;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				current.Use();
			}
			if (current.type == EventType.mouseDrag && current.button == 0 && this.draggingBar && flag)
			{
				if (Mathf.Abs(num - this.selectedStrengthTarget) > 1E-45f)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				this.selectedStrengthTarget = num;
				current.Use();
			}
			if (current.type == EventType.mouseUp && current.button == 0 && this.draggingBar)
			{
				if (this.selectedStrengthTarget >= 0f)
				{
					this.connection.DesiredConnectionStrength = this.selectedStrengthTarget;
				}
				this.selectedStrengthTarget = -1f;
				this.draggingBar = false;
				current.Use();
			}
			this.DrawStrengthTarget(rect, this.DesiredConnectionStrength);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.connection.ConnectionStrength.ToStringPercent());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x040061FA RID: 25082
		private CompTreeConnection connection;

		// Token: 0x040061FB RID: 25083
		private float selectedStrengthTarget = -1f;

		// Token: 0x040061FC RID: 25084
		private bool draggingBar;

		// Token: 0x040061FD RID: 25085
		private const float Width = 212f;

		// Token: 0x040061FE RID: 25086
		private static readonly Texture2D StrengthTex = SolidColorMaterials.NewSolidColorTexture(ColorLibrary.Orange);

		// Token: 0x040061FF RID: 25087
		private static readonly Texture2D StrengthHighlightTex = SolidColorMaterials.NewSolidColorTexture(ColorLibrary.LightOrange);

		// Token: 0x04006200 RID: 25088
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x04006201 RID: 25089
		private static readonly Texture2D StrengthTargetTex = SolidColorMaterials.NewSolidColorTexture(ColorLibrary.DarkOrange);

		// Token: 0x04006202 RID: 25090
		private float ExtraHeight = Text.LineHeight * 1.5f;
	}
}
