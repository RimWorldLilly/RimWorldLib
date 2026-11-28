using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001A6 RID: 422
	public class AlternateGraphic
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x0003E41E File Offset: 0x0003C61E
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0003E428 File Offset: 0x0003C628
		public Graphic GetGraphic(Graphic other)
		{
			if (this.graphicData == null)
			{
				this.graphicData = new GraphicData();
			}
			this.graphicData.CopyFrom(other.data);
			if (!this.texPath.NullOrEmpty())
			{
				this.graphicData.texPath = this.texPath;
			}
			this.graphicData.color = (this.color ?? other.color);
			this.graphicData.colorTwo = (this.colorTwo ?? other.colorTwo);
			return this.graphicData.Graphic;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0003E4D8 File Offset: 0x0003C6D8
		public Graphic GetSwimmingGraphic(Graphic other)
		{
			if (this.swimmingGraphicData == null)
			{
				this.swimmingGraphicData = new GraphicData();
			}
			this.swimmingGraphicData.CopyFrom(other.data);
			if (!this.swimmingTexPath.NullOrEmpty())
			{
				this.swimmingGraphicData.texPath = this.swimmingTexPath;
			}
			return this.swimmingGraphicData.Graphic;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0003E534 File Offset: 0x0003C734
		public Graphic GetDessicatedGraphic(Graphic other)
		{
			if (this.dessicatedGraphicData == null)
			{
				this.dessicatedGraphicData = new GraphicData();
			}
			this.dessicatedGraphicData.CopyFrom(other.data);
			if (!this.dessicatedTexPath.NullOrEmpty())
			{
				this.dessicatedGraphicData.texPath = this.dessicatedTexPath;
			}
			return this.dessicatedGraphicData.Graphic;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0003E590 File Offset: 0x0003C790
		public Graphic GetRottingGraphic(Graphic other)
		{
			if (this.rottingGraphicData == null)
			{
				this.rottingGraphicData = new GraphicData();
			}
			this.rottingGraphicData.CopyFrom(other.data);
			if (!this.rottingTexPath.NullOrEmpty())
			{
				this.rottingGraphicData.texPath = this.rottingTexPath;
			}
			return this.rottingGraphicData.Graphic;
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0003E5EC File Offset: 0x0003C7EC
		public Graphic GetStationaryGraphic(Graphic other)
		{
			if (this.stationaryGraphicData == null)
			{
				this.stationaryGraphicData = new GraphicData();
			}
			this.stationaryGraphicData.CopyFrom(other.data);
			if (!this.stationaryTexPath.NullOrEmpty())
			{
				this.stationaryGraphicData.texPath = this.stationaryTexPath;
			}
			return this.stationaryGraphicData.Graphic;
		}

		// Token: 0x04000AD5 RID: 2773
		private float weight = 0.5f;

		// Token: 0x04000AD6 RID: 2774
		private string texPath;

		// Token: 0x04000AD7 RID: 2775
		private string rottingTexPath;

		// Token: 0x04000AD8 RID: 2776
		private string dessicatedTexPath;

		// Token: 0x04000AD9 RID: 2777
		private string swimmingTexPath;

		// Token: 0x04000ADA RID: 2778
		private string stationaryTexPath;

		// Token: 0x04000ADB RID: 2779
		private Color? color;

		// Token: 0x04000ADC RID: 2780
		private Color? colorTwo;

		// Token: 0x04000ADD RID: 2781
		public GraphicData graphicData;

		// Token: 0x04000ADE RID: 2782
		public GraphicData dessicatedGraphicData;

		// Token: 0x04000ADF RID: 2783
		public GraphicData swimmingGraphicData;

		// Token: 0x04000AE0 RID: 2784
		public GraphicData rottingGraphicData;

		// Token: 0x04000AE1 RID: 2785
		public GraphicData stationaryGraphicData;

		// Token: 0x04000AE2 RID: 2786
		public List<AttachPoint> attachPoints;
	}
}
