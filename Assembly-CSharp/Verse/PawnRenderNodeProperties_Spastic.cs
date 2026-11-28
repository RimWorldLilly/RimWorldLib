using System;

namespace Verse
{
	// Token: 0x020003BA RID: 954
	public class PawnRenderNodeProperties_Spastic : PawnRenderNodeProperties
	{
		// Token: 0x06001C8D RID: 7309 RVA: 0x000AFDB0 File Offset: 0x000ADFB0
		public PawnRenderNodeProperties_Spastic()
		{
			this.nodeClass = typeof(PawnRenderNode_Spastic);
			this.workerClass = typeof(PawnRenderNodeWorker_Spastic);
		}

		// Token: 0x04001751 RID: 5969
		public bool rotateFacing = true;

		// Token: 0x04001752 RID: 5970
		public FloatRange scaleRange = FloatRange.One;

		// Token: 0x04001753 RID: 5971
		public FloatRange rotationRange = FloatRange.Zero;

		// Token: 0x04001754 RID: 5972
		public FloatRange offsetRangeX = FloatRange.Zero;

		// Token: 0x04001755 RID: 5973
		public FloatRange offsetRangeZ = FloatRange.Zero;

		// Token: 0x04001756 RID: 5974
		public IntRange durationTicksRange = new IntRange(60, 60);

		// Token: 0x04001757 RID: 5975
		public IntRange nextSpasmTicksRange = new IntRange(60, 60);
	}
}
