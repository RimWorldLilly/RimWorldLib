using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002296 RID: 8854
	public class QuestNode_Root_Site : QuestNode
	{
		// Token: 0x0600DAA2 RID: 55970 RVA: 0x00516230 File Offset: 0x00514430
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(slate, out tile))
			{
				Log.Error("Could not find valid site tile.");
				return;
			}
			Faction faction = (this.factionDef != null) ? Find.FactionManager.FirstFactionOfDef(this.factionDef.GetValue(slate)) : Faction.OfAncientsHostile;
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(this.sitePartDef.GetValue(slate), new SitePartParams
				{
					points = slate.Get<float>("points", 0f, false),
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, faction, false, null, this.worldObjectDef.GetValue(slate));
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
		}

		// Token: 0x0600DAA3 RID: 55971 RVA: 0x00516310 File Offset: 0x00514510
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			return this.TryFindSiteTile(slate, out planetTile);
		}

		// Token: 0x0600DAA4 RID: 55972 RVA: 0x0051632C File Offset: 0x0051452C
		protected virtual bool TryGetLayer(Slate slate, out PlanetTile source, out PlanetLayer layer)
		{
			QuestNode_Root_Site.<>c__DisplayClass18_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.slate = slate;
			layer = null;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map != null && map.Tile.Valid)
			{
				source = map.Tile;
			}
			else if (!TileFinder.TryFindRandomPlayerTile(out source, false, null, true, null))
			{
				source = Find.WorldGrid.Surface.Tiles.RandomElement<Tile>().tile;
			}
			if (this.<TryGetLayer>g__Validator|18_0(source, source.Layer, ref CS$<>8__locals1))
			{
				layer = source.Layer;
			}
			else
			{
				foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers.InRandomOrder(null))
				{
					int num;
					PlanetLayer planetLayer;
					keyValuePair.Deconstruct(out num, out planetLayer);
					PlanetLayer planetLayer2 = planetLayer;
					if (planetLayer2 != source.Layer && this.<TryGetLayer>g__Validator|18_0(source, planetLayer2, ref CS$<>8__locals1))
					{
						layer = planetLayer2;
						break;
					}
				}
			}
			return layer != null;
		}

		// Token: 0x0600DAA5 RID: 55973 RVA: 0x00516444 File Offset: 0x00514644
		protected virtual bool TryFindSiteTile(Slate slate, out PlanetTile tile)
		{
			PlanetTile planetTile;
			PlanetLayer planetLayer;
			if (!this.TryGetLayer(slate, out planetTile, out planetLayer))
			{
				tile = PlanetTile.Invalid;
				return false;
			}
			Hilliness valueOrDefault = this.maxHilliness.GetValue(slate).GetValueOrDefault(Hilliness.Mountainous);
			int trueMin = this.distanceFromColonyRange.GetValue(slate).TrueMin;
			int trueMax = this.distanceFromColonyRange.GetValue(slate).TrueMax;
			bool valueOrDefault2 = this.requiresLandmark.GetValue(slate).GetValueOrDefault();
			float valueOrDefault3 = this.selectLandmarkChance.GetValue(slate).GetValueOrDefault(0.5f);
			FastTileFinder.LandmarkMode landmarkMode = (valueOrDefault2 || Rand.Chance(valueOrDefault3)) ? FastTileFinder.LandmarkMode.Required : FastTileFinder.LandmarkMode.Forbidden;
			FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(planetTile, (float)trueMin, (float)trueMax, landmarkMode, true, Hilliness.Undefined, valueOrDefault, true, true, true);
			int num = this.desperateIgnoreDistance.GetValue(slate).GetValueOrDefault() ? 0 : this.distanceFromColonyRange.GetValue(slate).TrueMin;
			float num2 = this.desperateIgnoreDistance.GetValue(slate).GetValueOrDefault() ? float.MaxValue : ((float)this.distanceFromColonyRange.GetValue(slate).TrueMax);
			PlanetTile origin = planetTile;
			float minDistTiles = (float)num;
			float maxDistTiles = num2;
			FastTileFinder.LandmarkMode landmarkMode2 = FastTileFinder.LandmarkMode.Any;
			bool reachable = true;
			Hilliness minHilliness = Hilliness.Undefined;
			bool valueOrDefault4 = (!this.desperateIgnoreBiome.GetValue(slate)).GetValueOrDefault(true);
			FastTileFinder.TileQueryParams desperate = new FastTileFinder.TileQueryParams(origin, minDistTiles, maxDistTiles, landmarkMode2, reachable, minHilliness, valueOrDefault, valueOrDefault4, true, true);
			List<PlanetTile> list = planetLayer.FastTileFinder.Query(query, this.allowedBiomes.GetValue(slate), this.allowedLandmarks.GetValue(slate), desperate);
			if (!list.Empty<PlanetTile>())
			{
				tile = list.RandomElement<PlanetTile>();
				return true;
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x0600DAA7 RID: 55975 RVA: 0x00516624 File Offset: 0x00514824
		[CompilerGenerated]
		private bool <TryGetLayer>g__Validator|18_0(PlanetTile origin, PlanetLayer layer, ref QuestNode_Root_Site.<>c__DisplayClass18_0 A_3)
		{
			if (!this.canBeSpace.GetValue(A_3.slate) && layer.Def.isSpace)
			{
				return false;
			}
			List<PlanetLayerDef> value = this.layerWhitelist.GetValue(A_3.slate);
			List<PlanetLayerDef> value2 = this.layerBlacklist.GetValue(A_3.slate);
			return (value.NullOrEmpty<PlanetLayerDef>() || value.Contains(layer.Def)) && (value2.NullOrEmpty<PlanetLayerDef>() || !value2.Contains(layer.Def)) && (!this.requireSameOrAdjacentLayer.GetValue(A_3.slate) || !origin.Valid || origin.Layer == layer || layer.DirectConnectionTo(origin.Layer));
		}

		// Token: 0x04008BC7 RID: 35783
		private SlateRef<SitePartDef> sitePartDef;

		// Token: 0x04008BC8 RID: 35784
		private SlateRef<WorldObjectDef> worldObjectDef;

		// Token: 0x04008BC9 RID: 35785
		private SlateRef<FactionDef> factionDef;

		// Token: 0x04008BCA RID: 35786
		private SlateRef<List<BiomeDef>> allowedBiomes;

		// Token: 0x04008BCB RID: 35787
		private SlateRef<Hilliness?> maxHilliness;

		// Token: 0x04008BCC RID: 35788
		private SlateRef<IntRange> distanceFromColonyRange;

		// Token: 0x04008BCD RID: 35789
		private SlateRef<List<LandmarkDef>> allowedLandmarks;

		// Token: 0x04008BCE RID: 35790
		private SlateRef<float?> selectLandmarkChance;

		// Token: 0x04008BCF RID: 35791
		private SlateRef<bool?> requiresLandmark;

		// Token: 0x04008BD0 RID: 35792
		private SlateRef<bool?> desperateIgnoreBiome;

		// Token: 0x04008BD1 RID: 35793
		private SlateRef<bool?> desperateIgnoreDistance;

		// Token: 0x04008BD2 RID: 35794
		public SlateRef<bool> canBeSpace;

		// Token: 0x04008BD3 RID: 35795
		public SlateRef<bool> requireSameOrAdjacentLayer = true;

		// Token: 0x04008BD4 RID: 35796
		public SlateRef<List<PlanetLayerDef>> layerWhitelist;

		// Token: 0x04008BD5 RID: 35797
		public SlateRef<List<PlanetLayerDef>> layerBlacklist;

		// Token: 0x04008BD6 RID: 35798
		public const string SiteAlias = "site";
	}
}
