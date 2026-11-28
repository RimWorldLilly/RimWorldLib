using System;

namespace Verse
{
	// Token: 0x020001D9 RID: 473
	[Flags]
	public enum WorkTags
	{
		// Token: 0x04000D56 RID: 3414
		None = 0,
		// Token: 0x04000D57 RID: 3415
		ManualDumb = 2,
		// Token: 0x04000D58 RID: 3416
		ManualSkilled = 4,
		// Token: 0x04000D59 RID: 3417
		Violent = 8,
		// Token: 0x04000D5A RID: 3418
		Caring = 16,
		// Token: 0x04000D5B RID: 3419
		Social = 32,
		// Token: 0x04000D5C RID: 3420
		Commoner = 64,
		// Token: 0x04000D5D RID: 3421
		Intellectual = 128,
		// Token: 0x04000D5E RID: 3422
		Animals = 256,
		// Token: 0x04000D5F RID: 3423
		Artistic = 512,
		// Token: 0x04000D60 RID: 3424
		Crafting = 1024,
		// Token: 0x04000D61 RID: 3425
		Cooking = 2048,
		// Token: 0x04000D62 RID: 3426
		Firefighting = 4096,
		// Token: 0x04000D63 RID: 3427
		Cleaning = 8192,
		// Token: 0x04000D64 RID: 3428
		Hauling = 16384,
		// Token: 0x04000D65 RID: 3429
		PlantWork = 32768,
		// Token: 0x04000D66 RID: 3430
		Mining = 65536,
		// Token: 0x04000D67 RID: 3431
		Hunting = 131072,
		// Token: 0x04000D68 RID: 3432
		Constructing = 262144,
		// Token: 0x04000D69 RID: 3433
		Shooting = 524288,
		// Token: 0x04000D6A RID: 3434
		AllWork = 1048576
	}
}
