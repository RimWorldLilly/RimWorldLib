using System;

namespace RimWorld
{
	// Token: 0x02000D37 RID: 3383
	public class ShipJob_WaitForever : ShipJob_Wait
	{
		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x06005EDB RID: 24283 RVA: 0x0022AE23 File Offset: 0x00229023
		protected override bool ShouldEnd
		{
			get
			{
				return this.transportShip.shipThing == null || this.transportShip.shipThing.Destroyed;
			}
		}
	}
}
