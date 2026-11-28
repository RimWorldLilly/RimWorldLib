using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009C3 RID: 2499
	public static class SchoolUtility
	{
		// Token: 0x06004BF0 RID: 19440 RVA: 0x001D6114 File Offset: 0x001D4314
		public static Pawn FindTeacher(Pawn child)
		{
			foreach (Pawn pawn in ChildcareUtility.SpawnedColonistChildcarers(child.Map))
			{
				if (SchoolUtility.CanTeachNow(pawn) && pawn.CanReach(child, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x001D6188 File Offset: 0x001D4388
		public static bool CanTeachNow(Pawn teacher)
		{
			return !teacher.DevelopmentalStage.Juvenile() && !teacher.Downed && !teacher.Drafted && !teacher.WorkTypeIsDisabled(WorkTypeDefOf.Childcare) && teacher.workSettings.WorkIsActive(WorkTypeDefOf.Childcare) && teacher.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && teacher.Awake() && !teacher.IsBurning() && !PawnUtility.WillSoonHaveBasicNeed(teacher, 0.1f) && !teacher.InMentalState && teacher.GetLord() == null;
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x001D6218 File Offset: 0x001D4418
		public static bool NeedsTeacher(Pawn student)
		{
			if (student.CurJobDef != JobDefOf.Lessontaking)
			{
				return false;
			}
			Pawn pawn = student.CurJob.GetTarget(TargetIndex.B).Pawn;
			return pawn == null || pawn.CurJobDef != JobDefOf.Lessongiving || !(pawn.CurJob.GetTarget(TargetIndex.B) == student);
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x001D6275 File Offset: 0x001D4475
		public static IntVec3 DeskSpotTeacher(Thing desk)
		{
			if (desk.def != ThingDefOf.SchoolDesk)
			{
				return IntVec3.Invalid;
			}
			return desk.InteractionCells[1];
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x001D6296 File Offset: 0x001D4496
		public static IntVec3 DeskSpotStudent(Thing desk)
		{
			if (desk.def != ThingDefOf.SchoolDesk)
			{
				return IntVec3.Invalid;
			}
			return desk.InteractionCells[0];
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x001D62B8 File Offset: 0x001D44B8
		public static Thing ClosestSchoolDesk(Pawn child, Pawn teacher)
		{
			if (teacher == null)
			{
				return null;
			}
			return GenClosest.ClosestThingReachable(child.Position, child.Map, ThingRequest.ForDef(ThingDefOf.SchoolDesk), PathEndMode.InteractionCell, TraverseParms.For(child, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing d) => child.CanReserveSittableOrSpot(SchoolUtility.DeskSpotStudent(d), false) && teacher.CanReserveSittableOrSpot(SchoolUtility.DeskSpotTeacher(d), false) && !d.IsForbidden(child) && !d.IsForbidden(teacher), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x04002FF2 RID: 12274
		public const TargetIndex DeskIndex = TargetIndex.A;

		// Token: 0x04002FF3 RID: 12275
		public const TargetIndex TeacherIndex = TargetIndex.B;

		// Token: 0x04002FF4 RID: 12276
		public const TargetIndex StudentIndex = TargetIndex.B;

		// Token: 0x04002FF5 RID: 12277
		private const int DeskStudentInteractionIndex = 0;

		// Token: 0x04002FF6 RID: 12278
		private const int DeskTeacherInteractionIndex = 1;
	}
}
