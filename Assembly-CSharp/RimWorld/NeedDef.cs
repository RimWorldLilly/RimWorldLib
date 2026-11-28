using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB4 RID: 4020
	public class NeedDef : Def
	{
		// Token: 0x06006622 RID: 26146 RVA: 0x00244FA7 File Offset: 0x002431A7
		public override IEnumerable<string> ConfigErrors()
		{
			NeedDef.<ConfigErrors>d__30 <ConfigErrors>d__ = new NeedDef.<ConfigErrors>d__30(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003E5C RID: 15964
		public Type needClass;

		// Token: 0x04003E5D RID: 15965
		public Intelligence minIntelligence;

		// Token: 0x04003E5E RID: 15966
		public bool colonistAndPrisonersOnly;

		// Token: 0x04003E5F RID: 15967
		public bool playerMechsOnly;

		// Token: 0x04003E60 RID: 15968
		public bool colonistsOnly;

		// Token: 0x04003E61 RID: 15969
		public bool slavesOnly;

		// Token: 0x04003E62 RID: 15970
		public bool onlyIfCausedByHediff;

		// Token: 0x04003E63 RID: 15971
		public bool onlyIfCausedByGene;

		// Token: 0x04003E64 RID: 15972
		public bool onlyIfCausedByTrait;

		// Token: 0x04003E65 RID: 15973
		public bool onlyIfCausedByIdeo;

		// Token: 0x04003E66 RID: 15974
		public bool neverOnPrisoner;

		// Token: 0x04003E67 RID: 15975
		public bool neverOnSlave;

		// Token: 0x04003E68 RID: 15976
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x04003E69 RID: 15977
		public List<HediffDef> hediffRequiredAny;

		// Token: 0x04003E6A RID: 15978
		public List<PreceptDef> nullifyingPrecepts;

		// Token: 0x04003E6B RID: 15979
		public List<CompProperties> requiredComps;

		// Token: 0x04003E6C RID: 15980
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04003E6D RID: 15981
		public bool showOnNeedList = true;

		// Token: 0x04003E6E RID: 15982
		public float baseLevel = 0.5f;

		// Token: 0x04003E6F RID: 15983
		public bool major;

		// Token: 0x04003E70 RID: 15984
		public int listPriority;

		// Token: 0x04003E71 RID: 15985
		[NoTranslate]
		public string tutorHighlightTag;

		// Token: 0x04003E72 RID: 15986
		public bool showForCaravanMembers;

		// Token: 0x04003E73 RID: 15987
		public bool scaleBar;

		// Token: 0x04003E74 RID: 15988
		public bool showUnitTicks;

		// Token: 0x04003E75 RID: 15989
		public float fallPerDay = 0.5f;

		// Token: 0x04003E76 RID: 15990
		public float seekerRisePerHour;

		// Token: 0x04003E77 RID: 15991
		public float seekerFallPerHour;

		// Token: 0x04003E78 RID: 15992
		public bool freezeWhileSleeping;

		// Token: 0x04003E79 RID: 15993
		public bool freezeInMentalState;
	}
}
