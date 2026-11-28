using System;

namespace Verse
{
	// Token: 0x02000226 RID: 550
	public class LanguageWorker_Turkish : LanguageWorker
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x00059E1C File Offset: 0x0005801C
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return "bir " + str;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00059E2F File Offset: 0x0005802F
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			return str;
		}
	}
}
