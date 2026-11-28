using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007C8 RID: 1992
	public class SoundFilterReverb : SoundFilter
	{
		// Token: 0x06003FBF RID: 16319 RVA: 0x00197D08 File Offset: 0x00195F08
		public override void SetupOn(AudioSource source)
		{
			AudioReverbFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioReverbFilter>(source);
			this.baseSetup.ApplyTo(orMakeFilterOn);
		}

		// Token: 0x04002A45 RID: 10821
		[Description("The base setup for this filter.\n\nOnly used if no parameters ever affect this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();
	}
}
