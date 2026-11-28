using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F76 RID: 3958
	public class MTBByBiome
	{
		// Token: 0x06006568 RID: 25960 RVA: 0x00241CC8 File Offset: 0x0023FEC8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured MTBByBiome: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name, null, null, null);
			this.mtbDays = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04003BCE RID: 15310
		public BiomeDef biome;

		// Token: 0x04003BCF RID: 15311
		public float mtbDays;
	}
}
