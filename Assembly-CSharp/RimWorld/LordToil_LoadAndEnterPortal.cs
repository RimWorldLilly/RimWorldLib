using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CE6 RID: 3302
	public class LordToil_LoadAndEnterPortal : LordToil
	{
		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06005D48 RID: 23880 RVA: 0x002234DC File Offset: 0x002216DC
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x002234DF File Offset: 0x002216DF
		public LordToil_LoadAndEnterPortal(MapPortal portal)
		{
			this.portal = portal;
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x002234F0 File Offset: 0x002216F0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.LoadAndEnterPortal);
				pawnDuty.focus = new LocalTargetInfo(this.portal);
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04003508 RID: 13576
		public MapPortal portal;
	}
}
