using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C33 RID: 7219
	public class Designator_AreaBuildRoof : Designator_Cells
	{
		// Token: 0x1700213B RID: 8507
		// (get) Token: 0x0600B824 RID: 47140 RVA: 0x0040FDA6 File Offset: 0x0040DFA6
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700213C RID: 8508
		// (get) Token: 0x0600B825 RID: 47141 RVA: 0x0040FDA9 File Offset: 0x0040DFA9
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x0600B826 RID: 47142 RVA: 0x0040FDB0 File Offset: 0x0040DFB0
		public Designator_AreaBuildRoof()
		{
			this.defaultLabel = "DesignatorAreaBuildRoofExpand".Translate();
			this.defaultDesc = "DesignatorAreaBuildRoofExpandDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/BuildRoofArea", true);
			this.hotKey = KeyBindingDefOf.Misc9;
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_Roof;
			this.useMouseIcon = true;
			this.tutorTag = "AreaBuildRoofExpand";
		}

		// Token: 0x0600B827 RID: 47143 RVA: 0x0040FE3C File Offset: 0x0040E03C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			return !base.Map.areaManager.BuildRoof[c];
		}

		// Token: 0x0600B828 RID: 47144 RVA: 0x0040FE91 File Offset: 0x0040E091
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.Map.areaManager.BuildRoof[c] = true;
			base.Map.areaManager.NoRoof[c] = false;
		}

		// Token: 0x0600B829 RID: 47145 RVA: 0x0040FEC4 File Offset: 0x0040E0C4
		public override bool ShowWarningForCell(IntVec3 c)
		{
			foreach (Thing thing in base.Map.thingGrid.ThingsAt(c))
			{
				if (thing.def.plant != null && thing.def.plant.interferesWithRoof)
				{
					Messages.Message("MessageRoofIncompatibleWithPlant".Translate(thing), MessageTypeDefOf.CautionInput, false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B82A RID: 47146 RVA: 0x0040FF5C File Offset: 0x0040E15C
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			base.Map.areaManager.NoRoof.MarkForDraw();
			base.Map.areaManager.BuildRoof.MarkForDraw();
		}
	}
}
