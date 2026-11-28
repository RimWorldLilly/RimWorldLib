using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001920 RID: 6432
	public abstract class CompObelisk_ExplodingSpawner : CompObelisk
	{
		// Token: 0x17001BDA RID: 7130
		// (get) Token: 0x0600A15B RID: 41307 RVA: 0x0039AD4B File Offset: 0x00398F4B
		protected virtual IntRange ExplodeDelayTicks
		{
			get
			{
				return new IntRange(120, 180);
			}
		}

		// Token: 0x17001BDB RID: 7131
		// (get) Token: 0x0600A15C RID: 41308 RVA: 0x0039AD59 File Offset: 0x00398F59
		protected virtual IntRange SpawnIntervalTicks
		{
			get
			{
				return new IntRange(6, 60);
			}
		}

		// Token: 0x17001BDC RID: 7132
		// (get) Token: 0x0600A15D RID: 41309 RVA: 0x0039AD64 File Offset: 0x00398F64
		protected Lord Lord
		{
			get
			{
				return this.lord ?? LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(Faction.OfEntities, false, false, false, false, false, false, false), this.parent.Map, null);
			}
		}

		// Token: 0x0600A15E RID: 41310 RVA: 0x0039ADA4 File Offset: 0x00398FA4
		protected void Explode()
		{
			Map map = this.parent.Map;
			this.parent.Destroy(DestroyMode.KillFinalize);
			IntVec3 position = this.parent.Position;
			Map map2 = map;
			float radius = 4.9f;
			DamageDef vaporize = DamageDefOf.Vaporize;
			Thing parent = this.parent;
			int damAmount = -1;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef weapon = null;
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef postExplosionSpawnThingDef = null;
			float postExplosionSpawnChance = 0f;
			int postExplosionSpawnThingCount = 1;
			List<Thing> ignoredThings = this.thingsIgnoredByExplosion;
			GenExplosion.DoExplosion(position, map2, radius, vaporize, parent, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, 0f, false, null, ignoredThings, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x0600A15F RID: 41311 RVA: 0x0039AE51 File Offset: 0x00399051
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			base.Notify_KilledLeavingsLeft(leavings);
			if (this.thingsIgnoredByExplosion == null)
			{
				this.thingsIgnoredByExplosion = new List<Thing>();
			}
			else
			{
				this.thingsIgnoredByExplosion.Clear();
			}
			this.thingsIgnoredByExplosion.AddRange(leavings);
		}

		// Token: 0x0600A160 RID: 41312 RVA: 0x0039AE88 File Offset: 0x00399088
		public override void CompTick()
		{
			base.CompTick();
			if (this.activated && this.explodeTick > 0)
			{
				Effecter effecter = this.effecterExplodeWarmup;
				if (effecter != null)
				{
					effecter.EffectTick(this.parent, new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				if (Find.TickManager.TicksGame > this.explodeTick)
				{
					Effecter effecter2 = this.effecterExplodeWarmup;
					if (effecter2 != null)
					{
						effecter2.Cleanup();
					}
					this.effecterExplodeWarmup = null;
					this.Explode();
				}
			}
		}

		// Token: 0x0600A161 RID: 41313 RVA: 0x0039AF14 File Offset: 0x00399114
		protected void PrepareExplosion()
		{
			this.effecterExplodeWarmup = EffecterDefOf.ObeliskExplosionWarmup.Spawn();
			this.effecterExplodeWarmup.Trigger(this.parent, new TargetInfo(this.parent.Position, this.parent.Map, false), -1);
			this.explodeTick = Find.TickManager.TicksGame + this.ExplodeDelayTicks.RandomInRange;
		}

		// Token: 0x0600A162 RID: 41314 RVA: 0x0039AF84 File Offset: 0x00399184
		public override void OnActivityActivated()
		{
			base.OnActivityActivated();
			if (this.parent.Map != null)
			{
				this.pointsRemaining = (this.totalCombatPoints = StorytellerUtility.DefaultThreatPointsNow(this.parent.Map));
			}
		}

		// Token: 0x0600A163 RID: 41315 RVA: 0x0039AFC4 File Offset: 0x003991C4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.pointsRemaining, "pointsRemaining", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCombatPoints, "totalCombatPoints", 0f, false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Values.Look<int>(ref this.nextSpawnTick, "nextSpawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.explodeTick, "explodeTick", 0, false);
		}

		// Token: 0x04005F55 RID: 24405
		protected int explodeTick = -99999;

		// Token: 0x04005F56 RID: 24406
		protected float pointsRemaining;

		// Token: 0x04005F57 RID: 24407
		protected float totalCombatPoints;

		// Token: 0x04005F58 RID: 24408
		private Lord lord;

		// Token: 0x04005F59 RID: 24409
		protected int nextSpawnTick = -99999;

		// Token: 0x04005F5A RID: 24410
		private Effecter effecterExplodeWarmup;

		// Token: 0x04005F5B RID: 24411
		private List<Thing> thingsIgnoredByExplosion;
	}
}
