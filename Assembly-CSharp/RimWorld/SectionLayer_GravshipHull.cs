using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128C RID: 4748
	public class SectionLayer_GravshipHull : SectionLayer
	{
		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x060075AF RID: 30127 RVA: 0x00295380 File Offset: 0x00293580
		private static Shader WallShader
		{
			get
			{
				ShaderTypeDef shaderTypeDef = SectionLayer_GravshipHull.wallShader;
				return ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.CutoutOverlay;
			}
		}

		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x060075B0 RID: 30128 RVA: 0x0029539C File Offset: 0x0029359C
		private static Shader SubstructureShader
		{
			get
			{
				ShaderTypeDef shaderTypeDef = SectionLayer_GravshipHull.substructureShader;
				return ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.Transparent;
			}
		}

		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x060075B1 RID: 30129 RVA: 0x002953B8 File Offset: 0x002935B8
		public override bool Visible
		{
			get
			{
				return ModsConfig.OdysseyActive;
			}
		}

		// Token: 0x060075B2 RID: 30130 RVA: 0x002953BF File Offset: 0x002935BF
		public SectionLayer_GravshipHull(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Terrain | MapMeshFlagDefOf.Things | MapMeshFlagDefOf.Roofs);
		}

		// Token: 0x060075B3 RID: 30131 RVA: 0x002953FC File Offset: 0x002935FC
		public static List<LayerSubMesh> BakeGravshipIndoorMesh(Map map, CellRect bounds, Vector3 center)
		{
			Dictionary<SectionLayer_GravshipHull.CornerType, LayerSubMesh> dictionary = new Dictionary<SectionLayer_GravshipHull.CornerType, LayerSubMesh>();
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 intVec in bounds)
			{
				SectionLayer_GravshipHull.CornerType cornerType;
				Color color;
				if (SectionLayer_GravshipHull.ShouldDrawCornerPiece(intVec, map, terrainGrid, out cornerType, out color) && SectionLayer_GravshipHull.IsCornerSubstructure(intVec, cornerType) && SectionLayer_GravshipHull.IsCornerIndoorMasked(intVec, cornerType, map))
				{
					Material material = SectionLayer_GravshipHull.GetMaterial(cornerType).Material;
					Texture2D srcTex = material.mainTexture as Texture2D;
					Color color2 = material.color;
					Material material2 = MaterialPool.MatFrom(srcTex, ShaderDatabase.IndoorMaskMasked, color2, 3185);
					IntVec3 offset = SectionLayer_GravshipHull.GetOffset(cornerType);
					LayerSubMesh sm;
					if (!dictionary.TryGetValue(cornerType, out sm))
					{
						dictionary.Add(cornerType, sm = MapDrawLayer.CreateFreeSubMesh(material2, map));
					}
					SectionLayer_GravshipHull.AddQuad(sm, (intVec + offset).ToVector3() - center, SectionLayer_GravshipHull.HullCornerScale, SectionLayer_GravshipHull.bakedAltitude, color);
				}
			}
			foreach (LayerSubMesh layerSubMesh in dictionary.Values)
			{
				layerSubMesh.FinalizeMesh(MeshParts.All);
			}
			return dictionary.Values.ToList<LayerSubMesh>();
		}

		// Token: 0x060075B4 RID: 30132 RVA: 0x00295558 File Offset: 0x00293758
		public override void Regenerate()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.ClearSubMeshes(MeshParts.All);
			Map map = base.Map;
			TerrainGrid terrainGrid = map.terrainGrid;
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				SectionLayer_GravshipHull.CornerType cornerType;
				Color color;
				if (SectionLayer_GravshipHull.ShouldDrawCornerPiece(intVec, map, terrainGrid, out cornerType, out color))
				{
					CachedMaterial material = SectionLayer_GravshipHull.GetMaterial(cornerType);
					IntVec3 offset = SectionLayer_GravshipHull.GetOffset(cornerType);
					bool addGravshipMask = SectionLayer_GravshipHull.IsCornerSubstructure(intVec, cornerType);
					bool addIndoorMask = SectionLayer_GravshipHull.IsCornerIndoorMasked(intVec, cornerType, map);
					this.AddQuad(material.Material, intVec + offset, SectionLayer_GravshipHull.HullCornerScale, SectionLayer_GravshipHull.cornerAltitude, color, addGravshipMask, addIndoorMask);
					TerrainDef terrainDef = terrainGrid.FoundationAt(intVec + IntVec3.South);
					bool substructureToSouth = terrainDef != null && terrainDef.IsSubstructure;
					this.AddSubstructure(cornerType, intVec, substructureToSouth, addGravshipMask, addIndoorMask);
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x060075B5 RID: 30133 RVA: 0x00295670 File Offset: 0x00293870
		private static void EnsureInitialized()
		{
			if (SectionLayer_GravshipHull.initalized)
			{
				return;
			}
			SectionLayer_GravshipHull.initalized = true;
			SectionLayer_GravshipHull.mat_Corner_NW = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_northwest", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Corner_NE = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_northeast", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Corner_SW = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_southwest", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Corner_SE = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_southeast", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Diagonal_NW = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_northwest", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Diagonal_NE = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_northeast", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Diagonal_SW = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_southwest", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_Diagonal_SE = new CachedMaterial("Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_southeast", SectionLayer_GravshipHull.WallShader);
			SectionLayer_GravshipHull.mat_SubStructure_W = new CachedMaterial("Things/Building/Linked/GravshipHull/SubstructureCorner_Full_west", SectionLayer_GravshipHull.SubstructureShader);
			SectionLayer_GravshipHull.mat_SubStructure_E = new CachedMaterial("Things/Building/Linked/GravshipHull/SubstructureCorner_Full_east", SectionLayer_GravshipHull.SubstructureShader);
			SectionLayer_GravshipHull.mat_SubStructureExtra_W = new CachedMaterial("Things/Building/Linked/GravshipHull/SubstructureCorner_Tip_west", SectionLayer_GravshipHull.SubstructureShader);
			SectionLayer_GravshipHull.mat_SubStructureExtra_E = new CachedMaterial("Things/Building/Linked/GravshipHull/SubstructureCorner_Tip_east", SectionLayer_GravshipHull.SubstructureShader);
		}

		// Token: 0x060075B6 RID: 30134 RVA: 0x0029577B File Offset: 0x0029397B
		private static bool IsIndoorMasked(IntVec3 c, Map map)
		{
			return c.Roofed(map);
		}

		// Token: 0x060075B7 RID: 30135 RVA: 0x00295784 File Offset: 0x00293984
		private static bool IsCornerSubstructure(IntVec3 c, SectionLayer_GravshipHull.CornerType cornerType)
		{
			switch (cornerType)
			{
			case SectionLayer_GravshipHull.CornerType.Corner_NW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NW:
				return SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.North) || SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.West);
			case SectionLayer_GravshipHull.CornerType.Corner_NE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NE:
				return SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.North) || SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.East);
			case SectionLayer_GravshipHull.CornerType.Corner_SW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SW:
				return SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.South) || SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.West);
			case SectionLayer_GravshipHull.CornerType.Corner_SE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SE:
				return SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.South) || SectionLayer_GravshipMask.IsValidSubstructure(c + IntVec3.East);
			default:
				return false;
			}
		}

		// Token: 0x060075B8 RID: 30136 RVA: 0x00295854 File Offset: 0x00293A54
		private static bool IsCornerIndoorMasked(IntVec3 c, SectionLayer_GravshipHull.CornerType cornerType, Map map)
		{
			switch (cornerType)
			{
			case SectionLayer_GravshipHull.CornerType.Corner_NW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NW:
				return SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.North, map) || SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.West, map);
			case SectionLayer_GravshipHull.CornerType.Corner_NE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NE:
				return SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.North, map) || SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.East, map);
			case SectionLayer_GravshipHull.CornerType.Corner_SW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SW:
				return SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.South, map) || SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.West, map);
			case SectionLayer_GravshipHull.CornerType.Corner_SE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SE:
				return SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.South, map) || SectionLayer_GravshipHull.IsIndoorMasked(c + IntVec3.East, map);
			default:
				return false;
			}
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x0029592C File Offset: 0x00293B2C
		private static CachedMaterial GetMaterial(SectionLayer_GravshipHull.CornerType edgeType)
		{
			SectionLayer_GravshipHull.EnsureInitialized();
			CachedMaterial result;
			switch (edgeType)
			{
			case SectionLayer_GravshipHull.CornerType.Corner_NW:
				result = SectionLayer_GravshipHull.mat_Corner_NW;
				break;
			case SectionLayer_GravshipHull.CornerType.Corner_NE:
				result = SectionLayer_GravshipHull.mat_Corner_NE;
				break;
			case SectionLayer_GravshipHull.CornerType.Corner_SW:
				result = SectionLayer_GravshipHull.mat_Corner_SW;
				break;
			case SectionLayer_GravshipHull.CornerType.Corner_SE:
				result = SectionLayer_GravshipHull.mat_Corner_SE;
				break;
			case SectionLayer_GravshipHull.CornerType.Diagonal_NW:
				result = SectionLayer_GravshipHull.mat_Diagonal_NW;
				break;
			case SectionLayer_GravshipHull.CornerType.Diagonal_NE:
				result = SectionLayer_GravshipHull.mat_Diagonal_NE;
				break;
			case SectionLayer_GravshipHull.CornerType.Diagonal_SW:
				result = SectionLayer_GravshipHull.mat_Diagonal_SW;
				break;
			case SectionLayer_GravshipHull.CornerType.Diagonal_SE:
				result = SectionLayer_GravshipHull.mat_Diagonal_SE;
				break;
			default:
				throw new ArgumentOutOfRangeException("edgeType", edgeType, null);
			}
			return result;
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x002959BC File Offset: 0x00293BBC
		private static IntVec3 GetOffset(SectionLayer_GravshipHull.CornerType cornerType)
		{
			switch (cornerType)
			{
			case SectionLayer_GravshipHull.CornerType.Corner_NW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NW:
				return new IntVec3(-1, 0, 0);
			case SectionLayer_GravshipHull.CornerType.Corner_NE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_NE:
				return new IntVec3(0, 0, 0);
			case SectionLayer_GravshipHull.CornerType.Corner_SW:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SW:
				return new IntVec3(-1, 0, -1);
			case SectionLayer_GravshipHull.CornerType.Corner_SE:
			case SectionLayer_GravshipHull.CornerType.Diagonal_SE:
				return new IntVec3(0, 0, -1);
			default:
				return IntVec3.Zero;
			}
		}

		// Token: 0x060075BB RID: 30139 RVA: 0x00295A1C File Offset: 0x00293C1C
		private static void AddQuad(LayerSubMesh sm, Vector3 c, float scale, float altitude, Color color)
		{
			int count = sm.verts.Count;
			for (int i = 0; i < 4; i++)
			{
				sm.verts.Add(new Vector3(c.x + SectionLayer_GravshipHull.UVs[i].x * scale, altitude, c.z + SectionLayer_GravshipHull.UVs[i].y * scale));
				sm.uvs.Add(SectionLayer_GravshipHull.UVs[i % 4]);
				sm.colors.Add(color);
			}
			sm.tris.Add(count);
			sm.tris.Add(count + 1);
			sm.tris.Add(count + 2);
			sm.tris.Add(count);
			sm.tris.Add(count + 2);
			sm.tris.Add(count + 3);
		}

		// Token: 0x060075BC RID: 30140 RVA: 0x00295B04 File Offset: 0x00293D04
		private void AddQuad(Material mat, IntVec3 c, float scale, float altitude, Color color, bool addGravshipMask, bool addIndoorMask)
		{
			LayerSubMesh subMesh = base.GetSubMesh(mat);
			SectionLayer_GravshipHull.AddQuad(subMesh, c.ToVector3(), scale, altitude, color);
			if (addGravshipMask)
			{
				Texture2D srcTex = subMesh.material.mainTexture as Texture2D;
				Color color2 = subMesh.material.color;
				Material material = MaterialPool.MatFrom(srcTex, ShaderDatabase.GravshipMaskMasked, color2);
				SectionLayer_GravshipHull.AddQuad(base.GetSubMesh(material), c.ToVector3(), scale, altitude, color);
			}
			if (addIndoorMask)
			{
				Texture2D srcTex2 = subMesh.material.mainTexture as Texture2D;
				Color color3 = subMesh.material.color;
				Material material2 = MaterialPool.MatFrom(srcTex2, ShaderDatabase.IndoorMaskMasked, color3);
				SectionLayer_GravshipHull.AddQuad(base.GetSubMesh(material2), c.ToVector3(), scale, altitude, color);
			}
		}

		// Token: 0x060075BD RID: 30141 RVA: 0x00295BC0 File Offset: 0x00293DC0
		private void AddSubstructure(SectionLayer_GravshipHull.CornerType cornerType, IntVec3 c, bool substructureToSouth, bool addGravshipMask, bool addIndoorMask)
		{
			if (cornerType == SectionLayer_GravshipHull.CornerType.Corner_NW || cornerType == SectionLayer_GravshipHull.CornerType.Diagonal_NW)
			{
				this.AddQuad(SectionLayer_GravshipHull.mat_SubStructure_W.Material, c, 1f, SectionLayer_GravshipHull.substructureAltitude, Color.white, addGravshipMask, addIndoorMask);
				if (!substructureToSouth)
				{
					this.AddQuad(SectionLayer_GravshipHull.mat_SubStructureExtra_W.Material, c + IntVec3.South, 1f, SectionLayer_GravshipHull.substructureAltitude, Color.white, addGravshipMask, addIndoorMask);
				}
			}
			if (cornerType == SectionLayer_GravshipHull.CornerType.Corner_NE || cornerType == SectionLayer_GravshipHull.CornerType.Diagonal_NE)
			{
				this.AddQuad(SectionLayer_GravshipHull.mat_SubStructure_E.Material, c, 1f, SectionLayer_GravshipHull.substructureAltitude, Color.white, addGravshipMask, addIndoorMask);
				if (!substructureToSouth)
				{
					this.AddQuad(SectionLayer_GravshipHull.mat_SubStructureExtra_E.Material, c + IntVec3.South, 1f, SectionLayer_GravshipHull.substructureAltitude, Color.white, addGravshipMask, addIndoorMask);
				}
			}
		}

		// Token: 0x060075BE RID: 30142 RVA: 0x00295C88 File Offset: 0x00293E88
		public static bool ShouldDrawCornerPiece(IntVec3 pos, Map map, TerrainGrid terrGrid, out SectionLayer_GravshipHull.CornerType cornerType, out Color color)
		{
			cornerType = SectionLayer_GravshipHull.CornerType.None;
			color = Color.white;
			if (pos.GetEdifice(map) != null)
			{
				return false;
			}
			TerrainDef terrainDef = terrGrid.FoundationAt(pos);
			if (terrainDef != null && terrainDef.IsSubstructure)
			{
				return false;
			}
			for (int k = 0; k < SectionLayer_GravshipHull.Directions.Length; k++)
			{
				bool[] array = SectionLayer_GravshipHull.tmpChecks;
				int num = k;
				Building edificeSafe = (pos + SectionLayer_GravshipHull.Directions[k]).GetEdificeSafe(map);
				array[num] = (((edificeSafe != null) ? edificeSafe.def : null) == ThingDefOf.GravshipHull);
			}
			if (SectionLayer_GravshipHull.tmpChecks[0] && SectionLayer_GravshipHull.tmpChecks[3] && !SectionLayer_GravshipHull.tmpChecks[2] && !SectionLayer_GravshipHull.tmpChecks[1])
			{
				cornerType = (SectionLayer_GravshipHull.tmpChecks[4] ? SectionLayer_GravshipHull.CornerType.Corner_NW : SectionLayer_GravshipHull.CornerType.Diagonal_NW);
			}
			else if (SectionLayer_GravshipHull.tmpChecks[0] && SectionLayer_GravshipHull.tmpChecks[1] && !SectionLayer_GravshipHull.tmpChecks[2] && !SectionLayer_GravshipHull.tmpChecks[3])
			{
				cornerType = (SectionLayer_GravshipHull.tmpChecks[5] ? SectionLayer_GravshipHull.CornerType.Corner_NE : SectionLayer_GravshipHull.CornerType.Diagonal_NE);
			}
			else if (SectionLayer_GravshipHull.tmpChecks[2] && SectionLayer_GravshipHull.tmpChecks[1] && !SectionLayer_GravshipHull.tmpChecks[0] && !SectionLayer_GravshipHull.tmpChecks[3])
			{
				cornerType = (SectionLayer_GravshipHull.tmpChecks[6] ? SectionLayer_GravshipHull.CornerType.Corner_SE : SectionLayer_GravshipHull.CornerType.Diagonal_SE);
			}
			else if (SectionLayer_GravshipHull.tmpChecks[2] && SectionLayer_GravshipHull.tmpChecks[3] && !SectionLayer_GravshipHull.tmpChecks[0] && !SectionLayer_GravshipHull.tmpChecks[1])
			{
				cornerType = (SectionLayer_GravshipHull.tmpChecks[7] ? SectionLayer_GravshipHull.CornerType.Corner_SW : SectionLayer_GravshipHull.CornerType.Diagonal_SW);
			}
			if (cornerType == SectionLayer_GravshipHull.CornerType.None)
			{
				return false;
			}
			int[][] array2 = SectionLayer_GravshipHull.directionPairs;
			for (int j = 0; j < array2.Length; j++)
			{
				List<int> list = (from i in array2[j]
				where SectionLayer_GravshipHull.tmpChecks[i]
				select i).ToList<int>();
				if (list.Count > 0)
				{
					int num2 = list.First<int>();
					color = (pos + SectionLayer_GravshipHull.Directions[num2]).GetEdificeSafe(map).DrawColor;
					break;
				}
			}
			return true;
		}

		// Token: 0x040049BA RID: 18874
		private static readonly Vector2[] UVs = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x040049BB RID: 18875
		[TweakValue("HullCorners", 0f, 2f)]
		private static float HullCornerScale = 2f;

		// Token: 0x040049BC RID: 18876
		private const string TexPath_Corner_NW = "Things/Building/Linked/GravshipHull/AngledGravshipHull_northwest";

		// Token: 0x040049BD RID: 18877
		private const string TexPath_Corner_NE = "Things/Building/Linked/GravshipHull/AngledGravshipHull_northeast";

		// Token: 0x040049BE RID: 18878
		private const string TexPath_Corner_SW = "Things/Building/Linked/GravshipHull/AngledGravshipHull_southwest";

		// Token: 0x040049BF RID: 18879
		private const string TexPath_Corner_SE = "Things/Building/Linked/GravshipHull/AngledGravshipHull_southeast";

		// Token: 0x040049C0 RID: 18880
		private const string TexPath_Diagonal_NW = "Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_northwest";

		// Token: 0x040049C1 RID: 18881
		private const string TexPath_Diagonal_NE = "Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_northeast";

		// Token: 0x040049C2 RID: 18882
		private const string TexPath_Diagonal_SW = "Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_southwest";

		// Token: 0x040049C3 RID: 18883
		private const string TexPath_Diagonal_SE = "Things/Building/Linked/GravshipHull/AngledGravshipHull_Partial_southeast";

		// Token: 0x040049C4 RID: 18884
		private const string TexPath_SubStructure_W = "Things/Building/Linked/GravshipHull/SubstructureCorner_Full_west";

		// Token: 0x040049C5 RID: 18885
		private const string TexPath_SubStructure_E = "Things/Building/Linked/GravshipHull/SubstructureCorner_Full_east";

		// Token: 0x040049C6 RID: 18886
		private const string TexPath_SubStructureExtra_W = "Things/Building/Linked/GravshipHull/SubstructureCorner_Tip_west";

		// Token: 0x040049C7 RID: 18887
		private const string TexPath_SubStructureExtra_E = "Things/Building/Linked/GravshipHull/SubstructureCorner_Tip_east";

		// Token: 0x040049C8 RID: 18888
		private const int BakedIndoorMaskRenderQueue = 3185;

		// Token: 0x040049C9 RID: 18889
		private static CachedMaterial mat_Corner_NW;

		// Token: 0x040049CA RID: 18890
		private static CachedMaterial mat_Corner_NE;

		// Token: 0x040049CB RID: 18891
		private static CachedMaterial mat_Corner_SW;

		// Token: 0x040049CC RID: 18892
		private static CachedMaterial mat_Corner_SE;

		// Token: 0x040049CD RID: 18893
		private static CachedMaterial mat_Diagonal_NW;

		// Token: 0x040049CE RID: 18894
		private static CachedMaterial mat_Diagonal_NE;

		// Token: 0x040049CF RID: 18895
		private static CachedMaterial mat_Diagonal_SW;

		// Token: 0x040049D0 RID: 18896
		private static CachedMaterial mat_Diagonal_SE;

		// Token: 0x040049D1 RID: 18897
		private static CachedMaterial mat_SubStructure_W;

		// Token: 0x040049D2 RID: 18898
		private static CachedMaterial mat_SubStructure_E;

		// Token: 0x040049D3 RID: 18899
		private static CachedMaterial mat_SubStructureExtra_W;

		// Token: 0x040049D4 RID: 18900
		private static CachedMaterial mat_SubStructureExtra_E;

		// Token: 0x040049D5 RID: 18901
		private static readonly float cornerAltitude = AltitudeLayer.BuildingOnTop.AltitudeFor();

		// Token: 0x040049D6 RID: 18902
		private static readonly float substructureAltitude = AltitudeLayer.TerrainEdges.AltitudeFor();

		// Token: 0x040049D7 RID: 18903
		private static readonly float bakedAltitude = AltitudeLayer.MetaOverlays.AltitudeFor();

		// Token: 0x040049D8 RID: 18904
		private static bool initalized;

		// Token: 0x040049D9 RID: 18905
		private static ShaderTypeDef wallShader;

		// Token: 0x040049DA RID: 18906
		private static ShaderTypeDef substructureShader;

		// Token: 0x040049DB RID: 18907
		private static readonly IntVec3[] Directions = new IntVec3[]
		{
			IntVec3.North,
			IntVec3.East,
			IntVec3.South,
			IntVec3.West,
			IntVec3.North + IntVec3.West,
			IntVec3.North + IntVec3.East,
			IntVec3.South + IntVec3.East,
			IntVec3.South + IntVec3.West
		};

		// Token: 0x040049DC RID: 18908
		private static readonly int[][] directionPairs = new int[][]
		{
			new int[]
			{
				0,
				2
			},
			new int[]
			{
				1,
				3
			},
			new int[]
			{
				4,
				6
			},
			new int[]
			{
				5,
				7
			}
		};

		// Token: 0x040049DD RID: 18909
		private static bool[] tmpChecks = new bool[SectionLayer_GravshipHull.Directions.Length];

		// Token: 0x0200317F RID: 12671
		public enum CornerType
		{
			// Token: 0x0400C820 RID: 51232
			None,
			// Token: 0x0400C821 RID: 51233
			Corner_NW,
			// Token: 0x0400C822 RID: 51234
			Corner_NE,
			// Token: 0x0400C823 RID: 51235
			Corner_SW,
			// Token: 0x0400C824 RID: 51236
			Corner_SE,
			// Token: 0x0400C825 RID: 51237
			Diagonal_NW,
			// Token: 0x0400C826 RID: 51238
			Diagonal_NE,
			// Token: 0x0400C827 RID: 51239
			Diagonal_SW,
			// Token: 0x0400C828 RID: 51240
			Diagonal_SE
		}
	}
}
