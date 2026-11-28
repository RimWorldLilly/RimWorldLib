using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBB RID: 7611
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_MentalState : PawnColumnWorker_Icon
	{
		// Token: 0x0600C5D5 RID: 50645 RVA: 0x004898F7 File Offset: 0x00487AF7
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (!pawn.InMentalState)
			{
				return null;
			}
			if (!pawn.InAggroMentalState)
			{
				return PawnColumnWorker_MentalState.IconNonAggro;
			}
			return PawnColumnWorker_MentalState.IconAggro;
		}

		// Token: 0x0600C5D6 RID: 50646 RVA: 0x00489916 File Offset: 0x00487B16
		protected override string GetIconTip(Pawn pawn)
		{
			return pawn.InMentalState ? "IsInMentalState".Translate(pawn.MentalState.def.LabelCap) : null;
		}

		// Token: 0x040070E2 RID: 28898
		private static readonly Texture2D IconNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		// Token: 0x040070E3 RID: 28899
		private static readonly Texture2D IconAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);
	}
}
