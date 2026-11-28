using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000143 RID: 323
	public class MoteProperties
	{
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0003851B File Offset: 0x0003671B
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x0400060F RID: 1551
		public bool realTime;

		// Token: 0x04000610 RID: 1552
		public float fadeInTime;

		// Token: 0x04000611 RID: 1553
		public float solidTime = 1f;

		// Token: 0x04000612 RID: 1554
		public float fadeOutTime;

		// Token: 0x04000613 RID: 1555
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x04000614 RID: 1556
		public float speedPerTime;

		// Token: 0x04000615 RID: 1557
		public float growthRate;

		// Token: 0x04000616 RID: 1558
		public bool collide;

		// Token: 0x04000617 RID: 1559
		public float archHeight;

		// Token: 0x04000618 RID: 1560
		public float archDuration;

		// Token: 0x04000619 RID: 1561
		public float archStartOffset;

		// Token: 0x0400061A RID: 1562
		public SoundDef landSound;

		// Token: 0x0400061B RID: 1563
		public Vector3 unattachedDrawOffset = Vector3.zero;

		// Token: 0x0400061C RID: 1564
		public Vector3 attachedDrawOffset;

		// Token: 0x0400061D RID: 1565
		public bool needsMaintenance;

		// Token: 0x0400061E RID: 1566
		public bool rotateTowardsTarget;

		// Token: 0x0400061F RID: 1567
		public bool rotateTowardsMoveDirection;

		// Token: 0x04000620 RID: 1568
		public bool scaleToConnectTargets;

		// Token: 0x04000621 RID: 1569
		public bool attachedToHead;

		// Token: 0x04000622 RID: 1570
		public bool fadeOutUnmaintained;

		// Token: 0x04000623 RID: 1571
		public bool updateOffsetToMatchTargetRotation;

		// Token: 0x04000624 RID: 1572
		public float yFightingOffsetScalar01 = 1f;

		// Token: 0x04000625 RID: 1573
		public List<CurvedScaler> scalers;
	}
}
