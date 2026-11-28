using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001831 RID: 6193
	public abstract class CompBiosculpterPod_Cycle : ThingComp
	{
		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x06009B2B RID: 39723 RVA: 0x0037F42B File Offset: 0x0037D62B
		public CompProperties_BiosculpterPod_BaseCycle Props
		{
			get
			{
				return (CompProperties_BiosculpterPod_BaseCycle)this.props;
			}
		}

		// Token: 0x06009B2C RID: 39724
		public abstract void CycleCompleted(Pawn occupant);

		// Token: 0x06009B2D RID: 39725 RVA: 0x0037F438 File Offset: 0x0037D638
		public virtual string Description(Pawn tunedFor)
		{
			return this.Props.description;
		}

		// Token: 0x06009B2E RID: 39726 RVA: 0x0037F448 File Offset: 0x0037D648
		public List<string> MissingResearchLabels()
		{
			this.tmpMissingResearchLabels.Clear();
			if (this.Props.requiredResearch.NullOrEmpty<ResearchProjectDef>())
			{
				return this.tmpMissingResearchLabels;
			}
			foreach (ResearchProjectDef researchProjectDef in this.Props.requiredResearch)
			{
				if (!researchProjectDef.IsFinished)
				{
					this.tmpMissingResearchLabels.Add(researchProjectDef.LabelCap);
				}
			}
			return this.tmpMissingResearchLabels;
		}

		// Token: 0x04005C4A RID: 23626
		private List<string> tmpMissingResearchLabels = new List<string>();
	}
}
