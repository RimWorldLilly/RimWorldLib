using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A5D RID: 6749
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x0600A977 RID: 43383 RVA: 0x003C16A0 File Offset: 0x003BF8A0
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x0600A978 RID: 43384 RVA: 0x003C16A8 File Offset: 0x003BF8A8
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x0600A979 RID: 43385 RVA: 0x003C16B1 File Offset: 0x003BF8B1
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x0600A97A RID: 43386 RVA: 0x003C16DC File Offset: 0x003BF8DC
		public override void Print(SectionLayer layer, Thing parent, float extraRotation)
		{
			foreach (IntVec3 cell in parent.OccupiedRect())
			{
				Vector3 center = cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay);
				Printer_Plane.PrintPlane(layer, center, new Vector2(1f, 1f), this.LinkedDrawMatFrom(parent, cell), extraRotation, false, null, null, 0.01f, 0f);
			}
		}
	}
}
