using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000179 RID: 377
	public class HediffStage
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x0003C5C8 File Offset: 0x0003A7C8
		public bool AffectsMemory
		{
			get
			{
				return this.forgetMemoryThoughtMtbDays > 0f || this.pctConditionalThoughtsNullified > 0f;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0003C5E6 File Offset: 0x0003A7E6
		public bool AffectsSocialInteractions
		{
			get
			{
				return this.opinionOfOthersFactor != 1f;
			}
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0003C5F8 File Offset: 0x0003A7F8
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0003C606 File Offset: 0x0003A806
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return HediffStatsUtility.SpecialDisplayStats(this, null);
		}

		// Token: 0x040008E5 RID: 2277
		public float minSeverity;

		// Token: 0x040008E6 RID: 2278
		[MustTranslate]
		public string label;

		// Token: 0x040008E7 RID: 2279
		[MustTranslate]
		public string overrideLabel;

		// Token: 0x040008E8 RID: 2280
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;

		// Token: 0x040008E9 RID: 2281
		public bool becomeVisible = true;

		// Token: 0x040008EA RID: 2282
		public bool lifeThreatening;

		// Token: 0x040008EB RID: 2283
		public TaleDef tale;

		// Token: 0x040008EC RID: 2284
		public float vomitMtbDays = -1f;

		// Token: 0x040008ED RID: 2285
		public float deathMtbDays = -1f;

		// Token: 0x040008EE RID: 2286
		public bool mtbDeathDestroysBrain;

		// Token: 0x040008EF RID: 2287
		public float painFactor = 1f;

		// Token: 0x040008F0 RID: 2288
		public float painOffset;

		// Token: 0x040008F1 RID: 2289
		public float totalBleedFactor = 1f;

		// Token: 0x040008F2 RID: 2290
		public float naturalHealingFactor = -1f;

		// Token: 0x040008F3 RID: 2291
		public float regeneration = -1f;

		// Token: 0x040008F4 RID: 2292
		public bool showRegenerationStat = true;

		// Token: 0x040008F5 RID: 2293
		public float forgetMemoryThoughtMtbDays = -1f;

		// Token: 0x040008F6 RID: 2294
		public float pctConditionalThoughtsNullified;

		// Token: 0x040008F7 RID: 2295
		public float pctAllThoughtNullification;

		// Token: 0x040008F8 RID: 2296
		public float opinionOfOthersFactor = 1f;

		// Token: 0x040008F9 RID: 2297
		public float fertilityFactor = 1f;

		// Token: 0x040008FA RID: 2298
		public float hungerRateFactor = 1f;

		// Token: 0x040008FB RID: 2299
		public float hungerRateFactorOffset;

		// Token: 0x040008FC RID: 2300
		public float restFallFactor = 1f;

		// Token: 0x040008FD RID: 2301
		public float restFallFactorOffset;

		// Token: 0x040008FE RID: 2302
		public float socialFightChanceFactor = 1f;

		// Token: 0x040008FF RID: 2303
		public float foodPoisoningChanceFactor = 1f;

		// Token: 0x04000900 RID: 2304
		public float mentalBreakMtbDays = -1f;

		// Token: 0x04000901 RID: 2305
		public string mentalBreakExplanation;

		// Token: 0x04000902 RID: 2306
		public bool blocksMentalBreaks;

		// Token: 0x04000903 RID: 2307
		public bool blocksInspirations;

		// Token: 0x04000904 RID: 2308
		public float overrideMoodBase = -1f;

		// Token: 0x04000905 RID: 2309
		public float severityGainFactor = 1f;

		// Token: 0x04000906 RID: 2310
		public bool removeRoamMtb;

		// Token: 0x04000907 RID: 2311
		public bool preventVacuumBurns;

		// Token: 0x04000908 RID: 2312
		public List<MentalBreakIntensity> allowedMentalBreakIntensities;

		// Token: 0x04000909 RID: 2313
		public List<HediffDef> makeImmuneTo;

		// Token: 0x0400090A RID: 2314
		public List<PawnCapacityModifier> capMods = new List<PawnCapacityModifier>();

		// Token: 0x0400090B RID: 2315
		public List<HediffGiver> hediffGivers;

		// Token: 0x0400090C RID: 2316
		public List<MentalStateGiver> mentalStateGivers;

		// Token: 0x0400090D RID: 2317
		public List<StatModifier> statOffsets;

		// Token: 0x0400090E RID: 2318
		public List<StatModifier> statFactors;

		// Token: 0x0400090F RID: 2319
		public List<StatModifierBySeverity> statOffsetsBySeverity;

		// Token: 0x04000910 RID: 2320
		public List<StatModifierBySeverity> statFactorsBySeverity;

		// Token: 0x04000911 RID: 2321
		public List<DamageFactor> damageFactors = new List<DamageFactor>();

		// Token: 0x04000912 RID: 2322
		public List<NeedDef> enablesNeeds;

		// Token: 0x04000913 RID: 2323
		public List<NeedDef> disablesNeeds;

		// Token: 0x04000914 RID: 2324
		public bool multiplyStatChangesBySeverity;

		// Token: 0x04000915 RID: 2325
		public StatDef statOffsetEffectMultiplier;

		// Token: 0x04000916 RID: 2326
		public StatDef statFactorEffectMultiplier;

		// Token: 0x04000917 RID: 2327
		public StatDef capacityFactorEffectMultiplier;

		// Token: 0x04000918 RID: 2328
		public WorkTags disabledWorkTags;

		// Token: 0x04000919 RID: 2329
		[MustTranslate]
		public string overrideTooltip;

		// Token: 0x0400091A RID: 2330
		[MustTranslate]
		public string extraTooltip;

		// Token: 0x0400091B RID: 2331
		public bool blocksSleeping;

		// Token: 0x0400091C RID: 2332
		public float partEfficiencyOffset;

		// Token: 0x0400091D RID: 2333
		public bool partIgnoreMissingHP;

		// Token: 0x0400091E RID: 2334
		public bool destroyPart;
	}
}
