using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200224B RID: 8779
	public abstract class QuestNode_Root_Gravcore : QuestNode
	{
		// Token: 0x0600D921 RID: 55585 RVA: 0x00506C44 File Offset: 0x00504E44
		protected virtual bool TryFindSiteTile(out PlanetTile tile)
		{
			PlanetLayer planetLayer = null;
			PlanetTile origin;
			if (!TileFinder.TryFindRandomPlayerTile(out origin, false, null, true, null))
			{
				Log.Error("Failed to find a valid root tile for gravcore site.");
				tile = PlanetTile.Invalid;
				return false;
			}
			if (this.layer != null && !Find.WorldGrid.TryGetFirstAdjacentLayerOfDef(origin, this.layer, out planetLayer))
			{
				tile = PlanetTile.Invalid;
				return false;
			}
			if (planetLayer == null)
			{
				planetLayer = origin.Layer;
			}
			int trueMin = this.distanceFromColonyRange.TrueMin;
			int trueMax = this.distanceFromColonyRange.TrueMax;
			FastTileFinder.LandmarkMode landmarkMode = (this.requiresLandmark || Rand.Chance(this.selectLandmarkChance)) ? FastTileFinder.LandmarkMode.Required : FastTileFinder.LandmarkMode.Forbidden;
			FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(origin, (float)trueMin, (float)trueMax, landmarkMode, true, this.minHilliness, this.maxHilliness, true, true, true);
			FastTileFinder.TileQueryParams desperate = new FastTileFinder.TileQueryParams(origin, 1f, (float)(trueMax * 2), FastTileFinder.LandmarkMode.Any, true, this.minHilliness, this.maxHilliness, false, true, true);
			List<PlanetTile> list = planetLayer.FastTileFinder.Query(query, this.allowedBiomes, this.allowedLandmarks, desperate);
			if (!list.Empty<PlanetTile>())
			{
				tile = list.RandomElement<PlanetTile>();
				return true;
			}
			query = new FastTileFinder.TileQueryParams(origin, (float)trueMin, (float)trueMax, landmarkMode, true, this.minHilliness, this.maxHilliness, true, true, true);
			desperate = new FastTileFinder.TileQueryParams(origin, 1f, float.MaxValue, FastTileFinder.LandmarkMode.Any, true, this.minHilliness, this.maxHilliness, false, true, true);
			list = planetLayer.FastTileFinder.Query(query, this.allowedBiomes, this.allowedLandmarks, desperate);
			if (!list.Empty<PlanetTile>())
			{
				tile = list.RandomElement<PlanetTile>();
				return true;
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x0600D922 RID: 55586 RVA: 0x00506DD8 File Offset: 0x00504FD8
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			return ModsConfig.OdysseyActive && this.TryFindSiteTile(out planetTile);
		}

		// Token: 0x04008ACB RID: 35531
		protected List<BiomeDef> allowedBiomes = new List<BiomeDef>();

		// Token: 0x04008ACC RID: 35532
		private Hilliness minHilliness;

		// Token: 0x04008ACD RID: 35533
		private Hilliness maxHilliness;

		// Token: 0x04008ACE RID: 35534
		protected IntRange distanceFromColonyRange = new IntRange(20, 60);

		// Token: 0x04008ACF RID: 35535
		protected List<LandmarkDef> allowedLandmarks;

		// Token: 0x04008AD0 RID: 35536
		protected float selectLandmarkChance = 0.5f;

		// Token: 0x04008AD1 RID: 35537
		public int requiredSubquestsGiven;

		// Token: 0x04008AD2 RID: 35538
		private bool requiresLandmark;

		// Token: 0x04008AD3 RID: 35539
		protected PlanetLayerDef layer;

		// Token: 0x04008AD4 RID: 35540
		protected const string SiteAlias = "site";
	}
}
