using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C64 RID: 7268
	public class Designator_Strip : Designator
	{
		// Token: 0x17002180 RID: 8576
		// (get) Token: 0x0600B965 RID: 47461 RVA: 0x00415FAA File Offset: 0x004141AA
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Strip;
			}
		}

		// Token: 0x17002181 RID: 8577
		// (get) Token: 0x0600B966 RID: 47462 RVA: 0x00415FB1 File Offset: 0x004141B1
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B967 RID: 47463 RVA: 0x00415FB8 File Offset: 0x004141B8
		public Designator_Strip()
		{
			this.defaultLabel = "DesignatorStrip".Translate();
			this.defaultDesc = "DesignatorStripDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Strip", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x0600B968 RID: 47464 RVA: 0x00416039 File Offset: 0x00414239
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.StrippablesInCell(c).Any<Thing>())
			{
				return "MessageMustDesignateStrippable".Translate();
			}
			return true;
		}

		// Token: 0x0600B969 RID: 47465 RVA: 0x00416074 File Offset: 0x00414274
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.StrippablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0600B96A RID: 47466 RVA: 0x004160C4 File Offset: 0x004142C4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			return StrippableUtility.CanBeStrippedByColony(t);
		}

		// Token: 0x0600B96B RID: 47467 RVA: 0x004160F1 File Offset: 0x004142F1
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(t);
		}

		// Token: 0x0600B96C RID: 47468 RVA: 0x0041611B File Offset: 0x0041431B
		private IEnumerable<Thing> StrippablesInCell(IntVec3 c)
		{
			Designator_Strip.<StrippablesInCell>d__9 <StrippablesInCell>d__ = new Designator_Strip.<StrippablesInCell>d__9(-2);
			<StrippablesInCell>d__.<>4__this = this;
			<StrippablesInCell>d__.<>3__c = c;
			return <StrippablesInCell>d__;
		}
	}
}
