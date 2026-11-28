using System;

namespace Verse
{
	// Token: 0x0200021E RID: 542
	public class LanguageWorker_Italian : LanguageWorker
	{
		// Token: 0x06000FBF RID: 4031 RVA: 0x00058D18 File Offset: 0x00056F18
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (name)
			{
				return str;
			}
			char c = str[0];
			char c2 = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "un'" + str;
				}
				return "una " + str;
			}
			else
			{
				char c3 = char.ToLower(c);
				char c4 = char.ToLower(c2);
				if ((c == 's' || c == 'S') && !this.IsVowel(c2))
				{
					return "uno " + str;
				}
				if ((c3 == 'p' && c4 == 's') || (c3 == 'p' && c4 == 'n') || c3 == 'z' || c3 == 'x' || c3 == 'y' || (c3 == 'g' && c4 == 'n'))
				{
					return "uno " + str;
				}
				return "un " + str;
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00058DE8 File Offset: 0x00056FE8
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			if (name)
			{
				return str;
			}
			char c = str[0];
			char ch = (str.Length >= 2) ? str[1] : '\0';
			if (gender == Gender.Female)
			{
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "la " + str;
			}
			else
			{
				if (c == 'z' || c == 'Z')
				{
					return "lo " + str;
				}
				if ((c == 's' || c == 'S') && !this.IsVowel(ch))
				{
					return "lo " + str;
				}
				if (this.IsVowel(c))
				{
					return "l'" + str;
				}
				return "il " + str;
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00058E9B File Offset: 0x0005709B
		public bool IsVowel(char ch)
		{
			return "aeiouAEIOU".IndexOf(ch) >= 0;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00058EAE File Offset: 0x000570AE
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString() + "°";
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00058EC4 File Offset: 0x000570C4
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}
			string result;
			if (this.TryLookupPluralForm(str, gender, out result, count))
			{
				return result;
			}
			if (count != -1 && count < 2)
			{
				return str;
			}
			char ch = str[str.Length - 1];
			if (!this.IsVowel(ch))
			{
				return str;
			}
			if (gender == Gender.Female)
			{
				return str.Substring(0, str.Length - 1) + "e";
			}
			return str.Substring(0, str.Length - 1) + "i";
		}
	}
}
