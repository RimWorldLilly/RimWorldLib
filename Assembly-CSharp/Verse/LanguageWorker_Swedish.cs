using System;

namespace Verse
{
	// Token: 0x02000225 RID: 549
	public class LanguageWorker_Swedish : LanguageWorker
	{
		// Token: 0x06000FE7 RID: 4071 RVA: 0x00059D63 File Offset: 0x00057F63
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "en " + str;
			}
			return "ett " + str;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00059D8C File Offset: 0x00057F8C
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
			if (gender == Gender.Male || gender == Gender.Female)
			{
				if (this.IsVowel(ch))
				{
					return str + "n";
				}
				return str + "en";
			}
			else
			{
				if (this.IsVowel(ch))
				{
					return str + "t";
				}
				return str + "et";
			}
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00059E01 File Offset: 0x00058001
		public bool IsVowel(char ch)
		{
			return "aeiouyåäöAEIOUYÅÄÖ".IndexOf(ch) >= 0;
		}
	}
}
