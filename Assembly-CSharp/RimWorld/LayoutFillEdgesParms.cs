using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8C RID: 3980
	public class LayoutFillEdgesParms
	{
		// Token: 0x060065AF RID: 26031 RVA: 0x0024322B File Offset: 0x0024142B
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04003C98 RID: 15512
		public ThingDef def;

		// Token: 0x04003C99 RID: 15513
		public ThingDef stuff;

		// Token: 0x04003C9A RID: 15514
		public int padding;

		// Token: 0x04003C9B RID: 15515
		public int contractedBy = 1;

		// Token: 0x04003C9C RID: 15516
		public IntRange countRange = IntRange.Invalid;

		// Token: 0x04003C9D RID: 15517
		public FloatRange groupsPerTenEdgeCells = new FloatRange(1f, 1f);

		// Token: 0x04003C9E RID: 15518
		public IntRange groupCountRange = new IntRange(1, 2);

		// Token: 0x04003C9F RID: 15519
		public RotationDirection rotOffset = RotationDirection.Opposite;
	}
}
