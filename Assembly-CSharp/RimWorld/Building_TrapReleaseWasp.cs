using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A6E RID: 6766
	public class Building_TrapReleaseWasp : Building_TrapReleaseEntity
	{
		// Token: 0x17001DEA RID: 7658
		// (get) Token: 0x0600AA14 RID: 43540 RVA: 0x003C4324 File Offset: 0x003C2524
		protected override int CountToSpawn
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001DEB RID: 7659
		// (get) Token: 0x0600AA15 RID: 43541 RVA: 0x003C4327 File Offset: 0x003C2527
		protected override PawnKindDef PawnToSpawn
		{
			get
			{
				return PawnKindDefOf.Drone_Wasp;
			}
		}
	}
}
