using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002326 RID: 8998
	public class WorldDrawLayer_Terrain : WorldDrawLayer
	{
		// Token: 0x0600DE3B RID: 56891 RVA: 0x0052A6C5 File Offset: 0x005288C5
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_Terrain.<Regenerate>d__4 <Regenerate>d__ = new WorldDrawLayer_Terrain.<Regenerate>d__4(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x0600DE3C RID: 56892 RVA: 0x0052A6D8 File Offset: 0x005288D8
		private static int GetLandmarkRotationIncrements(Tile tile, PlanetLayer layer, Landmark landmark, NativeArray<int> tileIDToVerts_offsets, NativeArray<Vector3> verts)
		{
			int oneAfterLastVertIndex = WorldDrawLayer_Terrain.GetOneAfterLastVertIndex(tile.tile.tileId, tileIDToVerts_offsets, verts);
			int num = tileIDToVerts_offsets[tile.tile.tileId];
			int num2 = 0;
			layer.GetTileNeighbors(tile.tile, WorldDrawLayer_Terrain.neighbours);
			PlanetTile planetTile = PlanetTile.Invalid;
			for (int i = 0; i < WorldDrawLayer_Terrain.neighbours.Count; i++)
			{
				if (LandmarkDef.IsValidRotatableNeighbour(layer[WorldDrawLayer_Terrain.neighbours[i]].PrimaryBiome, landmark.def.coastRotateMode))
				{
					planetTile = WorldDrawLayer_Terrain.neighbours[i];
					break;
				}
			}
			if (planetTile.Valid)
			{
				int num3 = tileIDToVerts_offsets[planetTile.tileId];
				int oneAfterLastVertIndex2 = WorldDrawLayer_Terrain.GetOneAfterLastVertIndex(planetTile.tileId, tileIDToVerts_offsets, verts);
				bool flag = false;
				for (int j = num; j < oneAfterLastVertIndex; j++)
				{
					int index = WorldDrawLayer_Terrain.<GetLandmarkRotationIncrements>g__WrapIndex|5_0(num, oneAfterLastVertIndex, j + 1);
					int index2 = WorldDrawLayer_Terrain.<GetLandmarkRotationIncrements>g__WrapIndex|5_0(num, oneAfterLastVertIndex, j + 2);
					for (int k = num3; k < oneAfterLastVertIndex2; k++)
					{
						int index3 = WorldDrawLayer_Terrain.<GetLandmarkRotationIncrements>g__WrapIndex|5_0(num3, oneAfterLastVertIndex2, k);
						int index4 = WorldDrawLayer_Terrain.<GetLandmarkRotationIncrements>g__WrapIndex|5_0(num3, oneAfterLastVertIndex2, k + 1);
						if ((!(verts[index] != verts[index3]) || !(verts[index2] != verts[index3])) && (!(verts[index] != verts[index4]) || !(verts[index2] != verts[index4])))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
					num2++;
				}
				if (!flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(88, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to get increments for rotation of landmark with a valid neighbouring tile. Tile: ");
					defaultInterpolatedStringHandler.AppendFormatted<Tile>(tile);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(113, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Failed to get non background biome near tile with a landmark which has to rotate away from the background. Tile: ");
				defaultInterpolatedStringHandler2.AppendFormatted<Tile>(tile);
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			return num2;
		}

		// Token: 0x0600DE3D RID: 56893 RVA: 0x0052A8D4 File Offset: 0x00528AD4
		private static int GetOneAfterLastVertIndex(int index, NativeArray<int> tileIDToVerts_offsets, NativeArray<Vector3> verts)
		{
			if (index + 1 >= tileIDToVerts_offsets.Length)
			{
				return verts.Length;
			}
			return tileIDToVerts_offsets[index + 1];
		}

		// Token: 0x0600DE3E RID: 56894 RVA: 0x0052A8F4 File Offset: 0x00528AF4
		private IEnumerable CalculateInterpolatedVerticesParams()
		{
			WorldDrawLayer_Terrain.<CalculateInterpolatedVerticesParams>d__7 <CalculateInterpolatedVerticesParams>d__ = new WorldDrawLayer_Terrain.<CalculateInterpolatedVerticesParams>d__7(-2);
			<CalculateInterpolatedVerticesParams>d__.<>4__this = this;
			return <CalculateInterpolatedVerticesParams>d__;
		}

		// Token: 0x0600DE42 RID: 56898 RVA: 0x0052A92C File Offset: 0x00528B2C
		[CompilerGenerated]
		internal static int <GetLandmarkRotationIncrements>g__WrapIndex|5_0(int start, int oneAfterLast, int index)
		{
			if (index >= oneAfterLast)
			{
				return start + index % oneAfterLast;
			}
			return index;
		}

		// Token: 0x04008EA9 RID: 36521
		private readonly List<Vector3> elevationValues = new List<Vector3>();

		// Token: 0x04008EAA RID: 36522
		private static bool setGlobalMacro;

		// Token: 0x04008EAB RID: 36523
		private static readonly List<PlanetTile> neighbours = new List<PlanetTile>(8);

		// Token: 0x04008EAC RID: 36524
		private const string MacroTexturePath = "World/WorldMacro";
	}
}
