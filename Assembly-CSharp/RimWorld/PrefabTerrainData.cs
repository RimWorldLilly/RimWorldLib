using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F98 RID: 3992
	public class PrefabTerrainData
	{
		// Token: 0x060065CC RID: 26060 RVA: 0x00243983 File Offset: 0x00241B83
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04003CF2 RID: 15602
		public TerrainDef def;

		// Token: 0x04003CF3 RID: 15603
		public float chance = 1f;

		// Token: 0x04003CF4 RID: 15604
		public ColorDef color;

		// Token: 0x04003CF5 RID: 15605
		public List<CellRect> rects;
	}
}
