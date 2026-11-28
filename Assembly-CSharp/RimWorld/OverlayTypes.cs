using System;

namespace RimWorld
{
	// Token: 0x02001A62 RID: 6754
	[Flags]
	public enum OverlayTypes
	{
		// Token: 0x04006336 RID: 25398
		None = 0,
		// Token: 0x04006337 RID: 25399
		NeedsPower = 1,
		// Token: 0x04006338 RID: 25400
		PowerOff = 2,
		// Token: 0x04006339 RID: 25401
		BurningWick = 4,
		// Token: 0x0400633A RID: 25402
		Forbidden = 8,
		// Token: 0x0400633B RID: 25403
		ForbiddenBig = 16,
		// Token: 0x0400633C RID: 25404
		QuestionMark = 32,
		// Token: 0x0400633D RID: 25405
		BrokenDown = 64,
		// Token: 0x0400633E RID: 25406
		OutOfFuel = 128,
		// Token: 0x0400633F RID: 25407
		ForbiddenRefuel = 256,
		// Token: 0x04006340 RID: 25408
		SelfShutdown = 512,
		// Token: 0x04006341 RID: 25409
		ForbiddenAtomizer = 1024
	}
}
