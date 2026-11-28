using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA0 RID: 3744
	public static class ThingDefGenerator_Corpses
	{
		// Token: 0x0600629D RID: 25245 RVA: 0x002382D7 File Offset: 0x002364D7
		public static IEnumerable<ThingDef> ImpliedCorpseDefs(bool hotReload = false)
		{
			ThingDefGenerator_Corpses.<ImpliedCorpseDefs>d__6 <ImpliedCorpseDefs>d__ = new ThingDefGenerator_Corpses.<ImpliedCorpseDefs>d__6(-2);
			<ImpliedCorpseDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedCorpseDefs>d__;
		}

		// Token: 0x0600629E RID: 25246 RVA: 0x002382E8 File Offset: 0x002364E8
		private static ThingDef GenerateUnnaturalCorpseDef(bool hotReload, ThingDef pawnDef)
		{
			ThingDef thingDef = ThingDefGenerator_Corpses.GenerateCorpseDef(pawnDef, hotReload, "UnnaturalCorpse_" + pawnDef.defName);
			thingDef.label = "UnnaturalCorpse".Translate(pawnDef);
			thingDef.thingClass = typeof(UnnaturalCorpse);
			thingDef.SetStatBaseValue(StatDefOf.DeteriorationRate, 0.25f);
			thingDef.tickerType = TickerType.Normal;
			CompProperties_Studiable item = new CompProperties_Studiable
			{
				frequencyTicks = 120000,
				knowledgeCategory = KnowledgeCategoryDefOf.Advanced,
				anomalyKnowledge = new float?((float)2),
				minMonolithLevelForStudy = 1,
				studyEnabledByDefault = false,
				showToggleGizmo = true
			};
			thingDef.comps.Add(item);
			thingDef.inspectorTabs.Insert(1, typeof(ITab_StudyNotesUnnaturalCorpse));
			CompProperties_DisableUnnaturalCorpse item2 = new CompProperties_DisableUnnaturalCorpse
			{
				activeTicks = 1,
				ticksToActivate = 180,
				activateTexPath = "UI/Commands/DestroyUnnaturalCorpse",
				activateLabelString = "UnnaturalCorpseDeactivate".Translate(),
				activateDescString = "UnnaturalCorpseDeactivateDesc".Translate(),
				guiLabelString = "UnnaturalCorpseGuiLabel".Translate(),
				jobString = "UnnaturalCorpseJobString".Translate(),
				activatingString = "UnnaturalCorpseActivating".Translate(),
				activatingStringPending = "UnnaturalCorpseActivatingEnroute".Translate(),
				targetingParameters = new TargetingParameters
				{
					canTargetBuildings = false,
					canTargetAnimals = false,
					canTargetMechs = false,
					onlyTargetControlledPawns = true
				}
			};
			thingDef.comps.Add(item2);
			pawnDef.race.unnaturalCorpseDef = thingDef;
			pawnDef.race.corpseDef.virtualDefs.Add(thingDef);
			thingDef.virtualDefParent = pawnDef.race.corpseDef;
			return thingDef;
		}

		// Token: 0x0600629F RID: 25247 RVA: 0x002384B8 File Offset: 0x002366B8
		private static ThingDef GenerateCorpseDef(ThingDef pawnDef, bool hotReload = false, string customDefName = null)
		{
			string defName = customDefName ?? ("Corpse_" + pawnDef.defName);
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? new ThingDef()) : new ThingDef();
			thingDef.defName = defName;
			thingDef.label = "CorpseLabel".Translate(pawnDef.label);
			thingDef.description = "CorpseDesc".Translate(pawnDef.label);
			thingDef.soundImpactDefault = pawnDef.soundImpactDefault;
			thingDef.category = ThingCategory.Item;
			thingDef.thingClass = typeof(Corpse);
			thingDef.selectable = true;
			thingDef.tickerType = TickerType.Rare;
			thingDef.altitudeLayer = AltitudeLayer.ItemImportant;
			thingDef.scatterableOnMapGen = false;
			thingDef.alwaysHaulable = true;
			thingDef.soundDrop = SoundDefOf.Corpse_Drop;
			thingDef.pathCost = 14;
			thingDef.socialPropernessMatters = false;
			thingDef.tradeability = Tradeability.None;
			thingDef.messageOnDeteriorateInStorage = false;
			thingDef.modContentPack = pawnDef.modContentPack;
			thingDef.hideStats = pawnDef.hideStats;
			thingDef.hiddenWhileUndiscovered = pawnDef.race.corpseHiddenWhileUndiscovered;
			thingDef.inspectorTabs = new List<Type>
			{
				typeof(ITab_Pawn_Health),
				typeof(ITab_Pawn_Character),
				typeof(ITab_Pawn_Gear),
				typeof(ITab_Pawn_Social),
				typeof(ITab_Pawn_Log)
			};
			thingDef.comps.Add(new CompProperties_Forbiddable());
			if (ModsConfig.BiotechActive)
			{
				thingDef.inspectorTabs.Add(typeof(ITab_Genes));
			}
			thingDef.SetStatBaseValue(StatDefOf.Beauty, -50f);
			thingDef.SetStatBaseValue(StatDefOf.DeteriorationRate, 1f);
			thingDef.SetStatBaseValue(StatDefOf.FoodPoisonChanceFixedHuman, 0.05f);
			thingDef.recipes = new List<RecipeDef>();
			if (!pawnDef.race.IsMechanoid)
			{
				thingDef.recipes.Add(RecipeDefOf.RemoveBodyPart);
			}
			thingDef.SetStatBaseValue(StatDefOf.MarketValue, ThingDefGenerator_Corpses.CalculateMarketValue(pawnDef));
			thingDef.SetStatBaseValue(StatDefOf.Flammability, pawnDef.GetStatValueAbstract(StatDefOf.Flammability, null));
			thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)pawnDef.BaseMaxHitPoints);
			thingDef.SetStatBaseValue(StatDefOf.Mass, pawnDef.statBases.GetStatOffsetFromList(StatDefOf.Mass));
			thingDef.SetStatBaseValue(StatDefOf.Nutrition, 5.2f);
			thingDef.ingestible = new IngestibleProperties();
			thingDef.ingestible.parent = thingDef;
			IngestibleProperties ingestible = thingDef.ingestible;
			ingestible.foodType = FoodTypeFlags.Corpse;
			ingestible.sourceDef = pawnDef;
			ingestible.preferability = (pawnDef.race.IsFlesh ? FoodPreferability.DesperateOnly : FoodPreferability.NeverForNutrition);
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(ingestible, "tasteThought", ThoughtDefOf.AteCorpse.defName, null, null, null);
			ingestible.maxNumToIngestAtOnce = 1;
			ingestible.defaultNumToIngestAtOnce = 1;
			ingestible.ingestEffect = EffecterDefOf.EatMeat;
			ingestible.ingestSound = SoundDefOf.RawMeat_Eat;
			ingestible.specialThoughtDirect = pawnDef.race.FleshType.ateDirect;
			if (pawnDef.race.IsFlesh)
			{
				thingDef.comps.Add(new CompProperties_Rottable
				{
					daysToRotStart = 2.5f,
					daysToDessicated = 5f,
					rotDamagePerDay = 2f,
					dessicatedDamagePerDay = 0.7f
				});
				thingDef.comps.Add(new CompProperties_SpawnerFilth
				{
					filthDef = ThingDefOf.Filth_CorpseBile,
					spawnCountOnSpawn = 0,
					spawnMtbHours = 0f,
					spawnRadius = 0.1f,
					spawnEveryDays = 1f,
					requiredRotStage = new RotStage?(RotStage.Rotting)
				});
			}
			if (ModsConfig.AnomalyActive && pawnDef.race.IsFlesh)
			{
				thingDef.comps.Add(new CompProperties
				{
					compClass = typeof(CompHarbingerTreeConsumable)
				});
			}
			thingDef.race = pawnDef.race;
			if (thingDef.thingCategories == null)
			{
				thingDef.thingCategories = new List<ThingCategoryDef>();
			}
			DirectXmlCrossRefLoader.RegisterListWantsCrossRef<ThingCategoryDef>(thingDef.thingCategories, pawnDef.race.Humanlike ? ThingCategoryDefOf.CorpsesHumanlike.defName : pawnDef.race.FleshType.corpseCategory.defName, thingDef, null, null);
			return thingDef;
		}

		// Token: 0x060062A0 RID: 25248 RVA: 0x002388D8 File Offset: 0x00236AD8
		private static float CalculateMarketValue(ThingDef raceDef)
		{
			float num = 0f;
			if (raceDef.race.meatDef != null)
			{
				int num2 = Mathf.RoundToInt(raceDef.GetStatValueAbstract(StatDefOf.MeatAmount, null));
				num += (float)num2 * raceDef.race.meatDef.GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			if (raceDef.race.leatherDef != null)
			{
				int num3 = Mathf.RoundToInt(raceDef.GetStatValueAbstract(StatDefOf.LeatherAmount, null));
				num += (float)num3 * raceDef.race.leatherDef.GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			if (raceDef.butcherProducts != null)
			{
				for (int i = 0; i < raceDef.butcherProducts.Count; i++)
				{
					num += raceDef.butcherProducts[i].thingDef.BaseMarketValue * (float)raceDef.butcherProducts[i].count;
				}
			}
			return num * 0.6f;
		}

		// Token: 0x0400364A RID: 13898
		private const float DaysToStartRot = 2.5f;

		// Token: 0x0400364B RID: 13899
		private const float DaysToDessicate = 5f;

		// Token: 0x0400364C RID: 13900
		private const float RotDamagePerDay = 2f;

		// Token: 0x0400364D RID: 13901
		private const float DessicatedDamagePerDay = 0.7f;

		// Token: 0x0400364E RID: 13902
		private const float ButcherProductsMarketValueFactor = 0.6f;

		// Token: 0x0400364F RID: 13903
		private const string DeactivateImagePath = "UI/Commands/DestroyUnnaturalCorpse";
	}
}
