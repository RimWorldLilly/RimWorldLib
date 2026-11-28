using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200169A RID: 5786
	public class RitualObligationTargetWorker_Altar : RitualObligationTargetFilter
	{
		// Token: 0x0600910A RID: 37130 RVA: 0x00352912 File Offset: 0x00350B12
		public RitualObligationTargetWorker_Altar()
		{
		}

		// Token: 0x0600910B RID: 37131 RVA: 0x0035291A File Offset: 0x00350B1A
		public RitualObligationTargetWorker_Altar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600910C RID: 37132 RVA: 0x00352923 File Offset: 0x00350B23
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			RitualObligationTargetWorker_Altar.<GetTargets>d__2 <GetTargets>d__ = new RitualObligationTargetWorker_Altar.<GetTargets>d__2(-2);
			<GetTargets>d__.<>4__this = this;
			<GetTargets>d__.<>3__obligation = obligation;
			<GetTargets>d__.<>3__map = map;
			return <GetTargets>d__;
		}

		// Token: 0x0600910D RID: 37133 RVA: 0x00352941 File Offset: 0x00350B41
		public static IEnumerable<TargetInfo> GetTargetsWorker(RitualObligation obligation, Map map, Ideo ideo)
		{
			RitualObligationTargetWorker_Altar.<GetTargetsWorker>d__3 <GetTargetsWorker>d__ = new RitualObligationTargetWorker_Altar.<GetTargetsWorker>d__3(-2);
			<GetTargetsWorker>d__.<>3__map = map;
			<GetTargetsWorker>d__.<>3__ideo = ideo;
			return <GetTargetsWorker>d__;
		}

		// Token: 0x0600910E RID: 37134 RVA: 0x00352958 File Offset: 0x00350B58
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x0600910F RID: 37135 RVA: 0x00352974 File Offset: 0x00350B74
		public static bool CanUseTargetWorker(TargetInfo target, RitualObligation obligation, Ideo ideo)
		{
			Building building = target.Thing as Building;
			return building != null && building.Faction != null && building.Faction.IsPlayer && RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, building.Map, ideo).Contains(building);
		}

		// Token: 0x06009110 RID: 37136 RVA: 0x003529C5 File Offset: 0x00350BC5
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			RitualObligationTargetWorker_Altar.<GetTargetInfos>d__6 <GetTargetInfos>d__ = new RitualObligationTargetWorker_Altar.<GetTargetInfos>d__6(-2);
			<GetTargetInfos>d__.<>4__this = this;
			return <GetTargetInfos>d__;
		}

		// Token: 0x06009111 RID: 37137 RVA: 0x003529D5 File Offset: 0x00350BD5
		public static IEnumerable<string> GetTargetInfosWorker(Ideo ideo)
		{
			RitualObligationTargetWorker_Altar.<GetTargetInfosWorker>d__7 <GetTargetInfosWorker>d__ = new RitualObligationTargetWorker_Altar.<GetTargetInfosWorker>d__7(-2);
			<GetTargetInfosWorker>d__.<>3__ideo = ideo;
			return <GetTargetInfosWorker>d__;
		}

		// Token: 0x06009112 RID: 37138 RVA: 0x003529E5 File Offset: 0x00350BE5
		public override List<string> MissingTargetBuilding(Ideo ideo)
		{
			if (!this.GetTargetInfos(null).Any<string>())
			{
				return new List<string>
				{
					"Altar".Translate()
				};
			}
			return null;
		}
	}
}
