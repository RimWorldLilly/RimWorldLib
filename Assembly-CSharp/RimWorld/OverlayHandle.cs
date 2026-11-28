using System;

namespace RimWorld
{
	// Token: 0x02001A63 RID: 6755
	public struct OverlayHandle
	{
		// Token: 0x0600A9B2 RID: 43442 RVA: 0x003C28F6 File Offset: 0x003C0AF6
		public OverlayHandle(ThingOverlaysHandle thingOverlayHandle, OverlayTypes overlayType, int handleId)
		{
			this.thingOverlayHandle = thingOverlayHandle;
			this.overlayType = overlayType;
			this.handleId = handleId;
		}

		// Token: 0x04006342 RID: 25410
		public readonly ThingOverlaysHandle thingOverlayHandle;

		// Token: 0x04006343 RID: 25411
		public readonly OverlayTypes overlayType;

		// Token: 0x04006344 RID: 25412
		public int handleId;
	}
}
