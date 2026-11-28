using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C83 RID: 3203
	public class LordJob_SightstealerSwarm : LordJob_EntitySwarm
	{
		// Token: 0x06005AC2 RID: 23234 RVA: 0x00216203 File Offset: 0x00214403
		public LordJob_SightstealerSwarm()
		{
		}

		// Token: 0x06005AC3 RID: 23235 RVA: 0x0021620B File Offset: 0x0021440B
		public LordJob_SightstealerSwarm(IntVec3 startPos, IntVec3 destPos) : base(startPos, destPos)
		{
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x00216215 File Offset: 0x00214415
		protected override LordToil CreateTravelingToil(IntVec3 start, IntVec3 dest)
		{
			return new LordToil_SightstealerSwarm(start, dest);
		}
	}
}
