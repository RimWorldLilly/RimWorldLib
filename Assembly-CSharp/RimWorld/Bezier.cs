using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001E2F RID: 7727
	public static class Bezier
	{
		// Token: 0x0600C8C7 RID: 51399 RVA: 0x0049B5C8 File Offset: 0x004997C8
		public static Vector2 EvaluateQuadratic(Vector2 a, Vector2 b, Vector2 c, float t)
		{
			Vector2 a2 = Vector2.Lerp(a, b, t);
			Vector2 b2 = Vector2.Lerp(b, c, t);
			return Vector2.Lerp(a2, b2, t);
		}

		// Token: 0x0600C8C8 RID: 51400 RVA: 0x0049B5F0 File Offset: 0x004997F0
		public static Vector2 EvaluateCubic(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
		{
			Vector2 a2 = Bezier.EvaluateQuadratic(a, b, c, t);
			Vector2 b2 = Bezier.EvaluateQuadratic(b, c, d, t);
			return Vector2.Lerp(a2, b2, t);
		}
	}
}
