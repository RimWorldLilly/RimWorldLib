using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001729 RID: 5929
	public class RitualTargetFilter_IdeoBuilding : RitualTargetFilter
	{
		// Token: 0x060093DC RID: 37852 RVA: 0x0035E9DF File Offset: 0x0035CBDF
		public RitualTargetFilter_IdeoBuilding()
		{
		}

		// Token: 0x060093DD RID: 37853 RVA: 0x0035E9F2 File Offset: 0x0035CBF2
		public RitualTargetFilter_IdeoBuilding(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x060093DE RID: 37854 RVA: 0x0035EA08 File Offset: 0x0035CC08
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			TargetInfo targetInfo = this.BestTarget(initiator, selectedTarget);
			rejectionReason = "";
			if (!targetInfo.IsValid)
			{
				rejectionReason = "AbilitySpeechDisabledNoSpot".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x060093DF RID: 37855 RVA: 0x0035EA44 File Offset: 0x0035CC44
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			Pawn pawn = (Pawn)initiator.Thing;
			Ideo ideo = pawn.Ideo;
			this.candidateSpots.Clear();
			for (int i = 0; i < ideo.PreceptsListForReading.Count; i++)
			{
				Precept_Building precept_Building = ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null)
				{
					Thing thing = precept_Building.presenceDemand.BestBuilding(pawn.Map, false);
					if (thing != null && pawn.CanReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						this.candidateSpots.Add(thing);
					}
				}
			}
			this.candidateSpots.AddRange(this.ExtraCandidates(initiator));
			if (!this.candidateSpots.NullOrEmpty<Thing>())
			{
				return this.candidateSpots.RandomElement<Thing>();
			}
			if (!this.def.fallBackToGatherSpot)
			{
				return TargetInfo.Invalid;
			}
			this.candidateSpots.Clear();
			for (int j = 0; j < pawn.Map.gatherSpotLister.activeSpots.Count; j++)
			{
				ThingWithComps parent = pawn.Map.gatherSpotLister.activeSpots[j].parent;
				if (pawn.CanReach(parent, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
				{
					this.candidateSpots.Add(parent);
				}
			}
			if (!this.candidateSpots.NullOrEmpty<Thing>())
			{
				return (from s in this.candidateSpots
				orderby s.Position.DistanceTo(pawn.Position)
				select s).First<Thing>();
			}
			return TargetInfo.Invalid;
		}

		// Token: 0x060093E0 RID: 37856 RVA: 0x0035EBF6 File Offset: 0x0035CDF6
		protected virtual IEnumerable<Thing> ExtraCandidates(TargetInfo initiator)
		{
			return Enumerable.Empty<Thing>();
		}

		// Token: 0x060093E1 RID: 37857 RVA: 0x0035EBFD File Offset: 0x0035CDFD
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			RitualTargetFilter_IdeoBuilding.<GetTargetInfos>d__6 <GetTargetInfos>d__ = new RitualTargetFilter_IdeoBuilding.<GetTargetInfos>d__6(-2);
			<GetTargetInfos>d__.<>3__initiator = initiator;
			return <GetTargetInfos>d__;
		}

		// Token: 0x0400595A RID: 22874
		private List<Thing> candidateSpots = new List<Thing>();
	}
}
