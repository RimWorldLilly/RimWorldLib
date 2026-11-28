using System;

namespace Verse
{
	// Token: 0x0200013E RID: 318
	[Flags]
	public enum LinkFlags
	{
		// Token: 0x040005C7 RID: 1479
		None = 0,
		// Token: 0x040005C8 RID: 1480
		MapEdge = 1,
		// Token: 0x040005C9 RID: 1481
		Rock = 2,
		// Token: 0x040005CA RID: 1482
		Wall = 4,
		// Token: 0x040005CB RID: 1483
		Sandbags = 8,
		// Token: 0x040005CC RID: 1484
		PowerConduit = 16,
		// Token: 0x040005CD RID: 1485
		Barricades = 32,
		// Token: 0x040005CE RID: 1486
		Fences = 64,
		// Token: 0x040005CF RID: 1487
		Fleshmass = 128,
		// Token: 0x040005D0 RID: 1488
		SolidIce = 256,
		// Token: 0x040005D1 RID: 1489
		BurrowWall = 512,
		// Token: 0x040005D2 RID: 1490
		Custom1 = 131072,
		// Token: 0x040005D3 RID: 1491
		Custom2 = 262144,
		// Token: 0x040005D4 RID: 1492
		Custom3 = 524288,
		// Token: 0x040005D5 RID: 1493
		Custom4 = 1048576,
		// Token: 0x040005D6 RID: 1494
		Custom5 = 2097152,
		// Token: 0x040005D7 RID: 1495
		Custom6 = 4194304,
		// Token: 0x040005D8 RID: 1496
		Custom7 = 8388608,
		// Token: 0x040005D9 RID: 1497
		Custom8 = 16777216,
		// Token: 0x040005DA RID: 1498
		Custom9 = 33554432,
		// Token: 0x040005DB RID: 1499
		Custom10 = 67108864
	}
}
