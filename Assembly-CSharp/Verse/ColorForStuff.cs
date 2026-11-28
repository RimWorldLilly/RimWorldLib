using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000120 RID: 288
	public class ColorForStuff
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0003625E File Offset: 0x0003445E
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x00036266 File Offset: 0x00034466
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x040004C2 RID: 1218
		private ThingDef stuff;

		// Token: 0x040004C3 RID: 1219
		private Color color = Color.white;
	}
}
