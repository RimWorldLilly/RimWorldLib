using System;

namespace Verse
{
	// Token: 0x020003B8 RID: 952
	public class PawnRenderNodeProperties_Carried : PawnRenderNodeProperties
	{
		// Token: 0x06001C8A RID: 7306 RVA: 0x000AFD48 File Offset: 0x000ADF48
		public PawnRenderNodeProperties_Carried()
		{
			this.useGraphic = false;
			this.baseLayer = 90f;
			this.drawData = DrawData.NewWithData(new DrawData.RotationalData[]
			{
				new DrawData.RotationalData(new Rot4?(Rot4.North), -10f)
			});
		}
	}
}
