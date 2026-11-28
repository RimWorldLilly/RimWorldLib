using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001695 RID: 5781
	public class RitualObligationTargetWorker_ThingDef : RitualObligationTargetFilter
	{
		// Token: 0x060090ED RID: 37101 RVA: 0x00352561 File Offset: 0x00350761
		public RitualObligationTargetWorker_ThingDef()
		{
		}

		// Token: 0x060090EE RID: 37102 RVA: 0x00352569 File Offset: 0x00350769
		public RitualObligationTargetWorker_ThingDef(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x060090EF RID: 37103 RVA: 0x00352572 File Offset: 0x00350772
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_ThingDef.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_ThingDef.<GetTargets>d__2(-2);
			<GetTargets>d__.<>4__this = this;
			<GetTargets>d__.<>3__obligation = obligation;
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x060090F0 RID: 37104 RVA: 0x00352590 File Offset: 0x00350790
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			return target.HasThing && this.def.thingDefs.Contains(target.Thing.def) && (!this.def.colonistThingsOnly || (target.Thing.Faction != null && target.Thing.Faction.IsPlayer));
		}

		// Token: 0x060090F1 RID: 37105 RVA: 0x003525FE File Offset: 0x003507FE
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			RitualObligationTargetWorker_ThingDef.<GetTargetInfos>d__4 <GetTargetInfos>d__ = new RitualObligationTargetWorker_ThingDef.<GetTargetInfos>d__4(-2);
			<GetTargetInfos>d__.<>4__this = this;
			return <GetTargetInfos>d__;
		}
	}
}
