using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000381 RID: 897
	public static class AnimationUtility
	{
		// Token: 0x06001B52 RID: 6994 RVA: 0x000A8928 File Offset: 0x000A6B28
		public static Vector3 AdjustOffsetForRotationMode(RotationMode mode, Vector3 offset, Pawn pawn)
		{
			if (mode == RotationMode.None)
			{
				return offset;
			}
			if (mode == RotationMode.OneD)
			{
				if (!(pawn.Rotation == Rot4.South))
				{
					return offset;
				}
				return offset * -1f;
			}
			else
			{
				if (mode == RotationMode.TwoD)
				{
					return offset.RotatedBy(pawn.Rotation);
				}
				if (mode == RotationMode.PawnAimTarget && pawn.TargetCurrentlyAimingAt.IsValid)
				{
					return offset.RotatedBy((pawn.TargetCurrentlyAimingAt.CenterVector3 - pawn.Position.ToVector3()).ToAngleFlat());
				}
				return offset;
			}
		}
	}
}
