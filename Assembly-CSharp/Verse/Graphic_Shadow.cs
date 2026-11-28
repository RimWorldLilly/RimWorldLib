using System;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200057C RID: 1404
	public class Graphic_Shadow : Graphic
	{
		// Token: 0x06002993 RID: 10643 RVA: 0x000FEE5E File Offset: 0x000FD05E
		public Graphic_Shadow(ShadowData shadowInfo)
		{
			this.shadowInfo = shadowInfo;
			if (shadowInfo == null)
			{
				throw new ArgumentNullException("shadowInfo");
			}
			this.shadowMesh = ShadowMeshPool.GetShadowMesh(shadowInfo);
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000FEE88 File Offset: 0x000FD088
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (this.shadowMesh != null && thingDef != null && this.shadowInfo != null && (Find.CurrentMap == null || !Find.CurrentMap.Biome.disableShadows) && (Find.CurrentMap == null || !loc.ToIntVec3().InBounds(Find.CurrentMap) || !Find.CurrentMap.roofGrid.Roofed(loc.ToIntVec3())) && DebugViewSettings.drawShadows)
			{
				Vector3 position = loc + this.shadowInfo.offset;
				position.y = AltitudeLayer.Shadows.AltitudeFor();
				Graphics.DrawMesh(this.shadowMesh, position, rot.AsQuat, MatBases.SunShadowFade, 0);
			}
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000FEF40 File Offset: 0x000FD140
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector3 center = thing.TrueCenter() + (this.shadowInfo.offset + new Vector3(Graphic_Shadow.GlobalShadowPosOffsetX, 0f, Graphic_Shadow.GlobalShadowPosOffsetZ)).RotatedBy(thing.Rotation);
			center.y = AltitudeLayer.Shadows.AltitudeFor();
			Printer_Shadow.PrintShadow(layer, center, this.shadowInfo, thing.Rotation);
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000FEFA9 File Offset: 0x000FD1A9
		public override string ToString()
		{
			string str = "Graphic_Shadow(";
			ShadowData shadowData = this.shadowInfo;
			return str + ((shadowData != null) ? shadowData.ToString() : null) + ")";
		}

		// Token: 0x04001CE2 RID: 7394
		private Mesh shadowMesh;

		// Token: 0x04001CE3 RID: 7395
		private ShadowData shadowInfo;

		// Token: 0x04001CE4 RID: 7396
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetX;

		// Token: 0x04001CE5 RID: 7397
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetZ;
	}
}
