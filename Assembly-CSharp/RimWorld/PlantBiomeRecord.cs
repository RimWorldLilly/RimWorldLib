using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F39 RID: 3897
	public class PlantBiomeRecord
	{
		// Token: 0x0600649B RID: 25755 RVA: 0x0023ED2A File Offset: 0x0023CF2A
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040039E0 RID: 14816
		public BiomeDef biome;

		// Token: 0x040039E1 RID: 14817
		public float commonality;
	}
}
