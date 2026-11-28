using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200239D RID: 9117
	public static class ForagedFoodPerDayCalculator
	{
		// Token: 0x0600E2E1 RID: 58081 RVA: 0x00545BDC File Offset: 0x00543DDC
		[return: TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		public static ValueTuple<ThingDef, float> ForagedFoodPerDay(List<Pawn> pawns, BiomeDef biome, Faction faction, bool caravanMovingNow, bool caravanNightResting, StringBuilder explanation = null)
		{
			float foragedFoodCountPerInterval = ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(pawns, biome, faction, explanation);
			float progressPerTick = ForagedFoodPerDayCalculator.GetProgressPerTick(caravanMovingNow, caravanNightResting, explanation);
			float num = foragedFoodCountPerInterval * progressPerTick * 60000f;
			float num2;
			if (num != 0f)
			{
				num2 = num * biome.foragedFood.GetStatValueAbstract(StatDefOf.Nutrition, null);
			}
			else
			{
				num2 = 0f;
			}
			if (explanation != null)
			{
				explanation.AppendLine();
				explanation.AppendLine();
				TaggedString taggedString = "TotalNutrition".Translate() + ": " + num2.ToString("0.##") + " / " + "day".Translate();
				if (num2 > 0f)
				{
					taggedString += "\n= " + biome.LabelCap + ": " + biome.foragedFood.LabelCap + " x" + num.ToString("0.##") + " / " + "day".Translate();
				}
				explanation.Append(taggedString);
			}
			return new ValueTuple<ThingDef, float>(biome.foragedFood, num);
		}

		// Token: 0x0600E2E2 RID: 58082 RVA: 0x00545D08 File Offset: 0x00543F08
		public static float GetProgressPerTick(bool caravanMovingNow, bool caravanNightResting, StringBuilder explanation = null)
		{
			float num = 0.0001f;
			if (!caravanMovingNow && !caravanNightResting)
			{
				num *= 2f;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("CaravanNotMoving".Translate() + ": " + 2f.ToStringPercent());
				}
			}
			return num;
		}

		// Token: 0x0600E2E3 RID: 58083 RVA: 0x00545D64 File Offset: 0x00543F64
		public static float GetForagedFoodCountPerInterval(List<Pawn> pawns, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			float num = (biome.foragedFood != null) ? biome.forageability : 0f;
			if (explanation != null)
			{
				explanation.Append("ForagedNutritionPerDay".Translate() + ":");
			}
			float num2 = 0f;
			bool flag = false;
			int i = 0;
			int count = pawns.Count;
			while (i < count)
			{
				Pawn pawn = pawns[i];
				bool flag2;
				float baseForagedNutritionPerDay = ForagedFoodPerDayCalculator.GetBaseForagedNutritionPerDay(pawn, out flag2);
				if (!flag2)
				{
					num2 += baseForagedNutritionPerDay;
					flag = true;
					if (explanation != null)
					{
						explanation.AppendLine();
						explanation.Append("  - " + pawn.LabelShortCap + ": +" + baseForagedNutritionPerDay.ToString("0.##"));
					}
				}
				i++;
			}
			float num3 = num2;
			num2 /= 6f;
			if (explanation != null)
			{
				explanation.AppendLine();
				if (flag)
				{
					explanation.Append("  = " + num3.ToString("0.##"));
				}
				else
				{
					explanation.Append("  (" + "NoneCapable".Translate().ToLower() + ")");
				}
				explanation.AppendLine();
				explanation.AppendLine();
				explanation.Append("Biome".Translate() + ": x" + num.ToStringPercent() + " (" + biome.label + ")");
				if (faction.def.forageabilityFactor != 1f)
				{
					explanation.AppendLine();
					explanation.Append("  " + "FactionType".Translate() + ": " + faction.def.forageabilityFactor.ToStringPercent());
				}
			}
			num2 *= num;
			num2 *= faction.def.forageabilityFactor;
			if (biome.foragedFood != null)
			{
				return num2 / biome.foragedFood.ingestible.CachedNutrition;
			}
			return num2;
		}

		// Token: 0x0600E2E4 RID: 58084 RVA: 0x00545F68 File Offset: 0x00544168
		public static float GetBaseForagedNutritionPerDay(Pawn p, out bool skip)
		{
			if ((!p.IsAnimal && !p.IsFreeColonist) || p.InMentalState || p.Downed || p.CarriedByCaravan())
			{
				skip = true;
				return 0f;
			}
			skip = false;
			if (!StatDefOf.ForagedNutritionPerDay.Worker.IsDisabledFor(p))
			{
				return p.GetStatValue(StatDefOf.ForagedNutritionPerDay, true, -1);
			}
			return 0f;
		}

		// Token: 0x0600E2E5 RID: 58085 RVA: 0x00545FCE File Offset: 0x005441CE
		[return: TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		public static ValueTuple<ThingDef, float> ForagedFoodPerDay(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.ForagedFoodPerDay(caravan.PawnsListForReading, caravan.Biome, caravan.Faction, caravan.pather.MovingNow, caravan.NightResting, explanation);
		}

		// Token: 0x0600E2E6 RID: 58086 RVA: 0x00545FF9 File Offset: 0x005441F9
		public static float GetProgressPerTick(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.GetProgressPerTick(caravan.pather.MovingNow, caravan.NightResting, explanation);
		}

		// Token: 0x0600E2E7 RID: 58087 RVA: 0x00546012 File Offset: 0x00544212
		public static float GetForagedFoodCountPerInterval(Caravan caravan, StringBuilder explanation = null)
		{
			return ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(caravan.PawnsListForReading, caravan.Biome, caravan.Faction, explanation);
		}

		// Token: 0x0600E2E8 RID: 58088 RVA: 0x0054602C File Offset: 0x0054422C
		[return: TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		public static ValueTuple<ThingDef, float> ForagedFoodPerDay(List<TransferableOneWay> transferables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = 0; j < transferableOneWay.CountToTransfer; j++)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			ValueTuple<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600E2E9 RID: 58089 RVA: 0x005460B8 File Offset: 0x005442B8
		[return: TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		public static ValueTuple<ThingDef, float> ForagedFoodPerDayLeftAfterTransfer(List<TransferableOneWay> transferables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn)
				{
					for (int j = transferableOneWay.things.Count - 1; j >= transferableOneWay.CountToTransfer; j--)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add((Pawn)transferableOneWay.things[j]);
					}
				}
			}
			ValueTuple<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x0600E2EA RID: 58090 RVA: 0x0054614E File Offset: 0x0054434E
		[return: TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		public static ValueTuple<ThingDef, float> ForagedFoodPerDayLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, ForagedFoodPerDayCalculator.tmpThingCounts);
			ValueTuple<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpThingCounts, biome, faction, explanation);
			ForagedFoodPerDayCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600E2EB RID: 58091 RVA: 0x00546180 File Offset: 0x00544380
		[return: TupleElementNames(new string[]
		{
			"food",
			"perDay"
		})]
		public static ValueTuple<ThingDef, float> ForagedFoodPerDay(List<ThingCount> thingCounts, BiomeDef biome, Faction faction, StringBuilder explanation = null)
		{
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						ForagedFoodPerDayCalculator.tmpPawns.Add(pawn);
					}
				}
			}
			ValueTuple<ThingDef, float> result = ForagedFoodPerDayCalculator.ForagedFoodPerDay(ForagedFoodPerDayCalculator.tmpPawns, biome, faction, true, false, explanation);
			ForagedFoodPerDayCalculator.tmpPawns.Clear();
			return result;
		}

		// Token: 0x040090CE RID: 37070
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x040090CF RID: 37071
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x040090D0 RID: 37072
		private const float BaseProgressPerTick = 0.0001f;

		// Token: 0x040090D1 RID: 37073
		public const float NotMovingProgressFactor = 2f;
	}
}
