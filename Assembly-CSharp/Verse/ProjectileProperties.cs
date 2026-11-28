using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000146 RID: 326
	public class ProjectileProperties
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00038699 File Offset: 0x00036899
		public float SpeedTilesPerTick
		{
			get
			{
				return this.speed / 100f;
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x000386A8 File Offset: 0x000368A8
		public int GetDamageAmount(Thing weapon, StringBuilder explanation = null)
		{
			float damageMultiplier = (weapon != null) ? weapon.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true, -1) : 1f;
			return this.GetDamageAmount(damageMultiplier, weapon, explanation);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000386D8 File Offset: 0x000368D8
		public int GetDamageAmount(ThingDef weapon, ThingDef weaponStuff, StringBuilder explanation = null)
		{
			float damageMultiplier = (weapon != null) ? weapon.GetStatValueAbstract(StatDefOf.RangedWeapon_DamageMultiplier, weaponStuff) : 1f;
			return this.GetDamageAmount(damageMultiplier, null, explanation);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00038708 File Offset: 0x00036908
		public int GetDamageAmount(float damageMultiplier, Thing weapon, StringBuilder explanation = null)
		{
			int num;
			if (this.damageAmountBase != -1)
			{
				num = this.damageAmountBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					Log.ErrorOnce("Failed to find sane damage amount", 91094882);
					return 1;
				}
				num = this.damageDef.defaultDamage;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToString());
			}
			num = Mathf.RoundToInt((float)num * damageMultiplier);
			if (!Mathf.Approximately(damageMultiplier, 1f))
			{
				if (explanation != null)
				{
					explanation.AppendLine();
				}
				if (explanation != null)
				{
					explanation.AppendLine(StatDefOf.RangedWeapon_DamageMultiplier.LabelCap + ": " + damageMultiplier.ToStringPercent());
				}
				if (weapon != null && explanation != null)
				{
					explanation.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_DamageMultiplier, weapon));
				}
			}
			if (explanation != null)
			{
				explanation.AppendLine();
			}
			if (explanation != null)
			{
				explanation.Append("StatsReport_FinalValue".Translate() + ": " + num.ToString());
			}
			return num;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00038820 File Offset: 0x00036A20
		public float GetArmorPenetration(Thing weapon = null, StringBuilder explanation = null)
		{
			if (this.damageDef.armorCategory == null)
			{
				return 0f;
			}
			float num;
			if (this.damageAmountBase != -1 || this.armorPenetrationBase >= 0f)
			{
				num = this.armorPenetrationBase;
			}
			else
			{
				if (this.damageDef == null)
				{
					return 0f;
				}
				num = this.damageDef.defaultArmorPenetration;
			}
			if (num < 0f)
			{
				num = (float)this.GetDamageAmount(null, null) * 0.015f;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToStringPercent());
			}
			if (weapon != null)
			{
				float statValue = weapon.GetStatValue(StatDefOf.RangedWeapon_ArmorPenetrationMultiplier, true, -1);
				num *= statValue;
				if (!Mathf.Approximately(statValue, 1f))
				{
					if (explanation != null)
					{
						explanation.AppendLine();
					}
					if (explanation != null)
					{
						explanation.AppendLine(StatDefOf.RangedWeapon_ArmorPenetrationMultiplier.LabelCap + ": " + statValue.ToStringPercent());
					}
					if (explanation != null)
					{
						explanation.Append(StatUtility.GetOffsetsAndFactorsFor(StatDefOf.RangedWeapon_ArmorPenetrationMultiplier, weapon));
					}
				}
			}
			if (explanation != null)
			{
				explanation.AppendLine();
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_FinalValue".Translate() + ": " + num.ToStringPercent());
			}
			return num;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00038969 File Offset: 0x00036B69
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			ProjectileProperties.<ConfigErrors>d__58 <ConfigErrors>d__ = new ProjectileProperties.<ConfigErrors>d__58(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parent = parent;
			return <ConfigErrors>d__;
		}

		// Token: 0x04000631 RID: 1585
		public float speed = 5f;

		// Token: 0x04000632 RID: 1586
		public bool flyOverhead;

		// Token: 0x04000633 RID: 1587
		public bool alwaysFreeIntercept;

		// Token: 0x04000634 RID: 1588
		public DamageDef damageDef;

		// Token: 0x04000635 RID: 1589
		private int damageAmountBase = -1;

		// Token: 0x04000636 RID: 1590
		private float armorPenetrationBase = -1f;

		// Token: 0x04000637 RID: 1591
		public float stoppingPower = 0.5f;

		// Token: 0x04000638 RID: 1592
		public List<ExtraDamage> extraDamages;

		// Token: 0x04000639 RID: 1593
		public float arcHeightFactor;

		// Token: 0x0400063A RID: 1594
		public float shadowSize;

		// Token: 0x0400063B RID: 1595
		public EffecterDef landedEffecter;

		// Token: 0x0400063C RID: 1596
		public float spinRate;

		// Token: 0x0400063D RID: 1597
		public bool useGraphicClass;

		// Token: 0x0400063E RID: 1598
		public SoundDef soundHitThickRoof;

		// Token: 0x0400063F RID: 1599
		public SoundDef soundExplode;

		// Token: 0x04000640 RID: 1600
		public SoundDef soundImpactAnticipate;

		// Token: 0x04000641 RID: 1601
		public SoundDef soundAmbient;

		// Token: 0x04000642 RID: 1602
		public SoundDef soundImpact;

		// Token: 0x04000643 RID: 1603
		public float explosionRadius;

		// Token: 0x04000644 RID: 1604
		public float explosionRadiusDisplayPadding;

		// Token: 0x04000645 RID: 1605
		public int explosionDelay;

		// Token: 0x04000646 RID: 1606
		public bool doExplosionVFX = true;

		// Token: 0x04000647 RID: 1607
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04000648 RID: 1608
		public float preExplosionSpawnChance = 1f;

		// Token: 0x04000649 RID: 1609
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x0400064A RID: 1610
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x0400064B RID: 1611
		public ThingDef postExplosionSpawnThingDefWater;

		// Token: 0x0400064C RID: 1612
		public float postExplosionSpawnChance = 1f;

		// Token: 0x0400064D RID: 1613
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x0400064E RID: 1614
		public GasType? postExplosionGasType;

		// Token: 0x0400064F RID: 1615
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04000650 RID: 1616
		public float explosionChanceToStartFire;

		// Token: 0x04000651 RID: 1617
		public bool explosionDamageFalloff;

		// Token: 0x04000652 RID: 1618
		public float screenShakeFactor = 1f;

		// Token: 0x04000653 RID: 1619
		public ThingDef preExplosionSpawnSingleThingDef;

		// Token: 0x04000654 RID: 1620
		public ThingDef postExplosionSpawnSingleThingDef;

		// Token: 0x04000655 RID: 1621
		public ThingDef filth;

		// Token: 0x04000656 RID: 1622
		public float filthChance = 1f;

		// Token: 0x04000657 RID: 1623
		public IntRange filthCount;

		// Token: 0x04000658 RID: 1624
		public int numExtraHitCells;

		// Token: 0x04000659 RID: 1625
		public bool explosionSpawnsSingleFilth;

		// Token: 0x0400065A RID: 1626
		public TerrainDef spawnTerrain;

		// Token: 0x0400065B RID: 1627
		public float terrainChance = 1f;

		// Token: 0x0400065C RID: 1628
		public bool terrainReplacesFloors;

		// Token: 0x0400065D RID: 1629
		public EffecterDef explosionEffect;

		// Token: 0x0400065E RID: 1630
		public int explosionEffectLifetimeTicks;

		// Token: 0x0400065F RID: 1631
		public ThingDef spawnsThingDef;

		// Token: 0x04000660 RID: 1632
		public bool tryAdjacentFreeSpaces;

		// Token: 0x04000661 RID: 1633
		public PawnKindDef spawnsPawnKind;

		// Token: 0x04000662 RID: 1634
		public ThingDef beamMoteDef;

		// Token: 0x04000663 RID: 1635
		public float beamStartOffset;

		// Token: 0x04000664 RID: 1636
		public bool ai_IsIncendiary;
	}
}
