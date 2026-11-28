using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200072A RID: 1834
	public struct ShotReport
	{
		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x060039E4 RID: 14820 RVA: 0x00170FE8 File Offset: 0x0016F1E8
		private float FactorFromPosture
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance >= 4.5f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 0.5f;
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x00171038 File Offset: 0x0016F238
		private float FactorFromExecution
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance <= 3.9f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 7.5f;
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x060039E6 RID: 14822 RVA: 0x00171088 File Offset: 0x0016F288
		public float AimOnTargetChance_StandardTarget
		{
			get
			{
				float num = this.factorFromShooterAndDist * this.factorFromEquipment * this.factorFromWeather * this.factorFromCoveringGas * this.FactorFromExecution;
				num += this.offsetFromDarkness;
				if (num < 0.0201f)
				{
					num = 0.0201f;
				}
				return num;
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x060039E7 RID: 14823 RVA: 0x001710D0 File Offset: 0x0016F2D0
		public float AimOnTargetChance_IgnoringPosture
		{
			get
			{
				return this.AimOnTargetChance_StandardTarget * this.factorFromTargetSize;
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x001710DF File Offset: 0x0016F2DF
		public float AimOnTargetChance
		{
			get
			{
				return this.AimOnTargetChance_IgnoringPosture * this.FactorFromPosture;
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x060039E9 RID: 14825 RVA: 0x001710EE File Offset: 0x0016F2EE
		public float PassCoverChance
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x001710FC File Offset: 0x0016F2FC
		public float TotalEstimatedHitChance
		{
			get
			{
				return Mathf.Clamp01(this.AimOnTargetChance * this.PassCoverChance);
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x060039EB RID: 14827 RVA: 0x00171110 File Offset: 0x0016F310
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x00171118 File Offset: 0x0016F318
		public static ShotReport HitReportFor(Thing caster, Verb verb, LocalTargetInfo target)
		{
			IntVec3 cell = target.Cell;
			ShotReport shotReport;
			shotReport.distance = (cell - caster.Position).LengthHorizontal;
			shotReport.target = target.ToTargetInfo(caster.Map);
			CompUniqueWeapon compUniqueWeapon;
			bool flag = verb.EquipmentSource != null && verb.EquipmentSource.TryGetComp(out compUniqueWeapon) && compUniqueWeapon.IgnoreAccuracyMaluses;
			if (verb.verbProps.canGoWild)
			{
				shotReport.factorFromShooterAndDist = ShotReport.HitFactorFromShooter(caster, shotReport.distance, null);
			}
			else
			{
				shotReport.factorFromShooterAndDist = 1f;
			}
			shotReport.factorFromEquipment = verb.verbProps.GetHitChanceFactor(verb.EquipmentSource, shotReport.distance);
			shotReport.covers = CoverUtility.CalculateCoverGiverSet(target, caster.Position, caster.Map);
			shotReport.coversOverallBlockChance = CoverUtility.CalculateOverallBlockChance(target, caster.Position, caster.Map);
			shotReport.factorFromCoveringGas = 1f;
			if (verb.TryFindShootLineFromTo(verb.caster.Position, target, out shotReport.shootLine, false))
			{
				using (IEnumerator<IntVec3> enumerator = shotReport.shootLine.Points().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 cell2 = enumerator.Current;
						if (!flag && cell2.AnyGas(caster.Map, GasType.BlindSmoke))
						{
							shotReport.factorFromCoveringGas = 0.7f;
							break;
						}
					}
					goto IL_175;
				}
			}
			shotReport.shootLine = new ShootLine(IntVec3.Invalid, IntVec3.Invalid);
			IL_175:
			if (!flag && (!caster.Position.Roofed(caster.Map) || !target.Cell.Roofed(caster.Map)))
			{
				shotReport.factorFromWeather = caster.Map.weatherManager.CurWeatherAccuracyMultiplier;
			}
			else
			{
				shotReport.factorFromWeather = 1f;
			}
			if (target.HasThing)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					shotReport.factorFromTargetSize = pawn.BodySize;
				}
				else
				{
					shotReport.factorFromTargetSize = target.Thing.def.fillPercent * (float)target.Thing.def.size.x * (float)target.Thing.def.size.z * 2.5f;
				}
				shotReport.factorFromTargetSize = Mathf.Clamp(shotReport.factorFromTargetSize, 0.5f, 2f);
			}
			else
			{
				shotReport.factorFromTargetSize = 1f;
			}
			shotReport.forcedMissRadius = verb.verbProps.ForcedMissRadius;
			shotReport.offsetFromDarkness = 0f;
			if (ModsConfig.IdeologyActive && target.HasThing)
			{
				if (DarknessCombatUtility.IsOutdoorsAndLit(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyOutdoorsLitOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsOutdoorsAndDark(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyOutdoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndDark(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyIndoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndLit(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyIndoorsLitOffset, true, -1);
				}
			}
			return shotReport;
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x00171464 File Offset: 0x0016F664
		public static float HitFactorFromShooter(Thing caster, float distance, float? acc = null)
		{
			float f;
			if (acc != null)
			{
				f = acc.Value;
			}
			else if (caster is Pawn)
			{
				f = caster.GetStatValue(StatDefOf.ShootingAccuracyPawn, true, -1);
			}
			else if (caster != null)
			{
				f = caster.GetStatValue(StatDefOf.ShootingAccuracyTurret, true, -1);
			}
			else
			{
				f = 1f;
			}
			float num = Mathf.Pow(f, distance);
			if (caster is Pawn)
			{
				if (distance <= 3f)
				{
					num *= caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Touch, true, -1);
				}
				else if (distance <= 12f)
				{
					num *= Mathf.Lerp(caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Touch, true, -1), caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Short, true, -1), (distance - 3f) / 9f);
				}
				else if (distance <= 25f)
				{
					num *= Mathf.Lerp(caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Short, true, -1), caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Medium, true, -1), (distance - 12f) / 13f);
				}
				else if (distance <= 40f)
				{
					num *= Mathf.Lerp(caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Medium, true, -1), caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Long, true, -1), (distance - 25f) / 15f);
				}
				else
				{
					num *= caster.GetStatValue(StatDefOf.ShootingAccuracyFactor_Long, true, -1);
				}
			}
			return Mathf.Max(num, 0.0201f);
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x001715B0 File Offset: 0x0016F7B0
		public string GetTextReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.forcedMissRadius > 0.5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("WeaponMissRadius".Translate() + ": " + this.forcedMissRadius.ToString("F1"));
				stringBuilder.AppendLine("DirectHitChance".Translate() + ": " + (1f / (float)GenRadial.NumCellsInRadius(this.forcedMissRadius)).ToStringPercent());
			}
			else
			{
				stringBuilder.AppendLine(this.TotalEstimatedHitChance.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportShooterAbility".Translate() + ": " + this.factorFromShooterAndDist.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportWeapon".Translate() + ": " + this.factorFromEquipment.ToStringPercent());
				if (this.target.HasThing && this.factorFromTargetSize != 1f)
				{
					stringBuilder.AppendLine("   " + "TargetSize".Translate() + ": " + this.factorFromTargetSize.ToStringPercent());
				}
				if (this.factorFromWeather < 0.99f)
				{
					stringBuilder.AppendLine("   " + "Weather".Translate() + ": " + this.factorFromWeather.ToStringPercent());
				}
				if (this.factorFromCoveringGas < 0.99f)
				{
					stringBuilder.AppendLine("   " + "BlindSmoke".Translate().CapitalizeFirst() + ": " + this.factorFromCoveringGas.ToStringPercent());
				}
				if (this.FactorFromPosture < 0.9999f)
				{
					stringBuilder.AppendLine("   " + "TargetProne".Translate() + ": " + this.FactorFromPosture.ToStringPercent());
				}
				if (this.FactorFromExecution != 1f)
				{
					stringBuilder.AppendLine("   " + "Execution".Translate() + ": " + this.FactorFromExecution.ToStringPercent());
				}
				if (ModsConfig.IdeologyActive && this.target.HasThing && this.offsetFromDarkness != 0f)
				{
					if (DarknessCombatUtility.IsOutdoorsAndLit(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyOutdoorsLitOffset.LabelCap + ": " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsOutdoorsAndDark(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyOutdoorsDarkOffset.LabelCap + ": " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsIndoorsAndDark(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyIndoorsDarkOffset.LabelCap + ": " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsIndoorsAndLit(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyIndoorsLitOffset.LabelCap + "   " + this.offsetFromDarkness.ToStringPercent());
					}
				}
				if (this.PassCoverChance < 1f)
				{
					stringBuilder.AppendLine("   " + "ShootingCover".Translate() + ": " + this.PassCoverChance.ToStringPercent());
					for (int i = 0; i < this.covers.Count; i++)
					{
						CoverInfo coverInfo = this.covers[i];
						if (coverInfo.BlockChance > 0f)
						{
							stringBuilder.AppendLine("     " + "CoverThingBlocksPercentOfShots".Translate(coverInfo.Thing.LabelCap, coverInfo.BlockChance.ToStringPercent(), new NamedArgument(coverInfo.Thing.def, "COVER")).CapitalizeFirst());
						}
					}
				}
				else
				{
					stringBuilder.AppendLine("   (" + "NoCoverLower".Translate() + ")");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x00171AC4 File Offset: 0x0016FCC4
		public Thing GetRandomCoverToMissInto()
		{
			CoverInfo coverInfo;
			if (this.covers.TryRandomElementByWeight((CoverInfo c) => c.BlockChance, out coverInfo))
			{
				return coverInfo.Thing;
			}
			return null;
		}

		// Token: 0x0400263B RID: 9787
		private TargetInfo target;

		// Token: 0x0400263C RID: 9788
		private float distance;

		// Token: 0x0400263D RID: 9789
		private List<CoverInfo> covers;

		// Token: 0x0400263E RID: 9790
		private float coversOverallBlockChance;

		// Token: 0x0400263F RID: 9791
		private float factorFromShooterAndDist;

		// Token: 0x04002640 RID: 9792
		private float factorFromEquipment;

		// Token: 0x04002641 RID: 9793
		private float factorFromTargetSize;

		// Token: 0x04002642 RID: 9794
		private float factorFromWeather;

		// Token: 0x04002643 RID: 9795
		private float forcedMissRadius;

		// Token: 0x04002644 RID: 9796
		private float offsetFromDarkness;

		// Token: 0x04002645 RID: 9797
		private float factorFromCoveringGas;

		// Token: 0x04002646 RID: 9798
		private ShootLine shootLine;
	}
}
