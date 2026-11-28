using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E6 RID: 998
	public class DamageWorker_AddInjury : DamageWorker
	{
		// Token: 0x06001D55 RID: 7509 RVA: 0x000B2FEC File Offset: 0x000B11EC
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				return base.Apply(dinfo, thing);
			}
			return this.ApplyToPawn(dinfo, pawn);
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x000B3014 File Offset: 0x000B1214
		private DamageWorker.DamageResult ApplyToPawn(DamageInfo dinfo, Pawn pawn)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (dinfo.Amount <= 0f)
			{
				return damageResult;
			}
			if (!DebugSettings.enablePlayerDamage && pawn.Faction == Faction.OfPlayer)
			{
				return damageResult;
			}
			Map mapHeld = pawn.MapHeld;
			bool spawnedOrAnyParentSpawned = pawn.SpawnedOrAnyParentSpawned;
			if (dinfo.ApplyAllDamage)
			{
				float num = dinfo.Amount;
				int num2 = 25;
				float b = num / (float)dinfo.DamagePropagationPartsRange.RandomInRange;
				do
				{
					DamageInfo dinfo2 = dinfo;
					dinfo2.SetAmount(Mathf.Min(num, b));
					this.ApplyDamageToPart(dinfo2, pawn, damageResult);
					num -= damageResult.totalDamageDealt;
					if (num2-- <= 0)
					{
						break;
					}
				}
				while (num > 0f);
			}
			else if (dinfo.AllowDamagePropagation && dinfo.Amount >= (float)dinfo.Def.minDamageToFragment)
			{
				int randomInRange = dinfo.DamagePropagationPartsRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					DamageInfo dinfo3 = dinfo;
					dinfo3.SetAmount(dinfo.Amount / (float)randomInRange);
					this.ApplyDamageToPart(dinfo3, pawn, damageResult);
				}
			}
			else
			{
				this.ApplyDamageToPart(dinfo, pawn, damageResult);
				this.ApplySmallPawnDamagePropagation(dinfo, pawn, damageResult);
			}
			if (damageResult.wounded)
			{
				DamageWorker_AddInjury.PlayWoundedVoiceSound(dinfo, pawn);
				pawn.Drawer.Notify_DamageApplied(dinfo);
				EffecterDef damageEffecter = pawn.RaceProps.FleshType.damageEffecter;
				if (damageEffecter != null)
				{
					if (pawn.health.woundedEffecter != null && pawn.health.woundedEffecter.def != damageEffecter)
					{
						pawn.health.woundedEffecter.Cleanup();
					}
					pawn.health.woundedEffecter = damageEffecter.Spawn();
					pawn.health.woundedEffecter.Trigger(pawn, dinfo.Instigator ?? pawn, -1);
				}
				if (dinfo.Def.damageEffecter != null)
				{
					Effecter effecter = dinfo.Def.damageEffecter.Spawn();
					effecter.Trigger(pawn, pawn, -1);
					effecter.Cleanup();
				}
			}
			if (damageResult.headshot && pawn.Spawned)
			{
				MoteMaker.ThrowText(new Vector3((float)pawn.Position.x + 1f, (float)pawn.Position.y, (float)pawn.Position.z + 1f), pawn.Map, "Headshot".Translate(), Color.white, -1f);
				if (dinfo.Instigator != null)
				{
					Pawn pawn2 = dinfo.Instigator as Pawn;
					if (pawn2 != null)
					{
						pawn2.records.Increment(RecordDefOf.Headshots);
					}
				}
			}
			if ((damageResult.deflected || damageResult.diminished) && spawnedOrAnyParentSpawned)
			{
				EffecterDef effecterDef;
				if (damageResult.deflected)
				{
					if (damageResult.deflectedByMetalArmor && dinfo.Def.canUseDeflectMetalEffect)
					{
						if (dinfo.Def == DamageDefOf.Bullet)
						{
							effecterDef = EffecterDefOf.Deflect_Metal_Bullet;
						}
						else
						{
							effecterDef = EffecterDefOf.Deflect_Metal;
						}
					}
					else if (dinfo.Def == DamageDefOf.Bullet)
					{
						effecterDef = EffecterDefOf.Deflect_General_Bullet;
					}
					else
					{
						effecterDef = EffecterDefOf.Deflect_General;
					}
				}
				else if (damageResult.diminishedByMetalArmor)
				{
					effecterDef = EffecterDefOf.DamageDiminished_Metal;
				}
				else
				{
					effecterDef = EffecterDefOf.DamageDiminished_General;
				}
				if (pawn.health.deflectionEffecter == null || pawn.health.deflectionEffecter.def != effecterDef)
				{
					if (pawn.health.deflectionEffecter != null)
					{
						pawn.health.deflectionEffecter.Cleanup();
						pawn.health.deflectionEffecter = null;
					}
					pawn.health.deflectionEffecter = effecterDef.Spawn();
				}
				TargetInfo targetInfo = new TargetInfo(pawn.Position, mapHeld, false);
				Effecter deflectionEffecter = pawn.health.deflectionEffecter;
				TargetInfo a = targetInfo;
				Thing instigator = dinfo.Instigator;
				deflectionEffecter.Trigger(a, (instigator != null) ? instigator : targetInfo, -1);
				if (damageResult.deflected)
				{
					pawn.Drawer.Notify_DamageDeflected(dinfo);
				}
			}
			if (!damageResult.deflected && spawnedOrAnyParentSpawned)
			{
				ImpactSoundUtility.PlayImpactSound(pawn, dinfo.Def.impactSoundType, mapHeld);
			}
			return damageResult;
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x000B3400 File Offset: 0x000B1600
		private void ApplySmallPawnDamagePropagation(DamageInfo dinfo, Pawn pawn, DamageWorker.DamageResult result)
		{
			if (!dinfo.AllowDamagePropagation)
			{
				return;
			}
			if (result.LastHitPart != null && dinfo.Def.harmsHealth && result.LastHitPart != pawn.RaceProps.body.corePart && result.LastHitPart.parent != null && pawn.health.hediffSet.GetPartHealth(result.LastHitPart.parent) > 0f && result.LastHitPart.parent.coverageAbs > 0f && dinfo.Amount >= 10f && pawn.HealthScale <= 0.5001f)
			{
				DamageInfo dinfo2 = dinfo;
				dinfo2.SetHitPart(result.LastHitPart.parent);
				this.ApplyDamageToPart(dinfo2, pawn, result);
			}
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000B34CC File Offset: 0x000B16CC
		private void ApplyDamageToPart(DamageInfo dinfo, Pawn pawn, DamageWorker.DamageResult result)
		{
			BodyPartRecord exactPartFromDamageInfo = this.GetExactPartFromDamageInfo(dinfo, pawn);
			if (exactPartFromDamageInfo == null)
			{
				return;
			}
			dinfo.SetHitPart(exactPartFromDamageInfo);
			float num = dinfo.Amount;
			bool flag = !dinfo.InstantPermanentInjury && !dinfo.IgnoreArmor;
			bool deflectedByMetalArmor = false;
			if (flag)
			{
				DamageDef def = dinfo.Def;
				bool diminishedByMetalArmor;
				num = ArmorUtility.GetPostArmorDamage(pawn, num, dinfo.ArmorPenetrationInt, dinfo.HitPart, ref def, out deflectedByMetalArmor, out diminishedByMetalArmor);
				dinfo.Def = def;
				if (num < dinfo.Amount)
				{
					result.diminished = true;
					result.diminishedByMetalArmor = diminishedByMetalArmor;
				}
			}
			if (dinfo.Def.ExternalViolenceFor(pawn))
			{
				num *= pawn.GetStatValue(StatDefOf.IncomingDamageFactor, true, -1);
			}
			if (num <= 0f)
			{
				result.AddPart(pawn, dinfo.HitPart);
				result.deflected = true;
				result.deflectedByMetalArmor = deflectedByMetalArmor;
				return;
			}
			if (DamageWorker_AddInjury.IsHeadshot(dinfo, pawn))
			{
				result.headshot = true;
			}
			if (dinfo.InstantPermanentInjury && (HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart).CompPropsFor(typeof(HediffComp_GetsPermanent)) == null || dinfo.HitPart.def.permanentInjuryChanceFactor == 0f || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(dinfo.HitPart)))
			{
				return;
			}
			if (!dinfo.AllowDamagePropagation)
			{
				this.FinalizeAndAddInjury(pawn, num, dinfo, result);
				return;
			}
			this.ApplySpecialEffectsToPart(pawn, num, dinfo, result);
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x000B3629 File Offset: 0x000B1829
		protected virtual void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage = this.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn);
			this.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
			this.CheckDuplicateDamageToOuterParts(dinfo, pawn, totalDamage, result);
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x000B3650 File Offset: 0x000B1850
		protected float FinalizeAndAddInjury(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			if (pawn.health.hediffSet.PartIsMissing(dinfo.HitPart))
			{
				return 0f;
			}
			Pawn pawn2 = dinfo.Instigator as Pawn;
			HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart);
			Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
			hediff_Injury.Part = dinfo.HitPart;
			hediff_Injury.sourceDef = dinfo.Weapon;
			if (pawn2 != null && pawn2.IsMutant && dinfo.Weapon == ThingDefOf.Human)
			{
				hediff_Injury.sourceLabel = pawn2.mutant.Def.label;
			}
			else
			{
				Hediff hediff = hediff_Injury;
				ThingDef weapon = dinfo.Weapon;
				hediff.sourceLabel = (((weapon != null) ? weapon.label : null) ?? "");
			}
			hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
			hediff_Injury.sourceHediffDef = dinfo.WeaponLinkedHediff;
			Hediff hediff2 = hediff_Injury;
			Tool tool = dinfo.Tool;
			string sourceToolLabel;
			if ((sourceToolLabel = ((tool != null) ? tool.labelNoLocation : null)) == null)
			{
				Tool tool2 = dinfo.Tool;
				sourceToolLabel = ((tool2 != null) ? tool2.label : null);
			}
			hediff2.sourceToolLabel = sourceToolLabel;
			hediff_Injury.Severity = totalDamage;
			if (pawn2 != null && pawn2.CurJobDef == JobDefOf.SocialFight)
			{
				hediff_Injury.destroysBodyParts = false;
			}
			if (dinfo.InstantPermanentInjury)
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff_Injury.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null)
				{
					hediffComp_GetsPermanent.IsPermanent = true;
				}
				else
				{
					string str = "Tried to create instant permanent injury on Hediff without a GetsPermanent comp: ";
					HediffDef hediffDef = hediffDefFromDamage;
					Log.Error(str + ((hediffDef != null) ? hediffDef.ToString() : null) + " on " + ((pawn != null) ? pawn.ToString() : null));
				}
			}
			return this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, result);
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x000B37D8 File Offset: 0x000B19D8
		protected float FinalizeAndAddInjury(Pawn pawn, Hediff_Injury injury, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			HediffComp_GetsPermanent hediffComp_GetsPermanent = injury.TryGetComp<HediffComp_GetsPermanent>();
			if (hediffComp_GetsPermanent != null)
			{
				hediffComp_GetsPermanent.PreFinalizeInjury();
			}
			float partHealth = pawn.health.hediffSet.GetPartHealth(injury.Part);
			if (pawn.IsColonist && !dinfo.IgnoreInstantKillProtection && dinfo.Def.ExternalViolenceFor(pawn) && !Rand.Chance(Find.Storyteller.difficulty.allowInstantKillChance))
			{
				float num = injury.IsLethal ? (injury.def.lethalSeverity * 1.1f) : 1f;
				float min = 1f;
				float max = Mathf.Min(injury.Severity, partHealth);
				int num2 = 0;
				while (num2 < 7 && pawn.health.WouldDieAfterAddingHediff(injury))
				{
					float num3 = Mathf.Clamp(partHealth - num, min, max);
					if (DebugViewSettings.logCauseOfDeath)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(81, 6);
						defaultInterpolatedStringHandler.AppendLiteral("CauseOfDeath: attempt to prevent death for ");
						defaultInterpolatedStringHandler.AppendFormatted<Name>(pawn.Name);
						defaultInterpolatedStringHandler.AppendLiteral(" on ");
						defaultInterpolatedStringHandler.AppendFormatted(injury.Part.Label);
						defaultInterpolatedStringHandler.AppendLiteral(" attempt:");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num2 + 1);
						defaultInterpolatedStringHandler.AppendLiteral(" severity:");
						defaultInterpolatedStringHandler.AppendFormatted<float>(injury.Severity);
						defaultInterpolatedStringHandler.AppendLiteral("->");
						defaultInterpolatedStringHandler.AppendFormatted<float>(num3);
						defaultInterpolatedStringHandler.AppendLiteral(" part health:");
						defaultInterpolatedStringHandler.AppendFormatted<float>(partHealth);
						Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					injury.Severity = num3;
					num *= 2f;
					min = 0f;
					num2++;
				}
			}
			pawn.health.AddHediff(injury, null, new DamageInfo?(dinfo), result);
			float num4 = Mathf.Min(injury.Severity, partHealth);
			result.totalDamageDealt += num4;
			result.wounded = true;
			result.AddPart(pawn, injury.Part);
			result.AddHediff(injury);
			if (!this.def.additionalHediffsThisPart.NullOrEmpty<HediffDef>() && !pawn.health.hediffSet.PartIsMissing(injury.Part))
			{
				foreach (HediffDef def in this.def.additionalHediffsThisPart)
				{
					Hediff hediff = HediffMaker.MakeHediff(def, pawn, injury.Part);
					pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), result);
					result.AddHediff(hediff);
				}
			}
			return num4;
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000B3A6C File Offset: 0x000B1C6C
		private void CheckDuplicateDamageToOuterParts(DamageInfo dinfo, Pawn pawn, float totalDamage, DamageWorker.DamageResult result)
		{
			if (!dinfo.AllowDamagePropagation)
			{
				return;
			}
			if (dinfo.Def.harmAllLayersUntilOutside && dinfo.HitPart.depth == BodyPartDepth.Inside)
			{
				BodyPartRecord parent = dinfo.HitPart.parent;
				do
				{
					if (pawn.health.hediffSet.GetPartHealth(parent) != 0f && parent.coverageAbs > 0f)
					{
						Pawn pawn2 = dinfo.Instigator as Pawn;
						Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, parent), pawn, null);
						hediff_Injury.Part = parent;
						hediff_Injury.sourceDef = dinfo.Weapon;
						if (pawn2 != null && pawn2.IsMutant && dinfo.Weapon == ThingDefOf.Human)
						{
							hediff_Injury.sourceLabel = pawn2.mutant.Def.label;
						}
						else
						{
							Hediff hediff = hediff_Injury;
							ThingDef weapon = dinfo.Weapon;
							hediff.sourceLabel = (((weapon != null) ? weapon.label : null) ?? "");
						}
						hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
						hediff_Injury.Severity = totalDamage;
						if (hediff_Injury.Severity <= 0f)
						{
							hediff_Injury.Severity = 1f;
						}
						this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, result);
					}
					if (parent.depth == BodyPartDepth.Outside)
					{
						break;
					}
					parent = parent.parent;
				}
				while (parent != null);
			}
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x000B3BBB File Offset: 0x000B1DBB
		private static bool IsHeadshot(DamageInfo dinfo, Pawn pawn)
		{
			return !dinfo.InstantPermanentInjury && dinfo.HitPart.groups.Contains(BodyPartGroupDefOf.FullHead) && dinfo.Def.isRanged;
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000B3BF0 File Offset: 0x000B1DF0
		private BodyPartRecord GetExactPartFromDamageInfo(DamageInfo dinfo, Pawn pawn)
		{
			if (dinfo.HitPart == null)
			{
				BodyPartRecord bodyPartRecord = this.ChooseHitPart(dinfo, pawn);
				if (bodyPartRecord == null)
				{
					Log.Warning("ChooseHitPart returned null (any part).");
				}
				return bodyPartRecord;
			}
			if (!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x == dinfo.HitPart))
			{
				return null;
			}
			return dinfo.HitPart;
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x000B3C66 File Offset: 0x000B1E66
		protected virtual BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, dinfo.Depth, null);
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x000B3C90 File Offset: 0x000B1E90
		private static void PlayWoundedVoiceSound(DamageInfo dinfo, Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			if (dinfo.InstantPermanentInjury)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (dinfo.Def.ExternalViolenceFor(pawn))
			{
				LifeStageUtility.PlayNearestLifestageSound(pawn, (LifeStageAge lifeStage) => lifeStage.soundWounded, (GeneDef gene) => gene.soundWounded, (MutantDef mutantDef) => mutantDef.soundWounded, 1f);
			}
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x000B3D30 File Offset: 0x000B1F30
		protected float ReduceDamageToPreserveOutsideParts(float postArmorDamage, DamageInfo dinfo, Pawn pawn)
		{
			if (!DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(dinfo.HitPart))
			{
				return postArmorDamage;
			}
			float partHealth = pawn.health.hediffSet.GetPartHealth(dinfo.HitPart);
			if (postArmorDamage < partHealth)
			{
				return postArmorDamage;
			}
			float maxHealth = dinfo.HitPart.def.GetMaxHealth(pawn);
			float f = (postArmorDamage - partHealth) / maxHealth;
			if (Rand.Chance(this.def.overkillPctToDestroyPart.InverseLerpThroughRange(f)))
			{
				return postArmorDamage;
			}
			return postArmorDamage = partHealth - 1f;
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000B3DA8 File Offset: 0x000B1FA8
		public static bool ShouldReduceDamageToPreservePart(BodyPartRecord bodyPart)
		{
			return bodyPart.depth == BodyPartDepth.Outside && !bodyPart.IsCorePart;
		}
	}
}
