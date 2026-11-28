using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3A RID: 3898
	public class BiomeAnimalRecord
	{
		// Token: 0x0600649D RID: 25757 RVA: 0x0023ED59 File Offset: 0x0023CF59
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040039E2 RID: 14818
		public PawnKindDef animal;

		// Token: 0x040039E3 RID: 14819
		public float commonality;
	}
}
