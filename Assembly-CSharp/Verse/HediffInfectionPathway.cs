using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020006FA RID: 1786
	public class HediffInfectionPathway
	{
		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x060038C6 RID: 14534 RVA: 0x0016C975 File Offset: 0x0016AB75
		public InfectionPathwayDef PathwayDef
		{
			get
			{
				return this.pathwayDef;
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060038C7 RID: 14535 RVA: 0x0016C97D File Offset: 0x0016AB7D
		public string Explanation
		{
			get
			{
				return this.explanation;
			}
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x0016C988 File Offset: 0x0016AB88
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.Name == "li")
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "pathwayDef", xmlRoot.FirstChild.Value, null, null, null);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "pathwayDef", xmlRoot.Name, null, null, null);
			if (xmlRoot.HasChildNodes)
			{
				this.explanation = ParseHelper.FromString<string>(xmlRoot.FirstChild.Value);
			}
		}

		// Token: 0x040024F4 RID: 9460
		private InfectionPathwayDef pathwayDef;

		// Token: 0x040024F5 RID: 9461
		[MustTranslate]
		private string explanation;
	}
}
