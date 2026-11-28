using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007ED RID: 2029
	public static class SoundFilterUtility
	{
		// Token: 0x06004048 RID: 16456 RVA: 0x00199C76 File Offset: 0x00197E76
		public static void DisableAllFiltersOn(AudioSource source)
		{
			SoundFilterUtility.DisableFilterOn<AudioLowPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioHighPassFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioEchoFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioReverbFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioDistortionFilter>(source);
			SoundFilterUtility.DisableFilterOn<AudioChorusFilter>(source);
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x00199C9C File Offset: 0x00197E9C
		private static void DisableFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T component = source.GetComponent<T>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
	}
}
