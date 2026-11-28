using System;

namespace RimWorld
{
	// Token: 0x020018D7 RID: 6359
	public class CompProperties_Launchable_TransportPod : CompProperties_Launchable
	{
		// Token: 0x06009F87 RID: 40839 RVA: 0x003936FA File Offset: 0x003918FA
		public CompProperties_Launchable_TransportPod()
		{
			this.compClass = typeof(CompLaunchable_TransportPod);
		}

		// Token: 0x04005E56 RID: 24150
		public bool requiresFuelingPort = true;
	}
}
