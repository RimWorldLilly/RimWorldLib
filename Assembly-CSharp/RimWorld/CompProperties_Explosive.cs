using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED5 RID: 3797
	public class CompProperties_Explosive : CompProperties
	{
		// Token: 0x06006392 RID: 25490 RVA: 0x0023BC60 File Offset: 0x00239E60
		public CompProperties_Explosive()
		{
			this.compClass = typeof(CompExplosive);
		}

		// Token: 0x06006393 RID: 25491 RVA: 0x0023BD04 File Offset: 0x00239F04
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosiveDamageType == null)
			{
				this.explosiveDamageType = DamageDefOf.Bomb;
			}
		}

		// Token: 0x06006394 RID: 25492 RVA: 0x0023BD20 File Offset: 0x00239F20
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Explosive.<ConfigErrors>d__41 <ConfigErrors>d__ = new CompProperties_Explosive.<ConfigErrors>d__41(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x040037B7 RID: 14263
		public float explosiveRadius = 1.9f;

		// Token: 0x040037B8 RID: 14264
		public DamageDef explosiveDamageType;

		// Token: 0x040037B9 RID: 14265
		public int damageAmountBase = -1;

		// Token: 0x040037BA RID: 14266
		public float armorPenetrationBase = -1f;

		// Token: 0x040037BB RID: 14267
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x040037BC RID: 14268
		public float postExplosionSpawnChance;

		// Token: 0x040037BD RID: 14269
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x040037BE RID: 14270
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x040037BF RID: 14271
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x040037C0 RID: 14272
		public float preExplosionSpawnChance;

		// Token: 0x040037C1 RID: 14273
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x040037C2 RID: 14274
		public float chanceToStartFire;

		// Token: 0x040037C3 RID: 14275
		public bool damageFalloff;

		// Token: 0x040037C4 RID: 14276
		public bool explodeOnKilled;

		// Token: 0x040037C5 RID: 14277
		public bool explodeOnDestroyed;

		// Token: 0x040037C6 RID: 14278
		public GasType? postExplosionGasType;

		// Token: 0x040037C7 RID: 14279
		public float? postExplosionGasRadiusOverride;

		// Token: 0x040037C8 RID: 14280
		public int postExplosionGasAmount = 255;

		// Token: 0x040037C9 RID: 14281
		public bool doVisualEffects = true;

		// Token: 0x040037CA RID: 14282
		public bool doSoundEffects = true;

		// Token: 0x040037CB RID: 14283
		public float propagationSpeed = 1f;

		// Token: 0x040037CC RID: 14284
		public ThingDef postExplosionSpawnSingleThingDef;

		// Token: 0x040037CD RID: 14285
		public ThingDef preExplosionSpawnSingleThingDef;

		// Token: 0x040037CE RID: 14286
		public float explosiveExpandPerStackcount;

		// Token: 0x040037CF RID: 14287
		public float explosiveExpandPerFuel;

		// Token: 0x040037D0 RID: 14288
		public EffecterDef explosionEffect;

		// Token: 0x040037D1 RID: 14289
		public SoundDef explosionSound;

		// Token: 0x040037D2 RID: 14290
		public List<DamageDef> startWickOnDamageTaken;

		// Token: 0x040037D3 RID: 14291
		public float startWickHitPointsPercent = 0.2f;

		// Token: 0x040037D4 RID: 14292
		public IntRange wickTicks = new IntRange(140, 150);

		// Token: 0x040037D5 RID: 14293
		public float wickScale = 1f;

		// Token: 0x040037D6 RID: 14294
		public List<DamageDef> startWickOnInternalDamageTaken;

		// Token: 0x040037D7 RID: 14295
		public bool drawWick = true;

		// Token: 0x040037D8 RID: 14296
		public float chanceNeverExplodeFromDamage;

		// Token: 0x040037D9 RID: 14297
		public float destroyThingOnExplosionSize;

		// Token: 0x040037DA RID: 14298
		public DamageDef requiredDamageTypeToExplode;

		// Token: 0x040037DB RID: 14299
		public IntRange? countdownTicks;

		// Token: 0x040037DC RID: 14300
		public string extraInspectStringKey;

		// Token: 0x040037DD RID: 14301
		public List<WickMessage> wickMessages;
	}
}
