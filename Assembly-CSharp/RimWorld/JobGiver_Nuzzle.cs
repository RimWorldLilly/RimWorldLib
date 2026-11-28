using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB9 RID: 2745
	public class JobGiver_Nuzzle : ThinkNode_JobGiver
	{
		// Token: 0x06005292 RID: 21138 RVA: 0x001EC364 File Offset: 0x001EA564
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (NuzzleUtility.GetNuzzleMTBHours(pawn) <= 0f)
			{
				return null;
			}
			Pawn t;
			if (!(from p in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
			where !p.NonHumanlikeOrWildMan() && !p.IsSubhuman && p != pawn && p.Position.InHorDistOf(pawn.Position, 40f) && pawn.GetRoom(RegionType.Set_All) == p.GetRoom(RegionType.Set_All) && !p.Position.IsForbidden(pawn) && p.CanCasuallyInteractNow(false, false, false, false)
			select p).TryRandomElement(out t))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Nuzzle, t);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			job.expiryInterval = 3000;
			return job;
		}

		// Token: 0x040031DC RID: 12764
		private const float MaxNuzzleDistance = 40f;
	}
}
