using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C77 RID: 7287
	public class Designator_Plan_CopySelection : Designator_Plan
	{
		// Token: 0x170021BA RID: 8634
		// (get) Token: 0x0600BA1A RID: 47642 RVA: 0x0041A02E File Offset: 0x0041822E
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x170021BB RID: 8635
		// (get) Token: 0x0600BA1B RID: 47643 RVA: 0x0041A035 File Offset: 0x00418235
		public override bool DrawHighlight
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BA1C RID: 47644 RVA: 0x0041A038 File Offset: 0x00418238
		public Designator_Plan_CopySelection()
		{
			this.useMouseIcon = true;
			this.hideMouseIcon = true;
			this.mouseText = "CommandCopyPlanSelectionMouse".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanCopySelection", true);
			this.defaultLabel = "CommandCopyPlanSelectionLabel".Translate();
			this.defaultDesc = "CommandCopyPlanSelectionDesc".Translate();
			this.hotKey = KeyBindingDefOf.Designator_Cancel;
		}

		// Token: 0x0600BA1D RID: 47645 RVA: 0x0041A0B4 File Offset: 0x004182B4
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return loc.InBounds(base.Map);
		}

		// Token: 0x0600BA1E RID: 47646 RVA: 0x0041A0C8 File Offset: 0x004182C8
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			Designator_Plan_CopySelectionPaste designator_Plan_CopySelectionPaste = DesignatorUtility.FindAllowedDesignator<Designator_Plan_CopySelectionPaste>();
			designator_Plan_CopySelectionPaste.Initialize(cells);
			Find.DesignatorManager.Select(designator_Plan_CopySelectionPaste);
			SoundDefOf.Click.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
		}

		// Token: 0x0600BA1F RID: 47647 RVA: 0x0041A0FD File Offset: 0x004182FD
		public override void Selected()
		{
			base.Selected();
			Find.Selector.SelectedPlan = null;
		}
	}
}
