using System;

namespace Verse
{
	// Token: 0x02000526 RID: 1318
	public enum LoadSaveMode : byte
	{
		// Token: 0x04001C0E RID: 7182
		Inactive,
		// Token: 0x04001C0F RID: 7183
		Saving,
		// Token: 0x04001C10 RID: 7184
		LoadingVars,
		// Token: 0x04001C11 RID: 7185
		ResolvingCrossRefs,
		// Token: 0x04001C12 RID: 7186
		PostLoadInit
	}
}
