using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D2E RID: 7470
	public class FloatMenuOptionProvider_GhoulRest : FloatMenuOptionProvider
	{
		// Token: 0x1700236F RID: 9071
		// (get) Token: 0x0600C150 RID: 49488 RVA: 0x00467280 File Offset: 0x00465480
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002370 RID: 9072
		// (get) Token: 0x0600C151 RID: 49489 RVA: 0x00467283 File Offset: 0x00465483
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002371 RID: 9073
		// (get) Token: 0x0600C152 RID: 49490 RVA: 0x00467286 File Offset: 0x00465486
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C153 RID: 49491 RVA: 0x00467289 File Offset: 0x00465489
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return base.AppliesInt(context) && context.FirstSelectedPawn.IsMutant && context.FirstSelectedPawn.mutant.Def == MutantDefOf.Ghoul;
		}

		// Token: 0x0600C154 RID: 49492 RVA: 0x004672BC File Offset: 0x004654BC
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Building_Bed building_Bed = clickedThing as Building_Bed;
			if (building_Bed == null)
			{
				return null;
			}
			return building_Bed.GetBedRestFloatMenuOption(context.FirstSelectedPawn);
		}
	}
}
