using System;

namespace Verse
{
	// Token: 0x0200012A RID: 298
	public class CreepJoinerAggressiveDef : CreepJoinerBaseDef
	{
		// Token: 0x0400052D RID: 1325
		public bool hasMessage;

		// Token: 0x0400052E RID: 1326
		[MustTranslate]
		public string message;

		// Token: 0x0400052F RID: 1327
		public bool hasLetter;

		// Token: 0x04000530 RID: 1328
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04000531 RID: 1329
		[MustTranslate]
		public string letterDesc;

		// Token: 0x04000532 RID: 1330
		public LetterDef letterDef;

		// Token: 0x04000533 RID: 1331
		public Type workerType;
	}
}
