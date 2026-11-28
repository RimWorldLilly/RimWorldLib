using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBC RID: 7612
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Predator : PawnColumnWorker_Icon
	{
		// Token: 0x0600C5D9 RID: 50649 RVA: 0x00489976 File Offset: 0x00487B76
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (pawn.RaceProps.predator)
			{
				return PawnColumnWorker_Predator.Icon;
			}
			return null;
		}

		// Token: 0x0600C5DA RID: 50650 RVA: 0x0048998C File Offset: 0x00487B8C
		protected override string GetIconTip(Pawn pawn)
		{
			return "IsPredator".Translate();
		}

		// Token: 0x040070E4 RID: 28900
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Predator", true);
	}
}
