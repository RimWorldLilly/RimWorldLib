using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001017 RID: 4119
	public class XenotypeChance
	{
		// Token: 0x060067C9 RID: 26569 RVA: 0x0024AA01 File Offset: 0x00248C01
		public XenotypeChance()
		{
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x0024AA09 File Offset: 0x00248C09
		public XenotypeChance(XenotypeDef xenotype, float chance)
		{
			this.xenotype = xenotype;
			this.chance = chance;
		}

		// Token: 0x060067CB RID: 26571 RVA: 0x0024AA1F File Offset: 0x00248C1F
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "xenotype", xmlRoot.Name, null, null, null);
			this.chance = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04004235 RID: 16949
		public XenotypeDef xenotype;

		// Token: 0x04004236 RID: 16950
		public float chance;
	}
}
