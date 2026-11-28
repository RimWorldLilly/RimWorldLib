using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAE RID: 7598
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x170024B1 RID: 9393
		// (get) Token: 0x0600C58A RID: 50570 RVA: 0x00488EE8 File Offset: 0x004870E8
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x0600C58B RID: 50571 RVA: 0x00488EEF File Offset: 0x004870EF
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x0600C58C RID: 50572 RVA: 0x00488F00 File Offset: 0x00487100
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x0600C58D RID: 50573 RVA: 0x00488F31 File Offset: 0x00487131
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}

		// Token: 0x0600C58E RID: 50574 RVA: 0x00488F3C File Offset: 0x0048713C
		protected override bool ShouldConfirmDesignation(Pawn pawn, out string title)
		{
			if (pawn.HomeFaction == Faction.OfPlayer || pawn.HomeFaction == null)
			{
				return base.ShouldConfirmDesignation(pawn, out title);
			}
			title = "AnimalSlaughterConfirm".Translate(pawn.Named("PAWN"), pawn.HomeFaction.Named("FACTION"));
			return true;
		}
	}
}
