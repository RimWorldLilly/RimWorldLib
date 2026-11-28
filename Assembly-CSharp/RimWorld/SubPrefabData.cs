using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F97 RID: 3991
	public class SubPrefabData
	{
		// Token: 0x060065CA RID: 26058 RVA: 0x0024395D File Offset: 0x00241B5D
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "position");
		}

		// Token: 0x04003CED RID: 15597
		public PrefabDef def;

		// Token: 0x04003CEE RID: 15598
		public float chance = 1f;

		// Token: 0x04003CEF RID: 15599
		public IntVec3 position;

		// Token: 0x04003CF0 RID: 15600
		public List<IntVec3> positions;

		// Token: 0x04003CF1 RID: 15601
		public RotationDirection relativeRotation;
	}
}
