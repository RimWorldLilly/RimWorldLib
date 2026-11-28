using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4A RID: 3914
	public class ColorDef : Def
	{
		// Token: 0x04003A72 RID: 14962
		public Color color;

		// Token: 0x04003A73 RID: 14963
		public ColorType colorType;

		// Token: 0x04003A74 RID: 14964
		public int displayOrder = int.MaxValue;

		// Token: 0x04003A75 RID: 14965
		public bool displayInStylingStationUI = true;

		// Token: 0x04003A76 RID: 14966
		public bool randomlyPickable = true;
	}
}
