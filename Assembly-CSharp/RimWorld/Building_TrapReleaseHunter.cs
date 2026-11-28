using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A6D RID: 6765
	public class Building_TrapReleaseHunter : Building_TrapReleaseEntity
	{
		// Token: 0x17001DE8 RID: 7656
		// (get) Token: 0x0600AA11 RID: 43537 RVA: 0x003C4312 File Offset: 0x003C2512
		protected override int CountToSpawn
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001DE9 RID: 7657
		// (get) Token: 0x0600AA12 RID: 43538 RVA: 0x003C4315 File Offset: 0x003C2515
		protected override PawnKindDef PawnToSpawn
		{
			get
			{
				return PawnKindDefOf.Drone_Hunter;
			}
		}
	}
}
