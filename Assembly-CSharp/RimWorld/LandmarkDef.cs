using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200175F RID: 5983
	public class LandmarkDef : Def
	{
		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x060094D7 RID: 38103 RVA: 0x0036399B File Offset: 0x00361B9B
		public Material Material
		{
			get
			{
				return MaterialPool.MatFrom(this.iconTexturePath, ShaderDatabase.WorldOverlayTransparentLit, 3510);
			}
		}

		// Token: 0x17001971 RID: 6513
		// (get) Token: 0x060094D8 RID: 38104 RVA: 0x003639B2 File Offset: 0x00361BB2
		public Texture2D Texture
		{
			get
			{
				return CachedTexture.Get(this.iconTexturePath);
			}
		}

		// Token: 0x060094D9 RID: 38105 RVA: 0x003639C0 File Offset: 0x00361BC0
		public bool EverValid()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			foreach (MutatorChance mutatorChance in this.mutatorChances)
			{
				if (mutatorChance.required && !mutatorChance.mutator.EverValid())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060094DA RID: 38106 RVA: 0x00363A34 File Offset: 0x00361C34
		public static void ClearCache()
		{
			LandmarkDef.cachedSettlementTiles.Clear();
		}

		// Token: 0x060094DB RID: 38107 RVA: 0x00363A40 File Offset: 0x00361C40
		public bool IsValidTile(PlanetTile tile, PlanetLayer layer, bool canUseCache = false)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			bool flag = false;
			Tile tile2 = layer[tile];
			if (tile2.PrimaryBiome.impassable || tile2.hilliness == Hilliness.Impassable)
			{
				return false;
			}
			if (Find.World.landmarks[tile] != null)
			{
				return false;
			}
			if (tile2.Mutators.Any((TileMutatorDef m) => m.preventsLandmarks))
			{
				return false;
			}
			foreach (MutatorChance mutatorChance in this.mutatorChances)
			{
				if (mutatorChance.required)
				{
					if (!mutatorChance.mutator.IsValidTile(tile, layer))
					{
						return false;
					}
					flag = true;
				}
				if (!flag && mutatorChance.chance >= 1f && mutatorChance.mutator.IsValidTile(tile, layer))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (this.coastRotateMode != LandmarkDef.CoastRotateMode.None && !LandmarkDef.IsValidRotatableTile(tile, layer, this.coastRotateMode))
			{
				return false;
			}
			if (canUseCache)
			{
				if (!LandmarkDef.cachedSettlementTiles.ContainsKey(tile))
				{
					LandmarkDef.cachedSettlementTiles[tile] = Find.World.worldObjects.AnySettlementAt(tile);
				}
				if (LandmarkDef.cachedSettlementTiles[tile])
				{
					return false;
				}
			}
			else if (Find.World.worldObjects.AnySettlementAt(tile))
			{
				return false;
			}
			return true;
		}

		// Token: 0x060094DC RID: 38108 RVA: 0x00363BAC File Offset: 0x00361DAC
		private static bool IsValidRotatableTile(PlanetTile tile, PlanetLayer layer, LandmarkDef.CoastRotateMode mode)
		{
			layer.GetTileNeighbors(tile, LandmarkDef.neighbours);
			bool result = false;
			for (int i = 0; i < LandmarkDef.neighbours.Count; i++)
			{
				if (layer[LandmarkDef.neighbours[i]].Landmark != null)
				{
					return false;
				}
				if (LandmarkDef.IsValidRotatableNeighbour(layer[LandmarkDef.neighbours[i]].PrimaryBiome, mode))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x060094DD RID: 38109 RVA: 0x00363C1E File Offset: 0x00361E1E
		public static bool IsValidRotatableNeighbour(BiomeDef biome, LandmarkDef.CoastRotateMode mode)
		{
			if (mode == LandmarkDef.CoastRotateMode.ToCoast)
			{
				return !biome.isBackgroundBiome;
			}
			return biome.isBackgroundBiome;
		}

		// Token: 0x060094DE RID: 38110 RVA: 0x00363C34 File Offset: 0x00361E34
		public override IEnumerable<string> ConfigErrors()
		{
			LandmarkDef.<ConfigErrors>d__26 <ConfigErrors>d__ = new LandmarkDef.<ConfigErrors>d__26(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040059B0 RID: 22960
		public Type workerClass = typeof(Landmark);

		// Token: 0x040059B1 RID: 22961
		public float commonality;

		// Token: 0x040059B2 RID: 22962
		public List<MutatorChance> mutatorChances = new List<MutatorChance>();

		// Token: 0x040059B3 RID: 22963
		public List<MutatorChance> comboLandmarkMutators = new List<MutatorChance>();

		// Token: 0x040059B4 RID: 22964
		public RulePackDef nameMaker;

		// Token: 0x040059B5 RID: 22965
		[NoTranslate]
		public string iconTexturePath;

		// Token: 0x040059B6 RID: 22966
		public IntVec2 atlasSize = IntVec2.One;

		// Token: 0x040059B7 RID: 22967
		public float drawScale = 1f;

		// Token: 0x040059B8 RID: 22968
		public LandmarkDef.LandmarkDrawType drawType;

		// Token: 0x040059B9 RID: 22969
		public LandmarkDef.CoastRotateMode coastRotateMode;

		// Token: 0x040059BA RID: 22970
		public bool drawAboveRoads;

		// Token: 0x040059BB RID: 22971
		[NoTranslate]
		public string category;

		// Token: 0x040059BC RID: 22972
		public List<ShaderParameter> terrainParameters;

		// Token: 0x040059BD RID: 22973
		private static readonly List<PlanetTile> neighbours = new List<PlanetTile>(8);

		// Token: 0x040059BE RID: 22974
		private static Dictionary<PlanetTile, bool> cachedSettlementTiles = new Dictionary<PlanetTile, bool>();

		// Token: 0x0200354D RID: 13645
		public enum LandmarkDrawType
		{
			// Token: 0x0400D542 RID: 54594
			Standard,
			// Token: 0x0400D543 RID: 54595
			TerrainMask
		}

		// Token: 0x0200354E RID: 13646
		public enum CoastRotateMode
		{
			// Token: 0x0400D545 RID: 54597
			None,
			// Token: 0x0400D546 RID: 54598
			ToCoast,
			// Token: 0x0400D547 RID: 54599
			AwayFromCoast
		}
	}
}
