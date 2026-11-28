using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B56 RID: 6998
	public static class SkyfallerDrawPosUtility
	{
		// Token: 0x0600B3C0 RID: 46016 RVA: 0x003F6084 File Offset: 0x003F4284
		public static Vector3 DrawPos_Accelerate(Vector3 center, int ticksToImpact, float angle, float speed, bool flipHorizontal = false, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = Mathf.Pow((float)ticksToImpact, 0.95f) * 1.7f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle, flipHorizontal, offsetComp);
		}

		// Token: 0x0600B3C1 RID: 46017 RVA: 0x003F60BC File Offset: 0x003F42BC
		public static Vector3 DrawPos_ConstantSpeed(Vector3 center, int ticksToImpact, float angle, float speed, bool flipHorizontal = false, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)ticksToImpact * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle, flipHorizontal, offsetComp);
		}

		// Token: 0x0600B3C2 RID: 46018 RVA: 0x003F60E4 File Offset: 0x003F42E4
		public static Vector3 DrawPos_Decelerate(Vector3 center, int ticksToImpact, float angle, float speed, bool flipHorizontal = false, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			float dist = (float)(ticksToImpact * ticksToImpact) * 0.00721f * speed;
			return SkyfallerDrawPosUtility.PosAtDist(center, dist, angle, flipHorizontal, offsetComp);
		}

		// Token: 0x0600B3C3 RID: 46019 RVA: 0x003F6114 File Offset: 0x003F4314
		private static Vector3 PosAtDist(Vector3 center, float dist, float angle, bool flipHorizontal = false, CompSkyfallerRandomizeDirection offsetComp = null)
		{
			Vector3 vector = Vector3Utility.FromAngleFlat(angle - 90f) * dist + ((offsetComp != null) ? offsetComp.Offset : Vector3.zero);
			if (flipHorizontal)
			{
				vector.x *= -1f;
			}
			vector += center;
			return vector;
		}
	}
}
