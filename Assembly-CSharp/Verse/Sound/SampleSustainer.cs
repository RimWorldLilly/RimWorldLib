using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007EC RID: 2028
	public class SampleSustainer : Sample
	{
		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x0600403F RID: 16447 RVA: 0x001998F6 File Offset: 0x00197AF6
		public override float ParentStartRealTime
		{
			get
			{
				return this.subSustainer.creationRealTime;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06004040 RID: 16448 RVA: 0x00199903 File Offset: 0x00197B03
		public override float ParentStartTick
		{
			get
			{
				return (float)this.subSustainer.creationTick;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x00199911 File Offset: 0x00197B11
		public override float ParentHashCode
		{
			get
			{
				return (float)this.subSustainer.GetHashCode();
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06004042 RID: 16450 RVA: 0x0019991F File Offset: 0x00197B1F
		public override SoundParams ExternalParams
		{
			get
			{
				return this.subSustainer.ExternalParams;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06004043 RID: 16451 RVA: 0x0019992C File Offset: 0x00197B2C
		public override SoundInfo Info
		{
			get
			{
				return this.subSustainer.Info;
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06004044 RID: 16452 RVA: 0x0019993C File Offset: 0x00197B3C
		protected override float Volume
		{
			get
			{
				float num = base.Volume * this.subSustainer.parent.scopeFader.inScopePercent;
				float num2 = 1f;
				if (this.subSustainer.parent.Ended)
				{
					num2 = 1f - Mathf.Min(this.subSustainer.parent.TimeSinceEnd / this.subDef.parentDef.sustainFadeoutTime, 1f);
				}
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (base.AgeRealTime < this.subDef.sustainAttack)
				{
					if (this.resolvedSkipAttack || this.subDef.sustainAttack < 0.01f)
					{
						return num * num2;
					}
					float num3 = base.AgeRealTime / this.subDef.sustainAttack;
					num3 = Mathf.Sqrt(num3);
					return Mathf.Lerp(0f, num, num3) * num2;
				}
				else
				{
					if (realtimeSinceStartup > this.scheduledEndTime - this.subDef.sustainRelease)
					{
						float num4 = (realtimeSinceStartup - (this.scheduledEndTime - this.subDef.sustainRelease)) / this.subDef.sustainRelease;
						num4 = 1f - num4;
						num4 = Mathf.Max(num4, 0f);
						num4 = Mathf.Sqrt(num4);
						num4 = 1f - num4;
						return Mathf.Lerp(num, 0f, num4) * num2;
					}
					return num * num2;
				}
			}
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x00199A85 File Offset: 0x00197C85
		private SampleSustainer(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x00199A90 File Offset: 0x00197C90
		public static SampleSustainer TryMakeAndPlay(SubSustainer subSus, AudioClip clip, float scheduledEndTime, float startTime = 0f)
		{
			SampleSustainer sampleSustainer = new SampleSustainer(subSus.subDef);
			sampleSustainer.subSustainer = subSus;
			sampleSustainer.scheduledEndTime = scheduledEndTime;
			GameObject gameObject = new GameObject("SampleSource_" + sampleSustainer.subDef.name + "_" + sampleSustainer.startRealTime.ToString());
			GameObject gameObject2 = subSus.subDef.onCamera ? Find.Camera.gameObject : subSus.parent.worldRootObject;
			gameObject.transform.parent = gameObject2.transform;
			gameObject.transform.localPosition = Vector3.zero;
			sampleSustainer.source = AudioSourceMaker.NewAudioSourceOn(gameObject);
			if (sampleSustainer.source == null)
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
				return null;
			}
			sampleSustainer.source.clip = clip;
			sampleSustainer.source.volume = sampleSustainer.SanitizedVolume;
			sampleSustainer.source.pitch = sampleSustainer.SanitizedPitch;
			sampleSustainer.source.minDistance = sampleSustainer.subDef.distRange.TrueMin;
			sampleSustainer.source.maxDistance = sampleSustainer.subDef.distRange.TrueMax;
			sampleSustainer.source.spatialBlend = 1f;
			List<SoundFilter> filters = sampleSustainer.subDef.filters;
			for (int i = 0; i < filters.Count; i++)
			{
				filters[i].SetupOn(sampleSustainer.source);
			}
			if (sampleSustainer.subDef.sustainLoop)
			{
				sampleSustainer.source.loop = true;
			}
			sampleSustainer.Update();
			sampleSustainer.source.time = startTime;
			sampleSustainer.source.Play();
			sampleSustainer.source.Play();
			return sampleSustainer;
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x00199C3D File Offset: 0x00197E3D
		public override void SampleCleanup()
		{
			base.SampleCleanup();
			if (this.source != null && this.source.gameObject != null)
			{
				Object.Destroy(this.source.gameObject);
			}
		}

		// Token: 0x04002AAD RID: 10925
		public SubSustainer subSustainer;

		// Token: 0x04002AAE RID: 10926
		public float scheduledEndTime;

		// Token: 0x04002AAF RID: 10927
		public bool resolvedSkipAttack;
	}
}
