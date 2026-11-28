using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAA RID: 6826
	public static class DoorsDebugDrawer
	{
		// Token: 0x0600ACA2 RID: 44194 RVA: 0x003CE1AC File Offset: 0x003CC3AC
		public static void DrawDebug()
		{
			if (!DebugViewSettings.drawDoorsDebug)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
			for (int i = 0; i < list.Count; i++)
			{
				if (currentViewRect.Contains(list[i].Position))
				{
					Building_Door building_Door = list[i] as Building_Door;
					if (building_Door != null)
					{
						Color col;
						if (building_Door.FreePassage)
						{
							col = new Color(0f, 1f, 0f, 0.5f);
						}
						else
						{
							col = new Color(1f, 0f, 0f, 0.5f);
						}
						foreach (IntVec3 c in building_Door.OccupiedRect())
						{
							CellRenderer.RenderCell(c, SolidColorMaterials.SimpleSolidColorMaterial(col, false));
						}
					}
				}
			}
		}
	}
}
