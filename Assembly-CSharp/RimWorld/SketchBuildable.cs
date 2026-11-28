using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001396 RID: 5014
	public abstract class SketchBuildable : SketchEntity
	{
		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x06007C27 RID: 31783
		public abstract BuildableDef Buildable { get; }

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x06007C28 RID: 31784
		public abstract ThingDef Stuff { get; }

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x06007C29 RID: 31785 RVA: 0x002C5319 File Offset: 0x002C3519
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.Buildable, this.Stuff, 1);
			}
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x06007C2A RID: 31786 RVA: 0x002C532D File Offset: 0x002C352D
		public override bool LostImportantReferences
		{
			get
			{
				return this.Buildable == null;
			}
		}

		// Token: 0x06007C2B RID: 31787
		public abstract Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x06007C2C RID: 31788 RVA: 0x002C5338 File Offset: 0x002C3538
		public override bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map)
		{
			return at.InBounds(map) && (this.IsSameSpawned(at, map) || this.GetSpawnedBlueprintOrFrame(at, map) != null);
		}

		// Token: 0x06007C2D RID: 31789 RVA: 0x002C535C File Offset: 0x002C355C
		public Thing FirstPermanentBlockerAt(IntVec3 at, Map map)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(at, Rot4.North, this.Buildable.Size))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (!thingList[i].def.destroyable && !GenConstruct.CanPlaceBlueprintOver(this.Buildable, thingList[i].def, null, null))
						{
							return thingList[i];
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06007C2E RID: 31790 RVA: 0x002C5424 File Offset: 0x002C3624
		protected void ForceTerrainAffordance(IntVec3 at, Rot4 rot, Map map, TerrainDef defaultTerrain)
		{
			TerrainDef newTerr;
			if (defaultTerrain != null && defaultTerrain.affordances.Contains(this.Buildable.terrainAffordanceNeeded))
			{
				newTerr = defaultTerrain;
			}
			else
			{
				newTerr = map.Biome.TerrainForAffordance(this.Buildable.terrainAffordanceNeeded);
			}
			CellRect cellRect = GenAdj.OccupiedRect(at, rot, this.Buildable.Size);
			cellRect.ClipInsideMap(map);
			foreach (IntVec3 c in cellRect)
			{
				if (map.terrainGrid.TerrainAt(c).IsWater)
				{
					map.terrainGrid.SetFoundation(c, ModsConfig.OdysseyActive ? TerrainDefOf.AncientHeavyBridge : TerrainDefOf.AncientBridge);
				}
				else
				{
					map.terrainGrid.SetTerrain(c, newTerr);
				}
			}
		}
	}
}
