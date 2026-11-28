using System;

namespace RimWorld.Planet
{
	// Token: 0x020022D2 RID: 8914
	public class WorldObjectCompProperties_EnterCooldown : WorldObjectCompProperties
	{
		// Token: 0x0600DBD8 RID: 56280 RVA: 0x0051E7C1 File Offset: 0x0051C9C1
		public WorldObjectCompProperties_EnterCooldown()
		{
			this.compClass = typeof(EnterCooldownComp);
		}

		// Token: 0x04008D1D RID: 36125
		public bool autoStartOnMapRemoved = true;

		// Token: 0x04008D1E RID: 36126
		public float durationDays = 1f;
	}
}
