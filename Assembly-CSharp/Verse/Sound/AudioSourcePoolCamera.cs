using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007F6 RID: 2038
	public class AudioSourcePoolCamera
	{
		// Token: 0x06004067 RID: 16487 RVA: 0x0019A400 File Offset: 0x00198600
		public AudioSourcePoolCamera()
		{
			this.cameraSourcesContainer = new GameObject("OneShotSourcesCameraContainer");
			this.cameraSourcesContainer.transform.parent = Find.Camera.transform;
			this.cameraSourcesContainer.transform.localPosition = Vector3.zero;
			for (int i = 0; i < 16; i++)
			{
				AudioSource audioSource = AudioSourceMaker.NewAudioSourceOn(new GameObject("OneShotSourceCamera_" + i.ToString())
				{
					transform = 
					{
						parent = this.cameraSourcesContainer.transform,
						localPosition = Vector3.zero
					}
				});
				audioSource.bypassReverbZones = true;
				this.sourcesCamera.Add(audioSource);
			}
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0019A4C0 File Offset: 0x001986C0
		public AudioSource GetSourceCamera()
		{
			for (int i = 0; i < this.sourcesCamera.Count; i++)
			{
				AudioSource audioSource = this.sourcesCamera[i];
				if (!audioSource.isPlaying)
				{
					audioSource.clip = null;
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x04002AC1 RID: 10945
		public GameObject cameraSourcesContainer;

		// Token: 0x04002AC2 RID: 10946
		private List<AudioSource> sourcesCamera = new List<AudioSource>();

		// Token: 0x04002AC3 RID: 10947
		private const int NumSourcesCamera = 16;
	}
}
