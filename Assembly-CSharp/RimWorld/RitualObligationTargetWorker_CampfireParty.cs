using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016A3 RID: 5795
	public class RitualObligationTargetWorker_CampfireParty : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x06009138 RID: 37176 RVA: 0x00352FA9 File Offset: 0x003511A9
		public RitualObligationTargetWorker_CampfireParty()
		{
		}

		// Token: 0x06009139 RID: 37177 RVA: 0x00352FB1 File Offset: 0x003511B1
		public RitualObligationTargetWorker_CampfireParty(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600913A RID: 37178 RVA: 0x00352FBC File Offset: 0x003511BC
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!ModLister.CheckIdeology("Campfire party"))
			{
				return false;
			}
			if (!base.CanUseTargetInternal(target, obligation).canUse)
			{
				return false;
			}
			Thing thing = target.Thing;
			CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
			if (compRefuelable != null && !compRefuelable.HasFuel)
			{
				return "RitualTargetCampfireNoFuel".Translate();
			}
			List<Thing> forCell = target.Map.listerBuldingOfDefInProximity.GetForCell(target.Cell, (float)this.def.maxDrumDistance, ThingDefOf.Drum, null);
			bool flag = false;
			using (List<Thing>.Enumerator enumerator = forCell.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetRoom(RegionType.Set_All) == thing.GetRoom(RegionType.Set_All))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return "RitualTargetNoDrum".Translate();
			}
			return true;
		}

		// Token: 0x0600913B RID: 37179 RVA: 0x003530B0 File Offset: 0x003512B0
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			return new RitualObligationTargetWorker_CampfireParty.<GetTargetInfos>d__3(-2);
		}
	}
}
