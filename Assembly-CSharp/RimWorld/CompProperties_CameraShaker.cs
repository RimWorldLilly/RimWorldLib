using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC3 RID: 3779
	public class CompProperties_CameraShaker : CompProperties
	{
		// Token: 0x06006361 RID: 25441 RVA: 0x0023B385 File Offset: 0x00239585
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}

		// Token: 0x04003794 RID: 14228
		public float mag = 0.05f;
	}
}
