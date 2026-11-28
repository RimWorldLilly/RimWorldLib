using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AF2 RID: 2802
	public class JobGiver_ReturnedCaravan_PenAnimals : JobGiver_PrepareCaravan_RopePawns
	{
		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x0600538F RID: 21391 RVA: 0x001F1B05 File Offset: 0x001EFD05
		protected override JobDef RopeJobDef
		{
			get
			{
				return JobDefOf.ReturnedCaravan_PenAnimals;
			}
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x001F1B0C File Offset: 0x001EFD0C
		protected override bool AnimalNeedsGathering(Pawn pawn, Pawn animal)
		{
			return false;
		}

		// Token: 0x06005391 RID: 21393 RVA: 0x001F1B0F File Offset: 0x001EFD0F
		protected override void DecorateJob(Job job)
		{
			base.DecorateJob(job);
			job.ropeToUnenclosedPens = true;
		}
	}
}
