using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016A4 RID: 5796
	public class RitualObligationTargetWorker_SkyLanterns : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x0600913C RID: 37180 RVA: 0x003530B9 File Offset: 0x003512B9
		public RitualObligationTargetWorker_SkyLanterns()
		{
		}

		// Token: 0x0600913D RID: 37181 RVA: 0x003530C1 File Offset: 0x003512C1
		public RitualObligationTargetWorker_SkyLanterns(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600913E RID: 37182 RVA: 0x003530CA File Offset: 0x003512CA
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_SkyLanterns.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_SkyLanterns.<GetTargets>d__2(-2);
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x0600913F RID: 37183 RVA: 0x003530DC File Offset: 0x003512DC
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			Thing thing = target.Thing;
			if (thing.def != ThingDefOf.RitualSpot && thing.def != ThingDefOf.Campfire)
			{
				return false;
			}
			Room room = thing.GetRoom(RegionType.Set_All);
			if (this.def.colonistThingsOnly && (thing.Faction == null || !thing.Faction.IsPlayer))
			{
				return false;
			}
			int num = 0;
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(target.Cell, (float)this.def.unroofedCellSearchRadius, false))
			{
				if (intVec.InBounds(target.Map) && !intVec.Roofed(target.Map) && intVec.GetRoom(thing.Map) == room)
				{
					num++;
					if (num >= this.def.minUnroofedCells)
					{
						break;
					}
				}
			}
			if (num < this.def.minUnroofedCells)
			{
				return "RitualTargetNeedUnroofedCells".Translate(this.def.minUnroofedCells);
			}
			if (thing.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			if (thing.def == ThingDefOf.Campfire)
			{
				if (target.Cell.Roofed(target.Map))
				{
					return "RitualTargetCampfireMustBeUnroofed".Translate();
				}
				CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
				if (compRefuelable != null)
				{
					if (compRefuelable.HasFuel)
					{
						return true;
					}
					return "RitualTargetCampfireNoFuel".Translate();
				}
			}
			return false;
		}

		// Token: 0x06009140 RID: 37184 RVA: 0x00353280 File Offset: 0x00351480
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			return new RitualObligationTargetWorker_SkyLanterns.<GetTargetInfos>d__4(-2);
		}

		// Token: 0x06009141 RID: 37185 RVA: 0x0035328C File Offset: 0x0035148C
		public override bool ShouldGrayOut(Pawn pawn, ILordJobAssignmentsManager<RitualRole> assignments, out TaggedString reason)
		{
			if (RitualObligationTargetWorker_SkyLanterns.blocked.Contains(pawn) && assignments.PawnParticipating(pawn))
			{
				reason = "RitualTargetWoodInfoWarning".Translate(pawn.Named("PAWN"));
				return true;
			}
			reason = TaggedString.Empty;
			return false;
		}

		// Token: 0x06009142 RID: 37186 RVA: 0x003532D8 File Offset: 0x003514D8
		public override IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
		{
			RitualObligationTargetWorker_SkyLanterns.<GetBlockingIssues>d__7 <GetBlockingIssues>d__ = new RitualObligationTargetWorker_SkyLanterns.<GetBlockingIssues>d__7(-2);
			<GetBlockingIssues>d__.<>4__this = this;
			<GetBlockingIssues>d__.<>3__target = target;
			<GetBlockingIssues>d__.<>3__assignments = assignments;
			return <GetBlockingIssues>d__;
		}

		// Token: 0x0400585A RID: 22618
		private static readonly HashSet<Pawn> blocked = new HashSet<Pawn>();
	}
}
