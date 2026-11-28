using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F84 RID: 3972
	public class ComplexThreat
	{
		// Token: 0x060065A0 RID: 26016 RVA: 0x00242F86 File Offset: 0x00241186
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "selectionWeight");
		}

		// Token: 0x04003C5A RID: 15450
		public ComplexThreatDef def;

		// Token: 0x04003C5B RID: 15451
		public float chancePerComplex = 1f;

		// Token: 0x04003C5C RID: 15452
		public int maxPerComplex = int.MaxValue;

		// Token: 0x04003C5D RID: 15453
		public int maxPerRoom = 1;

		// Token: 0x04003C5E RID: 15454
		public float selectionWeight = 1f;
	}
}
