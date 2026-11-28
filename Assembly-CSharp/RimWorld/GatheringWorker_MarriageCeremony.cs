using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F62 RID: 3938
	public class GatheringWorker_MarriageCeremony : GatheringWorker
	{
		// Token: 0x06006541 RID: 25921 RVA: 0x0024174A File Offset: 0x0023F94A
		private static void FindFiancees(Pawn organizer, out Pawn firstFiance, out Pawn secondFiance)
		{
			firstFiance = organizer;
			secondFiance = organizer.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
		}

		// Token: 0x06006542 RID: 25922 RVA: 0x00241764 File Offset: 0x0023F964
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			Pawn firstPawn;
			Pawn secondPawn;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstPawn, out secondPawn);
			return new LordJob_Joinable_MarriageCeremony(firstPawn, secondPawn, spot);
		}

		// Token: 0x06006543 RID: 25923 RVA: 0x00241784 File Offset: 0x0023F984
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			Pawn firstFiance;
			Pawn secondFiance;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstFiance, out secondFiance);
			return RCellFinder.TryFindMarriageSite(firstFiance, secondFiance, out spot);
		}

		// Token: 0x06006544 RID: 25924 RVA: 0x002417A4 File Offset: 0x0023F9A4
		protected override void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Pawn pawn;
			Pawn pawn2;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
			Messages.Message("MessageNewMarriageCeremony".Translate(pawn.LabelShort, pawn2.LabelShort, pawn.Named("PAWN1"), pawn2.Named("PAWN2")), new TargetInfo(spot, pawn.Map, false), MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x06006545 RID: 25925 RVA: 0x00241814 File Offset: 0x0023FA14
		public override bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer != null)
			{
				Pawn pawn;
				Pawn pawn2;
				GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
				if (!GatheringsUtility.PawnCanStartOrContinueGathering(pawn) || !GatheringsUtility.PawnCanStartOrContinueGathering(pawn2))
				{
					return false;
				}
			}
			return base.CanExecute(map, organizer);
		}
	}
}
