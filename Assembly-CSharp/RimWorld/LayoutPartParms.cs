using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8F RID: 3983
	public class LayoutPartParms
	{
		// Token: 0x060065B5 RID: 26037 RVA: 0x00243338 File Offset: 0x00241538
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "chance");
		}

		// Token: 0x04003CAF RID: 15535
		public RoomPartDef def;

		// Token: 0x04003CB0 RID: 15536
		public float chance = 1f;

		// Token: 0x04003CB1 RID: 15537
		public IntRange threatPointsRange = IntRange.Invalid;

		// Token: 0x04003CB2 RID: 15538
		public IntRange countRange = IntRange.Invalid;
	}
}
