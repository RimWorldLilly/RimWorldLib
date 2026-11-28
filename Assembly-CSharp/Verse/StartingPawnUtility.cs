using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000205 RID: 517
	public static class StartingPawnUtility
	{
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0004C876 File Offset: 0x0004AA76
		private static Dictionary<Pawn, List<ThingDefCount>> StartingPossessions
		{
			get
			{
				return Find.GameInitData.startingPossessions;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x0004C882 File Offset: 0x0004AA82
		private static List<Pawn> StartingAndOptionalPawns
		{
			get
			{
				return Find.GameInitData.startingAndOptionalPawns;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0004C890 File Offset: 0x0004AA90
		private static PawnGenerationRequest DefaultStartingPawnRequest
		{
			get
			{
				PawnKindDef kind = Find.GameInitData.startingPawnKind ?? Faction.OfPlayer.def.basicMemberKind;
				Faction ofPlayer = Faction.OfPlayer;
				PawnGenerationContext context = PawnGenerationContext.PlayerStarter;
				bool tutorialMode = TutorSystem.TutorialMode;
				XenotypeDef forcedXenotype = ModsConfig.BiotechActive ? XenotypeDefOf.Baseliner : null;
				FloatRange? excludeBiologicalAgeRange = ModsConfig.BiotechActive ? new FloatRange?(StartingPawnUtility.ExcludeBiologicalAgeRange) : null;
				return new PawnGenerationRequest(kind, ofPlayer, context, null, true, false, false, true, tutorialMode, 20f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, forcedXenotype, null, null, 0f, DevelopmentalStage.Adult, null, excludeBiologicalAgeRange, null, false, false, false, -1, 0, false);
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0004C978 File Offset: 0x0004AB78
		public static void DrawPortraitArea(Rect rect, int pawnIndex, bool renderClothes, bool renderHeadgear)
		{
			Pawn pawn = StartingPawnUtility.StartingAndOptionalPawns[pawnIndex];
			Widgets.DrawMenuSection(rect);
			rect = rect.ContractedBy(17f);
			Rect position = new Rect(rect.center.x - StartingPawnUtility.PawnPortraitSize.x / 2f, rect.yMin - 24f, StartingPawnUtility.PawnPortraitSize.x, StartingPawnUtility.PawnPortraitSize.y);
			RenderTexture image = PortraitsCache.Get(pawn, StartingPawnUtility.PawnPortraitSize, Rot4.South, default(Vector3), 1f, true, true, renderHeadgear, renderClothes, null, null, true, null);
			GUI.DrawTexture(position, image);
			Rect rect2 = rect;
			rect2.width = 500f;
			CharacterCardUtility.DrawCharacterCard(rect2, pawn, delegate
			{
				StartingPawnUtility.RandomizePawn(pawnIndex);
			}, rect, true);
			pawn = StartingPawnUtility.StartingAndOptionalPawns[pawnIndex];
			bool flag = SocialCardUtility.AnyRelations(pawn);
			List<ThingDefCount> list = StartingPawnUtility.StartingPossessions[pawn];
			bool flag2 = list.Any<ThingDefCount>();
			int num = 1;
			if (flag)
			{
				num++;
			}
			if (flag2)
			{
				num++;
			}
			float height = (rect.height - 100f - (4f * (float)num - 1f)) / (float)num;
			float y = rect.y;
			Rect rect3 = rect;
			rect3.yMin += 100f;
			rect3.xMin = rect2.xMax + 5f;
			rect3.height = height;
			if (!HealthCardUtility.AnyHediffsDisplayed(pawn, true))
			{
				GUI.color = Color.gray;
			}
			Widgets.Label(rect3, "Health".Translate().AsTipTitle());
			GUI.color = Color.white;
			rect3.yMin += 35f;
			HealthCardUtility.DrawHediffListing(rect3, pawn, true, 0f, false);
			y = rect3.yMax + 4f;
			if (flag)
			{
				Rect rect4 = new Rect(rect3.x, y, rect3.width, height);
				Widgets.Label(rect4, "Relations".Translate().AsTipTitle());
				rect4.yMin += 35f;
				SocialCardUtility.DrawRelationsAndOpinions(rect4, pawn);
				y = rect4.yMax + 4f;
			}
			if (flag2)
			{
				Rect rect5 = new Rect(rect3.x, y, rect3.width, height);
				Widgets.Label(rect5, "Possessions".Translate().AsTipTitle());
				rect5.yMin += 35f;
				StartingPawnUtility.DrawPossessions(rect5, pawn, list);
			}
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0004CC18 File Offset: 0x0004AE18
		private static void DrawPossessions(Rect rect, Pawn selPawn, List<ThingDefCount> possessions)
		{
			GUI.BeginGroup(rect);
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, StartingPawnUtility.listScrollViewHeight);
			Rect rect2 = rect;
			if (viewRect.height > outRect.height)
			{
				rect2.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref StartingPawnUtility.listScrollPosition, viewRect, true);
			float num = 0f;
			if (StartingPawnUtility.StartingPossessions.ContainsKey(selPawn))
			{
				for (int i = 0; i < possessions.Count; i++)
				{
					ThingDefCount thingDefCount = possessions[i];
					Rect rect3 = new Rect(0f, num, Text.LineHeight, Text.LineHeight);
					Widgets.DefIcon(rect3, thingDefCount.ThingDef, null, 1f, null, false, null, null, null, 1f);
					Rect rect4 = new Rect(rect3.xMax + 17f, num, rect.width - rect3.width - 17f - 24f, Text.LineHeight);
					Widgets.Label(rect4, thingDefCount.LabelCap);
					if (Mouse.IsOver(rect4))
					{
						Widgets.DrawHighlight(rect4);
						TooltipHandler.TipRegion(rect4, thingDefCount.ThingDef.LabelCap.ToString().Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + thingDefCount.ThingDef.description);
					}
					Widgets.InfoCardButton(rect4.xMax, num, thingDefCount.ThingDef);
					num += Text.LineHeight;
				}
			}
			if (Event.current.type == EventType.layout)
			{
				StartingPawnUtility.listScrollViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0004CDF8 File Offset: 0x0004AFF8
		public static void DrawSkillSummaries(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			rect = rect.ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.min, new Vector2(rect.width, 45f)), "TeamSkills".Translate());
			Text.Font = GameFont.Small;
			rect.yMin += 45f;
			rect = rect.LeftPart(0.25f);
			rect.height = 27f;
			rect.y -= 4f;
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			if (StartingPawnUtility.SkillsPerColumn < 0)
			{
				StartingPawnUtility.SkillsPerColumn = Mathf.CeilToInt((float)allDefsListForReading.Count((SkillDef sd) => sd.pawnCreatorSummaryVisible) / 4f);
			}
			int num = 0;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				if (skillDef.pawnCreatorSummaryVisible)
				{
					Rect r = rect;
					r.x = rect.x + rect.width * (float)(num / StartingPawnUtility.SkillsPerColumn);
					r.y = rect.y + rect.height * (float)(num % StartingPawnUtility.SkillsPerColumn);
					r.height = 24f;
					r.width -= 4f;
					Pawn pawn = StartingPawnUtility.FindBestSkillOwner(skillDef);
					SkillUI.DrawSkill(pawn.skills.GetSkill(skillDef), r.Rounded(), SkillUI.SkillDrawMode.Menu, pawn.Name.ToString().Colorize(ColoredText.TipSectionTitleColor));
					num++;
				}
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0004CF9C File Offset: 0x0004B19C
		private static Pawn FindBestSkillOwner(SkillDef skill)
		{
			Pawn pawn = Find.GameInitData.startingAndOptionalPawns[0];
			SkillRecord skillRecord = pawn.skills.GetSkill(skill);
			for (int i = 1; i < Find.GameInitData.startingPawnCount; i++)
			{
				SkillRecord skill2 = Find.GameInitData.startingAndOptionalPawns[i].skills.GetSkill(skill);
				if (!skill2.TotallyDisabled && (skillRecord.TotallyDisabled || skill2.Level > skillRecord.Level || (skill2.Level == skillRecord.Level && skill2.passion > skillRecord.passion)))
				{
					pawn = Find.GameInitData.startingAndOptionalPawns[i];
					skillRecord = skill2;
				}
			}
			return pawn;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0004D048 File Offset: 0x0004B248
		public static void RandomizePawn(int pawnIndex)
		{
			if (!TutorSystem.AllowAction("RandomizePawn"))
			{
				return;
			}
			int num = 0;
			do
			{
				Pawn pawn = StartingPawnUtility.StartingAndOptionalPawns[pawnIndex];
				SpouseRelationUtility.Notify_PawnRegenerated(pawn);
				StartingPawnUtility.RandomizeInPlace(pawn);
				num++;
			}
			while (num <= 20 && !StartingPawnUtility.WorkTypeRequirementsSatisfied());
			TutorSystem.Notify_Event("RandomizePawn");
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0004D0A0 File Offset: 0x0004B2A0
		public static void ClearAllStartingPawns()
		{
			for (int i = StartingPawnUtility.StartingAndOptionalPawns.Count - 1; i >= 0; i--)
			{
				StartingPawnUtility.StartingAndOptionalPawns[i].relations.ClearAllRelations();
				if (Find.World != null)
				{
					PawnUtility.DestroyStartingColonistFamily(StartingPawnUtility.StartingAndOptionalPawns[i]);
					PawnComponentsUtility.RemoveComponentsOnDespawned(StartingPawnUtility.StartingAndOptionalPawns[i]);
					Find.WorldPawns.PassToWorld(StartingPawnUtility.StartingAndOptionalPawns[i], PawnDiscardDecideMode.Discard);
				}
				StartingPawnUtility.StartingPossessions.Remove(StartingPawnUtility.StartingAndOptionalPawns[i]);
				StartingPawnUtility.StartingAndOptionalPawns.RemoveAt(i);
			}
			StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.Clear();
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0004D141 File Offset: 0x0004B341
		public static Pawn RandomizeInPlace(Pawn p)
		{
			return StartingPawnUtility.RegenerateStartingPawnInPlace(StartingPawnUtility.StartingAndOptionalPawns.IndexOf(p));
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0004D154 File Offset: 0x0004B354
		private static Pawn RegenerateStartingPawnInPlace(int index)
		{
			bool flag = Current.ProgramState > ProgramState.Entry;
			Pawn pawn = StartingPawnUtility.StartingAndOptionalPawns[index];
			if (!flag)
			{
				PawnUtility.TryDestroyStartingColonistFamily(pawn);
			}
			pawn.relations.ClearAllRelations();
			PawnComponentsUtility.RemoveComponentsOnDespawned(pawn);
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			StartingPawnUtility.StartingPossessions.Remove(pawn);
			StartingPawnUtility.StartingAndOptionalPawns[index] = null;
			if (!flag)
			{
				for (int i = 0; i < StartingPawnUtility.StartingAndOptionalPawns.Count; i++)
				{
					if (StartingPawnUtility.StartingAndOptionalPawns[i] != null)
					{
						PawnUtility.TryDestroyStartingColonistFamily(StartingPawnUtility.StartingAndOptionalPawns[i]);
					}
				}
			}
			Pawn pawn2 = StartingPawnUtility.NewGeneratedStartingPawn(index);
			StartingPawnUtility.StartingAndOptionalPawns[index] = pawn2;
			return pawn2;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0004D1FB File Offset: 0x0004B3FB
		public static PawnGenerationRequest GetGenerationRequest(int index)
		{
			StartingPawnUtility.EnsureGenerationRequestInRangeOf(index);
			return StartingPawnUtility.StartingAndOptionalPawnGenerationRequests[index];
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0004D20E File Offset: 0x0004B40E
		public static void SetGenerationRequest(int index, PawnGenerationRequest request)
		{
			StartingPawnUtility.EnsureGenerationRequestInRangeOf(index);
			StartingPawnUtility.StartingAndOptionalPawnGenerationRequests[index] = request;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0004D224 File Offset: 0x0004B424
		public static void ReorderRequests(int from, int to)
		{
			StartingPawnUtility.EnsureGenerationRequestInRangeOf((from > to) ? from : to);
			PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(from);
			StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.Insert(to, generationRequest);
			StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.RemoveAt((from < to) ? from : (from + 1));
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0004D265 File Offset: 0x0004B465
		private static void EnsureGenerationRequestInRangeOf(int index)
		{
			while (StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.Count <= index)
			{
				StartingPawnUtility.StartingAndOptionalPawnGenerationRequests.Add(StartingPawnUtility.DefaultStartingPawnRequest);
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0004D285 File Offset: 0x0004B485
		public static int PawnIndex(Pawn pawn)
		{
			return Mathf.Max(StartingPawnUtility.StartingAndOptionalPawns.IndexOf(pawn), 0);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0004D298 File Offset: 0x0004B498
		public static Pawn NewGeneratedStartingPawn(int index = -1)
		{
			PawnGenerationRequest request = (index < 0) ? StartingPawnUtility.DefaultStartingPawnRequest : StartingPawnUtility.GetGenerationRequest(index);
			Pawn pawn = null;
			try
			{
				pawn = PawnGenerator.GeneratePawn(request);
			}
			catch (Exception ex)
			{
				string str = "There was an exception thrown by the PawnGenerator during generating a starting pawn. Trying one more time...\nException: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				pawn = PawnGenerator.GeneratePawn(request);
			}
			pawn.relations.everSeenByPlayer = true;
			PawnComponentsUtility.AddComponentsForSpawn(pawn);
			StartingPawnUtility.GeneratePossessions(pawn);
			return pawn;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0004D314 File Offset: 0x0004B514
		public static void GeneratePossessions(Pawn pawn)
		{
			if (!StartingPawnUtility.StartingPossessions.ContainsKey(pawn))
			{
				StartingPawnUtility.StartingPossessions.Add(pawn, new List<ThingDefCount>());
			}
			else
			{
				StartingPawnUtility.StartingPossessions[pawn].Clear();
			}
			if (Find.Scenario.AllParts.Any((ScenPart x) => x is ScenPart_NoPossessions))
			{
				return;
			}
			if (pawn.IsSubhuman)
			{
				return;
			}
			if (ModsConfig.BiotechActive && pawn.DevelopmentalStage.Baby())
			{
				StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(ThingDefOf.BabyFood, StartingPawnUtility.BabyFoodCountRange.RandomInRange));
				return;
			}
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
				{
					return;
				}
				Hediff_Addiction hediff_Addiction = hediff as Hediff_Addiction;
				if (hediff_Addiction != null)
				{
					Need_Chemical need = hediff_Addiction.Need;
					ThingDef thingDef = StartingPawnUtility.<GeneratePossessions>g__GetDrugFor|35_1(hediff_Addiction.Chemical);
					if (need != null && thingDef != null)
					{
						int count = GenMath.RoundRandom(need.def.fallPerDay * StartingPawnUtility.DaysSatisfied.RandomInRange / thingDef.GetCompProperties<CompProperties_Drug>().needLevelOffset);
						StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(thingDef, count));
					}
				}
			}
			if (ModsConfig.BiotechActive)
			{
				foreach (Hediff hediff2 in pawn.health.hediffSet.hediffs)
				{
					if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
					{
						return;
					}
					Hediff_ChemicalDependency hediff_ChemicalDependency = hediff2 as Hediff_ChemicalDependency;
					if (hediff_ChemicalDependency != null && hediff_ChemicalDependency.LinkedGene != null && hediff_ChemicalDependency.LinkedGene.Active)
					{
						ThingDef thingDef2 = StartingPawnUtility.<GeneratePossessions>g__GetDrugFor|35_1(hediff_ChemicalDependency.chemical);
						if (thingDef2 != null)
						{
							HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = hediff_ChemicalDependency.def.CompProps<HediffCompProperties_SeverityPerDay>();
							float num = (hediffCompProperties_SeverityPerDay != null) ? hediffCompProperties_SeverityPerDay.severityPerDay : 1f;
							StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(thingDef2, GenMath.RoundRandom(StartingPawnUtility.DaysSatisfied.RandomInRange * num)));
						}
					}
				}
			}
			if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
			{
				return;
			}
			if (ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Hemogenic))
			{
				StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(ThingDefOf.HemogenPack, StartingPawnUtility.HemogenCountRange.RandomInRange));
				if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
				{
					return;
				}
			}
			if (Rand.Value < 0.25f)
			{
				BackstoryDef backstory = pawn.story.GetBackstory(BackstorySlot.Adulthood);
				if (backstory != null)
				{
					foreach (PossessionThingDefCountClass possessionThingDefCountClass in backstory.possessions)
					{
						if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
						{
							return;
						}
						StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(possessionThingDefCountClass.key, Mathf.Clamp(possessionThingDefCountClass.value.RandomInRange, 1, possessionThingDefCountClass.key.stackLimit)));
					}
				}
			}
			if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
			{
				return;
			}
			if (Rand.Value < 0.25f)
			{
				foreach (Trait trait in pawn.story.traits.allTraits)
				{
					if (!trait.Suppressed)
					{
						foreach (PossessionThingDefCountClass possessionThingDefCountClass2 in trait.CurrentData.possessions)
						{
							if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
							{
								return;
							}
							StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(possessionThingDefCountClass2.key, Mathf.Clamp(possessionThingDefCountClass2.value.RandomInRange, 1, possessionThingDefCountClass2.key.stackLimit)));
						}
					}
				}
			}
			if (StartingPawnUtility.StartingPossessions[pawn].Count >= 2)
			{
				return;
			}
			if (Rand.Value < 0.06f)
			{
				ThingDef thingDef3;
				if ((from x in DefDatabase<ThingDef>.AllDefs
				where x.possessionCount > 0
				select x).TryRandomElement(out thingDef3))
				{
					StartingPawnUtility.StartingPossessions[pawn].Add(new ThingDefCount(thingDef3, Mathf.Min(thingDef3.stackLimit, thingDef3.possessionCount)));
				}
			}
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0004D840 File Offset: 0x0004BA40
		public static void AddNewPawn(int index = -1)
		{
			Pawn pawn = StartingPawnUtility.NewGeneratedStartingPawn(index);
			StartingPawnUtility.StartingAndOptionalPawns.Add(pawn);
			StartingPawnUtility.GeneratePossessions(pawn);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0004D868 File Offset: 0x0004BA68
		public static bool WorkTypeRequirementsSatisfied()
		{
			if (StartingPawnUtility.StartingAndOptionalPawns.Count == 0)
			{
				return false;
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (workTypeDef.requireCapableColonist)
				{
					bool flag = false;
					for (int j = 0; j < Find.GameInitData.startingPawnCount; j++)
					{
						if (!StartingPawnUtility.StartingAndOptionalPawns[j].WorkTypeIsDisabled(workTypeDef))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			if (TutorSystem.TutorialMode)
			{
				if (StartingPawnUtility.StartingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount).Any((Pawn p) => p.WorkTagIsDisabled(WorkTags.Violent)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0004D926 File Offset: 0x0004BB26
		public static IEnumerable<WorkTypeDef> RequiredWorkTypesDisabledForEveryone()
		{
			return new StartingPawnUtility.<RequiredWorkTypesDisabledForEveryone>d__38(-2);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0004D930 File Offset: 0x0004BB30
		public static Thing GenerateStartingPossession(ThingDefCount t)
		{
			Thing thing = ThingMaker.MakeThing(t.ThingDef, GenStuff.RandomStuffFor(t.ThingDef));
			if (thing.def.Minifiable)
			{
				thing = thing.MakeMinified(DestroyMode.Vanish);
			}
			if (t.ThingDef.IsIngestible && t.ThingDef.ingestible.IsMeal)
			{
				FoodUtility.GenerateGoodIngredients(thing, Faction.OfPlayer.ideos.PrimaryIdeo);
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(QualityUtility.GenerateQualityRandomEqualChance(), new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			thing.stackCount = t.Count;
			return thing;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0004DA4C File Offset: 0x0004BC4C
		[CompilerGenerated]
		internal static ThingDef <GeneratePossessions>g__GetDrugFor|35_1(ChemicalDef chemical)
		{
			ThingDef result;
			if (DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef x)
			{
				CompProperties_Drug compProperties = x.GetCompProperties<CompProperties_Drug>();
				return ((compProperties != null) ? compProperties.chemical : null) == chemical;
			}).TryRandomElementByWeight((ThingDef x) => x.generateCommonality, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04000E81 RID: 3713
		private const float ChanceToHavePossessionsFromBackground = 0.25f;

		// Token: 0x04000E82 RID: 3714
		private const float ChanceToHavePossessionsFromTraits = 0.25f;

		// Token: 0x04000E83 RID: 3715
		private static readonly IntRange BabyFoodCountRange = new IntRange(30, 40);

		// Token: 0x04000E84 RID: 3716
		private static readonly IntRange HemogenCountRange = new IntRange(8, 12);

		// Token: 0x04000E85 RID: 3717
		private static readonly FloatRange ExcludeBiologicalAgeRange = new FloatRange(12.1f, 13f);

		// Token: 0x04000E86 RID: 3718
		private const float RightRectLeftPadding = 5f;

		// Token: 0x04000E87 RID: 3719
		public static readonly Vector2 PawnPortraitSize = new Vector2(92f, 128f);

		// Token: 0x04000E88 RID: 3720
		private const int SkillSummaryColumns = 4;

		// Token: 0x04000E89 RID: 3721
		private static List<PawnGenerationRequest> StartingAndOptionalPawnGenerationRequests = new List<PawnGenerationRequest>();

		// Token: 0x04000E8A RID: 3722
		private static float listScrollViewHeight = 0f;

		// Token: 0x04000E8B RID: 3723
		private static Vector2 listScrollPosition;

		// Token: 0x04000E8C RID: 3724
		private static int SkillsPerColumn = -1;

		// Token: 0x04000E8D RID: 3725
		private const int MaxPossessionsCount = 2;

		// Token: 0x04000E8E RID: 3726
		private static readonly FloatRange DaysSatisfied = new FloatRange(25f, 35f);

		// Token: 0x04000E8F RID: 3727
		private const float ChanceForRandomPossession = 0.06f;
	}
}
