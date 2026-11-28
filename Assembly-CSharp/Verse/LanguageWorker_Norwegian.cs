using System;

namespace Verse
{
	// Token: 0x02000220 RID: 544
	public class LanguageWorker_Norwegian : LanguageWorker
	{
		// Token: 0x06000FD0 RID: 4048 RVA: 0x00059383 File Offset: 0x00057583
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
			return "et " + str;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x000593AC File Offset: 0x000575AC
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
			char c = str[str.Length - 1];
			if (gender == Gender.Male || gender == Gender.Female)
			{
				if (c == 'e')
				{
					return str + "n";
				}
				return str + "en";
			}
			else
			{
				if (c == 'e')
				{
					return str + "t";
				}
				return str + "et";
			}
		}
	}
}
