using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAC RID: 7596
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x170024AF RID: 9391
		// (get) Token: 0x0600C57F RID: 50559 RVA: 0x00488D39 File Offset: 0x00486F39
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x0600C580 RID: 50560 RVA: 0x00488D40 File Offset: 0x00486F40
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x0600C581 RID: 50561 RVA: 0x00488D51 File Offset: 0x00486F51
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x0600C582 RID: 50562 RVA: 0x00488D8A File Offset: 0x00486F8A
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
			Designator_Hunt.ShowDesignationWarnings(pawn);
		}
	}
}
