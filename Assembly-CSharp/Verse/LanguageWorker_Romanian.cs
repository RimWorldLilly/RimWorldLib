using System;

namespace Verse
{
	// Token: 0x02000222 RID: 546
	public class LanguageWorker_Romanian : LanguageWorker
	{
		// Token: 0x06000FD6 RID: 4054 RVA: 0x00059499 File Offset: 0x00057699
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (!plural)
			{
				return ((gender == Gender.Female) ? "a " : "un ") + str;
			}
			if (gender != Gender.Male)
			{
				return str + "e";
			}
			return str + "i";
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x000594D8 File Offset: 0x000576D8
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
			char ch = str[str.Length - 1];
			if (plural)
			{
				if (gender != Gender.Male)
				{
					return str + "e";
				}
				return str + "i";
			}
			else
			{
				if (!this.IsVowel(ch))
				{
					return str + "ul";
				}
				if (gender == Gender.Male)
				{
					return str + "le";
				}
				return str + "a";
			}
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00059553 File Offset: 0x00057753
		public bool IsVowel(char ch)
		{
			return "aeiouâîAEIOUÂÎ".IndexOf(ch) >= 0;
		}
	}
}
