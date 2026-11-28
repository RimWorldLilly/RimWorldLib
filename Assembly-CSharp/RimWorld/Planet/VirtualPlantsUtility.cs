using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200233B RID: 9019
	public static class VirtualPlantsUtility
	{
		// Token: 0x0600DED5 RID: 57045 RVA: 0x0052E502 File Offset: 0x0052C702
		public static bool CanEverEatVirtualPlants(Pawn p)
		{
			return p.RaceProps.Eats(FoodTypeFlags.Plant);
		}

		// Token: 0x0600DED6 RID: 57046 RVA: 0x0052E511 File Offset: 0x0052C711
		public static bool CanEatVirtualPlantsNow(Pawn p)
		{
			return VirtualPlantsUtility.CanEatVirtualPlants(p, GenTicks.TicksAbs);
		}

		// Token: 0x0600DED7 RID: 57047 RVA: 0x0052E520 File Offset: 0x0052C720
		public static bool CanEatVirtualPlants(Pawn p, int ticksAbs)
		{
			return p.Tile.Valid && !p.Dead && p.IsWorldPawn() && VirtualPlantsUtility.CanEverEatVirtualPlants(p) && VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(p.Tile, ticksAbs);
		}

		// Token: 0x0600DED8 RID: 57048 RVA: 0x0052E563 File Offset: 0x0052C763
		public static bool EnvironmentAllowsEatingVirtualPlantsNowAt(PlanetTile tile)
		{
			return VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, GenTicks.TicksAbs);
		}

		// Token: 0x0600DED9 RID: 57049 RVA: 0x0052E570 File Offset: 0x0052C770
		public static bool EnvironmentAllowsEatingVirtualPlantsAt(PlanetTile tile, int ticksAbs)
		{
			return Find.WorldGrid[tile].PrimaryBiome.hasVirtualPlants && GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile) >= 0f;
		}

		// Token: 0x0600DEDA RID: 57050 RVA: 0x0052E59C File Offset: 0x0052C79C
		public static void EatVirtualPlants(Pawn p)
		{
			float num = ThingDefOf.Plant_Grass.GetStatValueAbstract(StatDefOf.Nutrition, null) * VirtualPlantsUtility.VirtualPlantNutritionRandomFactor.RandomInRange;
			p.needs.food.CurLevel += num;
		}

		// Token: 0x0600DEDB RID: 57051 RVA: 0x0052E5E0 File Offset: 0x0052C7E0
		public static string GetVirtualPlantsStatusExplanationAt(PlanetTile tile, int ticksAbs)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (ticksAbs == GenTicks.TicksAbs)
			{
				stringBuilder.Append("AnimalsCanGrazeNow".Translate());
			}
			else if (ticksAbs > GenTicks.TicksAbs)
			{
				stringBuilder.Append("AnimalsWillBeAbleToGraze".Translate());
			}
			else
			{
				stringBuilder.Append("AnimalsCanGraze".Translate());
			}
			stringBuilder.Append(": ");
			bool flag = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, ticksAbs);
			stringBuilder.Append(flag ? "Yes".Translate() : "No".Translate());
			if (flag)
			{
				float? approxDaysUntilPossibleToGraze = VirtualPlantsUtility.GetApproxDaysUntilPossibleToGraze(tile, ticksAbs, true);
				if (approxDaysUntilPossibleToGraze != null)
				{
					stringBuilder.Append("\n" + "PossibleToGrazeFor".Translate(approxDaysUntilPossibleToGraze.Value.ToString("0.#")));
				}
				else
				{
					stringBuilder.Append("\n" + "PossibleToGrazeForever".Translate());
				}
			}
			else
			{
				if (!Find.WorldGrid[tile].PrimaryBiome.hasVirtualPlants)
				{
					stringBuilder.Append("\n" + "CantGrazeBecauseOfBiome".Translate(Find.WorldGrid[tile].PrimaryBiome.label));
				}
				float? approxDaysUntilPossibleToGraze2 = VirtualPlantsUtility.GetApproxDaysUntilPossibleToGraze(tile, ticksAbs, false);
				if (approxDaysUntilPossibleToGraze2 != null)
				{
					stringBuilder.Append("\n" + "CantGrazeBecauseOfTemp".Translate(approxDaysUntilPossibleToGraze2.Value.ToString("0.#")));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600DEDC RID: 57052 RVA: 0x0052E7A0 File Offset: 0x0052C9A0
		public static float? GetApproxDaysUntilPossibleToGraze(PlanetTile tile, int ticksAbs, bool untilNoLongerPossibleToGraze = false)
		{
			if (!untilNoLongerPossibleToGraze && !Find.WorldGrid[tile].PrimaryBiome.hasVirtualPlants)
			{
				return null;
			}
			float num = 0f;
			for (int i = 0; i < Mathf.CeilToInt(133.33334f); i++)
			{
				bool flag = VirtualPlantsUtility.EnvironmentAllowsEatingVirtualPlantsAt(tile, ticksAbs + (int)(num * 60000f));
				if ((!untilNoLongerPossibleToGraze && flag) || (untilNoLongerPossibleToGraze && !flag))
				{
					return new float?(num);
				}
				num += 0.45f;
			}
			return null;
		}

		// Token: 0x04008F13 RID: 36627
		private static readonly FloatRange VirtualPlantNutritionRandomFactor = new FloatRange(0.7f, 1f);
	}
}
