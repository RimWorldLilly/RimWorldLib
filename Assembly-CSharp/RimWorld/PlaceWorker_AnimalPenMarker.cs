using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200205D RID: 8285
	public class PlaceWorker_AnimalPenMarker : PlaceWorker
	{
		// Token: 0x0600D127 RID: 53543 RVA: 0x004CE944 File Offset: 0x004CCB44
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (thing == null)
			{
				PlaceWorker_AnimalPenMarker.tmpPlacingBlueprintPainter.Paint(center, Find.CurrentMap);
				return;
			}
			if (thing is Blueprint || thing is Frame)
			{
				PlaceWorker_AnimalPenMarker.tmpPlacedBlueprintPainter.Paint(center, Find.CurrentMap);
				return;
			}
			PlaceWorker_AnimalPenMarker.tmpPainter.Paint(center, Find.CurrentMap);
		}

		// Token: 0x0600D128 RID: 53544 RVA: 0x004CE999 File Offset: 0x004CCB99
		public override void DrawMouseAttachments(BuildableDef def)
		{
			Find.CurrentMap.animalPenManager.DrawPlacingMouseAttachments(def);
		}

		// Token: 0x040084DB RID: 34011
		private static readonly AnimalPenGUI.PenPainter tmpPainter = new AnimalPenGUI.PenPainter();

		// Token: 0x040084DC RID: 34012
		private static readonly AnimalPenGUI.PenBlueprintPainter tmpPlacingBlueprintPainter = new AnimalPenGUI.PenBlueprintPainter();

		// Token: 0x040084DD RID: 34013
		private static readonly AnimalPenGUI.PenBlueprintPainter tmpPlacedBlueprintPainter = new AnimalPenGUI.PenBlueprintPainter();
	}
}
