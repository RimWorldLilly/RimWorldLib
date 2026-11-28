using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020017BA RID: 6074
	public class ScenPart_StartingResearch : ScenPart
	{
		// Token: 0x0600977D RID: 38781 RVA: 0x0036F6EC File Offset: 0x0036D8EC
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.project.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<ResearchProjectDef>(this.NonRedundantResearchProjects(), (ResearchProjectDef d) => d.LabelCap, (ResearchProjectDef d) => delegate()
				{
					this.project = d;
				});
			}
		}

		// Token: 0x0600977E RID: 38782 RVA: 0x0036F75E File Offset: 0x0036D95E
		public override void Randomize()
		{
			this.project = this.NonRedundantResearchProjects().RandomElement<ResearchProjectDef>();
		}

		// Token: 0x0600977F RID: 38783 RVA: 0x0036F771 File Offset: 0x0036D971
		private IEnumerable<ResearchProjectDef> NonRedundantResearchProjects()
		{
			return DefDatabase<ResearchProjectDef>.AllDefs.Where(delegate(ResearchProjectDef d)
			{
				if (d.tags == null || Find.Scenario.playerFaction.factionDef.startingResearchTags == null)
				{
					return true;
				}
				return !d.tags.Any((ResearchProjectTagDef tag) => Find.Scenario.playerFaction.factionDef.startingResearchTags.Contains(tag));
			});
		}

		// Token: 0x06009780 RID: 38784 RVA: 0x0036F79C File Offset: 0x0036D99C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ResearchProjectDef>(ref this.project, "project");
		}

		// Token: 0x06009781 RID: 38785 RVA: 0x0036F7B4 File Offset: 0x0036D9B4
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartingResearchFinished".Translate(this.project.LabelCap);
		}

		// Token: 0x06009782 RID: 38786 RVA: 0x0036F7D5 File Offset: 0x0036D9D5
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.project == null;
		}

		// Token: 0x06009783 RID: 38787 RVA: 0x0036F7EA File Offset: 0x0036D9EA
		public override void PostGameStart()
		{
			if (this.project != null)
			{
				Find.ResearchManager.FinishProject(this.project, false, null, false);
			}
		}

		// Token: 0x06009784 RID: 38788 RVA: 0x0036F807 File Offset: 0x0036DA07
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.project != null) ? this.project.GetHashCode() : 0);
		}

		// Token: 0x04005ABC RID: 23228
		private ResearchProjectDef project;
	}
}
