using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F77 RID: 3959
	public class IncidentDef : Def
	{
		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x0600656A RID: 25962 RVA: 0x00241D2B File Offset: 0x0023FF2B
		public bool ShouldIgnoreRecentWeighting
		{
			get
			{
				return this.ignoreRecentSelectionWeighting || this.category == IncidentCategoryDefOf.Special || this.category == IncidentCategoryDefOf.GiveQuest;
			}
		}

		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x0600656B RID: 25963 RVA: 0x00241D51 File Offset: 0x0023FF51
		public IncidentWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (IncidentWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x0600656C RID: 25964 RVA: 0x00241D84 File Offset: 0x0023FF84
		public List<IncidentDef> RefireCheckIncidents
		{
			get
			{
				if (this.refireCheckTags == null)
				{
					return null;
				}
				if (this.cachedRefireCheckIncidents == null)
				{
					this.cachedRefireCheckIncidents = new List<IncidentDef>();
					List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (this.ShouldDoRefireCheckWith(allDefsListForReading[i]))
						{
							this.cachedRefireCheckIncidents.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.cachedRefireCheckIncidents;
			}
		}

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x0600656D RID: 25965 RVA: 0x00241DEC File Offset: 0x0023FFEC
		public bool IsAnomalyIncident
		{
			get
			{
				return this.minAnomalyThreatLevel >= 0;
			}
		}

		// Token: 0x0600656E RID: 25966 RVA: 0x00241DFA File Offset: 0x0023FFFA
		public static IncidentDef Named(string defName)
		{
			return DefDatabase<IncidentDef>.GetNamed(defName, true);
		}

		// Token: 0x0600656F RID: 25967 RVA: 0x00241E04 File Offset: 0x00240004
		private bool ShouldDoRefireCheckWith(IncidentDef other)
		{
			if (other.tags == null)
			{
				return false;
			}
			if (other == this)
			{
				return false;
			}
			for (int i = 0; i < other.tags.Count; i++)
			{
				for (int j = 0; j < this.refireCheckTags.Count; j++)
				{
					if (other.tags[i] == this.refireCheckTags[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006570 RID: 25968 RVA: 0x00241E6F File Offset: 0x0024006F
		public override IEnumerable<string> ConfigErrors()
		{
			IncidentDef.<ConfigErrors>d__66 <ConfigErrors>d__ = new IncidentDef.<ConfigErrors>d__66(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06006571 RID: 25969 RVA: 0x00241E7F File Offset: 0x0024007F
		public bool TargetTagAllowed(IncidentTargetTagDef target)
		{
			return this.targetTags.Contains(target);
		}

		// Token: 0x06006572 RID: 25970 RVA: 0x00241E8D File Offset: 0x0024008D
		public bool TargetAllowed(IIncidentTarget target)
		{
			return this.targetTags.Intersect(target.IncidentTargetTags()).Any<IncidentTargetTagDef>();
		}

		// Token: 0x04003BD0 RID: 15312
		public Type workerClass;

		// Token: 0x04003BD1 RID: 15313
		public IncidentCategoryDef category;

		// Token: 0x04003BD2 RID: 15314
		public List<IncidentTargetTagDef> targetTags;

		// Token: 0x04003BD3 RID: 15315
		public float baseChance;

		// Token: 0x04003BD4 RID: 15316
		public float baseChanceWithRoyalty = -1f;

		// Token: 0x04003BD5 RID: 15317
		public IncidentPopulationEffect populationEffect;

		// Token: 0x04003BD6 RID: 15318
		public int earliestDay;

		// Token: 0x04003BD7 RID: 15319
		public int minPopulation;

		// Token: 0x04003BD8 RID: 15320
		public bool requireColonistsPresent;

		// Token: 0x04003BD9 RID: 15321
		public float minRefireDays;

		// Token: 0x04003BDA RID: 15322
		[Obsolete]
		public int minDifficulty;

		// Token: 0x04003BDB RID: 15323
		public DifficultyConditionConfig disabledWhen = new DifficultyConditionConfig();

		// Token: 0x04003BDC RID: 15324
		public bool pointsScaleable;

		// Token: 0x04003BDD RID: 15325
		public float minThreatPoints = float.MinValue;

		// Token: 0x04003BDE RID: 15326
		public float maxThreatPoints = float.MaxValue;

		// Token: 0x04003BDF RID: 15327
		public List<BiomeDef> allowedBiomes;

		// Token: 0x04003BE0 RID: 15328
		public List<BiomeDef> disallowedBiomes;

		// Token: 0x04003BE1 RID: 15329
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04003BE2 RID: 15330
		[NoTranslate]
		public List<string> refireCheckTags;

		// Token: 0x04003BE3 RID: 15331
		public SimpleCurve chanceFactorByPopulationCurve;

		// Token: 0x04003BE4 RID: 15332
		public TaleDef tale;

		// Token: 0x04003BE5 RID: 15333
		public int minAnomalyThreatLevel = -1;

		// Token: 0x04003BE6 RID: 15334
		public int minGreatestPopulation = -1;

		// Token: 0x04003BE7 RID: 15335
		private bool ignoreRecentSelectionWeighting;

		// Token: 0x04003BE8 RID: 15336
		public bool hidden;

		// Token: 0x04003BE9 RID: 15337
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04003BEA RID: 15338
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04003BEB RID: 15339
		public bool canOccurOnAllPlanetLayers;

		// Token: 0x04003BEC RID: 15340
		public EntityCodexEntryDef codexEntry;

		// Token: 0x04003BED RID: 15341
		[MustTranslate]
		public string letterText;

		// Token: 0x04003BEE RID: 15342
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04003BEF RID: 15343
		public LetterDef letterDef;

		// Token: 0x04003BF0 RID: 15344
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x04003BF1 RID: 15345
		public bool letterSingularForm;

		// Token: 0x04003BF2 RID: 15346
		public PawnKindDef pawnKind;

		// Token: 0x04003BF3 RID: 15347
		public bool pawnMustBeCapableOfViolence;

		// Token: 0x04003BF4 RID: 15348
		public Gender pawnFixedGender;

		// Token: 0x04003BF5 RID: 15349
		public HediffDef pawnHediff;

		// Token: 0x04003BF6 RID: 15350
		public GameConditionDef gameCondition;

		// Token: 0x04003BF7 RID: 15351
		public FloatRange durationDays;

		// Token: 0x04003BF8 RID: 15352
		public HediffDef diseaseIncident;

		// Token: 0x04003BF9 RID: 15353
		public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

		// Token: 0x04003BFA RID: 15354
		public int diseaseMaxVictims = 99999;

		// Token: 0x04003BFB RID: 15355
		public List<BiomeDiseaseRecord> diseaseBiomeRecords;

		// Token: 0x04003BFC RID: 15356
		public List<BodyPartDef> diseasePartsToAffect;

		// Token: 0x04003BFD RID: 15357
		[MustTranslate]
		public string diseaseLethalLetterText;

		// Token: 0x04003BFE RID: 15358
		public DevelopmentalStage diseaseDevelopmentStage = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04003BFF RID: 15359
		public ThingDef mechClusterBuilding;

		// Token: 0x04003C00 RID: 15360
		public List<MTBByBiome> mtbDaysByBiome;

		// Token: 0x04003C01 RID: 15361
		public QuestScriptDef questScriptDef;

		// Token: 0x04003C02 RID: 15362
		public ThingDef treeDef;

		// Token: 0x04003C03 RID: 15363
		public GenStepDef treeGenStepDef;

		// Token: 0x04003C04 RID: 15364
		public string letterTextPlural;

		// Token: 0x04003C05 RID: 15365
		public float treeGrowth;

		// Token: 0x04003C06 RID: 15366
		[Unsaved(false)]
		private IncidentWorker workerInt;

		// Token: 0x04003C07 RID: 15367
		[Unsaved(false)]
		private List<IncidentDef> cachedRefireCheckIncidents;
	}
}
