using System;

namespace Verse
{
	// Token: 0x020001B4 RID: 436
	public class RoomStatScoreStage
	{
		// Token: 0x06000C1E RID: 3102 RVA: 0x0004047C File Offset: 0x0003E67C
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x04000B77 RID: 2935
		public float minScore = float.MinValue;

		// Token: 0x04000B78 RID: 2936
		public string label;

		// Token: 0x04000B79 RID: 2937
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;
	}
}
