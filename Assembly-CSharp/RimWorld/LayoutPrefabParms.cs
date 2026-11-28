using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8E RID: 3982
	public class LayoutPrefabParms
	{
		// Token: 0x060065B3 RID: 26035 RVA: 0x002432C8 File Offset: 0x002414C8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04003CA8 RID: 15528
		public PrefabDef def;

		// Token: 0x04003CA9 RID: 15529
		public IntRange countRange = IntRange.Invalid;

		// Token: 0x04003CAA RID: 15530
		public IntRange minMaxRange = new IntRange(-1, int.MaxValue);

		// Token: 0x04003CAB RID: 15531
		public bool ensureNoBlocks = true;

		// Token: 0x04003CAC RID: 15532
		public FloatRange countPerHundredCells = new FloatRange(1f, 2f);

		// Token: 0x04003CAD RID: 15533
		public FloatRange countPerTenEdgeCells = new FloatRange(1f, 1f);

		// Token: 0x04003CAE RID: 15534
		public RotationDirection rotOffset;
	}
}
