using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F5 RID: 8949
	public class MoveableArea_Allowed : MoveableArea
	{
		// Token: 0x0600DD39 RID: 56633 RVA: 0x00525E82 File Offset: 0x00524082
		public MoveableArea_Allowed()
		{
		}

		// Token: 0x0600DD3A RID: 56634 RVA: 0x00525E95 File Offset: 0x00524095
		public MoveableArea_Allowed(Gravship gravship, Area area) : base(gravship, area.Label, area.RenamableLabel, area.Color, area.ID)
		{
		}

		// Token: 0x0600DD3B RID: 56635 RVA: 0x00525EC1 File Offset: 0x005240C1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.assignedPawns, "assignedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0600DD3C RID: 56636 RVA: 0x00525EE0 File Offset: 0x005240E0
		public bool TryCreateArea(AreaManager areaManager, IntVec3 newOrigin)
		{
			if (this.relativeCells.Count == 0)
			{
				return false;
			}
			Area_Allowed area_Allowed = areaManager.GetLabeled(this.label) as Area_Allowed;
			if (area_Allowed == null && !areaManager.TryMakeNewAllowed(out area_Allowed))
			{
				return area_Allowed != null;
			}
			foreach (IntVec3 b in base.RelativeCells)
			{
				area_Allowed[newOrigin + b] = true;
			}
			area_Allowed.SetColor(this.color);
			area_Allowed.SetLabel(this.label);
			area_Allowed.RenamableLabel = this.renamableLabel;
			foreach (Pawn pawn in this.assignedPawns)
			{
				pawn.playerSettings.AreaRestrictionInPawnCurrentMap = area_Allowed;
			}
			return true;
		}

		// Token: 0x04008DCD RID: 36301
		public List<Pawn> assignedPawns = new List<Pawn>();
	}
}
