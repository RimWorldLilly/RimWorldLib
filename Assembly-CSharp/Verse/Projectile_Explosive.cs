using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000594 RID: 1428
	public class Projectile_Explosive : Projectile
	{
		// Token: 0x06002A36 RID: 10806 RVA: 0x0010200B File Offset: 0x0010020B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToDetonation, "ticksToDetonation", 0, false);
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x00102025 File Offset: 0x00100225
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.ticksToDetonation > 0)
			{
				this.ticksToDetonation -= delta;
				if (this.ticksToDetonation <= 0)
				{
					this.Explode();
				}
			}
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x00102054 File Offset: 0x00100254
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			if (blockedByShield || this.def.projectile.explosionDelay == 0)
			{
				this.Explode();
				return;
			}
			this.landed = true;
			this.ticksToDetonation = this.def.projectile.explosionDelay;
			GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this, base.DamageDef, this.launcher.Faction, this.launcher);
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x001020B8 File Offset: 0x001002B8
		protected virtual void Explode()
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.def.projectile.explosionEffect != null)
			{
				Effecter effecter = this.def.projectile.explosionEffect.Spawn();
				if (this.def.projectile.explosionEffectLifetimeTicks != 0)
				{
					map.effecterMaintainer.AddEffecterToMaintain(effecter, base.Position.ToVector3().ToIntVec3(), this.def.projectile.explosionEffectLifetimeTicks);
				}
				else
				{
					effecter.Trigger(new TargetInfo(base.Position, map, false), new TargetInfo(base.Position, map, false), -1);
					effecter.Cleanup();
				}
			}
			IntVec3 position = base.Position;
			Map map2 = map;
			float explosionRadius = this.def.projectile.explosionRadius;
			DamageDef damageDef = base.DamageDef;
			Thing launcher = this.launcher;
			int damageAmount = this.DamageAmount;
			float armorPenetration = this.ArmorPenetration;
			SoundDef soundExplode = this.def.projectile.soundExplode;
			ThingDef equipmentDef = this.equipmentDef;
			ThingDef def = this.def;
			Thing thing = this.intendedTarget.Thing;
			ThingDef postExplosionSpawnThingDef = this.def.projectile.postExplosionSpawnThingDef ?? (this.def.projectile.explosionSpawnsSingleFilth ? null : this.def.projectile.filth);
			ThingDef postExplosionSpawnThingDefWater = this.def.projectile.postExplosionSpawnThingDefWater;
			float postExplosionSpawnChance = this.def.projectile.postExplosionSpawnChance;
			int postExplosionSpawnThingCount = this.def.projectile.postExplosionSpawnThingCount;
			GasType? postExplosionGasType = this.def.projectile.postExplosionGasType;
			ThingDef preExplosionSpawnThingDef = this.def.projectile.preExplosionSpawnThingDef;
			float preExplosionSpawnChance = this.def.projectile.preExplosionSpawnChance;
			int preExplosionSpawnThingCount = this.def.projectile.preExplosionSpawnThingCount;
			bool applyDamageToExplosionCellsNeighbors = this.def.projectile.applyDamageToExplosionCellsNeighbors;
			float explosionChanceToStartFire = this.def.projectile.explosionChanceToStartFire;
			bool explosionDamageFalloff = this.def.projectile.explosionDamageFalloff;
			float? direction = new float?(this.origin.AngleToFlat(this.destination));
			float expolosionPropagationSpeed = base.DamageDef.expolosionPropagationSpeed;
			float screenShakeFactor = this.def.projectile.screenShakeFactor;
			bool doExplosionVFX = this.def.projectile.doExplosionVFX;
			ThingDef preExplosionSpawnSingleThingDef = this.def.projectile.preExplosionSpawnSingleThingDef;
			ThingDef postExplosionSpawnSingleThingDef = this.def.projectile.postExplosionSpawnSingleThingDef;
			GenExplosion.DoExplosion(position, map2, explosionRadius, damageDef, launcher, damageAmount, armorPenetration, soundExplode, equipmentDef, def, thing, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, postExplosionGasType, null, 255, applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, preExplosionSpawnChance, preExplosionSpawnThingCount, explosionChanceToStartFire, explosionDamageFalloff, direction, null, null, doExplosionVFX, expolosionPropagationSpeed, 0f, true, postExplosionSpawnThingDefWater, screenShakeFactor, null, null, postExplosionSpawnSingleThingDef, preExplosionSpawnSingleThingDef);
			if (this.def.projectile.explosionSpawnsSingleFilth && this.def.projectile.filth != null && this.def.projectile.filthCount.TrueMax > 0 && Rand.Chance(this.def.projectile.filthChance) && !base.Position.Filled(map))
			{
				FilthMaker.TryMakeFilth(base.Position, map, this.def.projectile.filth, this.def.projectile.filthCount.RandomInRange, FilthSourceFlags.None, true);
			}
		}

		// Token: 0x04001D37 RID: 7479
		private int ticksToDetonation;
	}
}
