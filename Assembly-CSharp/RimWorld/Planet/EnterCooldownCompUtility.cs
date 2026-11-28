using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002360 RID: 9056
	public static class EnterCooldownCompUtility
	{
		// Token: 0x0600E07C RID: 57468 RVA: 0x005378F8 File Offset: 0x00535AF8
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x0600E07D RID: 57469 RVA: 0x00537917 File Offset: 0x00535B17
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			if (component == null)
			{
				return 0f;
			}
			return component.DaysLeft;
		}

		// Token: 0x0600E07E RID: 57470 RVA: 0x00537930 File Offset: 0x00535B30
		public static float EnterCooldownHoursLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			if (component == null)
			{
				return 0f;
			}
			return component.DaysLeft * 24f;
		}

		// Token: 0x0600E07F RID: 57471 RVA: 0x0053795C File Offset: 0x00535B5C
		public static int EnterCooldownTicksLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return Mathf.CeilToInt((component != null) ? (component.DaysLeft * 60000f) : 0f);
		}
	}
}
