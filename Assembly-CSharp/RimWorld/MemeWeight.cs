using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F57 RID: 3927
	public class MemeWeight
	{
		// Token: 0x06006505 RID: 25861 RVA: 0x00240765 File Offset: 0x0023E965
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "meme", xmlRoot.Name, null, null, null);
			this.selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04003AEC RID: 15084
		public MemeDef meme;

		// Token: 0x04003AED RID: 15085
		public float selectionWeight;
	}
}
