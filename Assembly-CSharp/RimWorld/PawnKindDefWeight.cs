using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F44 RID: 3908
	public class PawnKindDefWeight
	{
		// Token: 0x060064C5 RID: 25797 RVA: 0x0023FA1B File Offset: 0x0023DC1B
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "kindDef", "weight");
		}

		// Token: 0x04003A50 RID: 14928
		public PawnKindDef kindDef;

		// Token: 0x04003A51 RID: 14929
		public float weight = 1f;
	}
}
