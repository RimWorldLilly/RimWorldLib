using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200026B RID: 619
	public struct FleckDrawPosition
	{
		// Token: 0x06001200 RID: 4608 RVA: 0x000670C2 File Offset: 0x000652C2
		public FleckDrawPosition(Vector3 worldPos, float height, Vector3 anchorOffset, Vector3 unattachedOffset)
		{
			this.worldPosition = worldPos;
			this.height = height;
			this.anchorOffset = anchorOffset;
			this.unattachedDrawOffset = unattachedOffset;
			this.attachedOffset = Vector3.zero;
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x000670EC File Offset: 0x000652EC
		public FleckDrawPosition(Vector3 worldPos, float height, Vector3 anchorOffset, Vector3 unattachedOffset, Vector3 attachedOffset)
		{
			this.worldPosition = worldPos;
			this.height = height;
			this.anchorOffset = anchorOffset;
			this.unattachedDrawOffset = unattachedOffset;
			this.attachedOffset = attachedOffset;
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x00067113 File Offset: 0x00065313
		public Vector3 ExactPosition
		{
			get
			{
				return this.worldPosition + this.unattachedDrawOffset + this.attachedOffset + Vector3.forward * this.height + this.anchorOffset;
			}
		}

		// Token: 0x04001058 RID: 4184
		public Vector3 worldPosition;

		// Token: 0x04001059 RID: 4185
		public float height;

		// Token: 0x0400105A RID: 4186
		public Vector3 anchorOffset;

		// Token: 0x0400105B RID: 4187
		public Vector3 unattachedDrawOffset;

		// Token: 0x0400105C RID: 4188
		public Vector3 attachedOffset;
	}
}
