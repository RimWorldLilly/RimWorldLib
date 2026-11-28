using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4C RID: 7244
	public class Designator_Forbid : Designator
	{
		// Token: 0x17002158 RID: 8536
		// (get) Token: 0x0600B8AF RID: 47279 RVA: 0x004124CC File Offset: 0x004106CC
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B8B0 RID: 47280 RVA: 0x004124D4 File Offset: 0x004106D4
		public Designator_Forbid()
		{
			this.defaultLabel = "DesignatorForbid".Translate();
			this.defaultDesc = "DesignatorForbidDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOn", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Checkbox_TurnedOff;
			this.hotKey = KeyBindingDefOf.Command_ItemForbid;
			this.hasDesignateAllFloatMenuOption = true;
			this.designateAllLabel = "ForbidAllItems".Translate();
		}

		// Token: 0x0600B8B1 RID: 47281 RVA: 0x00412574 File Offset: 0x00410774
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return "MessageMustDesignateForbiddable".Translate();
			}
			return true;
		}

		// Token: 0x0600B8B2 RID: 47282 RVA: 0x004125DC File Offset: 0x004107DC
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

		// Token: 0x0600B8B3 RID: 47283 RVA: 0x0041262C File Offset: 0x0041082C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.category != ThingCategory.Item)
			{
				return false;
			}
			CompForbiddable compForbiddable = t.TryGetComp<CompForbiddable>();
			return compForbiddable != null && !compForbiddable.Forbidden;
		}

		// Token: 0x0600B8B4 RID: 47284 RVA: 0x00412669 File Offset: 0x00410869
		public override void DesignateThing(Thing t)
		{
			t.SetForbidden(true, false);
		}
	}
}
