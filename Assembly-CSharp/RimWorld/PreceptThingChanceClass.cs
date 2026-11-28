using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC6 RID: 4038
	public class PreceptThingChanceClass
	{
		// Token: 0x06006680 RID: 26240 RVA: 0x00246214 File Offset: 0x00244414
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot, null, null, null);
			this.chance = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04003F0B RID: 16139
		public ThingDef def;

		// Token: 0x04003F0C RID: 16140
		public float chance;
	}
}
