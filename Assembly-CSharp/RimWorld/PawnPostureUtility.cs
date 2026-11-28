using System;

namespace RimWorld
{
	// Token: 0x020014F0 RID: 5360
	public static class PawnPostureUtility
	{
		// Token: 0x06008318 RID: 33560 RVA: 0x002F12F8 File Offset: 0x002EF4F8
		public static bool Laying(this PawnPosture posture)
		{
			return (posture & PawnPosture.LayingOnGroundNormal) > PawnPosture.Standing;
		}

		// Token: 0x06008319 RID: 33561 RVA: 0x002F1300 File Offset: 0x002EF500
		public static bool InBed(this PawnPosture posture)
		{
			return (posture & PawnPosture.InBedMask) > PawnPosture.Standing;
		}

		// Token: 0x0600831A RID: 33562 RVA: 0x002F1308 File Offset: 0x002EF508
		public static bool FaceUp(this PawnPosture posture)
		{
			return (posture & PawnPosture.FaceUpMask) > PawnPosture.Standing;
		}
	}
}
