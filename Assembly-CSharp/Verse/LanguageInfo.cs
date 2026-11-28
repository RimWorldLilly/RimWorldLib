using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000211 RID: 529
	public class LanguageInfo
	{
		// Token: 0x04000EBB RID: 3771
		public string friendlyNameNative;

		// Token: 0x04000EBC RID: 3772
		public string friendlyNameEnglish;

		// Token: 0x04000EBD RID: 3773
		public bool canBeTiny = true;

		// Token: 0x04000EBE RID: 3774
		public List<CreditsEntry> credits = new List<CreditsEntry>();

		// Token: 0x04000EBF RID: 3775
		public Type languageWorkerClass = typeof(LanguageWorker_Default);

		// Token: 0x04000EC0 RID: 3776
		public int? totalNumCaseCount;
	}
}
