using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2E RID: 3886
	public class BackstoryTrait
	{
		// Token: 0x06006473 RID: 25715 RVA: 0x0023E26F File Offset: 0x0023C46F
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.Name, null, null, null);
			if (xmlRoot.HasChildNodes)
			{
				this.degree = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
				return;
			}
			this.degree = 0;
		}

		// Token: 0x040039AB RID: 14763
		public TraitDef def;

		// Token: 0x040039AC RID: 14764
		public int degree;
	}
}
