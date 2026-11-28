using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F43 RID: 3907
	public class PawnKindDefCount
	{
		// Token: 0x060064C3 RID: 25795 RVA: 0x0023FA00 File Offset: 0x0023DC00
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "kindDef", "count");
		}

		// Token: 0x04003A4E RID: 14926
		public PawnKindDef kindDef;

		// Token: 0x04003A4F RID: 14927
		public int count;
	}
}
