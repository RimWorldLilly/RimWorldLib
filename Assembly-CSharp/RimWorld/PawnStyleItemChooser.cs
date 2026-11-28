using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D4 RID: 5332
	public static class PawnStyleItemChooser
	{
		// Token: 0x06008259 RID: 33369 RVA: 0x002EB604 File Offset: 0x002E9804
		public static HairDef RandomHairFor(Pawn pawn)
		{
			if (pawn.kindDef.forcedHair != null)
			{
				return pawn.kindDef.forcedHair;
			}
			return PawnStyleItemChooser.ChooseStyleItem<HairDef>(pawn, HairDefOf.Bald, null);
		}

		// Token: 0x0600825A RID: 33370 RVA: 0x002EB640 File Offset: 0x002E9840
		public static BeardDef RandomBeardFor(Pawn pawn)
		{
			if (!pawn.style.CanWantBeard)
			{
				return BeardDefOf.NoBeard;
			}
			return PawnStyleItemChooser.ChooseStyleItem<BeardDef>(pawn, BeardDefOf.NoBeard, null);
		}

		// Token: 0x0600825B RID: 33371 RVA: 0x002EB674 File Offset: 0x002E9874
		public static TattooDef RandomTattooFor(Pawn pawn, TattooType tattooType)
		{
			TattooDef tattooDef = (tattooType == TattooType.Face) ? TattooDefOf.NoTattoo_Face : TattooDefOf.NoTattoo_Body;
			if (!ModsConfig.IdeologyActive)
			{
				return tattooDef;
			}
			if (pawn.Ideo == null && Rand.Chance(0.9f))
			{
				return tattooDef;
			}
			if (pawn.Ideo == null && Rand.Chance(0.9f))
			{
				return tattooDef;
			}
			return PawnStyleItemChooser.ChooseStyleItem<TattooDef>(pawn, tattooDef, new TattooType?(tattooType));
		}

		// Token: 0x0600825C RID: 33372 RVA: 0x002EB6D4 File Offset: 0x002E98D4
		private static T ChooseStyleItem<T>(Pawn pawn, T fallback, TattooType? tattooType = null) where T : StyleItemDef
		{
			PawnStyleItemChooser.tmpStyleItemDefs.Clear();
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				if (PawnStyleItemChooser.WantsToUseStyle(pawn, t, tattooType))
				{
					PawnStyleItemChooser.tmpStyleItemDefs.Add(t);
				}
			}
			StyleItemDef styleItemDef;
			if (PawnStyleItemChooser.tmpStyleItemDefs.TryRandomElementByWeight((StyleItemDef s) => PawnStyleItemChooser.TotalStyleItemLikelihood(s, pawn), out styleItemDef))
			{
				return (T)((object)styleItemDef);
			}
			return fallback;
		}

		// Token: 0x0600825D RID: 33373 RVA: 0x002EB778 File Offset: 0x002E9978
		public static bool WantsToUseStyle(Pawn pawn, StyleItemDef styleItemDef, TattooType? tattooType = null)
		{
			if (pawn.genes != null && ModsConfig.BiotechActive && !pawn.genes.StyleItemAllowed(styleItemDef))
			{
				return false;
			}
			if (ModsConfig.AnomalyActive && pawn.IsMutant && !pawn.mutant.Def.StyleItemAllowed(styleItemDef))
			{
				return false;
			}
			if (ModsConfig.AnomalyActive && pawn.IsCreepJoiner && !pawn.creepjoiner.form.StyleItemAllowed(styleItemDef))
			{
				return false;
			}
			TattooDef tattooDef = styleItemDef as TattooDef;
			if (tattooDef != null)
			{
				if (!ModLister.CheckIdeology("Tattoos"))
				{
					return false;
				}
				if (tattooType != null && tattooDef.tattooType != tattooType.Value)
				{
					return false;
				}
			}
			else if (styleItemDef is BeardDef)
			{
				if (!pawn.style.CanWantBeard)
				{
					return styleItemDef == BeardDefOf.NoBeard;
				}
			}
			else
			{
				HairDef hairDef = styleItemDef as HairDef;
				if (hairDef != null && !PawnStyleItemChooser.AgeAppropriateHairStyle(pawn, hairDef))
				{
					return false;
				}
			}
			if (pawn.Ideo == null || Find.IdeoManager.classicMode || pawn.Ideo.style.GetFrequency(styleItemDef) != StyleItemFrequency.Never)
			{
				return true;
			}
			if (pawn.genes != null && ModsConfig.BiotechActive && (styleItemDef is HairDef || styleItemDef is BeardDef))
			{
				List<Gene> genesListForReading = pawn.genes.GenesListForReading;
				for (int i = 0; i < genesListForReading.Count; i++)
				{
					if (genesListForReading[i].Active)
					{
						if (styleItemDef is HairDef && (genesListForReading[i].def.forcedHair != null || genesListForReading[i].def.hairTagFilter != null))
						{
							return true;
						}
						if (styleItemDef is BeardDef && genesListForReading[i].def.beardTagFilter != null)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600825E RID: 33374 RVA: 0x002EB918 File Offset: 0x002E9B18
		public static bool AgeAppropriateHairStyle(Pawn pawn, HairDef hair)
		{
			LifeStageDef curLifeStage = pawn.ageTracker.CurLifeStage;
			return curLifeStage.hairStyleFilter == null || curLifeStage.hairStyleFilter.Allows(hair.styleTags);
		}

		// Token: 0x0600825F RID: 33375 RVA: 0x002EB950 File Offset: 0x002E9B50
		private static float FrequencyFromPawnIdentity(Pawn pawn, StyleItemDef styleItem)
		{
			if (!pawn.kindDef.styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
			{
				return PawnStyleItemChooser.StyleItemChoiceLikelihoodFromTags(styleItem, pawn.kindDef.styleItemTags);
			}
			if (ModsConfig.AnomalyActive && pawn.IsMutant && pawn.mutant.Def.hairTagFilter != null)
			{
				return 1f;
			}
			if (!ModsConfig.IdeologyActive || Find.IdeoManager.classicMode)
			{
				PawnStyleItemChooser.tmpStyleItemTags.Clear();
				if (pawn.HomeFaction == null || pawn.HomeFaction.def.allowedCultures.NullOrEmpty<CultureDef>())
				{
					return 1f;
				}
				foreach (CultureDef cultureDef in pawn.HomeFaction.def.allowedCultures)
				{
					if (!cultureDef.styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
					{
						PawnStyleItemChooser.tmpStyleItemTags.AddRange(cultureDef.styleItemTags);
					}
				}
				if (!PawnStyleItemChooser.tmpStyleItemTags.Any<StyleItemTagWeighted>())
				{
					return 1f;
				}
				return PawnStyleItemChooser.StyleItemChoiceLikelihoodFromTags(styleItem, PawnStyleItemChooser.tmpStyleItemTags);
			}
			else
			{
				if (pawn.Ideo == null)
				{
					return 1f;
				}
				return pawn.Ideo.style.GetFrequency(styleItem).GetFrequencyFloat();
			}
		}

		// Token: 0x06008260 RID: 33376 RVA: 0x002EBA98 File Offset: 0x002E9C98
		public static float StyleItemChoiceLikelihoodFromTags(StyleItemDef styleItem, List<StyleItemTagWeighted> tags)
		{
			StyleItemTagWeighted styleItemTagWeighted = new StyleItemTagWeighted("", 0f, 1f);
			int i;
			Predicate<StyleItemTagWeighted> <>9__0;
			int j;
			for (i = 0; i < styleItem.styleTags.Count; i = j + 1)
			{
				Predicate<StyleItemTagWeighted> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((StyleItemTagWeighted x) => x.Tag == styleItem.styleTags[i]));
				}
				StyleItemTagWeighted styleItemTagWeighted2 = tags.Find(match);
				if (styleItemTagWeighted2 != null)
				{
					styleItemTagWeighted.Add(styleItemTagWeighted2);
				}
				j = i;
			}
			return Mathf.Max(0f, styleItemTagWeighted.TotalWeight);
		}

		// Token: 0x06008261 RID: 33377 RVA: 0x002EBB3C File Offset: 0x002E9D3C
		public static float GetFrequencyFloat(this StyleItemFrequency styleItemFrequency)
		{
			switch (styleItemFrequency)
			{
			case StyleItemFrequency.Never:
				return 0f;
			case StyleItemFrequency.Rare:
				return 0.1f;
			case StyleItemFrequency.Uncommon:
				return 0.5f;
			case StyleItemFrequency.Normal:
				return 1f;
			case StyleItemFrequency.Common:
				return 2f;
			case StyleItemFrequency.Frequent:
				return 10f;
			default:
				Log.Error("Unknown StyleItemFrequency value.");
				return 0f;
			}
		}

		// Token: 0x06008262 RID: 33378 RVA: 0x002EBB9C File Offset: 0x002E9D9C
		public static string GetLabel(this StyleItemFrequency styleItemFrequency)
		{
			switch (styleItemFrequency)
			{
			case StyleItemFrequency.Never:
				return "StyleItemFrequencyNever".Translate();
			case StyleItemFrequency.Rare:
				return "StyleItemFrequencyRare".Translate();
			case StyleItemFrequency.Uncommon:
				return "StyleItemFrequencyUncommon".Translate();
			case StyleItemFrequency.Normal:
				return "StyleItemFrequencyNormal".Translate();
			case StyleItemFrequency.Common:
				return "StyleItemFrequencyCommon".Translate();
			case StyleItemFrequency.Frequent:
				return "StyleItemFrequencyFrequent".Translate();
			default:
				return "Unknown label";
			}
		}

		// Token: 0x06008263 RID: 33379 RVA: 0x002EBC2E File Offset: 0x002E9E2E
		public static StyleItemFrequency GetStyleItemFrequency(float freq)
		{
			if (freq <= 0f)
			{
				return StyleItemFrequency.Never;
			}
			if (freq <= 0.1f)
			{
				return StyleItemFrequency.Rare;
			}
			if (freq <= 0.5f)
			{
				return StyleItemFrequency.Uncommon;
			}
			if (freq <= 1f)
			{
				return StyleItemFrequency.Normal;
			}
			if (freq <= 2f)
			{
				return StyleItemFrequency.Common;
			}
			return StyleItemFrequency.Frequent;
		}

		// Token: 0x06008264 RID: 33380 RVA: 0x002EBC63 File Offset: 0x002E9E63
		public static float TotalStyleItemLikelihood(StyleItemDef styleItem, Pawn pawn)
		{
			return PawnStyleItemChooser.FrequencyFromGender(styleItem, pawn) * PawnStyleItemChooser.FrequencyFromPawnIdentity(pawn, styleItem);
		}

		// Token: 0x06008265 RID: 33381 RVA: 0x002EBC74 File Offset: 0x002E9E74
		public static float FrequencyFromGender(StyleItemDef styleItem, Pawn pawn)
		{
			if (ModsConfig.AnomalyActive && styleItem.requiredMutant != null)
			{
				Pawn_MutantTracker mutant = pawn.mutant;
				if (((mutant != null) ? mutant.Def : null) != styleItem.requiredMutant)
				{
					return 0f;
				}
			}
			if (pawn.gender == Gender.None || pawn.Ideo == null)
			{
				return 100f;
			}
			if (ModsConfig.BiotechActive && pawn.genes != null && styleItem.requiredGene != null && !pawn.genes.HasActiveGene(styleItem.requiredGene))
			{
				return 0f;
			}
			StyleGender gender = pawn.Ideo.style.GetGender(styleItem);
			if (pawn.gender == Gender.Male)
			{
				switch (gender)
				{
				case StyleGender.Male:
					return 70f;
				case StyleGender.MaleUsually:
					return 30f;
				case StyleGender.Any:
					return 60f;
				case StyleGender.FemaleUsually:
					return 5f;
				case StyleGender.Female:
					return 1f;
				}
			}
			if (pawn.gender == Gender.Female)
			{
				switch (gender)
				{
				case StyleGender.Male:
					return 1f;
				case StyleGender.MaleUsually:
					return 5f;
				case StyleGender.Any:
					return 60f;
				case StyleGender.FemaleUsually:
					return 30f;
				case StyleGender.Female:
					return 70f;
				}
			}
			Log.Error("Unknown hair likelihood for " + ((styleItem != null) ? styleItem.ToString() : null) + " with " + ((pawn != null) ? pawn.ToString() : null));
			return 0f;
		}

		// Token: 0x040050C4 RID: 20676
		public const float StyleItemFrequencyNever = 0f;

		// Token: 0x040050C5 RID: 20677
		public const float StyleItemFrequencyRare = 0.1f;

		// Token: 0x040050C6 RID: 20678
		public const float StyleItemFrequencyUncommon = 0.5f;

		// Token: 0x040050C7 RID: 20679
		public const float StyleItemFrequencyNormal = 1f;

		// Token: 0x040050C8 RID: 20680
		public const float StyleItemFrequencyCommon = 2f;

		// Token: 0x040050C9 RID: 20681
		public const float StyleItemFrequencyFrequent = 10f;

		// Token: 0x040050CA RID: 20682
		private static List<StyleItemDef> tmpStyleItemDefs = new List<StyleItemDef>();

		// Token: 0x040050CB RID: 20683
		private static List<StyleItemTagWeighted> tmpStyleItemTags = new List<StyleItemTagWeighted>();
	}
}
