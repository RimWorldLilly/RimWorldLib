using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F37 RID: 3895
	public class WeatherCommonalityRecord
	{
		// Token: 0x06006497 RID: 25751 RVA: 0x0023ECCC File Offset: 0x0023CECC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040039DC RID: 14812
		public WeatherDef weather;

		// Token: 0x040039DD RID: 14813
		public float commonality;
	}
}
