using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000117 RID: 279
	public class ApparelLayerDef : Def
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x00035A25 File Offset: 0x00033C25
		public bool IsUtilityLayer
		{
			get
			{
				return this == ApparelLayerDefOf.Belt;
			}
		}

		// Token: 0x04000483 RID: 1155
		public int drawOrder;
	}
}
