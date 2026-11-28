using System;
using UnityEngine;
using Verse.Noise;

namespace Verse.Sound
{
	// Token: 0x020007D5 RID: 2005
	public class SoundParamSource_Perlin : SoundParamSource
	{
		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06003FDB RID: 16347 RVA: 0x00197F7E File Offset: 0x0019617E
		public override string Label
		{
			get
			{
				return "Perlin noise";
			}
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x00197F88 File Offset: 0x00196188
		public override float ValueFor(Sample samp)
		{
			float num;
			if (this.syncType == PerlinMappingSyncType.Sync)
			{
				num = samp.ParentHashCode % 100f;
			}
			else
			{
				num = (float)(samp.GetHashCode() % 100);
			}
			if (this.timeType == TimeType.Ticks && Current.ProgramState == ProgramState.Playing)
			{
				float num2;
				if (this.syncType == PerlinMappingSyncType.Sync)
				{
					num2 = (float)Find.TickManager.TicksGame - samp.ParentStartTick;
				}
				else
				{
					num2 = (float)(Find.TickManager.TicksGame - samp.startTick);
				}
				num2 /= 60f;
				num += num2;
			}
			else
			{
				float num3;
				if (this.syncType == PerlinMappingSyncType.Sync)
				{
					num3 = Time.realtimeSinceStartup - samp.ParentStartRealTime;
				}
				else
				{
					num3 = Time.realtimeSinceStartup - samp.startRealTime;
				}
				num += num3;
			}
			num *= this.perlinFrequency;
			return ((float)SoundParamSource_Perlin.perlin.GetValue((double)num, 0.0, 0.0) * 2f + 1f) / 2f;
		}

		// Token: 0x04002A55 RID: 10837
		[Description("The type of time on which this perlin randomizer will work. If you use Ticks, it will freeze when paused and speed up in fast forward.")]
		public TimeType timeType;

		// Token: 0x04002A56 RID: 10838
		[Description("The frequency of the perlin output. The input time is multiplied by this amount.")]
		public float perlinFrequency = 1f;

		// Token: 0x04002A57 RID: 10839
		[Description("Whether to synchronize the Perlin output across different samples. If set to desync, each playing sample will get a separate Perlin output.")]
		public PerlinMappingSyncType syncType;

		// Token: 0x04002A58 RID: 10840
		private static Perlin perlin = new Perlin(0.009999999776482582, 2.0, 0.5, 4, Rand.Range(0, int.MaxValue), QualityMode.Medium);
	}
}
