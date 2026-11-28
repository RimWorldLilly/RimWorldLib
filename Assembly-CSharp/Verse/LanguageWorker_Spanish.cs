using System;

namespace Verse
{
	// Token: 0x02000224 RID: 548
	public class LanguageWorker_Spanish : LanguageWorker
	{
		// Token: 0x06000FE1 RID: 4065 RVA: 0x00059B85 File Offset: 0x00057D85
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "unas " : "unos ") + str;
			}
			return ((gender == Gender.Female) ? "una " : "un ") + str;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00059BBD File Offset: 0x00057DBD
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "las " : "los ") + str;
			}
			return ((gender == Gender.Female) ? "la " : "el ") + str;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00059BF5 File Offset: 0x00057DF5
		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number.ToString() + ".º";
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x00059C08 File Offset: 0x00057E08
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
			char c = str[str.Length - 1];
			char c2 = (str.Length >= 2) ? str[str.Length - 2] : '\0';
			if (this.IsVowel(c))
			{
				if (str == "sí")
				{
					return "síes";
				}
				if (c == 'í' || c == 'ú' || c == 'Í' || c == 'Ú')
				{
					return str + "es";
				}
				return str + "s";
			}
			else
			{
				if ((c == 'y' || c == 'Y') && this.IsVowel(c2))
				{
					return str + "es";
				}
				if ("lrndjsxLRNDJSX".IndexOf(c) >= 0 || (c == 'h' && c2 == 'c'))
				{
					return str + "es";
				}
				if (c == 'z' || c == 'Z')
				{
					string value = str.Substring(0, str.Length - 1);
					char c3 = (c == 'z') ? 'c' : 'C';
					ReadOnlySpan<char> str2 = value;
					char c4 = c3;
					return str2 + new ReadOnlySpan<char>(ref c4) + "es";
				}
				return str + "s";
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x00059D48 File Offset: 0x00057F48
		public bool IsVowel(char ch)
		{
			return "aeiouáéíóúAEIOUÁÉÍÓÚ".IndexOf(ch) >= 0;
		}
	}
}
