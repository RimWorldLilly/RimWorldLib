using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C55 RID: 3157
	public static class WorkGiverUtility
	{
		// Token: 0x0600599D RID: 22941 RVA: 0x0020F0F0 File Offset: 0x0020D2F0
		public static Job HaulStuffOffBillGiverJob(Pawn pawn, IBillGiver giver, Thing thingToIgnore)
		{
			foreach (IntVec3 c in giver.IngredientStackCells)
			{
				Thing thing = pawn.Map.thingGrid.ThingAt(c, ThingCategory.Item);
				if (thing != null && thing != thingToIgnore)
				{
					return HaulAIUtility.HaulAsideJobFor(pawn, thing);
				}
			}
			return null;
		}
	}
}
