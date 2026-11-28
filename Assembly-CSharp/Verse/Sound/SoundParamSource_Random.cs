using System;

namespace Verse.Sound
{
	// Token: 0x020007D6 RID: 2006
	public class SoundParamSource_Random : SoundParamSource
	{
		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06003FDF RID: 16351 RVA: 0x001980AE File Offset: 0x001962AE
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x001980B5 File Offset: 0x001962B5
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
