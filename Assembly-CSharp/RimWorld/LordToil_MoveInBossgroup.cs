using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CEA RID: 3306
	public class LordToil_MoveInBossgroup : LordToil
	{
		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06005D55 RID: 23893 RVA: 0x0022364F File Offset: 0x0022184F
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06005D56 RID: 23894 RVA: 0x00223652 File Offset: 0x00221852
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x00223655 File Offset: 0x00221855
		public LordToil_MoveInBossgroup(IEnumerable<Pawn> bosses)
		{
			this.bosses.AddRange(bosses);
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x00223674 File Offset: 0x00221874
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.bosses.Contains(this.lord.ownedPawns[i]))
				{
					this.bosses[i].mindState.duty = new PawnDuty(this.bosses[i].RaceProps.dutyBoss ?? DutyDefOf.AssaultColony);
				}
				else
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Escort, this.bosses.RandomElement<Pawn>(), LordToil_MoveInBossgroup.EscortRadiusRanged.RandomInRange);
				}
			}
		}

		// Token: 0x0400350B RID: 13579
		private static readonly FloatRange EscortRadiusRanged = new FloatRange(5f, 10f);

		// Token: 0x0400350C RID: 13580
		private List<Pawn> bosses = new List<Pawn>();
	}
}
