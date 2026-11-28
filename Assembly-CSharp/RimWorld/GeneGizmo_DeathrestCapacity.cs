using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200207C RID: 8316
	public class GeneGizmo_DeathrestCapacity : Gizmo
	{
		// Token: 0x0600D1A1 RID: 53665 RVA: 0x004D1082 File Offset: 0x004CF282
		public override float GetWidth(float maxWidth)
		{
			return 140f;
		}

		// Token: 0x0600D1A2 RID: 53666 RVA: 0x004D1089 File Offset: 0x004CF289
		public GeneGizmo_DeathrestCapacity(Gene_Deathrest gene)
		{
			this.gene = gene;
			this.Order = -100f;
		}

		// Token: 0x0600D1A3 RID: 53667 RVA: 0x004D10A4 File Offset: 0x004CF2A4
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect position = rect.ContractedBy(6f);
			float num = position.height / 3f;
			Widgets.DrawWindowBackground(rect);
			GUI.BeginGroup(position);
			Widgets.Label(new Rect(0f, 0f, position.width, num), "Deathrest".Translate().CapitalizeFirst());
			if (this.gene.DeathrestNeed != null)
			{
				this.gene.DeathrestNeed.DrawOnGUI(new Rect(0f, num, position.width, num + 2f), int.MaxValue, 2f, false, true, new Rect?(new Rect(0f, 0f, position.width, num * 2f)), false);
			}
			Rect rect2 = new Rect(0f, num * 2f, position.width, Text.LineHeight);
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Buildings".Translate().CapitalizeFirst());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.gene.CurrentCapacity);
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.gene.DeathrestCapacity);
			Widgets.Label(rect3, defaultInterpolatedStringHandler.ToStringAndClear());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect2, "DeathrestCapacityDesc".Translate() + "\n\n" + "PawnIsConnectedToBuildings".Translate(this.gene.pawn.Named("PAWN"), this.gene.CurrentCapacity.Named("CURRENT"), this.gene.DeathrestCapacity.Named("MAX")));
			}
			GUI.EndGroup();
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x040084FF RID: 34047
		protected Gene_Deathrest gene;

		// Token: 0x04008500 RID: 34048
		private const float Padding = 6f;

		// Token: 0x04008501 RID: 34049
		private const float Width = 140f;
	}
}
