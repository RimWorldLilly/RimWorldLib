using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200133B RID: 4923
	public class TileMutatorWorker_MineralRich : TileMutatorWorker
	{
		// Token: 0x170014CE RID: 5326
		// (get) Token: 0x060079EE RID: 31214 RVA: 0x002B64E4 File Offset: 0x002B46E4
		private List<ThingDef> Mineables
		{
			get
			{
				if (this.cachedMineables == null)
				{
					this.cachedMineables = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
					{
						BuildingProperties building = thingDef.building;
						if (((building != null) ? building.mineableThing : null) != null && thingDef.building.isResourceRock && !this.def.resourceBlacklist.Contains(thingDef))
						{
							this.cachedMineables.Add(thingDef);
						}
					}
				}
				return this.cachedMineables;
			}
		}

		// Token: 0x170014CF RID: 5327
		// (get) Token: 0x060079EF RID: 31215 RVA: 0x002B6584 File Offset: 0x002B4784
		private Perlin Perlin
		{
			get
			{
				if (this.perlin == null)
				{
					this.perlin = new Perlin(0.4000000059604645, 0.5, 0.5, 4, true, false, Gen.HashCombineInt(Find.World.info.Seed, 525214866), QualityMode.Medium);
				}
				return this.perlin;
			}
		}

		// Token: 0x060079F0 RID: 31216 RVA: 0x002B65E2 File Offset: 0x002B47E2
		public TileMutatorWorker_MineralRich(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079F1 RID: 31217 RVA: 0x002B65EB File Offset: 0x002B47EB
		public override string GetLabel(PlanetTile tile)
		{
			return "MineralRich".Translate(this.GetMineableThingDefForTile(tile).Named("MINERAL"));
		}

		// Token: 0x060079F2 RID: 31218 RVA: 0x002B660D File Offset: 0x002B480D
		public override string GetDescription(PlanetTile tile)
		{
			return "MineralRichDescription".Translate(this.GetMineableThingDefForTile(tile).Named("MINERAL"));
		}

		// Token: 0x060079F3 RID: 31219 RVA: 0x002B6630 File Offset: 0x002B4830
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			float num = GenStep_RocksFromGrid.GetResourceBlotchesPer10KCellsForMap(map);
			ThingDef mineableThingDefForTile = this.GetMineableThingDefForTile(map.Tile);
			float num2 = this.Mineables.Sum((ThingDef d) => d.building.mineableScatterCommonality);
			float num3 = mineableThingDefForTile.building.mineableScatterCommonality / num2;
			num *= num3;
			int num4 = Mathf.RoundToInt(10000f / num);
			int x = map.Size.x;
			if (Mathf.RoundToInt((float)(x * x) / (float)num4) < 2)
			{
				num = 10000f / ((float)(x * x) * 0.5f);
			}
			GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new GenStep_ScatterLumpsMineable
			{
				maxValue = float.MaxValue,
				countPer10kCellsRange = new FloatRange(num, num),
				forcedDefToScatter = mineableThingDefForTile
			};
			for (int i = 0; i < 2; i++)
			{
				genStep_ScatterLumpsMineable.Generate(map, default(GenStepParams));
			}
		}

		// Token: 0x060079F4 RID: 31220 RVA: 0x002B6720 File Offset: 0x002B4920
		private ThingDef GetMineableThingDefForTile(PlanetTile tile)
		{
			List<ThingDef> mineables = this.Mineables;
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tile);
			float value = this.Perlin.GetValue(tileCenter);
			int count = mineables.Count;
			int num = (int)(value * (float)count);
			if (num >= count)
			{
				num = count - 1;
			}
			return mineables[num];
		}

		// Token: 0x04004CEE RID: 19694
		private Perlin perlin;

		// Token: 0x04004CEF RID: 19695
		private const int SeedPart = 525214866;

		// Token: 0x04004CF0 RID: 19696
		private List<ThingDef> cachedMineables;
	}
}
