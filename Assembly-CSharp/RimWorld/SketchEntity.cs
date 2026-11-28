using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001397 RID: 5015
	public abstract class SketchEntity : IExposable
	{
		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x06007C30 RID: 31792
		public abstract string Label { get; }

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x06007C31 RID: 31793
		public abstract string LabelCap { get; }

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x06007C32 RID: 31794
		public abstract CellRect OccupiedRect { get; }

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x06007C33 RID: 31795
		public abstract float SpawnOrder { get; }

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x06007C34 RID: 31796 RVA: 0x002C550C File Offset: 0x002C370C
		public virtual bool LostImportantReferences
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007C35 RID: 31797
		public abstract void DrawGhost(IntVec3 at, Color color);

		// Token: 0x06007C36 RID: 31798
		public abstract bool IsSameSpawned(IntVec3 at, Map map);

		// Token: 0x06007C37 RID: 31799
		public abstract bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x06007C38 RID: 31800
		public abstract bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x06007C39 RID: 31801
		public abstract bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x06007C3A RID: 31802
		public abstract bool CanBuildOnTerrain(IntVec3 at, Map map);

		// Token: 0x06007C3B RID: 31803
		public abstract bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, List<Thing> spawnedThings = null, bool dormant = false, TerrainDef defaultAffordanceTerrain = null);

		// Token: 0x06007C3C RID: 31804
		public abstract bool SameForSubtracting(SketchEntity other);

		// Token: 0x06007C3D RID: 31805 RVA: 0x002C5510 File Offset: 0x002C3710
		public bool SpawnNear(IntVec3 near, Map map, float radius, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, List<Thing> spawnedThings = null, bool dormant = false, Func<SketchEntity, IntVec3, bool> validator = null, TerrainDef affordanceTerrain = null)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = near + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && (validator == null || validator(this, intVec)) && this.Spawn(intVec, map, faction, spawnMode, wipeIfCollides, forceTerrainAffordance, spawnedThings, dormant, null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C3E RID: 31806 RVA: 0x002C5574 File Offset: 0x002C3774
		public virtual SketchEntity DeepCopy()
		{
			SketchEntity sketchEntity = (SketchEntity)Activator.CreateInstance(base.GetType());
			sketchEntity.pos = this.pos;
			return sketchEntity;
		}

		// Token: 0x06007C3F RID: 31807 RVA: 0x002C5594 File Offset: 0x002C3794
		public virtual void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.pos, "pos", default(IntVec3), false);
		}

		// Token: 0x04004E3B RID: 20027
		public IntVec3 pos;
	}
}
