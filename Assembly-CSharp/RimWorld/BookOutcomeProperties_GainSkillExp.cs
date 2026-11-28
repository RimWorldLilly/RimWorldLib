using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EAF RID: 3759
	public class BookOutcomeProperties_GainSkillExp : BookOutcomeProperties
	{
		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x060062F7 RID: 25335 RVA: 0x00239CB3 File Offset: 0x00237EB3
		public override Type DoerClass
		{
			get
			{
				return typeof(BookOutcomeDoerGainSkillExp);
			}
		}

		// Token: 0x04003695 RID: 13973
		public List<BookOutcomeProperties_GainSkillExp.BookStatReward> skills = new List<BookOutcomeProperties_GainSkillExp.BookStatReward>();

		// Token: 0x02002F0E RID: 12046
		public class BookStatReward
		{
			// Token: 0x06011C3C RID: 72764 RVA: 0x006138FF File Offset: 0x00611AFF
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
			}

			// Token: 0x0400BEEA RID: 48874
			public SkillDef skill;
		}
	}
}
