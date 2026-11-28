using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001698 RID: 5784
	public class RitualObligationTargetWorker_AnyEmptyGrave : RitualObligationTargetFilter
	{
		// Token: 0x060090FE RID: 37118 RVA: 0x00352796 File Offset: 0x00350996
		public RitualObligationTargetWorker_AnyEmptyGrave()
		{
		}

		// Token: 0x060090FF RID: 37119 RVA: 0x0035279E File Offset: 0x0035099E
		public RitualObligationTargetWorker_AnyEmptyGrave(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009100 RID: 37120 RVA: 0x003527A7 File Offset: 0x003509A7
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_AnyEmptyGrave.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_AnyEmptyGrave.<GetTargets>d__2(-2);
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x06009101 RID: 37121 RVA: 0x003527B8 File Offset: 0x003509B8
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			bool canUse;
			if (target.HasThing)
			{
				Building_Grave building_Grave = target.Thing as Building_Grave;
				if (building_Grave != null)
				{
					canUse = (building_Grave.Corpse == null);
					goto IL_25;
				}
			}
			canUse = false;
			IL_25:
			return canUse;
		}

		// Token: 0x06009102 RID: 37122 RVA: 0x003527F0 File Offset: 0x003509F0
		public override bool ObligationTargetsValid(RitualObligation obligation)
		{
			Pawn pawn = obligation.targetA.Thing as Pawn;
			Corpse corpse = (pawn != null) ? pawn.Corpse : null;
			return obligation.targetA.HasThing && (corpse == null || !corpse.Destroyed);
		}

		// Token: 0x06009103 RID: 37123 RVA: 0x00352837 File Offset: 0x00350A37
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			RitualObligationTargetWorker_AnyEmptyGrave.<GetTargetInfos>d__5 <GetTargetInfos>d__ = new RitualObligationTargetWorker_AnyEmptyGrave.<GetTargetInfos>d__5(-2);
			<GetTargetInfos>d__.<>4__this = this;
			<GetTargetInfos>d__.<>3__obligation = obligation;
			return <GetTargetInfos>d__;
		}

		// Token: 0x06009104 RID: 37124 RVA: 0x0035284E File Offset: 0x00350A4E
		public override string LabelExtraPart(RitualObligation obligation)
		{
			return ((Pawn)obligation.targetA.Thing).LabelShort;
		}
	}
}
