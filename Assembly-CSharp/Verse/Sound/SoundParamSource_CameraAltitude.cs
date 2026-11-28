using System;

namespace Verse.Sound
{
	// Token: 0x020007CF RID: 1999
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06003FCF RID: 16335 RVA: 0x00197ED8 File Offset: 0x001960D8
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x00197EDF File Offset: 0x001960DF
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}
