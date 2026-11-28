using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC2 RID: 4034
	public class PlanetLayerSettings : IExposable
	{
		// Token: 0x0600667C RID: 26236 RVA: 0x002460FC File Offset: 0x002442FC
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 100f, false);
			Scribe_Values.Look<float>(ref this.viewAngle, "viewAngle", 180f, false);
			Scribe_Values.Look<float>(ref this.extraCameraAltitude, "extraCameraAltitude", 0f, false);
			Scribe_Values.Look<int>(ref this.subdivisions, "subdivisions", 10, false);
			Scribe_Values.Look<bool>(ref this.useSurfaceViewAngle, "useSurfaceViewAngle", false, false);
			Scribe_Values.Look<float>(ref this.backgroundWorldCameraOffset, "backgroundWorldCameraOffset", 0f, false);
			Scribe_Values.Look<float>(ref this.backgroundWorldCameraParallaxDistancePer100Cells, "backgroundWorldCameraParallaxDistancePer100Cells", 0f, false);
		}

		// Token: 0x04003EFC RID: 16124
		public Vector3 origin;

		// Token: 0x04003EFD RID: 16125
		public float radius = 100f;

		// Token: 0x04003EFE RID: 16126
		public float viewAngle = 180f;

		// Token: 0x04003EFF RID: 16127
		public float extraCameraAltitude;

		// Token: 0x04003F00 RID: 16128
		public int subdivisions = 10;

		// Token: 0x04003F01 RID: 16129
		public bool useSurfaceViewAngle;

		// Token: 0x04003F02 RID: 16130
		public float backgroundWorldCameraOffset;

		// Token: 0x04003F03 RID: 16131
		public float backgroundWorldCameraParallaxDistancePer100Cells;
	}
}
