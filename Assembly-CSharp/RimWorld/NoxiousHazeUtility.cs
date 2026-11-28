using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200114C RID: 4428
	public static class NoxiousHazeUtility
	{
		// Token: 0x06006F30 RID: 28464 RVA: 0x0026EAF0 File Offset: 0x0026CCF0
		public static bool IsExposedToNoxiousHaze(Thing thing)
		{
			return thing.SpawnedOrAnyParentSpawned && NoxiousHazeUtility.IsExposedToNoxiousHaze(thing, thing.PositionHeld, thing.MapHeld);
		}

		// Token: 0x06006F31 RID: 28465 RVA: 0x0026EB10 File Offset: 0x0026CD10
		public static bool IsExposedToNoxiousHaze(Thing thing, IntVec3 cell, Map map)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			GameCondition activeCondition = map.gameConditionManager.GetActiveCondition(GameConditionDefOf.NoxiousHaze);
			if (activeCondition == null || activeCondition.HiddenByOtherCondition(map))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.kindDef.immuneToGameConditionEffects)
			{
				return false;
			}
			if (thing.def.category == ThingCategory.Item)
			{
				return !cell.Roofed(map);
			}
			return !cell.Roofed(map) || cell.GetRoom(map).PsychologicallyOutdoors;
		}

		// Token: 0x06006F32 RID: 28466 RVA: 0x0026EB90 File Offset: 0x0026CD90
		public static bool TryGetNoxiousHazeMTB(PlanetTile tile, out float mtb)
		{
			float num = WorldPollutionUtility.CalculateNearbyPollutionScore(tile);
			if (num > 0f && num >= GameConditionDefOf.NoxiousHaze.minNearbyPollution)
			{
				mtb = GameConditionDefOf.NoxiousHaze.mtbOverNearbyPollutionCurve.Evaluate(num);
				return true;
			}
			mtb = 0f;
			return false;
		}
	}
}
