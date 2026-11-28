using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002105 RID: 8453
	public class SymbolResolver_FloorFill : SymbolResolver
	{
		// Token: 0x0600D451 RID: 54353 RVA: 0x004E7A14 File Offset: 0x004E5C14
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			TerrainGrid terrainGrid = map.terrainGrid;
			TerrainDef terrainDef = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			bool valueOrDefault = rp.floorOnlyIfTerrainSupports.GetValueOrDefault();
			bool valueOrDefault2 = rp.allowBridgeOnAnyImpassableTerrain.GetValueOrDefault();
			foreach (IntVec3 c in rp.rect)
			{
				if ((rp.chanceToSkipFloor == null || !Rand.Chance(rp.chanceToSkipFloor.Value)) && (!valueOrDefault || GenConstruct.CanBuildOnTerrain(terrainDef, c, map, Rot4.North, null, null) || (valueOrDefault2 && c.GetTerrain(map).passability == Traversability.Impassable)))
				{
					terrainGrid.SetTerrain(c, terrainDef);
					if (rp.filthDef != null)
					{
						FilthMaker.TryMakeFilth(c, map, rp.filthDef, (rp.filthDensity != null) ? Mathf.RoundToInt(rp.filthDensity.Value.RandomInRange) : 1, FilthSourceFlags.None, true);
					}
				}
			}
		}
	}
}
