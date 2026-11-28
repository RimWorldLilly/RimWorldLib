using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007E9 RID: 2025
	public abstract class Sample
	{
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06004015 RID: 16405 RVA: 0x00198B7A File Offset: 0x00196D7A
		public float AgeRealTime
		{
			get
			{
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06004016 RID: 16406 RVA: 0x00198B88 File Offset: 0x00196D88
		public int AgeTicks
		{
			get
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					return Find.TickManager.TicksGame - this.startTick;
				}
				return (int)(this.AgeRealTime * 60f);
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06004017 RID: 16407
		public abstract float ParentStartRealTime { get; }

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06004018 RID: 16408
		public abstract float ParentStartTick { get; }

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06004019 RID: 16409
		public abstract float ParentHashCode { get; }

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x0600401A RID: 16410
		public abstract SoundParams ExternalParams { get; }

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x0600401B RID: 16411
		public abstract SoundInfo Info { get; }

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x0600401C RID: 16412 RVA: 0x00198BB4 File Offset: 0x00196DB4
		public Map Map
		{
			get
			{
				return this.Info.Maker.Map;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x0600401D RID: 16413 RVA: 0x00198BD7 File Offset: 0x00196DD7
		protected bool TestPlaying
		{
			get
			{
				return this.Info.testPlay;
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x0600401E RID: 16414 RVA: 0x00198BE4 File Offset: 0x00196DE4
		protected float MappedVolumeMultiplier
		{
			get
			{
				float num = 1f;
				foreach (float num2 in this.volumeInMappings.Values)
				{
					num *= num2;
				}
				return num;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x0600401F RID: 16415 RVA: 0x00198C40 File Offset: 0x00196E40
		protected float ContextVolumeMultiplier
		{
			get
			{
				if (Screen_Credits.creditsShowing)
				{
					return 1f - Mathf.Clamp01((Time.realtimeSinceStartup - Screen_Credits.creationRealtime) / 3f);
				}
				if (!this.TestPlaying && !SoundDefHelper.CorrectContextNow(this.subDef.parentDef, this.Map))
				{
					return 0f;
				}
				if (this.subDef.parentDef.context != SoundContext.MapOnly && this.subDef.parentDef.context != SoundContext.WorldOnly)
				{
					return Prefs.VolumeUI;
				}
				if ((this.subDef.parentDef.context == SoundContext.MapOnly || this.subDef.parentDef.context == SoundContext.WorldOnly) && !this.IsAmbient)
				{
					return Prefs.VolumeGame;
				}
				return 1f;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x00198CFC File Offset: 0x00196EFC
		protected virtual float Volume
		{
			get
			{
				if (this.subDef.muteWhenPaused && Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused && !this.TestPlaying)
				{
					return 0f;
				}
				return this.resolvedVolume * this.Info.volumeFactor * this.MappedVolumeMultiplier * this.ContextVolumeMultiplier;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x00198D58 File Offset: 0x00196F58
		public float SanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.Volume, this.subDef.parentDef);
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06004022 RID: 16418 RVA: 0x00198D70 File Offset: 0x00196F70
		public bool IsAmbient
		{
			get
			{
				foreach (SubSoundDef subSoundDef in this.subDef.parentDef.subSounds)
				{
					using (List<SoundParameterMapping>.Enumerator enumerator2 = subSoundDef.paramMappings.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.inParam is SoundParamSource_AmbientVolume)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06004023 RID: 16419 RVA: 0x00198E14 File Offset: 0x00197014
		protected virtual float Pitch
		{
			get
			{
				float num = this.resolvedPitch * this.Info.pitchFactor;
				if (this.subDef.tempoAffectedByGameSpeed && Current.ProgramState == ProgramState.Playing && !this.TestPlaying && !Find.TickManager.Paused)
				{
					num *= Find.TickManager.TickRateMultiplier;
				}
				return num;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06004024 RID: 16420 RVA: 0x00198E6B File Offset: 0x0019706B
		public float SanitizedPitch
		{
			get
			{
				return AudioSourceUtility.GetSanitizedPitch(this.Pitch, this.subDef.parentDef);
			}
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x00198E84 File Offset: 0x00197084
		public Sample(SubSoundDef def)
		{
			this.subDef = def;
			this.resolvedVolume = def.RandomizedVolume();
			this.resolvedPitch = def.pitchRange.RandomInRange;
			this.startRealTime = Time.realtimeSinceStartup;
			this.startFrame = Time.frameCount;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.startTick = 0;
			}
			foreach (SoundParamTarget_Volume key in (from m in this.subDef.paramMappings
			select m.outParam).OfType<SoundParamTarget_Volume>())
			{
				this.volumeInMappings.Add(key, 0f);
			}
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00198F78 File Offset: 0x00197178
		public virtual void Update()
		{
			this.source.pitch = this.SanitizedPitch;
			this.ApplyMappedParameters();
			this.source.volume = this.SanitizedVolume;
			if (this.source.volume < 0.001f)
			{
				this.source.mute = true;
			}
			else
			{
				this.source.mute = false;
			}
			if (this.subDef.tempoAffectedByGameSpeed && !this.TestPlaying)
			{
				if (Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused)
				{
					if (this.source.isPlaying)
					{
						this.source.Pause();
						return;
					}
				}
				else if (!this.source.isPlaying)
				{
					this.source.UnPause();
				}
			}
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x00199034 File Offset: 0x00197234
		public void ApplyMappedParameters()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.paramUpdateMode != SoundParamUpdateMode.OncePerSample || !this.mappingsApplied)
				{
					soundParameterMapping.Apply(this);
				}
			}
			this.mappingsApplied = true;
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x0019908D File Offset: 0x0019728D
		public void SignalMappedVolume(float value, SoundParamTarget sourceParam)
		{
			this.volumeInMappings[sourceParam] = value;
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x0019909C File Offset: 0x0019729C
		public virtual void SampleCleanup()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.curve.HasView)
				{
					soundParameterMapping.curve.View.ClearDebugInputFrom(this);
				}
			}
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x001990F4 File Offset: 0x001972F4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Sample_");
			defaultInterpolatedStringHandler.AppendFormatted(this.subDef.name);
			defaultInterpolatedStringHandler.AppendLiteral(" volume=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.source.volume);
			defaultInterpolatedStringHandler.AppendLiteral(" pitch=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.source.pitch);
			defaultInterpolatedStringHandler.AppendLiteral(" at ");
			defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.source.transform.position.ToIntVec3());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x00199194 File Offset: 0x00197394
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.startRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x04002A9D RID: 10909
		public SubSoundDef subDef;

		// Token: 0x04002A9E RID: 10910
		public AudioSource source;

		// Token: 0x04002A9F RID: 10911
		public float startRealTime;

		// Token: 0x04002AA0 RID: 10912
		public int startFrame;

		// Token: 0x04002AA1 RID: 10913
		public int startTick;

		// Token: 0x04002AA2 RID: 10914
		public float resolvedVolume;

		// Token: 0x04002AA3 RID: 10915
		public float resolvedPitch;

		// Token: 0x04002AA4 RID: 10916
		private bool mappingsApplied;

		// Token: 0x04002AA5 RID: 10917
		private Dictionary<SoundParamTarget, float> volumeInMappings = new Dictionary<SoundParamTarget, float>();
	}
}
