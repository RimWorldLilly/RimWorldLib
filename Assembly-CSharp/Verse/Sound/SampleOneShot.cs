using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007EA RID: 2026
	public class SampleOneShot : Sample
	{
		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x0600402C RID: 16428 RVA: 0x001991AC File Offset: 0x001973AC
		public override float ParentStartRealTime
		{
			get
			{
				return this.startRealTime;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x001991B4 File Offset: 0x001973B4
		public override float ParentStartTick
		{
			get
			{
				return (float)this.startTick;
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x0600402E RID: 16430 RVA: 0x001991BD File Offset: 0x001973BD
		public override float ParentHashCode
		{
			get
			{
				return (float)this.GetHashCode();
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x0600402F RID: 16431 RVA: 0x001991C6 File Offset: 0x001973C6
		public override SoundParams ExternalParams
		{
			get
			{
				return this.externalParams;
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06004030 RID: 16432 RVA: 0x001991CE File Offset: 0x001973CE
		public override SoundInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x001991D6 File Offset: 0x001973D6
		private SampleOneShot(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x001991EC File Offset: 0x001973EC
		public static SampleOneShot TryMakeAndPlay(SubSoundDef def, AudioClip clip, SoundInfo info)
		{
			if ((double)info.pitchFactor <= 0.0001)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Played sound with pitchFactor ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(info.pitchFactor);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<SubSoundDef>(def);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<SoundInfo>(info);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 632321);
				return null;
			}
			SampleOneShot sampleOneShot = new SampleOneShot(def)
			{
				info = info,
				source = Find.SoundRoot.sourcePool.GetSource(def.onCamera)
			};
			if (sampleOneShot.source == null)
			{
				return null;
			}
			sampleOneShot.source.clip = clip;
			sampleOneShot.source.volume = sampleOneShot.SanitizedVolume;
			sampleOneShot.source.pitch = sampleOneShot.SanitizedPitch;
			sampleOneShot.source.minDistance = sampleOneShot.subDef.distRange.TrueMin;
			sampleOneShot.source.maxDistance = sampleOneShot.subDef.distRange.TrueMax;
			if (!def.onCamera)
			{
				sampleOneShot.source.gameObject.transform.position = info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
				sampleOneShot.source.minDistance = def.distRange.TrueMin;
				sampleOneShot.source.maxDistance = def.distRange.TrueMax;
				sampleOneShot.source.spatialBlend = 1f;
				if (def.canVacuumDampen)
				{
					Map map = info.Maker.Map;
					bool flag;
					if (map == null)
					{
						flag = false;
					}
					else
					{
						BiomeDef biome = map.Biome;
						flag = ((biome != null) ? new bool?(biome.inVacuum) : null).GetValueOrDefault();
					}
					if (flag)
					{
						float num = EasingFunctions.EaseOutCubic(info.Maker.Cell.GetVacuum(info.Maker.Map));
						sampleOneShot.source.volume *= Mathf.Lerp(1f, 0.05f, num);
						SampleOneShot.VacuumHighPassFilter.cutoffFrequency = SampleOneShot.VacuumFrequencyCutoffCurve.Evaluate(num);
						SampleOneShot.VacuumHighPassFilter.SetupOn(sampleOneShot.source);
					}
				}
			}
			else
			{
				sampleOneShot.source.spatialBlend = 0f;
			}
			for (int i = 0; i < def.filters.Count; i++)
			{
				def.filters[i].SetupOn(sampleOneShot.source);
			}
			foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
			{
				sampleOneShot.externalParams[keyValuePair.Key] = keyValuePair.Value;
			}
			sampleOneShot.Update();
			sampleOneShot.source.Play();
			Find.SoundRoot.oneShotManager.TryAddPlayingOneShot(sampleOneShot);
			return sampleOneShot;
		}

		// Token: 0x04002AA6 RID: 10918
		public SoundInfo info;

		// Token: 0x04002AA7 RID: 10919
		private const float MaxVacuumVolumePercent = 0.05f;

		// Token: 0x04002AA8 RID: 10920
		private SoundParams externalParams = new SoundParams();

		// Token: 0x04002AA9 RID: 10921
		private static readonly SimpleCurve VacuumFrequencyCutoffCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 22000f),
				true
			},
			{
				new CurvePoint(0.25f, 4000f),
				true
			},
			{
				new CurvePoint(1f, 400f),
				true
			}
		};

		// Token: 0x04002AAA RID: 10922
		private static readonly SoundFilterLowPass VacuumHighPassFilter = new SoundFilterLowPass
		{
			lowpassResonaceQ = 0f
		};
	}
}
