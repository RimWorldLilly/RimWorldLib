using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001351 RID: 4945
	public class LayoutParms
	{
		// Token: 0x06007A64 RID: 31332 RVA: 0x002B9E81 File Offset: 0x002B8081
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "weight");
		}

		// Token: 0x04004D58 RID: 19800
		public LayoutDef def;

		// Token: 0x04004D59 RID: 19801
		public float weight = 1f;
	}
}
