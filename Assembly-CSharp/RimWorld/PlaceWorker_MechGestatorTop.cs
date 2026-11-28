using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002068 RID: 8296
	public class PlaceWorker_MechGestatorTop : PlaceWorker
	{
		// Token: 0x0600D147 RID: 53575 RVA: 0x004CF1F8 File Offset: 0x004CD3F8
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(GraphicDatabase.Get<Graphic_Multi>(def.building.mechGestatorCylinderGraphic.texPath, ShaderDatabase.Cutout, def.building.mechGestatorCylinderGraphic.drawSize, Color.white), def, ghostCol, null).DrawFromDef(GenThing.TrueCenter(loc, rot, def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), rot, def, 0f);
			GhostUtility.GhostGraphicFor(GraphicDatabase.Get<Graphic_Multi>(def.building.mechGestatorTopGraphic.texPath, ShaderDatabase.Cutout, def.building.mechGestatorTopGraphic.drawSize, Color.white), def, ghostCol, null).DrawFromDef(GenThing.TrueCenter(loc, rot, def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), rot, def, 0f);
		}
	}
}
