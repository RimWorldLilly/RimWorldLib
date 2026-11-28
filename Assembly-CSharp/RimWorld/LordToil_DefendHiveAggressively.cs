using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CD0 RID: 3280
	public class LordToil_DefendHiveAggressively : LordToil_HiveRelated
	{
		// Token: 0x06005CEB RID: 23787 RVA: 0x00221890 File Offset: 0x0021FA90
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Hive hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				if (hiveFor == null)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
				else
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendHiveAggressively, hiveFor, this.distToHiveToAttack);
				}
			}
		}

		// Token: 0x040034ED RID: 13549
		public float distToHiveToAttack = 40f;
	}
}
