using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200094A RID: 2378
	public class LordToil_ExitMapFighting : LordToil_ExitMap
	{
		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06004915 RID: 18709 RVA: 0x001C929D File Offset: 0x001C749D
		public override DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x001C92A4 File Offset: 0x001C74A4
		public LordToil_ExitMapFighting(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false) : base(locomotion, canDig, interruptCurrentJob)
		{
		}
	}
}
