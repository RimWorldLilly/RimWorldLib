using System;

namespace Verse
{
	// Token: 0x02000527 RID: 1319
	public enum LookMode : byte
	{
		// Token: 0x04001C14 RID: 7188
		Undefined,
		// Token: 0x04001C15 RID: 7189
		Value,
		// Token: 0x04001C16 RID: 7190
		Deep,
		// Token: 0x04001C17 RID: 7191
		Reference,
		// Token: 0x04001C18 RID: 7192
		Def,
		// Token: 0x04001C19 RID: 7193
		LocalTargetInfo,
		// Token: 0x04001C1A RID: 7194
		TargetInfo,
		// Token: 0x04001C1B RID: 7195
		GlobalTargetInfo,
		// Token: 0x04001C1C RID: 7196
		BodyPart
	}
}
