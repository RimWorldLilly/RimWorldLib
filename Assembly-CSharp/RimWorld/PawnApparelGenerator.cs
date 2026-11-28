using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CB RID: 5323
	public static class PawnApparelGenerator
	{
		// Token: 0x06008220 RID: 33312 RVA: 0x002E85DD File Offset: 0x002E67DD
		static PawnApparelGenerator()
		{
			PawnApparelGenerator.Reset();
		}

		// Token: 0x06008221 RID: 33313 RVA: 0x002E8614 File Offset: 0x002E6814
		public static void Reset()
		{
			PawnApparelGenerator.allApparelPairs = ThingStuffPair.AllWith((ThingDef td) => td.IsApparel);
			PawnApparelGenerator.freeWarmParkaMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Parka, ThingDefOf.Cloth) * 1.3f));
			PawnApparelGenerator.freeWarmHatMaxPrice = (float)((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Tuque, ThingDefOf.Cloth) * 1.3f));
			PawnApparelGenerator.freeToxicEnvironmentResistanceApparelMaxPrice = (float)(ModsConfig.BiotechActive ? ((int)(StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_GasMask, null) * 1.3f)) : 0);
			if (ModsConfig.OdysseyActive)
			{
				float num = (float)((int)StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_Vacsuit, null));
				int num2 = (int)StatDefOf.MarketValue.Worker.GetValueAbstract(ThingDefOf.Apparel_VacsuitHelmet, null);
				PawnApparelGenerator.freeVacuumResistanceApparelMaxPrice = (num + (float)num2) * 1.3f;
				return;
			}
			PawnApparelGenerator.freeVacuumResistanceApparelMaxPrice = 0f;
		}

		// Token: 0x06008222 RID: 33314 RVA: 0x002E8710 File Offset: 0x002E6910
		public static void GenerateStartingApparelFor(Pawn pawn, PawnGenerationRequest request)
		{
			if (!pawn.RaceProps.ToolUser || !pawn.RaceProps.IsFlesh || pawn.RaceProps.IsAnomalyEntity)
			{
				return;
			}
			pawn.apparel.DestroyAll(DestroyMode.Vanish);
			Pawn_OutfitTracker outfits = pawn.outfits;
			if (outfits != null)
			{
				OutfitForcedHandler forcedHandler = outfits.forcedHandler;
				if (forcedHandler != null)
				{
					forcedHandler.Reset();
				}
			}
			float randomInRange = pawn.kindDef.apparelMoney.RandomInRange;
			float mapTemperature;
			NeededWarmth neededWarmth = PawnApparelGenerator.ApparelWarmthNeededNow(pawn, request, out mapTemperature);
			bool flag = PawnApparelGenerator.NeedVacuumResistance(pawn, request);
			bool flag2 = Rand.Value < pawn.kindDef.apparelAllowHeadgearChance;
			float num = PawnApparelGenerator.ApparelToxicEnvironmentToAddress(pawn, request);
			PawnApparelGenerator.debugSb = null;
			if (DebugViewSettings.logApparelGeneration)
			{
				PawnApparelGenerator.debugSb = new StringBuilder();
				StringBuilder stringBuilder = PawnApparelGenerator.debugSb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Generating apparel for ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(pawn);
				stringBuilder.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				StringBuilder stringBuilder2 = PawnApparelGenerator.debugSb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(7, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Money: ");
				defaultInterpolatedStringHandler2.AppendFormatted<float>(randomInRange, "F0");
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
				StringBuilder stringBuilder3 = PawnApparelGenerator.debugSb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler3.AppendLiteral("Needed warmth: ");
				defaultInterpolatedStringHandler3.AppendFormatted<NeededWarmth>(neededWarmth);
				stringBuilder3.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
				StringBuilder stringBuilder4 = PawnApparelGenerator.debugSb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(25, 1);
				defaultInterpolatedStringHandler4.AppendLiteral("Needs vacuum resistance: ");
				defaultInterpolatedStringHandler4.AppendFormatted<bool>(flag);
				stringBuilder4.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
				StringBuilder stringBuilder5 = PawnApparelGenerator.debugSb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(37, 1);
				defaultInterpolatedStringHandler5.AppendLiteral("Needed toxic environment resistance: ");
				defaultInterpolatedStringHandler5.AppendFormatted<float>(num);
				stringBuilder5.AppendLine(defaultInterpolatedStringHandler5.ToStringAndClear());
				StringBuilder stringBuilder6 = PawnApparelGenerator.debugSb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(18, 1);
				defaultInterpolatedStringHandler6.AppendLiteral("Headgear allowed: ");
				defaultInterpolatedStringHandler6.AppendFormatted<bool>(flag2);
				stringBuilder6.AppendLine(defaultInterpolatedStringHandler6.ToStringAndClear());
			}
			int @int = Rand.Int;
			PawnApparelGenerator.tmpApparelCandidates.Clear();
			for (int i = 0; i < PawnApparelGenerator.allApparelPairs.Count; i++)
			{
				ThingStuffPair thingStuffPair = PawnApparelGenerator.allApparelPairs[i];
				if (PawnApparelGenerator.CanUsePair(thingStuffPair, pawn, randomInRange, flag2, @int))
				{
					PawnApparelGenerator.tmpApparelCandidates.Add(thingStuffPair);
				}
			}
			if (!pawn.IsColonist && Rand.Chance(pawn.kindDef.nakedChance))
			{
				StringBuilder stringBuilder7 = PawnApparelGenerator.debugSb;
				if (stringBuilder7 != null)
				{
					stringBuilder7.AppendLine("Apparel overridden by nakedChance in PawnKindDef");
				}
				PawnApparelGenerator.workingSet.Reset(pawn);
				PawnApparelGenerator.GenerateSpecificRequiredApparel(pawn, randomInRange, true);
			}
			else if (randomInRange < 0.001f)
			{
				PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, PawnApparelGenerator.tmpApparelCandidates);
			}
			else
			{
				int num2 = 0;
				for (;;)
				{
					PawnApparelGenerator.GenerateWorkingPossibleApparelSetFor(pawn, randomInRange, PawnApparelGenerator.tmpApparelCandidates);
					StringBuilder stringBuilder8 = PawnApparelGenerator.debugSb;
					if (stringBuilder8 != null)
					{
						string str = num2.ToString().PadRight(5);
						string str2 = "Trying: ";
						PawnApparelGenerator.PossibleApparelSet possibleApparelSet = PawnApparelGenerator.workingSet;
						stringBuilder8.Append(str + str2 + ((possibleApparelSet != null) ? possibleApparelSet.ToString() : null));
					}
					if (num2 >= 10 || Rand.Value >= 0.85f || randomInRange >= 9999999f)
					{
						goto IL_3F8;
					}
					float num3 = Rand.Range(0.45f, 0.8f);
					float totalPrice = PawnApparelGenerator.workingSet.TotalPrice;
					if (totalPrice >= randomInRange * num3)
					{
						goto IL_3F8;
					}
					StringBuilder stringBuilder9 = PawnApparelGenerator.debugSb;
					if (stringBuilder9 != null)
					{
						stringBuilder9.AppendLine(string.Concat(new string[]
						{
							" -- Failed: Spent $",
							totalPrice.ToString("F0"),
							", < ",
							(num3 * 100f).ToString("F0"),
							"% of money."
						}));
					}
					IL_56C:
					num2++;
					continue;
					IL_3F8:
					if (num2 < 20 && Rand.Value < 0.97f && !PawnApparelGenerator.workingSet.Covers(BodyPartGroupDefOf.Torso))
					{
						StringBuilder stringBuilder10 = PawnApparelGenerator.debugSb;
						if (stringBuilder10 == null)
						{
							goto IL_56C;
						}
						stringBuilder10.AppendLine(" -- Failed: Does not cover torso.");
						goto IL_56C;
					}
					else if (num2 < 30 && Rand.Value < 0.8f && PawnApparelGenerator.workingSet.CoatButNoShirt())
					{
						StringBuilder stringBuilder11 = PawnApparelGenerator.debugSb;
						if (stringBuilder11 == null)
						{
							goto IL_56C;
						}
						stringBuilder11.AppendLine(" -- Failed: Coat but no shirt.");
						goto IL_56C;
					}
					else
					{
						if (num2 < 50)
						{
							bool mustBeSafe = num2 < 17;
							if (!PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, mustBeSafe, mapTemperature))
							{
								StringBuilder stringBuilder12 = PawnApparelGenerator.debugSb;
								if (stringBuilder12 == null)
								{
									goto IL_56C;
								}
								stringBuilder12.AppendLine(" -- Failed: Wrong warmth.");
								goto IL_56C;
							}
						}
						if (ModsConfig.BiotechActive && num2 < 10 && !PawnApparelGenerator.workingSet.SatisfiesNeededToxicEnvironmentResistance(num))
						{
							StringBuilder stringBuilder13 = PawnApparelGenerator.debugSb;
							if (stringBuilder13 == null)
							{
								goto IL_56C;
							}
							stringBuilder13.AppendLine(" -- Failed: Wrong toxic environment resistance.");
							goto IL_56C;
						}
						else
						{
							if (num2 >= 80 || !PawnApparelGenerator.workingSet.IsNaked(pawn.gender))
							{
								break;
							}
							StringBuilder stringBuilder14 = PawnApparelGenerator.debugSb;
							if (stringBuilder14 == null)
							{
								goto IL_56C;
							}
							stringBuilder14.AppendLine(" -- Failed: Naked.");
							goto IL_56C;
						}
					}
				}
				StringBuilder stringBuilder15 = PawnApparelGenerator.debugSb;
				if (stringBuilder15 != null)
				{
					stringBuilder15.Append(" -- Approved! Total price: $" + PawnApparelGenerator.workingSet.TotalPrice.ToString("F0") + ", TotalInsulationCold: " + PawnApparelGenerator.workingSet.TotalInsulationCold.ToString());
				}
			}
			if ((!pawn.kindDef.apparelIgnoreSeasons || request.ForceAddFreeWarmLayerIfNeeded) && !PawnApparelGenerator.workingSet.SatisfiesNeededWarmth(neededWarmth, true, mapTemperature))
			{
				PawnApparelGenerator.workingSet.AddFreeWarmthAsNeeded(neededWarmth, mapTemperature, pawn);
			}
			if (ModsConfig.BiotechActive && !pawn.kindDef.apparelIgnorePollution && num > 0.05f && !PawnApparelGenerator.workingSet.SatisfiesNeededToxicEnvironmentResistance(num))
			{
				PawnApparelGenerator.workingSet.AddFreeToxicEnvironmentResistanceAsNeeded(num, (ThingStuffPair pa) => pa.thing.apparel.CorrectAgeForWearing(pawn) && ((pawn.kindDef.apparelIgnoreSeasons && !request.ForceAddFreeWarmLayerIfNeeded) || PawnApparelGenerator.workingSet.GetReplacedInsulationCold(pa) <= pa.InsulationCold));
			}
			if (flag)
			{
				PawnApparelGenerator.workingSet.AddFreeVacuumResistanceAsNeeded(pawn);
			}
			if (DebugViewSettings.logApparelGeneration && PawnApparelGenerator.debugSb != null && PawnApparelGenerator.debugSb.Length > 0)
			{
				Log.Message(PawnApparelGenerator.debugSb.ToString());
			}
			PawnApparelGenerator.workingSet.GiveToPawn(pawn);
			PawnApparelGenerator.workingSet.Reset(null, null);
			foreach (Apparel apparel in pawn.apparel.WornApparel)
			{
				PawnApparelGenerator.PostProcessApparel(apparel, pawn);
				CompBiocodable compBiocodable = apparel.TryGetComp<CompBiocodable>();
				if (compBiocodable != null && !compBiocodable.Biocoded && Rand.Chance(request.BiocodeApparelChance))
				{
					compBiocodable.CodeFor(pawn);
				}
			}
		}

		// Token: 0x06008223 RID: 33315 RVA: 0x002E8E00 File Offset: 0x002E7000
		public static void PostProcessApparel(Apparel apparel, Pawn pawn)
		{
			if (pawn.kindDef.apparelColor != Color.white)
			{
				apparel.SetColor(pawn.kindDef.apparelColor, false);
			}
			Ideo ideo = pawn.Ideo;
			ThingStyleDef thingStyleDef = (ideo != null) ? ideo.GetStyleFor(apparel.def) : null;
			if (thingStyleDef != null)
			{
				apparel.SetStyleDef(thingStyleDef);
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (PawnApparelGenerator.ApparelRequirementHandlesThing(specificApparelRequirements[i], apparel.def))
					{
						Color color = specificApparelRequirements[i].GetColor();
						if (color != default(Color))
						{
							apparel.SetColor(color, false);
						}
						if (specificApparelRequirements[i].UseRandomStyleDef)
						{
							if (!apparel.def.randomStyle.NullOrEmpty<ThingStyleChance>() && Rand.Chance(apparel.def.randomStyleChance))
							{
								apparel.SetStyleDef(apparel.def.randomStyle.RandomElementByWeight((ThingStyleChance x) => x.Chance).StyleDef);
							}
						}
						else if (specificApparelRequirements[i].StyleDef != null)
						{
							apparel.SetStyleDef(specificApparelRequirements[i].StyleDef);
						}
						if (specificApparelRequirements[i].Locked)
						{
							pawn.apparel.Lock(apparel);
						}
						if (specificApparelRequirements[i].Biocode)
						{
							CompBiocodable compBiocodable = apparel.TryGetComp<CompBiocodable>();
							if (compBiocodable != null)
							{
								compBiocodable.CodeFor(pawn);
							}
						}
					}
				}
			}
		}

		// Token: 0x06008224 RID: 33316 RVA: 0x002E8F88 File Offset: 0x002E7188
		public static Apparel GenerateApparelOfDefFor(Pawn pawn, ThingDef apparelDef)
		{
			ThingStuffPair thingStuffPair;
			if (!(from pa in PawnApparelGenerator.allApparelPairs
			where pa.thing == apparelDef && PawnApparelGenerator.CanUseStuff(pawn, pa)
			select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out thingStuffPair))
			{
				if (!(from pa in PawnApparelGenerator.allApparelPairs
				where pa.thing == apparelDef
				select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out thingStuffPair))
				{
					return null;
				}
			}
			return (Apparel)ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
		}

		// Token: 0x06008225 RID: 33317 RVA: 0x002E9040 File Offset: 0x002E7240
		private static void GenerateWorkingPossibleApparelSetFor(Pawn pawn, float money, List<ThingStuffPair> apparelCandidates)
		{
			PawnApparelGenerator.workingSet.Reset(pawn);
			float num = PawnApparelGenerator.GenerateSpecificRequiredApparel(pawn, money, false);
			List<ThingDef> reqApparel = pawn.kindDef.apparelRequired;
			if (reqApparel != null)
			{
				int l;
				int i;
				for (i = 0; i < reqApparel.Count; i = l + 1)
				{
					if (reqApparel[i].apparel.CorrectAgeForWearing(pawn))
					{
						ThingStuffPair pair;
						if ((from pa in PawnApparelGenerator.allApparelPairs
						where pa.thing == reqApparel[i] && PawnApparelGenerator.CanUseStuff(pawn, pa) && !PawnApparelGenerator.workingSet.PairOverlapsAnything(pa)
						select pa).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair))
						{
							PawnApparelGenerator.workingSet.Add(pair);
							num -= pair.Price;
						}
					}
					l = i;
				}
			}
			PawnApparelGenerator.usableApparel.Clear();
			for (int j = 0; j < apparelCandidates.Count; j++)
			{
				if (!PawnApparelGenerator.workingSet.PairOverlapsAnything(apparelCandidates[j]))
				{
					PawnApparelGenerator.usableApparel.Add(apparelCandidates[j]);
				}
			}
			Func<ThingStuffPair, bool> <>9__2;
			while (pawn.Ideo == null || !pawn.Ideo.IdeoPrefersNudityForGender(pawn.gender) || (pawn.Faction != null && pawn.Faction.IsPlayer))
			{
				if (!pawn.IsColonist)
				{
					Pawn_StoryTracker story = pawn.story;
					if (((story != null) ? story.traits : null) != null && pawn.story.traits.HasTrait(TraitDefOf.Nudist))
					{
						break;
					}
				}
				if (Rand.Value < 0.1f && money < 9999999f)
				{
					break;
				}
				IEnumerable<ThingStuffPair> source = PawnApparelGenerator.usableApparel;
				Func<ThingStuffPair, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = ((ThingStuffPair pa) => PawnApparelGenerator.CanUseStuff(pawn, pa)));
				}
				ThingStuffPair pair2;
				if (!source.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair2))
				{
					break;
				}
				PawnApparelGenerator.workingSet.Add(pair2);
				num -= pair2.Price;
				for (int k = PawnApparelGenerator.usableApparel.Count - 1; k >= 0; k--)
				{
					if (PawnApparelGenerator.usableApparel[k].Price > num || PawnApparelGenerator.workingSet.PairOverlapsAnything(PawnApparelGenerator.usableApparel[k]))
					{
						PawnApparelGenerator.usableApparel.RemoveAt(k);
					}
				}
			}
		}

		// Token: 0x06008226 RID: 33318 RVA: 0x002E9318 File Offset: 0x002E7518
		private static float GenerateSpecificRequiredApparel(Pawn pawn, float moneyLeft, bool onlyGenerateIgnoreNaked)
		{
			List<SpecificApparelRequirement> att = pawn.kindDef.specificApparelRequirements;
			if (att != null)
			{
				int i;
				Func<ThingStuffPair, bool> <>9__0;
				int j;
				for (i = 0; i < att.Count; i = j + 1)
				{
					if (!att[i].RequiredTag.NullOrEmpty() || (!att[i].AlternateTagChoices.NullOrEmpty<SpecificApparelRequirement.TagChance>() && (!onlyGenerateIgnoreNaked || att[i].IgnoreNaked)))
					{
						IEnumerable<ThingStuffPair> source = PawnApparelGenerator.allApparelPairs;
						Func<ThingStuffPair, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((ThingStuffPair pa) => PawnApparelGenerator.ApparelRequirementTagsMatch(att[i], pa.thing) && PawnApparelGenerator.ApparelRequirementHandlesThing(att[i], pa.thing) && PawnApparelGenerator.CanUseStuff(pawn, pa) && pa.thing.apparel.PawnCanWear(pawn, false) && !PawnApparelGenerator.workingSet.PairOverlapsAnything(pa)));
						}
						ThingStuffPair pair;
						if (source.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality, out pair))
						{
							PawnApparelGenerator.workingSet.Add(pair);
							moneyLeft -= pair.Price;
						}
					}
					j = i;
				}
			}
			return moneyLeft;
		}

		// Token: 0x06008227 RID: 33319 RVA: 0x002E9448 File Offset: 0x002E7648
		private static bool CanUseStuff(Pawn pawn, ThingStuffPair pair)
		{
			if (pair.stuff != null && !pair.stuff.stuffProps.allowedInStuffGeneration)
			{
				return false;
			}
			List<SpecificApparelRequirement> specificApparelRequirements = pawn.kindDef.specificApparelRequirements;
			if (specificApparelRequirements != null)
			{
				for (int i = 0; i < specificApparelRequirements.Count; i++)
				{
					if (!PawnApparelGenerator.ApparelRequirementCanUseStuff(specificApparelRequirements[i], pair))
					{
						return false;
					}
				}
			}
			return pair.stuff == null || pawn.Faction == null || pawn.kindDef.ignoreFactionApparelStuffRequirements || pawn.Faction.def.CanUseStuffForApparel(pair.stuff);
		}

		// Token: 0x06008228 RID: 33320 RVA: 0x002E94DC File Offset: 0x002E76DC
		public static bool IsDerpApparel(ThingDef thing, ThingDef stuff)
		{
			if (stuff == null)
			{
				return false;
			}
			if (!thing.IsApparel)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < thing.stuffCategories.Count; i++)
			{
				if (thing.stuffCategories[i] != StuffCategoryDefOf.Woody && thing.stuffCategories[i] != StuffCategoryDefOf.Stony)
				{
					flag = true;
					break;
				}
			}
			return flag && (stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Woody) || stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Stony)) && (thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso) || thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs));
		}

		// Token: 0x06008229 RID: 33321 RVA: 0x002E95A0 File Offset: 0x002E77A0
		public static bool ApparelRequirementHandlesThing(SpecificApparelRequirement req, ThingDef thing)
		{
			return (req.BodyPartGroup == null || thing.apparel.bodyPartGroups.Contains(req.BodyPartGroup)) && (req.ApparelLayer == null || thing.apparel.layers.Contains(req.ApparelLayer)) && (req.ApparelDef == null || thing == req.ApparelDef);
		}

		// Token: 0x0600822A RID: 33322 RVA: 0x002E9608 File Offset: 0x002E7808
		public static bool ApparelRequirementTagsMatch(SpecificApparelRequirement req, ThingDef thing)
		{
			return (!req.RequiredTag.NullOrEmpty() && thing.apparel.tags.Contains(req.RequiredTag)) || (!req.AlternateTagChoices.NullOrEmpty<SpecificApparelRequirement.TagChance>() && req.AlternateTagChoices.Any((SpecificApparelRequirement.TagChance x) => thing.apparel.tags.Contains(x.tag) && Rand.Value < x.chance));
		}

		// Token: 0x0600822B RID: 33323 RVA: 0x002E9674 File Offset: 0x002E7874
		private static bool ApparelRequirementCanUseStuff(SpecificApparelRequirement req, ThingStuffPair pair)
		{
			return req.Stuff == null || !PawnApparelGenerator.ApparelRequirementHandlesThing(req, pair.thing) || (pair.stuff != null && req.Stuff == pair.stuff);
		}

		// Token: 0x0600822C RID: 33324 RVA: 0x002E96A8 File Offset: 0x002E78A8
		private static bool CanUsePair(ThingStuffPair pair, Pawn pawn, float moneyLeft, bool allowHeadgear, int fixedSeed)
		{
			if (pair.Price > moneyLeft)
			{
				return false;
			}
			if (!allowHeadgear && PawnApparelGenerator.IsHeadgear(pair.thing))
			{
				return false;
			}
			if (!pair.thing.apparel.PawnCanWear(pawn, false))
			{
				return false;
			}
			if (!pawn.kindDef.apparelTags.NullOrEmpty<string>())
			{
				bool flag = false;
				for (int i = 0; i < pawn.kindDef.apparelTags.Count; i++)
				{
					for (int j = 0; j < pair.thing.apparel.tags.Count; j++)
					{
						if (pawn.kindDef.apparelTags[i] == pair.thing.apparel.tags[j])
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (!pawn.kindDef.apparelDisallowTags.NullOrEmpty<string>())
			{
				for (int k = 0; k < pawn.kindDef.apparelDisallowTags.Count; k++)
				{
					if (pair.thing.apparel.tags.Contains(pawn.kindDef.apparelDisallowTags[k]))
					{
						return false;
					}
				}
			}
			return pawn.kindDef.ignoreApparelAllowChance || pair.thing.generateAllowChance >= 1f || Rand.ChanceSeeded(pair.thing.generateAllowChance, fixedSeed ^ (int)pair.thing.shortHash ^ 64128343);
		}

		// Token: 0x0600822D RID: 33325 RVA: 0x002E9814 File Offset: 0x002E7A14
		public static bool IsHeadgear(ThingDef td)
		{
			return td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead) || td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.UpperHead) || td.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Eyes);
		}

		// Token: 0x0600822E RID: 33326 RVA: 0x002E9868 File Offset: 0x002E7A68
		private static bool NeedVacuumResistance(Pawn pawn, PawnGenerationRequest request)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			PlanetTile tile = request.Tile;
			if (!tile.Valid)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					tile = anyPlayerHomeMap.Tile;
				}
			}
			return tile.Valid && tile.Tile.PrimaryBiome.inVacuum && pawn.ConcernedByVacuum;
		}

		// Token: 0x0600822F RID: 33327 RVA: 0x002E98C4 File Offset: 0x002E7AC4
		private static NeededWarmth ApparelWarmthNeededNow(Pawn pawn, PawnGenerationRequest request, out float mapTemperature)
		{
			PlanetTile tile = request.Tile;
			if (!tile.Valid)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					tile = anyPlayerHomeMap.Tile;
				}
			}
			if (!tile.Valid)
			{
				mapTemperature = 21f;
				return NeededWarmth.Any;
			}
			NeededWarmth neededWarmth = NeededWarmth.Any;
			Twelfth twelfth = GenLocalDate.Twelfth(tile);
			mapTemperature = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			for (int i = 0; i < 2; i++)
			{
				NeededWarmth neededWarmth2 = PawnApparelGenerator.CalculateNeededWarmth(pawn, tile, twelfth);
				if (neededWarmth2 != NeededWarmth.Any)
				{
					neededWarmth = neededWarmth2;
					break;
				}
				twelfth = twelfth.NextTwelfth();
			}
			if (!pawn.kindDef.apparelIgnoreSeasons)
			{
				return neededWarmth;
			}
			if (request.ForceAddFreeWarmLayerIfNeeded && neededWarmth == NeededWarmth.Warm)
			{
				return neededWarmth;
			}
			return NeededWarmth.Any;
		}

		// Token: 0x06008230 RID: 33328 RVA: 0x002E9960 File Offset: 0x002E7B60
		public static NeededWarmth CalculateNeededWarmth(Pawn pawn, PlanetTile tile, Twelfth twelfth)
		{
			float num = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, twelfth);
			if (num < pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - 4f)
			{
				return NeededWarmth.Warm;
			}
			if (num > pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) + 4f)
			{
				return NeededWarmth.Cool;
			}
			return NeededWarmth.Any;
		}

		// Token: 0x06008231 RID: 33329 RVA: 0x002E99B0 File Offset: 0x002E7BB0
		private static float ApparelToxicEnvironmentToAddress(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.kindDef.apparelIgnorePollution)
			{
				return 0f;
			}
			PlanetTile tile = request.Tile;
			if (!tile.Valid)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					tile = anyPlayerHomeMap.Tile;
				}
			}
			if (!tile.Valid)
			{
				return 0f;
			}
			return Mathf.Clamp01(Find.WorldGrid[tile].pollution);
		}

		// Token: 0x06008232 RID: 33330 RVA: 0x002E9A18 File Offset: 0x002E7C18
		[DebugOutput]
		private static void ApparelPairs()
		{
			IEnumerable<ThingStuffPair> dataSources = from p in PawnApparelGenerator.allApparelPairs
			orderby p.thing.defName descending
			select p;
			TableDataGetter<ThingStuffPair>[] array = new TableDataGetter<ThingStuffPair>[8];
			array[0] = new TableDataGetter<ThingStuffPair>("thing", (ThingStuffPair p) => p.thing.defName);
			array[1] = new TableDataGetter<ThingStuffPair>("stuff", delegate(ThingStuffPair p)
			{
				if (p.stuff == null)
				{
					return "";
				}
				return p.stuff.defName;
			});
			array[2] = new TableDataGetter<ThingStuffPair>("price", (ThingStuffPair p) => p.Price.ToString());
			array[3] = new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => (p.Commonality * 100f).ToString("F4"));
			array[4] = new TableDataGetter<ThingStuffPair>("generateCommonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F4"));
			array[5] = new TableDataGetter<ThingStuffPair>("insulationCold", delegate(ThingStuffPair p)
			{
				if (p.InsulationCold != 0f)
				{
					return p.InsulationCold.ToString();
				}
				return "";
			});
			array[6] = new TableDataGetter<ThingStuffPair>("headgear", delegate(ThingStuffPair p)
			{
				if (!PawnApparelGenerator.IsHeadgear(p.thing))
				{
					return "";
				}
				return "*";
			});
			array[7] = new TableDataGetter<ThingStuffPair>("derp", delegate(ThingStuffPair p)
			{
				if (!PawnApparelGenerator.IsDerpApparel(p.thing, p.stuff))
				{
					return "";
				}
				return "D";
			});
			DebugTables.MakeTablesDialog<ThingStuffPair>(dataSources, array);
		}

		// Token: 0x06008233 RID: 33331 RVA: 0x002E9BB9 File Offset: 0x002E7DB9
		[DebugOutput]
		private static void ApparelPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnApparelGenerator.allApparelPairs);
		}

		// Token: 0x04005096 RID: 20630
		private static List<ThingStuffPair> allApparelPairs = new List<ThingStuffPair>();

		// Token: 0x04005097 RID: 20631
		private static float freeWarmParkaMaxPrice;

		// Token: 0x04005098 RID: 20632
		private static float freeWarmHatMaxPrice;

		// Token: 0x04005099 RID: 20633
		private static float freeVacuumResistanceApparelMaxPrice;

		// Token: 0x0400509A RID: 20634
		private static float freeToxicEnvironmentResistanceApparelMaxPrice;

		// Token: 0x0400509B RID: 20635
		private static PawnApparelGenerator.PossibleApparelSet workingSet = new PawnApparelGenerator.PossibleApparelSet();

		// Token: 0x0400509C RID: 20636
		private static StringBuilder debugSb = null;

		// Token: 0x0400509D RID: 20637
		private const int PracticallyInfinity = 9999999;

		// Token: 0x0400509E RID: 20638
		private const float MinMapPollutionForFreeToxicResistanceApparel = 0.05f;

		// Token: 0x0400509F RID: 20639
		private static List<ThingStuffPair> tmpApparelCandidates = new List<ThingStuffPair>();

		// Token: 0x040050A0 RID: 20640
		private static List<ThingStuffPair> usableApparel = new List<ThingStuffPair>();

		// Token: 0x020032FB RID: 13051
		private class PossibleApparelSet
		{
			// Token: 0x17003171 RID: 12657
			// (get) Token: 0x06012E29 RID: 77353 RVA: 0x0064758F File Offset: 0x0064578F
			public int Count
			{
				get
				{
					return this.aps.Count;
				}
			}

			// Token: 0x17003172 RID: 12658
			// (get) Token: 0x06012E2A RID: 77354 RVA: 0x0064759C File Offset: 0x0064579C
			public float TotalPrice
			{
				get
				{
					return this.aps.Sum((ThingStuffPair pa) => pa.Price);
				}
			}

			// Token: 0x17003173 RID: 12659
			// (get) Token: 0x06012E2B RID: 77355 RVA: 0x006475C8 File Offset: 0x006457C8
			public float TotalInsulationCold
			{
				get
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold);
				}
			}

			// Token: 0x17003174 RID: 12660
			// (get) Token: 0x06012E2C RID: 77356 RVA: 0x006475F4 File Offset: 0x006457F4
			public List<ThingStuffPair> ApparelsForReading
			{
				get
				{
					return this.aps;
				}
			}

			// Token: 0x06012E2D RID: 77357 RVA: 0x006475FC File Offset: 0x006457FC
			public void Reset(BodyDef body, ThingDef raceDef)
			{
				this.aps.Clear();
				this.lgps.Clear();
				this.body = body;
				this.raceDef = raceDef;
				this.pawn = null;
			}

			// Token: 0x06012E2E RID: 77358 RVA: 0x0064762C File Offset: 0x0064582C
			public void Reset(Pawn pawn)
			{
				this.aps.Clear();
				this.lgps.Clear();
				this.pawn = pawn;
				BodyDef bodyDef;
				if (pawn == null)
				{
					bodyDef = null;
				}
				else
				{
					RaceProperties raceProps = pawn.RaceProps;
					bodyDef = ((raceProps != null) ? raceProps.body : null);
				}
				this.body = bodyDef;
				this.raceDef = ((pawn != null) ? pawn.def : null);
			}

			// Token: 0x06012E2F RID: 77359 RVA: 0x00647688 File Offset: 0x00645888
			public void Add(ThingStuffPair pair)
			{
				this.aps.Add(pair);
				for (int i = 0; i < pair.thing.apparel.layers.Count; i++)
				{
					ApparelLayerDef layer = pair.thing.apparel.layers[i];
					BodyPartGroupDef[] interferingBodyPartGroups = pair.thing.apparel.GetInterferingBodyPartGroups(this.body);
					for (int j = 0; j < interferingBodyPartGroups.Length; j++)
					{
						this.lgps.Add(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
					}
				}
			}

			// Token: 0x06012E30 RID: 77360 RVA: 0x00647714 File Offset: 0x00645914
			public bool PairOverlapsAnything(ThingStuffPair pair)
			{
				if (!this.lgps.Any<ApparelUtility.LayerGroupPair>())
				{
					return false;
				}
				for (int i = 0; i < pair.thing.apparel.layers.Count; i++)
				{
					ApparelLayerDef layer = pair.thing.apparel.layers[i];
					BodyPartGroupDef[] interferingBodyPartGroups = pair.thing.apparel.GetInterferingBodyPartGroups(this.body);
					for (int j = 0; j < interferingBodyPartGroups.Length; j++)
					{
						if (this.lgps.Contains(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j])))
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06012E31 RID: 77361 RVA: 0x006477A8 File Offset: 0x006459A8
			public bool CoatButNoShirt()
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (this.aps[i].thing.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
					{
						for (int j = 0; j < this.aps[i].thing.apparel.layers.Count; j++)
						{
							ApparelLayerDef apparelLayerDef = this.aps[i].thing.apparel.layers[j];
							if (apparelLayerDef == ApparelLayerDefOf.OnSkin)
							{
								flag2 = true;
							}
							if (apparelLayerDef == ApparelLayerDefOf.Shell || apparelLayerDef == ApparelLayerDefOf.Middle)
							{
								flag = true;
							}
						}
					}
				}
				return flag && !flag2;
			}

			// Token: 0x06012E32 RID: 77362 RVA: 0x00647874 File Offset: 0x00645A74
			public bool Covers(BodyPartGroupDef bp)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					if ((bp != BodyPartGroupDefOf.Legs || !this.aps[i].thing.apparel.legsNakedUnlessCoveredBySomethingElse) && this.aps[i].thing.apparel.bodyPartGroups.Contains(bp))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06012E33 RID: 77363 RVA: 0x006478E4 File Offset: 0x00645AE4
			public bool IsNaked(Gender gender)
			{
				switch (gender)
				{
				case Gender.None:
					return false;
				case Gender.Male:
					return !this.Covers(BodyPartGroupDefOf.Legs);
				case Gender.Female:
					return !this.Covers(BodyPartGroupDefOf.Legs) || !this.Covers(BodyPartGroupDefOf.Torso);
				default:
					return false;
				}
			}

			// Token: 0x06012E34 RID: 77364 RVA: 0x00647938 File Offset: 0x00645B38
			public bool SatisfiesNeededWarmth(NeededWarmth warmth, bool mustBeSafe = false, float mapTemperature = 21f)
			{
				if (warmth == NeededWarmth.Any)
				{
					return true;
				}
				if (mustBeSafe && !this.SafeTemperature(mapTemperature))
				{
					return false;
				}
				if (warmth == NeededWarmth.Cool)
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationHeat) >= -2f;
				}
				if (warmth == NeededWarmth.Warm)
				{
					return this.aps.Sum((ThingStuffPair a) => a.InsulationCold) >= 52f;
				}
				throw new NotImplementedException();
			}

			// Token: 0x06012E35 RID: 77365 RVA: 0x006479D0 File Offset: 0x00645BD0
			private bool SafeTemperature(float temp)
			{
				if (this.pawn != null)
				{
					return this.pawn.SafeTemperatureRange(this.aps).Includes(temp);
				}
				return GenTemperature.SafeTemperatureRange(this.raceDef, this.aps).Includes(temp);
			}

			// Token: 0x06012E36 RID: 77366 RVA: 0x00647A1C File Offset: 0x00645C1C
			public void AddFreeWarmthAsNeeded(NeededWarmth warmth, float mapTemperature, Pawn pawn)
			{
				PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass31_0 CS$<>8__locals1 = new PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass31_0();
				CS$<>8__locals1.pawn = pawn;
				CS$<>8__locals1.<>4__this = this;
				if (warmth == NeededWarmth.Any)
				{
					return;
				}
				if (warmth == NeededWarmth.Cool)
				{
					return;
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine();
					PawnApparelGenerator.debugSb.AppendLine("Trying to give free warm layer.");
				}
				PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass31_0 CS$<>8__locals2 = CS$<>8__locals1;
				Faction homeFaction = CS$<>8__locals1.pawn.HomeFaction;
				CS$<>8__locals2.homeFaction = ((homeFaction != null) ? homeFaction.def : null);
				for (int i = 0; i < 3; i++)
				{
					if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							StringBuilder debugSb = PawnApparelGenerator.debugSb;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Checking to give free torso-cover at max price ");
							defaultInterpolatedStringHandler.AppendFormatted<float>(PawnApparelGenerator.freeWarmParkaMaxPrice);
							debugSb.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						int j = 0;
						while (j < 2)
						{
							ThingStuffPair candidate;
							if (j == 0)
							{
								IEnumerable<ThingStuffPair> allApparelPairs = PawnApparelGenerator.allApparelPairs;
								Func<ThingStuffPair, bool> predicate;
								if ((predicate = CS$<>8__locals1.<>9__1) == null)
								{
									predicate = (CS$<>8__locals1.<>9__1 = ((ThingStuffPair pa) => base.<AddFreeWarmthAsNeeded>g__ParkaPairValidator|0(pa) && pa.InsulationCold < 40f));
								}
								if (allApparelPairs.Where(predicate).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out candidate))
								{
									goto IL_194;
								}
							}
							else
							{
								IEnumerable<ThingStuffPair> source = PawnApparelGenerator.allApparelPairs.Where(new Func<ThingStuffPair, bool>(CS$<>8__locals1.<AddFreeWarmthAsNeeded>g__ParkaPairValidator|0));
								Func<ThingStuffPair, float> selector;
								if ((selector = CS$<>8__locals1.<>9__3) == null)
								{
									selector = (CS$<>8__locals1.<>9__3 = ((ThingStuffPair x) => x.InsulationCold - CS$<>8__locals1.<>4__this.GetReplacedInsulationCold(x)));
								}
								if (source.TryMaxBy(selector, out candidate))
								{
									goto IL_194;
								}
							}
							j++;
							continue;
							IL_194:
							if (DebugViewSettings.logApparelGeneration)
							{
								StringBuilder debugSb2 = PawnApparelGenerator.debugSb;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(37, 2);
								defaultInterpolatedStringHandler2.AppendLiteral("Giving free torso-cover: ");
								defaultInterpolatedStringHandler2.AppendFormatted<ThingStuffPair>(candidate);
								defaultInterpolatedStringHandler2.AppendLiteral(" insulation=");
								defaultInterpolatedStringHandler2.AppendFormatted<float>(candidate.InsulationCold);
								debugSb2.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
								IEnumerable<ThingStuffPair> source2 = this.aps;
								Func<ThingStuffPair, bool> predicate2;
								Func<ThingStuffPair, bool> <>9__5;
								if ((predicate2 = <>9__5) == null)
								{
									predicate2 = (<>9__5 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, candidate.thing, CS$<>8__locals1.<>4__this.body)));
								}
								foreach (ThingStuffPair value in source2.Where(predicate2))
								{
									StringBuilder debugSb3 = PawnApparelGenerator.debugSb;
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(30, 2);
									defaultInterpolatedStringHandler3.AppendLiteral("    -replaces ");
									defaultInterpolatedStringHandler3.AppendFormatted<ThingStuffPair>(value);
									defaultInterpolatedStringHandler3.AppendLiteral(" InsulationCold=");
									defaultInterpolatedStringHandler3.AppendFormatted<float>(value.InsulationCold);
									debugSb3.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
								}
							}
							this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, candidate.thing, CS$<>8__locals1.<>4__this.body));
							this.aps.Add(candidate);
							break;
						}
					}
					if (this.SafeTemperature(mapTemperature))
					{
						break;
					}
				}
				if (!this.SatisfiesNeededWarmth(warmth, true, mapTemperature))
				{
					if (DebugViewSettings.logApparelGeneration)
					{
						StringBuilder debugSb4 = PawnApparelGenerator.debugSb;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(39, 1);
						defaultInterpolatedStringHandler4.AppendLiteral("Checking to give free hat at max price ");
						defaultInterpolatedStringHandler4.AppendFormatted<float>(PawnApparelGenerator.freeWarmHatMaxPrice);
						debugSb4.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
					}
					ThingStuffPair hatPair;
					if (PawnApparelGenerator.allApparelPairs.Where(new Func<ThingStuffPair, bool>(CS$<>8__locals1.<AddFreeWarmthAsNeeded>g__HatPairValidator|6)).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out hatPair))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							StringBuilder debugSb5 = PawnApparelGenerator.debugSb;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(29, 2);
							defaultInterpolatedStringHandler5.AppendLiteral("Giving free hat: ");
							defaultInterpolatedStringHandler5.AppendFormatted<ThingStuffPair>(hatPair);
							defaultInterpolatedStringHandler5.AppendLiteral(" insulation=");
							defaultInterpolatedStringHandler5.AppendFormatted<float>(hatPair.InsulationCold);
							debugSb5.AppendLine(defaultInterpolatedStringHandler5.ToStringAndClear());
							IEnumerable<ThingStuffPair> source3 = this.aps;
							Func<ThingStuffPair, bool> predicate3;
							Func<ThingStuffPair, bool> <>9__9;
							if ((predicate3 = <>9__9) == null)
							{
								predicate3 = (<>9__9 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, hatPair.thing, CS$<>8__locals1.<>4__this.body)));
							}
							foreach (ThingStuffPair value2 in source3.Where(predicate3))
							{
								StringBuilder debugSb6 = PawnApparelGenerator.debugSb;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(30, 2);
								defaultInterpolatedStringHandler6.AppendLiteral("    -replaces ");
								defaultInterpolatedStringHandler6.AppendFormatted<ThingStuffPair>(value2);
								defaultInterpolatedStringHandler6.AppendLiteral(" InsulationCold=");
								defaultInterpolatedStringHandler6.AppendFormatted<float>(value2.InsulationCold);
								debugSb6.AppendLine(defaultInterpolatedStringHandler6.ToStringAndClear());
							}
						}
						this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, hatPair.thing, CS$<>8__locals1.<>4__this.body));
						this.aps.Add(hatPair);
					}
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					StringBuilder debugSb7 = PawnApparelGenerator.debugSb;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler7 = new DefaultInterpolatedStringHandler(25, 1);
					defaultInterpolatedStringHandler7.AppendLiteral("New TotalInsulationCold: ");
					defaultInterpolatedStringHandler7.AppendFormatted<float>(this.TotalInsulationCold);
					debugSb7.AppendLine(defaultInterpolatedStringHandler7.ToStringAndClear());
				}
			}

			// Token: 0x06012E37 RID: 77367 RVA: 0x00647F34 File Offset: 0x00646134
			public bool SatisfiesNeededToxicEnvironmentResistance(float pollution)
			{
				if (pollution <= 0f)
				{
					return true;
				}
				return this.aps.Sum((ThingStuffPair ap) => ap.ToxicEnvironmentResistance) >= PawnApparelGenerator.PossibleApparelSet.ToxicEnvironmentResistanceOverPollutionCurve.Evaluate(pollution);
			}

			// Token: 0x06012E38 RID: 77368 RVA: 0x00647F88 File Offset: 0x00646188
			public void AddFreeVacuumResistanceAsNeeded(Pawn pawn)
			{
				PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass34_0 CS$<>8__locals1 = new PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass34_0();
				CS$<>8__locals1.pawn = pawn;
				CS$<>8__locals1.<>4__this = this;
				if (!ModsConfig.OdysseyActive)
				{
					return;
				}
				CS$<>8__locals1.desired = 0.9f - CS$<>8__locals1.pawn.GetStatValue(StatDefOf.VacuumResistance, true, -1);
				if (CS$<>8__locals1.desired <= 0f)
				{
					return;
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					PawnApparelGenerator.debugSb.AppendLine();
					PawnApparelGenerator.debugSb.AppendLine("Trying to give free vacuum resistance.");
				}
				int num = 0;
				while (num < 10 && !CS$<>8__locals1.<AddFreeVacuumResistanceAsNeeded>g__Protected|1())
				{
					ThingStuffPair pair;
					if (PawnApparelGenerator.allApparelPairs.Where(new Func<ThingStuffPair, bool>(CS$<>8__locals1.<AddFreeVacuumResistanceAsNeeded>g__VacuumValidator|0)).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out pair))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							StringBuilder debugSb = PawnApparelGenerator.debugSb;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Giving free vacuum resistance: ");
							defaultInterpolatedStringHandler.AppendFormatted<ThingStuffPair>(pair);
							defaultInterpolatedStringHandler.AppendLiteral(" VacuumResistance=");
							defaultInterpolatedStringHandler.AppendFormatted<float>(pair.VacuumResistance);
							debugSb.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
							IEnumerable<ThingStuffPair> source = this.aps;
							Func<ThingStuffPair, bool> predicate;
							Func<ThingStuffPair, bool> <>9__3;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, pair.thing, CS$<>8__locals1.<>4__this.body)));
							}
							foreach (ThingStuffPair value in source.Where(predicate))
							{
								StringBuilder debugSb2 = PawnApparelGenerator.debugSb;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(32, 2);
								defaultInterpolatedStringHandler2.AppendLiteral("    -replaces ");
								defaultInterpolatedStringHandler2.AppendFormatted<ThingStuffPair>(value);
								defaultInterpolatedStringHandler2.AppendLiteral(" VacuumResistance=");
								defaultInterpolatedStringHandler2.AppendFormatted<float>(value.VacuumResistance);
								debugSb2.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
							}
						}
						for (int i = this.aps.Count - 1; i >= 0; i--)
						{
							ThingStuffPair thingStuffPair = this.aps[i];
							if (!ApparelUtility.CanWearTogether(thingStuffPair.thing, pair.thing, this.body))
							{
								if (DebugViewSettings.logApparelGeneration)
								{
									StringBuilder debugSb3 = PawnApparelGenerator.debugSb;
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(32, 2);
									defaultInterpolatedStringHandler3.AppendLiteral("    -replaces ");
									defaultInterpolatedStringHandler3.AppendFormatted<ThingStuffPair>(thingStuffPair);
									defaultInterpolatedStringHandler3.AppendLiteral(" VacuumResistance=");
									defaultInterpolatedStringHandler3.AppendFormatted<float>(thingStuffPair.VacuumResistance);
									debugSb3.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
								}
								CS$<>8__locals1.desired += thingStuffPair.VacuumResistance;
								this.aps.RemoveAt(i);
							}
						}
						this.aps.Add(pair);
						CS$<>8__locals1.desired -= pair.VacuumResistance;
					}
					num++;
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					StringBuilder debugSb4 = PawnApparelGenerator.debugSb;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(22, 1);
					defaultInterpolatedStringHandler4.AppendLiteral("New VacuumResistance: ");
					defaultInterpolatedStringHandler4.AppendFormatted<float>(this.aps.Sum((ThingStuffPair a) => a.VacuumResistance));
					debugSb4.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
				}
			}

			// Token: 0x06012E39 RID: 77369 RVA: 0x006482E0 File Offset: 0x006464E0
			private bool CoversUnprotectedVacuumPart(ThingDef thing)
			{
				foreach (BodyPartRecord bodyPartRecord in this.GetUnprotectedVacuumParts())
				{
					for (int i = 0; i < bodyPartRecord.groups.Count; i++)
					{
						BodyPartGroupDef bodyPartGroupDef = bodyPartRecord.groups[i];
						for (int j = 0; j < thing.apparel.bodyPartGroups.Count; j++)
						{
							BodyPartGroupDef bodyPartGroupDef2 = thing.apparel.bodyPartGroups[j];
							if (bodyPartGroupDef == bodyPartGroupDef2)
							{
								return true;
							}
						}
					}
				}
				return false;
			}

			// Token: 0x06012E3A RID: 77370 RVA: 0x00648390 File Offset: 0x00646590
			private List<BodyPartRecord> GetUnprotectedVacuumParts()
			{
				PawnApparelGenerator.PossibleApparelSet.tmpParts.AddRange(this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Outside, null, null));
				for (int i = PawnApparelGenerator.PossibleApparelSet.tmpParts.Count - 1; i >= 0; i--)
				{
					BodyPartRecord bodyPartRecord = PawnApparelGenerator.PossibleApparelSet.tmpParts[i];
					if (!bodyPartRecord.def.canBeVacuumBurnt || !bodyPartRecord.def.IsSkinCovered(bodyPartRecord, this.pawn.health.hediffSet))
					{
						PawnApparelGenerator.PossibleApparelSet.tmpParts.RemoveAt(i);
					}
					else
					{
						for (int j = 0; j < this.aps.Count; j++)
						{
							ThingStuffPair thingStuffPair = this.aps[j];
							if (thingStuffPair.VacuumResistance >= 0.1f)
							{
								for (int k = 0; k < bodyPartRecord.groups.Count; k++)
								{
									BodyPartGroupDef bodyPartGroupDef = bodyPartRecord.groups[k];
									for (int l = 0; l < thingStuffPair.thing.apparel.bodyPartGroups.Count; l++)
									{
										BodyPartGroupDef bodyPartGroupDef2 = thingStuffPair.thing.apparel.bodyPartGroups[l];
										if (bodyPartGroupDef == bodyPartGroupDef2)
										{
											PawnApparelGenerator.PossibleApparelSet.tmpParts.RemoveAt(i);
											goto IL_12C;
										}
									}
								}
							}
						}
					}
					IL_12C:;
				}
				return PawnApparelGenerator.PossibleApparelSet.tmpParts;
			}

			// Token: 0x06012E3B RID: 77371 RVA: 0x006484DC File Offset: 0x006466DC
			public void AddFreeToxicEnvironmentResistanceAsNeeded(float pollution, Func<ThingStuffPair, bool> extraValidator = null)
			{
				PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass38_0 CS$<>8__locals1 = new PawnApparelGenerator.PossibleApparelSet.<>c__DisplayClass38_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.extraValidator = extraValidator;
				for (int i = 0; i < 5; i++)
				{
					if (this.SatisfiesNeededToxicEnvironmentResistance(pollution))
					{
						break;
					}
					ThingStuffPair pollutionPair;
					if (PawnApparelGenerator.allApparelPairs.Where(new Func<ThingStuffPair, bool>(CS$<>8__locals1.<AddFreeToxicEnvironmentResistanceAsNeeded>g__PollutionApparelValidator|0)).TryRandomElementByWeight((ThingStuffPair pa) => pa.Commonality / (pa.Price * pa.Price), out pollutionPair))
					{
						if (DebugViewSettings.logApparelGeneration)
						{
							StringBuilder debugSb = PawnApparelGenerator.debugSb;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Giving free toxic environment resistance: ");
							defaultInterpolatedStringHandler.AppendFormatted<ThingStuffPair>(pollutionPair);
							defaultInterpolatedStringHandler.AppendLiteral(" ToxicEnvironmentResistance=");
							defaultInterpolatedStringHandler.AppendFormatted<float>(pollutionPair.ToxicEnvironmentResistance);
							debugSb.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
							IEnumerable<ThingStuffPair> source = this.aps;
							Func<ThingStuffPair, bool> predicate;
							Func<ThingStuffPair, bool> <>9__3;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((ThingStuffPair a) => !ApparelUtility.CanWearTogether(a.thing, pollutionPair.thing, CS$<>8__locals1.<>4__this.body)));
							}
							foreach (ThingStuffPair value in source.Where(predicate))
							{
								StringBuilder debugSb2 = PawnApparelGenerator.debugSb;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(42, 2);
								defaultInterpolatedStringHandler2.AppendLiteral("    -replaces ");
								defaultInterpolatedStringHandler2.AppendFormatted<ThingStuffPair>(value);
								defaultInterpolatedStringHandler2.AppendLiteral(" ToxicEnvironmentResistance=");
								defaultInterpolatedStringHandler2.AppendFormatted<float>(value.ToxicEnvironmentResistance);
								debugSb2.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
							}
						}
						this.aps.RemoveAll((ThingStuffPair pa) => !ApparelUtility.CanWearTogether(pa.thing, pollutionPair.thing, CS$<>8__locals1.<>4__this.body));
						this.aps.Add(pollutionPair);
					}
				}
				if (DebugViewSettings.logApparelGeneration)
				{
					StringBuilder debugSb3 = PawnApparelGenerator.debugSb;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(32, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("New ToxicEnvironmentResistance: ");
					defaultInterpolatedStringHandler3.AppendFormatted<float>(this.aps.Sum((ThingStuffPair a) => a.ToxicEnvironmentResistance));
					debugSb3.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
			}

			// Token: 0x06012E3C RID: 77372 RVA: 0x00648708 File Offset: 0x00646908
			public void GiveToPawn(Pawn pawn)
			{
				for (int i = 0; i < this.aps.Count; i++)
				{
					Apparel apparel = (Apparel)ThingMaker.MakeThing(this.aps[i].thing, this.aps[i].stuff);
					PawnGenerator.PostProcessGeneratedGear(apparel, pawn);
					if (ApparelUtility.HasPartsToWear(pawn, apparel.def))
					{
						pawn.apparel.Wear(apparel, false, false);
					}
				}
				for (int j = 0; j < this.aps.Count; j++)
				{
					for (int k = 0; k < this.aps.Count; k++)
					{
						if (j != k && !ApparelUtility.CanWearTogether(this.aps[j].thing, this.aps[k].thing, pawn.RaceProps.body))
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 3);
							defaultInterpolatedStringHandler.AppendFormatted<Pawn>(pawn);
							defaultInterpolatedStringHandler.AppendLiteral(" generated with apparel that cannot be worn together: ");
							defaultInterpolatedStringHandler.AppendFormatted<ThingStuffPair>(this.aps[j]);
							defaultInterpolatedStringHandler.AppendLiteral(", ");
							defaultInterpolatedStringHandler.AppendFormatted<ThingStuffPair>(this.aps[k]);
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
							return;
						}
					}
				}
			}

			// Token: 0x06012E3D RID: 77373 RVA: 0x0064884C File Offset: 0x00646A4C
			public float GetReplacedInsulationCold(ThingStuffPair newAp)
			{
				float num = 0f;
				for (int i = 0; i < this.aps.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(this.aps[i].thing, newAp.thing, this.body))
					{
						num += this.aps[i].InsulationCold;
					}
				}
				return num;
			}

			// Token: 0x06012E3E RID: 77374 RVA: 0x006488B1 File Offset: 0x00646AB1
			private bool CorrectFactionForApparel(FactionDef faction, ThingDef apparel)
			{
				if (faction != null)
				{
					if (apparel.apparel.anyTechLevelCanUseForWarmth)
					{
						return true;
					}
					if (faction.techLevel >= TechLevel.Industrial && apparel.techLevel == TechLevel.Neolithic)
					{
						return false;
					}
					if (faction.techLevel == TechLevel.Neolithic && apparel.techLevel >= TechLevel.Industrial)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06012E3F RID: 77375 RVA: 0x006488F0 File Offset: 0x00646AF0
			public override string ToString()
			{
				string text = "[";
				for (int i = 0; i < this.aps.Count; i++)
				{
					string str = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
					defaultInterpolatedStringHandler.AppendFormatted<ThingStuffPair>(this.aps[i]);
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					text = str + defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return text + "]";
			}

			// Token: 0x0400CCC8 RID: 52424
			private List<ThingStuffPair> aps = new List<ThingStuffPair>();

			// Token: 0x0400CCC9 RID: 52425
			private HashSet<ApparelUtility.LayerGroupPair> lgps = new HashSet<ApparelUtility.LayerGroupPair>();

			// Token: 0x0400CCCA RID: 52426
			private BodyDef body;

			// Token: 0x0400CCCB RID: 52427
			private ThingDef raceDef;

			// Token: 0x0400CCCC RID: 52428
			private Pawn pawn;

			// Token: 0x0400CCCD RID: 52429
			private const float StartingMinTemperature = 12f;

			// Token: 0x0400CCCE RID: 52430
			private const float TargetMinTemperature = -40f;

			// Token: 0x0400CCCF RID: 52431
			private const float StartingMaxTemperature = 32f;

			// Token: 0x0400CCD0 RID: 52432
			private const float TargetMaxTemperature = 30f;

			// Token: 0x0400CCD1 RID: 52433
			private const float MinToxicEnvironmentResistanceForFreeApparel = 0.25f;

			// Token: 0x0400CCD2 RID: 52434
			private const float MinVacuumResistanceForFreeApparel = 0.9f;

			// Token: 0x0400CCD3 RID: 52435
			private const float MinToxicEnvironmentResistanceImprovement = 0.15f;

			// Token: 0x0400CCD4 RID: 52436
			private const float MinVacuumResistanceImprovement = 0.15f;

			// Token: 0x0400CCD5 RID: 52437
			private static readonly SimpleCurve ToxicEnvironmentResistanceOverPollutionCurve = new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(0.5f, 0.5f),
					true
				},
				{
					new CurvePoint(1f, 0.85f),
					true
				}
			};

			// Token: 0x0400CCD6 RID: 52438
			private const float DesiredVacuumResistance = 0.9f;

			// Token: 0x0400CCD7 RID: 52439
			private static readonly List<BodyPartRecord> tmpParts = new List<BodyPartRecord>();
		}
	}
}
