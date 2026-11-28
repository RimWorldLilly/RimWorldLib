using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B44 RID: 2884
	public abstract class JobGiver_GetEnergy : ThinkNode_JobGiver
	{
		// Token: 0x06005482 RID: 21634 RVA: 0x001F6678 File Offset: 0x001F4878
		public static int GetMinAutorechargeThreshold(Pawn pawn)
		{
			int num = pawn.RaceProps.maxMechEnergy;
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			if (JobGiver_GetEnergy.UseGroupRechargeLimits(mechControlGroup))
			{
				num = Mathf.RoundToInt((float)num * mechControlGroup.mechRechargeThresholds.min);
			}
			return num;
		}

		// Token: 0x06005483 RID: 21635 RVA: 0x001F66B8 File Offset: 0x001F48B8
		public static float GetMaxRechargeLimit(Pawn pawn)
		{
			int num = pawn.RaceProps.maxMechEnergy;
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			if (JobGiver_GetEnergy.UseGroupRechargeLimits(mechControlGroup))
			{
				num = Mathf.RoundToInt((float)num * mechControlGroup.mechRechargeThresholds.max);
			}
			return (float)num;
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x001F66F6 File Offset: 0x001F48F6
		private static bool UseGroupRechargeLimits(MechanitorControlGroup controlGroup)
		{
			return controlGroup != null && controlGroup.WorkMode != null && !controlGroup.WorkMode.ignoreGroupChargeLimits;
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x001F6714 File Offset: 0x001F4914
		protected virtual bool ShouldAutoRecharge(Pawn pawn)
		{
			Need_MechEnergy energy = pawn.needs.energy;
			return energy != null && (this.forced || energy.CurLevel + 0.1f < (float)JobGiver_GetEnergy.GetMinAutorechargeThreshold(pawn));
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x001F6751 File Offset: 0x001F4951
		public override float GetPriority(Pawn pawn)
		{
			if (!this.ShouldAutoRecharge(pawn))
			{
				return 0f;
			}
			return 9.5f;
		}

		// Token: 0x04003285 RID: 12933
		public const int DefaultEnergyLevelThreshold = 15;

		// Token: 0x04003286 RID: 12934
		private const float MinAutoRechargeDelta = 0.1f;

		// Token: 0x04003287 RID: 12935
		public bool forced;
	}
}
