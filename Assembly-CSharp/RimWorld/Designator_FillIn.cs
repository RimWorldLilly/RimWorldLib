using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4B RID: 7243
	public class Designator_FillIn : Designator
	{
		// Token: 0x17002156 RID: 8534
		// (get) Token: 0x0600B8A7 RID: 47271 RVA: 0x0041233D File Offset: 0x0041053D
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x17002157 RID: 8535
		// (get) Token: 0x0600B8A8 RID: 47272 RVA: 0x00412344 File Offset: 0x00410544
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.FillIn;
			}
		}

		// Token: 0x0600B8A9 RID: 47273 RVA: 0x0041234C File Offset: 0x0041054C
		public Designator_FillIn()
		{
			this.soundSucceeded = SoundDefOf.Tick_Low;
			this.defaultLabel = "DesignatorFillIn".Translate();
			this.defaultDesc = "DesignatorFillInDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/FillCrater", true);
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x0600B8AA RID: 47274 RVA: 0x004123AC File Offset: 0x004105AC
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
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B8AB RID: 47275 RVA: 0x00412410 File Offset: 0x00410610
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

		// Token: 0x0600B8AC RID: 47276 RVA: 0x0041245F File Offset: 0x0041065F
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			return t is Crater && base.Map.designationManager.DesignationOn(t, this.Designation) == null;
		}

		// Token: 0x0600B8AD RID: 47277 RVA: 0x0041248B File Offset: 0x0041068B
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
		}
	}
}
