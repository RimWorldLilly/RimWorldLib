using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128B RID: 4747
	[StaticConstructorOnStartup]
	public class SectionLayer_BridgeProps : SectionLayer
	{
		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x060075A9 RID: 30121 RVA: 0x00295007 File Offset: 0x00293207
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x060075AA RID: 30122 RVA: 0x0029500E File Offset: 0x0029320E
		protected virtual bool UseSpaceGraphics
		{
			get
			{
				return base.Map.generatorDef.renderWorld;
			}
		}

		// Token: 0x060075AB RID: 30123 RVA: 0x00295020 File Offset: 0x00293220
		public SectionLayer_BridgeProps(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Terrain;
		}

		// Token: 0x060075AC RID: 30124 RVA: 0x00295044 File Offset: 0x00293244
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			Map map = base.Map;
			TerrainGrid terrainGrid = map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			float y = AltitudeLayer.TerrainScatter.AltitudeFor();
			foreach (IntVec3 intVec in cellRect)
			{
				if (this.ShouldDrawPropsBelow(intVec, terrainGrid))
				{
					ValueTuple<Material, Material> materials = this.GetMaterials(terrainGrid.FoundationAt(intVec));
					Material item = materials.Item1;
					Material item2 = materials.Item2;
					IntVec3 c = intVec;
					c.x++;
					Material material;
					if (c.InBounds(map) && this.ShouldDrawPropsBelow(c, terrainGrid))
					{
						material = item;
					}
					else
					{
						material = item2;
					}
					LayerSubMesh subMesh = base.GetSubMesh(material);
					int count = subMesh.verts.Count;
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)(intVec.z - 1)));
					subMesh.verts.Add(new Vector3((float)intVec.x, y, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), y, (float)(intVec.z - 1)));
					subMesh.uvs.Add(new Vector2(0f, 0f));
					subMesh.uvs.Add(new Vector2(0f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 0f));
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 1);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count + 3);
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x060075AD RID: 30125 RVA: 0x002952A0 File Offset: 0x002934A0
		[return: TupleElementNames(new string[]
		{
			"loop",
			"right"
		})]
		public ValueTuple<Material, Material> GetMaterials(TerrainDef def)
		{
			ValueTuple<Material, Material> result;
			if (!this.propsMatCache.TryGetValue(def, out result))
			{
				Graphic graphic = this.UseSpaceGraphics ? def.spaceBridgePropsLoopGraphic : def.bridgePropsLoopGraphic;
				Graphic graphic2 = this.UseSpaceGraphics ? def.spaceBridgePropsRightGraphic : def.bridgePropsRightGraphic;
				result = (this.propsMatCache[def] = new ValueTuple<Material, Material>(graphic.MatSingle, graphic2.MatSingle));
			}
			return result;
		}

		// Token: 0x060075AE RID: 30126 RVA: 0x0029530C File Offset: 0x0029350C
		private bool ShouldDrawPropsBelow(IntVec3 c, TerrainGrid terrGrid)
		{
			TerrainDef terrainDef = terrGrid.FoundationAt(c);
			if (terrainDef == null || !terrainDef.bridge)
			{
				return false;
			}
			IntVec3 c2 = c;
			c2.z--;
			Map map = base.Map;
			if (!c2.InBounds(map))
			{
				return false;
			}
			TerrainDef terrainDef2 = terrGrid.TerrainAt(c2);
			return terrGrid.FoundationAt(c2) == null && (terrainDef2.passability == Traversability.Impassable || c2.SupportsStructureType(map, TerrainDefOf.Bridge.terrainAffordanceNeeded));
		}

		// Token: 0x040049B9 RID: 18873
		[TupleElementNames(new string[]
		{
			"loop",
			"right"
		})]
		private Dictionary<TerrainDef, ValueTuple<Material, Material>> propsMatCache = new Dictionary<TerrainDef, ValueTuple<Material, Material>>();
	}
}
