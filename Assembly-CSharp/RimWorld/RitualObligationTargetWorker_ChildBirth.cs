using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016A8 RID: 5800
	public class RitualObligationTargetWorker_ChildBirth : RitualObligationTargetFilter
	{
		// Token: 0x06009151 RID: 37201 RVA: 0x00353591 File Offset: 0x00351791
		public RitualObligationTargetWorker_ChildBirth()
		{
		}

		// Token: 0x06009152 RID: 37202 RVA: 0x00353599 File Offset: 0x00351799
		public RitualObligationTargetWorker_ChildBirth(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06009153 RID: 37203 RVA: 0x003535A2 File Offset: 0x003517A2
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_ChildBirth.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_ChildBirth.<GetTargets>d__2(-2);
			<GetTargets>d__.<>4__this = this;
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x06009154 RID: 37204 RVA: 0x003535BC File Offset: 0x003517BC
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Building_Bed building_Bed = target.Thing as Building_Bed;
			if (building_Bed == null)
			{
				return false;
			}
			if (!building_Bed.def.building.bed_humanlike)
			{
				return false;
			}
			if (this.def.colonistThingsOnly && (target.Thing.Faction == null || !target.Thing.Faction.IsPlayer))
			{
				return false;
			}
			foreach (Pawn pawn in this.ColonistsInLabor(target.Map))
			{
				Lord lord = pawn.GetLord();
				if (!(((lord != null) ? lord.LordJob : null) is LordJob_Ritual_ChildBirth) && PregnancyUtility.IsUsableBedFor(pawn, pawn, building_Bed))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009155 RID: 37205 RVA: 0x003536B4 File Offset: 0x003518B4
		protected IEnumerable<Pawn> ColonistsInLabor(Map map)
		{
			RitualObligationTargetWorker_ChildBirth.<ColonistsInLabor>d__6 <ColonistsInLabor>d__ = new RitualObligationTargetWorker_ChildBirth.<ColonistsInLabor>d__6(-2);
			<ColonistsInLabor>d__.<>3__map = map;
			return <ColonistsInLabor>d__;
		}

		// Token: 0x06009156 RID: 37206 RVA: 0x003536C4 File Offset: 0x003518C4
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			return new RitualObligationTargetWorker_ChildBirth.<GetTargetInfos>d__7(-2);
		}

		// Token: 0x0400585B RID: 22619
		private static readonly List<Pawn> cachedFreeColonistsAndPrisoners = new List<Pawn>();

		// Token: 0x0400585C RID: 22620
		private static int lastCacheTick = -1;
	}
}
