using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000258 RID: 600
	public class DrawStyle_FilledRectangle : DrawStyle
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x00064BB0 File Offset: 0x00062DB0
		public override bool CanHaveDuplicates
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00064BB4 File Offset: 0x00062DB4
		public override void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer)
		{
			buffer.AddRange(CellRect.FromLimits(origin, target).Cells);
		}
	}
}
