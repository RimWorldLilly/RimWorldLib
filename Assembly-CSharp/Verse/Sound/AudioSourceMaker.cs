using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007F4 RID: 2036
	public static class AudioSourceMaker
	{
		// Token: 0x06004064 RID: 16484 RVA: 0x0019A364 File Offset: 0x00198564
		public static AudioSource NewAudioSourceOn(GameObject go)
		{
			if (go.GetComponent<AudioSource>() != null)
			{
				Log.Warning("Adding audio source on " + ((go != null) ? go.ToString() : null) + " that already has one.");
				return go.GetComponent<AudioSource>();
			}
			AudioSource audioSource = go.AddComponent<AudioSource>();
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.dopplerLevel = 0f;
			audioSource.playOnAwake = false;
			return audioSource;
		}

		// Token: 0x04002ABE RID: 10942
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;
	}
}
