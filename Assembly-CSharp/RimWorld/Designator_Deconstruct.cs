using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C42 RID: 7234
	public class Designator_Deconstruct : Designator
	{
		// Token: 0x17002149 RID: 8521
		// (get) Token: 0x0600B863 RID: 47203 RVA: 0x00410FE8 File Offset: 0x0040F1E8
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x1700214A RID: 8522
		// (get) Token: 0x0600B864 RID: 47204 RVA: 0x00410FEF File Offset: 0x0040F1EF
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Orders;
			}
		}

		// Token: 0x0600B865 RID: 47205 RVA: 0x00410FF8 File Offset: 0x0040F1F8
		public Designator_Deconstruct()
		{
			this.defaultLabel = "DesignatorDeconstruct".Translate();
			this.defaultDesc = "DesignatorDeconstructDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Deconstruct", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Designator_Deconstruct;
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x0600B866 RID: 47206 RVA: 0x00411080 File Offset: 0x0040F280
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!DebugSettings.godMode && c.Fogged(base.Map))
			{
				return false;
			}
			AcceptanceReport result;
			if (this.TopDeconstructibleInCell(c, out result) == null)
			{
				return result;
			}
			return true;
		}

		// Token: 0x0600B867 RID: 47207 RVA: 0x004110D4 File Offset: 0x0040F2D4
		public override void DesignateSingleCell(IntVec3 loc)
		{
			AcceptanceReport acceptanceReport;
			this.DesignateThing(this.TopDeconstructibleInCell(loc, out acceptanceReport));
		}

		// Token: 0x0600B868 RID: 47208 RVA: 0x004110F0 File Offset: 0x0040F2F0
		private Thing TopDeconstructibleInCell(IntVec3 loc, out AcceptanceReport reportToDisplay)
		{
			reportToDisplay = AcceptanceReport.WasRejected;
			foreach (Thing thing in from t in base.Map.thingGrid.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
			{
				AcceptanceReport acceptanceReport = this.CanDesignateThing(thing);
				if (this.CanDesignateThing(thing).Accepted)
				{
					reportToDisplay = AcceptanceReport.WasAccepted;
					return thing;
				}
				if (!acceptanceReport.Reason.NullOrEmpty())
				{
					reportToDisplay = acceptanceReport;
				}
			}
			return null;
		}

		// Token: 0x0600B869 RID: 47209 RVA: 0x004111B4 File Offset: 0x0040F3B4
		public override void DesignateThing(Thing t)
		{
			Thing innerIfMinified = t.GetInnerIfMinified();
			if (DebugSettings.godMode || innerIfMinified.GetStatValue(StatDefOf.WorkToBuild, true, -1) == 0f || t.def.IsFrame)
			{
				t.Destroy(DestroyMode.Deconstruct);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = t as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
				{
					Messages.Message("MessageOccupiedHoldingPlatformDeconstructed".Translate(), building_HoldingPlatform, MessageTypeDefOf.CautionInput, true);
				}
			}
		}

		// Token: 0x0600B86A RID: 47210 RVA: 0x00411254 File Offset: 0x0040F454
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t.GetInnerIfMinified() as Building;
			if (building == null)
			{
				return false;
			}
			if (building.def.category != ThingCategory.Building)
			{
				return false;
			}
			AcceptanceReport acceptanceReport = building.DeconstructibleBy(Faction.OfPlayer);
			if (!acceptanceReport.Accepted)
			{
				if (building.def.IsNonDeconstructibleAttackableBuilding)
				{
					return "RemoveByAttackingTooltip".Translate();
				}
				return acceptanceReport.Reason;
			}
			else
			{
				if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
				{
					return false;
				}
				if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
				{
					return false;
				}
				return true;
			}
		}

		// Token: 0x0600B86B RID: 47211 RVA: 0x00411310 File Offset: 0x0040F510
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
