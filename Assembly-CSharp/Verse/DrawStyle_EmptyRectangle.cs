using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000256 RID: 598
	public class DrawStyle_EmptyRectangle : DrawStyle
	{
		// Token: 0x0600118B RID: 4491 RVA: 0x00064A44 File Offset: 0x00062C44
		public override void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer)
		{
			buffer.AddRange(CellRect.FromLimits(origin, target).EdgeCells);
		}
	}
}
