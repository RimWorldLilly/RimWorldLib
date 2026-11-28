using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B5A RID: 2906
	public class JobGiver_PrisonerEscape : ThinkNode_JobGiver
	{
		// Token: 0x060054DF RID: 21727 RVA: 0x001F9408 File Offset: 0x001F7608
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (this.ShouldStartEscaping(pawn) && RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn, true))
			{
				if (!pawn.guest.Released)
				{
					Messages.Message("MessagePrisonerIsEscaping".Translate(pawn.LabelShort, pawn), pawn, MessageTypeDefOf.ThreatSmall, true);
					Find.TickManager.slower.SignalForceNormalSpeed();
				}
				Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
				job.exitMapOnArrival = true;
				return job;
			}
			return null;
		}

		// Token: 0x060054E0 RID: 21728 RVA: 0x001F9490 File Offset: 0x001F7690
		private bool ShouldStartEscaping(Pawn pawn)
		{
			if (!pawn.guest.IsPrisoner || pawn.guest.HostFaction != Faction.OfPlayer || !pawn.guest.PrisonerIsSecure)
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.PrisonerSecure(pawn))
			{
				return false;
			}
			District district = pawn.GetDistrict(RegionType.Set_Passable);
			if (district.TouchesMapEdge)
			{
				return true;
			}
			bool found = false;
			RegionTraverser.BreadthFirstTraverse(district.Regions[0], (Region from, Region reg) => reg.door == null || reg.door.FreePassage, delegate(Region reg)
			{
				if (reg.District.TouchesMapEdge)
				{
					found = true;
					return true;
				}
				return false;
			}, 25, RegionType.Set_Passable);
			return found;
		}

		// Token: 0x040032B1 RID: 12977
		private const int MaxRegionsToCheckWhenEscapingThroughOpenDoors = 25;
	}
}
