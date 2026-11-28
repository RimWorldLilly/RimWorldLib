using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C19 RID: 7193
	public class Alert_NeedAnomalyProject : Alert
	{
		// Token: 0x0600B795 RID: 46997 RVA: 0x0040B5F8 File Offset: 0x004097F8
		public Alert_NeedAnomalyProject()
		{
			this.defaultLabel = "NeedAnomalyProject".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0600B796 RID: 46998 RVA: 0x0040B648 File Offset: 0x00409848
		public override AlertReport GetReport()
		{
			if (Find.CurrentMap == null)
			{
				return false;
			}
			if (!Find.Anomaly.AnomalyStudyEnabled)
			{
				return false;
			}
			if (!Find.ResearchManager.AnyProjectIsAvailable)
			{
				return false;
			}
			this.GetCurrentCategories();
			this.GetMissingCategories();
			this.missingProjectCategories.RemoveAll((KnowledgeCategoryDef c) => !Find.ResearchManager.AnyProjectsAvailableWithKnowledgeCategory(c));
			return this.missingProjectCategories.Count > 0;
		}

		// Token: 0x0600B797 RID: 46999 RVA: 0x0040B6D4 File Offset: 0x004098D4
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (KnowledgeCategoryDef knowledgeCategoryDef in this.missingProjectCategories)
			{
				StringBuilder stringBuilder = this.sb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 1);
				defaultInterpolatedStringHandler.AppendLiteral("  - ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("NeedAnomalyProjectDescLine".Translate(knowledgeCategoryDef.label.Named("NAME")));
				stringBuilder.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 3);
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("NeedAnomalyProjectDesc".Translate());
			defaultInterpolatedStringHandler2.AppendLiteral("\n");
			defaultInterpolatedStringHandler2.AppendFormatted(this.sb.ToString().TrimEndNewlines());
			defaultInterpolatedStringHandler2.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("NeedAnomalyProjectDescAppended".Translate());
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}

		// Token: 0x0600B798 RID: 47000 RVA: 0x0040B7DC File Offset: 0x004099DC
		private void GetMissingCategories()
		{
			this.missingProjectCategories.Clear();
			foreach (ResearchManager.KnowledgeCategoryProject knowledgeCategoryProject in Find.ResearchManager.CurrentAnomalyKnowledgeProjects)
			{
				if (knowledgeCategoryProject.project == null && this.currentCategories.Contains(knowledgeCategoryProject.category) && knowledgeCategoryProject.category.overflowCategory == null)
				{
					this.missingProjectCategories.Add(knowledgeCategoryProject.category);
				}
			}
		}

		// Token: 0x0600B799 RID: 47001 RVA: 0x0040B870 File Offset: 0x00409A70
		private void GetCurrentCategories()
		{
			this.currentCategories.Clear();
			foreach (Thing thing in Find.StudyManager.GetStudiableThingsAndPlatforms(Find.CurrentMap))
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					thing = building_HoldingPlatform.HeldPawn;
					CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.CanStudy)
					{
						continue;
					}
				}
				CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
				if (compStudiable != null && compStudiable.studyEnabled && compStudiable.KnowledgeCategory != null)
				{
					this.currentCategories.Add(compStudiable.KnowledgeCategory);
				}
			}
		}

		// Token: 0x0600B79A RID: 47002 RVA: 0x0040B924 File Offset: 0x00409B24
		protected override void OnClick()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
			((MainTabWindow_Research)MainButtonDefOf.Research.TabWindow).CurTab = ResearchTabDefOf.Anomaly;
		}

		// Token: 0x04006942 RID: 26946
		private HashSet<KnowledgeCategoryDef> currentCategories = new HashSet<KnowledgeCategoryDef>();

		// Token: 0x04006943 RID: 26947
		private List<KnowledgeCategoryDef> missingProjectCategories = new List<KnowledgeCategoryDef>();

		// Token: 0x04006944 RID: 26948
		private StringBuilder sb = new StringBuilder();
	}
}
