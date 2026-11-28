using System;

namespace Verse
{
	// Token: 0x02000221 RID: 545
	public class LanguageWorker_Portuguese : LanguageWorker
	{
		// Token: 0x06000FD3 RID: 4051 RVA: 0x00059421 File Offset: 0x00057621
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "umas " : "uns ") + str;
			}
			return ((gender == Gender.Female) ? "uma " : "um ") + str;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00059459 File Offset: 0x00057659
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return ((gender == Gender.Female) ? "as " : "os ") + str;
			}
			return ((gender == Gender.Female) ? "a " : "o ") + str;
		}
	}
}
