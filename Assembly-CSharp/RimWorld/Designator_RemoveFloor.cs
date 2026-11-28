using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C59 RID: 7257
	public class Designator_RemoveFloor : Designator_Cells
	{
		// Token: 0x17002176 RID: 8566
		// (get) Token: 0x0600B92B RID: 47403 RVA: 0x00414EAB File Offset: 0x004130AB
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002177 RID: 8567
		// (get) Token: 0x0600B92C RID: 47404 RVA: 0x00414EAE File Offset: 0x004130AE
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Floors;
			}
		}

		// Token: 0x0600B92D RID: 47405 RVA: 0x00414EB8 File Offset: 0x004130B8
		public Designator_RemoveFloor()
		{
			this.defaultLabel = "DesignatorRemoveFloor".Translate();
			this.defaultDesc = "DesignatorRemoveFloorDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveFloor", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_RemoveFloor;
			this.hotKey = KeyBindingDefOf.Misc1;
		}

		// Token: 0x0600B92E RID: 47406 RVA: 0x00414F3C File Offset: 0x0041313C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
			{
				return false;
			}
			if (!base.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				return "TerrainMustBeRemovable".Translate();
			}
			if (WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, base.Map))
			{
				return false;
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600B92F RID: 47407 RVA: 0x00414FFB File Offset: 0x004131FB
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.RemoveTopLayer(c, true);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.RemoveFloor, null));
		}

		// Token: 0x0600B930 RID: 47408 RVA: 0x00415038 File Offset: 0x00413238
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
