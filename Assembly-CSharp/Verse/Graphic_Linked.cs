using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056C RID: 1388
	public class Graphic_Linked : Graphic
	{
		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x000FCE99 File Offset: 0x000FB099
		public virtual LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Basic;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x0600292E RID: 10542 RVA: 0x000FCE9C File Offset: 0x000FB09C
		public override Material MatSingle
		{
			get
			{
				return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingle, LinkDirections.None);
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x0600292F RID: 10543 RVA: 0x000FCEAF File Offset: 0x000FB0AF
		public Graphic SubGraphic
		{
			get
			{
				return this.subGraphic;
			}
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000FCEB7 File Offset: 0x000FB0B7
		public Graphic_Linked()
		{
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000FCEBF File Offset: 0x000FB0BF
		public Graphic_Linked(Graphic subGraphic)
		{
			this.subGraphic = subGraphic;
			this.data = subGraphic.data;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000FCEDA File Offset: 0x000FB0DA
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_Linked(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000FCEFC File Offset: 0x000FB0FC
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Material mat = this.LinkedDrawMatFrom(thing, thing.Position);
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, extraRotation, false, null, null, 0.01f, 0f);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, 0f);
			}
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000FCF5E File Offset: 0x000FB15E
		public override Material MatSingleFor(Thing thing)
		{
			return this.LinkedDrawMatFrom(thing, thing.Position);
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000FCF70 File Offset: 0x000FB170
		protected virtual Material LinkedDrawMatFrom(Thing parent, IntVec3 cell)
		{
			int num = 0;
			int num2 = 1;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (this.ShouldLinkWith(c, parent))
				{
					num += num2;
				}
				num2 *= 2;
			}
			LinkDirections linkSet = (LinkDirections)num;
			return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingleFor(parent), linkSet);
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000FCFC8 File Offset: 0x000FB1C8
		public virtual bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (!c.InBounds(parent.Map))
			{
				return (parent.def.graphicData.linkFlags & LinkFlags.MapEdge) > LinkFlags.None;
			}
			if (ModsConfig.OdysseyActive)
			{
				TerrainDef terrainDef = parent.Map.terrainGrid.FoundationAt(c);
				bool flag = terrainDef != null && terrainDef.IsSubstructure;
				terrainDef = parent.Map.terrainGrid.FoundationAt(parent.Position);
				if (flag != (terrainDef != null && terrainDef.IsSubstructure))
				{
					return false;
				}
			}
			return (parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.linkFlags) > LinkFlags.None;
		}

		// Token: 0x04001CC8 RID: 7368
		protected Graphic subGraphic;
	}
}
