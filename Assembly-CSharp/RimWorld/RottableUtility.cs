using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195C RID: 6492
	public static class RottableUtility
	{
		// Token: 0x0600A2F1 RID: 41713 RVA: 0x003A3540 File Offset: 0x003A1740
		public static bool IsNotFresh(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x0600A2F2 RID: 41714 RVA: 0x003A3564 File Offset: 0x003A1764
		public static bool IsDessicated(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage == RotStage.Dessicated;
		}

		// Token: 0x0600A2F3 RID: 41715 RVA: 0x003A3588 File Offset: 0x003A1788
		public static RotStage GetRotStage(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			if (compRottable == null)
			{
				return RotStage.Fresh;
			}
			return compRottable.Stage;
		}

		// Token: 0x0600A2F4 RID: 41716 RVA: 0x003A35A7 File Offset: 0x003A17A7
		public static RotDrawMode GetRotDrawMode(RotStage stage)
		{
			switch (stage)
			{
			case RotStage.Rotting:
				return RotDrawMode.Rotting;
			case RotStage.Dessicated:
				return RotDrawMode.Dessicated;
			}
			return RotDrawMode.Fresh;
		}
	}
}
