using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000235 RID: 565
	public static class AnimalPenGUI
	{
		// Token: 0x06001080 RID: 4224 RVA: 0x0005ED74 File Offset: 0x0005CF74
		public static void DoAllowedAreaMessage(Rect rect, Pawn pawn)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
			TaggedString taggedString;
			TaggedString str;
			if (currentPenOf != null)
			{
				taggedString = "InPen".Translate() + ": " + currentPenOf.label;
				str = taggedString;
			}
			else
			{
				GUI.color = Color.gray;
				taggedString = "(" + "Unpenned".Translate() + ")";
				str = "UnpennedTooltip".Translate();
			}
			Widgets.Label(rect, taggedString);
			TooltipHandler.TipRegion(rect, str);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0005EE1C File Offset: 0x0005D01C
		public static void DrawPlacingMouseAttachments(IntVec3 mouseCell, Map map, PenFoodCalculator calc)
		{
			AnimalPenGUI.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.calc = calc;
			CS$<>8__locals1.sb = new StringBuilder();
			Vector2 location = Find.WorldGrid.LongLatOf(map.Tile);
			CS$<>8__locals1.summerQuadrum = CS$<>8__locals1.calc.GetSummerOrBestQuadrum();
			CS$<>8__locals1.summerLabel = CS$<>8__locals1.summerQuadrum.GetSeason(location);
			CS$<>8__locals1.sb.AppendLine(CS$<>8__locals1.calc.PenSizeDescription());
			CS$<>8__locals1.sb.AppendLine("PenExampleAnimals".Translate() + ":");
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Cow, ref CS$<>8__locals1);
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Goat, ref CS$<>8__locals1);
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Chicken, ref CS$<>8__locals1);
			Widgets.MouseAttachedLabel(CS$<>8__locals1.sb.ToString().TrimEnd(), 8f, 35f, null);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0005EEFC File Offset: 0x0005D0FC
		[CompilerGenerated]
		internal static void <DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDef animalDef, ref AnimalPenGUI.<>c__DisplayClass3_0 A_1)
		{
			A_1.sb.Append("PenCapacityDesc".Translate(animalDef.Named("ANIMALDEF")).CapitalizeFirst());
			A_1.sb.Append(" (").Append(A_1.summerLabel.Label()).Append("): ");
			if (A_1.calc.Unenclosed)
			{
				A_1.sb.Append("?");
			}
			else
			{
				A_1.sb.Append(A_1.calc.CapacityOf(A_1.summerQuadrum, animalDef).ToString("F1"));
			}
			A_1.sb.AppendLine();
		}

		// Token: 0x020025B1 RID: 9649
		public class PenPainter : AnimalPenEnclosureCalculator
		{
			// Token: 0x0600F085 RID: 61573 RVA: 0x00588408 File Offset: 0x00586608
			protected override void VisitDirectlyConnectedRegion(Region r)
			{
				this.directEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x0600F086 RID: 61574 RVA: 0x0058841B File Offset: 0x0058661B
			protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
			{
				this.indirectEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x0600F087 RID: 61575 RVA: 0x0058842E File Offset: 0x0058662E
			protected override void VisitPassableDoorway(Region r)
			{
				this.openDoorEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x0600F088 RID: 61576 RVA: 0x00588444 File Offset: 0x00586644
			public void Paint(IntVec3 position, Map map)
			{
				this.directEdgeCells.Clear();
				this.indirectEdgeCells.Clear();
				this.openDoorEdgeCells.Clear();
				if (base.VisitPen(position, map))
				{
					GenDraw.DrawFieldEdges(this.directEdgeCells, Color.green, null, null, 2900);
					GenDraw.DrawFieldEdges(this.indirectEdgeCells, Color.white, null, null, 2900);
					GenDraw.DrawFieldEdges(this.openDoorEdgeCells, Color.white, null, null, 2900);
					return;
				}
				GenDraw.DrawFieldEdges(this.openDoorEdgeCells, Color.red, null, null, 2900);
			}

			// Token: 0x04009A98 RID: 39576
			private readonly List<IntVec3> directEdgeCells = new List<IntVec3>();

			// Token: 0x04009A99 RID: 39577
			private readonly List<IntVec3> indirectEdgeCells = new List<IntVec3>();

			// Token: 0x04009A9A RID: 39578
			private readonly List<IntVec3> openDoorEdgeCells = new List<IntVec3>();
		}

		// Token: 0x020025B2 RID: 9650
		public class PenBlueprintPainter
		{
			// Token: 0x0600F08A RID: 61578 RVA: 0x00588524 File Offset: 0x00586724
			public void Paint(IntVec3 position, Map map)
			{
				this.filler.VisitPen(position, map);
				if (this.filler.isEnclosed)
				{
					GenDraw.DrawFieldEdges(this.filler.cellsFound, Color.white, null, null, 2900);
				}
			}

			// Token: 0x04009A9B RID: 39579
			private AnimalPenBlueprintEnclosureCalculator filler = new AnimalPenBlueprintEnclosureCalculator();
		}
	}
}
