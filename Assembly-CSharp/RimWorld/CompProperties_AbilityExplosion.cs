using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F7 RID: 5111
	public class CompProperties_AbilityExplosion : CompProperties_AbilityEffect
	{
		// Token: 0x06007EC8 RID: 32456 RVA: 0x002D4F20 File Offset: 0x002D3120
		public CompProperties_AbilityExplosion()
		{
			this.compClass = typeof(CompAbilityEffect_Explosion);
		}

		// Token: 0x04004F7F RID: 20351
		public DamageDef damageDef;

		// Token: 0x04004F80 RID: 20352
		public int damageAmount = -1;

		// Token: 0x04004F81 RID: 20353
		public float armorPenetration = -1f;

		// Token: 0x04004F82 RID: 20354
		public SoundDef soundExplode;

		// Token: 0x04004F83 RID: 20355
		public float explosionRadius;

		// Token: 0x04004F84 RID: 20356
		public bool doExplosionVFX = true;

		// Token: 0x04004F85 RID: 20357
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04004F86 RID: 20358
		public float preExplosionSpawnChance = 1f;

		// Token: 0x04004F87 RID: 20359
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04004F88 RID: 20360
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04004F89 RID: 20361
		public ThingDef postExplosionSpawnThingDefWater;

		// Token: 0x04004F8A RID: 20362
		public float postExplosionSpawnChance = 1f;

		// Token: 0x04004F8B RID: 20363
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04004F8C RID: 20364
		public GasType? postExplosionGasType;

		// Token: 0x04004F8D RID: 20365
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04004F8E RID: 20366
		public float explosionChanceToStartFire;

		// Token: 0x04004F8F RID: 20367
		public bool explosionDamageFalloff;

		// Token: 0x04004F90 RID: 20368
		public float screenShakeFactor = 1f;

		// Token: 0x04004F91 RID: 20369
		public ThingDef preExplosionSpawnSingleThingDef;

		// Token: 0x04004F92 RID: 20370
		public ThingDef postExplosionSpawnSingleThingDef;
	}
}
