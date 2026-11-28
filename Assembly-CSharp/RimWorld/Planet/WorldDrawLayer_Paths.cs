using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200231D RID: 8989
	public abstract class WorldDrawLayer_Paths : WorldDrawLayer
	{
		// Token: 0x170026E3 RID: 9955
		// (get) Token: 0x0600DDFF RID: 56831 RVA: 0x005295E5 File Offset: 0x005277E5
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026E4 RID: 9956
		// (get) Token: 0x0600DE00 RID: 56832 RVA: 0x005295E8 File Offset: 0x005277E8
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DE01 RID: 56833 RVA: 0x005295EC File Offset: 0x005277EC
		public void GeneratePaths(LayerSubMesh subMesh, PlanetTile tile, List<WorldDrawLayer_Paths.OutputDirection> nodes, Color32 color, bool allowSmoothTransition)
		{
			this.planetLayer.GetTileVertices(tile, this.tmpVerts);
			this.planetLayer.GetTileNeighbors(tile, this.tmpNeighbors);
			if (nodes.Count == 1 && this.pointyEnds)
			{
				int count = subMesh.verts.Count;
				this.AddPathEndpoint(subMesh, this.tmpVerts, this.tmpNeighbors.IndexOf(nodes[0].neighbor), color, tile, nodes[0]);
				subMesh.verts.Add(this.FinalizePoint(this.planetLayer.GetTileCenter(tile), nodes[0].distortionFrequency, nodes[0].distortionIntensity));
				subMesh.colors.Add(color.MutateAlpha(0));
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 2);
				return;
			}
			if (nodes.Count == 2)
			{
				int count2 = subMesh.verts.Count;
				int num = this.tmpNeighbors.IndexOf(nodes[0].neighbor);
				int num2 = this.tmpNeighbors.IndexOf(nodes[1].neighbor);
				if (allowSmoothTransition && Mathf.Abs(num - num2) > 1 && Mathf.Abs((num - num2 + this.tmpVerts.Count) % this.tmpVerts.Count) > 1)
				{
					this.AddPathEndpoint(subMesh, this.tmpVerts, num, color, tile, nodes[0]);
					this.AddPathEndpoint(subMesh, this.tmpVerts, num2, color, tile, nodes[1]);
					subMesh.tris.Add(count2);
					subMesh.tris.Add(count2 + 5);
					subMesh.tris.Add(count2 + 1);
					subMesh.tris.Add(count2 + 5);
					subMesh.tris.Add(count2 + 4);
					subMesh.tris.Add(count2 + 1);
					subMesh.tris.Add(count2 + 1);
					subMesh.tris.Add(count2 + 4);
					subMesh.tris.Add(count2 + 2);
					subMesh.tris.Add(count2 + 4);
					subMesh.tris.Add(count2 + 3);
					subMesh.tris.Add(count2 + 2);
					return;
				}
			}
			float num3 = 0f;
			for (int i = 0; i < nodes.Count; i++)
			{
				num3 = Mathf.Max(num3, nodes[i].width);
			}
			Vector3 vector = this.planetLayer.GetTileCenter(tile);
			this.tmpHexVerts.Clear();
			for (int j = 0; j < this.tmpVerts.Count; j++)
			{
				this.tmpHexVerts.Add(this.FinalizePoint(Vector3.LerpUnclamped(vector, this.tmpVerts[j], num3 * 0.5f * 2f), 0f, 0f));
			}
			vector = this.FinalizePoint(vector, 0f, 0f);
			int count3 = subMesh.verts.Count;
			subMesh.verts.Add(vector);
			subMesh.colors.Add(color);
			int count4 = subMesh.verts.Count;
			for (int k = 0; k < this.tmpHexVerts.Count; k++)
			{
				subMesh.verts.Add(this.tmpHexVerts[k]);
				subMesh.colors.Add(color.MutateAlpha(0));
				subMesh.tris.Add(count3);
				subMesh.tris.Add(count4 + (k + 1) % this.tmpHexVerts.Count);
				subMesh.tris.Add(count4 + k);
			}
			for (int l = 0; l < nodes.Count; l++)
			{
				if (nodes[l].width != 0f)
				{
					int count5 = subMesh.verts.Count;
					int num4 = this.tmpNeighbors.IndexOf(nodes[l].neighbor);
					this.AddPathEndpoint(subMesh, this.tmpVerts, num4, color, tile, nodes[l]);
					subMesh.tris.Add(count5);
					subMesh.tris.Add(count4 + (num4 + this.tmpHexVerts.Count - 1) % this.tmpHexVerts.Count);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count5);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count5 + 1);
					subMesh.tris.Add(count5 + 1);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count5 + 2);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count4 + (num4 + 2) % this.tmpHexVerts.Count);
					subMesh.tris.Add(count5 + 2);
				}
			}
		}

		// Token: 0x0600DE02 RID: 56834 RVA: 0x00529B1C File Offset: 0x00527D1C
		private void AddPathEndpoint(LayerSubMesh subMesh, List<Vector3> verts, int index, Color32 color, PlanetTile tileID, WorldDrawLayer_Paths.OutputDirection data)
		{
			int index2 = (index + 1) % verts.Count;
			Find.WorldGrid.GetTileNeighbors(tileID, WorldDrawLayer_Paths.lhsID);
			Find.WorldGrid.GetTileNeighbors(data.neighbor, WorldDrawLayer_Paths.rhsID);
			float num = WorldDrawLayer_Paths.lhsID.Intersect(WorldDrawLayer_Paths.rhsID).Any((PlanetTile id) => Find.WorldGrid[id].WaterCovered) ? 0.5f : 1f;
			Vector3 a = this.FinalizePoint(verts[index], data.distortionFrequency, data.distortionIntensity * num);
			Vector3 b = this.FinalizePoint(verts[index2], data.distortionFrequency, data.distortionIntensity * num);
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f - data.width));
			subMesh.colors.Add(color.MutateAlpha(0));
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f));
			subMesh.colors.Add(color);
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f + data.width));
			subMesh.colors.Add(color.MutateAlpha(0));
		}

		// Token: 0x0600DE03 RID: 56835
		public abstract Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity);

		// Token: 0x04008E85 RID: 36485
		protected bool pointyEnds;

		// Token: 0x04008E86 RID: 36486
		private readonly List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x04008E87 RID: 36487
		private readonly List<Vector3> tmpHexVerts = new List<Vector3>();

		// Token: 0x04008E88 RID: 36488
		private readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x04008E89 RID: 36489
		private static readonly List<PlanetTile> lhsID = new List<PlanetTile>();

		// Token: 0x04008E8A RID: 36490
		private static readonly List<PlanetTile> rhsID = new List<PlanetTile>();

		// Token: 0x02003DF8 RID: 15864
		public struct OutputDirection
		{
			// Token: 0x0400F479 RID: 62585
			public PlanetTile neighbor;

			// Token: 0x0400F47A RID: 62586
			public float width;

			// Token: 0x0400F47B RID: 62587
			public float distortionFrequency;

			// Token: 0x0400F47C RID: 62588
			public float distortionIntensity;
		}
	}
}
