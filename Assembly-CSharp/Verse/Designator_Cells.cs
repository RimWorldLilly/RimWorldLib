using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005AF RID: 1455
	public abstract class Designator_Cells : Designator
	{
		// Token: 0x06002C44 RID: 11332 RVA: 0x0010BCBD File Offset: 0x00109EBD
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
