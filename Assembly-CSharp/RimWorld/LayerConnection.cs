using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B0 RID: 6064
	public class LayerConnection : IEquatable<LayerConnection>, IExposable
	{
		// Token: 0x0600971B RID: 38683 RVA: 0x0036CEF6 File Offset: 0x0036B0F6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "tag", "fuelCost");
		}

		// Token: 0x0600971C RID: 38684 RVA: 0x0036CF0C File Offset: 0x0036B10C
		public bool Equals(LayerConnection other)
		{
			return other != null && (this == other || (this.tag == other.tag && this.fuelCost.Equals(other.fuelCost) && this.zoomMode == other.zoomMode));
		}

		// Token: 0x0600971D RID: 38685 RVA: 0x0036CF5A File Offset: 0x0036B15A
		public override bool Equals(object obj)
		{
			return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((LayerConnection)obj)));
		}

		// Token: 0x0600971E RID: 38686 RVA: 0x0036CF88 File Offset: 0x0036B188
		public override int GetHashCode()
		{
			return HashCode.Combine<string, float, LayerConnection.ZoomMode>(this.tag, this.fuelCost, this.zoomMode);
		}

		// Token: 0x0600971F RID: 38687 RVA: 0x0036CFA1 File Offset: 0x0036B1A1
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Values.Look<float>(ref this.fuelCost, "fuelCost", 0f, false);
			Scribe_Values.Look<LayerConnection.ZoomMode>(ref this.zoomMode, "zoomMode", LayerConnection.ZoomMode.None, false);
		}

		// Token: 0x04005A8B RID: 23179
		public string tag;

		// Token: 0x04005A8C RID: 23180
		public float fuelCost;

		// Token: 0x04005A8D RID: 23181
		public LayerConnection.ZoomMode zoomMode;

		// Token: 0x02003597 RID: 13719
		public enum ZoomMode : byte
		{
			// Token: 0x0400D626 RID: 54822
			None,
			// Token: 0x0400D627 RID: 54823
			ZoomIn,
			// Token: 0x0400D628 RID: 54824
			ZoomOut
		}
	}
}
