using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C67 RID: 7271
	public class Designator_Unforbid : Designator
	{
		// Token: 0x17002184 RID: 8580
		// (get) Token: 0x0600B979 RID: 47481 RVA: 0x00416683 File Offset: 0x00414883
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B97A RID: 47482 RVA: 0x0041668C File Offset: 0x0041488C
		public Designator_Unforbid()
		{
			this.defaultLabel = "DesignatorUnforbid".Translate();
			this.defaultDesc = "DesignatorUnforbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Checkbox_TurnedOn;
			this.hotKey = KeyBindingDefOf.Misc6;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "UnforbidAllItems".Translate();
			this.tutorTag = "Unforbid";
		}

		// Token: 0x0600B97B RID: 47483 RVA: 0x00416734 File Offset: 0x00414934
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return "MessageMustDesignateUnforbiddable".Translate();
			}
			return true;
		}

		// Token: 0x0600B97C RID: 47484 RVA: 0x0041679C File Offset: 0x0041499C
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x0600B97D RID: 47485 RVA: 0x004167EC File Offset: 0x004149EC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.category != ThingCategory.Item)
			{
				return false;
			}
			CompForbiddable compForbiddable = t.TryGetComp<CompForbiddable>();
			return compForbiddable != null && compForbiddable.Forbidden;
		}

		// Token: 0x0600B97E RID: 47486 RVA: 0x00416826 File Offset: 0x00414A26
		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(false, false);
		}
	}
}
