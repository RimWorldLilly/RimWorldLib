using System;

namespace Verse
{
	// Token: 0x02000171 RID: 369
	public class GraphicStateDef : Def
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0003BE44 File Offset: 0x0003A044
		public Graphic DefaultGraphic
		{
			get
			{
				return this.defaultGraphicData.Graphic;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0003BE51 File Offset: 0x0003A051
		public GraphicData DefaultGraphicData
		{
			get
			{
				return this.defaultGraphicData;
			}
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0003BE59 File Offset: 0x0003A059
		public bool TryGetDefaultGraphic(out Graphic graphic)
		{
			GraphicData graphicData = this.defaultGraphicData;
			graphic = ((graphicData != null) ? graphicData.Graphic : null);
			return graphic != null;
		}

		// Token: 0x0400086C RID: 2156
		public GraphicData defaultGraphicData;

		// Token: 0x0400086D RID: 2157
		public bool applySkinColorTint;
	}
}
