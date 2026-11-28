using System;

namespace Verse.Sound
{
	// Token: 0x020007DB RID: 2011
	public class SoundParamTarget_Pitch : SoundParamTarget
	{
		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06003FEC RID: 16364 RVA: 0x00198136 File Offset: 0x00196336
		public override string Label
		{
			get
			{
				return "Pitch";
			}
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x00198140 File Offset: 0x00196340
		public override void SetOn(Sample sample, float value)
		{
			float num;
			if (this.pitchType == PitchParamType.Multiply)
			{
				num = value;
			}
			else
			{
				num = (float)Math.Pow(1.05946, (double)value);
			}
			sample.source.pitch = AudioSourceUtility.GetSanitizedPitch(sample.SanitizedPitch * num, "SoundParamTarget_Pitch");
		}

		// Token: 0x04002A5D RID: 10845
		[Description("The scale used for this pitch input.\n\nMultiply means a multiplier for the natural frequency of the sound. 1.0 gives normal sound, 0.5 gives twice as long and one octave down, and 2.0 gives half as long and an octave up.\n\nSemitones sets a number of semitones to offset the sound.")]
		private PitchParamType pitchType;
	}
}
