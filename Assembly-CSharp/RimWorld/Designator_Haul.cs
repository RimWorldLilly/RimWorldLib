using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4D RID: 7245
	public class Designator_Haul : Designator
	{
		// Token: 0x17002159 RID: 8537
		// (get) Token: 0x0600B8B6 RID: 47286 RVA: 0x00412690 File Offset: 0x00410890
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Haul;
			}
		}

		// Token: 0x1700215A RID: 8538
		// (get) Token: 0x0600B8B7 RID: 47287 RVA: 0x00412697 File Offset: 0x00410897
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B8B8 RID: 47288 RVA: 0x004126A0 File Offset: 0x004108A0
		public Designator_Haul()
		{
			this.defaultLabel = "DesignatorHaulThings".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Haul", true);
			this.defaultDesc = "DesignatorHaulThingsDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Haul;
			this.hotKey = KeyBindingDefOf.Misc12;
		}

		// Token: 0x0600B8B9 RID: 47289 RVA: 0x00412724 File Offset: 0x00410924
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			Thing firstHaulable = c.GetFirstHaulable(base.Map);
			if (firstHaulable == null)
			{
				return "MessageMustDesignateHaulable".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(firstHaulable);
			if (!result.Accepted)
			{
				return result;
			}
			return true;
		}

		// Token: 0x0600B8BA RID: 47290 RVA: 0x0041278D File Offset: 0x0041098D
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetFirstHaulable(base.Map));
		}

		// Token: 0x0600B8BB RID: 47291 RVA: 0x004127A4 File Offset: 0x004109A4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.designateHaulable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			if (t.IsInValidStorage())
			{
				return "MessageAlreadyInStorage".Translate();
			}
			return true;
		}

		// Token: 0x0600B8BC RID: 47292 RVA: 0x00412803 File Offset: 0x00410A03
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
		}

		// Token: 0x0600B8BD RID: 47293 RVA: 0x00412827 File Offset: 0x00410A27
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
