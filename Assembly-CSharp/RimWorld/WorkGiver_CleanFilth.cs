using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF9 RID: 3065
	public class WorkGiver_CleanFilth : WorkGiver_Scanner
	{
		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06005799 RID: 22425 RVA: 0x0020705E File Offset: 0x0020525E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x0600579A RID: 22426 RVA: 0x00207061 File Offset: 0x00205261
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Filth);
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x0600579B RID: 22427 RVA: 0x0020706A File Offset: 0x0020526A
		public override int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600579C RID: 22428 RVA: 0x0020706D File Offset: 0x0020526D
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea;
		}

		// Token: 0x0600579D RID: 22429 RVA: 0x0020707F File Offset: 0x0020527F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea.Count == 0;
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x0020709C File Offset: 0x0020529C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Filth filth = t as Filth;
			return filth != null && filth.Map.areaManager.Home[filth.Position] && !filth.Fogged() && pawn.CanReserve(t, 1, -1, null, forced) && filth.TicksSinceThickened >= 600;
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x00207100 File Offset: 0x00205300
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			WorkGiver_CleanFilth.<>c__DisplayClass10_0 CS$<>8__locals1 = new WorkGiver_CleanFilth.<>c__DisplayClass10_0();
			CS$<>8__locals1.pawn = pawn;
			Job job = JobMaker.MakeJob(JobDefOf.Clean);
			job.AddQueuedTarget(TargetIndex.A, t);
			int num = 15;
			CS$<>8__locals1.map = t.Map;
			CS$<>8__locals1.room = t.GetRoom(RegionType.Set_All);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = t.Position + GenRadial.RadialPattern[i];
				if (CS$<>8__locals1.<JobOnThing>g__ShouldClean|0(c))
				{
					List<Thing> thingList = c.GetThingList(CS$<>8__locals1.map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (this.HasJobOnThing(CS$<>8__locals1.pawn, thing, forced) && thing != t)
						{
							job.AddQueuedTarget(TargetIndex.A, thing);
						}
					}
					if (job.GetTargetQueue(TargetIndex.A).Count >= num)
					{
						break;
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(CS$<>8__locals1.pawn.Position));
			}
			return job;
		}

		// Token: 0x04003336 RID: 13110
		private const int MinTicksSinceThickened = 600;
	}
}
