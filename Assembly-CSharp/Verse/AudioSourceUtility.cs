using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000534 RID: 1332
	public static class AudioSourceUtility
	{
		// Token: 0x06002745 RID: 10053 RVA: 0x000F5C8C File Offset: 0x000F3E8C
		public static float GetSanitizedVolume(float volume, object debugInfo)
		{
			if (float.IsNegativeInfinity(volume))
			{
				Log.ErrorOnce("Volume was negative infinity (" + ((debugInfo != null) ? debugInfo.ToString() : null) + ")", 863653423);
				return 0f;
			}
			if (float.IsPositiveInfinity(volume))
			{
				Log.ErrorOnce("Volume was positive infinity (" + ((debugInfo != null) ? debugInfo.ToString() : null) + ")", 954354323);
				return 1f;
			}
			if (float.IsNaN(volume))
			{
				Log.ErrorOnce("Volume was NaN (" + ((debugInfo != null) ? debugInfo.ToString() : null) + ")", 231846572);
				return 1f;
			}
			return Mathf.Clamp(volume, 0f, 1000f);
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000F5D44 File Offset: 0x000F3F44
		public static float GetSanitizedPitch(float pitch, object debugInfo)
		{
			if (float.IsNegativeInfinity(pitch))
			{
				Log.ErrorOnce("Pitch was negative infinity (" + ((debugInfo != null) ? debugInfo.ToString() : null) + ")", 546475990);
				return 0.0001f;
			}
			if (float.IsPositiveInfinity(pitch))
			{
				Log.ErrorOnce("Pitch was positive infinity (" + ((debugInfo != null) ? debugInfo.ToString() : null) + ")", 309856435);
				return 1f;
			}
			if (float.IsNaN(pitch))
			{
				Log.ErrorOnce("Pitch was NaN (" + ((debugInfo != null) ? debugInfo.ToString() : null) + ")", 800635427);
				return 1f;
			}
			if (pitch < 0f)
			{
				Log.ErrorOnce(string.Concat(new string[]
				{
					"Pitch was negative ",
					pitch.ToString(),
					" (",
					(debugInfo != null) ? debugInfo.ToString() : null,
					")"
				}), 384765707);
				return 0.0001f;
			}
			return Mathf.Clamp(pitch, 0.0001f, 1000f);
		}
	}
}
