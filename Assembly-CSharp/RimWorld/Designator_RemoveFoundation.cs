using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5B RID: 7259
	public class Designator_RemoveFoundation : Designator_RemoveFloor
	{
		// Token: 0x0600B937 RID: 47415 RVA: 0x00415178 File Offset: 0x00413378
		public Designator_RemoveFoundation()
		{
			this.defaultLabel = (ModsConfig.OdysseyActive ? "DesignatorRemoveFoundation".Translate() : "DesignatorRemoveBridge".Translate());
			this.defaultDesc = (ModsConfig.OdysseyActive ? "DesignatorRemoveFoundationDesc".Translate() : "DesignatorRemoveBridgeDesc".Translate());
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/RemoveBridge", true);
			this.soundSucceeded = SoundDefOf.Designate_RemoveBridge;
			this.hotKey = KeyBindingDefOf.Misc5;
		}

		// Token: 0x0600B938 RID: 47416 RVA: 0x00415204 File Offset: 0x00413404
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFoundation) != null)
			{
				return false;
			}
			if (!base.Map.terrainGrid.CanRemoveFoundationAt(c))
			{
				return false;
			}
			Building edifice = c.GetEdifice(base.Map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
			{
				return false;
			}
			if (WorkGiver_ConstructRemoveFoundation.AnyBuildingBlockingFoundationRemoval(c, base.Map))
			{
				return "MessageCannotRemoveSupportingFoundation".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600B939 RID: 47417 RVA: 0x004152C3 File Offset: 0x004134C3
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (DebugSettings.godMode)
			{
				base.Map.terrainGrid.RemoveFoundation(c, false);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.RemoveFoundation, null));
		}
	}
}
