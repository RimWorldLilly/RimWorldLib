using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000160 RID: 352
	public class VerbProperties
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0003A55A File Offset: 0x0003875A
		public bool CausesTimeSlowdown
		{
			get
			{
				return this.ai_IsWeapon && this.forceNormalTimeSpeed;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0003A56C File Offset: 0x0003876C
		public bool LaunchesProjectile
		{
			get
			{
				return typeof(Verb_LaunchProjectile).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0003A584 File Offset: 0x00038784
		public bool Ranged
		{
			get
			{
				return this.LaunchesProjectile || typeof(Verb_ShootBeam).IsAssignableFrom(this.verbClass) || typeof(Verb_SpewFire).IsAssignableFrom(this.verbClass) || typeof(Verb_Spray).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0003A5E0 File Offset: 0x000387E0
		public string AccuracySummaryString
		{
			get
			{
				return string.Concat(new string[]
				{
					this.accuracyTouch.ToStringPercent(),
					" - ",
					this.accuracyShort.ToStringPercent(),
					" - ",
					this.accuracyMedium.ToStringPercent(),
					" - ",
					this.accuracyLong.ToStringPercent()
				});
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0003A648 File Offset: 0x00038848
		public bool IsMeleeAttack
		{
			get
			{
				return typeof(Verb_MeleeAttack).IsAssignableFrom(this.verbClass);
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0003A660 File Offset: 0x00038860
		public bool CausesExplosion
		{
			get
			{
				return this.defaultProjectile != null && (typeof(Projectile_Explosive).IsAssignableFrom(this.defaultProjectile.thingClass) || typeof(Projectile_DoomsdayRocket).IsAssignableFrom(this.defaultProjectile.thingClass) || this.defaultProjectile.GetCompProperties<CompProperties_Explosive>() != null);
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0003A6C0 File Offset: 0x000388C0
		public float ForcedMissRadius
		{
			get
			{
				if (this.isMortar && this.forcedMissRadiusClassicMortars >= 0f)
				{
					Storyteller storyteller = Find.Storyteller;
					if (((storyteller != null) ? storyteller.difficulty : null) != null && Find.Storyteller.difficulty.classicMortars)
					{
						return this.forcedMissRadiusClassicMortars;
					}
				}
				return this.forcedMissRadius;
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0003A714 File Offset: 0x00038914
		public float AdjustedRange(Verb ownerVerb, Thing attacker)
		{
			float num = (this.rangeStat == null) ? this.range : attacker.GetStatValue(this.rangeStat, true, -1);
			CompUniqueWeapon compUniqueWeapon;
			if (((ownerVerb != null) ? ownerVerb.EquipmentSource : null) == null || !ownerVerb.EquipmentSource.TryGetComp(out compUniqueWeapon) || !compUniqueWeapon.IgnoreAccuracyMaluses)
			{
				Map mapHeld = attacker.MapHeld;
				if (mapHeld != null && mapHeld.weatherManager.CurWeatherMaxRangeCap >= 0f)
				{
					num = Mathf.Min(num, attacker.Map.weatherManager.CurWeatherMaxRangeCap);
				}
			}
			return num;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0003A7A4 File Offset: 0x000389A4
		public float AdjustedMeleeDamageAmount(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee damage amount for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 5469809);
				return 0f;
			}
			return this.AdjustedMeleeDamageAmount(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0003A7FC File Offset: 0x000389FC
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempting to get melee damage for a non-melee verb ");
				defaultInterpolatedStringHandler.AppendFormatted<VerbProperties>(this);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 26181238);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount(equipment, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0003A86C File Offset: 0x00038A6C
		public float AdjustedMeleeDamageAmount(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (!this.IsMeleeAttack)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempting to get melee damage for a non-melee verb ");
				defaultInterpolatedStringHandler.AppendFormatted<VerbProperties>(this);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 26181238);
			}
			float num;
			if (tool != null)
			{
				num = tool.AdjustedBaseMeleeDamageAmount(equipment, equipmentStuff, this.meleeDamageDef);
			}
			else
			{
				num = (float)this.meleeDamageBaseAmount;
			}
			if (attacker != null)
			{
				num *= this.GetDamageFactorFor(tool, attacker, hediffCompSource);
			}
			return num;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0003A8E0 File Offset: 0x00038AE0
		public float AdjustedArmorPenetration(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate armor penetration for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 9865767);
				return 0f;
			}
			return this.AdjustedArmorPenetration(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0003A938 File Offset: 0x00038B38
		public float AdjustedArmorPenetration(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValue = equipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true, -1);
				num *= statValue;
			}
			return num;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0003A98C File Offset: 0x00038B8C
		public float AdjustedArmorPenetration(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			float num;
			if (tool != null)
			{
				num = tool.armorPenetration;
			}
			else
			{
				num = this.meleeArmorPenetrationBase;
			}
			if (num < 0f)
			{
				num = this.AdjustedMeleeDamageAmount(tool, attacker, equipment, equipmentStuff, hediffCompSource) * 0.015f;
			}
			else if (equipment != null)
			{
				float statValueAbstract = equipment.GetStatValueAbstract(StatDefOf.MeleeWeapon_DamageMultiplier, equipmentStuff);
				num *= statValueAbstract;
			}
			return num;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0003A9E0 File Offset: 0x00038BE0
		private float AdjustedExpectedDamageForVerbUsableInMelee(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount(tool, attacker, equipment, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, null);
			}
			return 0f;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0003AA20 File Offset: 0x00038C20
		private float AdjustedExpectedDamageForVerbUsableInMelee(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource)
		{
			if (this.IsMeleeAttack)
			{
				return this.AdjustedMeleeDamageAmount(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			}
			if (this.LaunchesProjectile && this.defaultProjectile != null)
			{
				return (float)this.defaultProjectile.projectile.GetDamageAmount(equipment, equipmentStuff, null);
			}
			return 0f;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0003AA70 File Offset: 0x00038C70
		public float AdjustedMeleeSelectionWeight(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate melee selection weight for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 385716351);
				return 0f;
			}
			return this.AdjustedMeleeSelectionWeight(ownerVerb.tool, attacker, ownerVerb.EquipmentSource, ownerVerb.HediffCompSource, ownerVerb.DirectOwner is Pawn);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0003AAD4 File Offset: 0x00038CD4
		public float AdjustedMeleeSelectionWeight(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee(tool, attacker, equipment, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0003AB70 File Offset: 0x00038D70
		public float AdjustedMeleeSelectionWeight(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource, bool comesFromPawnNativeVerbs)
		{
			if (!this.IsMeleeAttack)
			{
				return 0f;
			}
			if (attacker != null && attacker.RaceProps.intelligence < this.minIntelligence)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = this.AdjustedExpectedDamageForVerbUsableInMelee(tool, attacker, equipment, equipmentStuff, hediffCompSource);
			if (num2 >= 0.001f || !typeof(Verb_MeleeApplyHediff).IsAssignableFrom(this.verbClass))
			{
				num *= num2 * num2;
			}
			num *= this.commonality;
			if (tool != null)
			{
				num *= tool.chanceFactor;
			}
			if (comesFromPawnNativeVerbs && (tool == null || !tool.alwaysTreatAsWeapon))
			{
				num *= 0.3f;
			}
			return num;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0003AC10 File Offset: 0x00038E10
		public float AdjustedCooldown(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate cooldown for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 19485711);
				return 0f;
			}
			return this.AdjustedCooldown(ownerVerb.tool, attacker, ownerVerb.EquipmentSource);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0003AC60 File Offset: 0x00038E60
		public float AdjustedCooldown(Tool tool, Pawn attacker, Thing equipment)
		{
			float num = this.defaultCooldownTime;
			if (tool != null)
			{
				num = tool.AdjustedCooldown(equipment);
			}
			else if (equipment != null && !this.IsMeleeAttack)
			{
				num = equipment.GetStatValue(StatDefOf.RangedWeapon_Cooldown, true, -1);
			}
			if (attacker != null)
			{
				if (this.IsMeleeAttack)
				{
					num *= attacker.GetStatValue(StatDefOf.MeleeCooldownFactor, true, -1);
				}
				else
				{
					num *= attacker.GetStatValue(StatDefOf.RangedCooldownFactor, true, -1);
				}
			}
			return num;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0003ACC8 File Offset: 0x00038EC8
		public float AdjustedCooldown(Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff)
		{
			float num = this.defaultCooldownTime;
			if (tool != null)
			{
				num = tool.AdjustedCooldown(equipment, equipmentStuff);
			}
			else if (equipment != null && !this.IsMeleeAttack)
			{
				num = equipment.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, equipmentStuff);
			}
			if (attacker != null)
			{
				if (this.IsMeleeAttack)
				{
					num *= attacker.GetStatValue(StatDefOf.MeleeCooldownFactor, true, -1);
				}
				else
				{
					num *= attacker.GetStatValue(StatDefOf.RangedCooldownFactor, true, -1);
				}
			}
			return num;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0003AD32 File Offset: 0x00038F32
		public int AdjustedCooldownTicks(Verb ownerVerb, Pawn attacker)
		{
			return this.AdjustedCooldown(ownerVerb, attacker).SecondsToTicks();
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0003AD44 File Offset: 0x00038F44
		private float AdjustedAccuracy(RangeCategory cat, Thing equipment)
		{
			if (equipment != null)
			{
				StatDef stat = null;
				switch (cat)
				{
				case RangeCategory.Touch:
					stat = StatDefOf.AccuracyTouch;
					break;
				case RangeCategory.Short:
					stat = StatDefOf.AccuracyShort;
					break;
				case RangeCategory.Medium:
					stat = StatDefOf.AccuracyMedium;
					break;
				case RangeCategory.Long:
					stat = StatDefOf.AccuracyLong;
					break;
				}
				return equipment.GetStatValue(stat, true, -1);
			}
			switch (cat)
			{
			case RangeCategory.Touch:
				return this.accuracyTouch;
			case RangeCategory.Short:
				return this.accuracyShort;
			case RangeCategory.Medium:
				return this.accuracyMedium;
			case RangeCategory.Long:
				return this.accuracyLong;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0003ADCF File Offset: 0x00038FCF
		public float AdjustedFullCycleTime(Verb ownerVerb, Pawn attacker)
		{
			return this.warmupTime + this.AdjustedCooldown(ownerVerb, attacker) + ((ownerVerb.BurstShotCount - 1) * ownerVerb.TicksBetweenBurstShots).TicksToSeconds();
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0003ADF8 File Offset: 0x00038FF8
		public float GetDamageFactorFor(Verb ownerVerb, Pawn attacker)
		{
			if (ownerVerb.verbProps != this)
			{
				Log.ErrorOnce("Tried to calculate damage factor for a verb with different verb props. verb=" + ((ownerVerb != null) ? ownerVerb.ToString() : null), 94324562);
				return 1f;
			}
			return this.GetDamageFactorFor(ownerVerb.tool, attacker, ownerVerb.HediffCompSource);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0003AE48 File Offset: 0x00039048
		public float GetDamageFactorFor(Tool tool, Pawn attacker, HediffComp_VerbGiver hediffCompSource)
		{
			float num = 1f;
			if (attacker != null)
			{
				if (hediffCompSource != null && hediffCompSource.parent.Part != null)
				{
					num *= PawnCapacityUtility.CalculatePartEfficiency(hediffCompSource.Pawn.health.hediffSet, hediffCompSource.parent.Part, true, null);
				}
				else if (attacker != null && this.AdjustedLinkedBodyPartsGroup(tool) != null)
				{
					float num2 = PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(attacker.health.hediffSet, this.AdjustedLinkedBodyPartsGroup(tool));
					if (this.AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(tool))
					{
						num2 = Mathf.Max(num2, 0.4f);
					}
					num *= num2;
				}
				if (attacker != null && this.IsMeleeAttack)
				{
					num *= attacker.ageTracker.CurLifeStage.meleeDamageFactor;
					num *= attacker.GetStatValue(StatDefOf.MeleeDamageFactor, true, -1);
				}
			}
			return num;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0003AF04 File Offset: 0x00039104
		public BodyPartGroupDef AdjustedLinkedBodyPartsGroup(Tool tool)
		{
			if (tool != null)
			{
				return tool.linkedBodyPartsGroup;
			}
			return this.linkedBodyPartsGroup;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0003AF16 File Offset: 0x00039116
		public bool AdjustedEnsureLinkedBodyPartsGroupAlwaysUsable(Tool tool)
		{
			if (tool != null)
			{
				return tool.ensureLinkedBodyPartsGroupAlwaysUsable;
			}
			return this.ensureLinkedBodyPartsGroupAlwaysUsable;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0003AF28 File Offset: 0x00039128
		public float EffectiveMinRange(LocalTargetInfo target, Thing caster)
		{
			return this.EffectiveMinRange(VerbUtility.AllowAdjacentShot(target, caster));
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0003AF38 File Offset: 0x00039138
		public float EffectiveMinRange(bool allowAdjacentShot)
		{
			float num = this.minRange;
			if (!allowAdjacentShot && !this.IsMeleeAttack && this.LaunchesProjectile)
			{
				num = Mathf.Max(num, 1.421f);
			}
			return num;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0003AF6C File Offset: 0x0003916C
		public float GetHitChanceFactor(Thing equipment, float dist)
		{
			float value;
			if (dist <= 3f)
			{
				value = this.AdjustedAccuracy(RangeCategory.Touch, equipment);
			}
			else if (dist <= 12f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Touch, equipment), this.AdjustedAccuracy(RangeCategory.Short, equipment), (dist - 3f) / 9f);
			}
			else if (dist <= 25f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Short, equipment), this.AdjustedAccuracy(RangeCategory.Medium, equipment), (dist - 12f) / 13f);
			}
			else if (dist <= 40f)
			{
				value = Mathf.Lerp(this.AdjustedAccuracy(RangeCategory.Medium, equipment), this.AdjustedAccuracy(RangeCategory.Long, equipment), (dist - 25f) / 15f);
			}
			else
			{
				value = this.AdjustedAccuracy(RangeCategory.Long, equipment);
			}
			return Mathf.Clamp(value, 0.01f, 1f);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0003B031 File Offset: 0x00039231
		public float GetForceMissFactorFor(Thing equipment, Pawn caster)
		{
			if (equipment.def.building != null && equipment.def.building.IsMortar)
			{
				return caster.GetStatValueForPawn(StatDefOf.MortarMissRadiusFactor, caster, true);
			}
			return 1f;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0003B068 File Offset: 0x00039268
		public void DrawRadiusRing(IntVec3 center, Verb verb = null)
		{
			if (Find.CurrentMap == null || Find.World.renderer.wantedMode == WorldRenderMode.Planet)
			{
				return;
			}
			if (!this.IsMeleeAttack && this.targetable)
			{
				float num = this.EffectiveMinRange(true);
				float num2 = (verb != null) ? verb.EffectiveRange : this.range;
				if (num > 0f && num < GenRadial.MaxRadialPatternRadius)
				{
					GenDraw.DrawRadiusRing(center, num);
				}
				if (num2 < (float)(Find.CurrentMap.Size.x + Find.CurrentMap.Size.z) && num2 < GenRadial.MaxRadialPatternRadius)
				{
					Func<IntVec3, bool> predicate = null;
					if (this.drawHighlightWithLineOfSight)
					{
						predicate = ((IntVec3 c) => GenSight.LineOfSight(center, c, Find.CurrentMap));
					}
					GenDraw.DrawRadiusRing(center, num2, Color.white, predicate);
				}
			}
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0003B13C File Offset: 0x0003933C
		public override string ToString()
		{
			string str;
			if (!this.label.NullOrEmpty())
			{
				str = this.label;
			}
			else
			{
				str = "range=" + this.range.ToString() + ", defaultProjectile=" + this.defaultProjectile.ToStringSafe<ThingDef>();
			}
			return "VerbProperties(" + str + ")";
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0003B195 File Offset: 0x00039395
		public new VerbProperties MemberwiseClone()
		{
			return (VerbProperties)base.MemberwiseClone();
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0003B1A2 File Offset: 0x000393A2
		public IEnumerable<string> ConfigErrors(ThingDef parent)
		{
			VerbProperties.<ConfigErrors>d__162 <ConfigErrors>d__ = new VerbProperties.<ConfigErrors>d__162(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parent = parent;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0003B1B9 File Offset: 0x000393B9
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x0400076A RID: 1898
		public VerbCategory category = VerbCategory.Misc;

		// Token: 0x0400076B RID: 1899
		[TranslationHandle]
		public Type verbClass = typeof(Verb);

		// Token: 0x0400076C RID: 1900
		[MustTranslate]
		public string label;

		// Token: 0x0400076D RID: 1901
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x0400076E RID: 1902
		public bool isPrimary = true;

		// Token: 0x0400076F RID: 1903
		public bool violent = true;

		// Token: 0x04000770 RID: 1904
		public float minRange;

		// Token: 0x04000771 RID: 1905
		public float range = 1.42f;

		// Token: 0x04000772 RID: 1906
		public StatDef rangeStat;

		// Token: 0x04000773 RID: 1907
		public int burstShotCount = 1;

		// Token: 0x04000774 RID: 1908
		public int ticksBetweenBurstShots = 15;

		// Token: 0x04000775 RID: 1909
		public bool showBurstShotStats = true;

		// Token: 0x04000776 RID: 1910
		public float noiseRadius = 3f;

		// Token: 0x04000777 RID: 1911
		public bool hasStandardCommand;

		// Token: 0x04000778 RID: 1912
		public bool targetable = true;

		// Token: 0x04000779 RID: 1913
		public bool nonInterruptingSelfCast;

		// Token: 0x0400077A RID: 1914
		public TargetingParameters targetParams = new TargetingParameters();

		// Token: 0x0400077B RID: 1915
		public bool requireLineOfSight = true;

		// Token: 0x0400077C RID: 1916
		public bool mustCastOnOpenGround;

		// Token: 0x0400077D RID: 1917
		public bool forceNormalTimeSpeed = true;

		// Token: 0x0400077E RID: 1918
		public bool onlyManualCast;

		// Token: 0x0400077F RID: 1919
		public bool stopBurstWithoutLos = true;

		// Token: 0x04000780 RID: 1920
		public SurpriseAttackProps surpriseAttack;

		// Token: 0x04000781 RID: 1921
		public float commonality = 1f;

		// Token: 0x04000782 RID: 1922
		public Intelligence minIntelligence;

		// Token: 0x04000783 RID: 1923
		public float consumeFuelPerShot;

		// Token: 0x04000784 RID: 1924
		public float consumeFuelPerBurst;

		// Token: 0x04000785 RID: 1925
		public bool stunTargetOnCastStart;

		// Token: 0x04000786 RID: 1926
		public string invalidTargetPawn;

		// Token: 0x04000787 RID: 1927
		public float commonalityVsEdificeFactor = 1f;

		// Token: 0x04000788 RID: 1928
		public SimpleCurve flammabilityAttachFireChanceCurve;

		// Token: 0x04000789 RID: 1929
		public bool useableInPocketMaps = true;

		// Token: 0x0400078A RID: 1930
		public bool useableInVacuum = true;

		// Token: 0x0400078B RID: 1931
		[MustTranslate]
		public string mouseTargetingText;

		// Token: 0x0400078C RID: 1932
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x0400078D RID: 1933
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x0400078E RID: 1934
		public float warmupTime;

		// Token: 0x0400078F RID: 1935
		public float defaultCooldownTime;

		// Token: 0x04000790 RID: 1936
		public string commandIcon;

		// Token: 0x04000791 RID: 1937
		public SoundDef soundCast;

		// Token: 0x04000792 RID: 1938
		public SoundDef soundCastTail;

		// Token: 0x04000793 RID: 1939
		public SoundDef soundAiming;

		// Token: 0x04000794 RID: 1940
		public float muzzleFlashScale;

		// Token: 0x04000795 RID: 1941
		public ThingDef impactMote;

		// Token: 0x04000796 RID: 1942
		public FleckDef impactFleck;

		// Token: 0x04000797 RID: 1943
		public bool drawAimPie = true;

		// Token: 0x04000798 RID: 1944
		public EffecterDef warmupEffecter;

		// Token: 0x04000799 RID: 1945
		public bool drawHighlightWithLineOfSight;

		// Token: 0x0400079A RID: 1946
		public ThingDef aimingLineMote;

		// Token: 0x0400079B RID: 1947
		public float? aimingLineMoteFixedLength;

		// Token: 0x0400079C RID: 1948
		public ThingDef aimingChargeMote;

		// Token: 0x0400079D RID: 1949
		public float aimingChargeMoteOffset;

		// Token: 0x0400079E RID: 1950
		public ThingDef aimingTargetMote;

		// Token: 0x0400079F RID: 1951
		public EffecterDef aimingTargetEffecter;

		// Token: 0x040007A0 RID: 1952
		public Color explosionRadiusRingColor = Color.white;

		// Token: 0x040007A1 RID: 1953
		public BodyPartGroupDef linkedBodyPartsGroup;

		// Token: 0x040007A2 RID: 1954
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		// Token: 0x040007A3 RID: 1955
		public DamageDef meleeDamageDef;

		// Token: 0x040007A4 RID: 1956
		public int meleeDamageBaseAmount = 1;

		// Token: 0x040007A5 RID: 1957
		public float meleeArmorPenetrationBase = -1f;

		// Token: 0x040007A6 RID: 1958
		public bool ai_IsWeapon = true;

		// Token: 0x040007A7 RID: 1959
		public bool ai_IsBuildingDestroyer;

		// Token: 0x040007A8 RID: 1960
		public float ai_AvoidFriendlyFireRadius;

		// Token: 0x040007A9 RID: 1961
		public bool ai_RangedAlawaysShootGroundBelowTarget;

		// Token: 0x040007AA RID: 1962
		public bool ai_IsDoorDestroyer;

		// Token: 0x040007AB RID: 1963
		public bool ai_ProjectileLaunchingIgnoresMeleeThreats;

		// Token: 0x040007AC RID: 1964
		public float ai_TargetHasRangedAttackScoreOffset;

		// Token: 0x040007AD RID: 1965
		public ThingDef defaultProjectile;

		// Token: 0x040007AE RID: 1966
		private float forcedMissRadius;

		// Token: 0x040007AF RID: 1967
		private float forcedMissRadiusClassicMortars = -1f;

		// Token: 0x040007B0 RID: 1968
		public bool forcedMissEvenDispersal;

		// Token: 0x040007B1 RID: 1969
		private bool isMortar;

		// Token: 0x040007B2 RID: 1970
		public float accuracyTouch = 1f;

		// Token: 0x040007B3 RID: 1971
		public float accuracyShort = 1f;

		// Token: 0x040007B4 RID: 1972
		public float accuracyMedium = 1f;

		// Token: 0x040007B5 RID: 1973
		public float accuracyLong = 1f;

		// Token: 0x040007B6 RID: 1974
		public bool canGoWild = true;

		// Token: 0x040007B7 RID: 1975
		public DamageDef beamDamageDef;

		// Token: 0x040007B8 RID: 1976
		public float beamWidth = 1f;

		// Token: 0x040007B9 RID: 1977
		public float beamMaxDeviation;

		// Token: 0x040007BA RID: 1978
		public FleckDef beamGroundFleckDef;

		// Token: 0x040007BB RID: 1979
		public EffecterDef beamEndEffecterDef;

		// Token: 0x040007BC RID: 1980
		public ThingDef beamMoteDef;

		// Token: 0x040007BD RID: 1981
		public float beamFleckChancePerTick;

		// Token: 0x040007BE RID: 1982
		public float beamCurvature;

		// Token: 0x040007BF RID: 1983
		public float beamChanceToStartFire;

		// Token: 0x040007C0 RID: 1984
		public float beamChanceToAttachFire;

		// Token: 0x040007C1 RID: 1985
		public float beamStartOffset;

		// Token: 0x040007C2 RID: 1986
		public float beamFullWidthRange;

		// Token: 0x040007C3 RID: 1987
		public FleckDef beamLineFleckDef;

		// Token: 0x040007C4 RID: 1988
		public SimpleCurve beamLineFleckChanceCurve;

		// Token: 0x040007C5 RID: 1989
		public FloatRange beamFireSizeRange = FloatRange.ZeroToOne;

		// Token: 0x040007C6 RID: 1990
		public SoundDef soundCastBeam;

		// Token: 0x040007C7 RID: 1991
		public bool beamTargetsGround;

		// Token: 0x040007C8 RID: 1992
		public bool beamSetsGroundOnFire;

		// Token: 0x040007C9 RID: 1993
		public float beamTotalDamage;

		// Token: 0x040007CA RID: 1994
		public bool beamHitsNeighborCells;

		// Token: 0x040007CB RID: 1995
		public bool beamCantHitWithinMinRange;

		// Token: 0x040007CC RID: 1996
		public bool beamHitsNeighborCellsRequiresLOS;

		// Token: 0x040007CD RID: 1997
		public bool ai_BeamIsIncendiary;

		// Token: 0x040007CE RID: 1998
		public float sprayWidth;

		// Token: 0x040007CF RID: 1999
		public float sprayArching;

		// Token: 0x040007D0 RID: 2000
		public int sprayNumExtraCells;

		// Token: 0x040007D1 RID: 2001
		public int sprayThicknessCells = 1;

		// Token: 0x040007D2 RID: 2002
		public EffecterDef sprayEffecterDef;

		// Token: 0x040007D3 RID: 2003
		public Color? highlightColor;

		// Token: 0x040007D4 RID: 2004
		public Color? secondaryHighlightColor;

		// Token: 0x040007D5 RID: 2005
		public ThingDef spawnDef;

		// Token: 0x040007D6 RID: 2006
		public TaleDef colonyWideTaleDef;

		// Token: 0x040007D7 RID: 2007
		public int affectedCellCount;

		// Token: 0x040007D8 RID: 2008
		public BodyPartTagDef bodypartTagTarget;

		// Token: 0x040007D9 RID: 2009
		public RulePackDef rangedFireRulepack;

		// Token: 0x040007DA RID: 2010
		public SoundDef soundLanding;

		// Token: 0x040007DB RID: 2011
		public EffecterDef flightEffecterDef;

		// Token: 0x040007DC RID: 2012
		public bool flyWithCarriedThing = true;

		// Token: 0x040007DD RID: 2013
		public MechWorkModeDef workModeDef;

		// Token: 0x040007DE RID: 2014
		public const float DefaultArmorPenetrationPerDamage = 0.015f;

		// Token: 0x040007DF RID: 2015
		private const float VerbSelectionWeightFactor_BodyPart = 0.3f;

		// Token: 0x040007E0 RID: 2016
		private const float MinLinkedBodyPartGroupEfficiencyIfMustBeAlwaysUsable = 0.4f;
	}
}
