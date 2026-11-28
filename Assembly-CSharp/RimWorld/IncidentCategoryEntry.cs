using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E3 RID: 4579
	public class IncidentCategoryEntry
	{
		// Token: 0x0600720D RID: 29197 RVA: 0x0027EBA4 File Offset: 0x0027CDA4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name, null, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400481D RID: 18461
		public IncidentCategoryDef category;

		// Token: 0x0400481E RID: 18462
		public float weight;
	}
}
