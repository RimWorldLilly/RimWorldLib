using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204E RID: 8270
	[StaticConstructorOnStartup]
	public class PlaceWorker_FuelingPort : PlaceWorker
	{
		// Token: 0x0600D0FA RID: 53498 RVA: 0x004CD974 File Offset: 0x004CBB74
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			if (def.building == null || !def.building.hasFuelingPort)
			{
				return;
			}
			if (!FuelingPortUtility.GetFuelingPortCell(center, rot).Standable(currentMap))
			{
				return;
			}
			PlaceWorker_FuelingPort.DrawFuelingPortCell(center, rot);
		}

		// Token: 0x0600D0FB RID: 53499 RVA: 0x004CD9B4 File Offset: 0x004CBBB4
		public static void DrawFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			Vector3 position = FuelingPortUtility.GetFuelingPortCell(center, rot).ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, PlaceWorker_FuelingPort.FuelingPortCellMaterial, 0);
		}

		// Token: 0x040084D6 RID: 34006
		private static readonly Material FuelingPortCellMaterial = MaterialPool.MatFrom("UI/Overlays/FuelingPort", ShaderDatabase.Transparent);
	}
}
