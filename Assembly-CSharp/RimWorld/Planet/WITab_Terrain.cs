using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023F6 RID: 9206
	public class WITab_Terrain : WITab
	{
		// Token: 0x1700289D RID: 10397
		// (get) Token: 0x0600E61B RID: 58907 RVA: 0x00558BE4 File Offset: 0x00556DE4
		public override bool IsVisible
		{
			get
			{
				return base.SelPlanetTile.Valid && base.SelPlanetTile.Tile.OnSurface;
			}
		}

		// Token: 0x0600E61C RID: 58908 RVA: 0x00558C16 File Offset: 0x00556E16
		public WITab_Terrain()
		{
			this.size = WITab_Terrain.WinSize;
			this.labelKey = "TabTerrain";
			this.tutorTag = "Terrain";
		}

		// Token: 0x0600E61D RID: 58909 RVA: 0x00558C40 File Offset: 0x00556E40
		protected override void FillTab()
		{
			Rect outRect = new Rect(0f, 0f, WITab_Terrain.WinSize.x, WITab_Terrain.WinSize.y).ContractedBy(10f);
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, base.SelTile.PrimaryBiome.LabelCap);
			Rect rect3 = rect2;
			rect3.yMin += 35f;
			rect3.height = 99999f;
			Text.Font = GameFont.Small;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.verticalSpacing = 0f;
			listing_Standard.Begin(rect3);
			this.DrawScrollContents(listing_Standard, rect3);
			listing_Standard.End();
			Widgets.EndScrollView();
		}

		// Token: 0x0600E61E RID: 58910 RVA: 0x00558D28 File Offset: 0x00556F28
		private void DrawScrollContents(Listing_Standard listing, Rect infoRect)
		{
			Tile selTile = base.SelTile;
			PlanetTile selPlanetTile = base.SelPlanetTile;
			listing.Label(selTile.PrimaryBiome.description, -1f, null);
			listing.Gap(8f);
			listing.GapLine(12f);
			if (!selTile.PrimaryBiome.implemented)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(selTile.PrimaryBiome.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("BiomeNotImplemented".Translate());
				listing.Label(defaultInterpolatedStringHandler.ToStringAndClear(), -1f, null);
			}
			WITab_Terrain.ListGeometricDetails(listing, selTile, selPlanetTile);
			listing.GapLine(12f);
			WITab_Terrain.ListTemperatureDetails(listing, selPlanetTile, selTile);
			WITab_Terrain.ListPollutionDetails(listing, selPlanetTile);
			listing.GapLine(12f);
			WITab_Terrain.ListMiscDetails(listing, selTile, selPlanetTile);
			this.lastDrawnHeight = infoRect.y + listing.CurHeight;
		}

		// Token: 0x0600E61F RID: 58911 RVA: 0x00558E24 File Offset: 0x00557024
		private static void ListMiscDetails(Listing_Standard listing, Tile ws, PlanetTile tile)
		{
			string leftLabel = "AverageDiseaseFrequency".Translate();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<float>(60f / ws.PrimaryBiome.diseaseMtbDays, "F1");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PerYear".Translate());
			listing.LabelDouble(leftLabel, defaultInterpolatedStringHandler.ToStringAndClear(), null);
			listing.LabelDouble("TimeZone".Translate(), GenDate.TimeZoneAt(Find.WorldGrid.LongLatOf(tile).x).ToStringWithSign(), null);
			if (Prefs.DevMode)
			{
				listing.LabelDouble("Debug world tile ID", tile.ToString(), null);
			}
		}

		// Token: 0x0600E620 RID: 58912 RVA: 0x00558EE4 File Offset: 0x005570E4
		private static void ListPollutionDetails(Listing_Standard listing, PlanetTile tileID)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			listing.GapLine(12f);
			listing.LabelDouble("TilePollution".Translate(), Find.WorldGrid[tileID].pollution.ToStringPercent(), "TerrainPollutionTip".Translate());
			string text = "";
			foreach (IGrouping<float, CurvePoint> grouping in from p in WorldPollutionUtility.NearbyPollutionOverDistanceCurve
			group p by p.y)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				if (grouping.Count<CurvePoint>() > 1)
				{
					CurvePoint curvePoint = grouping.MinBy((CurvePoint p) => p.x);
					CurvePoint curvePoint2 = grouping.MaxBy((CurvePoint p) => p.x);
					string str = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 5);
					defaultInterpolatedStringHandler.AppendLiteral(" - ");
					defaultInterpolatedStringHandler.AppendFormatted<float>(curvePoint.x);
					defaultInterpolatedStringHandler.AppendLiteral("-");
					defaultInterpolatedStringHandler.AppendFormatted<float>(curvePoint2.x);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("NearbyPollutionTilesAway".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					defaultInterpolatedStringHandler.AppendFormatted<float>(grouping.Key);
					defaultInterpolatedStringHandler.AppendLiteral("x ");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PollutionLower".Translate());
					text = str + defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					string str2 = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(8, 4);
					defaultInterpolatedStringHandler2.AppendLiteral(" - ");
					defaultInterpolatedStringHandler2.AppendFormatted<float>(grouping.First<CurvePoint>().x);
					defaultInterpolatedStringHandler2.AppendLiteral(" ");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("NearbyPollutionTilesAway".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(", ");
					defaultInterpolatedStringHandler2.AppendFormatted<float>(grouping.Key);
					defaultInterpolatedStringHandler2.AppendLiteral("x ");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("PollutionLower".Translate());
					text = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
				}
			}
			TaggedString taggedString = "NearbyPollutionTip".Translate(4, text);
			float num = WorldPollutionUtility.CalculateNearbyPollutionScore(tileID);
			if (num >= GameConditionDefOf.NoxiousHaze.minNearbyPollution)
			{
				float value = GameConditionDefOf.NoxiousHaze.mtbOverNearbyPollutionCurve.Evaluate(num);
				taggedString += "\n\n" + "NoxiousHazeInterval".Translate(value);
			}
			else
			{
				taggedString += "\n\n" + "NoxiousHazeNeverOccurring".Translate();
			}
			listing.LabelDouble("TilePollutionNearby".Translate(), WorldPollutionUtility.CalculateNearbyPollutionScore(tileID).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute), taggedString);
		}

		// Token: 0x0600E621 RID: 58913 RVA: 0x005591FC File Offset: 0x005573FC
		private static void ListTemperatureDetails(Listing_Standard listing, PlanetTile tile, Tile ws)
		{
			listing.LabelDouble("AvgTemp".Translate(), GenTemperature.GetAverageTemperatureLabel(tile), null);
			string rightLabel = WITab_Terrain.cachedGrowingQuadrumsDescription;
			if (WITab_Terrain.cachedGrowingQuadrumsTile != tile)
			{
				rightLabel = (WITab_Terrain.cachedGrowingQuadrumsDescription = Zone_Growing.GrowingQuadrumsDescription(tile, null));
				WITab_Terrain.cachedGrowingQuadrumsTile = tile;
			}
			listing.LabelDouble("OutdoorGrowingPeriod".Translate(), rightLabel, null);
			listing.LabelDouble("Rainfall".Translate(), ws.rainfall.ToString("F0") + "mm", null);
			if (ws.PrimaryBiome.foragedFood != null && ws.PrimaryBiome.forageability > 0f)
			{
				listing.LabelDouble("Forageability".Translate(), ws.PrimaryBiome.forageability.ToStringPercent() + " (" + ws.PrimaryBiome.foragedFood.label + ")", null);
			}
			else
			{
				listing.LabelDouble("Forageability".Translate(), "0%", null);
			}
			listing.LabelDouble("AnimalsCanGrazeNow".Translate(), VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsNowAt(tile) ? "Yes".Translate() : "No".Translate(), null);
		}

		// Token: 0x0600E622 RID: 58914 RVA: 0x00559350 File Offset: 0x00557550
		private static void ListGeometricDetails(Listing_Standard listing, Tile ws, PlanetTile tile)
		{
			if (ws.HillinessLabel != Hilliness.Undefined)
			{
				listing.LabelDouble("Terrain".Translate(), ws.HillinessLabel.GetLabelCap(), null);
			}
			SurfaceTile surfaceTile = ws as SurfaceTile;
			if (surfaceTile != null)
			{
				if (surfaceTile.Roads != null)
				{
					listing.LabelDouble("Road".Translate(), (from roadlink in surfaceTile.Roads
					select roadlink.road.label).Distinct<string>().ToCommaList(true, false).CapitalizeFirst(), null);
				}
				if (surfaceTile.Rivers != null)
				{
					listing.LabelDouble("River".Translate(), surfaceTile.Rivers.MaxBy((SurfaceTile.RiverLink riverlink) => riverlink.river.degradeThreshold).river.LabelCap, null);
				}
			}
			if (!Find.World.Impassable(tile))
			{
				StringBuilder stringBuilder = new StringBuilder();
				PlanetTile tile2 = tile;
				bool perceivedStatic = false;
				StringBuilder explanation = stringBuilder;
				string rightLabel = (WorldPathGrid.CalculatedMovementDifficultyAt(tile2, perceivedStatic, null, explanation) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(tile, PlanetTile.Invalid, stringBuilder)).ToString("0.#");
				if (WorldPathGrid.WillWinterEverAffectMovementDifficulty(tile) && WorldPathGrid.GetCurrentWinterMovementDifficultyOffset(tile, null, null) < 2f)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append(" (");
					StringBuilder stringBuilder2 = stringBuilder;
					string key = "MovementDifficultyOffsetInWinter";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
					defaultInterpolatedStringHandler.AppendLiteral("+");
					defaultInterpolatedStringHandler.AppendFormatted<float>(2f, "0.#");
					stringBuilder2.Append(key.Translate(defaultInterpolatedStringHandler.ToStringAndClear()));
					stringBuilder.Append(")");
				}
				listing.LabelDouble("MovementDifficulty".Translate(), rightLabel, stringBuilder.ToString());
			}
			if (ws.PrimaryBiome.canBuildBase)
			{
				listing.LabelDouble("StoneTypesHere".Translate(), (from rt in Find.World.NaturalRockTypesIn(tile)
				select rt.label).ToCommaList(true, false).CapitalizeFirst(), null);
			}
			listing.LabelDouble("Elevation".Translate(), ws.Layer.Def.elevationString.Formatted(ws.elevation.ToString("F0")), null);
			if (ModsConfig.OdysseyActive && ws.Landmark != null)
			{
				listing.LabelDouble("Landmark".Translate(), ws.Landmark.name, ws.Landmark.def.description);
			}
			if (ws.Mutators.Any<TileMutatorDef>())
			{
				IOrderedEnumerable<TileMutatorDef> source = from m in ws.Mutators
				orderby -m.displayPriority
				select m;
				listing.LabelDouble("TileMutators".Translate(), (from m in source
				select m.Label(tile)).ToCommaList(false, false).CapitalizeFirst(), (from m in source
				select m.Label(tile).Colorize(ColoredText.TipSectionTitleColor).CapitalizeFirst() + "\n" + m.Description(tile)).ToStringList("\n\n"));
			}
		}

		// Token: 0x04009247 RID: 37447
		private Vector2 scrollPosition;

		// Token: 0x04009248 RID: 37448
		private float lastDrawnHeight;

		// Token: 0x04009249 RID: 37449
		private static string cachedGrowingQuadrumsDescription;

		// Token: 0x0400924A RID: 37450
		private static PlanetTile cachedGrowingQuadrumsTile;

		// Token: 0x0400924B RID: 37451
		private static readonly Vector2 WinSize = new Vector2(440f, 540f);
	}
}
