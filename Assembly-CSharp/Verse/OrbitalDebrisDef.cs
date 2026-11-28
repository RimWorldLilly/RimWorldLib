using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000197 RID: 407
	public class OrbitalDebrisDef : Def
	{
		// Token: 0x040009E9 RID: 2537
		public List<OrbitalDebrisDef.DebrisGraphic> graphics = new List<OrbitalDebrisDef.DebrisGraphic>();

		// Token: 0x02002548 RID: 9544
		public class DebrisGraphic
		{
			// Token: 0x040098AD RID: 39085
			public GraphicData graphicData;

			// Token: 0x040098AE RID: 39086
			public float weighting = 1f;

			// Token: 0x040098AF RID: 39087
			public int order;

			// Token: 0x040098B0 RID: 39088
			public float parallaxPer10Cells = 1f;
		}
	}
}
