using System;

namespace Verse
{
	// Token: 0x020003BB RID: 955
	public class PawnRenderNodeProperties_BulbfreakTentacle : PawnRenderNodeProperties_Spastic
	{
		// Token: 0x06001C8E RID: 7310 RVA: 0x000AFE34 File Offset: 0x000AE034
		public PawnRenderNodeProperties_BulbfreakTentacle()
		{
			this.rotateFacing = true;
			this.baseLayer = 10f;
			this.scaleRange = new FloatRange(0.7f, 1.2f);
			this.rotationRange = new FloatRange(-25f, 25f);
			this.durationTicksRange = new IntRange(10, 35);
			this.nextSpasmTicksRange = new IntRange(0, 20);
		}
	}
}
