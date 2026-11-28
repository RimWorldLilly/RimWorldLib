using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8A RID: 3978
	public class LayoutScatterTerrainParms
	{
		// Token: 0x060065AB RID: 26027 RVA: 0x0024318D File Offset: 0x0024138D
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04003C8D RID: 15501
		public TerrainDef def;

		// Token: 0x04003C8E RID: 15502
		public IntRange groupCount = IntRange.Invalid;

		// Token: 0x04003C8F RID: 15503
		public FloatRange groupsPerHundredCells = new FloatRange(0f, 2f);

		// Token: 0x04003C90 RID: 15504
		public IntRange itemsPerGroup = new IntRange(2, 4);

		// Token: 0x04003C91 RID: 15505
		public IntRange groupDistRange = new IntRange(2, 5);

		// Token: 0x04003C92 RID: 15506
		public int minGroups;
	}
}
