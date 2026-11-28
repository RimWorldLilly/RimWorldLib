using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E59 RID: 3673
	public class ThoughtWorker_LookChangeDesired : ThoughtWorker
	{
		// Token: 0x060061C5 RID: 25029 RVA: 0x00233EC9 File Offset: 0x002320C9
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.style == null || !ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return p.style.LookChangeDesired;
		}
	}
}
