using System;

namespace Verse.Sound
{
	// Token: 0x020007D8 RID: 2008
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06003FE5 RID: 16357
		public abstract string Label { get; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06003FE6 RID: 16358 RVA: 0x00198112 File Offset: 0x00196312
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003FE7 RID: 16359
		public abstract void SetOn(Sample sample, float value);
	}
}
