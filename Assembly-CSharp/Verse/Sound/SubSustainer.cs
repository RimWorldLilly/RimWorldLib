using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007FA RID: 2042
	public class SubSustainer
	{
		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x0600406F RID: 16495 RVA: 0x0019A63B File Offset: 0x0019883B
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x06004070 RID: 16496 RVA: 0x0019A648 File Offset: 0x00198848
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x0019A658 File Offset: 0x00198858
		public SubSustainer(Sustainer parent, SubSoundDef subSoundDef)
		{
			this.parent = parent;
			this.subDef = subSoundDef;
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.creationFrame = Time.frameCount;
				this.creationRealTime = Time.realtimeSinceStartup;
				if (Current.ProgramState == ProgramState.Playing)
				{
					this.creationTick = Find.TickManager.TicksGame;
				}
				if (this.subDef.startDelayRange.TrueMax < 0.001f)
				{
					this.StartSample(this.subDef.randomStartPoint ? Rand.Value : 0f);
					return;
				}
				this.nextSampleStartTime = Time.realtimeSinceStartup + this.subDef.startDelayRange.RandomInRange;
			});
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0019A6B0 File Offset: 0x001988B0
		private void StartSample(float startPct = 0f)
		{
			ResolvedGrain resolvedGrain = this.subDef.RandomizedResolvedGrain();
			if (resolvedGrain == null)
			{
				string[] array = new string[5];
				array[0] = "SubSustainer for ";
				int num = 1;
				SubSoundDef subSoundDef = this.subDef;
				array[num] = ((subSoundDef != null) ? subSoundDef.ToString() : null);
				array[2] = " of ";
				int num2 = 3;
				SoundDef def = this.parent.def;
				array[num2] = ((def != null) ? def.ToString() : null);
				array[4] = " could not resolve any grains.";
				Log.Error(string.Concat(array));
				this.parent.End();
				return;
			}
			float num3;
			if (this.subDef.sustainLoop)
			{
				num3 = this.subDef.sustainLoopDurationRange.RandomInRange;
			}
			else
			{
				num3 = resolvedGrain.duration;
			}
			float num4 = resolvedGrain.duration * startPct;
			float num5 = 1f;
			if (this.subDef.sustainIntervalFactorByAggregateSize != null)
			{
				SimpleCurve sustainIntervalFactorByAggregateSize = this.subDef.sustainIntervalFactorByAggregateSize;
				SoundParams externalParams = this.ExternalParams;
				float? num6;
				if (externalParams == null)
				{
					num6 = null;
				}
				else
				{
					SoundSizeAggregator sizeAggregator = externalParams.sizeAggregator;
					num6 = ((sizeAggregator != null) ? new float?(sizeAggregator.AggregateSize) : null);
				}
				float? num7 = num6;
				num5 = sustainIntervalFactorByAggregateSize.Evaluate(num7.GetValueOrDefault(1f));
			}
			float num8 = Time.realtimeSinceStartup + num3 - num4;
			this.nextSampleStartTime = num8 + this.subDef.sustainIntervalRange.RandomInRange * num5;
			if (this.nextSampleStartTime < Time.realtimeSinceStartup + 0.01f)
			{
				this.nextSampleStartTime = Time.realtimeSinceStartup + 0.01f;
			}
			if (resolvedGrain is ResolvedGrain_Silence)
			{
				return;
			}
			SampleSustainer sampleSustainer = SampleSustainer.TryMakeAndPlay(this, ((ResolvedGrain_Clip)resolvedGrain).clip, num8, num4);
			this.subDef.Notify_GrainPlayed(resolvedGrain);
			if (sampleSustainer == null)
			{
				return;
			}
			if (this.subDef.sustainSkipFirstAttack && Time.frameCount == this.creationFrame)
			{
				sampleSustainer.resolvedSkipAttack = true;
			}
			this.samples.Add(sampleSustainer);
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0019A870 File Offset: 0x00198A70
		public void SubSustainerUpdate()
		{
			for (int i = this.samples.Count - 1; i >= 0; i--)
			{
				if (Time.realtimeSinceStartup > this.samples[i].scheduledEndTime)
				{
					this.EndSample(this.samples[i]);
				}
			}
			if (Time.realtimeSinceStartup > this.nextSampleStartTime)
			{
				this.StartSample(0f);
			}
			for (int j = 0; j < this.samples.Count; j++)
			{
				this.samples[j].Update();
			}
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0019A8FE File Offset: 0x00198AFE
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0019A913 File Offset: 0x00198B13
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x0019A937 File Offset: 0x00198B37
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame.ToString();
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x0019A959 File Offset: 0x00198B59
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0019A974 File Offset: 0x00198B74
		public string SamplesDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SampleSustainer sampleSustainer in this.samples)
			{
				stringBuilder.AppendLine(sampleSustainer.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002ACD RID: 10957
		public Sustainer parent;

		// Token: 0x04002ACE RID: 10958
		public SubSoundDef subDef;

		// Token: 0x04002ACF RID: 10959
		private List<SampleSustainer> samples = new List<SampleSustainer>();

		// Token: 0x04002AD0 RID: 10960
		private float nextSampleStartTime;

		// Token: 0x04002AD1 RID: 10961
		public int creationFrame = -1;

		// Token: 0x04002AD2 RID: 10962
		public int creationTick = -1;

		// Token: 0x04002AD3 RID: 10963
		public float creationRealTime = -1f;

		// Token: 0x04002AD4 RID: 10964
		private const float MinSampleStartInterval = 0.01f;
	}
}
