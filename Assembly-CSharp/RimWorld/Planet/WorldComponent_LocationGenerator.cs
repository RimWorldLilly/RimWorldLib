using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002340 RID: 9024
	public class WorldComponent_LocationGenerator : WorldComponent
	{
		// Token: 0x0600DF0B RID: 57099 RVA: 0x0052F5B4 File Offset: 0x0052D7B4
		public WorldComponent_LocationGenerator(World world) : base(world)
		{
			float planetCoverage = world.PlanetCoverage;
			int num;
			if (planetCoverage >= 0.051f)
			{
				if (planetCoverage >= 0.301f)
				{
					if (planetCoverage >= 0.501f)
					{
						num = 20;
					}
					else
					{
						num = 12;
					}
				}
				else
				{
					num = 8;
				}
			}
			else
			{
				num = 3;
			}
			this.worldLocationsTarget = num;
			foreach (GeneratedLocationDef generatedLocationDef in DefDatabase<GeneratedLocationDef>.AllDefsListForReading)
			{
				foreach (PlanetLayerDef key in generatedLocationDef.LayerDefs)
				{
					List<GeneratedLocationDef> list;
					if (!WorldComponent_LocationGenerator.LayerList.TryGetValue(key, out list))
					{
						list = (WorldComponent_LocationGenerator.LayerList[key] = new List<GeneratedLocationDef>());
					}
					list.Add(generatedLocationDef);
				}
			}
		}

		// Token: 0x0600DF0C RID: 57100 RVA: 0x0052F6A8 File Offset: 0x0052D8A8
		public override void WorldComponentTick()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				if (GenTicks.IsTickInterval(planetLayer2.GetHashCode().HashOffset(), 90000))
				{
					int num2 = Mathf.RoundToInt(planetLayer2.Def.generatedLocationFactor * (float)this.worldLocationsTarget);
					if (planetLayer2.Tiles.Count((Tile tile) => this.world.worldObjects.AnyGeneratedWorldLocationAt(tile.tile)) < num2)
					{
						this.GenerateUntilTarget(planetLayer2);
					}
				}
			}
		}

		// Token: 0x0600DF0D RID: 57101 RVA: 0x0052F754 File Offset: 0x0052D954
		private void GenerateUntilTarget()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer layer = planetLayer;
				this.GenerateUntilTarget(layer);
			}
		}

		// Token: 0x0600DF0E RID: 57102 RVA: 0x0052F7B4 File Offset: 0x0052D9B4
		private void GenerateUntilTarget(PlanetLayer layer)
		{
			int num = Mathf.RoundToInt(layer.Def.generatedLocationFactor * (float)this.worldLocationsTarget);
			for (int i = 0; i < num; i++)
			{
				this.GenerateLocationForLayer(layer);
			}
		}

		// Token: 0x0600DF0F RID: 57103 RVA: 0x0052F7F0 File Offset: 0x0052D9F0
		private void GenerateLocationForLayer(PlanetLayer layer)
		{
			if (!WorldComponent_LocationGenerator.LayerList.ContainsKey(layer.Def) || WorldComponent_LocationGenerator.LayerList[layer.Def].Empty<GeneratedLocationDef>())
			{
				return;
			}
			PlanetTile tile;
			if (!WorldComponent_LocationGenerator.TryFindSiteTile(layer, out tile))
			{
				return;
			}
			WorldComponent_LocationGenerator.tmpOptions.Clear();
			WorldComponent_LocationGenerator.tmpOptions.AddRange(WorldComponent_LocationGenerator.LayerList[layer.Def]);
			using (List<GeneratedLocationDef>.Enumerator enumerator = WorldComponent_LocationGenerator.LayerList[layer.Def].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GeneratedLocationDef location = enumerator.Current;
					if (location.layerMaximum >= 0 && layer.Tiles.Count((Tile t) => this.world.worldObjects.WorldObjectOfDefAt(location.worldObjectDef, t.tile) != null) >= location.layerMaximum)
					{
						WorldComponent_LocationGenerator.tmpOptions.Remove(location);
					}
				}
			}
			if (WorldComponent_LocationGenerator.tmpOptions.Empty<GeneratedLocationDef>())
			{
				return;
			}
			GeneratedLocationDef generatedLocationDef = WorldComponent_LocationGenerator.tmpOptions.RandomElementByWeight((GeneratedLocationDef def) => def.weight);
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(generatedLocationDef.worldObjectDef);
			INameableWorldObject nameableWorldObject = worldObject as INameableWorldObject;
			if (nameableWorldObject != null)
			{
				nameableWorldObject.Name = NameGenerator.GenerateName(worldObject.def.nameMaker, null, false, "r_name", null, null);
			}
			IResourceWorldObject resourceWorldObject = worldObject as IResourceWorldObject;
			if (resourceWorldObject != null && !generatedLocationDef.preciousResources.NullOrEmpty<ThingDef>())
			{
				resourceWorldObject.PreciousResource = generatedLocationDef.preciousResources.RandomElement<ThingDef>();
			}
			IExpirableWorldObject expirableWorldObject = worldObject as IExpirableWorldObject;
			if (expirableWorldObject != null && !generatedLocationDef.TimeoutRangeDays.IsZeros)
			{
				expirableWorldObject.ExpireAtTicks = GenTicks.TicksGame + (int)(generatedLocationDef.TimeoutRangeDays.RandomInRange * 60000f);
			}
			worldObject.isGeneratedLocation = true;
			worldObject.Tile = tile;
			this.world.worldObjects.Add(worldObject);
		}

		// Token: 0x0600DF10 RID: 57104 RVA: 0x0052F9EC File Offset: 0x0052DBEC
		public static bool TryFindSiteTile(PlanetLayer layer, out PlanetTile tile)
		{
			tile = PlanetTile.Invalid;
			PlanetTile other = new PlanetTile(0, Find.WorldGrid.Surface);
			PlanetTile planetTile;
			if (TileFinder.TryFindRandomPlayerTile(out planetTile, false, null, false, null))
			{
				other = planetTile;
			}
			return TileFinder.TryFindTileWithDistance(layer.GetClosestTile_NewTemp(other, true), 0, int.MaxValue, out tile, new Predicate<PlanetTile>(WorldComponent_LocationGenerator.<TryFindSiteTile>g__Validator|9_0), TileFinderMode.Random, false);
		}

		// Token: 0x0600DF11 RID: 57105 RVA: 0x0052FA47 File Offset: 0x0052DC47
		public override void FinalizeInit(bool fromLoad)
		{
			if (fromLoad)
			{
				return;
			}
			this.GenerateUntilTarget();
		}

		// Token: 0x0600DF14 RID: 57108 RVA: 0x0052FA81 File Offset: 0x0052DC81
		[CompilerGenerated]
		internal static bool <TryFindSiteTile>g__Validator|9_0(PlanetTile x)
		{
			return !Find.WorldObjects.AnyWorldObjectAt(x);
		}

		// Token: 0x04008F42 RID: 36674
		private const int UpdateInterval = 90000;

		// Token: 0x04008F43 RID: 36675
		private readonly int worldLocationsTarget;

		// Token: 0x04008F44 RID: 36676
		private static readonly Dictionary<PlanetLayerDef, List<GeneratedLocationDef>> LayerList = new Dictionary<PlanetLayerDef, List<GeneratedLocationDef>>();

		// Token: 0x04008F45 RID: 36677
		private static readonly List<GeneratedLocationDef> tmpOptions = new List<GeneratedLocationDef>();
	}
}
