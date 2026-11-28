using System;

namespace Verse.Sound
{
	// Token: 0x020007CD RID: 1997
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06003FC9 RID: 16329 RVA: 0x00197E48 File Offset: 0x00196048
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x00197E4F File Offset: 0x0019604F
		public override float ValueFor(Sample samp)
		{
			if (samp.ExternalParams.sizeAggregator == null)
			{
				return 0f;
			}
			return samp.ExternalParams.sizeAggregator.AggregateSize;
		}
	}
}
