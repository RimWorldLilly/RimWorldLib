using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200062F RID: 1583
	[StaticConstructorOnStartup]
	public static class Messages
	{
		// Token: 0x060031B0 RID: 12720 RVA: 0x0013D628 File Offset: 0x0013B828
		public static void Update()
		{
			if (Current.ProgramState == ProgramState.Playing && Messages.mouseoverMessageIndex >= 0 && Messages.mouseoverMessageIndex < Messages.liveMessages.Count)
			{
				Messages.liveMessages[Messages.mouseoverMessageIndex].lookTargets.TryHighlight(true, true, false);
			}
			Messages.mouseoverMessageIndex = -1;
			Messages.liveMessages.RemoveAll((Message m) => m.Expired);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x0013D6A2 File Offset: 0x0013B8A2
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, Quest quest, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets, quest), historical);
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x0013D6C3 File Offset: 0x0013B8C3
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets), historical);
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x0013D6E2 File Offset: 0x0013B8E2
		public static void Message(string text, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, TargetInfo.Invalid))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def), historical);
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x0013D70C File Offset: 0x0013B90C
		public static void Message(Message msg, bool historical = true)
		{
			if (!Messages.AcceptsMessage(msg.text, msg.lookTargets))
			{
				return;
			}
			if (historical && Find.Archive != null)
			{
				Find.Archive.Add(msg);
			}
			Messages.liveMessages.Add(msg);
			while (Messages.liveMessages.Count > 12)
			{
				Messages.liveMessages.RemoveAt(0);
			}
			if (msg.def.sound != null)
			{
				msg.def.sound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x0013D787 File Offset: 0x0013B987
		public static bool IsLive(Message msg)
		{
			return Messages.liveMessages.Contains(msg);
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x0013D794 File Offset: 0x0013B994
		public static void MessagesDoGUI()
		{
			Text.Font = GameFont.Small;
			int xOffset = (int)Messages.MessagesTopLeftStandard.x;
			int num = (int)Messages.MessagesTopLeftStandard.y;
			if (Current.Game != null && Find.ActiveLesson.ActiveLessonVisible)
			{
				num += (int)Find.ActiveLesson.Current.MessagesYOffset;
			}
			for (int i = Messages.liveMessages.Count - 1; i >= 0; i--)
			{
				Messages.liveMessages[i].Draw(xOffset, num);
				num += 26;
			}
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x0013D814 File Offset: 0x0013BA14
		public static bool CollidesWithAnyMessage(Rect rect, out float messageAlpha)
		{
			bool result = false;
			float num = 0f;
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Message message = Messages.liveMessages[i];
				if (rect.Overlaps(message.lastDrawRect))
				{
					result = true;
					num = Mathf.Max(num, message.Alpha);
				}
			}
			messageAlpha = num;
			return result;
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x0013D86C File Offset: 0x0013BA6C
		public static void Clear()
		{
			Messages.liveMessages.Clear();
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x0013D878 File Offset: 0x0013BA78
		public static void Notify_LoadedLevelChanged()
		{
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Messages.liveMessages[i].lookTargets = null;
			}
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x0013D8AC File Offset: 0x0013BAAC
		private static bool AcceptsMessage(string text, LookTargets lookTargets)
		{
			if (text.NullOrEmpty())
			{
				return false;
			}
			int i = 0;
			while (i < Messages.liveMessages.Count)
			{
				if (Messages.liveMessages[i].text == text && LookTargets.SameTargets(Messages.liveMessages[i].lookTargets, lookTargets))
				{
					if (Messages.liveMessages[i].startingFrame == RealTime.frameCount)
					{
						return false;
					}
					Messages.liveMessages[i].ResetTimer();
					Messages.liveMessages[i].Flash();
					Messages.liveMessages[i].def.sound.PlayOneShotOnCamera(null);
					return false;
				}
				else
				{
					i++;
				}
			}
			return true;
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x0013D965 File Offset: 0x0013BB65
		public static void Notify_Mouseover(Message msg)
		{
			Messages.mouseoverMessageIndex = Messages.liveMessages.IndexOf(msg);
		}

		// Token: 0x04001FD3 RID: 8147
		private static List<Message> liveMessages = new List<Message>();

		// Token: 0x04001FD4 RID: 8148
		private static int mouseoverMessageIndex = -1;

		// Token: 0x04001FD5 RID: 8149
		public static readonly Vector2 MessagesTopLeftStandard = new Vector2(140f, 16f);

		// Token: 0x04001FD6 RID: 8150
		private const int MessageYInterval = 26;

		// Token: 0x04001FD7 RID: 8151
		private const int MaxLiveMessages = 12;
	}
}
