using System;

namespace Verse.Sound
{
	// Token: 0x020007D2 RID: 2002
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06003FD8 RID: 16344 RVA: 0x00197F68 File Offset: 0x00196168
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x00197F6F File Offset: 0x0019616F
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}
