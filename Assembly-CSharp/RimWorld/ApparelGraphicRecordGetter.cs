using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002079 RID: 8313
	public static class ApparelGraphicRecordGetter
	{
		// Token: 0x0600D177 RID: 53623 RVA: 0x004D00D0 File Offset: 0x004CE2D0
		public static bool TryGetGraphicApparel(Apparel apparel, BodyTypeDef bodyType, bool forStatue, out ApparelGraphicRecord rec)
		{
			if (bodyType == null)
			{
				Log.Error("Getting apparel graphic with undefined body type.");
				bodyType = BodyTypeDefOf.Male;
			}
			if (apparel.WornGraphicPath.NullOrEmpty())
			{
				rec = new ApparelGraphicRecord(null, null);
				return false;
			}
			string path;
			if (apparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || apparel.def.apparel.LastLayer == ApparelLayerDefOf.EyeCover || apparel.RenderAsPack() || apparel.WornGraphicPath == BaseContent.PlaceholderImagePath || apparel.WornGraphicPath == BaseContent.PlaceholderGearImagePath)
			{
				path = apparel.WornGraphicPath;
			}
			else
			{
				path = apparel.WornGraphicPath + "_" + bodyType.defName;
			}
			Shader shader = ShaderDatabase.Cutout;
			if (!forStatue)
			{
				ThingStyleDef styleDef = apparel.StyleDef;
				if (((styleDef != null) ? styleDef.graphicData.shaderType : null) != null)
				{
					shader = apparel.StyleDef.graphicData.shaderType.Shader;
				}
				else if ((apparel.StyleDef == null && apparel.def.apparel.useWornGraphicMask) || (apparel.StyleDef != null && apparel.StyleDef.UseWornGraphicMask))
				{
					shader = ShaderDatabase.CutoutComplex;
				}
			}
			Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(path, shader, apparel.def.graphicData.drawSize, apparel.DrawColor);
			rec = new ApparelGraphicRecord(graphic, apparel);
			return true;
		}
	}
}
