using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAD RID: 7597
	public class PawnColumnWorker_ReleaseAnimalToWild : PawnColumnWorker_Designator
	{
		// Token: 0x170024B0 RID: 9392
		// (get) Token: 0x0600C584 RID: 50564 RVA: 0x00488DB0 File Offset: 0x00486FB0
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.ReleaseAnimalToWild;
			}
		}

		// Token: 0x0600C585 RID: 50565 RVA: 0x00488DB7 File Offset: 0x00486FB7
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorReleaseAnimalToWildDesc".Translate();
		}

		// Token: 0x0600C586 RID: 50566 RVA: 0x00488DC8 File Offset: 0x00486FC8
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x0600C587 RID: 50567 RVA: 0x00488DFC File Offset: 0x00486FFC
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				TaggedString taggedString = "AnimalReleaseConfirm".Translate(pawn.Named("PAWN"), pawn.Faction.Named("FACTION"));
				Find.WindowStack.Add(new Dialog_Confirm(taggedString, delegate()
				{
					ReleaseAnimalToWildUtility.CheckWarnAboutBondedAnimal(pawn);
				}));
				return;
			}
			ReleaseAnimalToWildUtility.CheckWarnAboutBondedAnimal(pawn);
		}

		// Token: 0x0600C588 RID: 50568 RVA: 0x00488E88 File Offset: 0x00487088
		protected override bool ShouldConfirmDesignation(Pawn pawn, out string title)
		{
			if (pawn.HomeFaction == Faction.OfPlayer || pawn.HomeFaction == null)
			{
				return base.ShouldConfirmDesignation(pawn, out title);
			}
			title = "AnimalReleaseConfirm".Translate(pawn.Named("PAWN"), pawn.HomeFaction.Named("FACTION"));
			return true;
		}
	}
}
