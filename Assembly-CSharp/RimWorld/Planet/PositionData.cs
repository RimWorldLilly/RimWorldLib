using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F0 RID: 8944
	public class PositionData : IExposable
	{
		// Token: 0x0600DCF2 RID: 56562 RVA: 0x00523F62 File Offset: 0x00522162
		public PositionData()
		{
		}

		// Token: 0x0600DCF3 RID: 56563 RVA: 0x00523F6C File Offset: 0x0052216C
		public PositionData(IntVec3 position, Rot4 rotation, bool drafted = false)
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (rotation == Rot4.East)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
			else if (rotation == Rot4.South)
			{
				rotationDirection = RotationDirection.Opposite;
			}
			else if (rotation == Rot4.West)
			{
				rotationDirection = RotationDirection.Counterclockwise;
			}
			this.relativeRotation = rotationDirection;
			this.position = position;
			this.drafted = drafted;
		}

		// Token: 0x0600DCF4 RID: 56564 RVA: 0x00523FC8 File Offset: 0x005221C8
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			Scribe_Values.Look<RotationDirection>(ref this.relativeRotation, "relativeRotation", RotationDirection.None, false);
			Scribe_Values.Look<bool>(ref this.drafted, "drafted", false, false);
		}

		// Token: 0x04008D82 RID: 36226
		public IntVec3 position;

		// Token: 0x04008D83 RID: 36227
		public RotationDirection relativeRotation;

		// Token: 0x04008D84 RID: 36228
		public bool drafted;

		// Token: 0x02003DE5 RID: 15845
		public struct Data
		{
			// Token: 0x0400F41A RID: 62490
			public IntVec3 local;

			// Token: 0x0400F41B RID: 62491
			public Rot4 rotation;

			// Token: 0x0400F41C RID: 62492
			public bool drafted;
		}
	}
}
