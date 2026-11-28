using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007C7 RID: 1991
	public class SoundFilterEcho : SoundFilter
	{
		// Token: 0x06003FBD RID: 16317 RVA: 0x00197C9C File Offset: 0x00195E9C
		public override void SetupOn(AudioSource source)
		{
			AudioEchoFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioEchoFilter>(source);
			orMakeFilterOn.delay = this.delay;
			orMakeFilterOn.decayRatio = this.decayRatio;
			orMakeFilterOn.wetMix = this.wetMix;
			orMakeFilterOn.dryMix = this.dryMix;
		}

		// Token: 0x04002A41 RID: 10817
		[EditSliderRange(10f, 5000f)]
		[Description("Echo delay in ms. 10 to 5000. Default = 500.")]
		private float delay = 500f;

		// Token: 0x04002A42 RID: 10818
		[EditSliderRange(0f, 1f)]
		[Description("Echo decay per delay. 0 to 1. 1.0 = No decay, 0.0 = total decay (ie simple 1 line delay).")]
		private float decayRatio = 0.5f;

		// Token: 0x04002A43 RID: 10819
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the echo signal to pass to output.")]
		private float wetMix = 1f;

		// Token: 0x04002A44 RID: 10820
		[EditSliderRange(0f, 1f)]
		[Description("The volume of the original signal to pass to output.")]
		private float dryMix = 1f;
	}
}
