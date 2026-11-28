using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001131 RID: 4401
	public static class GameConditionUtility
	{
		// Token: 0x06006E72 RID: 28274 RVA: 0x0026BF2D File Offset: 0x0026A12D
		public static float LerpInOutValue(GameCondition gameCondition, float lerpTime, float lerpTarget = 1f)
		{
			if (gameCondition.Permanent)
			{
				return GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, lerpTime + 1f, lerpTime, lerpTarget);
			}
			return GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, (float)gameCondition.TicksLeft, lerpTime, lerpTarget);
		}

		// Token: 0x06006E73 RID: 28275 RVA: 0x0026BF64 File Offset: 0x0026A164
		public static float LerpInOutValue(float timePassed, float timeLeft, float lerpTime, float lerpTarget = 1f)
		{
			float num = 1f;
			if (timePassed < lerpTime)
			{
				num = timePassed / lerpTime;
			}
			if (timeLeft < lerpTime)
			{
				num = Mathf.Min(num, timeLeft / lerpTime);
			}
			return Mathf.Lerp(0f, lerpTarget, num);
		}
	}
}
