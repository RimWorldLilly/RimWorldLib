using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007E8 RID: 2024
	public static class MouseoverSounds
	{
		// Token: 0x06004010 RID: 16400 RVA: 0x00198A24 File Offset: 0x00196C24
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x00198A32 File Offset: 0x00196C32
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x00198A40 File Offset: 0x00196C40
		public static void DoRegion(Rect rect, SoundDef sound)
		{
			if (sound == null)
			{
				return;
			}
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			Rect rect2 = new Rect(GUIUtility.GUIToScreenPoint(rect.position), rect.size);
			MouseoverSounds.MouseoverRegionCall item = default(MouseoverSounds.MouseoverRegionCall);
			item.rect = rect2;
			item.sound = sound;
			item.mouseIsOver = Mouse.IsOver(rect);
			MouseoverSounds.frameCalls.Add(item);
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x00198AAC File Offset: 0x00196CAC
		public static void ResolveFrame()
		{
			for (int i = 0; i < MouseoverSounds.frameCalls.Count; i++)
			{
				if (MouseoverSounds.frameCalls[i].mouseIsOver)
				{
					if (MouseoverSounds.lastUsedCallInd != i && !MouseoverSounds.frameCalls[i].Matches(MouseoverSounds.lastUsedCall) && MouseoverSounds.forceSilenceUntilFrame < Time.frameCount)
					{
						MouseoverSounds.frameCalls[i].sound.PlayOneShotOnCamera(null);
					}
					MouseoverSounds.lastUsedCallInd = i;
					MouseoverSounds.lastUsedCall = MouseoverSounds.frameCalls[i];
					MouseoverSounds.frameCalls.Clear();
					return;
				}
			}
			MouseoverSounds.lastUsedCall = MouseoverSounds.MouseoverRegionCall.Invalid;
			MouseoverSounds.lastUsedCallInd = -1;
			MouseoverSounds.frameCalls.Clear();
		}

		// Token: 0x04002A99 RID: 10905
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x04002A9A RID: 10906
		private static int lastUsedCallInd = -1;

		// Token: 0x04002A9B RID: 10907
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x04002A9C RID: 10908
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x02002AC9 RID: 10953
		private struct MouseoverRegionCall
		{
			// Token: 0x17002C09 RID: 11273
			// (get) Token: 0x060107C3 RID: 67523 RVA: 0x005CB207 File Offset: 0x005C9407
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17002C0A RID: 11274
			// (get) Token: 0x060107C4 RID: 67524 RVA: 0x005CB220 File Offset: 0x005C9420
			public static MouseoverSounds.MouseoverRegionCall Invalid
			{
				get
				{
					return new MouseoverSounds.MouseoverRegionCall
					{
						rect = new Rect(-1000f, -1000f, 0f, 0f)
					};
				}
			}

			// Token: 0x060107C5 RID: 67525 RVA: 0x005CB256 File Offset: 0x005C9456
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x060107C6 RID: 67526 RVA: 0x005CB26C File Offset: 0x005C946C
			public override string ToString()
			{
				if (!this.IsValid)
				{
					return "(Invalid)";
				}
				string str = "(rect=";
				Rect rect = this.rect;
				return str + rect.ToString() + (this.mouseIsOver ? "mouseIsOver" : "") + ")";
			}

			// Token: 0x0400AF97 RID: 44951
			public bool mouseIsOver;

			// Token: 0x0400AF98 RID: 44952
			public Rect rect;

			// Token: 0x0400AF99 RID: 44953
			public SoundDef sound;
		}
	}
}
