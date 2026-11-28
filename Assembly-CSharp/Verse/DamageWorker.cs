using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200013C RID: 316
	public class DamageWorker
	{
		// Token: 0x060009FC RID: 2556 RVA: 0x0003738C File Offset: 0x0003558C
		public virtual DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (victim.SpawnedOrAnyParentSpawned)
			{
				ImpactSoundUtility.PlayImpactSound(victim, dinfo.Def.impactSoundType, victim.MapHeld);
			}
			if (victim.def.useHitPoints && dinfo.Def.harmsHealth)
			{
				float num = dinfo.Amount;
				if (victim.def.category == ThingCategory.Building)
				{
					num *= dinfo.Def.buildingDamageFactor;
					if (victim.def.passability == Traversability.Impassable)
					{
						num *= dinfo.Def.buildingDamageFactorImpassable;
					}
					else
					{
						num *= dinfo.Def.buildingDamageFactorPassable;
					}
					if (dinfo.Def.scaleDamageToBuildingsBasedOnFlammability)
					{
						num *= Mathf.Max(0.05f, victim.GetStatValue(StatDefOf.Flammability, true, -1));
					}
					Pawn pawn = dinfo.Instigator as Pawn;
					if (pawn != null && pawn.IsShambler)
					{
						num *= 1.5f;
					}
					if (ModsConfig.BiotechActive && dinfo.Instigator != null && (dinfo.WeaponBodyPartGroup != null || (dinfo.Weapon != null && dinfo.Weapon.IsMeleeWeapon)) && victim.def.IsDoor)
					{
						num *= dinfo.Instigator.GetStatValue(StatDefOf.MeleeDoorDamageFactor, true, -1);
					}
				}
				if (victim.def.category == ThingCategory.Plant)
				{
					num *= dinfo.Def.plantDamageFactor;
				}
				else if (victim.def.IsCorpse)
				{
					num *= dinfo.Def.corpseDamageFactor;
				}
				damageResult.totalDamageDealt = (float)Mathf.Min(victim.HitPoints, GenMath.RoundRandom(num));
				victim.HitPoints -= Mathf.RoundToInt(damageResult.totalDamageDealt);
				if (victim.HitPoints <= 0)
				{
					victim.HitPoints = 0;
					victim.Kill(new DamageInfo?(dinfo), null);
				}
			}
			return damageResult;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0003755C File Offset: 0x0003575C
		public virtual void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			if (this.def.explosionHeatEnergyPerCell > 1E-45f)
			{
				GenTemperature.PushHeat(explosion.Position, explosion.Map, this.def.explosionHeatEnergyPerCell * (float)cellsToAffect.Count);
			}
			if (explosion.doVisualEffects)
			{
				FleckMaker.Static(explosion.Position, explosion.Map, FleckDefOf.ExplosionFlash, explosion.radius * 6f);
				if (explosion.Map == Find.CurrentMap)
				{
					float magnitude = (explosion.Position.ToVector3Shifted() - Find.Camera.transform.position).magnitude;
					Find.CameraDriver.shaker.DoShake(4f * explosion.radius * explosion.screenShakeFactor / magnitude);
				}
				this.ExplosionVisualEffectCenter(explosion);
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00037630 File Offset: 0x00035830
		protected virtual void ExplosionVisualEffectCenter(Explosion explosion)
		{
			for (int i = 0; i < 4; i++)
			{
				FleckMaker.ThrowSmoke(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, explosion.radius * 0.6f);
			}
			if (this.def.explosionCenterFleck != null)
			{
				FleckMaker.Static(explosion.Position.ToVector3Shifted(), explosion.Map, this.def.explosionCenterFleck, 1f);
			}
			else if (this.def.explosionCenterMote != null)
			{
				MoteMaker.MakeStaticMote(explosion.Position.ToVector3Shifted(), explosion.Map, this.def.explosionCenterMote, 1f, false, 0f);
			}
			if (this.def.explosionCenterEffecter != null)
			{
				this.def.explosionCenterEffecter.Spawn(explosion.Position, explosion.Map, Vector3.zero, 1f);
			}
			if (this.def.explosionInteriorMote != null || this.def.explosionInteriorFleck != null || this.def.explosionInteriorEffecter != null)
			{
				int num = Mathf.RoundToInt(3.1415927f * explosion.radius * explosion.radius / 6f * this.def.explosionInteriorCellCountMultiplier);
				for (int j = 0; j < num; j++)
				{
					Vector3 b = Gen.RandomHorizontalVector(explosion.radius * this.def.explosionInteriorCellDistanceMultiplier);
					if (this.def.explosionInteriorEffecter != null)
					{
						Vector3 vect = explosion.Position.ToVector3Shifted() + b;
						this.def.explosionInteriorEffecter.Spawn(explosion.Position, vect.ToIntVec3(), explosion.Map, 1f);
					}
					else if (this.def.explosionInteriorFleck != null)
					{
						FleckMaker.ThrowExplosionInterior(explosion.Position.ToVector3Shifted() + b, explosion.Map, this.def.explosionInteriorFleck);
					}
					else
					{
						MoteMaker.ThrowExplosionInteriorMote(explosion.Position.ToVector3Shifted() + b, explosion.Map, this.def.explosionInteriorMote);
					}
				}
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00037864 File Offset: 0x00035A64
		public virtual void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			if (explosion.doVisualEffects && (this.def.explosionCellMote != null || this.def.explosionCellFleck != null) && canThrowMotes)
			{
				float t = Mathf.Clamp01((explosion.Position - c).LengthHorizontal / explosion.radius);
				Color color = Color.Lerp(this.def.explosionColorCenter, this.def.explosionColorEdge, t);
				if (this.def.explosionCellMote != null)
				{
					Mote mote = c.GetFirstThing(explosion.Map, this.def.explosionCellMote) as Mote;
					if (mote != null)
					{
						mote.spawnTick = Find.TickManager.TicksGame;
					}
					else
					{
						MoteMaker.ThrowExplosionCell(c, explosion.Map, this.def.explosionCellMote, color);
					}
				}
				else
				{
					FleckMaker.ThrowExplosionCell(c, explosion.Map, this.def.explosionCellFleck, color);
				}
			}
			if (this.def.explosionCellEffecter != null && (this.def.explosionCellEffecterMaxRadius < 1E-45f || c.InHorDistOf(explosion.Position, this.def.explosionCellEffecterMaxRadius)) && Rand.Chance(this.def.explosionCellEffecterChance))
			{
				this.def.explosionCellEffecter.Spawn(explosion.Position, c, explosion.Map, 1f);
			}
			DamageWorker.thingsToAffect.Clear();
			float num = float.MinValue;
			bool flag = false;
			List<Thing> list = explosion.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.category != ThingCategory.Mote && thing.def.category != ThingCategory.Ethereal)
				{
					DamageWorker.thingsToAffect.Add(thing);
					if (thing.def.Fillage == FillCategory.Full && thing.def.Altitude > num)
					{
						flag = true;
						num = thing.def.Altitude;
					}
				}
			}
			for (int j = 0; j < DamageWorker.thingsToAffect.Count; j++)
			{
				if (DamageWorker.thingsToAffect[j].def.Altitude >= num)
				{
					this.ExplosionDamageThing(explosion, DamageWorker.thingsToAffect[j], damagedThings, ignoredThings, c);
				}
			}
			if (!flag)
			{
				this.ExplosionDamageTerrain(explosion, c);
			}
			if (this.def.explosionSnowMeltAmount > 0.0001f)
			{
				float lengthHorizontal = (c - explosion.Position).LengthHorizontal;
				float num2 = 1f - lengthHorizontal / explosion.radius;
				if (num2 > 0f)
				{
					explosion.Map.snowGrid.AddDepth(c, -num2 * this.def.explosionSnowMeltAmount);
				}
			}
			if (this.def == DamageDefOf.Bomb || this.def == DamageDefOf.Flame)
			{
				List<Thing> list2 = explosion.Map.listerThings.ThingsOfDef(ThingDefOf.RectTrigger);
				for (int k = 0; k < list2.Count; k++)
				{
					RectTrigger rectTrigger = (RectTrigger)list2[k];
					if (rectTrigger.activateOnExplosion && rectTrigger.Rect.Contains(c))
					{
						rectTrigger.ActivatedBy(null);
					}
				}
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00037B94 File Offset: 0x00035D94
		protected virtual void ExplosionDamageThing(Explosion explosion, Thing t, List<Thing> damagedThings, List<Thing> ignoredThings, IntVec3 cell)
		{
			if (t.def.category == ThingCategory.Mote || t.def.category == ThingCategory.Ethereal)
			{
				return;
			}
			if (damagedThings.Contains(t))
			{
				return;
			}
			damagedThings.Add(t);
			if (ignoredThings != null && ignoredThings.Contains(t))
			{
				return;
			}
			if (this.def == DamageDefOf.Bomb && t.def == ThingDefOf.Fire && !t.Destroyed)
			{
				t.Destroy(DestroyMode.Vanish);
				return;
			}
			float angle;
			if (t.Position == explosion.Position)
			{
				angle = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				angle = (t.Position - explosion.Position).AngleFlat;
			}
			DamageInfo dinfo = new DamageInfo(this.def, (float)explosion.GetDamageAmountAt(cell), explosion.GetArmorPenetrationAt(cell), angle, explosion.instigator, null, explosion.weapon, DamageInfo.SourceCategory.ThingOrUnknown, explosion.intendedTarget, true, true, QualityCategory.Normal, true, false);
			if (this.def.explosionAffectOutsidePartsOnly)
			{
				dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
			}
			BattleLogEntry_ExplosionImpact battleLogEntry_ExplosionImpact = null;
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				battleLogEntry_ExplosionImpact = new BattleLogEntry_ExplosionImpact(explosion.instigator, t, explosion.weapon, explosion.projectile, this.def);
				Find.BattleLog.Add(battleLogEntry_ExplosionImpact);
			}
			DamageWorker.DamageResult damageResult = t.TakeDamage(dinfo);
			damageResult.AssociateWithLog(battleLogEntry_ExplosionImpact);
			if (pawn != null && damageResult.wounded && pawn.stances != null)
			{
				pawn.stances.stagger.StaggerFor(95, 0.17f);
			}
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00037D08 File Offset: 0x00035F08
		protected virtual void ExplosionDamageTerrain(Explosion explosion, IntVec3 c)
		{
			Map map = explosion.Map;
			if (this.def != DamageDefOf.Bomb)
			{
				return;
			}
			if (!map.terrainGrid.CanRemoveTopLayerAt(c) && !map.terrainGrid.CanRemoveFoundationAt(c))
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(map);
			TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
			float num = (float)explosion.GetDamageAmountAt(c);
			if (terrain.destroyOnBombDamageThreshold >= 0f && num >= terrain.destroyOnBombDamageThreshold)
			{
				map.terrainGrid.Notify_TerrainDestroyed(c);
			}
			if (terrainDef != null && terrainDef.destroyOnBombDamageThreshold >= 0f && num >= terrainDef.destroyOnBombDamageThreshold)
			{
				map.terrainGrid.Notify_FoundationDestroyed(c);
			}
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00037DAC File Offset: 0x00035FAC
		public IEnumerable<IntVec3> ExplosionCellsToHit(Explosion explosion)
		{
			return this.ExplosionCellsToHit(explosion.Position, explosion.Map, explosion.radius, explosion.needLOSToCell1, explosion.needLOSToCell2, explosion.affectedAngle);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00037DD8 File Offset: 0x00035FD8
		public virtual IEnumerable<IntVec3> ExplosionCellsToHit(IntVec3 center, Map map, float radius, IntVec3? needLOSToCell1 = null, IntVec3? needLOSToCell2 = null, FloatRange? affectedAngle = null)
		{
			DamageWorker.openCells.Clear();
			DamageWorker.adjWallCells.Clear();
			float num = (affectedAngle != null) ? affectedAngle.GetValueOrDefault().min : 0f;
			float num2 = (affectedAngle != null) ? affectedAngle.GetValueOrDefault().max : 0f;
			int num3 = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num3; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && GenSight.LineOfSight(center, intVec, map, true, null, 0, 0))
				{
					if (affectedAngle != null)
					{
						float lengthHorizontal = (intVec - center).LengthHorizontal;
						float num4 = lengthHorizontal / radius;
						if (lengthHorizontal <= 0.5f)
						{
							goto IL_16B;
						}
						float num5 = Mathf.Atan2((float)(-(float)(intVec.z - center.z)), (float)(intVec.x - center.x)) * 57.29578f;
						float num6 = num;
						float num7 = num2;
						if (num5 - num6 < -0.5f * num4 || num5 - num7 > 0.5f * num4)
						{
							goto IL_16B;
						}
					}
					if (needLOSToCell1 != null || needLOSToCell2 != null)
					{
						bool flag = needLOSToCell1 != null && GenSight.LineOfSight(needLOSToCell1.Value, intVec, map);
						bool flag2 = needLOSToCell2 != null && GenSight.LineOfSight(needLOSToCell2.Value, intVec, map);
						if (!flag && !flag2)
						{
							goto IL_16B;
						}
					}
					DamageWorker.openCells.Add(intVec);
				}
				IL_16B:;
			}
			for (int j = 0; j < DamageWorker.openCells.Count; j++)
			{
				IntVec3 intVec2 = DamageWorker.openCells[j];
				Building edifice = intVec2.GetEdifice(map);
				if (intVec2.Walkable(map))
				{
					if (edifice != null && edifice.def.Fillage == FillCategory.Full)
					{
						Building_Door building_Door = edifice as Building_Door;
						if (building_Door == null || !building_Door.Open)
						{
							goto IL_242;
						}
					}
					for (int k = 0; k < 4; k++)
					{
						IntVec3 intVec3 = intVec2 + GenAdj.CardinalDirections[k];
						if (intVec3.InHorDistOf(center, radius) && intVec3.InBounds(map) && !intVec3.Standable(map) && intVec3.GetEdifice(map) != null && !DamageWorker.openCells.Contains(intVec3) && !DamageWorker.adjWallCells.Contains(intVec3))
						{
							DamageWorker.adjWallCells.Add(intVec3);
						}
					}
				}
				IL_242:;
			}
			return DamageWorker.openCells.Concat(DamageWorker.adjWallCells);
		}

		// Token: 0x040005BA RID: 1466
		public DamageDef def;

		// Token: 0x040005BB RID: 1467
		private const float ExplosionCamShakeMultiplier = 4f;

		// Token: 0x040005BC RID: 1468
		private const float DamageToBuildingsFromFlammabilityMinFactor = 0.05f;

		// Token: 0x040005BD RID: 1469
		private static List<Thing> thingsToAffect = new List<Thing>();

		// Token: 0x040005BE RID: 1470
		private static List<IntVec3> openCells = new List<IntVec3>();

		// Token: 0x040005BF RID: 1471
		private static List<IntVec3> adjWallCells = new List<IntVec3>();

		// Token: 0x02002528 RID: 9512
		public class DamageResult
		{
			// Token: 0x170029ED RID: 10733
			// (get) Token: 0x0600ED46 RID: 60742 RVA: 0x00576EEF File Offset: 0x005750EF
			public BodyPartRecord LastHitPart
			{
				get
				{
					if (this.parts == null)
					{
						return null;
					}
					if (this.parts.Count <= 0)
					{
						return null;
					}
					return this.parts[this.parts.Count - 1];
				}
			}

			// Token: 0x0600ED47 RID: 60743 RVA: 0x00576F24 File Offset: 0x00575124
			public void AddPart(Thing hitThing, BodyPartRecord part)
			{
				if (this.hitThing != null && this.hitThing != hitThing)
				{
					Log.ErrorOnce("Single damage worker referring to multiple things; will cause issues with combat log", 30667935);
				}
				this.hitThing = hitThing;
				if (this.parts == null)
				{
					this.parts = new List<BodyPartRecord>();
				}
				this.parts.Add(part);
			}

			// Token: 0x0600ED48 RID: 60744 RVA: 0x00576F77 File Offset: 0x00575177
			public void AddHediff(Hediff hediff)
			{
				if (this.hediffs == null)
				{
					this.hediffs = new List<Hediff>();
				}
				this.hediffs.Add(hediff);
			}

			// Token: 0x0600ED49 RID: 60745 RVA: 0x00576F98 File Offset: 0x00575198
			public void AssociateWithLog(LogEntry_DamageResult log)
			{
				if (log == null)
				{
					return;
				}
				Thing thing = this.hitThing;
				Pawn hitPawn = thing as Pawn;
				if (hitPawn != null)
				{
					List<BodyPartRecord> list = null;
					List<bool> recipientPartsDestroyed = null;
					if (!this.parts.NullOrEmpty<BodyPartRecord>())
					{
						list = this.parts.Distinct<BodyPartRecord>().ToList<BodyPartRecord>();
						recipientPartsDestroyed = (from part in list
						select hitPawn.health.hediffSet.GetPartHealth(part) <= 0f).ToList<bool>();
					}
					log.FillTargets(list, recipientPartsDestroyed, this.deflected);
				}
				if (this.hediffs != null)
				{
					for (int i = 0; i < this.hediffs.Count; i++)
					{
						this.hediffs[i].combatLogEntry = new WeakReference<LogEntry>(log);
						this.hediffs[i].combatLogText = log.ToGameStringFromPOV(null, false);
					}
				}
			}

			// Token: 0x040097FB RID: 38907
			public bool wounded;

			// Token: 0x040097FC RID: 38908
			public bool headshot;

			// Token: 0x040097FD RID: 38909
			public bool deflected;

			// Token: 0x040097FE RID: 38910
			public bool stunned;

			// Token: 0x040097FF RID: 38911
			public bool deflectedByMetalArmor;

			// Token: 0x04009800 RID: 38912
			public bool diminished;

			// Token: 0x04009801 RID: 38913
			public bool diminishedByMetalArmor;

			// Token: 0x04009802 RID: 38914
			public Thing hitThing;

			// Token: 0x04009803 RID: 38915
			public List<BodyPartRecord> parts;

			// Token: 0x04009804 RID: 38916
			public List<Hediff> hediffs;

			// Token: 0x04009805 RID: 38917
			public float totalDamageDealt;
		}
	}
}
