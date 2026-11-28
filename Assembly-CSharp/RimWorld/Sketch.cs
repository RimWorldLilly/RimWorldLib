using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001395 RID: 5013
	public class Sketch : IExposable
	{
		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x06007BED RID: 31725 RVA: 0x002C3324 File Offset: 0x002C1524
		public List<SketchEntity> Entities
		{
			get
			{
				return this.entities;
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x06007BEE RID: 31726 RVA: 0x002C332C File Offset: 0x002C152C
		public List<SketchThing> Things
		{
			get
			{
				return this.cachedThings;
			}
		}

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x06007BEF RID: 31727 RVA: 0x002C3334 File Offset: 0x002C1534
		public List<SketchTerrain> Terrain
		{
			get
			{
				return this.cachedTerrain;
			}
		}

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x06007BF0 RID: 31728 RVA: 0x002C333C File Offset: 0x002C153C
		public List<SketchBuildable> Buildables
		{
			get
			{
				return this.cachedBuildables;
			}
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x06007BF1 RID: 31729 RVA: 0x002C3344 File Offset: 0x002C1544
		public CellRect OccupiedRect
		{
			get
			{
				if (this.occupiedRectDirty)
				{
					this.cachedOccupiedRect = CellRect.Empty;
					bool flag = false;
					for (int i = 0; i < this.entities.Count; i++)
					{
						if (!flag)
						{
							this.cachedOccupiedRect = this.entities[i].OccupiedRect;
							flag = true;
						}
						else
						{
							CellRect occupiedRect = this.entities[i].OccupiedRect;
							this.cachedOccupiedRect = CellRect.FromLimits(Mathf.Min(this.cachedOccupiedRect.minX, occupiedRect.minX), Mathf.Min(this.cachedOccupiedRect.minZ, occupiedRect.minZ), Mathf.Max(this.cachedOccupiedRect.maxX, occupiedRect.maxX), Mathf.Max(this.cachedOccupiedRect.maxZ, occupiedRect.maxZ));
						}
					}
					this.occupiedRectDirty = false;
				}
				return this.cachedOccupiedRect;
			}
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x06007BF2 RID: 31730 RVA: 0x002C3428 File Offset: 0x002C1628
		public IntVec2 OccupiedSize
		{
			get
			{
				return new IntVec2(this.OccupiedRect.Width, this.OccupiedRect.Height);
			}
		}

		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x06007BF3 RID: 31731 RVA: 0x002C3458 File Offset: 0x002C1658
		public IntVec3 OccupiedCenter
		{
			get
			{
				return this.OccupiedRect.CenterCell;
			}
		}

		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x06007BF4 RID: 31732 RVA: 0x002C3473 File Offset: 0x002C1673
		public bool Empty
		{
			get
			{
				return !this.Entities.Any<SketchEntity>();
			}
		}

		// Token: 0x06007BF5 RID: 31733 RVA: 0x002C3484 File Offset: 0x002C1684
		public bool Add(SketchEntity entity, bool wipeIfCollides = true)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (this.entities.Contains(entity))
			{
				return true;
			}
			if (wipeIfCollides)
			{
				this.WipeColliding(entity);
			}
			else if (this.WouldCollide(entity))
			{
				return false;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			SketchTerrain entity2;
			if (sketchTerrain != null && this.terrainAt.TryGetValue(sketchTerrain.pos, out entity2))
			{
				this.Remove(entity2);
			}
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				for (int i = this.cachedBuildables.Count - 1; i >= 0; i--)
				{
					if (sketchBuildable.OccupiedRect.Overlaps(this.cachedBuildables[i].OccupiedRect) && GenSpawn.SpawningWipes(sketchBuildable.Buildable, this.cachedBuildables[i].Buildable, false))
					{
						this.Remove(this.cachedBuildables[i]);
					}
				}
			}
			this.entities.Add(entity);
			this.AddToCache(entity);
			return true;
		}

		// Token: 0x06007BF6 RID: 31734 RVA: 0x002C3578 File Offset: 0x002C1778
		public bool AddThing(ThingDef def, IntVec3 pos, Rot4 rot, ThingDef stuff = null, int stackCount = 1, QualityCategory? quality = null, int? hitPoints = null, bool wipeIfCollides = true, float spawnOrder = 2f)
		{
			SketchThing entity = new SketchThing
			{
				def = def,
				stuff = stuff,
				pos = pos,
				rot = rot,
				stackCount = stackCount,
				quality = quality,
				hitPoints = hitPoints,
				spawnOrder = spawnOrder
			};
			return this.Add(entity, wipeIfCollides);
		}

		// Token: 0x06007BF7 RID: 31735 RVA: 0x002C35D4 File Offset: 0x002C17D4
		public bool AddTerrain(TerrainDef def, IntVec3 pos, bool wipeIfCollides = true)
		{
			return this.Add(new SketchTerrain
			{
				def = def,
				pos = pos
			}, wipeIfCollides);
		}

		// Token: 0x06007BF8 RID: 31736 RVA: 0x002C35FD File Offset: 0x002C17FD
		public bool Remove(SketchEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			if (!this.entities.Contains(entity))
			{
				return false;
			}
			this.entities.Remove(entity);
			this.RemoveFromCache(entity);
			return true;
		}

		// Token: 0x06007BF9 RID: 31737 RVA: 0x002C362C File Offset: 0x002C182C
		public bool RemoveTerrain(IntVec3 cell)
		{
			SketchTerrain entity;
			return this.terrainAt.TryGetValue(cell, out entity) && this.Remove(entity);
		}

		// Token: 0x06007BFA RID: 31738 RVA: 0x002C3652 File Offset: 0x002C1852
		public void Clear()
		{
			this.entities.Clear();
			this.RecacheAll();
		}

		// Token: 0x06007BFB RID: 31739 RVA: 0x002C3668 File Offset: 0x002C1868
		public TerrainDef TerrainAt(IntVec3 pos)
		{
			SketchTerrain sketchTerrain = this.SketchTerrainAt(pos);
			if (sketchTerrain == null)
			{
				return null;
			}
			return sketchTerrain.def;
		}

		// Token: 0x06007BFC RID: 31740 RVA: 0x002C3688 File Offset: 0x002C1888
		public SketchTerrain SketchTerrainAt(IntVec3 pos)
		{
			SketchTerrain result;
			if (!this.terrainAt.TryGetValue(pos, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06007BFD RID: 31741 RVA: 0x002C36A8 File Offset: 0x002C18A8
		public bool AnyTerrainAt(int x, int z)
		{
			return this.AnyTerrainAt(new IntVec3(x, 0, z));
		}

		// Token: 0x06007BFE RID: 31742 RVA: 0x002C36B8 File Offset: 0x002C18B8
		public bool AnyTerrainAt(IntVec3 pos)
		{
			return this.TerrainAt(pos) != null;
		}

		// Token: 0x06007BFF RID: 31743 RVA: 0x002C36C4 File Offset: 0x002C18C4
		public IEnumerable<SketchThing> ThingsAt(IntVec3 pos)
		{
			SketchThing val;
			if (this.thingsAt_single.TryGetValue(pos, out val))
			{
				return Gen.YieldSingle<SketchThing>(val);
			}
			List<SketchThing> result;
			if (this.thingsAt_multiple.TryGetValue(pos, out result))
			{
				return result;
			}
			return Sketch.EmptySketchThingList;
		}

		// Token: 0x06007C00 RID: 31744 RVA: 0x002C3700 File Offset: 0x002C1900
		public bool AnyThingAt(IntVec3 pos)
		{
			SketchThing sketchThing;
			SketchThing sketchThing2;
			List<SketchThing> list;
			return (this.edificeAt.TryGetValue(pos, out sketchThing) && sketchThing != null) || this.thingsAt_single.TryGetValue(pos, out sketchThing2) || this.thingsAt_multiple.TryGetValue(pos, out list);
		}

		// Token: 0x06007C01 RID: 31745 RVA: 0x002C3744 File Offset: 0x002C1944
		public void ThingsAt(IntVec3 pos, out SketchThing singleResult, out List<SketchThing> multipleResults)
		{
			SketchThing sketchThing;
			if (this.thingsAt_single.TryGetValue(pos, out sketchThing))
			{
				singleResult = sketchThing;
				multipleResults = null;
				return;
			}
			List<SketchThing> list;
			if (this.thingsAt_multiple.TryGetValue(pos, out list))
			{
				singleResult = null;
				multipleResults = list;
				return;
			}
			singleResult = null;
			multipleResults = null;
		}

		// Token: 0x06007C02 RID: 31746 RVA: 0x002C3788 File Offset: 0x002C1988
		public SketchThing EdificeAt(IntVec3 pos)
		{
			SketchThing result;
			if (this.edificeAt.TryGetValue(pos, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06007C03 RID: 31747 RVA: 0x002C37A8 File Offset: 0x002C19A8
		public bool WouldCollide(SketchEntity entity)
		{
			if (this.entities.Contains(entity))
			{
				return false;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				return this.WouldCollide(sketchThing.def, sketchThing.pos, sketchThing.rot);
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			return sketchTerrain != null && this.WouldCollide(sketchTerrain.def, sketchTerrain.pos);
		}

		// Token: 0x06007C04 RID: 31748 RVA: 0x002C3808 File Offset: 0x002C1A08
		public bool WouldCollide(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			CellRect cellRect = GenAdj.OccupiedRect(pos, rot, def.size);
			if (def.terrainAffordanceNeeded != null)
			{
				foreach (IntVec3 pos2 in cellRect)
				{
					TerrainDef terrainDef = this.TerrainAt(pos2);
					if (terrainDef != null && !terrainDef.affordances.Contains(def.terrainAffordanceNeeded))
					{
						return true;
					}
				}
			}
			for (int i = 0; i < this.cachedThings.Count; i++)
			{
				if (cellRect.Overlaps(this.cachedThings[i].OccupiedRect))
				{
					if (def.race != null)
					{
						if (this.cachedThings[i].def.passability == Traversability.Impassable)
						{
							return true;
						}
					}
					else if (!GenConstruct.CanPlaceBlueprintOver(def, this.cachedThings[i].def, null, null))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007C05 RID: 31749 RVA: 0x002C3908 File Offset: 0x002C1B08
		public bool WouldCollide(TerrainDef def, IntVec3 pos)
		{
			if (!def.layerable && this.TerrainAt(pos) != null)
			{
				return true;
			}
			for (int i = 0; i < this.cachedThings.Count; i++)
			{
				if (this.cachedThings[i].OccupiedRect.Contains(pos) && this.cachedThings[i].def.terrainAffordanceNeeded != null && !def.affordances.Contains(this.cachedThings[i].def.terrainAffordanceNeeded))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C06 RID: 31750 RVA: 0x002C3998 File Offset: 0x002C1B98
		public void WipeColliding(SketchEntity entity)
		{
			if (!this.WouldCollide(entity))
			{
				return;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				this.WipeColliding(sketchThing.def, sketchThing.pos, sketchThing.rot);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				this.WipeColliding(sketchTerrain.def, sketchTerrain.pos);
			}
		}

		// Token: 0x06007C07 RID: 31751 RVA: 0x002C39F0 File Offset: 0x002C1BF0
		public void WipeColliding(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			if (!this.WouldCollide(def, pos, rot))
			{
				return;
			}
			CellRect cellRect = GenAdj.OccupiedRect(pos, rot, def.size);
			if (def.terrainAffordanceNeeded != null)
			{
				foreach (IntVec3 intVec in cellRect)
				{
					TerrainDef terrainDef = this.TerrainAt(intVec);
					if (terrainDef != null && !terrainDef.affordances.Contains(def.terrainAffordanceNeeded))
					{
						this.RemoveTerrain(intVec);
					}
				}
			}
			for (int i = this.cachedThings.Count - 1; i >= 0; i--)
			{
				if (cellRect.Overlaps(this.cachedThings[i].OccupiedRect) && !GenConstruct.CanPlaceBlueprintOver(def, this.cachedThings[i].def, null, null))
				{
					this.Remove(this.cachedThings[i]);
				}
			}
		}

		// Token: 0x06007C08 RID: 31752 RVA: 0x002C3AE8 File Offset: 0x002C1CE8
		public void WipeColliding(TerrainDef def, IntVec3 pos)
		{
			if (!this.WouldCollide(def, pos))
			{
				return;
			}
			if (!def.layerable && this.TerrainAt(pos) != null)
			{
				this.RemoveTerrain(pos);
			}
			for (int i = this.cachedThings.Count - 1; i >= 0; i--)
			{
				if (this.cachedThings[i].OccupiedRect.Contains(pos) && this.cachedThings[i].def.terrainAffordanceNeeded != null && !def.affordances.Contains(this.cachedThings[i].def.terrainAffordanceNeeded))
				{
					this.Remove(this.cachedThings[i]);
				}
			}
		}

		// Token: 0x06007C09 RID: 31753 RVA: 0x002C3B9C File Offset: 0x002C1D9C
		public bool IsSpawningBlocked(Map map, IntVec3 pos, Faction faction, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			for (int i = 0; i < this.entities.Count; i++)
			{
				if (this.entities[i].IsSpawningBlocked(this.entities[i].pos + offset, map, null, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C0A RID: 31754 RVA: 0x002C3BFC File Offset: 0x002C1DFC
		public bool AnyThingOutOfBounds(Map map, IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			for (int i = 0; i < this.entities.Count; i++)
			{
				SketchThing sketchThing = this.entities[i] as SketchThing;
				if (sketchThing != null)
				{
					if (sketchThing.def.size == IntVec2.One)
					{
						if (!(this.entities[i].pos + offset).InBounds(map))
						{
							return true;
						}
					}
					else
					{
						using (CellRect.Enumerator enumerator = sketchThing.OccupiedRect.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (!(enumerator.Current + offset).InBounds(map))
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007C0B RID: 31755 RVA: 0x002C3CD4 File Offset: 0x002C1ED4
		public void Spawn(Map map, IntVec3 pos, Faction faction, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, bool clearEdificeWhereFloor = false, List<Thing> spawnedThings = null, bool dormant = false, bool buildRoofsInstantly = false, Func<SketchEntity, IntVec3, bool> canSpawnThing = null, Action<IntVec3, SketchEntity> onFailedToSpawnThing = null, TerrainDef defaultAffordanceTerrain = null)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			if (clearEdificeWhereFloor)
			{
				for (int i = 0; i < this.cachedTerrain.Count; i++)
				{
					if (this.cachedTerrain[i].def.layerable)
					{
						Building edifice = (this.cachedTerrain[i].pos + offset).GetEdifice(map);
						if (edifice != null)
						{
							edifice.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
			foreach (SketchEntity sketchEntity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				IntVec3 intVec = sketchEntity.pos + offset;
				if (((canSpawnThing != null && !canSpawnThing(sketchEntity, intVec)) || !sketchEntity.Spawn(intVec, map, faction, spawnMode, wipeIfCollides, forceTerrainAffordance, spawnedThings, dormant, defaultAffordanceTerrain)) && onFailedToSpawnThing != null)
				{
					onFailedToSpawnThing(intVec, sketchEntity);
				}
			}
			if (spawnedThings != null && spawnMode == Sketch.SpawnMode.TransportPod && !wipeIfCollides)
			{
				bool flag = false;
				for (int j = 0; j < spawnedThings.Count; j++)
				{
					for (int k = j + 1; k < spawnedThings.Count; k++)
					{
						CellRect cellRect = GenAdj.OccupiedRect(spawnedThings[j].Position, spawnedThings[j].Rotation, spawnedThings[j].def.size);
						CellRect other = GenAdj.OccupiedRect(spawnedThings[k].Position, spawnedThings[k].Rotation, spawnedThings[k].def.size);
						if (cellRect.Overlaps(other) && (GenSpawn.SpawningWipes(spawnedThings[j].def, spawnedThings[k].def, false) || GenSpawn.SpawningWipes(spawnedThings[k].def, spawnedThings[j].def, false)))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					for (int l = 0; l < spawnedThings.Count; l++)
					{
						ActiveTransporterInfo activeTransporterInfo = spawnedThings[l].ParentHolder as ActiveTransporterInfo;
						if (activeTransporterInfo != null)
						{
							activeTransporterInfo.spawnWipeMode = null;
						}
					}
				}
			}
			if (buildRoofsInstantly && spawnMode == Sketch.SpawnMode.Normal)
			{
				foreach (IntVec3 a in this.GetSuggestedRoofCells())
				{
					IntVec3 c = a + offset;
					if (c.InBounds(map) && !c.Roofed(map))
					{
						map.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
					}
				}
			}
		}

		// Token: 0x06007C0C RID: 31756 RVA: 0x002C3FB8 File Offset: 0x002C21B8
		public void Merge(Sketch other, bool wipeIfCollides = true)
		{
			foreach (SketchEntity sketchEntity in from x in other.entities
			orderby x.SpawnOrder
			select x)
			{
				this.Add(sketchEntity.DeepCopy(), wipeIfCollides);
			}
		}

		// Token: 0x06007C0D RID: 31757 RVA: 0x002C4030 File Offset: 0x002C2230
		public void MergeAt(Sketch other, IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool wipeIfCollides = true)
		{
			Sketch sketch = other.DeepCopy();
			sketch.MoveBy(sketch.GetOffset(pos, posType));
			this.Merge(sketch, wipeIfCollides);
		}

		// Token: 0x06007C0E RID: 31758 RVA: 0x002C405C File Offset: 0x002C225C
		public void Subtract(Sketch other)
		{
			for (int i = 0; i < this.entities.Count; i++)
			{
				for (int j = 0; j < other.entities.Count; j++)
				{
					if (this.entities[i].SameForSubtracting(other.entities[j]))
					{
						this.Remove(this.entities[i]);
						i--;
						break;
					}
				}
			}
		}

		// Token: 0x06007C0F RID: 31759 RVA: 0x002C40CD File Offset: 0x002C22CD
		public void MoveBy(IntVec2 by)
		{
			this.MoveBy(by.ToIntVec3);
		}

		// Token: 0x06007C10 RID: 31760 RVA: 0x002C40DC File Offset: 0x002C22DC
		public void MoveBy(IntVec3 by)
		{
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				sketchEntity.pos += by;
			}
			this.RecacheAll();
		}

		// Token: 0x06007C11 RID: 31761 RVA: 0x002C4140 File Offset: 0x002C2340
		public void MoveOccupiedCenterToZero()
		{
			this.MoveBy(new IntVec3(-this.OccupiedCenter.x, 0, -this.OccupiedCenter.z));
		}

		// Token: 0x06007C12 RID: 31762 RVA: 0x002C4168 File Offset: 0x002C2368
		public Sketch DeepCopy()
		{
			Sketch sketch = new Sketch();
			foreach (SketchEntity sketchEntity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				sketch.Add(sketchEntity.DeepCopy(), true);
			}
			return sketch;
		}

		// Token: 0x06007C13 RID: 31763 RVA: 0x002C41E8 File Offset: 0x002C23E8
		private void AddToCache(SketchEntity entity)
		{
			this.occupiedRectDirty = true;
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				this.cachedBuildables.Add(sketchBuildable);
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				if (sketchThing.def.building != null && sketchThing.def.building.isEdifice)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						this.edificeAt[key] = sketchThing;
					}
				}
				foreach (IntVec3 key2 in sketchThing.OccupiedRect)
				{
					List<SketchThing> list;
					SketchThing item;
					if (this.thingsAt_multiple.TryGetValue(key2, out list))
					{
						list.Add(sketchThing);
					}
					else if (this.thingsAt_single.TryGetValue(key2, out item))
					{
						this.thingsAt_single.Remove(key2);
						List<SketchThing> list2 = new List<SketchThing>();
						list2.Add(item);
						list2.Add(sketchThing);
						this.thingsAt_multiple.Add(key2, list2);
					}
					else
					{
						this.thingsAt_single.Add(key2, sketchThing);
					}
				}
				this.cachedThings.Add(sketchThing);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				this.terrainAt[sketchTerrain.pos] = sketchTerrain;
				this.cachedTerrain.Add(sketchTerrain);
			}
		}

		// Token: 0x06007C14 RID: 31764 RVA: 0x002C437C File Offset: 0x002C257C
		private void RemoveFromCache(SketchEntity entity)
		{
			this.occupiedRectDirty = true;
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				this.cachedBuildables.Remove(sketchBuildable);
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				if (sketchThing.def.building != null && sketchThing.def.building.isEdifice)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						SketchThing sketchThing2;
						if (this.edificeAt.TryGetValue(key, out sketchThing2) && sketchThing2 == sketchThing)
						{
							this.edificeAt.Remove(key);
						}
					}
				}
				foreach (IntVec3 key2 in sketchThing.OccupiedRect)
				{
					List<SketchThing> list;
					SketchThing sketchThing3;
					if (this.thingsAt_multiple.TryGetValue(key2, out list))
					{
						list.Remove(sketchThing);
					}
					else if (this.thingsAt_single.TryGetValue(key2, out sketchThing3) && sketchThing3 == sketchThing)
					{
						this.thingsAt_single.Remove(key2);
					}
				}
				this.cachedThings.Remove(sketchThing);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				SketchTerrain sketchTerrain2;
				if (this.terrainAt.TryGetValue(sketchTerrain.pos, out sketchTerrain2) && sketchTerrain2 == sketchTerrain)
				{
					this.terrainAt.Remove(sketchTerrain.pos);
				}
				this.cachedTerrain.Remove(sketchTerrain);
			}
		}

		// Token: 0x06007C15 RID: 31765 RVA: 0x002C4510 File Offset: 0x002C2710
		private void Recache(SketchEntity entity)
		{
			this.RemoveFromCache(entity);
			this.AddToCache(entity);
		}

		// Token: 0x06007C16 RID: 31766 RVA: 0x002C4520 File Offset: 0x002C2720
		public void RecacheAll()
		{
			this.terrainAt.Clear();
			this.edificeAt.Clear();
			this.thingsAt_single.Clear();
			this.cachedThings.Clear();
			this.cachedTerrain.Clear();
			this.cachedBuildables.Clear();
			this.occupiedRectDirty = true;
			foreach (KeyValuePair<IntVec3, List<SketchThing>> keyValuePair in this.thingsAt_multiple)
			{
				keyValuePair.Value.Clear();
			}
			foreach (SketchEntity entity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				this.AddToCache(entity);
			}
		}

		// Token: 0x06007C17 RID: 31767 RVA: 0x002C4624 File Offset: 0x002C2824
		public bool LineOfSight(IntVec3 start, IntVec3 end, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if (!skipFirstCell || !(intVec == start))
				{
					if (!this.CanBeSeenOver(intVec))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06007C18 RID: 31768 RVA: 0x002C469C File Offset: 0x002C289C
		public bool LineOfSight(IntVec3 start, IntVec3 end, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if (endRect.Contains(intVec))
				{
					return true;
				}
				if (!startRect.Contains(intVec))
				{
					if (!this.CanBeSeenOver(intVec))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06007C19 RID: 31769 RVA: 0x002C4720 File Offset: 0x002C2920
		public bool CanBeSeenOver(IntVec3 c)
		{
			SketchThing sketchThing = this.EdificeAt(c);
			return sketchThing == null || sketchThing.def.Fillage != FillCategory.Full;
		}

		// Token: 0x06007C1A RID: 31770 RVA: 0x002C474B File Offset: 0x002C294B
		public bool Passable(int x, int z)
		{
			return this.Passable(new IntVec3(x, 0, z));
		}

		// Token: 0x06007C1B RID: 31771 RVA: 0x002C475C File Offset: 0x002C295C
		public bool Passable(IntVec3 pos)
		{
			TerrainDef terrainDef = this.TerrainAt(pos);
			if (terrainDef != null && terrainDef.passability == Traversability.Impassable)
			{
				return false;
			}
			using (IEnumerator<SketchThing> enumerator = this.ThingsAt(pos).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.passability == Traversability.Impassable)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x002C47CC File Offset: 0x002C29CC
		public void DrawGhost(IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool placingMode = false, Thing thingToIgnore = null, Func<SketchEntity, IntVec3, List<Thing>, Map, bool> validator = null)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			Map currentMap = Find.CurrentMap;
			bool flag = false;
			using (List<SketchEntity>.Enumerator enumerator = this.Entities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.OccupiedRect.MovedBy(offset).InBounds(currentMap))
					{
						flag = true;
						break;
					}
				}
			}
			foreach (SketchBuildable sketchBuildable in this.Buildables)
			{
				Thing spawnedBlueprintOrFrame = sketchBuildable.GetSpawnedBlueprintOrFrame(sketchBuildable.pos + offset, currentMap);
				if (spawnedBlueprintOrFrame != null)
				{
					Sketch.tmpSketchThings.Add(spawnedBlueprintOrFrame);
				}
				else
				{
					SketchThing sketchThing = sketchBuildable as SketchThing;
					if (sketchThing != null)
					{
						Thing sameSpawned = sketchThing.GetSameSpawned(sketchThing.pos + offset, currentMap);
						if (sameSpawned != null)
						{
							Sketch.tmpSketchThings.Add(sameSpawned);
						}
					}
				}
			}
			CellRect cellRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(1).ClipInsideMap(Find.CurrentMap);
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				if ((placingMode || !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + offset, currentMap)) && sketchEntity.OccupiedRect.MovedBy(offset).InBounds(currentMap))
				{
					Color color = (flag || (sketchEntity.IsSpawningBlocked(sketchEntity.pos + offset, currentMap, thingToIgnore, false) && !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + offset, currentMap)) || (validator != null && !validator(sketchEntity, offset, Sketch.tmpSketchThings, Find.CurrentMap))) ? Sketch.BlockedColor : Sketch.GhostColor;
					if (cellRect.Contains(sketchEntity.pos + offset))
					{
						sketchEntity.DrawGhost(sketchEntity.pos + offset, color);
					}
				}
			}
			Sketch.tmpSketchThings.Clear();
		}

		// Token: 0x06007C1D RID: 31773 RVA: 0x002C4A18 File Offset: 0x002C2C18
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Func<IntVec3, int, bool> processor, int maxCellsToProcess = 2147483647, CellRect? bounds = null, IEnumerable<IntVec3> extraRoots = null)
		{
			if (this.floodFillWorking)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.");
			}
			this.floodFillWorking = true;
			if (this.floodFillOpenSet == null)
			{
				this.floodFillOpenSet = new Queue<IntVec3>();
			}
			if (this.floodFillTraversalDistance == null)
			{
				this.floodFillTraversalDistance = new Dictionary<IntVec3, int>();
			}
			this.floodFillTraversalDistance.Clear();
			this.floodFillOpenSet.Clear();
			if (root.IsValid && extraRoots == null && !passCheck(root))
			{
				this.floodFillWorking = false;
				return;
			}
			if (bounds == null)
			{
				bounds = new CellRect?(this.OccupiedRect);
			}
			int area = bounds.Value.Area;
			IntVec3[] cardinalDirectionsAround = GenAdj.CardinalDirectionsAround;
			int num = cardinalDirectionsAround.Length;
			int num2 = 0;
			if (root.IsValid)
			{
				this.floodFillTraversalDistance.Add(root, 0);
				this.floodFillOpenSet.Enqueue(root);
			}
			if (extraRoots == null)
			{
				goto IL_23D;
			}
			IList<IntVec3> list = extraRoots as IList<IntVec3>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.floodFillTraversalDistance.SetOrAdd(list[i], 0);
					this.floodFillOpenSet.Enqueue(list[i]);
				}
				goto IL_23D;
			}
			using (IEnumerator<IntVec3> enumerator = extraRoots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 intVec = enumerator.Current;
					this.floodFillTraversalDistance.SetOrAdd(intVec, 0);
					this.floodFillOpenSet.Enqueue(intVec);
				}
				goto IL_23D;
			}
			IL_169:
			IntVec3 intVec2 = this.floodFillOpenSet.Dequeue();
			int num3 = this.floodFillTraversalDistance[intVec2];
			if (processor(intVec2, num3))
			{
				goto IL_24E;
			}
			num2++;
			if (num2 == maxCellsToProcess)
			{
				goto IL_24E;
			}
			for (int j = 0; j < num; j++)
			{
				IntVec3 intVec3 = intVec2 + cardinalDirectionsAround[j];
				if (bounds.Value.Contains(intVec3) && !this.floodFillTraversalDistance.ContainsKey(intVec3) && passCheck(intVec3))
				{
					this.floodFillOpenSet.Enqueue(intVec3);
					this.floodFillTraversalDistance.Add(intVec3, num3 + 1);
				}
			}
			if (this.floodFillOpenSet.Count > area)
			{
				Log.Error("Overflow on flood fill (>" + area.ToString() + " cells). Make sure we're not flooding over the same area after we check it.");
				this.floodFillWorking = false;
				return;
			}
			IL_23D:
			if (this.floodFillOpenSet.Count > 0)
			{
				goto IL_169;
			}
			IL_24E:
			this.floodFillWorking = false;
		}

		// Token: 0x06007C1E RID: 31774 RVA: 0x002C4C8C File Offset: 0x002C2E8C
		public IEnumerable<IntVec3> GetSuggestedRoofCells()
		{
			Sketch.<GetSuggestedRoofCells>d__83 <GetSuggestedRoofCells>d__ = new Sketch.<GetSuggestedRoofCells>d__83(-2);
			<GetSuggestedRoofCells>d__.<>4__this = this;
			return <GetSuggestedRoofCells>d__;
		}

		// Token: 0x06007C1F RID: 31775 RVA: 0x002C4C9C File Offset: 0x002C2E9C
		private IntVec3 GetOffset(IntVec3 pos, Sketch.SpawnPosType posType)
		{
			IntVec3 intVec;
			switch (posType)
			{
			case Sketch.SpawnPosType.Unchanged:
				intVec = IntVec3.Zero;
				break;
			case Sketch.SpawnPosType.OccupiedCenter:
				intVec = new IntVec3(-this.OccupiedCenter.x, 0, -this.OccupiedCenter.z);
				break;
			case Sketch.SpawnPosType.OccupiedBotLeft:
				intVec = new IntVec3(-this.OccupiedRect.minX, 0, -this.OccupiedRect.minZ);
				break;
			default:
				intVec = default(IntVec3);
				break;
			}
			intVec += pos;
			return intVec;
		}

		// Token: 0x06007C20 RID: 31776 RVA: 0x002C4D1C File Offset: 0x002C2F1C
		public void Rotate(Rot4 rot)
		{
			if (rot == this.rotation)
			{
				return;
			}
			RotationDirection relativeRotation = Rot4.GetRelativeRotation(this.rotation, rot);
			this.rotation = rot;
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				sketchEntity.pos = sketchEntity.pos.RotatedBy(relativeRotation);
				SketchThing sketchThing = sketchEntity as SketchThing;
				if (sketchThing != null)
				{
					if (sketchThing.def.rotatable)
					{
						sketchThing.rot.Rotate(relativeRotation);
					}
					else if (relativeRotation == RotationDirection.Clockwise && sketchThing.def.size.z % 2 == 0)
					{
						SketchEntity sketchEntity2 = sketchEntity;
						sketchEntity2.pos.z = sketchEntity2.pos.z - 1;
					}
					else if (relativeRotation == RotationDirection.Opposite)
					{
						if (sketchThing.def.size.x % 2 == 0)
						{
							SketchEntity sketchEntity3 = sketchEntity;
							sketchEntity3.pos.x = sketchEntity3.pos.x - 1;
						}
						if (sketchThing.def.size.z % 2 == 0)
						{
							SketchEntity sketchEntity4 = sketchEntity;
							sketchEntity4.pos.z = sketchEntity4.pos.z - 1;
						}
					}
					else if (relativeRotation == RotationDirection.Counterclockwise && sketchThing.def.size.x % 2 == 0)
					{
						SketchEntity sketchEntity5 = sketchEntity;
						sketchEntity5.pos.x = sketchEntity5.pos.x - 1;
					}
				}
			}
			this.RecacheAll();
		}

		// Token: 0x06007C21 RID: 31777 RVA: 0x002C4E74 File Offset: 0x002C3074
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SketchEntity>(ref this.entities, "entities", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<Rot4>(ref this.rotation, "rotation", Rot4.North, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.entities.RemoveAll((SketchEntity x) => x == null) != 0)
				{
					Log.Error("Some sketch entities were null after loading.");
				}
				if (this.entities.RemoveAll((SketchEntity x) => x.LostImportantReferences) != 0)
				{
					Log.Error("Some sketch entities had null defs after loading.");
				}
				this.RecacheAll();
				Sketch.tmpToRemove.Clear();
				for (int i = 0; i < this.cachedThings.Count; i++)
				{
					if (this.cachedThings[i].def.IsDoor)
					{
						for (int j = 0; j < this.cachedThings.Count; j++)
						{
							if (this.cachedThings[j].def == ThingDefOf.Wall && this.cachedThings[j].pos == this.cachedThings[i].pos)
							{
								Sketch.tmpToRemove.Add(this.cachedThings[j]);
							}
						}
					}
				}
				for (int k = 0; k < Sketch.tmpToRemove.Count; k++)
				{
					Log.Error("Sketch has a wall and a door in the same cell. Fixing.");
					this.Remove(Sketch.tmpToRemove[k]);
				}
				Sketch.tmpToRemove.Clear();
			}
		}

		// Token: 0x06007C22 RID: 31778 RVA: 0x002C5010 File Offset: 0x002C3210
		public void AddPrefab(PrefabDef prefab, IntVec3 pos, Rot4 rot)
		{
			rot = PrefabUtility.ValidateRotation(prefab, rot);
			IntVec3 root = PrefabUtility.GetRoot(prefab, pos, rot);
			foreach (ValueTuple<PrefabTerrainData, IntVec3> valueTuple in prefab.GetTerrain())
			{
				PrefabTerrainData item = valueTuple.Item1;
				IntVec3 item2 = valueTuple.Item2;
				if (Rand.Chance(item.chance))
				{
					IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(item2, rot);
					this.AddTerrain(item.def, root + adjustedLocalPosition, true);
				}
			}
			foreach (ValueTuple<PrefabThingData, IntVec3> valueTuple2 in prefab.GetThings())
			{
				PrefabThingData item3 = valueTuple2.Item1;
				IntVec3 item4 = valueTuple2.Item2;
				if (Rand.Chance(item3.chance))
				{
					IntVec3 adjustedThingLocalPosition = PrefabUtility.GetAdjustedThingLocalPosition(item3, rot, item4);
					this.AddThing(item3.def, root + adjustedThingLocalPosition, rot.Rotated(item3.relativeRotation), item3.stuff, item3.stackCountRange.RandomInRange, null, null, true, 2f);
				}
			}
			foreach (ValueTuple<SubPrefabData, IntVec3> valueTuple3 in prefab.GetPrefabs())
			{
				SubPrefabData item5 = valueTuple3.Item1;
				IntVec3 item6 = valueTuple3.Item2;
				if (Rand.Chance(item5.chance))
				{
					IntVec3 adjustedPrefabLocalPosition = PrefabUtility.GetAdjustedPrefabLocalPosition(item5, rot, item6);
					this.AddPrefab(item5.def, root + adjustedPrefabLocalPosition, rot.Rotated(item5.relativeRotation));
				}
			}
		}

		// Token: 0x06007C25 RID: 31781 RVA: 0x002C52E8 File Offset: 0x002C34E8
		[CompilerGenerated]
		private bool <GetSuggestedRoofCells>g__AnyRoofHolderAt|83_0(IntVec3 c)
		{
			SketchThing sketchThing = this.EdificeAt(c);
			return sketchThing != null && sketchThing.def.holdsRoof;
		}

		// Token: 0x04004E22 RID: 20002
		private List<SketchEntity> entities = new List<SketchEntity>();

		// Token: 0x04004E23 RID: 20003
		private readonly List<SketchThing> cachedThings = new List<SketchThing>();

		// Token: 0x04004E24 RID: 20004
		private readonly List<SketchTerrain> cachedTerrain = new List<SketchTerrain>();

		// Token: 0x04004E25 RID: 20005
		private readonly List<SketchBuildable> cachedBuildables = new List<SketchBuildable>();

		// Token: 0x04004E26 RID: 20006
		private readonly Dictionary<IntVec3, SketchTerrain> terrainAt = new Dictionary<IntVec3, SketchTerrain>();

		// Token: 0x04004E27 RID: 20007
		private readonly Dictionary<IntVec3, SketchThing> edificeAt = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x04004E28 RID: 20008
		private readonly Dictionary<IntVec3, SketchThing> thingsAt_single = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x04004E29 RID: 20009
		private readonly Dictionary<IntVec3, List<SketchThing>> thingsAt_multiple = new Dictionary<IntVec3, List<SketchThing>>();

		// Token: 0x04004E2A RID: 20010
		private bool occupiedRectDirty = true;

		// Token: 0x04004E2B RID: 20011
		private CellRect cachedOccupiedRect;

		// Token: 0x04004E2C RID: 20012
		private Rot4 rotation = Rot4.North;

		// Token: 0x04004E2D RID: 20013
		private bool floodFillWorking;

		// Token: 0x04004E2E RID: 20014
		private Queue<IntVec3> floodFillOpenSet;

		// Token: 0x04004E2F RID: 20015
		private Dictionary<IntVec3, int> floodFillTraversalDistance;

		// Token: 0x04004E30 RID: 20016
		public const float SpawnOrder_BeforeTerrain = 0.5f;

		// Token: 0x04004E31 RID: 20017
		public const float SpawnOrder_Terrain = 1f;

		// Token: 0x04004E32 RID: 20018
		public const float SpawnOrder_Thing = 2f;

		// Token: 0x04004E33 RID: 20019
		private static readonly List<SketchThing> EmptySketchThingList = new List<SketchThing>();

		// Token: 0x04004E34 RID: 20020
		private static readonly Color GhostColor = new Color(0.7f, 0.7f, 0.7f, 0.35f);

		// Token: 0x04004E35 RID: 20021
		private static readonly Color BlockedColor = new Color(0.8f, 0.2f, 0.2f, 0.35f);

		// Token: 0x04004E36 RID: 20022
		private static readonly List<Thing> tmpSketchThings = new List<Thing>();

		// Token: 0x04004E37 RID: 20023
		private static readonly HashSet<IntVec3> tmpSuggestedRoofCellsVisited = new HashSet<IntVec3>();

		// Token: 0x04004E38 RID: 20024
		private static readonly List<IntVec3> tmpSuggestedRoofCells = new List<IntVec3>();

		// Token: 0x04004E39 RID: 20025
		private static readonly HashSet<IntVec3> tmpYieldedSuggestedRoofCells = new HashSet<IntVec3>();

		// Token: 0x04004E3A RID: 20026
		private static readonly List<SketchThing> tmpToRemove = new List<SketchThing>();

		// Token: 0x02003279 RID: 12921
		public enum SpawnPosType
		{
			// Token: 0x0400CAD2 RID: 51922
			Unchanged,
			// Token: 0x0400CAD3 RID: 51923
			OccupiedCenter,
			// Token: 0x0400CAD4 RID: 51924
			OccupiedBotLeft
		}

		// Token: 0x0200327A RID: 12922
		public enum SpawnMode
		{
			// Token: 0x0400CAD6 RID: 51926
			Blueprint,
			// Token: 0x0400CAD7 RID: 51927
			Normal,
			// Token: 0x0400CAD8 RID: 51928
			TransportPod
		}
	}
}
