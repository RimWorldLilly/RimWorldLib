using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200172A RID: 5930
	public class RitualTargetFilter_IdeoBuildingOrRitualSpot : RitualTargetFilter_IdeoBuilding
	{
		// Token: 0x060093E2 RID: 37858 RVA: 0x0035EC0D File Offset: 0x0035CE0D
		public RitualTargetFilter_IdeoBuildingOrRitualSpot()
		{
		}

		// Token: 0x060093E3 RID: 37859 RVA: 0x0035EC15 File Offset: 0x0035CE15
		public RitualTargetFilter_IdeoBuildingOrRitualSpot(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x060093E4 RID: 37860 RVA: 0x0035EC20 File Offset: 0x0035CE20
		protected override IEnumerable<Thing> ExtraCandidates(TargetInfo initiator)
		{
			Pawn pawn = (Pawn)initiator.Thing;
			return from s in initiator.Map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot)
			where pawn.CanReach(s, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn)
			select s;
		}

		// Token: 0x060093E5 RID: 37861 RVA: 0x0035EC6C File Offset: 0x0035CE6C
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			RitualTargetFilter_IdeoBuildingOrRitualSpot.<GetTargetInfos>d__3 <GetTargetInfos>d__ = new RitualTargetFilter_IdeoBuildingOrRitualSpot.<GetTargetInfos>d__3(-2);
			<GetTargetInfos>d__.<>4__this = this;
			<GetTargetInfos>d__.<>3__initiator = initiator;
			return <GetTargetInfos>d__;
		}
	}
}
