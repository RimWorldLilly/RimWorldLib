using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C3 RID: 6083
	public class MusicManagerEntry
	{
		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x060097B4 RID: 38836 RVA: 0x00370653 File Offset: 0x0036E853
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x060097B5 RID: 38837 RVA: 0x00370665 File Offset: 0x0036E865
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x060097B6 RID: 38838 RVA: 0x00370678 File Offset: 0x0036E878
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			float curSanitizedVolume = this.CurSanitizedVolume;
			if (Time.frameCount > this.silentTillFrame)
			{
				this.silenceMultiplier = Mathf.Clamp01(this.silenceMultiplier + 1.75f * Time.deltaTime);
			}
			else if (Time.frameCount <= this.silentTillFrame)
			{
				this.silenceMultiplier = Mathf.Clamp01(this.silenceMultiplier - 1.75f * Time.deltaTime);
			}
			this.audioSource.volume = curSanitizedVolume * this.silenceMultiplier;
		}

		// Token: 0x060097B7 RID: 38839 RVA: 0x00370718 File Offset: 0x0036E918
		private void StartPlaying()
		{
			if (this.audioSource != null && !this.audioSource.isPlaying)
			{
				this.audioSource.Play();
				return;
			}
			if (GameObject.Find("MusicAudioSourceDummy") != null)
			{
				Log.Error("MusicManagerEntry did StartPlaying but there is already a music source GameObject.");
				return;
			}
			this.audioSource = new GameObject("MusicAudioSourceDummy")
			{
				transform = 
				{
					parent = Camera.main.transform
				}
			}.AddComponent<AudioSource>();
			this.audioSource.bypassEffects = true;
			this.audioSource.bypassListenerEffects = true;
			this.audioSource.bypassReverbZones = true;
			this.audioSource.priority = 0;
			this.audioSource.clip = SongDefOf.EntrySong.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.loop = true;
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
		}

		// Token: 0x060097B8 RID: 38840 RVA: 0x00370812 File Offset: 0x0036EA12
		public void MaintainSilence()
		{
			this.silentTillFrame = Time.frameCount + 1;
		}

		// Token: 0x04005ACA RID: 23242
		private AudioSource audioSource;

		// Token: 0x04005ACB RID: 23243
		private int silentTillFrame = -1;

		// Token: 0x04005ACC RID: 23244
		private float silenceMultiplier = 1f;

		// Token: 0x04005ACD RID: 23245
		private const string SourceGameObjectName = "MusicAudioSourceDummy";

		// Token: 0x04005ACE RID: 23246
		private const float SilenceMultiplierChangePerSecond = 1.75f;
	}
}
