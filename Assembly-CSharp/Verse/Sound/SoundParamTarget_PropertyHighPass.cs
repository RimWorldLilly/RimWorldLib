using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007DF RID: 2015
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06003FF3 RID: 16371 RVA: 0x00198216 File Offset: 0x00196416
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty.ToString();
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06003FF4 RID: 16372 RVA: 0x00198233 File Offset: 0x00196433
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x00198240 File Offset: 0x00196440
		public override void SetOn(Sample sample, float value)
		{
			AudioHighPassFilter audioHighPassFilter = sample.source.GetComponent<AudioHighPassFilter>();
			if (audioHighPassFilter == null)
			{
				audioHighPassFilter = sample.source.gameObject.AddComponent<AudioHighPassFilter>();
			}
			if (this.filterProperty == HighPassFilterProperty.Cutoff)
			{
				audioHighPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == HighPassFilterProperty.Resonance)
			{
				audioHighPassFilter.highpassResonanceQ = value;
			}
		}

		// Token: 0x04002A65 RID: 10853
		private HighPassFilterProperty filterProperty;
	}
}
