using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFC RID: 3836
	public static class DrugCategoryExtension
	{
		// Token: 0x060063D8 RID: 25560 RVA: 0x0023C7BC File Offset: 0x0023A9BC
		public static bool IncludedIn(this DrugCategory lhs, DrugCategory rhs)
		{
			return lhs <= rhs;
		}

		// Token: 0x060063D9 RID: 25561 RVA: 0x0023C7C8 File Offset: 0x0023A9C8
		public static string GetLabel(this DrugCategory category)
		{
			switch (category)
			{
			case DrugCategory.None:
				return "DrugCategory_None".Translate();
			case DrugCategory.Medical:
				return "DrugCategory_Medical".Translate();
			case DrugCategory.Social:
				return "DrugCategory_Social".Translate();
			case DrugCategory.Hard:
				return "DrugCategory_Hard".Translate();
			}
			return "DrugCategory_Any".Translate();
		}
	}
}
