using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0D RID: 3853
	public class MoteForRotationData
	{
		// Token: 0x0600640A RID: 25610 RVA: 0x0023D12C File Offset: 0x0023B32C
		public ThingDef GetForRotation(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.north;
			case 1:
				return this.east;
			case 2:
				return this.south;
			case 3:
				return this.west;
			default:
				return null;
			}
		}

		// Token: 0x040038D5 RID: 14549
		public ThingDef north;

		// Token: 0x040038D6 RID: 14550
		public ThingDef south;

		// Token: 0x040038D7 RID: 14551
		public ThingDef east;

		// Token: 0x040038D8 RID: 14552
		public ThingDef west;
	}
}
