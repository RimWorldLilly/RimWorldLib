using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006DD RID: 1757
	[StaticConstructorOnStartup]
	public static class GenderUtility
	{
		// Token: 0x06003799 RID: 14233 RVA: 0x00162AD2 File Offset: 0x00160CD2
		public static string GetGenderLabel(this Pawn pawn)
		{
			return pawn.gender.GetLabel(pawn.RaceProps.Animal);
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x00162AEC File Offset: 0x00160CEC
		public static string GetLabel(this Gender gender, bool animal = false)
		{
			switch (gender)
			{
			case Gender.None:
				return "NoneLower".Translate();
			case Gender.Male:
				return animal ? "MaleAnimal".Translate() : "Male".Translate();
			case Gender.Female:
				return animal ? "FemaleAnimal".Translate() : "Female".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x00162B60 File Offset: 0x00160D60
		public static string GetPronoun(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proit".Translate();
			case Gender.Male:
				return "Prohe".Translate();
			case Gender.Female:
				return "Proshe".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x00162BB8 File Offset: 0x00160DB8
		public static string GetPossessive(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proits".Translate();
			case Gender.Male:
				return "Prohis".Translate();
			case Gender.Female:
				return "Proher".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x00162C10 File Offset: 0x00160E10
		public static string GetObjective(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "ProitObj".Translate();
			case Gender.Male:
				return "ProhimObj".Translate();
			case Gender.Female:
				return "ProherObj".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x00162C68 File Offset: 0x00160E68
		public static string GetGenderNoun(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "ProItNoun".Translate();
			case Gender.Male:
				return "ProHeNoun".Translate();
			case Gender.Female:
				return "ProSheNoun".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x00162CBE File Offset: 0x00160EBE
		public static Texture2D GetIcon(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return GenderUtility.GenderlessIcon;
			case Gender.Male:
				return GenderUtility.MaleIcon;
			case Gender.Female:
				return GenderUtility.FemaleIcon;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x00162CEB File Offset: 0x00160EEB
		public static Gender Opposite(this Gender gender)
		{
			if (gender == Gender.Male)
			{
				return Gender.Female;
			}
			if (gender == Gender.Female)
			{
				return Gender.Male;
			}
			return Gender.None;
		}

		// Token: 0x04002453 RID: 9299
		private static readonly Texture2D GenderlessIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genderless", true);

		// Token: 0x04002454 RID: 9300
		private static readonly Texture2D MaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Male", true);

		// Token: 0x04002455 RID: 9301
		private static readonly Texture2D FemaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Female", true);
	}
}
