using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001873 RID: 6259
	public class CompEmptyStateGraphic : ThingComp
	{
		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x06009C94 RID: 40084 RVA: 0x00385EAA File Offset: 0x003840AA
		private CompProperties_EmptyStateGraphic Props
		{
			get
			{
				return (CompProperties_EmptyStateGraphic)this.props;
			}
		}

		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x06009C95 RID: 40085 RVA: 0x00385EB8 File Offset: 0x003840B8
		public bool ParentIsEmpty
		{
			get
			{
				IThingHolder thingHolder = this.parent as IThingHolder;
				if (thingHolder != null && thingHolder.GetDirectlyHeldThings().NullOrEmpty<Thing>())
				{
					return true;
				}
				CompPawnSpawnOnWakeup compPawnSpawnOnWakeup = this.parent.TryGetComp<CompPawnSpawnOnWakeup>();
				return compPawnSpawnOnWakeup != null && !compPawnSpawnOnWakeup.CanSpawn;
			}
		}

		// Token: 0x06009C96 RID: 40086 RVA: 0x00385EFD File Offset: 0x003840FD
		public override bool DontDrawParent()
		{
			return this.ParentIsEmpty && !this.Props.alwaysDrawParent;
		}

		// Token: 0x06009C97 RID: 40087 RVA: 0x00385F18 File Offset: 0x00384118
		public override void PostDraw()
		{
			if (!this.ParentIsEmpty || this.parent.def.drawerType == DrawerType.MapMeshOnly)
			{
				return;
			}
			Mesh mesh = this.Props.graphicData.Graphic.MeshAt(this.parent.Rotation);
			Vector3 drawPos = this.parent.DrawPos;
			Graphics.DrawMesh(mesh, drawPos + this.Props.graphicData.drawOffset.RotatedBy(this.parent.Rotation), Quaternion.identity, this.Props.graphicData.Graphic.MatAt(this.parent.Rotation, null), 0);
		}

		// Token: 0x06009C98 RID: 40088 RVA: 0x00385FBF File Offset: 0x003841BF
		public override void PostPrintOnto(SectionLayer layer)
		{
			if (this.ParentIsEmpty)
			{
				this.Props.graphicData.Graphic.Print(layer, this.parent, 0f);
			}
		}
	}
}
