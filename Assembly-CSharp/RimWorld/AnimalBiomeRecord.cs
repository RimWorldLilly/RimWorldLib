using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3B RID: 3899
	public class AnimalBiomeRecord
	{
		// Token: 0x0600649F RID: 25759 RVA: 0x0023ED88 File Offset: 0x0023CF88
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040039E4 RID: 14820
		public BiomeDef biome;

		// Token: 0x040039E5 RID: 14821
		public float commonality;
	}
}
