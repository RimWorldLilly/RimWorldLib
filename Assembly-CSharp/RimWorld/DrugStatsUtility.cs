using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC9 RID: 3785
	public static class DrugStatsUtility
	{
		// Token: 0x0600636C RID: 25452 RVA: 0x0023B4C5 File Offset: 0x002396C5
		public static CompProperties_Drug GetDrugComp(ThingDef d)
		{
			return d.GetCompProperties<CompProperties_Drug>();
		}

		// Token: 0x0600636D RID: 25453 RVA: 0x0023B4CD File Offset: 0x002396CD
		public static ChemicalDef GetChemical(ThingDef d)
		{
			CompProperties_Drug drugComp = DrugStatsUtility.GetDrugComp(d);
			if (drugComp == null)
			{
				return null;
			}
			return drugComp.chemical;
		}

		// Token: 0x0600636E RID: 25454 RVA: 0x0023B4E0 File Offset: 0x002396E0
		public static NeedDef GetNeed(ThingDef d)
		{
			ChemicalDef chemical = DrugStatsUtility.GetChemical(d);
			if (chemical == null)
			{
				return null;
			}
			HediffDef addictionHediff = chemical.addictionHediff;
			if (addictionHediff == null)
			{
				return null;
			}
			return addictionHediff.chemicalNeed;
		}

		// Token: 0x0600636F RID: 25455 RVA: 0x0023B4FE File Offset: 0x002396FE
		public static HediffDef GetTolerance(ThingDef d)
		{
			ChemicalDef chemical = DrugStatsUtility.GetChemical(d);
			if (chemical == null)
			{
				return null;
			}
			return chemical.toleranceHediff;
		}

		// Token: 0x06006370 RID: 25456 RVA: 0x0023B514 File Offset: 0x00239714
		public static float GetAddictivenessAtTolerance(ThingDef d, float tolerance)
		{
			float num = DrugStatsUtility.GetDrugComp(d).addictiveness;
			if (DrugStatsUtility.GetTolerance(d) != null && tolerance > 0f)
			{
				num *= DrugStatsUtility.ToleranceToAddictivenessFactorCurve.Evaluate(tolerance);
			}
			return num;
		}

		// Token: 0x06006371 RID: 25457 RVA: 0x0023B54C File Offset: 0x0023974C
		public static IngestionOutcomeDoer_GiveHediff GetDrugHighGiver(ThingDef d)
		{
			if (d.ingestible == null || d.ingestible.outcomeDoers == null)
			{
				return null;
			}
			foreach (IngestionOutcomeDoer ingestionOutcomeDoer in d.ingestible.outcomeDoers)
			{
				IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff = ingestionOutcomeDoer as IngestionOutcomeDoer_GiveHediff;
				if (ingestionOutcomeDoer_GiveHediff != null && typeof(Hediff_High).IsAssignableFrom(ingestionOutcomeDoer_GiveHediff.hediffDef.hediffClass))
				{
					return ingestionOutcomeDoer_GiveHediff;
				}
			}
			return null;
		}

		// Token: 0x06006372 RID: 25458 RVA: 0x0023B5E0 File Offset: 0x002397E0
		public static IngestionOutcomeDoer_GiveHediff GetToleranceGiver(ThingDef d)
		{
			if (d.ingestible == null || d.ingestible.outcomeDoers == null)
			{
				return null;
			}
			foreach (IngestionOutcomeDoer ingestionOutcomeDoer in d.ingestible.outcomeDoers)
			{
				IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff = ingestionOutcomeDoer as IngestionOutcomeDoer_GiveHediff;
				if (ingestionOutcomeDoer_GiveHediff != null && ingestionOutcomeDoer_GiveHediff.hediffDef == DrugStatsUtility.GetTolerance(d))
				{
					return ingestionOutcomeDoer_GiveHediff;
				}
			}
			return null;
		}

		// Token: 0x06006373 RID: 25459 RVA: 0x0023B668 File Offset: 0x00239868
		public static float GetHighOffsetPerDay(ThingDef d)
		{
			IngestionOutcomeDoer_GiveHediff drugHighGiver = DrugStatsUtility.GetDrugHighGiver(d);
			if (drugHighGiver == null)
			{
				return 0f;
			}
			HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = drugHighGiver.hediffDef.CompProps<HediffCompProperties_SeverityPerDay>();
			if (hediffCompProperties_SeverityPerDay == null)
			{
				return 0f;
			}
			return hediffCompProperties_SeverityPerDay.severityPerDay;
		}

		// Token: 0x06006374 RID: 25460 RVA: 0x0023B6A0 File Offset: 0x002398A0
		public static float GetToleranceGain(ThingDef d)
		{
			if (d.ingestible == null || d.ingestible.outcomeDoers == null)
			{
				return 0f;
			}
			HediffDef tolerance = DrugStatsUtility.GetTolerance(d);
			if (tolerance != null)
			{
				foreach (IngestionOutcomeDoer ingestionOutcomeDoer in d.ingestible.outcomeDoers)
				{
					IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff = ingestionOutcomeDoer as IngestionOutcomeDoer_GiveHediff;
					if (ingestionOutcomeDoer_GiveHediff != null && ingestionOutcomeDoer_GiveHediff.hediffDef == tolerance)
					{
						return ingestionOutcomeDoer_GiveHediff.severity;
					}
				}
			}
			return 0f;
		}

		// Token: 0x06006375 RID: 25461 RVA: 0x0023B738 File Offset: 0x00239938
		public static float GetToleranceOffsetPerDay(ThingDef d)
		{
			HediffDef tolerance = DrugStatsUtility.GetTolerance(d);
			if (tolerance == null)
			{
				return 0f;
			}
			HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = tolerance.CompProps<HediffCompProperties_SeverityPerDay>();
			if (hediffCompProperties_SeverityPerDay == null)
			{
				return 0f;
			}
			return hediffCompProperties_SeverityPerDay.severityPerDay;
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x0023B76C File Offset: 0x0023996C
		public static float GetAddictionOffsetPerDay(ThingDef d)
		{
			ChemicalDef chemical = DrugStatsUtility.GetChemical(d);
			HediffDef hediffDef = (chemical != null) ? chemical.addictionHediff : null;
			if (hediffDef == null)
			{
				return 0f;
			}
			HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = hediffDef.CompProps<HediffCompProperties_SeverityPerDay>();
			if (hediffCompProperties_SeverityPerDay == null)
			{
				return 0f;
			}
			return hediffCompProperties_SeverityPerDay.severityPerDay;
		}

		// Token: 0x06006377 RID: 25463 RVA: 0x0023B7AC File Offset: 0x002399AC
		public static float GetAddictionNeedCostPerDay(ThingDef d)
		{
			NeedDef need = DrugStatsUtility.GetNeed(d);
			if (need != null)
			{
				return d.BaseMarketValue * need.fallPerDay * (1f + (1f - DrugStatsUtility.GetDrugComp(d).needLevelOffset));
			}
			return 0f;
		}

		// Token: 0x06006378 RID: 25464 RVA: 0x0023B7F0 File Offset: 0x002399F0
		public static float GetSafeDoseInterval(ThingDef d, float bodySizeFactor = 1f)
		{
			CompProperties_Drug drugComp = DrugStatsUtility.GetDrugComp(d);
			if (drugComp == null || !drugComp.Addictive)
			{
				return 0f;
			}
			if (drugComp.addictiveness >= 1f || DrugStatsUtility.GetToleranceGiver(d) == null)
			{
				return -1f;
			}
			if (bodySizeFactor == 0f)
			{
				return -1f;
			}
			float num = Mathf.Abs(DrugStatsUtility.GetToleranceOffsetPerDay(d));
			return Mathf.Max(drugComp.overdoseSeverityOffset.TrueMax, (num > 0f) ? (DrugStatsUtility.GetToleranceGiver(d).severity / num) : -1f) / bodySizeFactor;
		}

		// Token: 0x06006379 RID: 25465 RVA: 0x0023B878 File Offset: 0x00239A78
		public static string GetSafeDoseIntervalReadout(ThingDef d)
		{
			IngestionOutcomeDoer_GiveHediff toleranceGiver = DrugStatsUtility.GetToleranceGiver(d);
			float safeDoseInterval = DrugStatsUtility.GetSafeDoseInterval(d, 1f);
			float num = (toleranceGiver != null) ? (DrugStatsUtility.GetDrugComp(d).minToleranceToAddict / toleranceGiver.severity) : 0f;
			string result;
			if (safeDoseInterval == 0f)
			{
				result = "AlwaysSafe".Translate();
			}
			else if (num < 1f)
			{
				result = "NeverSafe".Translate();
			}
			else
			{
				result = "PeriodDays".Translate(safeDoseInterval.ToString("F1"));
			}
			return result;
		}

		// Token: 0x0600637A RID: 25466 RVA: 0x0023B90B File Offset: 0x00239B0B
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef def)
		{
			DrugStatsUtility.<SpecialDisplayStats>d__15 <SpecialDisplayStats>d__ = new DrugStatsUtility.<SpecialDisplayStats>d__15(-2);
			<SpecialDisplayStats>d__.<>3__def = def;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x040037A4 RID: 14244
		public static readonly SimpleCurve ToleranceToAddictivenessFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 5f),
				true
			},
			{
				new CurvePoint(0.8f, 15f),
				true
			}
		};
	}
}
