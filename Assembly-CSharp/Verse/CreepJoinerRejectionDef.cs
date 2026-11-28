using System;

namespace Verse
{
	// Token: 0x0200012B RID: 299
	public class CreepJoinerRejectionDef : CreepJoinerBaseDef
	{
		// Token: 0x04000534 RID: 1332
		public bool hasLetter;

		// Token: 0x04000535 RID: 1333
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04000536 RID: 1334
		[MustTranslate]
		public string letterDesc;

		// Token: 0x04000537 RID: 1335
		public LetterDef letterDef;

		// Token: 0x04000538 RID: 1336
		public Type workerType;
	}
}
