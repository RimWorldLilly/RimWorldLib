using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D3 RID: 5331
	public static class PawnInventoryGenerator
	{
		// Token: 0x06008254 RID: 33364 RVA: 0x002EB1E8 File Offset: 0x002E93E8
		public static void GenerateInventoryFor(Pawn p, PawnGenerationRequest request)
		{
			p.inventory.DestroyAll(DestroyMode.Vanish);
			for (int i = 0; i < p.kindDef.fixedInventory.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = p.kindDef.fixedInventory[i];
				Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, thingDefCountClass.stuff);
				thing.stackCount = thingDefCountClass.count;
				CompQuality compQuality;
				if (thing.TryGetComp(out compQuality))
				{
					compQuality.SetQuality(thingDefCountClass.quality, new ArtGenerationContext?(ArtGenerationContext.Outsider));
				}
				CompColorable compColorable;
				if (thingDefCountClass.color != null && thing.TryGetComp(out compColorable))
				{
					compColorable.SetColor(thingDefCountClass.color.Value);
				}
				if (thing.HasComp<CompEquippable>())
				{
					p.equipment.AddEquipment((ThingWithComps)thing);
				}
				else
				{
					Apparel apparel = thing as Apparel;
					if (apparel != null)
					{
						p.apparel.Wear(apparel, true, false);
					}
					else
					{
						p.inventory.innerContainer.TryAdd(thing, true);
					}
				}
			}
			if (p.kindDef.inventoryOptions != null)
			{
				foreach (Thing item in p.kindDef.inventoryOptions.GenerateThings())
				{
					p.inventory.innerContainer.TryAdd(item, true);
				}
			}
			if (request.AllowFood)
			{
				PawnInventoryGenerator.GiveRandomFood(p);
			}
			PawnInventoryGenerator.GiveDrugsIfAddicted(p);
			PawnInventoryGenerator.GiveCombatEnhancingDrugs(p);
		}

		// Token: 0x06008255 RID: 33365 RVA: 0x002EB368 File Offset: 0x002E9568
		public static void GiveRandomFood(Pawn p)
		{
			if (p.kindDef.invNutrition > 0.001f)
			{
				ThingDef def;
				if (p.kindDef.invFoodDef != null)
				{
					def = p.kindDef.invFoodDef;
				}
				else
				{
					float value = Rand.Value;
					if (value < 0.5f)
					{
						def = ThingDefOf.MealSimple;
					}
					else if ((double)value < 0.75)
					{
						def = ThingDefOf.MealFine;
					}
					else
					{
						def = ThingDefOf.MealSurvivalPack;
					}
				}
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.stackCount = GenMath.RoundRandom(p.kindDef.invNutrition / thing.GetStatValue(StatDefOf.Nutrition, true, -1));
				p.inventory.TryAddItemNotForSale(thing);
			}
		}

		// Token: 0x06008256 RID: 33366 RVA: 0x002EB410 File Offset: 0x002E9610
		private static void GiveDrugsIfAddicted(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			p.health.hediffSet.GetHediffs<Hediff_Addiction>(ref PawnInventoryGenerator.tmpAddictionHediffs, null);
			using (List<Hediff_Addiction>.Enumerator enumerator = PawnInventoryGenerator.tmpAddictionHediffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hediff_Addiction addiction = enumerator.Current;
					ThingDef def;
					if (DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef x)
					{
						if (x.category != ThingCategory.Item)
						{
							return false;
						}
						if (p.Faction != null && x.techLevel > p.Faction.def.techLevel)
						{
							return false;
						}
						CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
						return compProperties != null && compProperties.chemical != null && compProperties.chemical.addictionHediff == addiction.def;
					}).TryRandomElement(out def))
					{
						int stackCount = Rand.RangeInclusive(2, 5);
						Thing thing = ThingMaker.MakeThing(def, null);
						thing.stackCount = stackCount;
						p.inventory.TryAddItemNotForSale(thing);
					}
				}
			}
		}

		// Token: 0x06008257 RID: 33367 RVA: 0x002EB4FC File Offset: 0x002E96FC
		private static void GiveCombatEnhancingDrugs(Pawn pawn)
		{
			if (Rand.Value >= pawn.kindDef.combatEnhancingDrugsChance)
			{
				return;
			}
			if (pawn.IsTeetotaler())
			{
				return;
			}
			for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
			{
				CompDrug compDrug = pawn.inventory.innerContainer[i].TryGetComp<CompDrug>();
				if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
				{
					return;
				}
			}
			int randomInRange = pawn.kindDef.combatEnhancingDrugsCount.RandomInRange;
			if (randomInRange <= 0)
			{
				return;
			}
			IEnumerable<ThingDef> source = DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef x)
			{
				if (x.category != ThingCategory.Item)
				{
					return false;
				}
				if (pawn.Faction != null && x.techLevel > pawn.Faction.def.techLevel)
				{
					return false;
				}
				CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.isCombatEnhancingDrug;
			});
			int num = 0;
			ThingDef def;
			while (num < randomInRange && source.TryRandomElement(out def))
			{
				pawn.inventory.innerContainer.TryAdd(ThingMaker.MakeThing(def, null), true);
				num++;
			}
		}

		// Token: 0x040050C3 RID: 20675
		private static List<Hediff_Addiction> tmpAddictionHediffs = new List<Hediff_Addiction>();
	}
}
