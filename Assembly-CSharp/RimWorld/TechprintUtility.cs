using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8C RID: 7052
	public static class TechprintUtility
	{
		// Token: 0x0600B4E6 RID: 46310 RVA: 0x003FBA08 File Offset: 0x003F9C08
		public static IEnumerable<ResearchProjectDef> GetResearchProjectsNeedingTechprintsNow(Faction faction, List<ThingDef> alreadyGeneratedTechprints = null, float maxMarketValue = 3.4028235E+38f)
		{
			return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Where(delegate(ResearchProjectDef p)
			{
				if (p.TechprintCount == 0)
				{
					return false;
				}
				if (p.IsFinished || p.TechprintRequirementMet)
				{
					return false;
				}
				if (faction != null && (p.heldByFactionCategoryTags == null || !p.heldByFactionCategoryTags.Contains(faction.def.categoryTag)))
				{
					return false;
				}
				if (maxMarketValue != 3.4028235E+38f && p.Techprint.BaseMarketValue > maxMarketValue)
				{
					return false;
				}
				if (alreadyGeneratedTechprints != null)
				{
					CompProperties_Techprint compProperties = p.Techprint.GetCompProperties<CompProperties_Techprint>();
					if (compProperties != null)
					{
						int num = compProperties.project.TechprintCount - compProperties.project.TechprintsApplied;
						int num2 = 0;
						for (int i = 0; i < alreadyGeneratedTechprints.Count; i++)
						{
							if (alreadyGeneratedTechprints[i] == p.Techprint)
							{
								num2++;
							}
						}
						if (num2 >= num)
						{
							return false;
						}
					}
				}
				return true;
			});
		}

		// Token: 0x0600B4E7 RID: 46311 RVA: 0x003FBA46 File Offset: 0x003F9C46
		[Obsolete]
		public static float GetSelectionWeight(ResearchProjectDef project)
		{
			return TechprintUtility.GetSelectionWeight_NewTemp(project, true);
		}

		// Token: 0x0600B4E8 RID: 46312 RVA: 0x003FBA4F File Offset: 0x003F9C4F
		public static float GetSelectionWeight_NewTemp(ResearchProjectDef project, bool weightForPlayerNeed = true)
		{
			if (!weightForPlayerNeed)
			{
				return project.techprintCommonality;
			}
			return project.techprintCommonality * (project.PrerequisitesCompleted ? 1f : 0.02f);
		}

		// Token: 0x0600B4E9 RID: 46313 RVA: 0x003FBA76 File Offset: 0x003F9C76
		[Obsolete]
		public static bool TryGetTechprintDefToGenerate(Faction faction, out ThingDef result, List<ThingDef> alreadyGeneratedTechprints = null, float maxMarketValue = 3.4028235E+38f)
		{
			return TechprintUtility.TryGetTechprintDefToGenerate_NewTemp(faction, out result, alreadyGeneratedTechprints, maxMarketValue, true);
		}

		// Token: 0x0600B4EA RID: 46314 RVA: 0x003FBA84 File Offset: 0x003F9C84
		public static bool TryGetTechprintDefToGenerate_NewTemp(Faction faction, out ThingDef result, List<ThingDef> alreadyGeneratedTechprints = null, float maxMarketValue = 3.4028235E+38f, bool weightForPlayerNeed = true)
		{
			ResearchProjectDef researchProjectDef;
			if (!TechprintUtility.GetResearchProjectsNeedingTechprintsNow(faction, alreadyGeneratedTechprints, maxMarketValue).TryRandomElementByWeight((ResearchProjectDef p) => TechprintUtility.GetSelectionWeight_NewTemp(p, weightForPlayerNeed), out researchProjectDef))
			{
				result = null;
				return false;
			}
			result = researchProjectDef.Techprint;
			return true;
		}

		// Token: 0x0600B4EB RID: 46315 RVA: 0x003FBACC File Offset: 0x003F9CCC
		[DebugOutput]
		public static void TechprintsFromFactions()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Techprints generated from these factions:");
			foreach (Faction faction in from fa in Find.FactionManager.AllFactions
			where fa.def.humanlikeFaction && !fa.Hidden
			select fa)
			{
				stringBuilder.AppendLine(faction.Name);
				for (int i = 0; i < 30; i++)
				{
					ThingDef thingDef;
					if (!TechprintUtility.TryGetTechprintDefToGenerate_NewTemp(faction, out thingDef, null, 3.4028235E+38f, true))
					{
						stringBuilder.AppendLine("    none possible");
						break;
					}
					stringBuilder.AppendLine("    " + thingDef.LabelCap);
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600B4EC RID: 46316 RVA: 0x003FBBB0 File Offset: 0x003F9DB0
		[DebugOutput]
		public static void TechprintsFromFactionsChances()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in from fa in Find.FactionManager.AllFactions
			where fa.def.humanlikeFaction && !fa.Hidden
			select fa)
			{
				Faction localFac = localFac2;
				list.Add(new FloatMenuOption(localFac.Name + " (" + localFac.def.defName + ")", delegate()
				{
					List<TableDataGetter<ResearchProjectDef>> list2 = new List<TableDataGetter<ResearchProjectDef>>();
					list2.Add(new TableDataGetter<ResearchProjectDef>("defName", (ResearchProjectDef d) => d.defName));
					IEnumerable<ResearchProjectDef> researchProjectsNeedingTechprintsNow = TechprintUtility.GetResearchProjectsNeedingTechprintsNow(localFac, null, float.MaxValue);
					if (researchProjectsNeedingTechprintsNow.Any<ResearchProjectDef>())
					{
						float sum = researchProjectsNeedingTechprintsNow.Sum((ResearchProjectDef x) => TechprintUtility.GetSelectionWeight_NewTemp(x, true));
						list2.Add(new TableDataGetter<ResearchProjectDef>("chance", (ResearchProjectDef x) => (TechprintUtility.GetSelectionWeight_NewTemp(x, true) / sum).ToStringPercent()));
						list2.Add(new TableDataGetter<ResearchProjectDef>("weight", (ResearchProjectDef x) => TechprintUtility.GetSelectionWeight_NewTemp(x, true).ToString("0.###")));
					}
					DebugTables.MakeTablesDialog<ResearchProjectDef>(researchProjectsNeedingTechprintsNow, list2.ToArray());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}
	}
}
