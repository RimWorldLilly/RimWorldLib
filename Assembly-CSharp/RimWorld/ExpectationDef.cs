using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F53 RID: 3923
	public class ExpectationDef : Def
	{
		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x060064F8 RID: 25848 RVA: 0x002405D3 File Offset: 0x0023E7D3
		public bool WealthTriggered
		{
			get
			{
				return this.maxMapWealth >= 0f;
			}
		}

		// Token: 0x060064F9 RID: 25849 RVA: 0x002405E5 File Offset: 0x0023E7E5
		public override IEnumerable<string> ConfigErrors()
		{
			ExpectationDef.<ConfigErrors>d__9 <ConfigErrors>d__ = new ExpectationDef.<ConfigErrors>d__9(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003ADD RID: 15069
		public int order = -1;

		// Token: 0x04003ADE RID: 15070
		public int thoughtStage = -1;

		// Token: 0x04003ADF RID: 15071
		public float maxMapWealth = -1f;

		// Token: 0x04003AE0 RID: 15072
		public float joyToleranceDropPerDay;

		// Token: 0x04003AE1 RID: 15073
		public float ritualQualityOffset;

		// Token: 0x04003AE2 RID: 15074
		public int joyKindsNeeded;

		// Token: 0x04003AE3 RID: 15075
		public bool forRoles;
	}
}
