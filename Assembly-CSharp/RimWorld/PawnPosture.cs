using System;

namespace RimWorld
{
	// Token: 0x020014EF RID: 5359
	[Flags]
	public enum PawnPosture : byte
	{
		// Token: 0x0400512C RID: 20780
		Standing = 0,
		// Token: 0x0400512D RID: 20781
		LayingOnGroundFaceUp = 3,
		// Token: 0x0400512E RID: 20782
		LayingOnGroundNormal = 1,
		// Token: 0x0400512F RID: 20783
		LayingInBed = 5,
		// Token: 0x04005130 RID: 20784
		LayingInBedFaceUp = 7,
		// Token: 0x04005131 RID: 20785
		LayingMask = 1,
		// Token: 0x04005132 RID: 20786
		FaceUpMask = 2,
		// Token: 0x04005133 RID: 20787
		InBedMask = 4
	}
}
