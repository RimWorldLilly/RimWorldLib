using System;

namespace Verse
{
	// Token: 0x02000219 RID: 537
	public class LanguageWorker_Dutch : LanguageWorker
	{
		// Token: 0x06000F9F RID: 3999 RVA: 0x00057FDD File Offset: 0x000561DD
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return str;
			}
			return "een " + str;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00057FF5 File Offset: 0x000561F5
		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			if (plural)
			{
				return "de " + str;
			}
			if (gender == Gender.Male || gender == Gender.Female)
			{
				return "de " + str;
			}
			return "het " + str;
		}
	}
}
