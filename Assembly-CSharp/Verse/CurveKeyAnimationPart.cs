using System;
using System.Collections.Generic;
using LudeonTK;

namespace Verse
{
	// Token: 0x02000114 RID: 276
	public class CurveKeyAnimationPart : AnimationPart
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x000359B7 File Offset: 0x00033BB7
		protected override Type DefaultWorker
		{
			get
			{
				return typeof(AnimationWorker_Curves);
			}
		}

		// Token: 0x04000478 RID: 1144
		public List<CurveKey> keyframes;

		// Token: 0x04000479 RID: 1145
		public bool normalized = true;

		// Token: 0x0400047A RID: 1146
		public Vector3Curve offset = new Vector3Curve();

		// Token: 0x0400047B RID: 1147
		public ComplexCurve angle = new ComplexCurve();

		// Token: 0x0400047C RID: 1148
		public Vector3Curve scale = new Vector3Curve();
	}
}
