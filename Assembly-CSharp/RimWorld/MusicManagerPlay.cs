using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020017C4 RID: 6084
	public class MusicManagerPlay
	{
		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x060097BA RID: 38842 RVA: 0x0037083B File Offset: 0x0036EA3B
		public MusicManagerPlay.MusicManagerState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x060097BB RID: 38843 RVA: 0x00370843 File Offset: 0x0036EA43
		// (set) Token: 0x060097BC RID: 38844 RVA: 0x0037084B File Offset: 0x0036EA4B
		public bool IsPlaying { get; private set; }

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x060097BD RID: 38845 RVA: 0x00370854 File Offset: 0x0036EA54
		public MusicTransition TriggeredTransition
		{
			get
			{
				return this.triggeredTransition;
			}
		}

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x060097BE RID: 38846 RVA: 0x0037085C File Offset: 0x0036EA5C
		public MusicSequenceWorker MusicSequenceWorker
		{
			get
			{
				return this.sequenceWorker;
			}
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x060097BF RID: 38847 RVA: 0x00370864 File Offset: 0x0036EA64
		public SongDef CurrentSong
		{
			get
			{
				return this.currentSong;
			}
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x060097C0 RID: 38848 RVA: 0x0037086C File Offset: 0x0036EA6C
		public float FadeoutPercent
		{
			get
			{
				return 1f - this.fadeoutFactor;
			}
		}

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x060097C1 RID: 38849 RVA: 0x0037087A File Offset: 0x0036EA7A
		public float FadeInPercent
		{
			get
			{
				return this.fadeinFactor;
			}
		}

		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x060097C2 RID: 38850 RVA: 0x00370884 File Offset: 0x0036EA84
		public float PausedVolumeFactor
		{
			get
			{
				MusicSequenceWorker musicSequenceWorker = this.sequenceWorker;
				if (musicSequenceWorker != null && musicSequenceWorker.def.pausedVolumeFactor != null)
				{
					return Mathf.Lerp(this.sequenceWorker.def.pausedVolumeFactor.Value, 1f, this.pausedVolumeFactor);
				}
				return 1f;
			}
		}

		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x060097C3 RID: 38851 RVA: 0x003708DA File Offset: 0x0036EADA
		public float FadeoutDuration
		{
			get
			{
				return this.fadeoutDuration;
			}
		}

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x060097C4 RID: 38852 RVA: 0x003708E2 File Offset: 0x0036EAE2
		public float NextSongTimer
		{
			get
			{
				return this.nextSongStartTime - Time.time;
			}
		}

		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x060097C5 RID: 38853 RVA: 0x003708F0 File Offset: 0x0036EAF0
		private float CurTime
		{
			get
			{
				return Time.time;
			}
		}

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x060097C6 RID: 38854 RVA: 0x003708F7 File Offset: 0x0036EAF7
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerPlay");
			}
		}

		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x060097C7 RID: 38855 RVA: 0x00370909 File Offset: 0x0036EB09
		// (set) Token: 0x060097C8 RID: 38856 RVA: 0x00370911 File Offset: 0x0036EB11
		public bool OverrideDangerMode { get; set; }

		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x060097C9 RID: 38857 RVA: 0x0037091A File Offset: 0x0036EB1A
		public float SongTime
		{
			get
			{
				return this.audioSource.time;
			}
		}

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x060097CA RID: 38858 RVA: 0x00370927 File Offset: 0x0036EB27
		public float SongDuration
		{
			get
			{
				if (!(this.audioSource.clip != null))
				{
					return 0f;
				}
				return this.audioSource.clip.length;
			}
		}

		// Token: 0x170019D0 RID: 6608
		// (get) Token: 0x060097CB RID: 38859 RVA: 0x00370954 File Offset: 0x0036EB54
		public bool DangerMusicMode
		{
			get
			{
				if (this.OverrideDangerMode)
				{
					return true;
				}
				if (Find.Scenario.OverrideDangerMusic)
				{
					return true;
				}
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].dangerWatcher.DangerRating == StoryDanger.High)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x060097CC RID: 38860 RVA: 0x003709A8 File Offset: 0x0036EBA8
		private float CurVolume
		{
			get
			{
				float num;
				if (!this.ignorePrefsVolumeThisSong)
				{
					MusicSequenceWorker musicSequenceWorker = this.sequenceWorker;
					num = ((((musicSequenceWorker != null) ? musicSequenceWorker.def.overrideVolume : null) * Prefs.VolumeMaster) ?? Prefs.VolumeMusic);
				}
				else
				{
					num = Prefs.VolumeMaster;
				}
				float num2 = num;
				if (this.currentSong == null)
				{
					return num2;
				}
				return this.currentSong.volume * num2 * this.fadeoutFactor * this.fadeinFactor * this.musicOverridenFadeFactor * this.PausedVolumeFactor;
			}
		}

		// Token: 0x060097CD RID: 38861 RVA: 0x00370A5A File Offset: 0x0036EC5A
		public void ForceFadeoutAndSilenceFor(float time, float fadeoutTime = 10f, bool preventDangerTransition = false)
		{
			this.StopAndFadeoutSequence(fadeoutTime);
			this.nextSongStartTime = this.CurTime + time;
			this.preventDangerTransitionCheck = preventDangerTransition;
		}

		// Token: 0x060097CE RID: 38862 RVA: 0x00370A78 File Offset: 0x0036EC78
		public void ForceSilenceFor(float time)
		{
			this.Stop();
			this.nextSongStartTime = this.CurTime + time;
		}

		// Token: 0x060097CF RID: 38863 RVA: 0x00370A90 File Offset: 0x0036EC90
		public void MusicUpdate()
		{
			if (!this.gameObjectCreated)
			{
				this.InitializeMusicManager();
			}
			this.UpdateSubtleAmbienceSoundVolumeMultiplier();
			if (this.disabled)
			{
				return;
			}
			TickManager tickManager = Find.TickManager;
			if (tickManager.Paused != this.isPaused)
			{
				this.isPaused = tickManager.Paused;
				this.pauseToggledTime = Time.time;
				this.pausedVolumeFactorAtToggle = this.pausedVolumeFactor;
			}
			float t = Mathf.Clamp01((Time.time - this.pauseToggledTime) / 0.5f);
			this.pausedVolumeFactor = Mathf.Clamp01(Mathf.Lerp(this.pausedVolumeFactorAtToggle, !this.isPaused, t));
			if (this.songWasForced)
			{
				this.state = MusicManagerPlay.MusicManagerState.Normal;
				this.fadeoutFactor = 1f;
			}
			if (this.CurTime >= this.songEndTime)
			{
				if (this.loopWhileFading)
				{
					this.PlaySong(this.CurrentSong, false, false);
				}
				else
				{
					this.IsPlaying = false;
				}
			}
			if (this.IsPlaying && this.state == MusicManagerPlay.MusicManagerState.Normal && !this.songWasForced && ((this.DangerMusicMode && !this.currentSong.tense) || (!this.DangerMusicMode && this.currentSong.tense)))
			{
				this.StartFadeout(10f);
			}
			this.audioSource.volume = this.CurSanitizedVolume;
			if (this.state == MusicManagerPlay.MusicManagerState.Sequence)
			{
				this.UpdateSequenceState();
			}
			if (this.state != MusicManagerPlay.MusicManagerState.Fadeout)
			{
				this.CheckTransitionInterval();
			}
			if (this.state == MusicManagerPlay.MusicManagerState.Fadeout)
			{
				this.UpdateFadeout();
			}
			if (this.IsPlaying)
			{
				this.UpdateMusicFadeout();
				return;
			}
			if (!this.preventDangerTransitionCheck && this.DangerMusicMode && this.nextSongStartTime > this.CurTime + MusicManagerPlay.SongIntervalTension.max)
			{
				this.nextSongStartTime = this.CurTime + MusicManagerPlay.SongIntervalTension.RandomInRange;
			}
			if (this.nextSongStartTime < this.CurTime - 5f)
			{
				float num = this.DangerMusicMode ? MusicManagerPlay.SongIntervalTension.RandomInRange : MusicManagerPlay.SongIntervalRelax.RandomInRange;
				this.nextSongStartTime = this.CurTime + num;
			}
			if (this.CurTime >= this.nextSongStartTime)
			{
				this.StartNewSong();
			}
		}

		// Token: 0x060097D0 RID: 38864 RVA: 0x00370CA9 File Offset: 0x0036EEA9
		private void CheckTransitionInterval()
		{
			if (!GenTicks.IsTickInterval(60) && this.initalized)
			{
				return;
			}
			this.initalized = true;
			this.CheckTransitions();
		}

		// Token: 0x060097D1 RID: 38865 RVA: 0x00370CCC File Offset: 0x0036EECC
		public void CheckTransitions()
		{
			bool flag = this.sequenceWorker != null && this.sequenceWorker.def.useTransitionForLifetime && !this.manuallyTriggered;
			bool flag2 = this.sequenceWorker == null || this.sequenceWorker.CanBeInterrupted();
			MusicTransition musicTransition = null;
			foreach (MusicTransition musicTransition2 in this.transitions)
			{
				if (musicTransition2 == this.triggeredTransition)
				{
					if (musicTransition == null || musicTransition2.def.priority >= musicTransition.def.priority)
					{
						musicTransition = musicTransition2;
					}
				}
				else if ((flag2 || musicTransition2.def.overridesInterruptions) && (this.triggeredTransition == null || this.triggeredTransition.def.priority <= musicTransition2.def.priority) && musicTransition2.IsTransitionSatisfied() && (musicTransition == null || musicTransition2.def.priority > musicTransition.def.priority))
				{
					musicTransition = musicTransition2;
				}
			}
			if (musicTransition != null && musicTransition != this.triggeredTransition)
			{
				this.triggeredTransition = musicTransition;
				this.StartSequence(musicTransition.def.sequence);
				return;
			}
			if (this.triggeredTransition != null && flag && this.sequenceWorker.MinTimeSatisfied() && !this.triggeredTransition.IsTransitionSatisfied())
			{
				this.StopAndFadeoutSequence(10f);
			}
		}

		// Token: 0x060097D2 RID: 38866 RVA: 0x00370E40 File Offset: 0x0036F040
		private void UpdateSequenceState()
		{
			if (this.sequenceWorker.def.nextSequence != null && this.sequenceWorker.ShouldTransition())
			{
				this.StartSequence(this.sequenceWorker.def.nextSequence);
				return;
			}
			if (this.sequenceWorker.ShouldEnd())
			{
				this.StopAndFadeoutSequence(10f);
				return;
			}
			if (!this.IsPlaying)
			{
				if (this.sequenceWorker.ShouldLoop())
				{
					if (!(this.sequenceWorker.def.loopDelayRange != FloatRange.Zero))
					{
						MusicSequenceWorker musicSequenceWorker = this.sequenceWorker;
						int timesLooped = musicSequenceWorker.timesLooped;
						musicSequenceWorker.timesLooped = timesLooped + 1;
						this.PlayNextSongInSequence();
						return;
					}
					if (this.nextSongStartTime < this.CurTime - 5f)
					{
						this.nextSongStartTime = this.CurTime + this.sequenceWorker.def.loopDelayRange.RandomInRange;
						return;
					}
					if (this.CurTime >= this.nextSongStartTime)
					{
						MusicSequenceWorker musicSequenceWorker2 = this.sequenceWorker;
						int timesLooped = musicSequenceWorker2.timesLooped;
						musicSequenceWorker2.timesLooped = timesLooped + 1;
						this.PlayNextSongInSequence();
						return;
					}
				}
				else
				{
					this.Stop();
				}
			}
		}

		// Token: 0x060097D3 RID: 38867 RVA: 0x00370F58 File Offset: 0x0036F158
		private void StopAndFadeoutSequence(float duration = 10f)
		{
			if (this.sequenceWorker != null)
			{
				if (this.sequenceWorker.def.overrideFadeout != null)
				{
					duration = this.sequenceWorker.def.overrideFadeout.Value;
				}
				if (this.sequenceWorker.def.loopFadeout)
				{
					this.loopWhileFading = true;
				}
			}
			this.sequenceWorker = null;
			this.triggeredTransition = null;
			this.manuallyTriggered = false;
			this.StartFadeout(duration);
		}

		// Token: 0x060097D4 RID: 38868 RVA: 0x00370FD0 File Offset: 0x0036F1D0
		public void Stop()
		{
			if (this.audioSource != null)
			{
				this.audioSource.Stop();
			}
			this.sequenceWorker = null;
			this.triggeredTransition = null;
			this.IsPlaying = false;
			this.loopWhileFading = false;
			this.manuallyTriggered = false;
			this.state = MusicManagerPlay.MusicManagerState.Normal;
			float num = this.DangerMusicMode ? MusicManagerPlay.SongIntervalTension.RandomInRange : MusicManagerPlay.SongIntervalRelax.RandomInRange;
			this.nextSongStartTime = this.CurTime + num;
		}

		// Token: 0x060097D5 RID: 38869 RVA: 0x00371054 File Offset: 0x0036F254
		private void StartSequence(MusicSequenceDef def)
		{
			float curVolume = this.CurVolume;
			this.sequenceWorker = (MusicSequenceWorker)Activator.CreateInstance(def.workerType);
			this.sequenceWorker.InitializeWorker(def, this);
			if (this.sequenceWorker.ShouldEnd())
			{
				this.Stop();
				return;
			}
			if (this.IsPlaying && this.sequenceWorker.def.fadeoutLastSongDuration > 0f && curVolume > 0f)
			{
				this.StartFadeout(this.sequenceWorker.def.fadeoutLastSongDuration);
				return;
			}
			this.state = MusicManagerPlay.MusicManagerState.Sequence;
			this.PlayNextSongInSequence();
		}

		// Token: 0x060097D6 RID: 38870 RVA: 0x003710EC File Offset: 0x0036F2EC
		private void UpdateMusicFadeout()
		{
			if (this.fadeInStarted != 0f)
			{
				this.fadeinFactor = Mathf.Clamp01(Mathf.InverseLerp(this.fadeInStarted, this.fadeInStarted + this.fadeInDuration, this.CurTime));
			}
			else
			{
				this.fadeinFactor = 1f;
			}
			Map currentMap = Find.CurrentMap;
			if (currentMap != null && !WorldRendererUtility.WorldSelected && !Find.TickManager.Paused)
			{
				float num = 1f;
				Camera camera = Find.Camera;
				List<Thing> list = currentMap.listerThings.ThingsInGroup(ThingRequestGroup.MusicalInstrument);
				for (int i = 0; i < list.Count; i++)
				{
					Building_MusicalInstrument building_MusicalInstrument = (Building_MusicalInstrument)list[i];
					if (building_MusicalInstrument.IsBeingPlayed)
					{
						float fadeFactor = this.GetFadeFactor(building_MusicalInstrument.Position.ToVector3Shifted(), building_MusicalInstrument.SoundRange, camera);
						if (fadeFactor < num)
						{
							num = fadeFactor;
						}
					}
				}
				List<Thing> list2 = currentMap.listerThings.ThingsInGroup(ThingRequestGroup.MusicSource);
				for (int j = 0; j < list2.Count; j++)
				{
					Thing thing = list2[j];
					CompPlaysMusic compPlaysMusic = thing.TryGetComp<CompPlaysMusic>();
					if (compPlaysMusic.Playing)
					{
						float fadeFactor2 = this.GetFadeFactor(thing.Position.ToVector3Shifted(), compPlaysMusic.SoundRange, camera);
						if (fadeFactor2 < num)
						{
							num = fadeFactor2;
						}
					}
				}
				foreach (Lord lord in currentMap.lordManager.lords)
				{
					LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
					if (lordJob_Ritual != null && lordJob_Ritual.AmbiencePlaying != null && !lordJob_Ritual.AmbiencePlaying.def.subSounds.NullOrEmpty<SubSoundDef>())
					{
						float fadeFactor3 = this.GetFadeFactor(lordJob_Ritual.selectedTarget.CenterVector3, lordJob_Ritual.AmbiencePlaying.def.subSounds.First<SubSoundDef>().distRange, camera);
						if (fadeFactor3 < num)
						{
							num = fadeFactor3;
						}
					}
				}
				this.musicOverridenFadeFactor = num;
				return;
			}
			this.musicOverridenFadeFactor = 1f;
		}

		// Token: 0x060097D7 RID: 38871 RVA: 0x003712FC File Offset: 0x0036F4FC
		private void UpdateFadeout()
		{
			this.fadeoutFactor -= Time.deltaTime / this.fadeoutDuration;
			if (this.fadeoutFactor <= 0f)
			{
				this.IsPlaying = false;
				this.loopWhileFading = false;
				this.audioSource.Stop();
				this.fadeoutFactor = 1f;
				if (this.sequenceWorker != null)
				{
					this.PlayNextSongInSequence();
					this.state = MusicManagerPlay.MusicManagerState.Sequence;
					return;
				}
				this.state = MusicManagerPlay.MusicManagerState.Normal;
			}
		}

		// Token: 0x060097D8 RID: 38872 RVA: 0x00371370 File Offset: 0x0036F570
		private void StartFadeout(float duration)
		{
			this.state = MusicManagerPlay.MusicManagerState.Fadeout;
			this.fadeoutDuration = duration;
		}

		// Token: 0x060097D9 RID: 38873 RVA: 0x00371380 File Offset: 0x0036F580
		private float GetFadeFactor(Vector3 pos, FloatRange soundRange, Camera camera)
		{
			Vector3 vector = camera.transform.position - pos;
			vector.y = Mathf.Max(vector.y - 15f, 0f);
			vector.y *= 3.5f;
			return Mathf.Min(Mathf.Max(vector.magnitude - soundRange.min, 0f) / (soundRange.max - soundRange.min), 1f);
		}

		// Token: 0x060097DA RID: 38874 RVA: 0x003713FC File Offset: 0x0036F5FC
		private void InitializeMusicManager()
		{
			this.gameObjectCreated = true;
			this.audioSource = new GameObject("MusicAudioSourceDummy")
			{
				transform = 
				{
					parent = Find.Root.soundRoot.sourcePool.sourcePoolCamera.cameraSourcesContainer.transform
				}
			}.AddComponent<AudioSource>();
			this.audioSource.bypassEffects = true;
			this.audioSource.bypassListenerEffects = true;
			this.audioSource.bypassReverbZones = true;
			this.audioSource.priority = 0;
			this.CacheTransitions();
			if (!this.disabled && !this.IsPlaying)
			{
				this.CheckTransitionInterval();
				if (!this.IsPlaying)
				{
					this.StartNewSong();
				}
			}
		}

		// Token: 0x060097DB RID: 38875 RVA: 0x003714AC File Offset: 0x0036F6AC
		private void CacheTransitions()
		{
			this.transitions.Clear();
			List<MusicTransitionDef> list = (from x in DefDatabase<MusicTransitionDef>.AllDefsListForReading
			orderby x.priority descending
			select x).ToList<MusicTransitionDef>();
			for (int i = 0; i < list.Count; i++)
			{
				MusicTransitionDef musicTransitionDef = list[i];
				MusicTransition musicTransition = (MusicTransition)Activator.CreateInstance(musicTransitionDef.workerType);
				musicTransition.InitializeWorker(musicTransitionDef, this);
				this.transitions.Add(musicTransition);
			}
		}

		// Token: 0x060097DC RID: 38876 RVA: 0x00371534 File Offset: 0x0036F734
		private void UpdateSubtleAmbienceSoundVolumeMultiplier()
		{
			if (this.IsPlaying && this.CurSanitizedVolume > 0.001f)
			{
				this.subtleAmbienceSoundVolumeMultiplier -= Time.deltaTime * 0.1f;
			}
			else
			{
				this.subtleAmbienceSoundVolumeMultiplier += Time.deltaTime * 0.1f;
			}
			this.subtleAmbienceSoundVolumeMultiplier = Mathf.Clamp01(this.subtleAmbienceSoundVolumeMultiplier);
		}

		// Token: 0x060097DD RID: 38877 RVA: 0x00371599 File Offset: 0x0036F799
		public void StartNewSong()
		{
			this.PlaySong(this.ChooseNextSong(), false, false);
		}

		// Token: 0x060097DE RID: 38878 RVA: 0x003715A9 File Offset: 0x0036F7A9
		public void ScheduleNewSong()
		{
			this.nextSongStartTime = this.CurTime + 1f;
		}

		// Token: 0x060097DF RID: 38879 RVA: 0x003715C0 File Offset: 0x0036F7C0
		private void PlayNextSongInSequence()
		{
			if (this.sequenceWorker == null)
			{
				Log.Error("Attempted to select a song with a null sequence");
				return;
			}
			if (this.sequenceWorker.def.fadeInDuration > 0f && this.sequenceWorker.timesLooped == 0)
			{
				this.fadeInDuration = this.sequenceWorker.def.fadeInDuration;
				this.fadeInStarted = this.CurTime;
				this.fadeinFactor = 0f;
			}
			this.PlaySong(this.sequenceWorker.SelectSong(), false, false);
		}

		// Token: 0x060097E0 RID: 38880 RVA: 0x00371644 File Offset: 0x0036F844
		private void PlaySong(SongDef song, bool forced = false, bool ignorePrefsVolume = false)
		{
			this.songWasForced = forced;
			this.currentSong = song;
			this.audioSource.clip = song.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.spatialBlend = 0f;
			this.ignorePrefsVolumeThisSong = ignorePrefsVolume;
			this.songEndTime = this.CurTime + song.clip.length;
			this.audioSource.Play();
			this.IsPlaying = true;
			this.preventDangerTransitionCheck = false;
			this.recentSongs.Enqueue(song);
		}

		// Token: 0x060097E1 RID: 38881 RVA: 0x003716D8 File Offset: 0x0036F8D8
		public void ForceTriggerNextSongOrSequence()
		{
			if (this.sequenceWorker == null)
			{
				return;
			}
			if (this.sequenceWorker.def.nextSequence != null)
			{
				this.StartSequence(this.sequenceWorker.def.nextSequence);
				return;
			}
			if (this.sequenceWorker.ShouldLoop())
			{
				MusicSequenceWorker musicSequenceWorker = this.sequenceWorker;
				int timesLooped = musicSequenceWorker.timesLooped;
				musicSequenceWorker.timesLooped = timesLooped + 1;
				this.StartFadeout(10f);
			}
		}

		// Token: 0x060097E2 RID: 38882 RVA: 0x00371744 File Offset: 0x0036F944
		public void ForceTriggerTransition(MusicTransitionDef transition)
		{
			foreach (MusicTransition musicTransition in this.transitions)
			{
				if (musicTransition.def == transition)
				{
					this.manuallyTriggered = true;
					this.triggeredTransition = musicTransition;
					this.StartSequence(musicTransition.def.sequence);
					break;
				}
			}
		}

		// Token: 0x060097E3 RID: 38883 RVA: 0x003717BC File Offset: 0x0036F9BC
		public void ForcePlaySong(SongDef song, bool ignorePrefsVolume)
		{
			this.PlaySong(song, true, ignorePrefsVolume);
		}

		// Token: 0x060097E4 RID: 38884 RVA: 0x003717C8 File Offset: 0x0036F9C8
		private SongDef ChooseNextSong()
		{
			while (this.recentSongs.Count > 7)
			{
				this.recentSongs.Dequeue();
			}
			List<SongDef> list = DefDatabase<SongDef>.AllDefs.Where(new Func<SongDef, bool>(this.AppropriateNow)).ToList<SongDef>();
			if (list.Empty<SongDef>())
			{
				this.recentSongs.Clear();
				list = DefDatabase<SongDef>.AllDefs.Where(new Func<SongDef, bool>(this.AppropriateNow)).ToList<SongDef>();
			}
			if (list.Empty<SongDef>())
			{
				Log.Error("Could not get any appropriate song. Getting random and logging song selection data.");
				this.SongSelectionData();
				return DefDatabase<SongDef>.GetRandom();
			}
			return list.RandomElementByWeight((SongDef s) => s.commonality);
		}

		// Token: 0x060097E5 RID: 38885 RVA: 0x00371880 File Offset: 0x0036FA80
		private bool AppropriateNow(SongDef song)
		{
			if (!song.playOnMap)
			{
				return false;
			}
			if (this.DangerMusicMode)
			{
				if (!song.tense)
				{
					return false;
				}
			}
			else if (song.tense)
			{
				return false;
			}
			Map map = Find.AnyPlayerHomeMap ?? Find.CurrentMap;
			if (!song.allowedSeasons.NullOrEmpty<Season>())
			{
				if (map == null)
				{
					return false;
				}
				if (!song.allowedSeasons.Contains(GenLocalDate.Season(map)))
				{
					return false;
				}
			}
			if (song.minRoyalTitle != null && !PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists.Any((Pawn p) => p.royalty != null && p.royalty.AllTitlesForReading.Any<RoyalTitle>() && p.royalty.MostSeniorTitle.def.seniority >= song.minRoyalTitle.seniority && !p.IsQuestLodger()))
			{
				return false;
			}
			if (this.recentSongs.Contains(song))
			{
				return false;
			}
			if (song.allowedTimeOfDay == TimeOfDay.Any)
			{
				return true;
			}
			if (map == null)
			{
				return true;
			}
			if (song.allowedTimeOfDay == TimeOfDay.Night)
			{
				return GenLocalDate.DayPercent(map) < 0.2f || GenLocalDate.DayPercent(map) > 0.7f;
			}
			return GenLocalDate.DayPercent(map) > 0.2f && GenLocalDate.DayPercent(map) < 0.7f;
		}

		// Token: 0x060097E6 RID: 38886 RVA: 0x003719A6 File Offset: 0x0036FBA6
		public void OnApplicationFocus()
		{
		}

		// Token: 0x060097E7 RID: 38887 RVA: 0x003719A8 File Offset: 0x0036FBA8
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MusicManagerMap");
			stringBuilder.AppendLine("state: " + this.state.ToString());
			string str = "lastStartedSong: ";
			SongDef songDef = this.currentSong;
			stringBuilder.AppendLine(str + ((songDef != null) ? songDef.ToString() : null));
			stringBuilder.AppendLine("fadeoutFactor: " + this.fadeoutFactor.ToString());
			stringBuilder.AppendLine("nextSongStartTime: " + this.nextSongStartTime.ToString());
			stringBuilder.AppendLine("CurTime: " + this.CurTime.ToString());
			stringBuilder.AppendLine("recentSongs: " + (from s in this.recentSongs
			select s.defName).ToCommaList(true, false));
			stringBuilder.AppendLine("disabled: " + this.disabled.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x060097E8 RID: 38888 RVA: 0x00371AC8 File Offset: 0x0036FCC8
		[DebugOutput]
		public void SongSelectionData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Most recent song: " + ((this.currentSong != null) ? this.currentSong.defName : "None"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Songs appropriate to play now:");
			foreach (SongDef songDef in DefDatabase<SongDef>.AllDefs.Where(new Func<SongDef, bool>(this.AppropriateNow)))
			{
				stringBuilder.AppendLine("   " + songDef.defName);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Recently played songs:");
			foreach (SongDef songDef2 in this.recentSongs)
			{
				stringBuilder.AppendLine("   " + songDef2.defName);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04005ACF RID: 23247
		private AudioSource audioSource;

		// Token: 0x04005AD0 RID: 23248
		private MusicManagerPlay.MusicManagerState state;

		// Token: 0x04005AD1 RID: 23249
		private float fadeoutFactor = 1f;

		// Token: 0x04005AD2 RID: 23250
		private float fadeinFactor = 1f;

		// Token: 0x04005AD3 RID: 23251
		private float pausedVolumeFactor = 1f;

		// Token: 0x04005AD4 RID: 23252
		private float musicOverridenFadeFactor = 1f;

		// Token: 0x04005AD5 RID: 23253
		private float nextSongStartTime = 12f;

		// Token: 0x04005AD6 RID: 23254
		private float pauseToggledTime = -9999f;

		// Token: 0x04005AD7 RID: 23255
		private float pausedVolumeFactorAtToggle;

		// Token: 0x04005AD8 RID: 23256
		private bool isPaused;

		// Token: 0x04005AD9 RID: 23257
		public bool disabled;

		// Token: 0x04005ADA RID: 23258
		private bool songWasForced;

		// Token: 0x04005ADB RID: 23259
		private bool gameObjectCreated;

		// Token: 0x04005ADC RID: 23260
		private bool ignorePrefsVolumeThisSong;

		// Token: 0x04005ADD RID: 23261
		private bool manuallyTriggered;

		// Token: 0x04005ADE RID: 23262
		private bool preventDangerTransitionCheck;

		// Token: 0x04005ADF RID: 23263
		private bool initalized;

		// Token: 0x04005AE0 RID: 23264
		public float subtleAmbienceSoundVolumeMultiplier = 1f;

		// Token: 0x04005AE1 RID: 23265
		private MusicSequenceWorker sequenceWorker;

		// Token: 0x04005AE2 RID: 23266
		private MusicTransition triggeredTransition;

		// Token: 0x04005AE3 RID: 23267
		private float fadeoutDuration;

		// Token: 0x04005AE4 RID: 23268
		private float fadeInDuration;

		// Token: 0x04005AE5 RID: 23269
		private float fadeInStarted;

		// Token: 0x04005AE6 RID: 23270
		private bool loopWhileFading;

		// Token: 0x04005AE7 RID: 23271
		private SongDef currentSong;

		// Token: 0x04005AE8 RID: 23272
		private float songEndTime;

		// Token: 0x04005AE9 RID: 23273
		private readonly Queue<SongDef> recentSongs = new Queue<SongDef>();

		// Token: 0x04005AEA RID: 23274
		private readonly List<MusicTransition> transitions = new List<MusicTransition>();

		// Token: 0x04005AEB RID: 23275
		private static readonly FloatRange SongIntervalRelax = new FloatRange(85f, 105f);

		// Token: 0x04005AEC RID: 23276
		private static readonly FloatRange SongIntervalTension = new FloatRange(2f, 5f);

		// Token: 0x04005AED RID: 23277
		private const float DefaultFadeout = 10f;

		// Token: 0x04005AEE RID: 23278
		private const float PauseVolumeFadeTime = 0.5f;

		// Token: 0x04005AEF RID: 23279
		private const int TransitionUpdatePeriodTicks = 60;

		// Token: 0x020035C0 RID: 13760
		public enum MusicManagerState
		{
			// Token: 0x0400D6A5 RID: 54949
			Normal,
			// Token: 0x0400D6A6 RID: 54950
			Sequence,
			// Token: 0x0400D6A7 RID: 54951
			Fadeout
		}
	}
}
