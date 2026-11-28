using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200231B RID: 8987
	public class WorldDrawLayer_Landmarks : WorldDrawLayer
	{
		// Token: 0x170026DC RID: 9948
		// (get) Token: 0x0600DDF2 RID: 56818 RVA: 0x005293C9 File Offset: 0x005275C9
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026DD RID: 9949
		// (get) Token: 0x0600DDF3 RID: 56819 RVA: 0x005293CC File Offset: 0x005275CC
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026DE RID: 9950
		// (get) Token: 0x0600DDF4 RID: 56820 RVA: 0x005293CF File Offset: 0x005275CF
		public override bool Visible
		{
			get
			{
				return ModsConfig.OdysseyActive;
			}
		}

		// Token: 0x0600DDF5 RID: 56821 RVA: 0x005293D6 File Offset: 0x005275D6
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_Landmarks.<Regenerate>d__7 <Regenerate>d__ = new WorldDrawLayer_Landmarks.<Regenerate>d__7(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x0600DDF6 RID: 56822 RVA: 0x005293E8 File Offset: 0x005275E8
		private void DrawStandard(Landmark landmark, PlanetTile tile)
		{
			LayerSubMesh subMesh = base.GetSubMesh(landmark.def.Material);
			Vector3 tileCenter = this.planetLayer.GetTileCenter(tile);
			float rotation = Rand.Range(0f, 360f);
			if (landmark.def.coastRotateMode != LandmarkDef.CoastRotateMode.None)
			{
				this.planetLayer.GetTileNeighbors(tile, WorldDrawLayer_Landmarks.neighbours);
				PlanetTile toTile = PlanetTile.Invalid;
				for (int i = 0; i < WorldDrawLayer_Landmarks.neighbours.Count; i++)
				{
					if (LandmarkDef.IsValidRotatableNeighbour(this.planetLayer[WorldDrawLayer_Landmarks.neighbours[i]].PrimaryBiome, landmark.def.coastRotateMode))
					{
						toTile = WorldDrawLayer_Landmarks.neighbours[i];
						break;
					}
				}
				if (toTile.Valid)
				{
					rotation = this.planetLayer.GetHeadingFromTo(tile, toTile) - 90f;
				}
			}
			Rand.PushState(tile.tileId);
			WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, this.planetLayer.GetTileSize(tile) * landmark.def.drawScale, landmark.def.drawAboveRoads ? 0.025f : 0.0075f, subMesh, false, rotation, false);
			WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, landmark.def.atlasSize.x), Rand.Range(0, landmark.def.atlasSize.z), landmark.def.atlasSize.x, landmark.def.atlasSize.z, subMesh);
			Rand.PopState();
		}

		// Token: 0x04008E84 RID: 36484
		private static readonly List<PlanetTile> neighbours = new List<PlanetTile>(8);
	}
}
