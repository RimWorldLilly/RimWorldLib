using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007C6 RID: 1990
	public class SoundFilterHighPass : SoundFilter
	{
		// Token: 0x06003FBB RID: 16315 RVA: 0x00197C5F File Offset: 0x00195E5F
		public override void SetupOn(AudioSource source)
		{
			AudioHighPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioHighPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.highpassResonanceQ = this.highpassResonanceQ;
		}

		// Token: 0x04002A3F RID: 10815
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies below this cutoff frequency.")]
		public float cutoffFrequency = 10000f;

		// Token: 0x04002A40 RID: 10816
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		public float highpassResonanceQ = 1f;
	}
}
