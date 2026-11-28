using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007E1 RID: 2017
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06003FF7 RID: 16375 RVA: 0x0019829A File Offset: 0x0019649A
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty.ToString();
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x001982B7 File Offset: 0x001964B7
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x001982C4 File Offset: 0x001964C4
		public override void SetOn(Sample sample, float value)
		{
			AudioEchoFilter audioEchoFilter = sample.source.GetComponent<AudioEchoFilter>();
			if (audioEchoFilter == null)
			{
				audioEchoFilter = sample.source.gameObject.AddComponent<AudioEchoFilter>();
			}
			if (this.filterProperty == EchoFilterProperty.Delay)
			{
				audioEchoFilter.delay = value;
			}
			if (this.filterProperty == EchoFilterProperty.DecayRatio)
			{
				audioEchoFilter.decayRatio = value;
			}
			if (this.filterProperty == EchoFilterProperty.WetMix)
			{
				audioEchoFilter.wetMix = value;
			}
			if (this.filterProperty == EchoFilterProperty.DryMix)
			{
				audioEchoFilter.dryMix = value;
			}
		}

		// Token: 0x04002A6B RID: 10859
		private EchoFilterProperty filterProperty;
	}
}
