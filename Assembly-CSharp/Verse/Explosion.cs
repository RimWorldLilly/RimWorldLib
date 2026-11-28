using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200054E RID: 1358
	public class Explosion : Thing
	{
		// Token: 0x0600285C RID: 10332 RVA: 0x000F92A8 File Offset: 0x000F74A8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.cellsToAffect = SimplePool<List<IntVec3>>.Get();
				this.cellsToAffect.Clear();
				this.damagedThings = SimplePool<List<Thing>>.Get();
				this.damagedThings.Clear();
				this.addedCellsAffectedOnlyByDamage = SimplePool<HashSet<IntVec3>>.Get();
				this.addedCellsAffectedOnlyByDamage.Clear();
			}
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000F930C File Offset: 0x000F750C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.cellsToAffect.Clear();
			SimplePool<List<IntVec3>>.Return(this.cellsToAffect);
			this.cellsToAffect = null;
			this.damagedThings.Clear();
			SimplePool<List<Thing>>.Return(this.damagedThings);
			this.damagedThings = null;
			this.addedCellsAffectedOnlyByDamage.Clear();
			SimplePool<HashSet<IntVec3>>.Return(this.addedCellsAffectedOnlyByDamage);
			this.addedCellsAffectedOnlyByDamage = null;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000F9378 File Offset: 0x000F7578
		public virtual void StartExplosion(SoundDef explosionSound, List<Thing> ignoredThings)
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartExplosion() on unspawned thing.");
				return;
			}
			this.startTick = Find.TickManager.TicksGame;
			this.ignoredThings = ignoredThings;
			this.cellsToAffect.Clear();
			this.damagedThings.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			if (!this.overrideCells.NullOrEmpty<IntVec3>())
			{
				this.cellsToAffect.AddRange(this.overrideCells);
			}
			else
			{
				this.cellsToAffect.AddRange(this.damType.Worker.ExplosionCellsToHit(this));
			}
			if (this.applyDamageToExplosionCellsNeighbors)
			{
				this.AddCellsNeighbors(this.cellsToAffect);
			}
			this.damType.Worker.ExplosionStart(this, this.cellsToAffect);
			this.PlayExplosionSound(explosionSound);
			if (this.doVisualEffects)
			{
				FleckMaker.WaterSplash(base.Position.ToVector3Shifted(), base.Map, this.radius * 6f, 20f);
			}
			this.cellsToAffect.Sort((IntVec3 a, IntVec3 b) => this.GetCellAffectTick(b).CompareTo(this.GetCellAffectTick(a)));
			RegionTraverser.BreadthFirstTraverse(base.Position, base.Map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> allThings = x.ListerThings.AllThings;
				for (int i = allThings.Count - 1; i >= 0; i--)
				{
					if (allThings[i].Spawned)
					{
						allThings[i].Notify_Explosion(this);
					}
				}
				return false;
			}, 25, RegionType.Set_Passable);
			this.TrySpawnSingleThing(this.preExplosionSpawnSingleThingDef);
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000F94D4 File Offset: 0x000F76D4
		protected override void Tick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			int num = this.cellsToAffect.Count - 1;
			while (num >= 0 && ticksGame >= this.GetCellAffectTick(this.cellsToAffect[num]))
			{
				try
				{
					this.AffectCell(this.cellsToAffect[num]);
				}
				catch (Exception ex)
				{
					string str = "Explosion could not affect cell ";
					string str2 = this.cellsToAffect[num].ToString();
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				this.cellsToAffect.RemoveAt(num);
				num--;
			}
			if (!this.cellsToAffect.Any<IntVec3>())
			{
				this.ExplosionEnded();
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000F95A4 File Offset: 0x000F77A4
		protected virtual void ExplosionEnded()
		{
			this.TrySpawnSingleThing(this.postExplosionSpawnSingleThingDef);
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000F95B4 File Offset: 0x000F77B4
		private void TrySpawnSingleThing(ThingDef thingDef)
		{
			if (thingDef == null)
			{
				return;
			}
			CellRect cellRect = base.Position.RectAbout(thingDef.Size);
			bool flag = false;
			if (thingDef.terrainAffordanceNeeded != null)
			{
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.GetAffordances(base.Map).Contains(thingDef.terrainAffordanceNeeded))
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				this.TrySpawnExplosionThing(thingDef, base.Position, 1);
			}
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000F9648 File Offset: 0x000F7848
		public int GetDamageAmountAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.damAmount;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Max(GenMath.RoundRandom(Mathf.Lerp((float)this.damAmount, (float)this.damAmount * 0.2f, t)), 1);
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000F96A0 File Offset: 0x000F78A0
		public float GetArmorPenetrationAt(IntVec3 c)
		{
			if (!this.damageFalloff)
			{
				return this.armorPenetration;
			}
			float t = c.DistanceTo(base.Position) / this.radius;
			return Mathf.Lerp(this.armorPenetration, this.armorPenetration * 0.2f, t);
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000F96E8 File Offset: 0x000F78E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damType, "damType");
			Scribe_Values.Look<int>(ref this.damAmount, "damAmount", 0, false);
			Scribe_Values.Look<float>(ref this.armorPenetration, "armorPenetration", 0f, false);
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weapon, "weapon");
			Scribe_Defs.Look<ThingDef>(ref this.projectile, "projectile");
			Scribe_References.Look<Thing>(ref this.intendedTarget, "intendedTarget", false);
			Scribe_Values.Look<bool>(ref this.applyDamageToExplosionCellsNeighbors, "applyDamageToExplosionCellsNeighbors", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.preExplosionSpawnThingDef, "preExplosionSpawnThingDef");
			Scribe_Values.Look<float>(ref this.preExplosionSpawnChance, "preExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.preExplosionSpawnThingCount, "preExplosionSpawnThingCount", 1, false);
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDef, "postExplosionSpawnThingDef");
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnThingDefWater, "postExplosionSpawnThingDefWater");
			Scribe_Values.Look<float>(ref this.postExplosionSpawnChance, "postExplosionSpawnChance", 0f, false);
			Scribe_Values.Look<int>(ref this.postExplosionSpawnThingCount, "postExplosionSpawnThingCount", 1, false);
			Scribe_Values.Look<GasType?>(ref this.postExplosionGasType, "postExplosionGasType", null, false);
			Scribe_Values.Look<float>(ref this.chanceToStartFire, "chanceToStartFire", 0f, false);
			Scribe_Deep.Look<SimpleCurve>(ref this.flammabilityChanceCurve, "flammabilityChanceCurve", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.damageFalloff, "dealMoreDamageAtCenter", false, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell1, "needLOSToCell1", null, false);
			Scribe_Values.Look<IntVec3?>(ref this.needLOSToCell2, "needLOSToCell2", null, false);
			Scribe_Values.Look<FloatRange?>(ref this.affectedAngle, "affectedAngle", null, false);
			Scribe_Values.Look<float>(ref this.propagationSpeed, "propagationSpeed", 0f, false);
			Scribe_Values.Look<float>(ref this.excludeRadius, "canTargetLocations", 0f, false);
			Scribe_Values.Look<bool>(ref this.doSoundEffects, "doSoundEffects", true, false);
			Scribe_Values.Look<bool>(ref this.doVisualEffects, "doVisualEffects", true, false);
			Scribe_Values.Look<float>(ref this.screenShakeFactor, "screenShakeFactor", 1f, false);
			Scribe_Defs.Look<ThingDef>(ref this.postExplosionSpawnSingleThingDef, "postExplosionSpawnSingleThingDef");
			Scribe_Defs.Look<ThingDef>(ref this.preExplosionSpawnSingleThingDef, "preExplosionSpawnSingleThingDef");
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Collections.Look<IntVec3>(ref this.cellsToAffect, "cellsToAffect", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.damagedThings, "damagedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.ignoredThings, "ignoredThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.addedCellsAffectedOnlyByDamage, "addedCellsAffectedOnlyByDamage", LookMode.Value);
			Scribe_Collections.Look<IntVec3>(ref this.overrideCells, "overrideCells", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.damagedThings != null)
				{
					this.damagedThings.RemoveAll((Thing x) => x == null);
				}
				if (this.ignoredThings != null)
				{
					this.ignoredThings.RemoveAll((Thing x) => x == null);
				}
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000F9A34 File Offset: 0x000F7C34
		private int GetCellAffectTick(IntVec3 cell)
		{
			return this.startTick + (int)((cell - base.Position).LengthHorizontal * 1.5f / this.propagationSpeed);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x000F9A6C File Offset: 0x000F7C6C
		private void AffectCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return;
			}
			if (this.excludeRadius > 0f && (float)c.DistanceToSquared(base.Position) < this.excludeRadius * this.excludeRadius)
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(base.Map);
			bool flag = this.ShouldCellBeAffectedOnlyByDamage(c);
			if (!flag && Rand.Chance(this.preExplosionSpawnChance) && c.Walkable(base.Map))
			{
				this.TrySpawnExplosionThing(this.preExplosionSpawnThingDef, c, this.preExplosionSpawnThingCount);
			}
			this.damType.Worker.ExplosionAffectCell(this, c, this.damagedThings, this.ignoredThings, !flag);
			if (!flag)
			{
				if (Rand.Chance(this.postExplosionSpawnChance) && c.Walkable(base.Map))
				{
					ThingDef thingDef = terrain.IsWater ? (this.postExplosionSpawnThingDefWater ?? this.postExplosionSpawnThingDef) : this.postExplosionSpawnThingDef;
					this.TrySpawnExplosionThing(thingDef, c, this.postExplosionSpawnThingCount);
				}
				if (this.postExplosionGasType != null)
				{
					float num = this.postExplosionGasRadiusOverride ?? this.radius;
					float num2 = num * num;
					if ((float)c.DistanceToSquared(base.Position) <= num2)
					{
						GasUtility.AddGas(c, base.Map, this.postExplosionGasType.Value, this.postExplosionGasAmount);
					}
				}
			}
			float num3 = this.chanceToStartFire;
			if (this.damageFalloff)
			{
				num3 *= Mathf.Lerp(1f, 0.2f, c.DistanceTo(base.Position) / this.radius);
			}
			if (Rand.Chance(num3))
			{
				FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f), this.instigator, this.flammabilityChanceCurve);
			}
			if (terrain.temporary)
			{
				TempTerrainProps tempTerrain = terrain.tempTerrain;
				if (tempTerrain != null && tempTerrain.removedByExplosions)
				{
					base.Map.terrainGrid.RemoveTempTerrain(c, false, false);
				}
			}
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x000F9C5C File Offset: 0x000F7E5C
		private void TrySpawnExplosionThing(ThingDef thingDef, IntVec3 c, int count)
		{
			if (thingDef == null)
			{
				return;
			}
			if (thingDef.IsFilth)
			{
				FilthMaker.TryMakeFilth(c, base.Map, thingDef, count, FilthSourceFlags.None, true);
				return;
			}
			Thing thing;
			if (GenSpawn.TrySpawn(thingDef, c, base.Map, out thing, WipeMode.Vanish, true))
			{
				thing.stackCount = count;
				CompReleaseGas compReleaseGas = thing.TryGetComp<CompReleaseGas>();
				if (compReleaseGas == null)
				{
					return;
				}
				compReleaseGas.StartRelease();
			}
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x000F9CB4 File Offset: 0x000F7EB4
		private void PlayExplosionSound(SoundDef explosionSound)
		{
			if (!this.doSoundEffects)
			{
				return;
			}
			bool flag;
			if (Prefs.DevMode)
			{
				flag = (explosionSound != null);
			}
			else
			{
				flag = !explosionSound.NullOrUndefined();
			}
			if (flag)
			{
				explosionSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				return;
			}
			this.damType.soundExplosion.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x000F9D2C File Offset: 0x000F7F2C
		private void AddCellsNeighbors(List<IntVec3> cells)
		{
			Explosion.tmpCells.Clear();
			this.addedCellsAffectedOnlyByDamage.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				Explosion.tmpCells.Add(cells[i]);
			}
			for (int j = 0; j < cells.Count; j++)
			{
				if (cells[j].Walkable(base.Map))
				{
					for (int k = 0; k < GenAdj.AdjacentCells.Length; k++)
					{
						IntVec3 intVec = cells[j] + GenAdj.AdjacentCells[k];
						if (intVec.InBounds(base.Map) && Explosion.tmpCells.Add(intVec))
						{
							this.addedCellsAffectedOnlyByDamage.Add(intVec);
						}
					}
				}
			}
			cells.Clear();
			foreach (IntVec3 item in Explosion.tmpCells)
			{
				cells.Add(item);
			}
			Explosion.tmpCells.Clear();
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x000F9E40 File Offset: 0x000F8040
		private bool ShouldCellBeAffectedOnlyByDamage(IntVec3 c)
		{
			return this.applyDamageToExplosionCellsNeighbors && this.addedCellsAffectedOnlyByDamage.Contains(c);
		}

		// Token: 0x04001C6B RID: 7275
		public float radius;

		// Token: 0x04001C6C RID: 7276
		public DamageDef damType;

		// Token: 0x04001C6D RID: 7277
		public int damAmount;

		// Token: 0x04001C6E RID: 7278
		public float armorPenetration;

		// Token: 0x04001C6F RID: 7279
		public Thing instigator;

		// Token: 0x04001C70 RID: 7280
		public ThingDef weapon;

		// Token: 0x04001C71 RID: 7281
		public ThingDef projectile;

		// Token: 0x04001C72 RID: 7282
		public Thing intendedTarget;

		// Token: 0x04001C73 RID: 7283
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04001C74 RID: 7284
		public FloatRange? affectedAngle;

		// Token: 0x04001C75 RID: 7285
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04001C76 RID: 7286
		public float preExplosionSpawnChance;

		// Token: 0x04001C77 RID: 7287
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04001C78 RID: 7288
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04001C79 RID: 7289
		public ThingDef postExplosionSpawnThingDefWater;

		// Token: 0x04001C7A RID: 7290
		public float postExplosionSpawnChance;

		// Token: 0x04001C7B RID: 7291
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04001C7C RID: 7292
		public GasType? postExplosionGasType;

		// Token: 0x04001C7D RID: 7293
		public int postExplosionGasAmount = 255;

		// Token: 0x04001C7E RID: 7294
		public float? postExplosionGasRadiusOverride;

		// Token: 0x04001C7F RID: 7295
		public float chanceToStartFire;

		// Token: 0x04001C80 RID: 7296
		public SimpleCurve flammabilityChanceCurve;

		// Token: 0x04001C81 RID: 7297
		public bool damageFalloff;

		// Token: 0x04001C82 RID: 7298
		public IntVec3? needLOSToCell1;

		// Token: 0x04001C83 RID: 7299
		public IntVec3? needLOSToCell2;

		// Token: 0x04001C84 RID: 7300
		public bool doVisualEffects = true;

		// Token: 0x04001C85 RID: 7301
		public float propagationSpeed = 1f;

		// Token: 0x04001C86 RID: 7302
		public float excludeRadius;

		// Token: 0x04001C87 RID: 7303
		public bool doSoundEffects = true;

		// Token: 0x04001C88 RID: 7304
		public float screenShakeFactor = 1f;

		// Token: 0x04001C89 RID: 7305
		public List<IntVec3> overrideCells;

		// Token: 0x04001C8A RID: 7306
		public ThingDef preExplosionSpawnSingleThingDef;

		// Token: 0x04001C8B RID: 7307
		public ThingDef postExplosionSpawnSingleThingDef;

		// Token: 0x04001C8C RID: 7308
		private int startTick;

		// Token: 0x04001C8D RID: 7309
		private List<IntVec3> cellsToAffect;

		// Token: 0x04001C8E RID: 7310
		private List<Thing> damagedThings;

		// Token: 0x04001C8F RID: 7311
		private List<Thing> ignoredThings;

		// Token: 0x04001C90 RID: 7312
		private HashSet<IntVec3> addedCellsAffectedOnlyByDamage;

		// Token: 0x04001C91 RID: 7313
		private const float DamageFactorAtEdge = 0.2f;

		// Token: 0x04001C92 RID: 7314
		private static HashSet<IntVec3> tmpCells = new HashSet<IntVec3>();
	}
}
