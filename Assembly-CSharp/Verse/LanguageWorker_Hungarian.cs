using System;

namespace Verse
{
	// Token: 0x0200021D RID: 541
	public class LanguageWorker_Hungarian : LanguageWorker
	{
		// Token: 0x06000FBB RID: 4027 RVA: 0x00058CA3 File Offset: 0x00056EA3
		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (name)
			{
				return str;
			}
			return "egy " + str;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00058CB8 File Offset: 0x00056EB8
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
			char ch = str[0];
			if (this.IsVowel(ch))
			{
				return "az " + str;
			}
			return "a " + str;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00058CFD File Offset: 0x00056EFD
		public bool IsVowel(char ch)
		{
			return "eéöőüűiíaáoóuúEÉÖŐÜŰIÍAÁOÓUÚ".IndexOf(ch) >= 0;
		}
	}
}
