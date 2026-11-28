using System;

namespace Verse
{
	// Token: 0x02000215 RID: 533
	public class LanguageWorker_Catalan : LanguageWorker
	{
		// Token: 0x06000F87 RID: 3975 RVA: 0x00057553 File Offset: 0x00055753
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "unes " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00057593 File Offset: 0x00055793
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return this.WithElLaArticle(str, gender, true);
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "les " : "els ") + str;
			}
			return this.WithElLaArticle(str, gender, false);
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x000575C8 File Offset: 0x000557C8
		private string WithElLaArticle(string str, Gender gender, bool name)
		{
			if (str.Length == 0 || (!this.IsVowel(str[0]) && str[0] != 'h' && str[0] != 'H'))
			{
				return ((gender == Gender.Female) ? "la " : "el ") + str;
			}
			if (name)
			{
				return ((gender == Gender.Female) ? "l'" : "n'") + str;
			}
			return "l'" + str;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00057640 File Offset: 0x00055840
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			if (gender == Gender.Female)
			{
				return number.ToString() + "a";
			}
			if (number == 1 || number == 3)
			{
				return number.ToString() + "r";
			}
			if (number == 2)
			{
				return number.ToString() + "n";
			}
			if (number == 4)
			{
				return number.ToString() + "t";
			}
			return number.ToString() + "è";
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x000576BA File Offset: 0x000558BA
		public bool IsVowel(char ch)
		{
			return "ieɛaoɔuəuàêèéòóüúIEƐAOƆUƏUÀÊÈÉÒÓÜÚ".IndexOf(ch) >= 0;
		}
	}
}
