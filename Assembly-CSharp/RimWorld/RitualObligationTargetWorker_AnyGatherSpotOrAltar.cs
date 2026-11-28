using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200169B RID: 5787
	public class RitualObligationTargetWorker_AnyGatherSpotOrAltar : RitualObligationTargetWorker_AnyGatherSpot
	{
		// Token: 0x06009113 RID: 37139 RVA: 0x00352A11 File Offset: 0x00350C11
		public RitualObligationTargetWorker_AnyGatherSpotOrAltar()
		{
		}

		// Token: 0x06009114 RID: 37140 RVA: 0x00352A19 File Offset: 0x00350C19
		public RitualObligationTargetWorker_AnyGatherSpotOrAltar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009115 RID: 37141 RVA: 0x00352A22 File Offset: 0x00350C22
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_AnyGatherSpotOrAltar.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_AnyGatherSpotOrAltar.<GetTargets>d__2(-2);
			<GetTargets>d__.<>4__this = this;
			<GetTargets>d__.<>3__obligation = obligation;
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x06009116 RID: 37142 RVA: 0x00352A40 File Offset: 0x00350C40
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Thing thing = target.Thing;
			if (this.def.colonistThingsOnly && (thing.Faction == null || !thing.Faction.IsPlayer))
			{
				return false;
			}
			if (thing.def == ThingDefOf.PartySpot)
			{
				return true;
			}
			if (thing.def == ThingDefOf.RitualSpot)
			{
				return true;
			}
			CompGatherSpot compGatherSpot = thing.TryGetComp<CompGatherSpot>();
			if (compGatherSpot != null && compGatherSpot.Active)
			{
				return true;
			}
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x06009117 RID: 37143 RVA: 0x00352AE6 File Offset: 0x00350CE6
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			RitualObligationTargetWorker_AnyGatherSpotOrAltar.<GetTargetInfos>d__4 <GetTargetInfos>d__ = new RitualObligationTargetWorker_AnyGatherSpotOrAltar.<GetTargetInfos>d__4(-2);
			<GetTargetInfos>d__.<>4__this = this;
			return <GetTargetInfos>d__;
		}
	}
}
