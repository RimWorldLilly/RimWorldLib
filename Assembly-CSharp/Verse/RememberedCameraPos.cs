using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000106 RID: 262
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x0600091F RID: 2335 RVA: 0x00034038 File Offset: 0x00032238
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0003406C File Offset: 0x0003226C
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}

		// Token: 0x04000441 RID: 1089
		public Vector3 rootPos;

		// Token: 0x04000442 RID: 1090
		public float rootSize;
	}
}
