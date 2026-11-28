using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CE9 RID: 3305
	public class LordToil_MetalhorrorActive : LordToil
	{
		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06005D50 RID: 23888 RVA: 0x0022363D File Offset: 0x0022183D
		public override bool AssignsDuties
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06005D51 RID: 23889 RVA: 0x00223640 File Offset: 0x00221840
		public override bool AllowAggressiveTargetingOfRoamers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x00223643 File Offset: 0x00221843
		public override void UpdateAllDuties()
		{
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x00223645 File Offset: 0x00221845
		public override void LordToilTick()
		{
		}

		// Token: 0x0400350A RID: 13578
		private const int UpdateIntervalTicks = 300;
	}
}
