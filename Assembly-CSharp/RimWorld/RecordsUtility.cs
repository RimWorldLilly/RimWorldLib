using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F5 RID: 5365
	public static class RecordsUtility
	{
		// Token: 0x060083A8 RID: 33704 RVA: 0x002F5720 File Offset: 0x002F3920
		public static void Notify_PawnKilled(Pawn killed, Pawn killer)
		{
			killer.records.Increment(RecordDefOf.Kills);
			RaceProperties raceProps = killed.RaceProps;
			if (ModsConfig.AnomalyActive && (killed.IsSubhuman || raceProps.IsAnomalyEntity))
			{
				killer.records.Increment(RecordDefOf.KillsEntities);
			}
			else if (raceProps.Humanlike)
			{
				killer.records.Increment(RecordDefOf.KillsHumanlikes);
			}
			if (raceProps.Animal)
			{
				killer.records.Increment(RecordDefOf.KillsAnimals);
			}
			if (raceProps.IsMechanoid)
			{
				killer.records.Increment(RecordDefOf.KillsMechanoids);
			}
		}

		// Token: 0x060083A9 RID: 33705 RVA: 0x002F57B8 File Offset: 0x002F39B8
		public static void Notify_PawnDowned(Pawn downed, Pawn instigator)
		{
			instigator.records.Increment(RecordDefOf.PawnsDowned);
			RaceProperties raceProps = downed.RaceProps;
			if (ModsConfig.AnomalyActive && (downed.IsSubhuman || raceProps.IsAnomalyEntity))
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedEntities);
			}
			else if (raceProps.Humanlike)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedHumanlikes);
			}
			if (raceProps.Animal)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedAnimals);
			}
			if (raceProps.IsMechanoid)
			{
				instigator.records.Increment(RecordDefOf.PawnsDownedMechanoids);
			}
		}

		// Token: 0x060083AA RID: 33706 RVA: 0x002F5850 File Offset: 0x002F3A50
		public static void Notify_BillDone(Pawn billDoer, List<Thing> products)
		{
			for (int i = 0; i < products.Count; i++)
			{
				for (int j = 0; j < products[i].stackCount; j++)
				{
					if (products[i].def.IsNutritionGivingIngestible && products[i].def.ingestible.preferability >= FoodPreferability.MealAwful)
					{
						billDoer.records.Increment(RecordDefOf.MealsCooked);
					}
					else if (RecordsUtility.ShouldIncrementThingsCrafted(products[i]))
					{
						billDoer.records.Increment(RecordDefOf.ThingsCrafted);
					}
				}
			}
		}

		// Token: 0x060083AB RID: 33707 RVA: 0x002F58E4 File Offset: 0x002F3AE4
		private static bool ShouldIncrementThingsCrafted(Thing crafted)
		{
			return crafted.def.IsApparel || crafted.def.IsWeapon || crafted.def.HasComp(typeof(CompArt)) || crafted.def.HasComp(typeof(CompQuality));
		}
	}
}
