using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001601 RID: 5633
	public static class FactionGenerator
	{
		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x06008C2E RID: 35886 RVA: 0x0032F0BC File Offset: 0x0032D2BC
		public static IEnumerable<FactionDef> ConfigurableFactions
		{
			get
			{
				return new FactionGenerator.<get_ConfigurableFactions>d__2(-2);
			}
		}

		// Token: 0x06008C2F RID: 35887 RVA: 0x0032F0C8 File Offset: 0x0032D2C8
		public static void GenerateFactionsIntoWorldLayer(PlanetLayer layer, List<FactionDef> factions = null)
		{
			FactionGenerator.<>c__DisplayClass3_0 CS$<>8__locals1 = new FactionGenerator.<>c__DisplayClass3_0();
			CS$<>8__locals1.layer = layer;
			FactionGenerator.InitializeFactions(CS$<>8__locals1.layer, factions);
			IEnumerable<Faction> source = Find.World.factionManager.AllFactionsListForReading.Where(new Func<Faction, bool>(CS$<>8__locals1.<GenerateFactionsIntoWorldLayer>g__Validator|0));
			if (source.Any<Faction>())
			{
				float num = CS$<>8__locals1.layer.Def.viewAngleSettlementsFactorCurve.Evaluate(Mathf.Clamp01(CS$<>8__locals1.layer.ViewAngle / 180f));
				float randomInRange = CS$<>8__locals1.layer.Def.settlementsPer100kTiles.RandomInRange;
				float scaleFactor = Find.World.info.overallPopulation.GetScaleFactor();
				int num2 = GenMath.RoundRandom((float)CS$<>8__locals1.layer.TilesCount / 100000f * randomInRange * scaleFactor * num);
				num2 -= Find.WorldObjects.AllSettlementsOnLayer(CS$<>8__locals1.layer).Count;
				for (int i = 0; i < num2; i++)
				{
					Faction faction = source.RandomElementByWeight((Faction x) => x.def.settlementGenerationWeight);
					WorldObject worldObject = WorldObjectMaker.MakeWorldObject(CS$<>8__locals1.layer.Def.SettlementWorldObjectDef);
					worldObject.SetFaction(faction);
					worldObject.Tile = TileFinder.RandomSettlementTileFor(CS$<>8__locals1.layer, faction, false, null);
					INameableWorldObject nameableWorldObject = worldObject as INameableWorldObject;
					if (nameableWorldObject != null)
					{
						nameableWorldObject.Name = SettlementNameGenerator.GenerateSettlementName(worldObject, null);
					}
					Find.WorldObjects.Add(worldObject);
				}
			}
			Find.IdeoManager.SortIdeos();
		}

		// Token: 0x06008C30 RID: 35888 RVA: 0x0032F254 File Offset: 0x0032D454
		private static void InitializeFactions(PlanetLayer layer, List<FactionDef> factions)
		{
			if (factions != null)
			{
				using (List<FactionDef>.Enumerator enumerator = factions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FactionDef factionDef = enumerator.Current;
						if (FactionGenerator.CanExistOnLayer(layer, factionDef))
						{
							FactionGenerator.AddFactionToManager(layer, factionDef);
						}
					}
					return;
				}
			}
			IOrderedEnumerable<FactionDef> orderedEnumerable = from x in DefDatabase<FactionDef>.AllDefs
			orderby x.hidden
			select x;
			using (IEnumerator<FactionDef> enumerator2 = orderedEnumerable.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					FactionDef facDef = enumerator2.Current;
					if (!orderedEnumerable.Any((FactionDef x) => x.requiredCountAtGameStart > 0 && x.replacesFaction == facDef) && FactionGenerator.CanExistOnLayer(layer, facDef))
					{
						for (int i = 0; i < facDef.requiredCountAtGameStart; i++)
						{
							FactionGenerator.AddFactionToManager(layer, facDef);
						}
					}
				}
			}
		}

		// Token: 0x06008C31 RID: 35889 RVA: 0x0032F364 File Offset: 0x0032D564
		private static bool CanExistOnLayer(PlanetLayer layer, FactionDef f)
		{
			return (f.layerBlacklist.NullOrEmpty<PlanetLayerDef>() || !f.layerBlacklist.Contains(layer.Def)) && ((f.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && layer.IsRootSurface) || f.layerWhitelist.Contains(layer.Def));
		}

		// Token: 0x06008C32 RID: 35890 RVA: 0x0032F3BB File Offset: 0x0032D5BB
		private static void AddFactionToManager(PlanetLayer layer, FactionDef facDef)
		{
			FactionGenerator.CreateFactionAndAddToManager(layer, facDef);
		}

		// Token: 0x06008C33 RID: 35891 RVA: 0x0032F3C4 File Offset: 0x0032D5C4
		public static void CreateFactionAndAddToManager(FactionDef facDef)
		{
			FactionGenerator.CreateFactionAndAddToManager(Find.WorldGrid.Surface, facDef);
		}

		// Token: 0x06008C34 RID: 35892 RVA: 0x0032F3D8 File Offset: 0x0032D5D8
		public static void CreateFactionAndAddToManager(PlanetLayer layer, FactionDef facDef)
		{
			string text;
			List<StyleCategoryDef> styles;
			List<DeityPreset> deityPresets;
			bool hiddenIdeo;
			string text2;
			if (facDef.fixedIdeo)
			{
				bool forceNoExpansionIdeo = false;
				List<PreceptDef> disallowedPrecepts = null;
				List<MemeDef> disallowedMemes = null;
				text = facDef.ideoName;
				styles = facDef.styles;
				deityPresets = facDef.deityPresets;
				hiddenIdeo = facDef.hiddenIdeo;
				text2 = facDef.ideoDescription;
				IdeoGenerationParms ideoGenerationParms = new IdeoGenerationParms(facDef, forceNoExpansionIdeo, disallowedPrecepts, disallowedMemes, facDef.forcedMemes, false, false, false, true, text, styles, deityPresets, hiddenIdeo, text2, facDef.requiredPreceptsOnly);
				Find.FactionManager.Add(FactionGenerator.NewGeneratedFaction(layer, new FactionGeneratorParms(facDef, ideoGenerationParms, null)));
				return;
			}
			bool forceNoExpansionIdeo2 = false;
			List<PreceptDef> disallowedPrecepts2 = null;
			List<MemeDef> disallowedMemes2 = null;
			text2 = facDef.ideoName;
			styles = facDef.styles;
			deityPresets = facDef.deityPresets;
			hiddenIdeo = facDef.hiddenIdeo;
			text = facDef.ideoDescription;
			IdeoGenerationParms ideoGenerationParms2 = new IdeoGenerationParms(facDef, forceNoExpansionIdeo2, disallowedPrecepts2, disallowedMemes2, facDef.forcedMemes, false, false, false, false, text2, styles, deityPresets, hiddenIdeo, text, facDef.requiredPreceptsOnly);
			Find.FactionManager.Add(FactionGenerator.NewGeneratedFaction(layer, new FactionGeneratorParms(facDef, ideoGenerationParms2, null)));
		}

		// Token: 0x06008C35 RID: 35893 RVA: 0x0032F4BE File Offset: 0x0032D6BE
		public static Faction NewGeneratedFaction(FactionGeneratorParms parms)
		{
			return FactionGenerator.NewGeneratedFaction(Find.WorldGrid.Surface, parms);
		}

		// Token: 0x06008C36 RID: 35894 RVA: 0x0032F4D0 File Offset: 0x0032D6D0
		public static Faction NewGeneratedFaction(PlanetLayer layer, FactionGeneratorParms parms)
		{
			FactionDef factionDef = parms.factionDef;
			parms.ideoGenerationParms.forFaction = factionDef;
			Faction faction = new Faction();
			faction.def = factionDef;
			faction.loadID = Find.UniqueIDsManager.GetNextFactionID();
			faction.colorFromSpectrum = FactionGenerator.NewRandomColorFromSpectrum(faction);
			faction.hidden = parms.hidden;
			if (factionDef.humanlikeFaction)
			{
				faction.ideos = new FactionIdeosTracker(faction);
				if (!faction.IsPlayer || !ModsConfig.IdeologyActive || !Find.GameInitData.startedFromEntry)
				{
					faction.ideos.ChooseOrGenerateIdeo(parms.ideoGenerationParms);
				}
			}
			if (!factionDef.isPlayer)
			{
				if (factionDef.fixedName != null)
				{
					faction.Name = factionDef.fixedName;
				}
				else
				{
					string text = "";
					for (int i = 0; i < 10; i++)
					{
						string text2 = NameGenerator.GenerateName(faction.def.factionNameMaker, from fac in Find.FactionManager.AllFactionsVisible
						select fac.Name, false, null);
						if (text2.Length <= 20)
						{
							text = text2;
						}
					}
					if (text.NullOrEmpty())
					{
						text = NameGenerator.GenerateName(faction.def.factionNameMaker, from fac in Find.FactionManager.AllFactionsVisible
						select fac.Name, false, null);
					}
					faction.Name = text;
				}
			}
			foreach (Faction other in Find.FactionManager.AllFactionsListForReading)
			{
				faction.TryMakeInitialRelationsWith(other);
			}
			if (!faction.Hidden && !factionDef.isPlayer)
			{
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(layer.Def.SettlementWorldObjectDef);
				worldObject.SetFaction(faction);
				worldObject.Tile = TileFinder.RandomSettlementTileFor(layer, faction, false, null);
				INameableWorldObject nameableWorldObject = worldObject as INameableWorldObject;
				if (nameableWorldObject != null)
				{
					nameableWorldObject.Name = SettlementNameGenerator.GenerateSettlementName(worldObject, null);
				}
				Find.WorldObjects.Add(worldObject);
			}
			faction.TryGenerateNewLeader();
			return faction;
		}

		// Token: 0x06008C37 RID: 35895 RVA: 0x0032F6F4 File Offset: 0x0032D8F4
		public static Faction NewGeneratedFactionWithRelations(FactionDef facDef, List<FactionRelation> relations, bool hidden = false)
		{
			PlanetLayer surface = Find.WorldGrid.Surface;
			bool? hidden2 = new bool?(hidden);
			return FactionGenerator.NewGeneratedFactionWithRelations(surface, new FactionGeneratorParms(facDef, default(IdeoGenerationParms), hidden2), relations);
		}

		// Token: 0x06008C38 RID: 35896 RVA: 0x0032F72C File Offset: 0x0032D92C
		public static Faction NewGeneratedFactionWithRelations(PlanetLayer layer, FactionDef facDef, List<FactionRelation> relations, bool hidden = false)
		{
			bool? hidden2 = new bool?(hidden);
			return FactionGenerator.NewGeneratedFactionWithRelations(layer, new FactionGeneratorParms(facDef, default(IdeoGenerationParms), hidden2), relations);
		}

		// Token: 0x06008C39 RID: 35897 RVA: 0x0032F758 File Offset: 0x0032D958
		public static Faction NewGeneratedFactionWithRelations(FactionGeneratorParms parms, List<FactionRelation> relations)
		{
			return FactionGenerator.NewGeneratedFactionWithRelations(Find.WorldGrid.Surface, parms, relations);
		}

		// Token: 0x06008C3A RID: 35898 RVA: 0x0032F76C File Offset: 0x0032D96C
		public static Faction NewGeneratedFactionWithRelations(PlanetLayer layer, FactionGeneratorParms parms, List<FactionRelation> relations)
		{
			Faction faction = FactionGenerator.NewGeneratedFaction(layer, parms);
			for (int i = 0; i < relations.Count; i++)
			{
				faction.SetRelation(relations[i]);
			}
			return faction;
		}

		// Token: 0x06008C3B RID: 35899 RVA: 0x0032F7A0 File Offset: 0x0032D9A0
		public static float NewRandomColorFromSpectrum(Faction faction)
		{
			float num = -1f;
			float result = 0f;
			for (int i = 0; i < 20; i++)
			{
				float value = Rand.Value;
				float num2 = 1f;
				List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
				for (int j = 0; j < allFactionsListForReading.Count; j++)
				{
					Faction faction2 = allFactionsListForReading[j];
					if (faction2.def == faction.def)
					{
						float num3 = Mathf.Abs(value - faction2.colorFromSpectrum);
						if (num3 < num2)
						{
							num2 = num3;
						}
					}
				}
				if (num2 > num)
				{
					num = num2;
					result = value;
				}
			}
			return result;
		}

		// Token: 0x0400561B RID: 22043
		private const int MaxPreferredFactionNameLength = 20;
	}
}
