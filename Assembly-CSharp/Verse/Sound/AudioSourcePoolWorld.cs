using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007F7 RID: 2039
	public class AudioSourcePoolWorld
	{
		// Token: 0x06004069 RID: 16489 RVA: 0x0019A508 File Offset: 0x00198708
		public AudioSourcePoolWorld()
		{
			GameObject gameObject = new GameObject("OneShotSourcesWorldContainer");
			gameObject.transform.position = Vector3.zero;
			for (int i = 0; i < 32; i++)
			{
				GameObject gameObject2 = new GameObject("OneShotSource_" + i.ToString());
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				this.sourcesWorld.Add(AudioSourceMaker.NewAudioSourceOn(gameObject2));
			}
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0019A598 File Offset: 0x00198798
		public AudioSource GetSourceWorld()
		{
			foreach (AudioSource audioSource in this.sourcesWorld)
			{
				if (!audioSource.isPlaying)
				{
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x04002AC4 RID: 10948
		private List<AudioSource> sourcesWorld = new List<AudioSource>();

		// Token: 0x04002AC5 RID: 10949
		private const int NumSourcesWorld = 32;
	}
}
