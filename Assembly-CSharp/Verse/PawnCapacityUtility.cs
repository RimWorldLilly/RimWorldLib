using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D0 RID: 1232
	public static class PawnCapacityUtility
	{
		// Token: 0x0600225C RID: 8796 RVA: 0x000CBC2C File Offset: 0x000C9E2C
		public static bool BodyCanEverDoCapacity(BodyDef bodyDef, PawnCapacityDef capacity)
		{
			return capacity.Worker.CanHaveCapacity(bodyDef);
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x000CBC3C File Offset: 0x000C9E3C
		public static float CalculateCapacityLevel(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors = null, bool forTradePrice = false)
		{
			if (capacity.zeroIfCannotBeAwake && !diffSet.pawn.health.capacities.CanBeAwake)
			{
				if (impactors != null)
				{
					impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
					{
						capacity = PawnCapacityDefOf.Consciousness
					});
				}
				return 0f;
			}
			float num = capacity.Worker.CalculateCapacityLevel(diffSet, impactors);
			if (num > 0f)
			{
				float num2 = 99999f;
				float num3 = 1f;
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff hediff = diffSet.hediffs[i];
					if (!forTradePrice || hediff.def.priceImpact)
					{
						List<PawnCapacityModifier> capMods = hediff.CapMods;
						if (capMods != null)
						{
							for (int j = 0; j < capMods.Count; j++)
							{
								PawnCapacityModifier pawnCapacityModifier = capMods[j];
								if (pawnCapacityModifier.capacity == capacity)
								{
									num += pawnCapacityModifier.offset;
									float num4 = pawnCapacityModifier.postFactor;
									if (hediff.CurStage != null && hediff.CurStage.capacityFactorEffectMultiplier != null)
									{
										num4 = StatWorker.ScaleFactor(num4, hediff.pawn.GetStatValue(hediff.CurStage.capacityFactorEffectMultiplier, true, -1));
									}
									num3 *= num4;
									float num5 = pawnCapacityModifier.EvaluateSetMax(diffSet.pawn);
									if (num5 < num2)
									{
										num2 = num5;
									}
									if (impactors != null)
									{
										impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
										{
											hediff = hediff
										});
									}
								}
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && diffSet.pawn.genes != null)
				{
					for (int k = 0; k < diffSet.pawn.genes.GenesListForReading.Count; k++)
					{
						Gene gene = diffSet.pawn.genes.GenesListForReading[k];
						if (gene.Active)
						{
							List<PawnCapacityModifier> capMods2 = gene.def.capMods;
							if (!capMods2.NullOrEmpty<PawnCapacityModifier>())
							{
								for (int l = 0; l < capMods2.Count; l++)
								{
									PawnCapacityModifier pawnCapacityModifier2 = capMods2[l];
									if (pawnCapacityModifier2.capacity == capacity)
									{
										num += pawnCapacityModifier2.offset;
										num3 *= pawnCapacityModifier2.postFactor;
										float num6 = pawnCapacityModifier2.EvaluateSetMax(diffSet.pawn);
										if (num6 < num2)
										{
											num2 = num6;
										}
										if (impactors != null)
										{
											impactors.Add(new PawnCapacityUtility.CapacityImpactorGene
											{
												gene = gene
											});
										}
									}
								}
							}
						}
					}
				}
				num *= num3;
				num = Mathf.Min(num, num2);
			}
			num = Mathf.Max(num, capacity.minValue);
			return GenMath.RoundedHundredth(num);
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x000CBEB8 File Offset: 0x000CA0B8
		public static float CalculatePartEfficiency(HediffSet diffSet, BodyPartRecord part, bool ignoreAddedParts = false, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			for (BodyPartRecord parent = part.parent; parent != null; parent = parent.parent)
			{
				if (diffSet.HasDirectlyAddedPartFor(parent))
				{
					Hediff_AddedPart firstHediffMatchingPart = diffSet.GetFirstHediffMatchingPart<Hediff_AddedPart>(parent);
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
						{
							hediff = firstHediffMatchingPart
						});
					}
					return firstHediffMatchingPart.def.addedPartProps.partEfficiency;
				}
			}
			if (part.parent != null && diffSet.PartIsMissing(part.parent))
			{
				return 0f;
			}
			float num = 1f;
			if (!ignoreAddedParts)
			{
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff_AddedPart hediff_AddedPart = diffSet.hediffs[i] as Hediff_AddedPart;
					if (hediff_AddedPart != null && hediff_AddedPart.Part == part)
					{
						num *= hediff_AddedPart.def.addedPartProps.partEfficiency;
						if (hediff_AddedPart.def.addedPartProps.partEfficiency != 1f && impactors != null)
						{
							impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
							{
								hediff = hediff_AddedPart
							});
						}
					}
				}
			}
			float b = -1f;
			float num2 = 0f;
			bool flag = false;
			for (int j = 0; j < diffSet.hediffs.Count; j++)
			{
				if (diffSet.hediffs[j].Part == part && diffSet.hediffs[j].CurStage != null && diffSet.hediffs[j].CurStage != null)
				{
					HediffStage curStage = diffSet.hediffs[j].CurStage;
					num2 += curStage.partEfficiencyOffset;
					flag |= curStage.partIgnoreMissingHP;
					if (curStage.partEfficiencyOffset != 0f && curStage.becomeVisible && impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
						{
							hediff = diffSet.hediffs[j]
						});
					}
				}
			}
			if (!flag)
			{
				float num3 = diffSet.GetPartHealth(part) / part.def.GetMaxHealth(diffSet.pawn);
				if (num3 != 1f)
				{
					if (DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(part))
					{
						num3 = Mathf.InverseLerp(0.1f, 1f, num3);
					}
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorBodyPartHealth
						{
							bodyPart = part
						});
					}
					num *= num3;
				}
			}
			num += num2;
			if (num > 0.0001f)
			{
				num = Mathf.Max(num, b);
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x000CC103 File Offset: 0x000CA303
		public static float CalculateImmediatePartEfficiencyAndRecord(HediffSet diffSet, BodyPartRecord part, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			if (diffSet.AncestorHasDirectlyAddedParts(part))
			{
				return 1f;
			}
			return PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, impactors);
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x000CC120 File Offset: 0x000CA320
		public static float CalculateNaturalPartsAverageEfficiency(HediffSet diffSet, BodyPartGroupDef bodyPartGroup)
		{
			float num = 0f;
			int num2 = 0;
			foreach (BodyPartRecord part in from x in diffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.groups.Contains(bodyPartGroup)
			select x)
			{
				if (!diffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
				{
					num += PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, null);
				}
				num2++;
			}
			if (num2 == 0 || num < 0f)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000CC1C4 File Offset: 0x000CA3C4
		public static float CalculateTagEfficiency(HediffSet diffSet, BodyPartTagDef tag, float maximum = 3.4028235E+38f, FloatRange lerp = default(FloatRange), List<PawnCapacityUtility.CapacityImpactor> impactors = null, float bestPartEfficiencySpecialWeight = -1f)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			float num3 = 0f;
			List<PawnCapacityUtility.CapacityImpactor> list = null;
			foreach (BodyPartRecord part in body.GetPartsWithTag(tag))
			{
				float num4 = PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, list);
				if (impactors != null && num4 != 1f && list == null)
				{
					list = new List<PawnCapacityUtility.CapacityImpactor>();
					PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, list);
				}
				num += num4;
				num3 = Mathf.Max(num3, num4);
				num2++;
			}
			if (num2 == 0)
			{
				return 1f;
			}
			float num5;
			if (bestPartEfficiencySpecialWeight >= 0f && num2 >= 2)
			{
				num5 = num3 * bestPartEfficiencySpecialWeight + (num - num3) / (float)(num2 - 1) * (1f - bestPartEfficiencySpecialWeight);
			}
			else
			{
				num5 = num / (float)num2;
			}
			float num6 = num5;
			if (lerp != default(FloatRange))
			{
				num6 = lerp.LerpThroughRange(num6);
			}
			num6 = Mathf.Min(num6, maximum);
			if (impactors != null && list != null && (maximum != 1f || num5 <= 1f || num6 == 1f))
			{
				impactors.AddRange(list);
			}
			return num6;
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000CC300 File Offset: 0x000CA500
		public static float CalculateLimbEfficiency(HediffSet diffSet, BodyPartTagDef limbCoreTag, BodyPartTagDef limbSegmentTag, BodyPartTagDef limbDigitTag, float appendageWeight, out float functionalPercentage, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			Func<BodyPartRecord, float> <>9__0;
			foreach (BodyPartRecord bodyPartRecord in body.GetPartsWithTag(limbCoreTag))
			{
				float num4 = PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, bodyPartRecord, impactors);
				foreach (BodyPartRecord part in bodyPartRecord.GetConnectedParts(limbSegmentTag))
				{
					num4 *= PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, part, impactors);
				}
				if (bodyPartRecord.HasChildParts(limbDigitTag))
				{
					float a = num4;
					float num5 = num4;
					IEnumerable<BodyPartRecord> childParts = bodyPartRecord.GetChildParts(limbDigitTag);
					Func<BodyPartRecord, float> selector;
					if ((selector = <>9__0) == null)
					{
						selector = (<>9__0 = ((BodyPartRecord digitPart) => PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, digitPart, impactors)));
					}
					num4 = Mathf.Lerp(a, num5 * childParts.Average(selector), appendageWeight);
				}
				num += num4;
				num2++;
				if (num4 > 0f)
				{
					num3++;
				}
			}
			if (num2 == 0)
			{
				functionalPercentage = 0f;
				return 0f;
			}
			functionalPercentage = (float)num3 / (float)num2;
			return num / (float)num2;
		}

		// Token: 0x02002703 RID: 9987
		public abstract class CapacityImpactor
		{
			// Token: 0x17002B33 RID: 11059
			// (get) Token: 0x0600F64B RID: 63051 RVA: 0x00599B3F File Offset: 0x00597D3F
			public virtual bool IsDirect
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600F64C RID: 63052
			public abstract string Readable(Pawn pawn);
		}

		// Token: 0x02002704 RID: 9988
		public class CapacityImpactorBodyPartHealth : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x0600F64E RID: 63054 RVA: 0x00599B4C File Offset: 0x00597D4C
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1} / {2}", this.bodyPart.LabelCap, pawn.health.hediffSet.GetPartHealth(this.bodyPart), this.bodyPart.def.GetMaxHealth(pawn));
			}

			// Token: 0x04009FDD RID: 40925
			public BodyPartRecord bodyPart;
		}

		// Token: 0x02002705 RID: 9989
		public class CapacityImpactorCapacity : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x17002B34 RID: 11060
			// (get) Token: 0x0600F650 RID: 63056 RVA: 0x00599BA7 File Offset: 0x00597DA7
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600F651 RID: 63057 RVA: 0x00599BAC File Offset: 0x00597DAC
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", this.capacity.GetLabelFor(pawn).CapitalizeFirst(), (pawn.health.capacities.GetLevel(this.capacity) * 100f).ToString("F0"));
			}

			// Token: 0x04009FDE RID: 40926
			public PawnCapacityDef capacity;
		}

		// Token: 0x02002706 RID: 9990
		public class CapacityImpactorHediff : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x0600F653 RID: 63059 RVA: 0x00599C05 File Offset: 0x00597E05
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}", this.hediff.LabelCap);
			}

			// Token: 0x04009FDF RID: 40927
			public Hediff hediff;
		}

		// Token: 0x02002707 RID: 9991
		public class CapacityImpactorGene : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x0600F655 RID: 63061 RVA: 0x00599C24 File Offset: 0x00597E24
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}", this.gene.LabelCap);
			}

			// Token: 0x04009FE0 RID: 40928
			public Gene gene;
		}

		// Token: 0x02002708 RID: 9992
		public class CapacityImpactorPain : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x17002B35 RID: 11061
			// (get) Token: 0x0600F657 RID: 63063 RVA: 0x00599C43 File Offset: 0x00597E43
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600F658 RID: 63064 RVA: 0x00599C48 File Offset: 0x00597E48
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", "Pain".Translate(), (pawn.health.hediffSet.PainTotal * 100f).ToString("F0"));
			}
		}
	}
}
