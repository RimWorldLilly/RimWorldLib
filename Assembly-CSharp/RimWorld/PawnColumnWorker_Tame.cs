using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAF RID: 7599
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x170024B2 RID: 9394
		// (get) Token: 0x0600C590 RID: 50576 RVA: 0x00488F9C File Offset: 0x0048719C
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x0600C591 RID: 50577 RVA: 0x00488FA3 File Offset: 0x004871A3
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x0600C592 RID: 50578 RVA: 0x00488FB4 File Offset: 0x004871B4
		protected override bool HasCheckbox(Pawn pawn)
		{
			return TameUtility.CanTame(pawn) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x0600C593 RID: 50579 RVA: 0x00488FC6 File Offset: 0x004871C6
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn, true);
		}
	}
}
