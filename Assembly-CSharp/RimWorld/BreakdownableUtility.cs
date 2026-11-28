using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001840 RID: 6208
	public static class BreakdownableUtility
	{
		// Token: 0x06009B74 RID: 39796 RVA: 0x00380810 File Offset: 0x0037EA10
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
