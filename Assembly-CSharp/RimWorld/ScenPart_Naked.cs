using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200178D RID: 6029
	public class ScenPart_Naked : ScenPart_PawnModifier
	{
		// Token: 0x0600963D RID: 38461 RVA: 0x0036933C File Offset: 0x0036753C
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsAreNaked".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x0600963E RID: 38462 RVA: 0x00369380 File Offset: 0x00367580
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600963F RID: 38463 RVA: 0x00369398 File Offset: 0x00367598
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}
	}
}
