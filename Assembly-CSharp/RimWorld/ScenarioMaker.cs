using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001788 RID: 6024
	public static class ScenarioMaker
	{
		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x06009604 RID: 38404 RVA: 0x00368174 File Offset: 0x00366374
		public static Scenario GeneratingScenario
		{
			get
			{
				return ScenarioMaker.scen;
			}
		}

		// Token: 0x06009605 RID: 38405 RVA: 0x0036817C File Offset: 0x0036637C
		public static Scenario GenerateNewRandomScenario(string seed)
		{
			Rand.PushState();
			Rand.Seed = seed.GetHashCode();
			int @int = Rand.Int;
			int[] array = new int[10];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Rand.Int;
			}
			int int2 = Rand.Int;
			ScenarioMaker.scen = new Scenario();
			ScenarioMaker.scen.Category = ScenarioCategory.CustomLocal;
			ScenarioMaker.scen.name = NameGenerator.GenerateName(RulePackDefOf.NamerScenario, null, false, null, null, null);
			ScenarioMaker.scen.description = null;
			ScenarioMaker.scen.summary = null;
			Rand.Seed = @int;
			ScenarioMaker.scen.playerFaction = (ScenPart_PlayerFaction)ScenarioMaker.MakeScenPart(ScenPartDefOf.PlayerFaction);
			ScenarioMaker.scen.parts.Add(ScenarioMaker.MakeScenPart(ScenPartDefOf.ConfigPage_ConfigureStartingPawns));
			ScenarioMaker.scen.parts.Add(ScenarioMaker.MakeScenPart(ScenPartDefOf.PlayerPawnsArriveMethod));
			ScenarioMaker.scen.surfaceLayer = new ScenPart_PlanetLayer
			{
				def = ScenPartDefOf.PlanetLayerFixed,
				layer = PlanetLayerDefOf.Surface,
				settingsDef = PlanetLayerSettingsDefOf.Surface,
				hide = true,
				tag = "Surface"
			};
			if (ModsConfig.OdysseyActive)
			{
				ScenPart_PlanetLayer scenPart_PlanetLayer = new ScenPart_PlanetLayer
				{
					def = ScenPartDefOf.PlanetLayerFixed,
					layer = PlanetLayerDefOf.Orbit,
					settingsDef = PlanetLayerSettingsDefOf.Orbit,
					hide = true,
					tag = "Orbit"
				};
				ScenarioMaker.scen.parts.Add(scenPart_PlanetLayer);
				ScenarioMaker.scen.surfaceLayer.connections.Add(new LayerConnection
				{
					tag = scenPart_PlanetLayer.tag,
					zoomMode = LayerConnection.ZoomMode.ZoomOut
				});
				scenPart_PlanetLayer.connections.Add(new LayerConnection
				{
					tag = ScenarioMaker.scen.surfaceLayer.tag,
					zoomMode = LayerConnection.ZoomMode.ZoomIn
				});
			}
			Rand.Seed = array[0];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.PlayerPawnFilter, Rand.RangeInclusive(-1, 2));
			Rand.Seed = array[1];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.StartingImportant, Rand.RangeInclusive(0, 2));
			Rand.Seed = array[2];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.PlayerPawnModifier, Rand.RangeInclusive(-1, 2));
			Rand.Seed = array[3];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.Rule, Rand.RangeInclusive(-2, 3));
			Rand.Seed = array[4];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.StartingItem, Rand.RangeInclusive(0, 6));
			Rand.Seed = array[5];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.WorldThing, Rand.RangeInclusive(-3, 6));
			Rand.Seed = array[6];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.GameCondition, Rand.RangeInclusive(-1, 2));
			Rand.Seed = int2;
			foreach (ScenPart scenPart in ScenarioMaker.scen.AllParts)
			{
				scenPart.Randomize();
			}
			for (int j = 0; j < ScenarioMaker.scen.parts.Count; j++)
			{
				for (int k = 0; k < ScenarioMaker.scen.parts.Count; k++)
				{
					if (j != k && ScenarioMaker.scen.parts[j].TryMerge(ScenarioMaker.scen.parts[k]))
					{
						ScenarioMaker.scen.parts.RemoveAt(k);
						k--;
						if (j > k)
						{
							j--;
						}
					}
				}
			}
			for (int l = 0; l < ScenarioMaker.scen.parts.Count; l++)
			{
				for (int m = 0; m < ScenarioMaker.scen.parts.Count; m++)
				{
					if (l != m && !ScenarioMaker.scen.parts[l].CanCoexistWith(ScenarioMaker.scen.parts[m]))
					{
						ScenarioMaker.scen.parts.RemoveAt(m);
						m--;
						if (l > m)
						{
							l--;
						}
					}
				}
			}
			foreach (string text in ScenarioMaker.scen.ConfigErrors())
			{
				Log.Error(text);
			}
			Rand.PopState();
			Scenario result = ScenarioMaker.scen;
			ScenarioMaker.scen = null;
			return result;
		}

		// Token: 0x06009606 RID: 38406 RVA: 0x003685BC File Offset: 0x003667BC
		private static void AddCategoryScenParts(Scenario scen, ScenPartCategory cat, int count)
		{
			scen.parts.AddRange(ScenarioMaker.RandomScenPartsOfCategory(scen, cat, count));
		}

		// Token: 0x06009607 RID: 38407 RVA: 0x003685D1 File Offset: 0x003667D1
		private static IEnumerable<ScenPart> RandomScenPartsOfCategory(Scenario scen, ScenPartCategory cat, int count)
		{
			ScenarioMaker.<RandomScenPartsOfCategory>d__5 <RandomScenPartsOfCategory>d__ = new ScenarioMaker.<RandomScenPartsOfCategory>d__5(-2);
			<RandomScenPartsOfCategory>d__.<>3__scen = scen;
			<RandomScenPartsOfCategory>d__.<>3__cat = cat;
			<RandomScenPartsOfCategory>d__.<>3__count = count;
			return <RandomScenPartsOfCategory>d__;
		}

		// Token: 0x06009608 RID: 38408 RVA: 0x003685F0 File Offset: 0x003667F0
		public static IEnumerable<ScenPartDef> AddableParts(Scenario scen)
		{
			return from d in DefDatabase<ScenPartDef>.AllDefs
			where scen.AllParts.Count((ScenPart p) => p.def == d) < d.maxUses
			select d;
		}

		// Token: 0x06009609 RID: 38409 RVA: 0x00368620 File Offset: 0x00366820
		private static bool CanAddPart(Scenario scen, ScenPart newPart)
		{
			for (int i = 0; i < scen.parts.Count; i++)
			{
				if (!newPart.CanCoexistWith(scen.parts[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600960A RID: 38410 RVA: 0x0036865A File Offset: 0x0036685A
		public static ScenPart MakeScenPart(ScenPartDef def)
		{
			ScenPart scenPart = (ScenPart)Activator.CreateInstance(def.scenPartClass);
			scenPart.def = def;
			return scenPart;
		}

		// Token: 0x04005A35 RID: 23093
		private static Scenario scen;
	}
}
