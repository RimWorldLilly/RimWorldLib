using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8D RID: 3981
	public class LayoutFillInteriorParms
	{
		// Token: 0x060065B1 RID: 26033 RVA: 0x0024328A File Offset: 0x0024148A
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04003CA0 RID: 15520
		public ThingDef def;

		// Token: 0x04003CA1 RID: 15521
		public ThingDef stuff;

		// Token: 0x04003CA2 RID: 15522
		public Rot4? fixedRot;

		// Token: 0x04003CA3 RID: 15523
		public bool alignWithRect;

		// Token: 0x04003CA4 RID: 15524
		public int contractedBy = 2;

		// Token: 0x04003CA5 RID: 15525
		public bool snapToGrid;

		// Token: 0x04003CA6 RID: 15526
		public IntRange countRange = IntRange.Invalid;

		// Token: 0x04003CA7 RID: 15527
		public FloatRange thingsPerHundredCells = new FloatRange(2f, 3f);
	}
}
