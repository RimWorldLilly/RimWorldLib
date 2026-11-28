using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064A RID: 1610
	public static class GhostUtility
	{
		// Token: 0x060032A0 RID: 12960 RVA: 0x00142F90 File Offset: 0x00141190
		public static Graphic GhostGraphicFor(Graphic baseGraphic, ThingDef thingDef, Color ghostCol, ThingDef stuff = null)
		{
			if (thingDef.useSameGraphicForGhost)
			{
				return baseGraphic;
			}
			int num = 0;
			num = Gen.HashCombine<Graphic>(num, baseGraphic);
			num = Gen.HashCombine<ThingDef>(num, thingDef);
			num = Gen.HashCombineStruct<Color>(num, ghostCol);
			num = Gen.HashCombine<ThingDef>(num, stuff);
			Graphic graphic;
			if (!GhostUtility.ghostGraphics.TryGetValue(num, out graphic))
			{
				if (thingDef.graphicData.Linked || (thingDef.IsDoor && !thingDef.building.isSupportDoor))
				{
					graphic = GraphicDatabase.Get<Graphic_Single>(thingDef.uiIconPath, ShaderTypeDefOf.EdgeDetect.Shader, thingDef.graphicData.drawSize, ghostCol);
				}
				else
				{
					if (thingDef.useBlueprintGraphicAsGhost)
					{
						baseGraphic = thingDef.blueprintDef.graphic;
					}
					else if (baseGraphic == null)
					{
						baseGraphic = thingDef.graphic;
					}
					GraphicData graphicData = null;
					if (baseGraphic.data != null)
					{
						graphicData = new GraphicData();
						graphicData.CopyFrom(baseGraphic.data);
						graphicData.shadowData = null;
					}
					string path = baseGraphic.path;
					Graphic_Appearances graphic_Appearances = baseGraphic as Graphic_Appearances;
					if (graphic_Appearances != null && stuff != null)
					{
						graphic = GraphicDatabase.Get<Graphic_Single>(graphic_Appearances.SubGraphicFor(stuff).path, ShaderTypeDefOf.EdgeDetect.Shader, thingDef.graphicData.drawSize, ghostCol, Color.white, graphicData, null);
					}
					else
					{
						graphic = GraphicDatabase.Get(baseGraphic.GetType(), path, ShaderTypeDefOf.EdgeDetect.Shader, baseGraphic.drawSize, ghostCol, Color.white, graphicData, null, null);
					}
				}
				GhostUtility.ghostGraphics.Add(num, graphic);
			}
			return graphic;
		}

		// Token: 0x0400205B RID: 8283
		private static Dictionary<int, Graphic> ghostGraphics = new Dictionary<int, Graphic>();
	}
}
