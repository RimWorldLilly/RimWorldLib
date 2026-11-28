using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F38 RID: 3896
	public class BiomePlantRecord
	{
		// Token: 0x06006499 RID: 25753 RVA: 0x0023ECFB File Offset: 0x0023CEFB
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040039DE RID: 14814
		public ThingDef plant;

		// Token: 0x040039DF RID: 14815
		public float commonality;
	}
}
