using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006F8 RID: 1784
	public static class RealTime
	{
		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x060038BF RID: 14527 RVA: 0x0016C878 File Offset: 0x0016AA78
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x060038C0 RID: 14528 RVA: 0x0016C87F File Offset: 0x0016AA7F
		public static float UnpausedRealTime
		{
			get
			{
				return RealTime.unpausedTime;
			}
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x0016C888 File Offset: 0x0016AA88
		public static void Update()
		{
			RealTime.frameCount = Time.frameCount;
			RealTime.deltaTime = Time.deltaTime;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			RealTime.realDeltaTime = realtimeSinceStartup - RealTime.lastRealTime;
			RealTime.lastRealTime = realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Find.Maps != null && !Find.Maps.Empty<Map>())
				{
					RealTime.moteList.MoteListUpdate();
				}
				if (Current.Game != null)
				{
					RealTime.unpausedTime += RealTime.deltaTime * Find.TickManager.TickRateMultiplier;
				}
			}
			else
			{
				RealTime.moteList.Clear();
			}
			if (DebugSettings.lowFPS && Time.deltaTime < 100f)
			{
				Thread.Sleep((int)(100f - Time.deltaTime));
			}
		}

		// Token: 0x040024EE RID: 9454
		public static float deltaTime;

		// Token: 0x040024EF RID: 9455
		public static float realDeltaTime;

		// Token: 0x040024F0 RID: 9456
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x040024F1 RID: 9457
		public static int frameCount;

		// Token: 0x040024F2 RID: 9458
		private static float unpausedTime;

		// Token: 0x040024F3 RID: 9459
		private static float lastRealTime = 0f;
	}
}
