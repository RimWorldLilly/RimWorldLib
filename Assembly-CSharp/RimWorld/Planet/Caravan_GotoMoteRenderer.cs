using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002396 RID: 9110
	[StaticConstructorOnStartup]
	public class Caravan_GotoMoteRenderer
	{
		// Token: 0x0600E299 RID: 58009 RVA: 0x00542460 File Offset: 0x00540660
		public void RenderMote()
		{
			float num = (Time.time - this.lastOrderedToTileTime) / 0.5f;
			if (num > 1f)
			{
				return;
			}
			if (Caravan_GotoMoteRenderer.cachedMaterial == null)
			{
				Caravan_GotoMoteRenderer.cachedMaterial = MaterialPool.MatFrom((Texture2D)Caravan_GotoMoteRenderer.FeedbackGoto.mainTexture, Caravan_GotoMoteRenderer.FeedbackGoto.shader, Color.white, 3600);
			}
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(this.tile);
			Color value = new Color(1f, 1f, 1f, 1f - num);
			Caravan_GotoMoteRenderer.propertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			WorldRendererUtility.DrawQuadTangentialToPlanet(tileCenter, 0.8f * worldGrid.AverageTileSize, 0.05f, Caravan_GotoMoteRenderer.cachedMaterial, 0f, false, false, Caravan_GotoMoteRenderer.propertyBlock);
		}

		// Token: 0x0600E29A RID: 58010 RVA: 0x00542529 File Offset: 0x00540729
		public void OrderedToTile(PlanetTile tile)
		{
			this.tile = tile;
			this.lastOrderedToTileTime = Time.time;
		}

		// Token: 0x04009097 RID: 37015
		private PlanetTile tile;

		// Token: 0x04009098 RID: 37016
		private float lastOrderedToTileTime = -0.51f;

		// Token: 0x04009099 RID: 37017
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400909A RID: 37018
		private static Material cachedMaterial;

		// Token: 0x0400909B RID: 37019
		public static readonly Material FeedbackGoto = MaterialPool.MatFrom("Things/Mote/FeedbackGoto", ShaderDatabase.WorldOverlayTransparent, 3600);

		// Token: 0x0400909C RID: 37020
		private const float Duration = 0.5f;

		// Token: 0x0400909D RID: 37021
		private const float BaseSize = 0.8f;
	}
}
