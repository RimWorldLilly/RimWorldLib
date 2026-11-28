using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001398 RID: 5016
	public class SketchTerrain : SketchBuildable
	{
		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x06007C41 RID: 31809 RVA: 0x002C55C3 File Offset: 0x002C37C3
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x06007C42 RID: 31810 RVA: 0x002C55CB File Offset: 0x002C37CB
		public override ThingDef Stuff
		{
			get
			{
				return this.stuffForComparingSimilar;
			}
		}

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x06007C43 RID: 31811 RVA: 0x002C55D3 File Offset: 0x002C37D3
		public override float SpawnOrder
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x06007C44 RID: 31812 RVA: 0x002C55DA File Offset: 0x002C37DA
		public override CellRect OccupiedRect
		{
			get
			{
				if (this.cachedOccupiedRectPos != this.pos)
				{
					this.cachedOccupiedRect = CellRect.SingleCell(this.pos);
					this.cachedOccupiedRectPos = this.pos;
				}
				return this.cachedOccupiedRect;
			}
		}

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x06007C45 RID: 31813 RVA: 0x002C5614 File Offset: 0x002C3814
		public override string Label
		{
			get
			{
				if (this.def.designatorDropdown == null || this.def.designatorDropdown.label.NullOrEmpty() || !this.treatSimilarAsSame)
				{
					return base.Label;
				}
				return this.def.designatorDropdown.label;
			}
		}

		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x06007C46 RID: 31814 RVA: 0x002C5664 File Offset: 0x002C3864
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x06007C47 RID: 31815 RVA: 0x002C5674 File Offset: 0x002C3874
		public override void DrawGhost(IntVec3 at, Color color)
		{
			ThingDef blueprintDef = this.def.blueprintDef;
			GraphicDatabase.Get(blueprintDef.graphic.GetType(), blueprintDef.graphic.path, blueprintDef.graphic.Shader, blueprintDef.graphic.drawSize, color, Color.white, blueprintDef.graphicData, null, null).DrawFromDef(at.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint), Rot4.North, this.def.blueprintDef, 0f);
		}

		// Token: 0x06007C48 RID: 31816 RVA: 0x002C56EF File Offset: 0x002C38EF
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return at.InBounds(map) && this.IsSameOrSimilar(at.GetTerrain(map));
		}

		// Token: 0x06007C49 RID: 31817 RVA: 0x002C570C File Offset: 0x002C390C
		public bool IsSameOrSimilar(BuildableDef other)
		{
			if (other == null)
			{
				return false;
			}
			if (!this.treatSimilarAsSame)
			{
				return other == this.def;
			}
			if (this.def.designatorDropdown == null && other.designatorDropdown == null && other.BuildableByPlayer)
			{
				return true;
			}
			if (this.def.designatorDropdown == null || other.designatorDropdown == null)
			{
				return other == this.def;
			}
			return other.designatorDropdown == this.def.designatorDropdown;
		}

		// Token: 0x06007C4A RID: 31818 RVA: 0x002C5784 File Offset: 0x002C3984
		public override Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].Position == at && this.IsSameOrSimilar(thingList[i].def.entityDefToBuild))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06007C4B RID: 31819 RVA: 0x002C57EC File Offset: 0x002C39EC
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !GenAdj.OccupiedRect(at, Rot4.North, this.def.Size).InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, Rot4.North, map, wipeIfCollides, thingToIgnore, null, null, false, false, false).Accepted;
		}

		// Token: 0x06007C4C RID: 31820 RVA: 0x002C5852 File Offset: 0x002C3A52
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return !at.InBounds(map) || !this.CanBuildOnTerrain(at, map) || base.FirstPermanentBlockerAt(at, map) != null;
		}

		// Token: 0x06007C4D RID: 31821 RVA: 0x002C5878 File Offset: 0x002C3A78
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, Rot4.North, null, null);
		}

		// Token: 0x06007C4E RID: 31822 RVA: 0x002C5890 File Offset: 0x002C3A90
		public override bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, List<Thing> spawnedThings = null, bool dormant = false, TerrainDef defaultAffordanceTerrain = null)
		{
			if (!at.InBounds(map))
			{
				return false;
			}
			if (forceTerrainAffordance && !this.CanBuildOnTerrain(at, map))
			{
				base.ForceTerrainAffordance(at, Rot4.North, map, defaultAffordanceTerrain);
			}
			if (this.IsSpawningBlocked(at, map, null, wipeIfCollides))
			{
				return false;
			}
			if (spawnMode != Sketch.SpawnMode.Blueprint)
			{
				if (spawnMode != Sketch.SpawnMode.Normal)
				{
					throw new NotImplementedException("Spawn mode " + spawnMode.ToString() + " not implemented!");
				}
				if (this.def.isFoundation)
				{
					map.terrainGrid.SetFoundation(at, this.GetDefFromStuff());
				}
				else if (this.def.temporary)
				{
					map.terrainGrid.SetTempTerrain(at, this.GetDefFromStuff());
				}
				else
				{
					map.terrainGrid.SetTerrain(at, this.GetDefFromStuff());
				}
			}
			else
			{
				GenConstruct.PlaceBlueprintForBuild(this.GetDefFromStuff(), at, map, Rot4.North, faction, null, null, null, false);
			}
			return true;
		}

		// Token: 0x06007C4F RID: 31823 RVA: 0x002C5974 File Offset: 0x002C3B74
		private TerrainDef GetDefFromStuff()
		{
			if (this.stuffForComparingSimilar == null)
			{
				return this.def;
			}
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				if (this.IsSameOrSimilar(terrainDef) && !terrainDef.CostList.NullOrEmpty<ThingDefCountClass>() && terrainDef.CostList[0].thingDef == this.stuffForComparingSimilar)
				{
					return terrainDef;
				}
			}
			return this.def;
		}

		// Token: 0x06007C50 RID: 31824 RVA: 0x002C5A04 File Offset: 0x002C3C04
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchTerrain sketchTerrain = other as SketchTerrain;
			return sketchTerrain != null && (sketchTerrain == this || (this.IsSameOrSimilar(sketchTerrain.Buildable) && this.pos == sketchTerrain.pos));
		}

		// Token: 0x06007C51 RID: 31825 RVA: 0x002C5A44 File Offset: 0x002C3C44
		public override SketchEntity DeepCopy()
		{
			SketchTerrain sketchTerrain = (SketchTerrain)base.DeepCopy();
			sketchTerrain.def = this.def;
			sketchTerrain.stuffForComparingSimilar = this.stuffForComparingSimilar;
			sketchTerrain.treatSimilarAsSame = this.treatSimilarAsSame;
			return sketchTerrain;
		}

		// Token: 0x06007C52 RID: 31826 RVA: 0x002C5A75 File Offset: 0x002C3C75
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuffForComparingSimilar, "stuff");
			Scribe_Values.Look<bool>(ref this.treatSimilarAsSame, "treatSimilarAsSame", false, false);
		}

		// Token: 0x04004E3C RID: 20028
		public TerrainDef def;

		// Token: 0x04004E3D RID: 20029
		public ThingDef stuffForComparingSimilar;

		// Token: 0x04004E3E RID: 20030
		public bool treatSimilarAsSame;

		// Token: 0x04004E3F RID: 20031
		private CellRect cachedOccupiedRect = CellRect.Empty;

		// Token: 0x04004E40 RID: 20032
		private IntVec3 cachedOccupiedRectPos = IntVec3.Invalid;
	}
}
