using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C5 RID: 709
	public abstract class SectionLayer_Things : SectionLayer
	{
		// Token: 0x0600158F RID: 5519 RVA: 0x00080E8A File Offset: 0x0007F08A
		public SectionLayer_Things(Section section) : base(section)
		{
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00080E9E File Offset: 0x0007F09E
		public override CellRect GetBoundaryRect()
		{
			return this.bounds;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x00080EA8 File Offset: 0x0007F0A8
		public override void DrawLayer()
		{
			if (!DebugViewSettings.drawThingsPrinted)
			{
				return;
			}
			if (WorldComponent_GravshipController.CutsceneInProgress && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				this.tmpFormerlyEnabled.Clear();
				for (int i = 0; i < this.subMeshes.Count; i++)
				{
					LayerSubMesh layerSubMesh = this.subMeshes[i];
					if (!layerSubMesh.disabled && layerSubMesh.material.renderQueue == 2950)
					{
						this.tmpFormerlyEnabled.Add(layerSubMesh);
						layerSubMesh.disabled = true;
					}
				}
				base.DrawLayer();
				for (int j = 0; j < this.tmpFormerlyEnabled.Count; j++)
				{
					this.tmpFormerlyEnabled[j].disabled = false;
				}
				return;
			}
			base.DrawLayer();
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00080F64 File Offset: 0x0007F164
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			this.bounds = this.section.CellRect;
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Thing thing = list[i];
					if ((thing.def.seeThroughFog || !base.Map.fogGrid.IsFogged(thing.Position)) && thing.def.drawerType != DrawerType.None && (thing.def.drawerType != DrawerType.RealtimeOnly || !this.requireAddToMapMesh) && (thing.def.hideAtSnowOrSandDepth >= 1f || Math.Max(base.Map.snowGrid.GetDepth(thing.Position), thing.Position.GetSandDepth(base.Map)) <= thing.def.hideAtSnowOrSandDepth) && (thing.def.plant == null || thing.def.plant.showInFrozenWater || thing.Position.GetTerrain(base.Map) != TerrainDefOf.ThinIce) && thing.Position.x == intVec.x && thing.Position.z == intVec.z)
					{
						this.TakePrintFrom(thing);
						this.bounds = this.bounds.Encapsulate(thing.OccupiedDrawRect());
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06001593 RID: 5523
		protected abstract void TakePrintFrom(Thing t);

		// Token: 0x040012C2 RID: 4802
		private CellRect bounds;

		// Token: 0x040012C3 RID: 4803
		private List<LayerSubMesh> tmpFormerlyEnabled = new List<LayerSubMesh>();

		// Token: 0x040012C4 RID: 4804
		protected bool requireAddToMapMesh;
	}
}
