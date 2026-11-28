using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000615 RID: 1557
	public static class GUIEventFilterForOSX
	{
		// Token: 0x060030B2 RID: 12466 RVA: 0x001380A8 File Offset: 0x001362A8
		public static void CheckRejectGUIEvent()
		{
			if (UnityData.platform != RuntimePlatform.OSXPlayer)
			{
				return;
			}
			if (Event.current.type != EventType.mouseDown && Event.current.type != EventType.mouseUp)
			{
				return;
			}
			if (Time.frameCount != GUIEventFilterForOSX.lastRecordedFrame)
			{
				GUIEventFilterForOSX.eventsThisFrame.Clear();
				GUIEventFilterForOSX.lastRecordedFrame = Time.frameCount;
			}
			for (int i = 0; i < GUIEventFilterForOSX.eventsThisFrame.Count; i++)
			{
				if (GUIEventFilterForOSX.EventsAreEquivalent(GUIEventFilterForOSX.eventsThisFrame[i], Event.current))
				{
					GUIEventFilterForOSX.RejectEvent();
				}
			}
			GUIEventFilterForOSX.eventsThisFrame.Add(Event.current);
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x00138138 File Offset: 0x00136338
		private static bool EventsAreEquivalent(Event A, Event B)
		{
			return A.button == B.button && A.keyCode == B.keyCode && A.type == B.type;
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x00138168 File Offset: 0x00136368
		private static void RejectEvent()
		{
			if (DebugViewSettings.logInput)
			{
				Log.Message("Frame " + Time.frameCount.ToString() + ": REJECTED " + Event.current.ToStringFull());
			}
			Event.current.Use();
		}

		// Token: 0x04001F60 RID: 8032
		private static List<Event> eventsThisFrame = new List<Event>();

		// Token: 0x04001F61 RID: 8033
		private static int lastRecordedFrame = -1;
	}
}
