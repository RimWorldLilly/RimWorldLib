using System;

namespace Verse.Sound
{
	// Token: 0x020007D0 RID: 2000
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06003FD2 RID: 16338 RVA: 0x00197EFD File Offset: 0x001960FD
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00197F04 File Offset: 0x00196104
		public override float ValueFor(Sample samp)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 0f;
			}
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			return Find.CurrentMap.mapTemperature.OutdoorTemp;
		}
	}
}
