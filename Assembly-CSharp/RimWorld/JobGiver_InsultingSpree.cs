using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA0 RID: 2976
	public class JobGiver_InsultingSpree : ThinkNode_JobGiver
	{
		// Token: 0x0600560E RID: 22030 RVA: 0x002000F8 File Offset: 0x001FE2F8
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = pawn.MentalState as MentalState_InsultingSpree;
			if (mentalState_InsultingSpree == null || mentalState_InsultingSpree.target == null || !pawn.CanReach(mentalState_InsultingSpree.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			if (!SocialInteractionUtility.BestInteractableCell(pawn, mentalState_InsultingSpree.target).IsValid)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Insult, mentalState_InsultingSpree.target);
		}
	}
}
