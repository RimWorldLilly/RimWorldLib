using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DD RID: 5341
	public static class PawnHairColors
	{
		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x0600829F RID: 33439 RVA: 0x002EE374 File Offset: 0x002EC574
		private static List<GeneDef> HairColorGenes
		{
			get
			{
				if (PawnHairColors.cachedHairColorGenes == null)
				{
					PawnHairColors.cachedHairColorGenes = (from x in DefDatabase<GeneDef>.AllDefs
					where x.hairColorOverride != null
					select x).ToList<GeneDef>();
				}
				return PawnHairColors.cachedHairColorGenes;
			}
		}

		// Token: 0x060082A0 RID: 33440 RVA: 0x002EE3C0 File Offset: 0x002EC5C0
		public static void ResetStaticData()
		{
			PawnHairColors.cachedHairColorGenes = null;
		}

		// Token: 0x060082A1 RID: 33441 RVA: 0x002EE3C8 File Offset: 0x002EC5C8
		public static Color RandomHairColor(Pawn pawn, Color skinColor, int ageYears)
		{
			if (Rand.Value < 0.02f)
			{
				return new Color(Rand.Value, Rand.Value, Rand.Value);
			}
			if (PawnHairColors.HasGreyHair(pawn, ageYears))
			{
				return PawnHairColors.RandomGreyHairColor();
			}
			if (PawnSkinColors.IsDarkSkin(skinColor) || Rand.Value < 0.5f)
			{
				float value = Rand.Value;
				if (value < 0.25f)
				{
					return PawnHairColors.DarkBlack;
				}
				if (value < 0.5f)
				{
					return PawnHairColors.MidBlack;
				}
				if (value < 0.75f)
				{
					return PawnHairColors.DarkReddish;
				}
				return PawnHairColors.DarkSaturatedReddish;
			}
			else
			{
				float value2 = Rand.Value;
				if (value2 < 0.25f)
				{
					return PawnHairColors.DarkBrown;
				}
				if (value2 < 0.5f)
				{
					return PawnHairColors.ReddishBrown;
				}
				if (value2 < 0.75f)
				{
					return PawnHairColors.SandyBlonde;
				}
				return PawnHairColors.Blonde;
			}
		}

		// Token: 0x060082A2 RID: 33442 RVA: 0x002EE484 File Offset: 0x002EC684
		public static bool HasGreyHair(Pawn pawn, int ageYears)
		{
			if (ageYears <= 40)
			{
				return false;
			}
			if (ModsConfig.BiotechActive && pawn.genes != null)
			{
				using (List<Gene>.Enumerator enumerator = pawn.genes.GenesListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.neverGrayHair)
						{
							return false;
						}
					}
				}
			}
			float num = GenMath.SmootherStep(40f, 75f, (float)ageYears);
			return Rand.Value < num;
		}

		// Token: 0x060082A3 RID: 33443 RVA: 0x002EE518 File Offset: 0x002EC718
		public static Color RandomGreyHairColor()
		{
			float num = Rand.Range(0.65f, 0.85f);
			return new Color(num, num, num);
		}

		// Token: 0x060082A4 RID: 33444 RVA: 0x002EE530 File Offset: 0x002EC730
		public static GeneDef RandomHairColorGeneFor(Pawn pawn)
		{
			return PawnHairColors.RandomHairColorGene(pawn.story.SkinColorBase, ModsConfig.AnomalyActive && pawn.Faction == Faction.OfHoraxCult);
		}

		// Token: 0x060082A5 RID: 33445 RVA: 0x002EE55C File Offset: 0x002EC75C
		private static GeneDef RandomHairColorGene(Color skinColor, bool cultist = false)
		{
			GeneDef result;
			if (PawnHairColors.HairColorGenes.TryRandomElementByWeight(delegate(GeneDef g)
			{
				float num = g.selectionWeight;
				if (PawnSkinColors.IsDarkSkin(skinColor))
				{
					num *= g.selectionWeightFactorDarkSkin;
				}
				if (cultist)
				{
					num *= g.selectionWeightCultist;
				}
				return num;
			}, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060082A6 RID: 33446 RVA: 0x002EE59C File Offset: 0x002EC79C
		public static GeneDef ClosestHairColorGene(Color hairColor, Color skinColor)
		{
			foreach (GeneDef geneDef in PawnHairColors.HairColorGenes)
			{
				if (hairColor.IndistinguishableFrom(geneDef.hairColorOverride.Value))
				{
					return geneDef;
				}
			}
			return PawnHairColors.RandomHairColorGene(skinColor, false);
		}

		// Token: 0x040050EB RID: 20715
		public static readonly Color DarkBlack = new Color(0.2f, 0.2f, 0.2f);

		// Token: 0x040050EC RID: 20716
		public static readonly Color MidBlack = new Color(0.31f, 0.28f, 0.26f);

		// Token: 0x040050ED RID: 20717
		public static readonly Color DarkReddish = new Color(0.25f, 0.2f, 0.15f);

		// Token: 0x040050EE RID: 20718
		public static readonly Color DarkSaturatedReddish = new Color(0.3f, 0.2f, 0.1f);

		// Token: 0x040050EF RID: 20719
		public static readonly Color DarkBrown = new Color(0.3529412f, 0.22745098f, 0.1254902f);

		// Token: 0x040050F0 RID: 20720
		public static readonly Color ReddishBrown = new Color(0.5176471f, 0.3254902f, 0.18431373f);

		// Token: 0x040050F1 RID: 20721
		public static readonly Color SandyBlonde = new Color(0.75686276f, 0.57254905f, 0.33333334f);

		// Token: 0x040050F2 RID: 20722
		public static readonly Color Blonde = new Color(0.92941177f, 0.7921569f, 0.6117647f);

		// Token: 0x040050F3 RID: 20723
		private static List<GeneDef> cachedHairColorGenes;
	}
}
