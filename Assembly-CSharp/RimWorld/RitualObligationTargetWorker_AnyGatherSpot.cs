using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001699 RID: 5785
	public class RitualObligationTargetWorker_AnyGatherSpot : RitualObligationTargetFilter
	{
		// Token: 0x06009105 RID: 37125 RVA: 0x00352865 File Offset: 0x00350A65
		public RitualObligationTargetWorker_AnyGatherSpot()
		{
		}

		// Token: 0x06009106 RID: 37126 RVA: 0x0035286D File Offset: 0x00350A6D
		public RitualObligationTargetWorker_AnyGatherSpot(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009107 RID: 37127 RVA: 0x00352876 File Offset: 0x00350A76
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_AnyGatherSpot.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_AnyGatherSpot.<GetTargets>d__2(-2);
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x06009108 RID: 37128 RVA: 0x00352888 File Offset: 0x00350A88
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
			CompGatherSpot compGatherSpot = thing.TryGetComp<CompGatherSpot>();
			if (compGatherSpot != null && compGatherSpot.Active)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06009109 RID: 37129 RVA: 0x00352909 File Offset: 0x00350B09
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			return new RitualObligationTargetWorker_AnyGatherSpot.<GetTargetInfos>d__4(-2);
		}
	}
}
