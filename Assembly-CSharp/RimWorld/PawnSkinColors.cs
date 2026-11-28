using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EE RID: 5358
	public static class PawnSkinColors
	{
		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x0600830F RID: 33551 RVA: 0x002F103C File Offset: 0x002EF23C
		public static List<GeneDef> SkinColorGenesInOrder
		{
			get
			{
				if (PawnSkinColors.skinColorGenes == null)
				{
					PawnSkinColors.skinColorGenes = new List<GeneDef>();
					foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefs)
					{
						if ((geneDef.endogeneCategory == EndogeneCategory.Melanin || geneDef.minMelanin < 0f) && geneDef.skinColorBase != null)
						{
							PawnSkinColors.skinColorGenes.Add(geneDef);
						}
					}
					PawnSkinColors.skinColorGenes.SortBy((GeneDef x) => x.minMelanin);
				}
				return PawnSkinColors.skinColorGenes;
			}
		}

		// Token: 0x06008310 RID: 33552 RVA: 0x002F10F4 File Offset: 0x002EF2F4
		public static void ResetStaticData()
		{
			PawnSkinColors.skinColorGenes = null;
		}

		// Token: 0x06008311 RID: 33553 RVA: 0x002F10FC File Offset: 0x002EF2FC
		public static bool IsDarkSkin(Color color)
		{
			Color skinColor = PawnSkinColors.GetSkinColor(0.5f);
			return color.r + color.g + color.b <= skinColor.r + skinColor.g + skinColor.b + 0.01f;
		}

		// Token: 0x06008312 RID: 33554 RVA: 0x002F1147 File Offset: 0x002EF347
		public static Color GetSkinColor(float melanin)
		{
			return PawnSkinColors.GetSkinColorGene(melanin).skinColorBase.Value;
		}

		// Token: 0x06008313 RID: 33555 RVA: 0x002F115C File Offset: 0x002EF35C
		public static GeneDef GetSkinColorGene(float melanin)
		{
			List<GeneDef> skinColorGenesInOrder = PawnSkinColors.SkinColorGenesInOrder;
			for (int i = skinColorGenesInOrder.Count - 1; i >= 0; i--)
			{
				if (melanin >= skinColorGenesInOrder[i].minMelanin)
				{
					return skinColorGenesInOrder[i];
				}
			}
			return skinColorGenesInOrder.RandomElement<GeneDef>();
		}

		// Token: 0x06008314 RID: 33556 RVA: 0x002F11A0 File Offset: 0x002EF3A0
		public static GeneDef RandomSkinColorGene(Pawn pawn)
		{
			if (pawn.Faction != null)
			{
				return PawnSkinColors.GetSkinColorGene(pawn.Faction.def.melaninRange.RandomInRange);
			}
			return PawnSkinColors.SkinColorGenesInOrder.RandomElementByWeight((GeneDef x) => x.selectionWeight);
		}

		// Token: 0x06008315 RID: 33557 RVA: 0x002F11FC File Offset: 0x002EF3FC
		public static int SkinColorIndex(Pawn pawn)
		{
			if (((pawn != null) ? pawn.genes : null) == null)
			{
				return -1;
			}
			GeneDef firstEndogeneByCategory = pawn.genes.GetFirstEndogeneByCategory(EndogeneCategory.Melanin);
			if (firstEndogeneByCategory != null)
			{
				return PawnSkinColors.SkinColorGenesInOrder.IndexOf(firstEndogeneByCategory);
			}
			return -1;
		}

		// Token: 0x06008316 RID: 33558 RVA: 0x002F1238 File Offset: 0x002EF438
		public static List<GeneDef> SkinColorsFromParents(Pawn father, Pawn mother)
		{
			PawnSkinColors.tmpSkinColorGenes.Clear();
			if (father == null && mother == null)
			{
				return PawnSkinColors.tmpSkinColorGenes;
			}
			int num = PawnSkinColors.SkinColorIndex(father);
			int num2 = PawnSkinColors.SkinColorIndex(mother);
			if (num >= 0 && num2 < 0)
			{
				PawnSkinColors.tmpSkinColorGenes.Add(father.genes.GetFirstEndogeneByCategory(EndogeneCategory.Melanin));
			}
			else if (num2 >= 0 && num < 0)
			{
				PawnSkinColors.tmpSkinColorGenes.Add(mother.genes.GetFirstEndogeneByCategory(EndogeneCategory.Melanin));
			}
			else if (num >= 0 && num2 >= 0)
			{
				int num3 = Mathf.Min(num, num2);
				int num4 = Mathf.Max(num, num2);
				for (int i = num3; i <= num4; i++)
				{
					PawnSkinColors.tmpSkinColorGenes.Add(PawnSkinColors.SkinColorGenesInOrder[i]);
				}
			}
			return PawnSkinColors.tmpSkinColorGenes;
		}

		// Token: 0x04005129 RID: 20777
		private static List<GeneDef> skinColorGenes;

		// Token: 0x0400512A RID: 20778
		private static List<GeneDef> tmpSkinColorGenes = new List<GeneDef>();
	}
}
