using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D65 RID: 3429
	public class ThinkNode_ConditionalMustKeepLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06005F4C RID: 24396 RVA: 0x0022BE78 File Offset: 0x0022A078
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.CurJob == null || !pawn.GetPosture().Laying())
			{
				return false;
			}
			if (!pawn.Downed)
			{
				if (RestUtility.DisturbancePreventsLyingDown(pawn))
				{
					return false;
				}
				if (!pawn.CurJob.restUntilHealed || !HealthAIUtility.ShouldSeekMedicalRest(pawn))
				{
					if (!pawn.jobs.curDriver.asleep)
					{
						return false;
					}
					if (!pawn.CurJob.playerForced && RestUtility.TimetablePreventsLayDown(pawn))
					{
						return false;
					}
					Pawn_NeedsTracker needs = pawn.needs;
					if (((needs != null) ? needs.rest : null) != null)
					{
						if (pawn.needs.rest.CurLevel > 0.14f && ChildcareUtility.ShouldWakeUpToAutofeedUrgent(pawn))
						{
							return false;
						}
						if (pawn.needs.rest.CurLevel > 0.14f)
						{
							Need_Food food = pawn.needs.food;
							Thing thing;
							ThingDef thingDef;
							if (food != null && food.CurCategory >= HungerCategory.UrgentlyHungry && FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, false, out thing, out thingDef, true, true, false, false, true, false, false, false, false, false, false, FoodPreferability.Undefined))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}
	}
}
