using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C53 RID: 7251
	public class Designator_Open : Designator
	{
		// Token: 0x1700216A RID: 8554
		// (get) Token: 0x0600B8FC RID: 47356 RVA: 0x00413F0A File Offset: 0x0041210A
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Open;
			}
		}

		// Token: 0x1700216B RID: 8555
		// (get) Token: 0x0600B8FD RID: 47357 RVA: 0x00413F11 File Offset: 0x00412111
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B8FE RID: 47358 RVA: 0x00413F18 File Offset: 0x00412118
		public Designator_Open()
		{
			this.defaultLabel = "DesignatorOpen".Translate();
			this.defaultDesc = "DesignatorOpenDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Open", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.hotKey = KeyBindingDefOf.Misc5;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
		}

		// Token: 0x0600B8FF RID: 47359 RVA: 0x00413F99 File Offset: 0x00412199
		protected override void FinalizeDesignationFailed()
		{
			base.FinalizeDesignationFailed();
			Messages.Message("MessageMustDesignateOpenable".Translate(), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0600B900 RID: 47360 RVA: 0x00413FBB File Offset: 0x004121BB
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.OpenablesInCell(c).Any<Thing>())
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B901 RID: 47361 RVA: 0x00413FF0 File Offset: 0x004121F0
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.OpenablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0600B902 RID: 47362 RVA: 0x00414040 File Offset: 0x00412240
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			IOpenable openable = t as IOpenable;
			if (openable == null || !openable.CanOpen || base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B903 RID: 47363 RVA: 0x00414085 File Offset: 0x00412285
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
		}

		// Token: 0x0600B904 RID: 47364 RVA: 0x004140A9 File Offset: 0x004122A9
		private IEnumerable<Thing> OpenablesInCell(IntVec3 c)
		{
			Designator_Open.<OpenablesInCell>d__10 <OpenablesInCell>d__ = new Designator_Open.<OpenablesInCell>d__10(-2);
			<OpenablesInCell>d__.<>4__this = this;
			<OpenablesInCell>d__.<>3__c = c;
			return <OpenablesInCell>d__;
		}
	}
}
