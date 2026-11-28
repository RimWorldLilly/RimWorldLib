using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EAD RID: 3757
	public class BookOutcomeProperties_GainResearch : BookOutcomeProperties
	{
		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x060062E5 RID: 25317 RVA: 0x00239520 File Offset: 0x00237720
		public override Type DoerClass
		{
			get
			{
				return typeof(ReadingOutcomeDoerGainResearch);
			}
		}

		// Token: 0x060062E6 RID: 25318 RVA: 0x0023952C File Offset: 0x0023772C
		public void PostLoad()
		{
			if (this.tab != null)
			{
				this.tabs.Add(new BookOutcomeProperties_GainResearch.BookTabItem
				{
					tab = this.tab
				});
			}
		}

		// Token: 0x0400368C RID: 13964
		public List<BookOutcomeProperties_GainResearch.BookResearchItem> include = new List<BookOutcomeProperties_GainResearch.BookResearchItem>();

		// Token: 0x0400368D RID: 13965
		public List<BookOutcomeProperties_GainResearch.BookResearchItem> exclude = new List<BookOutcomeProperties_GainResearch.BookResearchItem>();

		// Token: 0x0400368E RID: 13966
		public ResearchTabDef tab;

		// Token: 0x0400368F RID: 13967
		public List<BookOutcomeProperties_GainResearch.BookTabItem> tabs = new List<BookOutcomeProperties_GainResearch.BookTabItem>();

		// Token: 0x04003690 RID: 13968
		public bool ignoreZeroBaseCost = true;

		// Token: 0x04003691 RID: 13969
		public bool usesHiddenProjects;

		// Token: 0x02002F09 RID: 12041
		public class BookResearchItem
		{
			// Token: 0x06011C27 RID: 72743 RVA: 0x006136AB File Offset: 0x006118AB
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "project", xmlRoot.Name, null, null, null);
			}

			// Token: 0x0400BEDE RID: 48862
			public ResearchProjectDef project;
		}

		// Token: 0x02002F0A RID: 12042
		public class BookTabItem
		{
			// Token: 0x06011C29 RID: 72745 RVA: 0x006136C9 File Offset: 0x006118C9
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "tab", xmlRoot.Name, null, null, null);
			}

			// Token: 0x0400BEDF RID: 48863
			public ResearchTabDef tab;
		}
	}
}
