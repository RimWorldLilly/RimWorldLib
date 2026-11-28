using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001125 RID: 4389
	public sealed class ResearchManager : IExposable
	{
		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x06006DEF RID: 28143 RVA: 0x00268355 File Offset: 0x00266555
		public bool AnyProjectIsAvailable
		{
			get
			{
				return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Any((ResearchProjectDef x) => x.baseCost > 0f && x.CanStartNow);
			}
		}

		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x06006DF0 RID: 28144 RVA: 0x00268380 File Offset: 0x00266580
		public List<ResearchManager.KnowledgeCategoryProject> CurrentAnomalyKnowledgeProjects
		{
			get
			{
				this.EnsureKnowledgeProjectsInitialized();
				return this.currentAnomalyKnowledgeProjects;
			}
		}

		// Token: 0x06006DF1 RID: 28145 RVA: 0x00268390 File Offset: 0x00266590
		public bool TabInfoVisible(ResearchTabDef tab)
		{
			if (this.tabInfoVisibility == null)
			{
				this.tabInfoVisibility = new DefMap<ResearchTabDef, bool>();
				foreach (ResearchTabDef researchTabDef in DefDatabase<ResearchTabDef>.AllDefs)
				{
					this.tabInfoVisibility[researchTabDef] = researchTabDef.visibleByDefault;
				}
			}
			return this.tabInfoVisibility[tab];
		}

		// Token: 0x06006DF2 RID: 28146 RVA: 0x00268408 File Offset: 0x00266608
		public void ExposeData()
		{
			Scribe_Defs.Look<ResearchProjectDef>(ref this.currentProj, "currentProj");
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.progress, "progress", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ResearchProjectDef, int>(ref this.techprints, "techprints", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.anomalyKnowledge, "knowledge", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ResearchManager.KnowledgeCategoryProject>(ref this.currentAnomalyKnowledgeProjects, "currentKnowledgeProjects", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<DefMap<ResearchTabDef, bool>>(ref this.tabInfoVisibility, "tabInfoVisibility", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.gravEngineInspected, "gravEngineInspected", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.ResearchManagerPostLoadInit();
				if (this.tabInfoVisibility == null)
				{
					this.tabInfoVisibility = new DefMap<ResearchTabDef, bool>();
				}
				foreach (ResearchTabDef researchTabDef in DefDatabase<ResearchTabDef>.AllDefs)
				{
					if (researchTabDef.visibleByDefault)
					{
						this.tabInfoVisibility[researchTabDef] = true;
					}
				}
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				if (this.techprints == null)
				{
					this.techprints = new Dictionary<ResearchProjectDef, int>();
				}
				if (ModsConfig.AnomalyActive)
				{
					if (this.anomalyKnowledge == null)
					{
						this.anomalyKnowledge = new Dictionary<ResearchProjectDef, float>();
					}
					this.EnsureKnowledgeProjectsInitialized();
				}
			}
		}

		// Token: 0x06006DF3 RID: 28147 RVA: 0x00268540 File Offset: 0x00266740
		public void SetCurrentProject(ResearchProjectDef proj)
		{
			if (proj.baseCost > 0f)
			{
				this.currentProj = proj;
			}
			if (ModsConfig.AnomalyActive && proj.knowledgeCategory != null)
			{
				foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in this.CurrentAnomalyKnowledgeProjects)
				{
					if (knowledgeCategoryProject.category == proj.knowledgeCategory)
					{
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnomalyResearch, KnowledgeAmount.Total);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.StudyingEntities, KnowledgeAmount.Total);
						knowledgeCategoryProject.project = proj;
						break;
					}
				}
			}
		}

		// Token: 0x06006DF4 RID: 28148 RVA: 0x002685DC File Offset: 0x002667DC
		public void StopProject(ResearchProjectDef proj)
		{
			if (this.currentProj == proj)
			{
				this.currentProj = null;
				return;
			}
			if (ModsConfig.AnomalyActive && proj.knowledgeCategory != null)
			{
				foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in this.CurrentAnomalyKnowledgeProjects)
				{
					if (knowledgeCategoryProject.category == proj.knowledgeCategory)
					{
						knowledgeCategoryProject.project = null;
						break;
					}
				}
			}
		}

		// Token: 0x06006DF5 RID: 28149 RVA: 0x00268660 File Offset: 0x00266860
		public ResearchProjectDef GetProject(KnowledgeCategoryDef category = null)
		{
			if (category == null)
			{
				return this.currentProj;
			}
			foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in this.CurrentAnomalyKnowledgeProjects)
			{
				if (knowledgeCategoryProject.category == category)
				{
					return knowledgeCategoryProject.project;
				}
			}
			return null;
		}

		// Token: 0x06006DF6 RID: 28150 RVA: 0x002686CC File Offset: 0x002668CC
		public bool IsCurrentProject(ResearchProjectDef proj)
		{
			if (proj == this.currentProj)
			{
				return true;
			}
			if (ModsConfig.AnomalyActive)
			{
				using (List<ResearchManager.KnowledgeCategoryProject>.Enumerator enumerator = this.CurrentAnomalyKnowledgeProjects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.project == proj)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06006DF7 RID: 28151 RVA: 0x00268738 File Offset: 0x00266938
		public float GetProgress(ResearchProjectDef proj)
		{
			if (proj.baseCost > 0f)
			{
				float result;
				if (this.progress.TryGetValue(proj, out result))
				{
					return result;
				}
				this.progress.Add(proj, 0f);
				return 0f;
			}
			else
			{
				if (!ModsConfig.AnomalyActive || proj.knowledgeCost <= 0f)
				{
					return 0f;
				}
				float result2;
				if (this.anomalyKnowledge.TryGetValue(proj, out result2))
				{
					return result2;
				}
				this.anomalyKnowledge.Add(proj, 0f);
				return 0f;
			}
		}

		// Token: 0x06006DF8 RID: 28152 RVA: 0x002687C0 File Offset: 0x002669C0
		public void AddProgress(ResearchProjectDef proj, float amount, Pawn source = null)
		{
			float num;
			if (this.progress.TryGetValue(proj, out num))
			{
				this.progress[proj] = num + amount;
			}
			else
			{
				this.progress.Add(proj, amount);
			}
			this.progress[proj] = Mathf.Min(this.progress[proj], proj.Cost);
			if (proj.PrerequisitesCompleted && proj.IsFinished)
			{
				this.FinishProject(proj, true, source, true);
			}
		}

		// Token: 0x06006DF9 RID: 28153 RVA: 0x00268838 File Offset: 0x00266A38
		public int GetTechprints(ResearchProjectDef proj)
		{
			int result;
			if (!this.techprints.TryGetValue(proj, out result))
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06006DFA RID: 28154 RVA: 0x00268858 File Offset: 0x00266A58
		public void ApplyTechprint(ResearchProjectDef proj, Pawn applyingPawn)
		{
			if (!ModLister.CheckRoyalty("Techprint"))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("LetterTechprintAppliedPartIntro".Translate(proj.Named("PROJECT")));
			stringBuilder.AppendLine();
			if (proj.TechprintCount > this.GetTechprints(proj))
			{
				this.AddTechprints(proj, 1);
				if (proj.TechprintCount == this.GetTechprints(proj))
				{
					stringBuilder.AppendLine("LetterTechprintAppliedPartJustUnlocked".Translate(proj.Named("PROJECT")));
					stringBuilder.AppendLine();
				}
				else
				{
					stringBuilder.AppendLine("LetterTechprintAppliedPartNotUnlockedYet".Translate(this.GetTechprints(proj), proj.TechprintCount.ToString(), proj.Named("PROJECT")));
					stringBuilder.AppendLine();
				}
			}
			else if (proj.IsFinished)
			{
				stringBuilder.AppendLine("LetterTechprintAppliedPartAlreadyResearched".Translate(proj.Named("PROJECT")));
				stringBuilder.AppendLine();
			}
			else if (!proj.IsFinished && proj.baseCost > 0f)
			{
				float num = (proj.baseCost - this.GetProgress(proj)) * 0.5f;
				stringBuilder.AppendLine("LetterTechprintAppliedPartAlreadyUnlocked".Translate(num, proj.Named("PROJECT")));
				stringBuilder.AppendLine();
				float num2;
				if (!this.progress.TryGetValue(proj, out num2))
				{
					this.progress.Add(proj, Mathf.Min(num, proj.baseCost));
				}
				else
				{
					this.progress[proj] = Mathf.Min(num2 + num, proj.baseCost);
				}
			}
			if (applyingPawn != null)
			{
				stringBuilder.AppendLine("LetterTechprintAppliedPartExpAwarded".Translate(2000.ToString(), SkillDefOf.Intellectual.label, applyingPawn.Named("PAWN")));
				applyingPawn.skills.Learn(SkillDefOf.Intellectual, 2000f, true, true);
			}
			if (stringBuilder.Length > 0)
			{
				Find.LetterStack.ReceiveLetter("LetterTechprintAppliedLabel".Translate(proj.Named("PROJECT")), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.PositiveEvent, null, 0, true);
			}
		}

		// Token: 0x06006DFB RID: 28155 RVA: 0x00268AB8 File Offset: 0x00266CB8
		public void AddTechprints(ResearchProjectDef proj, int amount)
		{
			int num;
			if (this.techprints.TryGetValue(proj, out num))
			{
				num += amount;
				if (num > proj.TechprintCount)
				{
					num = proj.TechprintCount;
				}
				this.techprints[proj] = num;
				return;
			}
			this.techprints.Add(proj, amount);
		}

		// Token: 0x06006DFC RID: 28156 RVA: 0x00268B04 File Offset: 0x00266D04
		public void ResearchPerformed(float amount, Pawn researcher)
		{
			if (this.currentProj == null)
			{
				Log.Error("Researched without having an active project.");
				return;
			}
			amount *= 0.00825f;
			amount *= Find.Storyteller.difficulty.researchSpeedFactor;
			if (((researcher != null) ? researcher.Faction : null) != null)
			{
				amount /= this.currentProj.CostFactor(researcher.Faction.def.techLevel);
			}
			if (DebugSettings.fastResearch)
			{
				amount *= 500f;
			}
			if (researcher != null)
			{
				researcher.records.AddTo(RecordDefOf.ResearchPointsResearched, amount);
			}
			float num = this.GetProgress(this.currentProj);
			num += amount;
			this.progress[this.currentProj] = num;
			if (this.currentProj.IsFinished)
			{
				this.FinishProject(this.currentProj, true, researcher, true);
			}
		}

		// Token: 0x06006DFD RID: 28157 RVA: 0x00268BD0 File Offset: 0x00266DD0
		public float GetKnowledge(ResearchProjectDef proj)
		{
			float result;
			if (this.anomalyKnowledge.TryGetValue(proj, out result))
			{
				return result;
			}
			return 0f;
		}

		// Token: 0x06006DFE RID: 28158 RVA: 0x00268BF4 File Offset: 0x00266DF4
		public void ApplyKnowledge(KnowledgeCategoryDef category, float amount)
		{
			if (!ModLister.CheckAnomaly("Knowledge"))
			{
				return;
			}
			if (amount <= 0f)
			{
				return;
			}
			bool flag = false;
			ResearchProjectDef project = this.GetProject(category);
			if (project != null)
			{
				float num;
				if (this.ApplyKnowledge(project, amount, out num) && num > 0f)
				{
					amount = num;
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag && category.overflowCategory != null)
			{
				this.ApplyKnowledge(category.overflowCategory, amount);
			}
		}

		// Token: 0x06006DFF RID: 28159 RVA: 0x00268C5C File Offset: 0x00266E5C
		public bool ApplyKnowledge(ResearchProjectDef project, float amount, out float remainder)
		{
			float num;
			if (this.anomalyKnowledge.TryGetValue(project, out num))
			{
				this.anomalyKnowledge[project] = num + amount;
			}
			else
			{
				this.anomalyKnowledge.Add(project, amount);
			}
			if (project.PrerequisitesCompleted && project.IsFinished)
			{
				remainder = this.anomalyKnowledge[project] - project.knowledgeCost;
				this.anomalyKnowledge[project] = Mathf.Min(this.anomalyKnowledge[project], project.Cost);
				this.FinishProject(project, true, null, false);
				return true;
			}
			remainder = 0f;
			return false;
		}

		// Token: 0x06006E00 RID: 28160 RVA: 0x00268CF4 File Offset: 0x00266EF4
		public void ReapplyAllMods()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				if (researchProjectDef.IsFinished)
				{
					researchProjectDef.ReapplyAllMods();
				}
			}
		}

		// Token: 0x06006E01 RID: 28161 RVA: 0x00268D48 File Offset: 0x00266F48
		public void FinishProject(ResearchProjectDef proj, bool doCompletionDialog = false, Pawn researcher = null, bool doCompletionLetter = true)
		{
			if (proj.prerequisites != null)
			{
				for (int i = 0; i < proj.prerequisites.Count; i++)
				{
					if (!proj.prerequisites[i].IsFinished)
					{
						this.FinishProject(proj.prerequisites[i], doCompletionDialog, researcher, doCompletionLetter);
					}
				}
			}
			int num = this.GetTechprints(proj);
			if (num < proj.TechprintCount)
			{
				this.AddTechprints(proj, proj.TechprintCount - num);
			}
			if (proj.RequiredAnalyzedThingCount > 0)
			{
				for (int j = 0; j < proj.requiredAnalyzed.Count; j++)
				{
					CompProperties_CompAnalyzableUnlockResearch compProperties = proj.requiredAnalyzed[j].GetCompProperties<CompProperties_CompAnalyzableUnlockResearch>();
					Find.AnalysisManager.ForceCompleteAnalysisProgress(compProperties.analysisID);
				}
			}
			if (proj.baseCost > 0f)
			{
				this.progress[proj] = proj.baseCost;
			}
			else if (ModsConfig.AnomalyActive && proj.knowledgeCost > 0f)
			{
				this.anomalyKnowledge.SetOrAdd(proj, proj.knowledgeCost);
				Find.SignalManager.SendSignal(new Signal("ThingStudied", true));
			}
			if (researcher != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.FinishedResearchProject, new object[]
				{
					researcher,
					proj
				});
			}
			this.ReapplyAllMods();
			if (proj.recalculatePower)
			{
				try
				{
					foreach (Map map in Find.Maps)
					{
						foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.PowerTrader))
						{
							CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
							if (compPowerTrader != null)
							{
								compPowerTrader.SetUpPowerVars();
							}
						}
					}
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
			if (doCompletionDialog)
			{
				DiaNode diaNode = new DiaNode("ResearchFinished".Translate(proj.LabelCap) + "\n\n" + proj.description);
				diaNode.options.Add(DiaOption.DefaultOK);
				DiaOption diaOption = new DiaOption("ResearchScreen".Translate());
				diaOption.resolveTree = true;
				diaOption.action = delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
					MainTabWindow_Research mainTabWindow_Research = MainButtonDefOf.Research.TabWindow as MainTabWindow_Research;
					if (mainTabWindow_Research != null && proj.tab != null)
					{
						mainTabWindow_Research.CurTab = proj.tab;
					}
				};
				diaNode.options.Add(diaOption);
				Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
			}
			if (doCompletionLetter && !proj.discoveredLetterTitle.NullOrEmpty() && Find.Storyteller.difficulty.AllowedBy(proj.discoveredLetterDisabledWhen))
			{
				Find.LetterStack.ReceiveLetter(proj.discoveredLetterTitle, proj.discoveredLetterText, LetterDefOf.NeutralEvent, null, 0, true);
			}
			if (proj.teachConcept != null)
			{
				LessonAutoActivator.TeachOpportunity(proj.teachConcept, OpportunityType.Important);
			}
			if (this.currentProj == proj)
			{
				this.currentProj = null;
			}
			else if (ModsConfig.AnomalyActive && proj.knowledgeCategory != null)
			{
				foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in this.CurrentAnomalyKnowledgeProjects)
				{
					if (knowledgeCategoryProject.project == proj)
					{
						knowledgeCategoryProject.project = null;
						break;
					}
				}
			}
			foreach (Def def in proj.UnlockedDefs)
			{
				ThingDef thingDef = def as ThingDef;
				if (thingDef != null)
				{
					thingDef.Notify_UnlockedByResearch();
				}
			}
			Find.SignalManager.SendSignal(new Signal("ResearchCompleted", true));
		}

		// Token: 0x06006E02 RID: 28162 RVA: 0x002691B8 File Offset: 0x002673B8
		public void ResetAllProgress()
		{
			this.progress.Clear();
			this.techprints.Clear();
			this.anomalyKnowledge.Clear();
			this.currentProj = null;
			this.gravEngineInspected = false;
		}

		// Token: 0x06006E03 RID: 28163 RVA: 0x002691EC File Offset: 0x002673EC
		private void EnsureKnowledgeProjectsInitialized()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (this.currentAnomalyKnowledgeProjects == null)
			{
				this.currentAnomalyKnowledgeProjects = new List<ResearchManager.KnowledgeCategoryProject>();
			}
			this.currentAnomalyKnowledgeProjects.RemoveAll((ResearchManager.KnowledgeCategoryProject x) => x.category == null);
			using (IEnumerator<KnowledgeCategoryDef> enumerator = DefDatabase<KnowledgeCategoryDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KnowledgeCategoryDef k = enumerator.Current;
					if (!this.currentAnomalyKnowledgeProjects.Any((ResearchManager.KnowledgeCategoryProject x) => x.category == k))
					{
						this.currentAnomalyKnowledgeProjects.Add(new ResearchManager.KnowledgeCategoryProject
						{
							category = k,
							project = null
						});
					}
				}
			}
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x002692C0 File Offset: 0x002674C0
		public void Notify_MonolithLevelChanged(int newLevel)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			foreach (ResearchTabDef researchTabDef in DefDatabase<ResearchTabDef>.AllDefs)
			{
				if (!this.TabInfoVisible(researchTabDef) && researchTabDef.minMonolithLevelVisible > 0 && (newLevel >= researchTabDef.minMonolithLevelVisible || (!Find.Anomaly.GenerateMonolith && Find.Storyteller.difficulty.AnomalyPlaystyleDef.enableAnomalyContent)))
				{
					this.tabInfoVisibility[researchTabDef] = true;
				}
			}
		}

		// Token: 0x06006E05 RID: 28165 RVA: 0x00269358 File Offset: 0x00267558
		public void DebugSetAllProjectsFinished()
		{
			this.progress.Clear();
			this.anomalyKnowledge.Clear();
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				if (researchProjectDef.baseCost > 0f)
				{
					this.progress.Add(researchProjectDef, researchProjectDef.baseCost);
				}
				else if (researchProjectDef.knowledgeCost > 0f)
				{
					this.anomalyKnowledge.Add(researchProjectDef, researchProjectDef.knowledgeCost);
				}
			}
			this.ReapplyAllMods();
		}

		// Token: 0x06006E06 RID: 28166 RVA: 0x002693FC File Offset: 0x002675FC
		public bool AnyProjectsAvailableWithKnowledgeCategory(KnowledgeCategoryDef category)
		{
			return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Any((ResearchProjectDef x) => x.CanStartNow && x.knowledgeCategory == category);
		}

		// Token: 0x04004649 RID: 17993
		private ResearchProjectDef currentProj;

		// Token: 0x0400464A RID: 17994
		private List<ResearchManager.KnowledgeCategoryProject> currentAnomalyKnowledgeProjects;

		// Token: 0x0400464B RID: 17995
		private Dictionary<ResearchProjectDef, float> progress = new Dictionary<ResearchProjectDef, float>();

		// Token: 0x0400464C RID: 17996
		private Dictionary<ResearchProjectDef, int> techprints = new Dictionary<ResearchProjectDef, int>();

		// Token: 0x0400464D RID: 17997
		private Dictionary<ResearchProjectDef, float> anomalyKnowledge = new Dictionary<ResearchProjectDef, float>();

		// Token: 0x0400464E RID: 17998
		private DefMap<ResearchTabDef, bool> tabInfoVisibility;

		// Token: 0x0400464F RID: 17999
		public bool gravEngineInspected;

		// Token: 0x04004650 RID: 18000
		public const float ResearchPointsPerWorkTick = 0.00825f;

		// Token: 0x04004651 RID: 18001
		public const int IntellectualExpPerTechprint = 2000;

		// Token: 0x04004652 RID: 18002
		public const string ResearchCompletedSignal = "ResearchCompleted";

		// Token: 0x0200307B RID: 12411
		public class KnowledgeCategoryProject : IExposable
		{
			// Token: 0x06012527 RID: 75047 RVA: 0x006305E7 File Offset: 0x0062E7E7
			public void ExposeData()
			{
				Scribe_Defs.Look<KnowledgeCategoryDef>(ref this.category, "category");
				Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
			}

			// Token: 0x0400C4FD RID: 50429
			public KnowledgeCategoryDef category;

			// Token: 0x0400C4FE RID: 50430
			public ResearchProjectDef project;
		}
	}
}
