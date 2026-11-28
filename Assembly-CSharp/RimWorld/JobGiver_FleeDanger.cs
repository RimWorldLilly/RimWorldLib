using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B3B RID: 2875
	public class JobGiver_FleeDanger : ThinkNode_JobGiver
	{
		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06005469 RID: 21609 RVA: 0x001F5EDF File Offset: 0x001F40DF
		public override string CrawlingReportStringOverride
		{
			get
			{
				return base.CrawlingReportStringOverride ?? "ReportStringCrawlingToSafety".Translate();
			}
		}

		// Token: 0x0600546A RID: 21610 RVA: 0x001F5EFC File Offset: 0x001F40FC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.Downed && !pawn.health.CanCrawl)
			{
				return null;
			}
			Job job = FleeUtility.FleeLargeFireJob(pawn, 1, 4, 10);
			if (job != null)
			{
				return job;
			}
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				Thing thing = potentialTargetsFor[i].Thing;
				if (pawn.Position.InHorDistOf(thing.Position, 4f) && FleeUtility.ShouldFleeFrom(thing, pawn, false, true))
				{
					Pawn pawn2 = thing as Pawn;
					if (pawn2 == null || !pawn2.AnimalOrWildMan() || pawn2.Faction != null)
					{
						Job job2 = FleeUtility.FleeJob(pawn, thing, 10);
						if (job2 != null)
						{
							return job2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x04003274 RID: 12916
		private const int FleeDistance = 10;

		// Token: 0x04003275 RID: 12917
		private const int DistToFireToFlee = 4;

		// Token: 0x04003276 RID: 12918
		private const int DistToDangerToFlee = 4;
	}
}
