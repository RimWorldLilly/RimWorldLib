using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3D RID: 3901
	public class FishChance
	{
		// Token: 0x060064A2 RID: 25762 RVA: 0x0023EDCA File Offset: 0x0023CFCA
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "fishDef", xmlRoot, null, null, null);
			this.chance = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040039E9 RID: 14825
		public ThingDef fishDef;

		// Token: 0x040039EA RID: 14826
		public float chance = 1f;
	}
}
