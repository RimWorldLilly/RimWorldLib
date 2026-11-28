using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C68 RID: 7272
	public class Designator_Uninstall : Designator
	{
		// Token: 0x17002185 RID: 8581
		// (get) Token: 0x0600B980 RID: 47488 RVA: 0x0041684C File Offset: 0x00414A4C
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x17002186 RID: 8582
		// (get) Token: 0x0600B981 RID: 47489 RVA: 0x00416853 File Offset: 0x00414A53
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B982 RID: 47490 RVA: 0x0041685C File Offset: 0x00414A5C
		public Designator_Uninstall()
		{
			this.defaultLabel = "DesignatorUninstall".Translate();
			this.defaultDesc = "DesignatorUninstallDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Uninstall", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Misc12;
		}

		// Token: 0x0600B983 RID: 47491 RVA: 0x004168E0 File Offset: 0x00414AE0
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
			if (this.TopUninstallableInCell(c) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B984 RID: 47492 RVA: 0x00416934 File Offset: 0x00414B34
		public override void DesignateSingleCell(IntVec3 loc)
		{
			this.DesignateThing(this.TopUninstallableInCell(loc));
		}

		// Token: 0x0600B985 RID: 47493 RVA: 0x00416944 File Offset: 0x00414B44
		private Thing TopUninstallableInCell(IntVec3 loc)
		{
			foreach (Thing thing in from t in base.Map.thingGrid.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
			{
				if (this.CanDesignateThing(thing).Accepted)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x0600B986 RID: 47494 RVA: 0x004169D4 File Offset: 0x00414BD4
		public override void DesignateThing(Thing t)
		{
			if (t.Faction != Faction.OfPlayer)
			{
				t.SetFaction(Faction.OfPlayer, null);
			}
			if (DebugSettings.godMode || t.GetStatValue(StatDefOf.WorkToBuild, true, -1) == 0f || t.def.IsFrame)
			{
				t.Uninstall();
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = t as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
				{
					Messages.Message("MessageOccupiedHoldingPlatformUninstalled".Translate(), building_HoldingPlatform, MessageTypeDefOf.CautionInput, true);
				}
			}
		}

		// Token: 0x0600B987 RID: 47495 RVA: 0x00416A88 File Offset: 0x00414C88
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Building building = t as Building;
			if (building == null)
			{
				return false;
			}
			if (building.def.category != ThingCategory.Building)
			{
				return false;
			}
			if (!building.def.Minifiable)
			{
				return false;
			}
			if (!DebugSettings.godMode && building.Faction != Faction.OfPlayer && !building.def.building.alwaysUninstallable)
			{
				if (building.Faction != null)
				{
					return false;
				}
				if (!building.ClaimableBy(Faction.OfPlayer))
				{
					return false;
				}
			}
			if (base.Map.designationManager.DesignationOn(t, this.Designation) != null)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B988 RID: 47496 RVA: 0x00416B62 File Offset: 0x00414D62
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}
	}
}
