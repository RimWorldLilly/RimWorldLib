using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014B6 RID: 5302
	public static class NociosphereUtility
	{
		// Token: 0x060081C5 RID: 33221 RVA: 0x002E4F20 File Offset: 0x002E3120
		public static Thing FindTarget(Pawn pawn)
		{
			List<Thing> source = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn);
			NociosphereUtility.CheckForTargets(pawn, source, NociosphereUtility.targetsTmp, new Func<Thing, bool>(NociosphereUtility.IsPawnTarget));
			if (NociosphereUtility.targetsTmp.Empty<Thing>())
			{
				List<Thing> source2 = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				NociosphereUtility.CheckForTargets(pawn, source2, NociosphereUtility.targetsTmp, new Func<Thing, bool>(NociosphereUtility.IsBuildingTarget));
			}
			Thing result = null;
			if (NociosphereUtility.targetsTmp.Any<Thing>())
			{
				result = NociosphereUtility.targetsTmp.RandomElement<Thing>();
			}
			NociosphereUtility.targetsTmp.Clear();
			return result;
		}

		// Token: 0x060081C6 RID: 33222 RVA: 0x002E4FB4 File Offset: 0x002E31B4
		public static void SkipTo(Pawn pawn, IntVec3 cell)
		{
			Ability ability = pawn.abilities.GetAbility(AbilityDefOf.EntitySkip, false);
			ability.ResetCooldown();
			if (pawn.IsOnHoldingPlatform)
			{
				Building_HoldingPlatform building_HoldingPlatform = (Building_HoldingPlatform)pawn.ParentHolder;
				Thing thing;
				building_HoldingPlatform.innerContainer.TryDrop(pawn, building_HoldingPlatform.Position, building_HoldingPlatform.Map, ThingPlaceMode.Direct, 1, out thing, null, null);
				CompHoldingPlatformTarget compHoldingPlatformTarget = pawn.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget != null)
				{
					compHoldingPlatformTarget.targetHolder = null;
				}
			}
			Job job = ability.GetJob(pawn, cell);
			pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
		}

		// Token: 0x060081C7 RID: 33223 RVA: 0x002E505C File Offset: 0x002E325C
		private static void CheckForTargets(Pawn pawn, List<Thing> source, List<Thing> output, Func<Thing, bool> validator)
		{
			output.Clear();
			for (int i = 0; i < source.Count; i++)
			{
				if (pawn.Position.DistanceToSquared(source[i].Position) <= NociosphereUtility.NociosphereLOSSqr && validator(source[i]) && GenSight.LineOfSightToThing(pawn.Position, source[i], pawn.Map, true, null))
				{
					output.Add(source[i]);
				}
			}
		}

		// Token: 0x060081C8 RID: 33224 RVA: 0x002E50D8 File Offset: 0x002E32D8
		private static bool IsPawnTarget(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && !pawn.Downed && pawn.kindDef != PawnKindDefOf.Nociosphere;
		}

		// Token: 0x060081C9 RID: 33225 RVA: 0x002E510C File Offset: 0x002E330C
		private static bool IsBuildingTarget(Thing thing)
		{
			Building building = thing as Building;
			return building != null && building.def.building.IsTurret;
		}

		// Token: 0x0400507A RID: 20602
		public const float NociosphereLOS = 24.9f;

		// Token: 0x0400507B RID: 20603
		public static readonly int NociosphereLOSSqr = Mathf.FloorToInt(620.01f);

		// Token: 0x0400507C RID: 20604
		private static readonly List<Thing> targetsTmp = new List<Thing>();
	}
}
