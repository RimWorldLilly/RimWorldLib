using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C48 RID: 7240
	public class Designator_ExtractTree : Designator
	{
		// Token: 0x17002155 RID: 8533
		// (get) Token: 0x0600B89A RID: 47258 RVA: 0x00411E6B File Offset: 0x0041006B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B89B RID: 47259 RVA: 0x00411E74 File Offset: 0x00410074
		public Designator_ExtractTree()
		{
			this.defaultLabel = "DesignatorExtractTree".Translate();
			this.defaultDesc = "DesignatorExtractTreeDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ExtractTree", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ExtractTree;
			this.hotKey = KeyBindingDefOf.Misc12;
			this.tutorTag = "ExtractTree";
		}

		// Token: 0x0600B89C RID: 47260 RVA: 0x00411F00 File Offset: 0x00410100
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B89D RID: 47261 RVA: 0x00411F8C File Offset: 0x0041018C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Plant plant = t as Plant;
			if (plant != null && plant.def.Minifiable && plant.def.plant.IsTree && base.Map.designationManager.DesignationOn(plant, DesignationDefOf.ExtractTree) == null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600B89E RID: 47262 RVA: 0x00411FE8 File Offset: 0x004101E8
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					this.DesignateThing(t);
				}
			}
		}

		// Token: 0x0600B89F RID: 47263 RVA: 0x00412054 File Offset: 0x00410254
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation((Plant)t, DesignationDefOf.ExtractTree, null));
			Designation designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.CutPlant);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
			designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.HarvestPlant);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
		}
	}
}
