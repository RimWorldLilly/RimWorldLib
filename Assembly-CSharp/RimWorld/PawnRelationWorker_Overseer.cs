using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153D RID: 5437
	public class PawnRelationWorker_Overseer : PawnRelationWorker
	{
		// Token: 0x060084E2 RID: 34018 RVA: 0x002FE141 File Offset: 0x002FC341
		public override void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
			base.OnRelationCreated(firstPawn, secondPawn);
			if (MechanitorUtility.IsMechanitor(firstPawn))
			{
				firstPawn.mechanitor.AssignPawnControlGroup(secondPawn, null);
				return;
			}
			if (MechanitorUtility.IsMechanitor(secondPawn))
			{
				secondPawn.mechanitor.AssignPawnControlGroup(firstPawn, null);
			}
		}

		// Token: 0x060084E3 RID: 34019 RVA: 0x002FE176 File Offset: 0x002FC376
		public override void OnRelationRemoved(Pawn firstPawn, Pawn secondPawn)
		{
			base.OnRelationRemoved(firstPawn, secondPawn);
			if (MechanitorUtility.IsMechanitor(firstPawn))
			{
				firstPawn.mechanitor.UnassignPawnFromAnyControlGroup(secondPawn);
				return;
			}
			if (MechanitorUtility.IsMechanitor(secondPawn))
			{
				secondPawn.mechanitor.UnassignPawnFromAnyControlGroup(firstPawn);
			}
		}

		// Token: 0x060084E4 RID: 34020 RVA: 0x002FE1AC File Offset: 0x002FC3AC
		public override void Notify_PostRemovedByDeath(Pawn firstPawn, Pawn secondPawn)
		{
			Pawn pawn = MechanitorUtility.IsMechanitor(firstPawn) ? firstPawn : secondPawn;
			Pawn pawn2 = (firstPawn == pawn) ? secondPawn : firstPawn;
			if (!pawn2.Dead)
			{
				CompOverseerSubject overseerSubject = pawn2.OverseerSubject;
				if (overseerSubject != null)
				{
					overseerSubject.Notify_DisconnectedFromOverseer();
				}
			}
			if (pawn != null && !pawn.Dead)
			{
				Messages.Message(Find.ActiveLanguageWorker.PostProcessed("MessageMechanitorLostControlOfMech".Translate(pawn, pawn2) + ": " + pawn2.LabelShortCap), new LookTargets(new Thing[]
				{
					pawn,
					pawn2
				}), MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x060084E5 RID: 34021 RVA: 0x002FE24C File Offset: 0x002FC44C
		public override void Notify_PostRemovedLeftBehind(Pawn firstPawn, Pawn secondPawn)
		{
			Pawn pawn = MechanitorUtility.IsMechanitor(firstPawn) ? firstPawn : secondPawn;
			Pawn pawn2 = (firstPawn == pawn) ? secondPawn : firstPawn;
			if (!pawn2.Dead)
			{
				CompOverseerSubject overseerSubject = pawn2.OverseerSubject;
				if (overseerSubject != null)
				{
					overseerSubject.Notify_DisconnectedFromOverseer();
				}
			}
			if (pawn != null && !pawn.Dead)
			{
				Messages.Message(Find.ActiveLanguageWorker.PostProcessed("MessageMechanitorLostControlOfMech".Translate(pawn, pawn2) + ": " + pawn2.LabelShortCap), new LookTargets(new Thing[]
				{
					pawn,
					pawn2
				}), MessageTypeDefOf.NeutralEvent, true);
			}
		}
	}
}
