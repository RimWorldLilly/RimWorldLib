using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A5C RID: 6748
	public class Graphic_LinkedTransmitter : Graphic_Linked
	{
		// Token: 0x0600A974 RID: 43380 RVA: 0x003C15BF File Offset: 0x003BF7BF
		public Graphic_LinkedTransmitter(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x0600A975 RID: 43381 RVA: 0x003C15C8 File Offset: 0x003BF7C8
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && (base.ShouldLinkWith(c, parent) || parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null);
		}

		// Token: 0x0600A976 RID: 43382 RVA: 0x003C15FC File Offset: 0x003BF7FC
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			base.Print(layer, thing, extraRotation);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = thing.Position + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(thing.Map))
				{
					Building transmitter = intVec.GetTransmitter(thing.Map);
					if (transmitter != null && !transmitter.def.graphicData.Linked)
					{
						Material mat = this.LinkedDrawMatFrom(thing, intVec);
						Printer_Plane.PrintPlane(layer, intVec.ToVector3ShiftedWithAltitude(thing.def.Altitude), Vector2.one, mat, extraRotation, false, null, null, 0.01f, 0f);
					}
				}
			}
		}
	}
}
