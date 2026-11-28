using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003F1 RID: 1009
	public class DamageWorker_Vaporize : DamageWorker_AddInjury
	{
		// Token: 0x06001D81 RID: 7553 RVA: 0x000B4BEC File Offset: 0x000B2DEC
		public override void ExplosionAffectCell(Explosion explosion, IntVec3 c, List<Thing> damagedThings, List<Thing> ignoredThings, bool canThrowMotes)
		{
			bool flag = c.DistanceTo(explosion.Position) <= 2.9f;
			Thing firstThing = c.GetFirstThing(explosion.Map, ThingDefOf.Filth_FireFoam);
			if (firstThing != null)
			{
				firstThing.Destroy(DestroyMode.Vanish);
			}
			base.ExplosionAffectCell(explosion, c, damagedThings, ignoredThings, canThrowMotes && flag);
			FireUtility.TryStartFireIn(c, explosion.Map, DamageWorker_Vaporize.FireSizeRange.RandomInRange, explosion.instigator, null);
			if (flag)
			{
				FleckMaker.ThrowSmoke(c.ToVector3Shifted(), explosion.Map, 2f);
			}
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000B4C77 File Offset: 0x000B2E77
		protected override void ExplosionDamageThing(Explosion explosion, Thing t, List<Thing> damagedThings, List<Thing> ignoredThings, IntVec3 cell)
		{
			if (cell.DistanceTo(explosion.Position) <= 2.9f)
			{
				base.ExplosionDamageThing(explosion, t, damagedThings, ignoredThings, cell);
			}
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x000B4C9A File Offset: 0x000B2E9A
		public override void ExplosionStart(Explosion explosion, List<IntVec3> cellsToAffect)
		{
			base.ExplosionStart(explosion, cellsToAffect);
			Effecter effecter = EffecterDefOf.Vaporize_Heatwave.Spawn();
			effecter.Trigger(new TargetInfo(explosion.Position, explosion.Map, false), TargetInfo.Invalid, -1);
			effecter.Cleanup();
		}

		// Token: 0x0400177B RID: 6011
		private const float VaporizeRadius = 2.9f;

		// Token: 0x0400177C RID: 6012
		private static readonly FloatRange FireSizeRange = new FloatRange(0.4f, 0.8f);
	}
}
