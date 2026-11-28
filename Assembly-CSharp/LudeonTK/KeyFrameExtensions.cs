using System;
using UnityEngine;

namespace LudeonTK
{
	// Token: 0x0200002E RID: 46
	public static class KeyFrameExtensions
	{
		// Token: 0x06000149 RID: 329 RVA: 0x00009EF8 File Offset: 0x000080F8
		public static Vector2 ToV2(this Keyframe kf)
		{
			return new Vector2(kf.time, kf.value);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00009F0D File Offset: 0x0000810D
		public static Vector4 ToVector4(this Keyframe kf)
		{
			return new Vector4(kf.time, kf.value, kf.inTangent, kf.outTangent);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00009F30 File Offset: 0x00008130
		public static Keyframe ToKeyframe(this Vector4 kf)
		{
			return new Keyframe(kf.x, kf.y, kf.z, kf.w);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00009F4F File Offset: 0x0000814F
		public static Keyframe ToKeyframe(this Vector2 v2)
		{
			return new Keyframe(v2.x, v2.y);
		}
	}
}
