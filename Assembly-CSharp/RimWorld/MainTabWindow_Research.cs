using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DA0 RID: 7584
	[StaticConstructorOnStartup]
	public class MainTabWindow_Research : MainTabWindow
	{
		// Token: 0x1700249E RID: 9374
		// (get) Token: 0x0600C502 RID: 50434 RVA: 0x00483F3B File Offset: 0x0048213B
		// (set) Token: 0x0600C503 RID: 50435 RVA: 0x00483F44 File Offset: 0x00482144
		public ResearchTabDef CurTab
		{
			get
			{
				return this.curTabInt;
			}
			set
			{
				if (value == this.curTabInt)
				{
					return;
				}
				this.curTabInt = value;
				Vector2 vector = this.ViewSize(this.CurTab);
				this.rightViewWidth = vector.x;
				this.rightScrollPosition = Vector2.zero;
			}
		}

		// Token: 0x1700249F RID: 9375
		// (get) Token: 0x0600C504 RID: 50436 RVA: 0x00483F88 File Offset: 0x00482188
		private MainTabWindow_Research.ResearchTabRecord CurTabRecord
		{
			get
			{
				foreach (MainTabWindow_Research.ResearchTabRecord researchTabRecord in this.tabs)
				{
					if (researchTabRecord.def == this.CurTab)
					{
						return researchTabRecord;
					}
				}
				return null;
			}
		}

		// Token: 0x170024A0 RID: 9376
		// (get) Token: 0x0600C505 RID: 50437 RVA: 0x00483FEC File Offset: 0x004821EC
		private bool ColonistsHaveResearchBench
		{
			get
			{
				bool result = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].listerBuildings.ColonistsHaveResearchBench())
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x170024A1 RID: 9377
		// (get) Token: 0x0600C506 RID: 50438 RVA: 0x0048402C File Offset: 0x0048222C
		public List<ResearchProjectDef> VisibleResearchProjects
		{
			get
			{
				if (this.cachedVisibleResearchProjects == null)
				{
					this.cachedVisibleResearchProjects = new List<ResearchProjectDef>(from d in DefDatabase<ResearchProjectDef>.AllDefsListForReading
					where Find.Storyteller.difficulty.AllowedBy(d.hideWhen) || Find.ResearchManager.IsCurrentProject(d)
					select d);
				}
				return this.cachedVisibleResearchProjects;
			}
		}

		// Token: 0x170024A2 RID: 9378
		// (get) Token: 0x0600C507 RID: 50439 RVA: 0x0048407C File Offset: 0x0048227C
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 initialSize = base.InitialSize;
				float b = (float)(UI.screenHeight - 35);
				float num = 0f;
				foreach (ResearchTabDef tab in DefDatabase<ResearchTabDef>.AllDefs)
				{
					num = Mathf.Max(num, this.ViewSize(tab).y);
				}
				float num2 = Mathf.Max(270f, (float)UI.screenWidth * 0.22f);
				float overflowTabHeight = TabDrawer.GetOverflowTabHeight<MainTabWindow_Research.ResearchTabRecord>(new Rect(num2, 0f, (float)UI.screenWidth - num2 - 200f - 4f, 0f), this.tabs, 100f, 200f);
				float b2 = this.Margin + 10f + overflowTabHeight + 10f + num + 10f + 10f + this.Margin;
				float a = Mathf.Max(initialSize.y, b2);
				initialSize.y = Mathf.Min(a, b);
				return initialSize;
			}
		}

		// Token: 0x0600C508 RID: 50440 RVA: 0x00484190 File Offset: 0x00482390
		private Vector2 ViewSize(ResearchTabDef tab)
		{
			List<ResearchProjectDef> visibleResearchProjects = this.VisibleResearchProjects;
			float num = 0f;
			float num2 = 0f;
			Text.Font = GameFont.Small;
			float num3 = 0f;
			if (ModsConfig.AnomalyActive && tab == ResearchTabDefOf.Anomaly)
			{
				num3 = 14f;
			}
			for (int i = 0; i < visibleResearchProjects.Count; i++)
			{
				ResearchProjectDef researchProjectDef = visibleResearchProjects[i];
				if (researchProjectDef.tab == tab)
				{
					Rect rect = new Rect(0f, 0f, 140f, 0f);
					Widgets.LabelCacheHeight(ref rect, this.GetLabelWithNewlineCached(this.GetLabel(researchProjectDef)), false, false);
					num = Mathf.Max(num, this.PosX(researchProjectDef) + 140f + num3);
					num2 = Mathf.Max(num2, this.PosY(researchProjectDef) + rect.height);
				}
			}
			return new Vector2(num + 20f + 4f, num2 + 20f + 4f);
		}

		// Token: 0x0600C509 RID: 50441 RVA: 0x0048427C File Offset: 0x0048247C
		public override void PreOpen()
		{
			base.PreOpen();
			this.UpdateSelectedProject(Find.ResearchManager);
			this.scrollPositioner.Arm(true);
			this.cachedVisibleResearchProjects = null;
			this.cachedUnlockedDefsGroupedByPrerequisites = null;
			this.quickSearchWidget.Reset();
			if (this.CurTab == null)
			{
				this.CurTab = ((this.selectedProject != null) ? this.selectedProject.tab : ResearchTabDefOf.Main);
			}
			this.UpdateSearchResults();
		}

		// Token: 0x0600C50A RID: 50442 RVA: 0x004842ED File Offset: 0x004824ED
		public void Select(ResearchProjectDef project)
		{
			this.CurTab = project.tab;
			this.selectedProject = project;
		}

		// Token: 0x0600C50B RID: 50443 RVA: 0x00484304 File Offset: 0x00482504
		private void UpdateSelectedProject(ResearchManager researchManager)
		{
			if (ModsConfig.AnomalyActive && this.curTabInt == ResearchTabDefOf.Anomaly)
			{
				this.selectedProject = null;
				using (List<ResearchManager.KnowledgeCategoryProject>.Enumerator enumerator = researchManager.CurrentAnomalyKnowledgeProjects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject = enumerator.Current;
						if (knowledgeCategoryProject.project != null)
						{
							this.selectedProject = knowledgeCategoryProject.project;
							break;
						}
					}
					return;
				}
			}
			this.selectedProject = researchManager.GetProject(null);
		}

		// Token: 0x0600C50C RID: 50444 RVA: 0x00484390 File Offset: 0x00482590
		public override void DoWindowContents(Rect inRect)
		{
			this.windowRect.width = (float)UI.screenWidth;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			float width = Mathf.Max(270f, inRect.width * 0.22f);
			Rect leftOutRect = new Rect(0f, 0f, width, inRect.height);
			Rect searchRect = new Rect(inRect.xMax - 200f, 0f, 200f, 24f);
			Rect rightOutRect = new Rect(leftOutRect.xMax + 10f, 0f, inRect.width - leftOutRect.width - 10f, inRect.height);
			this.DrawSearchRect(searchRect);
			this.DrawLeftRect(leftOutRect);
			this.DrawRightRect(rightOutRect, searchRect.x - 4f);
		}

		// Token: 0x0600C50D RID: 50445 RVA: 0x00484465 File Offset: 0x00482665
		private void DrawSearchRect(Rect searchRect)
		{
			this.quickSearchWidget.OnGUI(searchRect, new Action(this.UpdateSearchResults), null);
		}

		// Token: 0x0600C50E RID: 50446 RVA: 0x00484480 File Offset: 0x00482680
		private void DrawLeftRect(Rect leftOutRect)
		{
			Widgets.BeginGroup(leftOutRect);
			if (this.selectedProject != null)
			{
				this.DrawProjectInfo(leftOutRect);
			}
			else
			{
				this.DrawCurrentTabInfo(leftOutRect);
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600C50F RID: 50447 RVA: 0x004844B4 File Offset: 0x004826B4
		private void DrawCurrentTabInfo(Rect rect)
		{
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.leftScrollViewHeight);
			Widgets.BeginScrollView(outRect, ref this.leftScrollPosition, viewRect, true);
			float num = 0f;
			Text.Font = GameFont.Medium;
			GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
			Rect rect2 = new Rect(0f, num, viewRect.width - 0f, 50f);
			Widgets.LabelCacheHeight(ref rect2, this.curTabInt.generalTitle.CapitalizeFirst(), true, false);
			GenUI.ResetLabelAlign();
			Text.Font = GameFont.Small;
			num += rect2.height;
			Rect rect3 = new Rect(0f, num, viewRect.width, 0f);
			Widgets.LabelCacheHeight(ref rect3, this.curTabInt.generalDescription, true, false);
			num += rect3.height;
			this.leftScrollViewHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x0600C510 RID: 50448 RVA: 0x004845B4 File Offset: 0x004827B4
		private void DrawProjectInfo(Rect rect)
		{
			int num = (ModsConfig.AnomalyActive && this.curTabInt == ResearchTabDefOf.Anomaly) ? 2 : 1;
			float num2 = (num > 1) ? (75f * (float)num) : 100f;
			Rect rect2 = rect;
			rect2.yMin = rect.yMax - num2;
			rect2.yMax = rect.yMax;
			Rect rect3 = rect2;
			Rect rect4 = rect2;
			rect4.y = rect2.y - 30f;
			rect4.height = 28f;
			rect2 = rect2.ContractedBy(10f);
			rect2.y += 5f;
			Text.Font = GameFont.Medium;
			string key = (num > 1) ? "ActiveProjectPlural" : "ActiveProject";
			Widgets.Label(rect4, key.Translate());
			Text.Font = GameFont.Small;
			Rect startButRect = new Rect
			{
				y = rect4.y - 55f - 10f,
				height = 55f,
				x = rect.center.x - rect.width / 4f,
				width = rect.width / 2f + 20f
			};
			Widgets.DrawMenuSection(rect3);
			if (ModsConfig.AnomalyActive && this.curTabInt == ResearchTabDefOf.Anomaly)
			{
				Rect rect5 = rect2;
				rect5.height = rect2.height / 2f;
				Rect rect6 = rect5;
				rect6.yMin = rect2.yMax - rect5.height;
				rect6.yMax = rect2.yMax;
				ResearchProjectDef project = Find.ResearchManager.GetProject(KnowledgeCategoryDefOf.Basic);
				ResearchProjectDef project2 = Find.ResearchManager.GetProject(KnowledgeCategoryDefOf.Advanced);
				if (project == null && project2 == null)
				{
					using (new TextBlock(TextAnchor.MiddleCenter))
					{
						Widgets.Label(rect2, "NoProjectSelected".Translate());
						goto IL_285;
					}
				}
				float prefixWidth = DefDatabase<KnowledgeCategoryDef>.AllDefs.Max((KnowledgeCategoryDef x) => Text.CalcSize(x.LabelCap + ":").x);
				this.DrawProjectProgress(rect5, project, KnowledgeCategoryDefOf.Basic.LabelCap, prefixWidth);
				this.DrawProjectProgress(rect6, project2, KnowledgeCategoryDefOf.Advanced.LabelCap, prefixWidth);
			}
			else
			{
				ResearchProjectDef project3 = Find.ResearchManager.GetProject(null);
				if (project3 == null)
				{
					using (new TextBlock(TextAnchor.MiddleCenter))
					{
						Widgets.Label(rect2, "NoProjectSelected".Translate());
						goto IL_285;
					}
				}
				this.DrawProjectProgress(rect2, project3, null, 75f);
			}
			IL_285:
			this.DrawStartButton(startButRect);
			if (Prefs.DevMode && !Find.ResearchManager.IsCurrentProject(this.selectedProject) && !this.selectedProject.IsFinished)
			{
				Text.Font = GameFont.Tiny;
				if (Widgets.ButtonText(new Rect(rect.xMax - 120f, rect4.y, 120f, 25f), "Debug: Finish now", true, true, true, null))
				{
					Find.ResearchManager.SetCurrentProject(this.selectedProject);
					Find.ResearchManager.FinishProject(this.selectedProject, false, null, true);
				}
				Text.Font = GameFont.Small;
			}
			if (Prefs.DevMode && !this.selectedProject.TechprintRequirementMet)
			{
				Text.Font = GameFont.Tiny;
				if (Widgets.ButtonText(new Rect(rect.xMax - 300f, rect4.y, 170f, 25f), "Debug: Apply techprint", true, true, true, null))
				{
					Find.ResearchManager.ApplyTechprint(this.selectedProject, null);
					SoundDefOf.TechprintApplied.PlayOneShotOnCamera(null);
				}
				Text.Font = GameFont.Small;
			}
			float y = 0f;
			this.DrawProjectPrimaryInfo(rect, ref y);
			this.DrawProjectScrollView(new Rect(0f, y, rect.width, 0f)
			{
				yMax = startButRect.yMin - 10f
			});
		}

		// Token: 0x0600C511 RID: 50449 RVA: 0x004849BC File Offset: 0x00482BBC
		private void DrawProjectPrimaryInfo(Rect rect, ref float y)
		{
			using (new TextBlock(GameFont.Medium, TextAnchor.MiddleLeft))
			{
				Widgets.Label(new Rect(0f, y, rect.width - 0f, 50f), ref y, this.selectedProject.LabelCap, default(TipSignal));
			}
			y += 10f;
			string text = this.selectedProject.Description;
			if (ModsConfig.AnomalyActive && this.selectedProject.knowledgeCategory != null)
			{
				text = text + "\n\n" + "AnomalyResearchDescriptionHelpText".Translate().Colorize(ColoredText.SubtleGrayColor);
			}
			Widgets.Label(0f, ref y, rect.width, text, default(TipSignal));
			y += 10f;
			Widgets.DrawLineHorizontal(rect.x - 8f, y, rect.width, Color.gray);
			y += 10f;
			if (ModsConfig.AnomalyActive && this.selectedProject.knowledgeCategory != null)
			{
				Widgets.Label(0f, ref y, rect.width, "KnowledgeCategory".Translate() + ": " + this.selectedProject.knowledgeCategory.LabelCap, default(TipSignal));
			}
			Rect rect2 = new Rect(0f, y, rect.width, 500f);
			this.DrawTechprintInfo(rect2, ref y);
		}

		// Token: 0x0600C512 RID: 50450 RVA: 0x00484B48 File Offset: 0x00482D48
		private void DrawProjectScrollView(Rect rect)
		{
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.leftScrollViewHeight);
			float num = 3f;
			Widgets.BeginScrollView(rect, ref this.leftScrollPosition, viewRect, true);
			if (this.selectedProject.techLevel > Faction.OfPlayer.def.techLevel)
			{
				float num2 = this.selectedProject.CostFactor(Faction.OfPlayer.def.techLevel);
				Rect rect2 = new Rect(0f, num, viewRect.width, 0f);
				string text = "TechLevelTooLow".Translate(Faction.OfPlayer.def.techLevel.ToStringHuman(), this.selectedProject.techLevel.ToStringHuman(), (1f / num2).ToStringPercent());
				if (num2 != 1f)
				{
					text += " " + "ResearchCostComparison".Translate(this.selectedProject.Cost.ToString("F0"), this.selectedProject.CostApparent.ToString("F0"));
				}
				Widgets.LabelCacheHeight(ref rect2, text, true, false);
				num += rect2.height;
			}
			Rect rect3 = new Rect(0f, num, viewRect.width, 0f);
			this.DrawResearchPrerequisites(rect3, ref num, this.selectedProject);
			Rect rect4 = new Rect(0f, num, viewRect.width, 500f);
			num += this.DrawResearchBenchRequirements(this.selectedProject, rect4);
			Rect rect5 = new Rect(0f, num, viewRect.width, 500f);
			num += this.DrawStudyRequirements(this.selectedProject, rect5);
			Rect rect6 = new Rect(0f, num, viewRect.width, 500f);
			num += this.DrawInspectionRequirements(this.selectedProject, rect6);
			Rect rect7 = new Rect(0f, num, viewRect.width, 500f);
			Rect visibleRect = new Rect(0f, this.leftScrollPosition.y, viewRect.width, rect.height);
			num += this.DrawUnlockableHyperlinks(rect7, visibleRect, this.selectedProject);
			num += this.DrawCustomUnlockables(new Rect(0f, num, viewRect.width, 500f), this.selectedProject);
			Rect rect8 = new Rect(0f, num, viewRect.width, 500f);
			num += this.DrawContentSource(rect8, this.selectedProject);
			num += 3f;
			this.leftScrollViewHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x0600C513 RID: 50451 RVA: 0x00484E08 File Offset: 0x00483008
		private void DrawStartButton(Rect startButRect)
		{
			if (this.selectedProject.CanStartNow && !Find.ResearchManager.IsCurrentProject(this.selectedProject))
			{
				if (Widgets.ButtonText(startButRect, "Research".Translate(), true, true, true, null))
				{
					this.AttemptBeginResearch(this.selectedProject);
				}
				return;
			}
			if (Find.ResearchManager.IsCurrentProject(this.selectedProject))
			{
				if (Widgets.ButtonText(startButRect, "StopResearch".Translate(), true, true, true, null))
				{
					Find.ResearchManager.StopProject(this.selectedProject);
				}
				return;
			}
			MainTabWindow_Research.lockedReasons.Clear();
			string text;
			if (this.selectedProject.IsFinished)
			{
				text = "Finished".Translate();
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else if (Find.ResearchManager.IsCurrentProject(this.selectedProject))
			{
				text = "InProgress".Translate();
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				if (!this.selectedProject.PrerequisitesCompleted)
				{
					MainTabWindow_Research.lockedReasons.Add("PrerequisitesNotCompleted".Translate());
				}
				if (!this.selectedProject.TechprintRequirementMet)
				{
					MainTabWindow_Research.lockedReasons.Add("InsufficientTechprintsApplied".Translate(this.selectedProject.TechprintsApplied, this.selectedProject.TechprintCount));
				}
				if ((!ModsConfig.AnomalyActive || this.curTabInt != ResearchTabDefOf.Anomaly) && !this.selectedProject.PlayerHasAnyAppropriateResearchBench)
				{
					MainTabWindow_Research.lockedReasons.Add("MissingRequiredResearchFacilities".Translate());
				}
				if (!this.selectedProject.PlayerMechanitorRequirementMet)
				{
					MainTabWindow_Research.lockedReasons.Add("MissingRequiredMechanitor".Translate());
				}
				if (!this.selectedProject.InspectionRequirementsMet)
				{
					MainTabWindow_Research.lockedReasons.Add("MissingGravEngineInspection".Translate());
				}
				if (!this.selectedProject.AnalyzedThingsRequirementsMet)
				{
					for (int i = 0; i < this.selectedProject.requiredAnalyzed.Count; i++)
					{
						MainTabWindow_Research.lockedReasons.Add("NotStudied".Translate(this.selectedProject.requiredAnalyzed[i].LabelCap));
					}
				}
				if (MainTabWindow_Research.lockedReasons.NullOrEmpty<string>())
				{
					Log.ErrorOnce("Research " + this.selectedProject.defName + " locked but no reasons given", this.selectedProject.GetHashCode() ^ 100543441);
				}
				text = "Locked".Translate();
			}
			Widgets.DrawHighlight(startButRect);
			startButRect = startButRect.ContractedBy(4f);
			string text2 = text;
			if (!MainTabWindow_Research.lockedReasons.NullOrEmpty<string>())
			{
				text2 = text2 + ":\n" + MainTabWindow_Research.lockedReasons.ToLineList("  ");
			}
			Vector2 vector = Text.CalcSize(text2);
			if (vector.x > startButRect.width || vector.y > startButRect.height)
			{
				TooltipHandler.TipRegion(startButRect.ExpandedBy(4f), text2);
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				text = text2;
			}
			Widgets.Label(startButRect, text);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600C514 RID: 50452 RVA: 0x00485138 File Offset: 0x00483338
		private void DrawProjectProgress(Rect rect, ResearchProjectDef project, string prefixTitle = null, float prefixWidth = 75f)
		{
			Rect rect2 = rect;
			if (!string.IsNullOrEmpty(prefixTitle))
			{
				Rect rect3 = rect2;
				rect3.width = prefixWidth;
				rect2.xMin = rect3.xMax + 10f;
				using (new TextBlock(TextAnchor.MiddleLeft))
				{
					Widgets.Label(rect3, prefixTitle + ":");
				}
			}
			if (project == null)
			{
				using (new TextBlock(TextAnchor.MiddleCenter))
				{
					Widgets.Label(rect2, "NoProjectSelected".Translate());
				}
				return;
			}
			rect2 = rect2.ContractedBy(15f);
			Widgets.FillableBar(rect2, project.ProgressPercent, MainTabWindow_Research.ResearchBarFillTex, MainTabWindow_Research.ResearchBarBGTex, true);
			Text.Anchor = TextAnchor.MiddleCenter;
			string label = project.ProgressApparentString + " / " + project.CostApparent.ToString("F0");
			Widgets.Label(rect2, label);
			Rect rect4 = rect2;
			rect4.y = rect2.y - 22f;
			rect4.height = 22f;
			float x = Text.CalcSize(project.LabelCap).x;
			Widgets.Label(rect4, project.LabelCap.Truncate(rect4.width, null));
			if (x > rect4.width)
			{
				TooltipHandler.TipRegion(rect4, project.LabelCap);
				Widgets.DrawHighlightIfMouseover(rect4);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600C515 RID: 50453 RVA: 0x004852AC File Offset: 0x004834AC
		private void AttemptBeginResearch(ResearchProjectDef projectToStart)
		{
			List<ValueTuple<BuildableDef, List<string>>> list = this.ComputeUnlockedDefsThatHaveMissingMemes(projectToStart);
			if (!list.Any<ValueTuple<BuildableDef, List<string>>>())
			{
				this.DoBeginResearch(projectToStart);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ResearchProjectHasDefsWithMissingMemes".Translate(projectToStart.LabelCap)).Append(":");
			stringBuilder.AppendLine();
			foreach (ValueTuple<BuildableDef, List<string>> valueTuple in list)
			{
				BuildableDef item = valueTuple.Item1;
				List<string> item2 = valueTuple.Item2;
				stringBuilder.AppendLine();
				stringBuilder.Append("  - ").Append(item.LabelCap.Colorize(ColoredText.NameColor)).Append(" (").Append(item2.ToCommaList(false, false)).Append(")");
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), delegate()
			{
				this.DoBeginResearch(projectToStart);
			}, false, null, WindowLayer.Dialog));
			SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
		}

		// Token: 0x0600C516 RID: 50454 RVA: 0x004853F4 File Offset: 0x004835F4
		private List<ValueTuple<BuildableDef, List<string>>> ComputeUnlockedDefsThatHaveMissingMemes(ResearchProjectDef project)
		{
			this.cachedDefsWithMissingMemes.Clear();
			if (!ModsConfig.IdeologyActive)
			{
				return this.cachedDefsWithMissingMemes;
			}
			FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
			if (((ideos != null) ? ideos.PrimaryIdeo : null) == null)
			{
				return this.cachedDefsWithMissingMemes;
			}
			foreach (Def def in project.UnlockedDefs)
			{
				BuildableDef buildableDef = def as BuildableDef;
				if (buildableDef != null && !buildableDef.canGenerateDefaultDesignator)
				{
					List<string> list = null;
					foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefsListForReading)
					{
						if (!Faction.OfPlayer.ideos.HasAnyIdeoWithMeme(memeDef) && memeDef.AllDesignatorBuildables.Contains(buildableDef))
						{
							if (list == null)
							{
								list = new List<string>();
							}
							list.Add(memeDef.LabelCap);
						}
					}
					if (list != null)
					{
						this.cachedDefsWithMissingMemes.Add(new ValueTuple<BuildableDef, List<string>>(buildableDef, list));
					}
				}
			}
			return this.cachedDefsWithMissingMemes;
		}

		// Token: 0x0600C517 RID: 50455 RVA: 0x0048552C File Offset: 0x0048372C
		private void DoBeginResearch(ResearchProjectDef projectToStart)
		{
			SoundDefOf.ResearchStart.PlayOneShotOnCamera(null);
			Find.ResearchManager.SetCurrentProject(projectToStart);
			TutorSystem.Notify_Event("StartResearchProject");
			if ((!ModsConfig.AnomalyActive || projectToStart.knowledgeCategory == null) && !this.ColonistsHaveResearchBench)
			{
				Messages.Message("MessageResearchMenuWithoutBench".Translate(), MessageTypeDefOf.CautionInput, true);
			}
		}

		// Token: 0x0600C518 RID: 50456 RVA: 0x0048558F File Offset: 0x0048378F
		private float CoordToPixelsX(float x)
		{
			return x * 190f;
		}

		// Token: 0x0600C519 RID: 50457 RVA: 0x00485598 File Offset: 0x00483798
		private float CoordToPixelsY(float y)
		{
			return y * 100f;
		}

		// Token: 0x0600C51A RID: 50458 RVA: 0x004855A1 File Offset: 0x004837A1
		private float PixelsToCoordX(float x)
		{
			return x / 190f;
		}

		// Token: 0x0600C51B RID: 50459 RVA: 0x004855AA File Offset: 0x004837AA
		private float PixelsToCoordY(float y)
		{
			return y / 100f;
		}

		// Token: 0x0600C51C RID: 50460 RVA: 0x004855B3 File Offset: 0x004837B3
		private float PosX(ResearchProjectDef d)
		{
			return this.CoordToPixelsX(d.ResearchViewX);
		}

		// Token: 0x0600C51D RID: 50461 RVA: 0x004855C1 File Offset: 0x004837C1
		private float PosY(ResearchProjectDef d)
		{
			return this.CoordToPixelsY(d.ResearchViewY);
		}

		// Token: 0x0600C51E RID: 50462 RVA: 0x004855D0 File Offset: 0x004837D0
		public override void PostOpen()
		{
			base.PostOpen();
			this.tabs.Clear();
			using (IEnumerator<ResearchTabDef> enumerator = DefDatabase<ResearchTabDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ResearchTabDef tabDef = enumerator.Current;
					this.tabs.Add(new MainTabWindow_Research.ResearchTabRecord(tabDef, tabDef.LabelCap, delegate()
					{
						this.CurTab = tabDef;
						this.UpdateSelectedProject(Find.ResearchManager);
					}, () => this.CurTab == tabDef));
				}
			}
		}

		// Token: 0x0600C51F RID: 50463 RVA: 0x00485678 File Offset: 0x00483878
		private void DrawRightRect(Rect rightOutRect, float maxTabX)
		{
			Rect baseRect = rightOutRect;
			baseRect.xMax = maxTabX;
			rightOutRect.yMin += TabDrawer.GetOverflowTabHeight<MainTabWindow_Research.ResearchTabRecord>(baseRect, this.tabs, 100f, 200f);
			Widgets.DrawMenuSection(rightOutRect);
			TabDrawer.DrawTabsOverflow<MainTabWindow_Research.ResearchTabRecord>(baseRect, this.tabs, 100f, 200f);
			if (Prefs.DevMode)
			{
				Rect rect = rightOutRect;
				rect.yMax = rect.yMin + 20f;
				rect.xMin = rect.xMax - 80f;
				Rect butRect = rect.RightPartPixels(30f);
				rect = rect.LeftPartPixels(rect.width - 30f);
				Widgets.CheckboxLabeled(rect, "Edit", ref this.editMode, false, null, null, false, false);
				if (Widgets.ButtonImageFitted(butRect, TexButton.Copy))
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (ResearchProjectDef researchProjectDef in from def in this.VisibleResearchProjects
					where def.Debug_IsPositionModified()
					select def)
					{
						stringBuilder.AppendLine(researchProjectDef.defName);
						StringBuilder stringBuilder2 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
						defaultInterpolatedStringHandler.AppendLiteral("  <researchViewX>");
						defaultInterpolatedStringHandler.AppendFormatted<float>(researchProjectDef.ResearchViewX, "F2");
						defaultInterpolatedStringHandler.AppendLiteral("</researchViewX>");
						stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
						StringBuilder stringBuilder3 = stringBuilder;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(33, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("  <researchViewY>");
						defaultInterpolatedStringHandler2.AppendFormatted<float>(researchProjectDef.ResearchViewY, "F2");
						defaultInterpolatedStringHandler2.AppendLiteral("</researchViewY>");
						stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
						stringBuilder.AppendLine();
					}
					GUIUtility.systemCopyBuffer = stringBuilder.ToString();
					Messages.Message("Modified data copied to clipboard.", MessageTypeDefOf.SituationResolved, false);
				}
			}
			else
			{
				this.editMode = false;
			}
			bool flag = false;
			Rect rect2 = rightOutRect.ContractedBy(1f);
			Rect rect3 = new Rect(0f, 0f, Mathf.Max(this.rightViewWidth + 20f, rect2.width), rightOutRect.height - 16f);
			Rect rect4 = rect3.ContractedBy(20f);
			Rect rect5 = rect3;
			rect5.height = rightOutRect.height;
			this.scrollPositioner.ClearInterestRects();
			if (Find.ResearchManager.TabInfoVisible(this.CurTab))
			{
				Widgets.ScrollHorizontal(rect2, ref this.rightScrollPosition, rect3, 20f);
				Widgets.BeginScrollView(rect2, ref this.rightScrollPosition, rect3, true);
				if (this.CurTab == ResearchTabDefOf.Anomaly)
				{
					Color color = GUI.color;
					GUI.color = new Color(1f, 1f, 1f, 0.15f);
					GUI.DrawTexture(rect5.LeftPartPixels(830f), MainTabWindow_Research.BasicBackgroundTex.Texture, ScaleMode.StretchToFill);
					GUI.DrawTexture(rect5.RightPartPixels(rect5.width - 830f + 1f), MainTabWindow_Research.AdvancedBackgroundTex.Texture, ScaleMode.StretchToFill);
					GUI.color = color;
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnomalyResearch, OpportunityType.GoodToKnow);
				}
				Widgets.BeginGroup(rect4);
				this.ListProjects(rect4, ref flag);
				Widgets.EndGroup();
				Widgets.EndScrollView();
				this.scrollPositioner.ScrollHorizontally(ref this.rightScrollPosition, rect2.size);
				if (this.editMode)
				{
					if (!flag && Input.GetMouseButtonDown(0))
					{
						this.draggingTabs.Clear();
					}
					if (!this.draggingTabs.NullOrEmpty<ResearchProjectDef>())
					{
						if (Input.GetMouseButtonUp(0))
						{
							for (int i = 0; i < this.draggingTabs.Count; i++)
							{
								this.draggingTabs[i].Debug_SnapPositionData();
							}
							return;
						}
						if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && Event.current.type == EventType.layout)
						{
							for (int j = 0; j < this.draggingTabs.Count; j++)
							{
								this.draggingTabs[j].Debug_ApplyPositionDelta(new Vector2(this.PixelsToCoordX(Event.current.delta.x), this.PixelsToCoordY(Event.current.delta.y)));
							}
							return;
						}
					}
				}
			}
			else
			{
				Widgets.BeginGroup(rect2);
				using (new TextBlock(TextAnchor.MiddleCenter, ColoredText.SubtleGrayColor))
				{
					Widgets.Label(rect2.AtZero(), "ResearchNotDiscovered".Translate());
				}
				Widgets.EndGroup();
			}
		}

		// Token: 0x0600C520 RID: 50464 RVA: 0x00485B10 File Offset: 0x00483D10
		private void ListProjects(Rect rightInRect, ref bool elementClicked)
		{
			List<ResearchProjectDef> visibleResearchProjects = this.VisibleResearchProjects;
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);
			for (int i = 0; i < 2; i++)
			{
				foreach (ResearchProjectDef researchProjectDef in visibleResearchProjects)
				{
					if (researchProjectDef.tab == this.CurTab)
					{
						float num = 0f;
						if (ModsConfig.AnomalyActive && researchProjectDef.knowledgeCategory != null)
						{
							num = 14f;
						}
						start.x = this.PosX(researchProjectDef);
						start.y = this.PosY(researchProjectDef) + 25f;
						for (int j = 0; j < researchProjectDef.prerequisites.CountAllowNull<ResearchProjectDef>(); j++)
						{
							ResearchProjectDef researchProjectDef2 = researchProjectDef.prerequisites[j];
							if (researchProjectDef2 != null && researchProjectDef2.tab == this.CurTab)
							{
								end.x = this.PosX(researchProjectDef2) + 140f + num;
								end.y = this.PosY(researchProjectDef2) + 25f;
								if (this.selectedProject == researchProjectDef || this.selectedProject == researchProjectDef2)
								{
									if (i == 1)
									{
										Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
									}
								}
								else if (i == 0)
								{
									Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
								}
							}
						}
					}
				}
			}
			Rect other = new Rect(this.rightScrollPosition.x, this.rightScrollPosition.y, rightInRect.width, rightInRect.height).ExpandedBy(10f);
			using (List<ResearchProjectDef>.Enumerator enumerator = visibleResearchProjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ResearchProjectDef project = enumerator.Current;
					if (project.tab == this.CurTab)
					{
						float num2 = 0f;
						if (ModsConfig.AnomalyActive && project.knowledgeCategory != null)
						{
							num2 = 14f;
						}
						Rect rect = new Rect(this.PosX(project), this.PosY(project), 140f, 50f);
						rect.xMax += num2;
						string label = this.GetLabel(project);
						Rect rect2 = rect;
						rect2.xMin += num2;
						Widgets.LabelCacheHeight(ref rect2, label, true, false);
						Rect rect3 = rect2;
						rect3.y = rect2.yMax - 4f;
						Widgets.LabelCacheHeight(ref rect3, " ", true, false);
						rect.yMax = rect3.yMax;
						bool flag = this.quickSearchWidget.filter.Active && !this.matchingProjects.Contains(project);
						bool flag2 = this.quickSearchWidget.filter.Active && this.matchingProjects.Contains(project);
						if (flag2 || this.selectedProject == project)
						{
							this.scrollPositioner.RegisterInterestRect(rect);
						}
						if (project.IsHidden)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
							defaultInterpolatedStringHandler.AppendLiteral("(");
							defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("UnknownResearch".Translate());
							defaultInterpolatedStringHandler.AppendLiteral(")");
							label = defaultInterpolatedStringHandler.ToStringAndClear();
						}
						if (rect.Overlaps(other))
						{
							Color color = Widgets.NormalOptionColor;
							Color color2 = TexUI.OtherActiveResearchColor;
							Color windowBGFillColor = Widgets.WindowBGFillColor;
							Color color3 = default(Color);
							bool flag3 = !project.IsFinished && !project.CanStartNow;
							bool flag4 = false;
							if (project.IsHidden)
							{
								color2 = TexUI.HiddenResearchColor;
								color = MainTabWindow_Research.HiddenProjectLabelColor;
							}
							else if (Find.ResearchManager.IsCurrentProject(project))
							{
								color2 = TexUI.ActiveResearchColor;
								color = MainTabWindow_Research.ActiveProjectLabelColor;
							}
							else if (project.IsFinished)
							{
								color2 = TexUI.FinishedResearchColor;
							}
							else if (flag3)
							{
								color2 = TexUI.LockedResearchColor;
							}
							if (flag3)
							{
								color = MainTabWindow_Research.ProjectWithMissingPrerequisiteLabelColor;
							}
							if (this.editMode && this.draggingTabs.Contains(project))
							{
								color3 = Color.yellow;
							}
							else if (this.selectedProject == project)
							{
								color2 += TexUI.HighlightBgResearchColor;
								color3 = TexUI.BorderResearchSelectedColor;
								color = TexUI.BorderResearchSelectedColor;
								flag4 = true;
							}
							else if (Find.ResearchManager.IsCurrentProject(project))
							{
								color3 = TexUI.BorderResearchingColor;
								flag4 = true;
							}
							else
							{
								color3 = TexUI.DefaultBorderResearchColor;
							}
							if (this.selectedProject != null)
							{
								if (project.prerequisites.NotNullAndContains(this.selectedProject) || project.hiddenPrerequisites.NotNullAndContains(this.selectedProject))
								{
									color3 = TexUI.HighlightLineResearchColor;
								}
								if (this.selectedProject.prerequisites.NotNullAndContains(project) || this.selectedProject.hiddenPrerequisites.NotNullAndContains(project))
								{
									color3 = (project.IsFinished ? TexUI.HighlightLineResearchColor : TexUI.DependencyOutlineResearchColor);
								}
							}
							Color color4 = project.TechprintRequirementMet ? MainTabWindow_Research.FulfilledPrerequisiteColor : MainTabWindow_Research.MissingPrerequisiteColor;
							Color color5 = project.AnalyzedThingsRequirementsMet ? MainTabWindow_Research.FulfilledPrerequisiteColor : MainTabWindow_Research.MissingPrerequisiteColor;
							if (flag)
							{
								color = this.NoMatchTint(color);
								color2 = this.NoMatchTint(color2);
								color3 = this.NoMatchTint(color3);
								color4 = this.NoMatchTint(color4);
								color5 = this.NoMatchTint(color5);
							}
							if (flag2)
							{
								Widgets.DrawStrongHighlight(rect.ExpandedBy(4f), null);
							}
							int num3 = flag4 ? 2 : 1;
							if (Widgets.CustomButtonText(ref rect, "", color2, color, color3, windowBGFillColor, false, (float)num3, true, true, project.ProgressPercent) && !project.IsHidden)
							{
								SoundDefOf.Click.PlayOneShotOnCamera(null);
								this.selectedProject = project;
							}
							Color color6 = GUI.color;
							TextAnchor anchor = Text.Anchor;
							if (ModsConfig.AnomalyActive && project.knowledgeCategory != null)
							{
								Rect position = rect;
								position.x += 4f;
								position.width = 14f;
								GUI.color = project.knowledgeCategory.color;
								GUI.DrawTexture(position, project.knowledgeCategory.Tex, ScaleMode.ScaleToFit);
							}
							GUI.color = color;
							Text.Anchor = TextAnchor.UpperCenter;
							Widgets.Label(rect2, label);
							if (!project.IsHidden)
							{
								this.DrawBottomRow(rect3, project, color4, color5);
								if (Mouse.IsOver(rect) && !this.editMode)
								{
									Widgets.DrawLightHighlight(rect);
									TooltipHandler.TipRegion(rect, () => project.GetTip(), project.GetHashCode() ^ 91727);
								}
							}
							GUI.color = color6;
							Text.Anchor = anchor;
							if (this.editMode && Mouse.IsOver(rect2) && Input.GetMouseButtonDown(0))
							{
								elementClicked = true;
								if (Input.GetKey(KeyCode.LeftShift))
								{
									if (!this.draggingTabs.Contains(project))
									{
										this.draggingTabs.Add(project);
									}
								}
								else if (!Input.GetKey(KeyCode.LeftControl) && !this.draggingTabs.Contains(project))
								{
									this.draggingTabs.Clear();
									this.draggingTabs.Add(project);
								}
								if (Input.GetKey(KeyCode.LeftControl) && this.draggingTabs.Contains(project))
								{
									this.draggingTabs.Remove(project);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600C521 RID: 50465 RVA: 0x0048632C File Offset: 0x0048452C
		private void DrawBottomRow(Rect rect, ResearchProjectDef project, Color techprintColor, Color studiedColor)
		{
			Color color = GUI.color;
			TextAnchor anchor = Text.Anchor;
			int num = 1;
			if (project.TechprintCount > 0)
			{
				num++;
			}
			if (project.RequiredAnalyzedThingCount > 0)
			{
				num++;
			}
			float num2 = rect.width / (float)num;
			Rect rect2 = rect;
			rect2.x = rect.x;
			rect2.width = num2;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect2, project.CostApparent.ToString());
			rect2.x += num2;
			if (project.TechprintCount > 0)
			{
				string text = this.GetTechprintsInfoCached(project.TechprintsApplied, project.TechprintCount);
				Vector2 vector = Text.CalcSize(text);
				Rect rect3 = rect2;
				rect3.xMin = rect2.xMax - vector.x - 10f;
				Rect rect4 = rect2;
				rect4.width = rect4.height;
				rect4.x = rect3.x - rect4.width;
				GUI.color = techprintColor;
				Widgets.Label(rect3, text);
				GUI.color = Color.white;
				GUI.DrawTexture(rect4.ContractedBy(4f), MainTabWindow_Research.TechprintRequirementTex.Texture);
				rect2.x += num2;
			}
			if (project.RequiredAnalyzedThingCount > 0)
			{
				string text2 = this.GetTechprintsInfoCached(project.AnalyzedThingsCompleted, project.RequiredAnalyzedThingCount);
				Vector2 vector2 = Text.CalcSize(text2);
				Rect rect5 = rect2;
				rect5.xMin = rect2.xMax - vector2.x - 10f;
				Rect rect6 = rect2;
				rect6.width = rect6.height;
				rect6.x = rect5.x - rect6.width;
				GUI.color = studiedColor;
				Widgets.Label(rect5, text2);
				GUI.color = Color.white;
				GUI.DrawTexture(rect6.ContractedBy(4f), MainTabWindow_Research.StudyRequirementTex.Texture);
			}
			GUI.color = color;
			Text.Anchor = anchor;
		}

		// Token: 0x0600C522 RID: 50466 RVA: 0x0048650E File Offset: 0x0048470E
		private Color NoMatchTint(Color color)
		{
			return Color.Lerp(color, MainTabWindow_Research.NoMatchTintColor, 0.4f);
		}

		// Token: 0x0600C523 RID: 50467 RVA: 0x00486520 File Offset: 0x00484720
		private void DrawResearchPrerequisites(Rect rect, ref float y, ResearchProjectDef project)
		{
			if (project.prerequisites.NullOrEmpty<ResearchProjectDef>() && project.hiddenPrerequisites.NullOrEmpty<ResearchProjectDef>())
			{
				return;
			}
			Rect rect2 = rect;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Prerequisites".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(":");
			Widgets.Label(rect2, ref y, defaultInterpolatedStringHandler.ToStringAndClear(), default(TipSignal));
			rect.xMin += 6f;
			foreach (ResearchProjectDef researchProjectDef in project.prerequisites.OrElseEmptyEnumerable<ResearchProjectDef>())
			{
				this.SetPrerequisiteStatusColor(researchProjectDef.IsFinished, project);
				Rect rect3 = rect;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("- ");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>(researchProjectDef.LabelCap);
				Widgets.Label(rect3, ref y, defaultInterpolatedStringHandler2.ToStringAndClear(), default(TipSignal));
			}
			if (project.hiddenPrerequisites != null)
			{
				foreach (ResearchProjectDef researchProjectDef2 in project.hiddenPrerequisites)
				{
					this.SetPrerequisiteStatusColor(researchProjectDef2.IsFinished, project);
					Rect rect4 = rect;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(2, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("- ");
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>(researchProjectDef2.LabelCap);
					Widgets.Label(rect4, ref y, defaultInterpolatedStringHandler3.ToStringAndClear(), default(TipSignal));
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x0600C524 RID: 50468 RVA: 0x004866B4 File Offset: 0x004848B4
		private string GetLabelWithNewlineCached(string label)
		{
			if (!MainTabWindow_Research.labelsWithNewlineCached.ContainsKey(label))
			{
				MainTabWindow_Research.labelsWithNewlineCached.Add(label, label + "\n");
			}
			return MainTabWindow_Research.labelsWithNewlineCached[label];
		}

		// Token: 0x0600C525 RID: 50469 RVA: 0x004866E4 File Offset: 0x004848E4
		private string GetTechprintsInfoCached(int applied, int total)
		{
			Pair<int, int> key = new Pair<int, int>(applied, total);
			if (!MainTabWindow_Research.techprintsInfoCached.ContainsKey(key))
			{
				MainTabWindow_Research.techprintsInfoCached.Add(key, applied.ToString() + " / " + total.ToString());
			}
			return MainTabWindow_Research.techprintsInfoCached[key];
		}

		// Token: 0x0600C526 RID: 50470 RVA: 0x00486738 File Offset: 0x00484938
		private float DrawResearchBenchRequirements(ResearchProjectDef project, Rect rect)
		{
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			if (project.requiredResearchBuilding != null)
			{
				bool present = false;
				Predicate<Building> <>9__0;
				foreach (Map map in Find.Maps)
				{
					List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
					Predicate<Building> match;
					if ((match = <>9__0) == null)
					{
						match = (<>9__0 = ((Building x) => x.def == project.requiredResearchBuilding));
					}
					if (allBuildingsColonist.Find(match) != null)
					{
						present = true;
						break;
					}
				}
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBench".Translate() + ":", true, false);
				rect.xMin += 6f;
				rect.yMin += rect.height;
				this.SetPrerequisiteStatusColor(present, project);
				rect.height = Text.CalcHeight(project.requiredResearchBuilding.LabelCap, rect.width - 24f - 6f);
				Widgets.HyperlinkWithIcon(rect, new Dialog_InfoCard.Hyperlink(project.requiredResearchBuilding, -1), null, 2f, 6f, null, false, null);
				rect.yMin += rect.height + 4f;
				GUI.color = Color.white;
				rect.xMin = xMin;
			}
			if (!project.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			{
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBenchFacilities".Translate() + ":", true, false);
				rect.yMin += rect.height;
				Building_ResearchBench building_ResearchBench = this.FindBenchFulfillingMostRequirements(project.requiredResearchBuilding, project.requiredResearchFacilities);
				CompAffectedByFacilities bestMatchingBench = null;
				if (building_ResearchBench != null)
				{
					bestMatchingBench = building_ResearchBench.TryGetComp<CompAffectedByFacilities>();
				}
				rect.xMin += 6f;
				foreach (ThingDef requiredFacility in project.requiredResearchFacilities)
				{
					this.DrawResearchBenchFacilityRequirement(requiredFacility, bestMatchingBench, project, ref rect);
					rect.yMin += rect.height;
				}
				rect.yMin += 4f;
			}
			GUI.color = Color.white;
			rect.xMin = xMin;
			return rect.yMin - yMin;
		}

		// Token: 0x0600C527 RID: 50471 RVA: 0x004869F4 File Offset: 0x00484BF4
		private float DrawStudyRequirements(ResearchProjectDef project, Rect rect)
		{
			float yMin = rect.yMin;
			if (project.RequiredAnalyzedThingCount > 0)
			{
				Widgets.LabelCacheHeight(ref rect, "StudyRequirements".Translate() + ":", true, false);
				rect.xMin += 6f;
				rect.yMin += rect.height;
				foreach (ThingDef thingDef in project.requiredAnalyzed)
				{
					Rect rect2 = new Rect(rect.x, rect.yMin, rect.width, 24f);
					Color? color = null;
					if (this.quickSearchWidget.filter.Active)
					{
						if (this.MatchesUnlockedDef(thingDef))
						{
							Widgets.DrawTextHighlight(rect2, 4f, null);
						}
						else
						{
							color = new Color?(this.NoMatchTint(Widgets.NormalOptionColor));
						}
					}
					Dialog_InfoCard.Hyperlink hyperlink = new Dialog_InfoCard.Hyperlink(thingDef, -1);
					Widgets.HyperlinkWithIcon(rect2, hyperlink, null, 2f, 6f, color, false, this.LabelSuffixForUnlocked(thingDef));
					rect.yMin += 24f;
				}
			}
			return rect.yMin - yMin;
		}

		// Token: 0x0600C528 RID: 50472 RVA: 0x00486B54 File Offset: 0x00484D54
		private float DrawInspectionRequirements(ResearchProjectDef project, Rect rect)
		{
			float yMin = rect.yMin;
			if (ModsConfig.OdysseyActive && project.requireGravEngineInspected)
			{
				Widgets.LabelCacheHeight(ref rect, "RequiresInspectionof".Translate() + ":", true, false);
				rect.yMin += 24f;
				rect.xMin += 6f;
				Rect rect2 = new Rect(rect.x, rect.yMin, rect.width, 24f);
				Widgets.HyperlinkWithIcon(rect2, new Dialog_InfoCard.Hyperlink(ThingDefOf.GravEngine, -1), null, 2f, 6f, null, false, null);
				rect.yMin += rect2.height;
			}
			return rect.yMin - yMin;
		}

		// Token: 0x0600C529 RID: 50473 RVA: 0x00486C2C File Offset: 0x00484E2C
		private float DrawUnlockableHyperlinks(Rect rect, Rect visibleRect, ResearchProjectDef project)
		{
			List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> list = this.UnlockedDefsGroupedByPrerequisites(project);
			if (list.NullOrEmpty<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>())
			{
				return 0f;
			}
			float yMin = rect.yMin;
			float x = rect.x;
			foreach (Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>> pair in list)
			{
				ResearchPrerequisitesUtility.UnlockedHeader first = pair.First;
				rect.x = x;
				if (!first.unlockedBy.Any<ResearchProjectDef>())
				{
					Widgets.LabelCacheHeight(ref rect, "Unlocks".Translate() + ":", true, false);
				}
				else
				{
					Widgets.LabelCacheHeight(ref rect, "UnlockedWith".Translate() + " " + this.HeaderLabel(first) + ":", true, false);
				}
				rect.x += 6f;
				rect.yMin += rect.height;
				foreach (Def def in pair.Second)
				{
					Rect rect2 = new Rect(rect.x, rect.yMin, rect.width, 24f);
					if (visibleRect.Overlaps(rect2))
					{
						Color? color = null;
						if (this.quickSearchWidget.filter.Active)
						{
							if (this.MatchesUnlockedDef(def))
							{
								Widgets.DrawTextHighlight(rect2, 4f, null);
							}
							else
							{
								color = new Color?(this.NoMatchTint(Widgets.NormalOptionColor));
							}
						}
						Dialog_InfoCard.Hyperlink hyperlink = new Dialog_InfoCard.Hyperlink(def, -1);
						Widgets.HyperlinkWithIcon(rect2, hyperlink, null, 2f, 6f, color, false, this.LabelSuffixForUnlocked(def));
					}
					rect.yMin += 24f;
				}
			}
			return rect.yMin - yMin;
		}

		// Token: 0x0600C52A RID: 50474 RVA: 0x00486E58 File Offset: 0x00485058
		private float DrawCustomUnlockables(Rect rect, ResearchProjectDef project)
		{
			if (project.customUnlockTexts.NullOrEmpty<string>())
			{
				return 0f;
			}
			float yMin = rect.yMin;
			for (int i = 0; i < project.customUnlockTexts.Count; i++)
			{
				if (i == 0)
				{
					Widgets.LabelCacheHeight(ref rect, "Unlocks".Translate() + ":", true, false);
					rect.x += 6f;
					rect.yMin += rect.height;
				}
				Widgets.LabelCacheHeight(ref rect, project.customUnlockTexts[i], true, false);
				rect.yMin += rect.height;
			}
			return rect.yMin - yMin;
		}

		// Token: 0x0600C52B RID: 50475 RVA: 0x00486F1C File Offset: 0x0048511C
		private float DrawContentSource(Rect rect, ResearchProjectDef project)
		{
			if (project.modContentPack == null || project.modContentPack.IsCoreMod)
			{
				return 0f;
			}
			float yMin = rect.yMin;
			TaggedString taggedString = "Stat_Source_Label".Translate() + ":  " + project.modContentPack.Name;
			Widgets.LabelCacheHeight(ref rect, taggedString.Colorize(Color.grey), true, false);
			ExpansionDef expansionDef = ModLister.AllExpansions.Find((ExpansionDef e) => e.linkedMod == project.modContentPack.PackageId);
			if (expansionDef != null)
			{
				GUI.DrawTexture(new Rect(Text.CalcSize(taggedString).x + 4f, rect.y, 20f, 20f), expansionDef.IconFromStatus);
			}
			return rect.yMax - yMin;
		}

		// Token: 0x0600C52C RID: 50476 RVA: 0x00486FFC File Offset: 0x004851FC
		private string LabelSuffixForUnlocked(Def unlocked)
		{
			if (!ModLister.IdeologyInstalled)
			{
				return null;
			}
			this.tmpSuffixesForUnlocked.Clear();
			foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefs)
			{
				if (memeDef.AllDesignatorBuildables.Contains(unlocked))
				{
					this.tmpSuffixesForUnlocked.AddUnique(memeDef.LabelCap);
				}
				if (!memeDef.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
				{
					using (List<ThingStyleCategoryWithPriority>.Enumerator enumerator2 = memeDef.thingStyleCategories.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.category.AllDesignatorBuildables.Contains(unlocked))
							{
								this.tmpSuffixesForUnlocked.AddUnique(memeDef.LabelCap);
							}
						}
					}
				}
			}
			foreach (CultureDef cultureDef in DefDatabase<CultureDef>.AllDefs)
			{
				if (!cultureDef.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
				{
					using (List<ThingStyleCategoryWithPriority>.Enumerator enumerator2 = cultureDef.thingStyleCategories.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.category.AllDesignatorBuildables.Contains(unlocked))
							{
								this.tmpSuffixesForUnlocked.AddUnique(cultureDef.LabelCap);
							}
						}
					}
				}
			}
			if (!this.tmpSuffixesForUnlocked.Any<string>())
			{
				return null;
			}
			return " (" + this.tmpSuffixesForUnlocked.ToCommaList(false, false) + ")";
		}

		// Token: 0x0600C52D RID: 50477 RVA: 0x004871C0 File Offset: 0x004853C0
		private List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> UnlockedDefsGroupedByPrerequisites(ResearchProjectDef project)
		{
			if (this.cachedUnlockedDefsGroupedByPrerequisites == null)
			{
				this.cachedUnlockedDefsGroupedByPrerequisites = new Dictionary<ResearchProjectDef, List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>>();
			}
			List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> list;
			if (!this.cachedUnlockedDefsGroupedByPrerequisites.TryGetValue(project, out list))
			{
				list = ResearchPrerequisitesUtility.UnlockedDefsGroupedByPrerequisites(project);
				this.cachedUnlockedDefsGroupedByPrerequisites.Add(project, list);
			}
			return list;
		}

		// Token: 0x0600C52E RID: 50478 RVA: 0x00487208 File Offset: 0x00485408
		private string HeaderLabel(ResearchPrerequisitesUtility.UnlockedHeader headerProject)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = "";
			for (int i = 0; i < headerProject.unlockedBy.Count; i++)
			{
				ResearchProjectDef researchProjectDef = headerProject.unlockedBy[i];
				string text = researchProjectDef.LabelCap;
				if (!researchProjectDef.IsFinished)
				{
					text = text.Colorize(MainTabWindow_Research.MissingPrerequisiteColor);
				}
				stringBuilder.Append(value).Append(text);
				value = ", ";
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600C52F RID: 50479 RVA: 0x0048727C File Offset: 0x0048547C
		private void DrawTechprintInfo(Rect rect, ref float y)
		{
			if (this.selectedProject.TechprintCount == 0)
			{
				return;
			}
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			string text = "ResearchTechprintsFromFactions".Translate();
			float num = Text.CalcHeight(text, rect.width);
			Widgets.Label(new Rect(rect.x, yMin, rect.width, num), text);
			rect.x += 6f;
			if (this.selectedProject.heldByFactionCategoryTags != null)
			{
				foreach (string b in this.selectedProject.heldByFactionCategoryTags)
				{
					foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
					{
						if (faction.def.categoryTag == b)
						{
							string name = faction.Name;
							Rect rect2 = new Rect(rect.x, yMin + num, rect.width, Mathf.Max(24f, Text.CalcHeight(name, rect.width - 24f - 6f)));
							Widgets.BeginGroup(rect2);
							Rect r = new Rect(0f, 0f, 24f, 24f).ContractedBy(2f);
							FactionUIUtility.DrawFactionIconWithTooltip(r, faction);
							Rect rect3 = new Rect(r.xMax + 6f, 0f, rect2.width - r.width - 6f, rect2.height);
							Text.Anchor = TextAnchor.MiddleLeft;
							Text.WordWrap = false;
							Widgets.Label(rect3, faction.Name);
							Text.Anchor = TextAnchor.UpperLeft;
							Text.WordWrap = true;
							Widgets.EndGroup();
							num += rect2.height;
						}
					}
				}
			}
			rect.xMin = xMin;
			y += num;
		}

		// Token: 0x0600C530 RID: 50480 RVA: 0x004874B4 File Offset: 0x004856B4
		private string GetLabel(ResearchProjectDef r)
		{
			return r.LabelCap;
		}

		// Token: 0x0600C531 RID: 50481 RVA: 0x004874C1 File Offset: 0x004856C1
		private void SetPrerequisiteStatusColor(bool present, ResearchProjectDef project)
		{
			if (project.IsFinished)
			{
				return;
			}
			GUI.color = (present ? MainTabWindow_Research.FulfilledPrerequisiteColor : MainTabWindow_Research.MissingPrerequisiteColor);
		}

		// Token: 0x0600C532 RID: 50482 RVA: 0x004874E0 File Offset: 0x004856E0
		private void DrawResearchBenchFacilityRequirement(ThingDef requiredFacility, CompAffectedByFacilities bestMatchingBench, ResearchProjectDef project, ref Rect rect)
		{
			Thing thing = null;
			Thing thing2 = null;
			if (bestMatchingBench != null)
			{
				thing = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility);
				thing2 = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility && bestMatchingBench.IsFacilityActive(x));
			}
			this.SetPrerequisiteStatusColor(thing2 != null, project);
			string text = requiredFacility.LabelCap;
			if (thing != null && thing2 == null)
			{
				text += " (" + "InactiveFacility".Translate() + ")";
			}
			rect.height = Text.CalcHeight(text, rect.width - 24f - 6f);
			Widgets.HyperlinkWithIcon(rect, new Dialog_InfoCard.Hyperlink(requiredFacility, -1), text, 2f, 6f, null, false, null);
		}

		// Token: 0x0600C533 RID: 50483 RVA: 0x004875E4 File Offset: 0x004857E4
		private Building_ResearchBench FindBenchFulfillingMostRequirements(ThingDef requiredResearchBench, List<ThingDef> requiredFacilities)
		{
			MainTabWindow_Research.tmpAllBuildings.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				MainTabWindow_Research.tmpAllBuildings.AddRange(maps[i].listerBuildings.allBuildingsColonist);
			}
			float num = 0f;
			Building_ResearchBench building_ResearchBench = null;
			for (int j = 0; j < MainTabWindow_Research.tmpAllBuildings.Count; j++)
			{
				Building_ResearchBench building_ResearchBench2 = MainTabWindow_Research.tmpAllBuildings[j] as Building_ResearchBench;
				if (building_ResearchBench2 != null && (requiredResearchBench == null || building_ResearchBench2.def == requiredResearchBench))
				{
					float researchBenchRequirementsScore = this.GetResearchBenchRequirementsScore(building_ResearchBench2, requiredFacilities);
					if (building_ResearchBench == null || researchBenchRequirementsScore > num)
					{
						num = researchBenchRequirementsScore;
						building_ResearchBench = building_ResearchBench2;
					}
				}
			}
			MainTabWindow_Research.tmpAllBuildings.Clear();
			return building_ResearchBench;
		}

		// Token: 0x0600C534 RID: 50484 RVA: 0x00487698 File Offset: 0x00485898
		private float GetResearchBenchRequirementsScore(Building_ResearchBench bench, List<ThingDef> requiredFacilities)
		{
			float num = 0f;
			int i;
			Predicate<Thing> <>9__1;
			int j;
			for (i = 0; i < requiredFacilities.Count; i = j + 1)
			{
				CompAffectedByFacilities benchComp = bench.GetComp<CompAffectedByFacilities>();
				if (benchComp != null)
				{
					List<Thing> linkedFacilitiesListForReading = benchComp.LinkedFacilitiesListForReading;
					if (linkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacilities[i] && benchComp.IsFacilityActive(x)) != null)
					{
						num += 1f;
					}
					else
					{
						List<Thing> list = linkedFacilitiesListForReading;
						Predicate<Thing> match;
						if ((match = <>9__1) == null)
						{
							match = (<>9__1 = ((Thing x) => x.def == requiredFacilities[i]));
						}
						if (list.Find(match) != null)
						{
							num += 0.6f;
						}
					}
				}
				j = i;
			}
			return num;
		}

		// Token: 0x0600C535 RID: 50485 RVA: 0x00487778 File Offset: 0x00485978
		private void UpdateSearchResults()
		{
			this.quickSearchWidget.noResultsMatched = false;
			this.matchingProjects.Clear();
			foreach (MainTabWindow_Research.ResearchTabRecord researchTabRecord in this.tabs)
			{
				researchTabRecord.Reset();
			}
			if (!this.quickSearchWidget.filter.Active)
			{
				return;
			}
			foreach (ResearchProjectDef researchProjectDef in this.VisibleResearchProjects)
			{
				if (!researchProjectDef.IsHidden && (this.quickSearchWidget.filter.Matches(this.GetLabel(researchProjectDef)) || this.<UpdateSearchResults>g__MatchesUnlockedDefs|109_0(researchProjectDef)))
				{
					this.matchingProjects.Add(researchProjectDef);
				}
			}
			this.quickSearchWidget.noResultsMatched = !this.matchingProjects.Any<ResearchProjectDef>();
			using (List<MainTabWindow_Research.ResearchTabRecord>.Enumerator enumerator = this.tabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MainTabWindow_Research.ResearchTabRecord tab = enumerator.Current;
					tab.firstMatch = (from p in this.matchingProjects
					where tab.def == p.tab
					orderby p.ResearchViewX
					select p).FirstOrDefault<ResearchProjectDef>();
					if (!tab.AnyMatches)
					{
						tab.labelColor = new Color?(Color.grey);
					}
				}
			}
			if (!this.CurTabRecord.AnyMatches)
			{
				foreach (MainTabWindow_Research.ResearchTabRecord researchTabRecord2 in this.tabs)
				{
					if (researchTabRecord2.AnyMatches)
					{
						this.CurTab = researchTabRecord2.def;
						break;
					}
				}
			}
			this.scrollPositioner.Arm(true);
			if (this.CurTabRecord.firstMatch != null)
			{
				this.selectedProject = this.CurTabRecord.firstMatch;
			}
		}

		// Token: 0x0600C536 RID: 50486 RVA: 0x004879C4 File Offset: 0x00485BC4
		private bool MatchesUnlockedDef(Def unlocked)
		{
			return this.quickSearchWidget.filter.Matches(unlocked.label);
		}

		// Token: 0x0600C537 RID: 50487 RVA: 0x004879DC File Offset: 0x00485BDC
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			this.quickSearchWidget.Unfocus();
		}

		// Token: 0x0600C53A RID: 50490 RVA: 0x00487B50 File Offset: 0x00485D50
		[CompilerGenerated]
		private bool <UpdateSearchResults>g__MatchesUnlockedDefs|109_0(ResearchProjectDef proj)
		{
			foreach (Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>> pair in this.UnlockedDefsGroupedByPrerequisites(proj))
			{
				foreach (Def unlocked in pair.Second)
				{
					if (this.MatchesUnlockedDef(unlocked))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04007099 RID: 28825
		protected ResearchProjectDef selectedProject;

		// Token: 0x0400709A RID: 28826
		private ScrollPositioner scrollPositioner = new ScrollPositioner();

		// Token: 0x0400709B RID: 28827
		private Vector2 leftScrollPosition = Vector2.zero;

		// Token: 0x0400709C RID: 28828
		private float leftScrollViewHeight;

		// Token: 0x0400709D RID: 28829
		private Vector2 rightScrollPosition;

		// Token: 0x0400709E RID: 28830
		private float rightViewWidth;

		// Token: 0x0400709F RID: 28831
		private ResearchTabDef curTabInt;

		// Token: 0x040070A0 RID: 28832
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x040070A1 RID: 28833
		private bool editMode;

		// Token: 0x040070A2 RID: 28834
		private List<ResearchProjectDef> draggingTabs = new List<ResearchProjectDef>();

		// Token: 0x040070A3 RID: 28835
		private List<MainTabWindow_Research.ResearchTabRecord> tabs = new List<MainTabWindow_Research.ResearchTabRecord>();

		// Token: 0x040070A4 RID: 28836
		private List<ResearchProjectDef> cachedVisibleResearchProjects;

		// Token: 0x040070A5 RID: 28837
		private Dictionary<ResearchProjectDef, List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>> cachedUnlockedDefsGroupedByPrerequisites;

		// Token: 0x040070A6 RID: 28838
		private readonly HashSet<ResearchProjectDef> matchingProjects = new HashSet<ResearchProjectDef>();

		// Token: 0x040070A7 RID: 28839
		private const float leftAreaWidthPercent = 0.22f;

		// Token: 0x040070A8 RID: 28840
		private const float LeftAreaWidthMin = 270f;

		// Token: 0x040070A9 RID: 28841
		private const float ProjectTitleHeight = 50f;

		// Token: 0x040070AA RID: 28842
		private const float ProjectTitleLeftMargin = 0f;

		// Token: 0x040070AB RID: 28843
		private const int ResearchItemW = 140;

		// Token: 0x040070AC RID: 28844
		private const int ResearchItemH = 50;

		// Token: 0x040070AD RID: 28845
		private const int ResearchItemPaddingW = 50;

		// Token: 0x040070AE RID: 28846
		private const int ResearchItemPaddingH = 50;

		// Token: 0x040070AF RID: 28847
		private const int CategoryRectW = 14;

		// Token: 0x040070B0 RID: 28848
		private const float IndentSpacing = 6f;

		// Token: 0x040070B1 RID: 28849
		private const float RowHeight = 24f;

		// Token: 0x040070B2 RID: 28850
		private const float LeftStartButHeight = 55f;

		// Token: 0x040070B3 RID: 28851
		private const float SearchBoxHeight = 24f;

		// Token: 0x040070B4 RID: 28852
		private const float SearchBoxWidth = 200f;

		// Token: 0x040070B5 RID: 28853
		private const float MinTabWidth = 100f;

		// Token: 0x040070B6 RID: 28854
		private const float MaxTabWidth = 200f;

		// Token: 0x040070B7 RID: 28855
		private const int SearchHighlightMargin = 4;

		// Token: 0x040070B8 RID: 28856
		private const KeyCode SelectMultipleKey = KeyCode.LeftShift;

		// Token: 0x040070B9 RID: 28857
		private const KeyCode DeselectKey = KeyCode.LeftControl;

		// Token: 0x040070BA RID: 28858
		private static readonly Texture2D ResearchBarFillTex = SolidColorMaterials.NewSolidColorTexture(TexUI.ResearchMainTabColor);

		// Token: 0x040070BB RID: 28859
		private static readonly Texture2D ResearchBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f));

		// Token: 0x040070BC RID: 28860
		private static readonly CachedTexture BasicBackgroundTex = new CachedTexture("UI/AnomalyResearchCategoryMarkers/AnomalyResearchBackground_Basic");

		// Token: 0x040070BD RID: 28861
		private static readonly CachedTexture AdvancedBackgroundTex = new CachedTexture("UI/AnomalyResearchCategoryMarkers/AnomalyResearchBackground_Advanced");

		// Token: 0x040070BE RID: 28862
		private static readonly Color FulfilledPrerequisiteColor = Color.green;

		// Token: 0x040070BF RID: 28863
		private static readonly Color MissingPrerequisiteColor = ColorLibrary.RedReadable;

		// Token: 0x040070C0 RID: 28864
		private static readonly Color ProjectWithMissingPrerequisiteLabelColor = Color.gray;

		// Token: 0x040070C1 RID: 28865
		private static readonly Color HiddenProjectLabelColor = Color.gray;

		// Token: 0x040070C2 RID: 28866
		private static readonly Color ActiveProjectLabelColor = new ColorInt(219, 201, 126, 255).ToColor;

		// Token: 0x040070C3 RID: 28867
		private static readonly Color NoMatchTintColor = Widgets.MenuSectionBGFillColor;

		// Token: 0x040070C4 RID: 28868
		private const float NoMatchTintFactor = 0.4f;

		// Token: 0x040070C5 RID: 28869
		private static readonly CachedTexture TechprintRequirementTex = new CachedTexture("UI/Icons/Research/Techprint");

		// Token: 0x040070C6 RID: 28870
		private static readonly CachedTexture StudyRequirementTex = new CachedTexture("UI/Icons/Study");

		// Token: 0x040070C7 RID: 28871
		private static List<string> lockedReasons = new List<string>();

		// Token: 0x040070C8 RID: 28872
		private List<ValueTuple<BuildableDef, List<string>>> cachedDefsWithMissingMemes = new List<ValueTuple<BuildableDef, List<string>>>();

		// Token: 0x040070C9 RID: 28873
		private static Dictionary<string, string> labelsWithNewlineCached = new Dictionary<string, string>();

		// Token: 0x040070CA RID: 28874
		private static Dictionary<Pair<int, int>, string> techprintsInfoCached = new Dictionary<Pair<int, int>, string>();

		// Token: 0x040070CB RID: 28875
		private List<string> tmpSuffixesForUnlocked = new List<string>();

		// Token: 0x040070CC RID: 28876
		private static List<Building> tmpAllBuildings = new List<Building>();

		// Token: 0x02003B02 RID: 15106
		private class ResearchTabRecord : TabRecord
		{
			// Token: 0x1700365B RID: 13915
			// (get) Token: 0x060153ED RID: 87021 RVA: 0x006BED33 File Offset: 0x006BCF33
			public bool AnyMatches
			{
				get
				{
					return this.firstMatch != null;
				}
			}

			// Token: 0x1700365C RID: 13916
			// (get) Token: 0x060153EE RID: 87022 RVA: 0x006BED3E File Offset: 0x006BCF3E
			public override string TutorTag
			{
				get
				{
					return this.def.tutorTag;
				}
			}

			// Token: 0x060153EF RID: 87023 RVA: 0x006BED4B File Offset: 0x006BCF4B
			public ResearchTabRecord(ResearchTabDef def, string label, Action clickedAction, Func<bool> selected) : base(label, clickedAction, selected)
			{
				this.def = def;
			}

			// Token: 0x060153F0 RID: 87024 RVA: 0x006BED5E File Offset: 0x006BCF5E
			public void Reset()
			{
				this.firstMatch = null;
				this.labelColor = null;
			}

			// Token: 0x060153F1 RID: 87025 RVA: 0x006BED74 File Offset: 0x006BCF74
			public override string GetTip()
			{
				if (this.cachedTip == null)
				{
					this.cachedTip = (this.def.generalTitle.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor) ?? "");
					if (!this.def.generalDescription.NullOrEmpty())
					{
						this.cachedTip = this.cachedTip + "\n" + this.def.generalDescription;
					}
				}
				return this.cachedTip;
			}

			// Token: 0x0400EA5E RID: 59998
			public readonly ResearchTabDef def;

			// Token: 0x0400EA5F RID: 59999
			public ResearchProjectDef firstMatch;

			// Token: 0x0400EA60 RID: 60000
			private string cachedTip;
		}
	}
}
