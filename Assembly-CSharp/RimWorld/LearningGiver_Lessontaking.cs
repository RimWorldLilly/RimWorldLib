using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B90 RID: 2960
	public class LearningGiver_Lessontaking : LearningGiver
	{
		// Token: 0x060055DA RID: 21978 RVA: 0x001FF488 File Offset: 0x001FD688
		public override bool CanDo(Pawn pawn)
		{
			if (!base.CanDo(pawn))
			{
				return false;
			}
			if (PawnUtility.WillSoonHaveBasicNeed(pawn, 0.05f))
			{
				return false;
			}
			Pawn pawn2 = SchoolUtility.FindTeacher(pawn);
			if (pawn2 == null)
			{
				return false;
			}
			Thing thing = SchoolUtility.ClosestSchoolDesk(pawn, pawn2);
			return thing != null && pawn.CanReserveSittableOrSpot(SchoolUtility.DeskSpotStudent(thing), false);
		}

		// Token: 0x060055DB RID: 21979 RVA: 0x001FF4D8 File Offset: 0x001FD6D8
		public override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = SchoolUtility.FindTeacher(pawn);
			if (pawn2 == null)
			{
				return null;
			}
			Thing thing = SchoolUtility.ClosestSchoolDesk(pawn, pawn2);
			if (thing == null)
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, thing);
		}
	}
}
