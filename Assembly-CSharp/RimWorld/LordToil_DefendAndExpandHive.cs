using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CCC RID: 3276
	public class LordToil_DefendAndExpandHive : LordToil_HiveRelated
	{
		// Token: 0x06005CDD RID: 23773 RVA: 0x00221464 File Offset: 0x0021F664
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
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendAndExpandHive, hiveFor, this.distToHiveToAttack);
				}
			}
		}

		// Token: 0x040034E7 RID: 13543
		public float distToHiveToAttack = 10f;
	}
}
