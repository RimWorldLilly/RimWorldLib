using System;

namespace Verse.Sound
{
	// Token: 0x020007D1 RID: 2001
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06003FD5 RID: 16341 RVA: 0x00197F38 File Offset: 0x00196138
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x00197F3F File Offset: 0x0019613F
		public override float ValueFor(Sample samp)
		{
			if (Current.ProgramState != ProgramState.Playing || Find.MusicManagerPlay == null)
			{
				return 1f;
			}
			return Find.MusicManagerPlay.subtleAmbienceSoundVolumeMultiplier;
		}
	}
}
