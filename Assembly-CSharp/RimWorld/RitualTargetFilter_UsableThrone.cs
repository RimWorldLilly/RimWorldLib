using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001728 RID: 5928
	public class RitualTargetFilter_UsableThrone : RitualTargetFilter
	{
		// Token: 0x060093D7 RID: 37847 RVA: 0x0035E8E8 File Offset: 0x0035CAE8
		public RitualTargetFilter_UsableThrone()
		{
		}

		// Token: 0x060093D8 RID: 37848 RVA: 0x0035E8F0 File Offset: 0x0035CAF0
		public RitualTargetFilter_UsableThrone(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x060093D9 RID: 37849 RVA: 0x0035E8FC File Offset: 0x0035CAFC
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			Pawn pawn = initiator.Thing as Pawn;
			rejectionReason = "";
			if (pawn == null)
			{
				return false;
			}
			Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(pawn) ?? pawn.ownership.AssignedThrone;
			if (building_Throne == null)
			{
				rejectionReason = "AbilitySpeechDisabledNoThroneAssigned".Translate();
				return false;
			}
			if (!pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				rejectionReason = "AbilitySpeechDisabledNoThroneIsNotAccessible".Translate();
				return false;
			}
			if (pawn.royalty.AnyUnmetThroneroomRequirements())
			{
				rejectionReason = "AbilitySpeechDisabledNoThroneUndignified".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x060093DA RID: 37850 RVA: 0x0035E99C File Offset: 0x0035CB9C
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone((Pawn)initiator.Thing);
			if (building_Throne == null)
			{
				return TargetInfo.Invalid;
			}
			return new TargetInfo(building_Throne.InteractionCell, building_Throne.Map, false);
		}

		// Token: 0x060093DB RID: 37851 RVA: 0x0035E9D6 File Offset: 0x0035CBD6
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			return new RitualTargetFilter_UsableThrone.<GetTargetInfos>d__4(-2);
		}
	}
}
