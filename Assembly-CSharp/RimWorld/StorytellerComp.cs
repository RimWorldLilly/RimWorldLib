using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C5 RID: 4549
	public abstract class StorytellerComp
	{
		// Token: 0x060071AA RID: 29098 RVA: 0x0027D9BB File Offset: 0x0027BBBB
		public virtual IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			return Enumerable.Empty<FiringIncident>();
		}

		// Token: 0x060071AB RID: 29099 RVA: 0x0027D9C2 File Offset: 0x0027BBC2
		public virtual void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
		}

		// Token: 0x060071AC RID: 29100 RVA: 0x0027D9C4 File Offset: 0x0027BBC4
		public virtual void Notify_DissolutionEvent(Thing thing)
		{
		}

		// Token: 0x060071AD RID: 29101 RVA: 0x0027D9C6 File Offset: 0x0027BBC6
		public virtual IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			return StorytellerUtility.DefaultParmsNow(incCat, target);
		}

		// Token: 0x060071AE RID: 29102 RVA: 0x0027D9D0 File Offset: 0x0027BBD0
		protected IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, IncidentParms parms)
		{
			IncidentParms parmsCopy = parms.ShallowCopy();
			return this.UsableIncidentsInCategory(cat, (IncidentDef x) => parmsCopy);
		}

		// Token: 0x060071AF RID: 29103 RVA: 0x0027DA04 File Offset: 0x0027BC04
		protected virtual IEnumerable<IncidentDef> UsableIncidentsInCategory(IncidentCategoryDef cat, Func<IncidentDef, IncidentParms> parmsGetter)
		{
			StorytellerComp.<>c__DisplayClass7_0 CS$<>8__locals1 = new StorytellerComp.<>c__DisplayClass7_0();
			CS$<>8__locals1.cat = cat;
			CS$<>8__locals1.parmsGetter = parmsGetter;
			if (!CS$<>8__locals1.cat.canUseAnomalyChance)
			{
				return DefDatabase<IncidentDef>.AllDefsListForReading.Where(new Func<IncidentDef, bool>(CS$<>8__locals1.<UsableIncidentsInCategory>g__CanUseIncident|0));
			}
			if (!ModsConfig.AnomalyActive || !Rand.Chance(Find.Storyteller.AnomalyIncidentChanceNow))
			{
				return Storyteller.NonAnomalyIncidents.Where(new Func<IncidentDef, bool>(CS$<>8__locals1.<UsableIncidentsInCategory>g__CanUseIncident|0));
			}
			return Storyteller.AnomalyIncidents.Where(new Func<IncidentDef, bool>(CS$<>8__locals1.<UsableIncidentsInCategory>g__CanUseIncident|0));
		}

		// Token: 0x060071B0 RID: 29104 RVA: 0x0027DA94 File Offset: 0x0027BC94
		protected float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
		{
			if (def.chanceFactorByPopulationCurve == null)
			{
				return 1f;
			}
			int num = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists.Count<Pawn>();
			return def.chanceFactorByPopulationCurve.Evaluate((float)num);
		}

		// Token: 0x060071B1 RID: 29105 RVA: 0x0027DAC8 File Offset: 0x0027BCC8
		protected float IncidentChanceFactor_PopulationIntent(IncidentDef def)
		{
			if (def.populationEffect == IncidentPopulationEffect.None)
			{
				return 1f;
			}
			float num;
			switch (def.populationEffect)
			{
			case IncidentPopulationEffect.IncreaseHard:
				num = 0.4f;
				break;
			case IncidentPopulationEffect.IncreaseMedium:
				num = 0f;
				break;
			case IncidentPopulationEffect.IncreaseEasy:
				num = -0.4f;
				break;
			default:
				throw new Exception();
			}
			return Mathf.Max(StorytellerUtilityPopulation.PopulationIntent + num, this.props.minIncChancePopulationIntentFactor);
		}

		// Token: 0x060071B2 RID: 29106 RVA: 0x0027DB38 File Offset: 0x0027BD38
		protected bool TrySelectRandomIncident(IEnumerable<IncidentDef> incidents, out IncidentDef foundDef, IIncidentTarget target)
		{
			bool flag = Rand.Chance(StorytellerComp.IncreasesPopChanceByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent));
			foundDef = null;
			if (flag)
			{
				foundDef = (from i in incidents
				where i.populationEffect > IncidentPopulationEffect.None
				select i).RandomElementByWeightWithFallback((IncidentDef i) => this.IncidentChanceFinal(i, target), null);
			}
			else
			{
				foundDef = (from i in incidents
				where i.populationEffect == IncidentPopulationEffect.None
				select i).RandomElementByWeightWithFallback((IncidentDef i) => this.IncidentChanceFinal(i, target), null);
			}
			if (foundDef == null)
			{
				foundDef = incidents.RandomElementByWeightWithFallback((IncidentDef i) => this.IncidentChanceFinal(i, target), null);
			}
			return foundDef != null;
		}

		// Token: 0x060071B3 RID: 29107 RVA: 0x0027DC04 File Offset: 0x0027BE04
		protected float IncidentChanceFinal(IncidentDef def, IIncidentTarget target)
		{
			float num = def.Worker.BaseChanceThisGame;
			num *= this.IncidentChanceFactor_CurrentPopulation(def);
			num *= this.IncidentChanceFactor_PopulationIntent(def);
			num *= def.Worker.ChanceFactorNow(target);
			if (!def.ShouldIgnoreRecentWeighting)
			{
				for (int i = 0; i < target.StoryState.RecentRandomIncidents.Count; i++)
				{
					if (target.StoryState.RecentRandomIncidents[i] == def)
					{
						num *= QuestTuning.RecentStoryWeightFactors[i];
					}
				}
			}
			return Mathf.Max(0f, num);
		}

		// Token: 0x060071B4 RID: 29108 RVA: 0x0027DC8C File Offset: 0x0027BE8C
		public virtual void Initialize()
		{
		}

		// Token: 0x060071B5 RID: 29109 RVA: 0x0027DC90 File Offset: 0x0027BE90
		public override string ToString()
		{
			string text = base.GetType().Name;
			string text2 = typeof(StorytellerComp).Name + "_";
			if (text.StartsWith(text2))
			{
				text = text.Substring(text2.Length);
			}
			if (!this.props.allowedTargetTags.NullOrEmpty<IncidentTargetTagDef>())
			{
				text = text + " (" + (from x in this.props.allowedTargetTags
				select x.ToString()).ToCommaList(false, false) + ")";
			}
			return text;
		}

		// Token: 0x060071B6 RID: 29110 RVA: 0x0027DD34 File Offset: 0x0027BF34
		public virtual void DebugTablesIncidentChances()
		{
			IEnumerable<IncidentDef> dataSources = from d in DefDatabase<IncidentDef>.AllDefs
			orderby d.category.defName descending, this.IncidentChanceFinal(d, Find.CurrentMap) descending
			select d;
			TableDataGetter<IncidentDef>[] array = new TableDataGetter<IncidentDef>[21];
			array[0] = new TableDataGetter<IncidentDef>("defName", (IncidentDef d) => d.defName);
			array[1] = new TableDataGetter<IncidentDef>("category", (IncidentDef d) => d.category);
			array[2] = new TableDataGetter<IncidentDef>("can fire", (IncidentDef d) => StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|14_1(d).ToStringCheckBlank());
			array[3] = new TableDataGetter<IncidentDef>("base\nchance", (IncidentDef d) => d.baseChance.ToString("F2"));
			array[4] = new TableDataGetter<IncidentDef>("base\nchance\nwith\nRoyalty", delegate(IncidentDef d)
			{
				if (d.baseChanceWithRoyalty < 0f)
				{
					return "-";
				}
				return d.baseChanceWithRoyalty.ToString("F2");
			});
			array[5] = new TableDataGetter<IncidentDef>("base\nchance\nthis\ngame", (IncidentDef d) => d.Worker.BaseChanceThisGame.ToString("F2"));
			array[6] = new TableDataGetter<IncidentDef>("recently\noccured\nfactor", delegate(IncidentDef d)
			{
				if (!d.ShouldIgnoreRecentWeighting)
				{
					return StorytellerComp.<DebugTablesIncidentChances>g__RecentChanceFactor|14_2(d).ToString("F2");
				}
				return "-";
			});
			array[7] = new TableDataGetter<IncidentDef>("Factor from:\ncurrent pop", (IncidentDef d) => this.IncidentChanceFactor_CurrentPopulation(d).ToString());
			array[8] = new TableDataGetter<IncidentDef>("Factor from:\npop intent", (IncidentDef d) => this.IncidentChanceFactor_PopulationIntent(d).ToString());
			array[9] = new TableDataGetter<IncidentDef>("final\nchance", (IncidentDef d) => this.IncidentChanceFinal(d, Find.CurrentMap).ToString("F2"));
			array[10] = new TableDataGetter<IncidentDef>("final\nchance\npossible", delegate(IncidentDef d)
			{
				if (!StorytellerComp.<DebugTablesIncidentChances>g__CanFireLocal|14_1(d))
				{
					return "-";
				}
				return this.IncidentChanceFinal(d, Find.CurrentMap).ToString("F2");
			});
			array[11] = new TableDataGetter<IncidentDef>("default target", delegate(IncidentDef d)
			{
				if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|14_0(d) == null)
				{
					return "-";
				}
				return StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|14_0(d).ToString();
			});
			array[12] = new TableDataGetter<IncidentDef>("current\npop", (IncidentDef d) => PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists.Count<Pawn>().ToString());
			array[13] = new TableDataGetter<IncidentDef>("pop\nintent", (IncidentDef d) => StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			array[14] = new TableDataGetter<IncidentDef>("cur\npoints", delegate(IncidentDef d)
			{
				if (StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|14_0(d) == null)
				{
					return "-";
				}
				return StorytellerUtility.DefaultThreatPointsNow(StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|14_0(d)).ToString("F0");
			});
			array[15] = new TableDataGetter<IncidentDef>("DLC", delegate(IncidentDef d)
			{
				if (d.modContentPack.IsCoreMod)
				{
					return "Core";
				}
				if (!d.modContentPack.IsOfficialMod)
				{
					return "-";
				}
				return d.modContentPack.Name.Split('.', StringSplitOptions.None).Last<string>();
			});
			array[16] = new TableDataGetter<IncidentDef>("earliestDay", (IncidentDef d) => d.earliestDay);
			array[17] = new TableDataGetter<IncidentDef>("minRefireDays", (IncidentDef d) => d.minRefireDays);
			array[18] = new TableDataGetter<IncidentDef>("minAnomalyThreatLevel", (IncidentDef d) => d.minAnomalyThreatLevel);
			array[19] = new TableDataGetter<IncidentDef>("minPopulation", (IncidentDef d) => d.minPopulation);
			array[20] = new TableDataGetter<IncidentDef>("min/max threatpoints", (IncidentDef d) => ((d.minThreatPoints > float.MinValue) ? d.minThreatPoints.ToString("F2") : "minUnset") + "/" + ((d.maxThreatPoints < float.MaxValue) ? d.maxThreatPoints.ToString("F2") : "maxUnset"));
			DebugTables.MakeTablesDialog<IncidentDef>(dataSources, array);
		}

		// Token: 0x060071B9 RID: 29113 RVA: 0x0027E145 File Offset: 0x0027C345
		[CompilerGenerated]
		internal static IIncidentTarget <DebugTablesIncidentChances>g__GetDefaultTarget|14_0(IncidentDef d)
		{
			if (d.TargetAllowed(Find.CurrentMap))
			{
				return Find.CurrentMap;
			}
			if (d.TargetAllowed(Find.World))
			{
				return Find.World;
			}
			return null;
		}

		// Token: 0x060071BA RID: 29114 RVA: 0x0027E170 File Offset: 0x0027C370
		[CompilerGenerated]
		internal static bool <DebugTablesIncidentChances>g__CanFireLocal|14_1(IncidentDef d)
		{
			IIncidentTarget incidentTarget = StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|14_0(d);
			if (incidentTarget == null)
			{
				return false;
			}
			IncidentParms parms = StorytellerUtility.DefaultParmsNow(d.category, incidentTarget);
			return d.Worker.CanFireNow(parms);
		}

		// Token: 0x060071BB RID: 29115 RVA: 0x0027E1A4 File Offset: 0x0027C3A4
		[CompilerGenerated]
		internal static float <DebugTablesIncidentChances>g__RecentChanceFactor|14_2(IncidentDef def)
		{
			IIncidentTarget incidentTarget = StorytellerComp.<DebugTablesIncidentChances>g__GetDefaultTarget|14_0(def);
			if (incidentTarget == null)
			{
				return 1f;
			}
			for (int i = 0; i < incidentTarget.StoryState.RecentRandomIncidents.Count; i++)
			{
				if (incidentTarget.StoryState.RecentRandomIncidents[i] == def)
				{
					return QuestTuning.RecentStoryWeightFactors[i];
				}
			}
			return 1f;
		}

		// Token: 0x040047EE RID: 18414
		public StorytellerCompProperties props;

		// Token: 0x040047EF RID: 18415
		public static readonly SimpleCurve IncreasesPopChanceByPopIntentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 0.3f),
				true
			},
			{
				new CurvePoint(3f, 0.45f),
				true
			}
		};
	}
}
