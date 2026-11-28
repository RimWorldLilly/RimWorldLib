using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace Verse.Glow
{
	// Token: 0x020007B5 RID: 1973
	public struct GlowCellComparer : IComparer<IntVec3>
	{
		// Token: 0x06003F87 RID: 16263 RVA: 0x00196B6E File Offset: 0x00194D6E
		public GlowCellComparer(UnsafeList<GlowCell> area, in GlowLight light)
		{
			this.area = area;
			this.light = light;
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x00196B84 File Offset: 0x00194D84
		public int Compare(IntVec3 a, IntVec3 b)
		{
			int index = this.light.DeltaToLocalIndex(a);
			int index2 = this.light.DeltaToLocalIndex(b);
			return this.area[index].intDist.CompareTo(this.area[index2].intDist);
		}

		// Token: 0x04002A08 RID: 10760
		private UnsafeList<GlowCell> area;

		// Token: 0x04002A09 RID: 10761
		private GlowLight light;
	}
}
