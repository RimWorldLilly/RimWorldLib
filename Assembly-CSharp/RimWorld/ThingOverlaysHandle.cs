using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A64 RID: 6756
	public class ThingOverlaysHandle
	{
		// Token: 0x17001DDB RID: 7643
		// (get) Token: 0x0600A9B3 RID: 43443 RVA: 0x003C2910 File Offset: 0x003C0B10
		public OverlayTypes OverlayTypes
		{
			get
			{
				if (this.overlayTypesDirty)
				{
					this.overlayTypes = OverlayTypes.None;
					for (int i = 0; i < this.overlayHandles.Count; i++)
					{
						this.overlayTypes |= this.overlayHandles[i].overlayType;
					}
					this.overlayTypesDirty = false;
				}
				return this.overlayTypes;
			}
		}

		// Token: 0x0600A9B4 RID: 43444 RVA: 0x003C296D File Offset: 0x003C0B6D
		public ThingOverlaysHandle(OverlayDrawer drawer, Thing thing)
		{
			this.drawer = drawer;
			this.thing = thing;
		}

		// Token: 0x0600A9B5 RID: 43445 RVA: 0x003C2990 File Offset: 0x003C0B90
		public OverlayHandle Enable(OverlayTypes overlayType)
		{
			int num = this.handleIdCounter;
			this.handleIdCounter = num + 1;
			OverlayHandle overlayHandle = new OverlayHandle(this, overlayType, num);
			this.overlayHandles.Add(overlayHandle);
			this.overlayTypesDirty = true;
			return overlayHandle;
		}

		// Token: 0x0600A9B6 RID: 43446 RVA: 0x003C29CC File Offset: 0x003C0BCC
		public void Disable(OverlayHandle handle)
		{
			for (int i = this.overlayHandles.Count - 1; i >= 0; i--)
			{
				if (this.overlayHandles[i].handleId == handle.handleId)
				{
					this.overlayHandles.RemoveAt(i);
				}
			}
			this.overlayTypesDirty = true;
		}

		// Token: 0x0600A9B7 RID: 43447 RVA: 0x003C2A1D File Offset: 0x003C0C1D
		public void Disable(ref OverlayHandle? handle)
		{
			if (handle != null)
			{
				this.Disable(handle.Value);
				handle = null;
			}
		}

		// Token: 0x0600A9B8 RID: 43448 RVA: 0x003C2A3A File Offset: 0x003C0C3A
		public void Dispose()
		{
			if (this.disposed)
			{
				Log.Error("Tried disposing already disposed ThingOverlaysHandle!");
				return;
			}
			this.disposed = true;
		}

		// Token: 0x04006345 RID: 25413
		public readonly OverlayDrawer drawer;

		// Token: 0x04006346 RID: 25414
		public readonly Thing thing;

		// Token: 0x04006347 RID: 25415
		private bool disposed;

		// Token: 0x04006348 RID: 25416
		private int handleIdCounter;

		// Token: 0x04006349 RID: 25417
		private bool overlayTypesDirty;

		// Token: 0x0400634A RID: 25418
		private OverlayTypes overlayTypes;

		// Token: 0x0400634B RID: 25419
		private List<OverlayHandle> overlayHandles = new List<OverlayHandle>();
	}
}
