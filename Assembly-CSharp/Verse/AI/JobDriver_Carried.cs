using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000817 RID: 2071
	public class JobDriver_Carried : JobDriver
	{
		// Token: 0x06004124 RID: 16676 RVA: 0x0019E0FB File Offset: 0x0019C2FB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0019E0FE File Offset: 0x0019C2FE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Carried.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_Carried.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x0019E10E File Offset: 0x0019C30E
		private static Toil CarryToil()
		{
			Toil toil = ToilMaker.MakeToil("CarryToil");
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x0019E121 File Offset: 0x0019C321
		public override string GetReport()
		{
			return JobDriver_Carried.GetReport(this.pawn, this.pawn.SpawnedParentOrMe);
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x0019E13C File Offset: 0x0019C33C
		public static string GetReport(Pawn pawn, Thing spawnedParentOrMe)
		{
			if (spawnedParentOrMe == null || spawnedParentOrMe == pawn)
			{
				return "";
			}
			Pawn pawn2 = spawnedParentOrMe as Pawn;
			if (pawn2 != null)
			{
				if (ModsConfig.AnomalyActive && pawn2.CurJobDef == JobDefOf.DevourerDigest)
				{
					return "DigestedBy".Translate(pawn2);
				}
				return "CarriedBy".Translate(pawn2);
			}
			else
			{
				if (spawnedParentOrMe is PawnFlyer)
				{
					return spawnedParentOrMe.LabelCap + ".";
				}
				return "InContainer".Translate(spawnedParentOrMe);
			}
		}
	}
}
