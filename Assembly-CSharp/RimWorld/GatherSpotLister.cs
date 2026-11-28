using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200189E RID: 6302
	public class GatherSpotLister
	{
		// Token: 0x06009DD7 RID: 40407 RVA: 0x0038B170 File Offset: 0x00389370
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x06009DD8 RID: 40408 RVA: 0x0038B18C File Offset: 0x0038938C
		public void RegisterDeactivated(CompGatherSpot spot)
		{
			if (this.activeSpots.Contains(spot))
			{
				this.activeSpots.Remove(spot);
			}
		}

		// Token: 0x04005D6B RID: 23915
		public List<CompGatherSpot> activeSpots = new List<CompGatherSpot>();
	}
}
