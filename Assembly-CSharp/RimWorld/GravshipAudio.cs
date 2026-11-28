using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001615 RID: 5653
	public class GravshipAudio
	{
		// Token: 0x06008CD8 RID: 36056 RVA: 0x00332D84 File Offset: 0x00330F84
		private SoundDef[] GetRetroThrusterSounds()
		{
			return new SoundDef[]
			{
				SoundDefOf.Gravship_RetroThruster_Oneshot_01,
				SoundDefOf.Gravship_RetroThruster_Oneshot_02,
				SoundDefOf.Gravship_RetroThruster_Oneshot_03
			};
		}

		// Token: 0x06008CD9 RID: 36057 RVA: 0x00332DA4 File Offset: 0x00330FA4
		public void BeginTakeoff()
		{
			this.active = true;
			if (!Prefs.GravshipCutscenes)
			{
				return;
			}
			SoundDefOf.Gravship_Engine_Start.PlayOneShotOnCamera(null);
			this.InitialiseSustainers();
			this.engineLoop.FadeIn(1f);
		}

		// Token: 0x06008CDA RID: 36058 RVA: 0x00332DD8 File Offset: 0x00330FD8
		public void BeginLanding()
		{
			this.active = true;
			if (!Prefs.GravshipCutscenes)
			{
				return;
			}
			this.InitialiseSustainers();
			this.landingServos = new GravshipAudio.TriggeredOneOff(SoundDefOf.Gravship_LandingServos);
			this.touchdown = new GravshipAudio.TriggeredOneOff(SoundDefOf.Gravship_Touchdown);
			this.engineStop = new GravshipAudio.TriggeredOneOff(SoundDefOf.Gravship_Engine_Stop);
			this.retroThrusterQueue.Clear();
			foreach (SoundDef def in this.GetRetroThrusterSounds().InRandomOrder(null))
			{
				this.retroThrusterQueue.Enqueue(new GravshipAudio.TriggeredOneOff(def));
			}
			this.engineLoop.FadeIn(0.5f);
			this.antigravLoop.FadeIn(0.5f);
			this.retroThrusterLoop.FadeIn(0.5f);
		}

		// Token: 0x06008CDB RID: 36059 RVA: 0x00332EB8 File Offset: 0x003310B8
		public void InitialiseSustainers()
		{
			this.engineLoop = new GravshipAudio.ManagedSustainer(SoundDefOf.Gravship_Engine_Loop);
			this.antigravLoop = new GravshipAudio.ManagedSustainer(SoundDefOf.Gravship_Antigrav_Loop);
			this.retroThrusterLoop = new GravshipAudio.ManagedSustainer(SoundDefOf.Gravship_RetroThruster_Loop);
		}

		// Token: 0x06008CDC RID: 36060 RVA: 0x00332EEC File Offset: 0x003310EC
		public void Update(float cutsceneTime, float progress, bool isTakeoff)
		{
			if (!this.active)
			{
				return;
			}
			GravshipAudio.ManagedSustainer managedSustainer = this.engineLoop;
			if (managedSustainer != null)
			{
				managedSustainer.Update();
			}
			GravshipAudio.ManagedSustainer managedSustainer2 = this.antigravLoop;
			if (managedSustainer2 != null)
			{
				managedSustainer2.Update();
			}
			GravshipAudio.ManagedSustainer managedSustainer3 = this.retroThrusterLoop;
			if (managedSustainer3 != null)
			{
				managedSustainer3.Update();
			}
			if (isTakeoff)
			{
				if (cutsceneTime > 1.5f)
				{
					GravshipAudio.ManagedSustainer managedSustainer4 = this.antigravLoop;
					if (managedSustainer4 != null)
					{
						managedSustainer4.FadeIn(0.1f);
					}
				}
			}
			else
			{
				if (progress > 0.15f)
				{
					GravshipAudio.ManagedSustainer managedSustainer5 = this.retroThrusterLoop;
					if (managedSustainer5 != null)
					{
						managedSustainer5.FadeIn(0f);
					}
					if (this.retroThrusterQueue.Count > 0)
					{
						GravshipAudio.TriggeredOneOff triggeredOneOff = this.retroThrusterQueue.Peek();
						if (triggeredOneOff.CurrentTimestamp > 0.5f)
						{
							this.retroThrusterQueue.Dequeue();
						}
						else
						{
							triggeredOneOff.Trigger();
						}
					}
				}
				if (progress > 0.5f)
				{
					GravshipAudio.TriggeredOneOff triggeredOneOff2 = this.landingServos;
					if (triggeredOneOff2 != null)
					{
						triggeredOneOff2.Trigger();
					}
				}
				if (progress > 0.8f)
				{
					GravshipAudio.TriggeredOneOff triggeredOneOff3 = this.touchdown;
					if (triggeredOneOff3 != null)
					{
						triggeredOneOff3.Trigger();
					}
					GravshipAudio.ManagedSustainer managedSustainer6 = this.retroThrusterLoop;
					if (managedSustainer6 != null)
					{
						managedSustainer6.FadeOut(0.5f);
					}
				}
				if (progress > 0.9f)
				{
					GravshipAudio.ManagedSustainer managedSustainer7 = this.engineLoop;
					if (managedSustainer7 != null)
					{
						managedSustainer7.FadeOut(1.2f);
					}
					GravshipAudio.ManagedSustainer managedSustainer8 = this.antigravLoop;
					if (managedSustainer8 != null)
					{
						managedSustainer8.FadeOut(1.2f);
					}
					GravshipAudio.TriggeredOneOff triggeredOneOff4 = this.engineStop;
					if (triggeredOneOff4 != null)
					{
						triggeredOneOff4.Trigger();
					}
				}
			}
			if (!Find.GravshipController.IsGravshipTravelling && !isTakeoff)
			{
				GravshipAudio.ManagedSustainer managedSustainer9 = this.engineLoop;
				if (managedSustainer9 == null || !managedSustainer9.Alive)
				{
					GravshipAudio.ManagedSustainer managedSustainer10 = this.antigravLoop;
					if (managedSustainer10 == null || !managedSustainer10.Alive)
					{
						GravshipAudio.TriggeredOneOff triggeredOneOff5 = this.engineStop;
						if (triggeredOneOff5 == null || triggeredOneOff5.Finished)
						{
							this.active = false;
						}
					}
				}
			}
		}

		// Token: 0x06008CDD RID: 36061 RVA: 0x0033309C File Offset: 0x0033129C
		public void EndTakeoff()
		{
			GravshipAudio.ManagedSustainer managedSustainer = this.engineLoop;
			if (managedSustainer != null)
			{
				managedSustainer.Stop();
			}
			GravshipAudio.ManagedSustainer managedSustainer2 = this.antigravLoop;
			if (managedSustainer2 != null)
			{
				managedSustainer2.Stop();
			}
			GravshipAudio.ManagedSustainer managedSustainer3 = this.retroThrusterLoop;
			if (managedSustainer3 != null)
			{
				managedSustainer3.Stop();
			}
			this.engineLoop = null;
			this.antigravLoop = null;
			this.retroThrusterLoop = null;
		}

		// Token: 0x04005647 RID: 22087
		private bool active;

		// Token: 0x04005648 RID: 22088
		private GravshipAudio.ManagedSustainer engineLoop;

		// Token: 0x04005649 RID: 22089
		private GravshipAudio.ManagedSustainer antigravLoop;

		// Token: 0x0400564A RID: 22090
		private GravshipAudio.ManagedSustainer retroThrusterLoop;

		// Token: 0x0400564B RID: 22091
		private GravshipAudio.TriggeredOneOff landingServos;

		// Token: 0x0400564C RID: 22092
		private GravshipAudio.TriggeredOneOff touchdown;

		// Token: 0x0400564D RID: 22093
		private GravshipAudio.TriggeredOneOff engineStop;

		// Token: 0x0400564E RID: 22094
		private Queue<GravshipAudio.TriggeredOneOff> retroThrusterQueue = new Queue<GravshipAudio.TriggeredOneOff>();

		// Token: 0x0400564F RID: 22095
		private const float ProgressThrustersDyingDown = 0.9f;

		// Token: 0x04005650 RID: 22096
		private const float ProgressDustCloudsVisible = 0.15f;

		// Token: 0x04005651 RID: 22097
		private const float ProgressTouchdown = 0.8f;

		// Token: 0x04005652 RID: 22098
		private const float ThrusterSFXInterval = 0.5f;

		// Token: 0x02003411 RID: 13329
		private class TriggeredOneOff
		{
			// Token: 0x170031E5 RID: 12773
			// (get) Token: 0x060132C6 RID: 78534 RVA: 0x00654A94 File Offset: 0x00652C94
			public bool Finished
			{
				get
				{
					return this.played && Time.time > this.playedTime + this.def.Duration.max;
				}
			}

			// Token: 0x170031E6 RID: 12774
			// (get) Token: 0x060132C7 RID: 78535 RVA: 0x00654ABE File Offset: 0x00652CBE
			public float CurrentTimestamp
			{
				get
				{
					if (this.played)
					{
						return Time.time - this.playedTime;
					}
					return 0f;
				}
			}

			// Token: 0x060132C8 RID: 78536 RVA: 0x00654ADA File Offset: 0x00652CDA
			public TriggeredOneOff(SoundDef def)
			{
				this.def = def;
			}

			// Token: 0x060132C9 RID: 78537 RVA: 0x00654AE9 File Offset: 0x00652CE9
			public void Trigger()
			{
				if (this.played)
				{
					return;
				}
				this.played = true;
				this.playedTime = Time.time;
				this.def.PlayOneShotOnCamera(null);
			}

			// Token: 0x0400D0D9 RID: 53465
			private SoundDef def;

			// Token: 0x0400D0DA RID: 53466
			private bool played;

			// Token: 0x0400D0DB RID: 53467
			private float playedTime;
		}

		// Token: 0x02003412 RID: 13330
		private class ManagedSustainer
		{
			// Token: 0x170031E7 RID: 12775
			// (get) Token: 0x060132CA RID: 78538 RVA: 0x00654B12 File Offset: 0x00652D12
			public Sustainer Sustainer
			{
				get
				{
					return this.sustainer;
				}
			}

			// Token: 0x170031E8 RID: 12776
			// (get) Token: 0x060132CB RID: 78539 RVA: 0x00654B1A File Offset: 0x00652D1A
			public bool Fading
			{
				get
				{
					return this.fading;
				}
			}

			// Token: 0x170031E9 RID: 12777
			// (get) Token: 0x060132CC RID: 78540 RVA: 0x00654B22 File Offset: 0x00652D22
			public bool Alive
			{
				get
				{
					return this.alive;
				}
			}

			// Token: 0x060132CD RID: 78541 RVA: 0x00654B2A File Offset: 0x00652D2A
			public ManagedSustainer(SoundDef def)
			{
				this.sustainer = def.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
				this.sustainer.info.volumeFactor = 0f;
			}

			// Token: 0x060132CE RID: 78542 RVA: 0x00654B60 File Offset: 0x00652D60
			public void Update()
			{
				if (this.fading || this.alive)
				{
					this.sustainer.Maintain();
				}
				if (this.fading)
				{
					this.fadeTime += Time.deltaTime;
					this.sustainer.info.volumeFactor = Mathf.Lerp(this.fadeInitialVolume, this.fadeTarget, this.fadeTime / this.fadeDuration);
					if (this.fadeTime >= this.fadeDuration)
					{
						this.fading = false;
					}
				}
			}

			// Token: 0x060132CF RID: 78543 RVA: 0x00654BE8 File Offset: 0x00652DE8
			private void Fade(float duration, float volume)
			{
				if (!this.alive)
				{
					return;
				}
				this.fading = true;
				this.fadeInitialVolume = this.sustainer.info.volumeFactor;
				this.fadeTarget = volume;
				this.fadeTime = 0f;
				this.fadeDuration = duration;
			}

			// Token: 0x060132D0 RID: 78544 RVA: 0x00654C34 File Offset: 0x00652E34
			public void FadeIn(float duration)
			{
				if (this.hasFadedIn)
				{
					return;
				}
				this.hasFadedIn = true;
				this.Fade(duration, 1f);
			}

			// Token: 0x060132D1 RID: 78545 RVA: 0x00654C52 File Offset: 0x00652E52
			public void FadeOut(float duration)
			{
				if (!this.hasFadedIn || this.hasFadedOut)
				{
					return;
				}
				this.hasFadedOut = true;
				this.Fade(duration, 0f);
				this.alive = false;
			}

			// Token: 0x060132D2 RID: 78546 RVA: 0x00654C7F File Offset: 0x00652E7F
			public void Stop()
			{
				if (!this.alive)
				{
					return;
				}
				this.alive = false;
				this.sustainer.End();
				this.sustainer = null;
			}

			// Token: 0x0400D0DC RID: 53468
			private Sustainer sustainer;

			// Token: 0x0400D0DD RID: 53469
			private bool alive = true;

			// Token: 0x0400D0DE RID: 53470
			private bool hasFadedIn;

			// Token: 0x0400D0DF RID: 53471
			private bool hasFadedOut;

			// Token: 0x0400D0E0 RID: 53472
			private bool fading;

			// Token: 0x0400D0E1 RID: 53473
			private float fadeTime;

			// Token: 0x0400D0E2 RID: 53474
			private float fadeDuration;

			// Token: 0x0400D0E3 RID: 53475
			private float fadeInitialVolume;

			// Token: 0x0400D0E4 RID: 53476
			private float fadeTarget;
		}
	}
}
