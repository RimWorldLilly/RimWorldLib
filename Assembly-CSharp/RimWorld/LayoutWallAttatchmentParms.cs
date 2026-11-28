using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8B RID: 3979
	public class LayoutWallAttatchmentParms
	{
		// Token: 0x060065AD RID: 26029 RVA: 0x002431E9 File Offset: 0x002413E9
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04003C93 RID: 15507
		public ThingDef def;

		// Token: 0x04003C94 RID: 15508
		public ThingDef stuff;

		// Token: 0x04003C95 RID: 15509
		public IntRange countRange = IntRange.Invalid;

		// Token: 0x04003C96 RID: 15510
		public FloatRange thingsPer10EdgeCells = new FloatRange(1f, 1f);

		// Token: 0x04003C97 RID: 15511
		public float spawnChancePerPosition = 1f;
	}
}
