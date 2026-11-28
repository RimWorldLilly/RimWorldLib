using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007DD RID: 2013
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06003FEF RID: 16367 RVA: 0x00198190 File Offset: 0x00196390
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty.ToString();
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06003FF0 RID: 16368 RVA: 0x001981AD File Offset: 0x001963AD
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x001981BC File Offset: 0x001963BC
		public override void SetOn(Sample sample, float value)
		{
			AudioLowPassFilter audioLowPassFilter = sample.source.GetComponent<AudioLowPassFilter>();
			if (audioLowPassFilter == null)
			{
				audioLowPassFilter = sample.source.gameObject.AddComponent<AudioLowPassFilter>();
			}
			if (this.filterProperty == LowPassFilterProperty.Cutoff)
			{
				audioLowPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == LowPassFilterProperty.Resonance)
			{
				audioLowPassFilter.lowpassResonanceQ = value;
			}
		}

		// Token: 0x04002A61 RID: 10849
		private LowPassFilterProperty filterProperty;
	}
}
