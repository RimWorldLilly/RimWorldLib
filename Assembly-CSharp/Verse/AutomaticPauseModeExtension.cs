using System;

namespace Verse
{
	// Token: 0x0200071A RID: 1818
	public static class AutomaticPauseModeExtension
	{
		// Token: 0x060039B6 RID: 14774 RVA: 0x0016FA78 File Offset: 0x0016DC78
		public static string ToStringHuman(this AutomaticPauseMode mode)
		{
			switch (mode)
			{
			case AutomaticPauseMode.Never:
				return "AutomaticPauseMode_Never".Translate();
			case AutomaticPauseMode.MajorThreat:
				return "AutomaticPauseMode_MajorThreat".Translate();
			case AutomaticPauseMode.AnyThreat:
				return "AutomaticPauseMode_AnyThreat".Translate();
			case AutomaticPauseMode.AnyLetter:
				return "AutomaticPauseMode_AnyLetter".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
