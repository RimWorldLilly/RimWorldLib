using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AE7 RID: 6887
	[StaticConstructorOnStartup]
	public class Bombardment : OrbitalStrike
	{
		// Token: 0x0600AFE4 RID: 45028 RVA: 0x003DFD69 File Offset: 0x003DDF69
		public override void SpawnSetup(Map map, bool respawningAfterReload)
		{
			base.SpawnSetup(map, respawningAfterReload);
			if (!respawningAfterReload)
			{
				this.GetNextExplosionCell();
			}
		}

		// Token: 0x0600AFE5 RID: 45029 RVA: 0x003DFD7C File Offset: 0x003DDF7C
		public override void StartStrike()
		{
			this.duration = this.bombIntervalTicks * this.explosionCount;
			base.StartStrike();
		}

		// Token: 0x0600AFE6 RID: 45030 RVA: 0x003DFD98 File Offset: 0x003DDF98
		protected override void Tick()
		{
			if (base.Destroyed)
			{
				return;
			}
			if (this.warmupTicks > 0)
			{
				this.warmupTicks--;
				if (this.warmupTicks <= 0)
				{
					this.StartStrike();
				}
			}
			else
			{
				base.Tick();
				if (base.TicksLeft > 0 && this.IsHashIntervalTick(20))
				{
					this.StartRandomFire();
				}
			}
			this.EffectTick();
		}

		// Token: 0x0600AFE7 RID: 45031 RVA: 0x003DFDFC File Offset: 0x003DDFFC
		private void EffectTick()
		{
			if (!this.nextExplosionCell.IsValid)
			{
				this.ticksToNextEffect = this.warmupTicks - this.bombIntervalTicks;
				this.GetNextExplosionCell();
			}
			this.ticksToNextEffect--;
			if (this.ticksToNextEffect <= 0 && base.TicksLeft >= this.bombIntervalTicks)
			{
				SoundDefOf.Bombardment_PreImpact.PlayOneShot(new TargetInfo(this.nextExplosionCell, base.Map, false));
				this.projectiles.Add(new Bombardment.BombardmentProjectile(60, this.nextExplosionCell));
				this.ticksToNextEffect = this.bombIntervalTicks;
				this.GetNextExplosionCell();
			}
			for (int i = this.projectiles.Count - 1; i >= 0; i--)
			{
				this.projectiles[i].Tick();
				if (this.projectiles[i].LifeTime <= 0)
				{
					this.TryDoExplosion(this.projectiles[i]);
					this.projectiles.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600AFE8 RID: 45032 RVA: 0x003DFEFC File Offset: 0x003DE0FC
		private void TryDoExplosion(Bombardment.BombardmentProjectile proj)
		{
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].TryGetComp<CompProjectileInterceptor>().CheckBombardmentIntercept(this, proj))
				{
					return;
				}
			}
			IntVec3 targetCell = proj.targetCell;
			Map map = base.Map;
			float randomInRange = this.explosionRadiusRange.RandomInRange;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing instigator = this.instigator;
			int damAmount = -1;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef def = this.def;
			GenExplosion.DoExplosion(targetCell, map, randomInRange, bomb, instigator, damAmount, armorPenetration, explosionSound, this.weaponDef, def, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x0600AFE9 RID: 45033 RVA: 0x003DFFD8 File Offset: 0x003DE1D8
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.projectiles.NullOrEmpty<Bombardment.BombardmentProjectile>())
			{
				return;
			}
			for (int i = 0; i < this.projectiles.Count; i++)
			{
				this.projectiles[i].Draw(Bombardment.ProjectileMaterial);
			}
		}

		// Token: 0x0600AFEA RID: 45034 RVA: 0x003E0028 File Offset: 0x003DE228
		private void StartRandomFire()
		{
			IntVec3 intVec = (from x in GenRadial.RadialCellsAround(base.Position, (float)this.randomFireRadius, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position)));
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].TryGetComp<CompProjectileInterceptor>().BombardmentCanStartFireAt(this, intVec))
				{
					return;
				}
			}
			RoofDef roof = intVec.GetRoof(base.Map);
			if (roof != null && roof.isThickRoof)
			{
				return;
			}
			FireUtility.TryStartFireIn(intVec, base.Map, Rand.Range(0.1f, 0.925f), this.instigator, null);
		}

		// Token: 0x0600AFEB RID: 45035 RVA: 0x003E00E1 File Offset: 0x003DE2E1
		private void GetNextExplosionCell()
		{
			this.nextExplosionCell = (from x in GenRadial.RadialCellsAround(base.Position, this.impactAreaRadius, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position) / this.impactAreaRadius));
		}

		// Token: 0x0600AFEC RID: 45036 RVA: 0x003E0120 File Offset: 0x003DE320
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.impactAreaRadius, "impactAreaRadius", 15f, false);
			Scribe_Values.Look<FloatRange>(ref this.explosionRadiusRange, "explosionRadiusRange", new FloatRange(6f, 8f), false);
			Scribe_Values.Look<int>(ref this.randomFireRadius, "randomFireRadius", 25, false);
			Scribe_Values.Look<int>(ref this.bombIntervalTicks, "bombIntervalTicks", 18, false);
			Scribe_Values.Look<int>(ref this.warmupTicks, "warmupTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextEffect, "ticksToNextEffect", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.nextExplosionCell, "nextExplosionCell", default(IntVec3), false);
			Scribe_Collections.Look<Bombardment.BombardmentProjectile>(ref this.projectiles, "projectiles", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.nextExplosionCell.IsValid)
				{
					this.GetNextExplosionCell();
				}
				if (this.projectiles == null)
				{
					this.projectiles = new List<Bombardment.BombardmentProjectile>();
				}
			}
		}

		// Token: 0x04006585 RID: 25989
		public float impactAreaRadius = 15f;

		// Token: 0x04006586 RID: 25990
		public FloatRange explosionRadiusRange = new FloatRange(6f, 8f);

		// Token: 0x04006587 RID: 25991
		public int randomFireRadius = 25;

		// Token: 0x04006588 RID: 25992
		public int bombIntervalTicks = 18;

		// Token: 0x04006589 RID: 25993
		public int warmupTicks = 60;

		// Token: 0x0400658A RID: 25994
		public int explosionCount = 30;

		// Token: 0x0400658B RID: 25995
		private int ticksToNextEffect;

		// Token: 0x0400658C RID: 25996
		private IntVec3 nextExplosionCell = IntVec3.Invalid;

		// Token: 0x0400658D RID: 25997
		private List<Bombardment.BombardmentProjectile> projectiles = new List<Bombardment.BombardmentProjectile>();

		// Token: 0x0400658E RID: 25998
		public const int EffectiveAreaRadius = 23;

		// Token: 0x0400658F RID: 25999
		private const int StartRandomFireEveryTicks = 20;

		// Token: 0x04006590 RID: 26000
		private const int EffectDuration = 60;

		// Token: 0x04006591 RID: 26001
		private static readonly Material ProjectileMaterial = MaterialPool.MatFrom("Things/Projectile/Bullet_Big", ShaderDatabase.Transparent, Color.white);

		// Token: 0x04006592 RID: 26002
		public static readonly SimpleCurve DistanceChanceFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.1f),
				true
			}
		};

		// Token: 0x02003803 RID: 14339
		public class BombardmentProjectile : IExposable
		{
			// Token: 0x1700352C RID: 13612
			// (get) Token: 0x06014771 RID: 83825 RVA: 0x0069433D File Offset: 0x0069253D
			public int LifeTime
			{
				get
				{
					return this.lifeTime;
				}
			}

			// Token: 0x06014772 RID: 83826 RVA: 0x00694345 File Offset: 0x00692545
			public BombardmentProjectile()
			{
			}

			// Token: 0x06014773 RID: 83827 RVA: 0x0069434D File Offset: 0x0069254D
			public BombardmentProjectile(int lifeTime, IntVec3 targetCell)
			{
				this.lifeTime = lifeTime;
				this.maxLifeTime = lifeTime;
				this.targetCell = targetCell;
			}

			// Token: 0x06014774 RID: 83828 RVA: 0x0069436A File Offset: 0x0069256A
			public void Tick()
			{
				this.lifeTime--;
			}

			// Token: 0x06014775 RID: 83829 RVA: 0x0069437C File Offset: 0x0069257C
			public void Draw(Material material)
			{
				if (this.lifeTime > 0)
				{
					Vector3 pos = this.targetCell.ToVector3() + Vector3.forward * Mathf.Lerp(60f, 0f, 1f - (float)this.lifeTime / (float)this.maxLifeTime);
					pos.z += 1.25f;
					pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
					Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0f, 180f, 0f), new Vector3(2.5f, 1f, 2.5f));
					Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
				}
			}

			// Token: 0x06014776 RID: 83830 RVA: 0x00694430 File Offset: 0x00692630
			public void ExposeData()
			{
				Scribe_Values.Look<int>(ref this.lifeTime, "lifeTime", 0, false);
				Scribe_Values.Look<int>(ref this.maxLifeTime, "maxLifeTime", 0, false);
				Scribe_Values.Look<IntVec3>(ref this.targetCell, "targetCell", default(IntVec3), false);
			}

			// Token: 0x0400DFB3 RID: 57267
			private int lifeTime;

			// Token: 0x0400DFB4 RID: 57268
			private int maxLifeTime;

			// Token: 0x0400DFB5 RID: 57269
			public IntVec3 targetCell;

			// Token: 0x0400DFB6 RID: 57270
			private const float StartZ = 60f;

			// Token: 0x0400DFB7 RID: 57271
			private const float Scale = 2.5f;

			// Token: 0x0400DFB8 RID: 57272
			private const float Angle = 180f;
		}
	}
}
