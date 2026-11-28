using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007C5 RID: 1989
	public class SoundFilterLowPass : SoundFilter
	{
		// Token: 0x06003FB9 RID: 16313 RVA: 0x00197C22 File Offset: 0x00195E22
		public override void SetupOn(AudioSource source)
		{
			AudioLowPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioLowPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.lowpassResonanceQ = this.lowpassResonaceQ;
		}

		// Token: 0x04002A3D RID: 10813
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies above this cutoff frequency.")]
		public float cutoffFrequency = 10000f;

		// Token: 0x04002A3E RID: 10814
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		public float lowpassResonaceQ = 1f;
	}
}
