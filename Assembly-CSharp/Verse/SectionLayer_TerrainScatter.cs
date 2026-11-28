using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C0 RID: 704
	public class SectionLayer_TerrainScatter : SectionLayer
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600157D RID: 5501 RVA: 0x0008001E File Offset: 0x0007E21E
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00080025 File Offset: 0x0007E225
		public SectionLayer_TerrainScatter(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Terrain;
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0008004C File Offset: 0x0007E24C
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			if (this.scats == null)
			{
				this.scats = new List<SectionLayer_TerrainScatter.Scatterable>();
				this.GenerateScats();
			}
			this.scats.RemoveAll((SectionLayer_TerrainScatter.Scatterable scat) => !scat.IsOnValidTerrain);
			for (int i = 0; i < this.scats.Count; i++)
			{
				this.scats[i].PrintOnto(this);
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x000800D8 File Offset: 0x0007E2D8
		private void GenerateScats()
		{
			foreach (IntVec3 intVec in SectionLayer_TerrainScatter.GetScatPoints(base.Map))
			{
				if (this.section.CellRect.Contains(intVec))
				{
					TerrainDef terrainDef = base.Map.terrainGrid.TerrainAt(intVec);
					if (terrainDef.scatterType != null)
					{
						if (!this.scatterDefsByType.ContainsKey(terrainDef.scatterType))
						{
							this.scatterDefsByType[terrainDef.scatterType] = new List<ScatterableDef>();
							foreach (ScatterableDef scatterableDef in DefDatabase<ScatterableDef>.AllDefs)
							{
								if (scatterableDef.scatterType == terrainDef.scatterType)
								{
									this.scatterDefsByType[terrainDef.scatterType].Add(scatterableDef);
								}
							}
						}
						if (!this.scatterDefsByType[terrainDef.scatterType].Empty<ScatterableDef>())
						{
							ScatterableDef scatterableDef2 = this.scatterDefsByType[terrainDef.scatterType].RandomElement<ScatterableDef>();
							if (Rand.Chance(scatterableDef2.scatterChance))
							{
								Vector3 loc = new Vector3((float)intVec.x + (Rand.Value - 0.5f), 0f, (float)intVec.z + (Rand.Value - 0.5f));
								SectionLayer_TerrainScatter.Scatterable scatterable = new SectionLayer_TerrainScatter.Scatterable(scatterableDef2, loc, base.Map);
								if (scatterable.IsOnValidTerrain)
								{
									this.scats.Add(scatterable);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x000802A4 File Offset: 0x0007E4A4
		private static List<IntVec3> GetScatPoints(Map map)
		{
			int key = map.ConstantRandSeed * Find.World.info.Seed;
			List<IntVec3> result;
			if (SectionLayer_TerrainScatter.cachedScatterablePoints.TryGetValue(key, out result))
			{
				return result;
			}
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 item in map.AllCells)
			{
				list.Add(item);
			}
			using (new ProfilerBlock("GenerateBlueNoisePoints"))
			{
				SectionLayer_TerrainScatter.cachedScatterablePoints[key] = SectionLayer_TerrainScatter.GenerateBlueNoisePoints(list, 5f);
			}
			return SectionLayer_TerrainScatter.cachedScatterablePoints[key];
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0008036C File Offset: 0x0007E56C
		private static List<IntVec3> GenerateBlueNoisePoints(List<IntVec3> candidatePoints, float minimumSpacing)
		{
			IntVec3[] array = new IntVec3[candidatePoints.Count];
			List<IntVec3> list = new List<IntVec3>();
			int num = 0;
			foreach (IntVec3 intVec in candidatePoints.InRandomOrder(null))
			{
				array[num] = intVec;
				num++;
			}
			for (int i = array.Length - 1; i >= 0; i--)
			{
				bool flag = true;
				for (int j = 0; j < list.Count; j++)
				{
					IntVec3 otherLoc = list[j];
					if (array[i].InHorDistOf(otherLoc, minimumSpacing))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(array[i]);
				}
			}
			return list;
		}

		// Token: 0x040012B5 RID: 4789
		private const float MinimumSpacing = 5f;

		// Token: 0x040012B6 RID: 4790
		private List<SectionLayer_TerrainScatter.Scatterable> scats;

		// Token: 0x040012B7 RID: 4791
		private Dictionary<string, List<ScatterableDef>> scatterDefsByType = new Dictionary<string, List<ScatterableDef>>();

		// Token: 0x040012B8 RID: 4792
		private static readonly Dictionary<int, List<IntVec3>> cachedScatterablePoints = new Dictionary<int, List<IntVec3>>();

		// Token: 0x020025FF RID: 9727
		private class Scatterable
		{
			// Token: 0x0600F1C3 RID: 61891 RVA: 0x0058C488 File Offset: 0x0058A688
			public Scatterable(ScatterableDef def, Vector3 loc, Map map)
			{
				this.def = def;
				this.loc = loc;
				this.map = map;
				this.size = Rand.Range(def.minSize, def.maxSize);
				this.rotation = Rand.Range(0f, 360f);
			}

			// Token: 0x0600F1C4 RID: 61892 RVA: 0x0058C4DC File Offset: 0x0058A6DC
			public void PrintOnto(SectionLayer layer)
			{
				Material mat = this.def.mat;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Terrain, false, false, out mat, out uvs, out color);
				Printer_Plane.PrintPlane(layer, this.loc, Vector2.one * this.size, mat, this.rotation, false, uvs, null, 0.01f, 0f);
			}

			// Token: 0x17002A9D RID: 10909
			// (get) Token: 0x0600F1C5 RID: 61893 RVA: 0x0058C538 File Offset: 0x0058A738
			public bool IsOnValidTerrain
			{
				get
				{
					IntVec3 intVec = this.loc.ToIntVec3();
					if (this.def.scatterType != this.map.terrainGrid.TerrainAt(intVec).scatterType || intVec.Filled(this.map))
					{
						return false;
					}
					if (!this.def.placeUnderNaturalRoofs)
					{
						RoofDef roof = intVec.GetRoof(this.map);
						if (roof != null && roof.isNatural)
						{
							return false;
						}
					}
					foreach (IntVec3 c in CellRect.CenteredOn(intVec, Mathf.FloorToInt(this.size / 2f)).ClipInsideMap(this.map))
					{
						TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
						if (terrainDef.IsFloor || terrainDef.IsSubstructure || terrainDef == TerrainDefOf.Space)
						{
							return false;
						}
					}
					return true;
				}
			}

			// Token: 0x04009BDC RID: 39900
			private Map map;

			// Token: 0x04009BDD RID: 39901
			private ScatterableDef def;

			// Token: 0x04009BDE RID: 39902
			private Vector3 loc;

			// Token: 0x04009BDF RID: 39903
			private float size;

			// Token: 0x04009BE0 RID: 39904
			private float rotation;
		}
	}
}
