using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002051 RID: 8273
	public class PlaceWorker_TurretTop : PlaceWorker
	{
		// Token: 0x0600D107 RID: 53511 RVA: 0x004CDE0C File Offset: 0x004CC00C
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(GraphicDatabase.Get<Graphic_Single>(def.building.turretGunDef.graphicData.texPath, ShaderDatabase.Cutout, new Vector2(def.building.turretTopDrawSize, def.building.turretTopDrawSize), Color.white), def, ghostCol, null).DrawFromDef(GenThing.TrueCenter(loc, rot, def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), rot, def, (float)TurretTop.ArtworkRotation);
		}
	}
}
