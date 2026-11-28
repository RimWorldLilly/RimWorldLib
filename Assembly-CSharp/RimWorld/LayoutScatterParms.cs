using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F89 RID: 3977
	public class LayoutScatterParms
	{
		// Token: 0x060065A9 RID: 26025 RVA: 0x0024312F File Offset: 0x0024132F
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04003C86 RID: 15494
		public ThingDef def;

		// Token: 0x04003C87 RID: 15495
		public ThingDef stuff;

		// Token: 0x04003C88 RID: 15496
		public IntRange groupCount = IntRange.Invalid;

		// Token: 0x04003C89 RID: 15497
		public FloatRange groupsPerHundredCells = new FloatRange(0f, 2f);

		// Token: 0x04003C8A RID: 15498
		public IntRange itemsPerGroup = new IntRange(2, 4);

		// Token: 0x04003C8B RID: 15499
		public IntRange groupDistRange = new IntRange(2, 5);

		// Token: 0x04003C8C RID: 15500
		public int minGroups;
	}
}
