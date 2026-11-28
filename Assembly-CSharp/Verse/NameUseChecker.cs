using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000298 RID: 664
	public static class NameUseChecker
	{
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x00072C30 File Offset: 0x00070E30
		public static IEnumerable<Name> AllPawnsNamesEverUsed
		{
			get
			{
				return new NameUseChecker.<get_AllPawnsNamesEverUsed>d__1(-2);
			}
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00072C3C File Offset: 0x00070E3C
		public static bool NameWordIsUsed(string singleName)
		{
			foreach (Name name in NameUseChecker.AllPawnsNamesEverUsed)
			{
				NameTriple nameTriple = name as NameTriple;
				if (nameTriple != null && (singleName == nameTriple.First || singleName == nameTriple.Nick || singleName == nameTriple.Last))
				{
					return true;
				}
				NameSingle nameSingle = name as NameSingle;
				if (nameSingle != null && nameSingle.Name == singleName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x00072CDC File Offset: 0x00070EDC
		public static bool NameSingleIsUsed(string candidate)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				NameSingle nameSingle = pawn.Name as NameSingle;
				if (nameSingle != null && nameSingle.Name == candidate)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x00072D4C File Offset: 0x00070F4C
		public static bool XenotypeNameIsUsed(string candidate)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn.genes != null && pawn.genes.UniqueXenotype)
				{
					string xenotypeName = pawn.genes.xenotypeName;
					if (candidate == xenotypeName)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
