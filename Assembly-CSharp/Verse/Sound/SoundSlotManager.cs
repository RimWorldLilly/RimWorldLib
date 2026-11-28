using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007F2 RID: 2034
	public static class SoundSlotManager
	{
		// Token: 0x0600405E RID: 16478 RVA: 0x0019A050 File Offset: 0x00198250
		public static bool CanPlayNow(string slotName)
		{
			if (slotName == "")
			{
				return true;
			}
			float num = 0f;
			return !SoundSlotManager.allowedPlayTimes.TryGetValue(slotName, out num) || Time.realtimeSinceStartup >= SoundSlotManager.allowedPlayTimes[slotName];
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0019A098 File Offset: 0x00198298
		public static void Notify_Played(string slot, float duration)
		{
			if (slot == "")
			{
				return;
			}
			float a;
			if (SoundSlotManager.allowedPlayTimes.TryGetValue(slot, out a))
			{
				SoundSlotManager.allowedPlayTimes[slot] = Mathf.Max(a, Time.realtimeSinceStartup + duration);
				return;
			}
			SoundSlotManager.allowedPlayTimes[slot] = Time.realtimeSinceStartup + duration;
		}

		// Token: 0x04002ABD RID: 10941
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();
	}
}
