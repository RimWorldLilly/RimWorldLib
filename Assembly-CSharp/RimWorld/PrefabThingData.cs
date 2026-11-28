using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F96 RID: 3990
	public class PrefabThingData
	{
		// Token: 0x060065C8 RID: 26056 RVA: 0x00243929 File Offset: 0x00241B29
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04003CE0 RID: 15584
		public ThingDef def;

		// Token: 0x04003CE1 RID: 15585
		public ThingDef stuff;

		// Token: 0x04003CE2 RID: 15586
		public ColorDef colorDef;

		// Token: 0x04003CE3 RID: 15587
		public Color color;

		// Token: 0x04003CE4 RID: 15588
		public IntRange stackCountRange = IntRange.One;

		// Token: 0x04003CE5 RID: 15589
		public int hp;

		// Token: 0x04003CE6 RID: 15590
		public float chance = 1f;

		// Token: 0x04003CE7 RID: 15591
		public bool canOverrideData = true;

		// Token: 0x04003CE8 RID: 15592
		public QualityCategory? quality;

		// Token: 0x04003CE9 RID: 15593
		public IntVec3 position;

		// Token: 0x04003CEA RID: 15594
		public List<CellRect> rects;

		// Token: 0x04003CEB RID: 15595
		public List<IntVec3> positions;

		// Token: 0x04003CEC RID: 15596
		public RotationDirection relativeRotation;
	}
}
