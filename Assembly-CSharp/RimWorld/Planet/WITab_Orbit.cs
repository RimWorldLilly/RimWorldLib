using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023F4 RID: 9204
	public class WITab_Orbit : WITab
	{
		// Token: 0x1700289A RID: 10394
		// (get) Token: 0x0600E60F RID: 58895 RVA: 0x00558774 File Offset: 0x00556974
		public override bool IsVisible
		{
			get
			{
				return ModsConfig.OdysseyActive && base.SelPlanetTile.Valid && base.SelPlanetTile.LayerDef == PlanetLayerDefOf.Orbit;
			}
		}

		// Token: 0x0600E610 RID: 58896 RVA: 0x005587AF File Offset: 0x005569AF
		public WITab_Orbit()
		{
			this.size = WITab_Orbit.WinSize;
			this.labelKey = "TabOrbit";
			this.tutorTag = "Orbit";
		}

		// Token: 0x0600E611 RID: 58897 RVA: 0x005587D8 File Offset: 0x005569D8
		protected override void FillTab()
		{
			Rect outRect = new Rect(0f, 0f, WITab_Orbit.WinSize.x, WITab_Orbit.WinSize.y).ContractedBy(10f);
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

		// Token: 0x0600E612 RID: 58898 RVA: 0x005588C0 File Offset: 0x00556AC0
		private void DrawScrollContents(Listing_Standard listing, Rect infoRect)
		{
			Tile selTile = base.SelTile;
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
			WITab_Orbit.ListOrbitalDetails(listing, selTile, base.SelPlanetTile);
			listing.GapLine(12f);
			WITab_Orbit.ListMiscDetails(listing, selTile, base.SelPlanetTile);
			this.lastDrawnHeight = infoRect.y + listing.CurHeight;
		}

		// Token: 0x0600E613 RID: 58899 RVA: 0x005589A4 File Offset: 0x00556BA4
		private static void ListOrbitalDetails(Listing_Standard listing, Tile ws, PlanetTile tile)
		{
			listing.LabelDouble("Elevation".Translate(), ws.Layer.Def.elevationString.Formatted(ws.elevation.ToString("F0")), null);
			listing.LabelDouble("AvgTemp".Translate(), GenTemperature.GetAverageTemperatureLabel(tile), null);
		}

		// Token: 0x0600E614 RID: 58900 RVA: 0x00558A14 File Offset: 0x00556C14
		private static void ListMiscDetails(Listing_Standard listing, Tile ws, PlanetTile tile)
		{
			listing.LabelDouble("TimeZone".Translate(), GenDate.TimeZoneAt(Find.WorldGrid.LongLatOf(tile).x).ToStringWithSign(), null);
			if (Prefs.DevMode)
			{
				listing.LabelDouble("Debug world tile ID", tile.ToString(), null);
			}
		}

		// Token: 0x04009243 RID: 37443
		private Vector2 scrollPosition;

		// Token: 0x04009244 RID: 37444
		private float lastDrawnHeight;

		// Token: 0x04009245 RID: 37445
		private static readonly Vector2 WinSize = new Vector2(440f, 540f);
	}
}
