using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200073B RID: 1851
	public class Verb_LaunchProjectile : Verb
	{
		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06003AB4 RID: 15028 RVA: 0x001762A3 File Offset: 0x001744A3
		public override float EffectiveRange
		{
			get
			{
				float effectiveRange = base.EffectiveRange;
				ThingWithComps equipmentSource = base.EquipmentSource;
				return effectiveRange * ((equipmentSource != null) ? equipmentSource.GetStatValue(StatDefOf.RangedWeapon_RangeMultiplier, true, -1) : 1f);
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06003AB5 RID: 15029 RVA: 0x001762C9 File Offset: 0x001744C9
		public override float WarmupTime
		{
			get
			{
				float warmupTime = base.WarmupTime;
				ThingWithComps equipmentSource = base.EquipmentSource;
				return warmupTime * ((equipmentSource != null) ? equipmentSource.GetStatValue(StatDefOf.RangedWeapon_WarmupMultiplier, true, -1) : 1f);
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06003AB6 RID: 15030 RVA: 0x001762F0 File Offset: 0x001744F0
		public virtual ThingDef Projectile
		{
			get
			{
				ThingWithComps equipmentSource = base.EquipmentSource;
				CompChangeableProjectile compChangeableProjectile = (equipmentSource != null) ? equipmentSource.GetComp<CompChangeableProjectile>() : null;
				if (compChangeableProjectile != null && compChangeableProjectile.Loaded)
				{
					return compChangeableProjectile.Projectile;
				}
				return this.verbProps.defaultProjectile;
			}
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x00176330 File Offset: 0x00174530
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			BattleLog battleLog = Find.BattleLog;
			Thing caster = this.caster;
			Thing target = this.currentTarget.HasThing ? this.currentTarget.Thing : null;
			ThingWithComps equipmentSource = base.EquipmentSource;
			battleLog.Add(new BattleLogEntry_RangedFire(caster, target, (equipmentSource != null) ? equipmentSource.def : null, this.Projectile, this.ShotsPerBurst > 1));
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x00176394 File Offset: 0x00174594
		protected IntVec3 GetForcedMissTarget(float forcedMissRadius)
		{
			if (this.verbProps.forcedMissEvenDispersal)
			{
				if (this.forcedMissTargetEvenDispersalCache.Count <= 0)
				{
					this.forcedMissTargetEvenDispersalCache.AddRange(Verb_LaunchProjectile.GenerateEvenDispersalForcedMissTargets(this.currentTarget.Cell, forcedMissRadius, this.burstShotsLeft));
					this.forcedMissTargetEvenDispersalCache.SortByDescending((IntVec3 p) => p.DistanceToSquared(this.Caster.Position));
				}
				if (this.forcedMissTargetEvenDispersalCache.Count > 0)
				{
					return this.forcedMissTargetEvenDispersalCache.Pop<IntVec3>();
				}
			}
			int maxExclusive = GenRadial.NumCellsInRadius(forcedMissRadius);
			int num = Rand.Range(0, maxExclusive);
			return this.currentTarget.Cell + GenRadial.RadialPattern[num];
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x00176439 File Offset: 0x00174639
		private static IEnumerable<IntVec3> GenerateEvenDispersalForcedMissTargets(IntVec3 root, float radius, int count)
		{
			Verb_LaunchProjectile.<GenerateEvenDispersalForcedMissTargets>d__9 <GenerateEvenDispersalForcedMissTargets>d__ = new Verb_LaunchProjectile.<GenerateEvenDispersalForcedMissTargets>d__9(-2);
			<GenerateEvenDispersalForcedMissTargets>d__.<>3__root = root;
			<GenerateEvenDispersalForcedMissTargets>d__.<>3__radius = radius;
			<GenerateEvenDispersalForcedMissTargets>d__.<>3__count = count;
			return <GenerateEvenDispersalForcedMissTargets>d__;
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00176458 File Offset: 0x00174658
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			ThingDef projectile = this.Projectile;
			if (projectile == null)
			{
				return false;
			}
			ShootLine shootLine;
			bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine, false);
			if (this.verbProps.stopBurstWithoutLos && !flag)
			{
				return false;
			}
			if (base.EquipmentSource != null)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompApparelVerbOwner_Charged comp2 = base.EquipmentSource.GetComp<CompApparelVerbOwner_Charged>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			this.lastShotTick = Find.TickManager.TicksGame;
			Thing thing = this.caster;
			Thing thing2 = base.EquipmentSource;
			CompMannable compMannable = this.caster.TryGetComp<CompMannable>();
			if (((compMannable != null) ? compMannable.ManningPawn : null) != null)
			{
				thing = compMannable.ManningPawn;
				thing2 = this.caster;
			}
			Vector3 drawPos = this.caster.DrawPos;
			Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, shootLine.Source, this.caster.Map, WipeMode.Vanish);
			CompUniqueWeapon compUniqueWeapon;
			if (thing2.TryGetComp(out compUniqueWeapon))
			{
				foreach (WeaponTraitDef weaponTraitDef in compUniqueWeapon.TraitsListForReading)
				{
					if (weaponTraitDef.damageDefOverride != null)
					{
						projectile2.damageDefOverride = weaponTraitDef.damageDefOverride;
					}
					if (!weaponTraitDef.extraDamages.NullOrEmpty<ExtraDamage>())
					{
						Projectile projectile3 = projectile2;
						if (projectile3.extraDamages == null)
						{
							projectile3.extraDamages = new List<ExtraDamage>();
						}
						projectile2.extraDamages.AddRange(weaponTraitDef.extraDamages);
					}
				}
			}
			if (this.verbProps.ForcedMissRadius > 0.5f)
			{
				float num = this.verbProps.ForcedMissRadius;
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					num *= this.verbProps.GetForceMissFactorFor(thing2, pawn);
				}
				float num2 = VerbUtility.CalculateAdjustedForcedMiss(num, this.currentTarget.Cell - this.caster.Position);
				if (num2 > 0.5f)
				{
					IntVec3 forcedMissTarget = this.GetForcedMissTarget(num2);
					if (forcedMissTarget != this.currentTarget.Cell)
					{
						this.ThrowDebugText("ToRadius");
						this.ThrowDebugText("Rad\nDest", forcedMissTarget);
						ProjectileHitFlags projectileHitFlags = ProjectileHitFlags.NonTargetWorld;
						if (Rand.Chance(0.5f))
						{
							projectileHitFlags = ProjectileHitFlags.All;
						}
						if (!this.canHitNonTargetPawnsNow)
						{
							projectileHitFlags &= ~ProjectileHitFlags.NonTargetPawns;
						}
						projectile2.Launch(thing, drawPos, forcedMissTarget, this.currentTarget, projectileHitFlags, this.preventFriendlyFire, thing2, null);
						return true;
					}
				}
			}
			ShotReport shotReport = ShotReport.HitReportFor(this.caster, this, this.currentTarget);
			Thing randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
			ThingDef targetCoverDef = (randomCoverToMissInto != null) ? randomCoverToMissInto.def : null;
			if (this.verbProps.canGoWild && !Rand.Chance(shotReport.AimOnTargetChance_IgnoringPosture))
			{
				bool flag2;
				if (projectile2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					ThingDef def = projectile2.def;
					flag2 = (((def != null) ? def.projectile : null) != null);
				}
				bool flyOverhead = flag2 && projectile2.def.projectile.flyOverhead;
				shootLine.ChangeDestToMissWild(shotReport.AimOnTargetChance_StandardTarget, flyOverhead, this.caster.Map);
				this.ThrowDebugText("ToWild" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
				this.ThrowDebugText("Wild\nDest", shootLine.Dest);
				ProjectileHitFlags projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
				if (Rand.Chance(0.5f) && this.canHitNonTargetPawnsNow)
				{
					projectileHitFlags2 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(thing, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags2, this.preventFriendlyFire, thing2, targetCoverDef);
				return true;
			}
			if (this.currentTarget.Thing != null && this.currentTarget.Thing.def.CanBenefitFromCover && !Rand.Chance(shotReport.PassCoverChance))
			{
				this.ThrowDebugText("ToCover" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
				this.ThrowDebugText("Cover\nDest", randomCoverToMissInto.Position);
				ProjectileHitFlags projectileHitFlags3 = ProjectileHitFlags.NonTargetWorld;
				if (this.canHitNonTargetPawnsNow)
				{
					projectileHitFlags3 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(thing, drawPos, randomCoverToMissInto, this.currentTarget, projectileHitFlags3, this.preventFriendlyFire, thing2, targetCoverDef);
				return true;
			}
			ProjectileHitFlags projectileHitFlags4 = ProjectileHitFlags.IntendedTarget;
			if (this.canHitNonTargetPawnsNow)
			{
				projectileHitFlags4 |= ProjectileHitFlags.NonTargetPawns;
			}
			if (!this.currentTarget.HasThing || this.currentTarget.Thing.def.Fillage == FillCategory.Full)
			{
				projectileHitFlags4 |= ProjectileHitFlags.NonTargetWorld;
			}
			this.ThrowDebugText("ToHit" + (this.canHitNonTargetPawnsNow ? "\nchntp" : ""));
			if (this.currentTarget.Thing != null)
			{
				projectile2.Launch(thing, drawPos, this.currentTarget, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, thing2, targetCoverDef);
				this.ThrowDebugText("Hit\nDest", this.currentTarget.Cell);
			}
			else
			{
				projectile2.Launch(thing, drawPos, shootLine.Dest, this.currentTarget, projectileHitFlags4, this.preventFriendlyFire, thing2, targetCoverDef);
				this.ThrowDebugText("Hit\nDest", shootLine.Dest);
			}
			return true;
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x001769A0 File Offset: 0x00174BA0
		private void ThrowDebugText(string text)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(this.caster.DrawPos, this.caster.Map, text, -1f);
			}
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x001769CA File Offset: 0x00174BCA
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), this.caster.Map, text, -1f);
			}
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x001769F0 File Offset: 0x00174BF0
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = true;
			ThingDef projectile = this.Projectile;
			if (projectile == null)
			{
				return 0f;
			}
			float num = projectile.projectile.explosionRadius + projectile.projectile.explosionRadiusDisplayPadding;
			float forcedMissRadius = this.verbProps.ForcedMissRadius;
			if (forcedMissRadius > 0f && base.BurstShotCount > 1)
			{
				num += forcedMissRadius;
			}
			return num;
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x00176A4C File Offset: 0x00174C4C
		public override bool Available()
		{
			if (!base.Available())
			{
				return false;
			}
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Faction != Faction.OfPlayer && !this.verbProps.ai_ProjectileLaunchingIgnoresMeleeThreats && casterPawn.mindState.MeleeThreatStillThreat && casterPawn.mindState.meleeThreat.Position.AdjacentTo8WayOrInside(casterPawn.Position))
				{
					return false;
				}
			}
			return this.Projectile != null;
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x00176AC1 File Offset: 0x00174CC1
		public override void Reset()
		{
			base.Reset();
			this.forcedMissTargetEvenDispersalCache.Clear();
		}

		// Token: 0x040026A2 RID: 9890
		private List<IntVec3> forcedMissTargetEvenDispersalCache = new List<IntVec3>();
	}
}
