using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000260 RID: 608
	public struct FleckCreationData
	{
		// Token: 0x0400101C RID: 4124
		public FleckDef def;

		// Token: 0x0400101D RID: 4125
		public Vector3 spawnPosition;

		// Token: 0x0400101E RID: 4126
		public float rotation;

		// Token: 0x0400101F RID: 4127
		public float scale;

		// Token: 0x04001020 RID: 4128
		public Vector3? exactScale;

		// Token: 0x04001021 RID: 4129
		public Color? instanceColor;

		// Token: 0x04001022 RID: 4130
		public float velocityAngle;

		// Token: 0x04001023 RID: 4131
		public float velocitySpeed;

		// Token: 0x04001024 RID: 4132
		public Vector3? velocity;

		// Token: 0x04001025 RID: 4133
		public float rotationRate;

		// Token: 0x04001026 RID: 4134
		public float? solidTimeOverride;

		// Token: 0x04001027 RID: 4135
		public float? airTimeLeft;

		// Token: 0x04001028 RID: 4136
		public int ageTicksOverride;

		// Token: 0x04001029 RID: 4137
		public FleckAttachLink link;

		// Token: 0x0400102A RID: 4138
		public float targetSize;

		// Token: 0x0400102B RID: 4139
		public float orbitSpeed;

		// Token: 0x0400102C RID: 4140
		public float orbitSnapStrength;
	}
}
