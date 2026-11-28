using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001350 RID: 4944
	public class PrefabParms
	{
		// Token: 0x06007A62 RID: 31330 RVA: 0x002B9E50 File Offset: 0x002B8050
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "weight");
		}

		// Token: 0x04004D55 RID: 19797
		public PrefabDef def;

		// Token: 0x04004D56 RID: 19798
		public float weight = 1f;

		// Token: 0x04004D57 RID: 19799
		public IntRange minMaxRange = IntRange.Invalid;
	}
}
