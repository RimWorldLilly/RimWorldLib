using System;

namespace Verse.Sound
{
	// Token: 0x020007D9 RID: 2009
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06003FE9 RID: 16361 RVA: 0x0019811D File Offset: 0x0019631D
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x00198124 File Offset: 0x00196324
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
