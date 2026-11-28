using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007E2 RID: 2018
	public class SoundParamTarget_PropertyReverb : SoundParamTarget
	{
		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06003FFB RID: 16379 RVA: 0x0019833E File Offset: 0x0019653E
		public override string Label
		{
			get
			{
				return "ReverbFilter-interpolant";
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06003FFC RID: 16380 RVA: 0x00198345 File Offset: 0x00196545
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterReverb);
			}
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x00198354 File Offset: 0x00196554
		public override void SetOn(Sample sample, float value)
		{
			AudioReverbFilter audioReverbFilter = sample.source.GetComponent<AudioReverbFilter>();
			if (audioReverbFilter == null)
			{
				audioReverbFilter = sample.source.gameObject.AddComponent<AudioReverbFilter>();
			}
			ReverbSetup reverbSetup;
			if (value < 0.001f)
			{
				reverbSetup = this.baseSetup;
			}
			if (value > 0.999f)
			{
				reverbSetup = this.targetSetup;
			}
			else
			{
				reverbSetup = ReverbSetup.Lerp(this.baseSetup, this.targetSetup, value);
			}
			reverbSetup.ApplyTo(audioReverbFilter);
		}

		// Token: 0x04002A6C RID: 10860
		[Description("The base setup for the reverb.\n\nOnly used if no parameters are touching this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();

		// Token: 0x04002A6D RID: 10861
		[Description("The interpolation target setup for this filter.\n\nWhen the interpolant parameter is at 1, these settings will be active.")]
		private ReverbSetup targetSetup = new ReverbSetup();
	}
}
