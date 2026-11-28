using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001697 RID: 5783
	public class RitualObligationTargetWorker_GraveWithTarget : RitualObligationTargetFilter
	{
		// Token: 0x060090F7 RID: 37111 RVA: 0x00352666 File Offset: 0x00350866
		public RitualObligationTargetWorker_GraveWithTarget()
		{
		}

		// Token: 0x060090F8 RID: 37112 RVA: 0x0035266E File Offset: 0x0035086E
		public RitualObligationTargetWorker_GraveWithTarget(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x060090F9 RID: 37113 RVA: 0x00352677 File Offset: 0x00350877
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_GraveWithTarget.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_GraveWithTarget.<GetTargets>d__2(-2);
			<GetTargets>d__.<>3__obligation = obligation;
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x060090FA RID: 37114 RVA: 0x00352690 File Offset: 0x00350890
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			bool canUse;
			if (target.HasThing)
			{
				Building_Grave building_Grave = target.Thing as Building_Grave;
				if (building_Grave != null)
				{
					if (building_Grave.Corpse != obligation.targetA.Thing)
					{
						Corpse corpse = building_Grave.Corpse;
						canUse = (((corpse != null) ? corpse.InnerPawn : null) == obligation.targetA.Thing);
						goto IL_51;
					}
					canUse = true;
					goto IL_51;
				}
			}
			canUse = false;
			IL_51:
			return canUse;
		}

		// Token: 0x060090FB RID: 37115 RVA: 0x003526F4 File Offset: 0x003508F4
		public override bool ObligationTargetsValid(RitualObligation obligation)
		{
			Pawn pawn = obligation.targetA.Thing as Pawn;
			Corpse corpse = (pawn != null) ? pawn.Corpse : null;
			return obligation.targetA.HasThing && (corpse == null || !corpse.Destroyed);
		}

		// Token: 0x060090FC RID: 37116 RVA: 0x0035273B File Offset: 0x0035093B
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			RitualObligationTargetWorker_GraveWithTarget.<GetTargetInfos>d__5 <GetTargetInfos>d__ = new RitualObligationTargetWorker_GraveWithTarget.<GetTargetInfos>d__5(-2);
			<GetTargetInfos>d__.<>4__this = this;
			<GetTargetInfos>d__.<>3__obligation = obligation;
			return <GetTargetInfos>d__;
		}

		// Token: 0x060090FD RID: 37117 RVA: 0x00352754 File Offset: 0x00350954
		public override string LabelExtraPart(RitualObligation obligation)
		{
			Pawn pawn = obligation.targetA.Thing as Pawn;
			if (pawn != null)
			{
				return pawn.LabelShort;
			}
			return ((Corpse)obligation.targetA.Thing).InnerPawn.LabelShort;
		}
	}
}
