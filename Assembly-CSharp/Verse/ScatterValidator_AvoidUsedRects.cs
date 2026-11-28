using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002DA RID: 730
	public class ScatterValidator_AvoidUsedRects : ScattererValidator
	{
		// Token: 0x06001636 RID: 5686 RVA: 0x000853A0 File Offset: 0x000835A0
		public override bool Allows(IntVec3 c, Map map)
		{
			List<CellRect> list;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				return true;
			}
			CellRect cellRect = new CellRect(c.x, c.z, this.size.x, this.size.z);
			foreach (CellRect other in list)
			{
				if (cellRect.Overlaps(other))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001337 RID: 4919
		public IntVec2 size = new IntVec2(1, 1);
	}
}
