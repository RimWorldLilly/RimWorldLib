using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001616 RID: 5654
	public class Capture
	{
		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x06008CDF RID: 36063 RVA: 0x00333104 File Offset: 0x00331304
		public bool IsTerrainCapture
		{
			get
			{
				return this.engine == null;
			}
		}

		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x06008CE0 RID: 36064 RVA: 0x0033310F File Offset: 0x0033130F
		public bool IsGravshipCapture
		{
			get
			{
				return this.engine != null;
			}
		}

		// Token: 0x06008CE1 RID: 36065 RVA: 0x0033311A File Offset: 0x0033131A
		public Capture(Building_GravEngine engine)
		{
			this.engine = engine;
		}

		// Token: 0x04005653 RID: 22099
		public Building_GravEngine engine;

		// Token: 0x04005654 RID: 22100
		public SavedTexture2D capture;

		// Token: 0x04005655 RID: 22101
		public Vector3 drawSize;

		// Token: 0x04005656 RID: 22102
		public Vector3 captureCenter;

		// Token: 0x04005657 RID: 22103
		public float minCameraSize;

		// Token: 0x04005658 RID: 22104
		public float maxCameraSize;

		// Token: 0x04005659 RID: 22105
		public CellRect captureBounds;
	}
}
