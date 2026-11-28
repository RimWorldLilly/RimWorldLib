using System;

namespace Verse.Sound
{
	// Token: 0x020007CB RID: 1995
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06003FC3 RID: 16323
		public abstract string Label { get; }

		// Token: 0x06003FC4 RID: 16324
		public abstract float ValueFor(Sample samp);
	}
}
