using System;
using System.Xml;

namespace Verse
{
	// Token: 0x020000BE RID: 190
	public class DamageFactor
	{
		// Token: 0x06000775 RID: 1909 RVA: 0x0002BA5F File Offset: 0x00029C5F
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "damageDef", xmlRoot.Name, null, null, null);
			this.factor = (xmlRoot.HasChildNodes ? ParseHelper.FromString<float>(xmlRoot.FirstChild.Value) : 1f);
		}

		// Token: 0x0400033F RID: 831
		public DamageDef damageDef;

		// Token: 0x04000340 RID: 832
		public float factor = 1f;
	}
}
