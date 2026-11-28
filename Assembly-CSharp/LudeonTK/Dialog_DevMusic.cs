using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000047 RID: 71
	[StaticConstructorOnStartup]
	public class Dialog_DevMusic : Window_Dev
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000D46A File Offset: 0x0000B66A
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000D46D File Offset: 0x0000B66D
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000D474 File Offset: 0x0000B674
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(275f, 360f);
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000D488 File Offset: 0x0000B688
		public Dialog_DevMusic()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.windowPosition = Prefs.DevPalettePosition;
			this.onlyDrawInDevMode = true;
			this.doCloseX = true;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000D4E8 File Offset: 0x0000B6E8
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, 24f);
			DevGUI.Label(rect, "Music Debugger");
			float y = rect.height + 6f;
			Text.Font = GameFont.Tiny;
			MusicManagerPlay musicManagerPlay = Find.MusicManagerPlay;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
			defaultInterpolatedStringHandler.AppendLiteral("State: ");
			defaultInterpolatedStringHandler.AppendFormatted<MusicManagerPlay.MusicManagerState>(musicManagerPlay.State);
			this.PrintLabel(defaultInterpolatedStringHandler.ToStringAndClear(), inRect, ref y);
			this.PrintLabel("", inRect, ref y);
			string str = "Song: ";
			string str2;
			if (!musicManagerPlay.IsPlaying)
			{
				str2 = "NA";
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(5, 3);
				defaultInterpolatedStringHandler2.AppendFormatted(musicManagerPlay.CurrentSong.defName);
				defaultInterpolatedStringHandler2.AppendLiteral(" (");
				defaultInterpolatedStringHandler2.AppendFormatted<float>(musicManagerPlay.SongTime, "0");
				defaultInterpolatedStringHandler2.AppendLiteral("/");
				defaultInterpolatedStringHandler2.AppendFormatted<float>(musicManagerPlay.SongDuration, "0");
				defaultInterpolatedStringHandler2.AppendLiteral("s)");
				str2 = defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			this.PrintLabel(str + str2, inRect, ref y);
			if (musicManagerPlay.State == MusicManagerPlay.MusicManagerState.Fadeout)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(22, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("Fadeout Progress: ");
				defaultInterpolatedStringHandler3.AppendFormatted(musicManagerPlay.FadeoutPercent.ToStringPercent("0"));
				defaultInterpolatedStringHandler3.AppendLiteral(" (");
				defaultInterpolatedStringHandler3.AppendFormatted<float>(musicManagerPlay.FadeoutDuration, "0.0");
				defaultInterpolatedStringHandler3.AppendLiteral("s)");
				this.PrintLabel(defaultInterpolatedStringHandler3.ToStringAndClear(), inRect, ref y);
			}
			else if (musicManagerPlay.IsPlaying)
			{
				this.PrintLabel("Volume: " + musicManagerPlay.CurSanitizedVolume.ToStringPercent("0"), inRect, ref y);
			}
			else
			{
				string str3 = "Next Song: ";
				string str4;
				if (musicManagerPlay.IsPlaying || musicManagerPlay.NextSongTimer <= 0f)
				{
					str4 = "NA";
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(1, 1);
					defaultInterpolatedStringHandler4.AppendFormatted<float>(musicManagerPlay.NextSongTimer, "0.0");
					defaultInterpolatedStringHandler4.AppendLiteral("s");
					str4 = defaultInterpolatedStringHandler4.ToStringAndClear();
				}
				this.PrintLabel(str3 + str4, inRect, ref y);
			}
			this.PrintLabel("Pause Vol Factor: " + musicManagerPlay.PausedVolumeFactor.ToStringPercent("0"), inRect, ref y);
			Rect rect2 = inRect;
			rect2.y = y;
			rect2.height = 18f;
			rect2.xMin = rect2.width * 0.65f;
			rect2.xMax -= 2f;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler5.AppendLiteral("Danger Mode: ");
			defaultInterpolatedStringHandler5.AppendFormatted<bool>(musicManagerPlay.DangerMusicMode);
			this.PrintLabel(defaultInterpolatedStringHandler5.ToStringAndClear(), inRect, ref y);
			if (DevGUI.ButtonText(rect2, musicManagerPlay.OverrideDangerMode ? "Override On" : "Override Off", null))
			{
				musicManagerPlay.OverrideDangerMode = !musicManagerPlay.OverrideDangerMode;
			}
			this.PrintLabel("", inRect, ref y);
			this.PrintLabel("Source Transition: " + ((musicManagerPlay.TriggeredTransition != null) ? musicManagerPlay.TriggeredTransition.def.defName : "NA"), inRect, ref y);
			this.PrintLabel("Current Sequence: " + ((musicManagerPlay.MusicSequenceWorker != null) ? musicManagerPlay.MusicSequenceWorker.def.defName : "NA"), inRect, ref y);
			MusicSequenceWorker musicSequenceWorker = musicManagerPlay.MusicSequenceWorker;
			if (musicSequenceWorker != null)
			{
				this.PrintLabel("   - Next Sequence: " + ((musicSequenceWorker.def.nextSequence != null) ? musicSequenceWorker.def.nextSequence.defName : "NA"), inRect, ref y);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(11, 1);
				defaultInterpolatedStringHandler6.AppendLiteral("   - Loop: ");
				defaultInterpolatedStringHandler6.AppendFormatted<bool>(musicSequenceWorker.ShouldLoop());
				this.PrintLabel(defaultInterpolatedStringHandler6.ToStringAndClear(), inRect, ref y);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler7 = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler7.AppendLiteral("   - Can Transition: ");
				defaultInterpolatedStringHandler7.AppendFormatted<bool>(musicSequenceWorker.ShouldTransition());
				this.PrintLabel(defaultInterpolatedStringHandler7.ToStringAndClear(), inRect, ref y);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler8 = new DefaultInterpolatedStringHandler(20, 1);
				defaultInterpolatedStringHandler8.AppendLiteral("   - Interruptible: ");
				defaultInterpolatedStringHandler8.AppendFormatted<bool>(musicSequenceWorker.CanBeInterrupted());
				this.PrintLabel(defaultInterpolatedStringHandler8.ToStringAndClear(), inRect, ref y);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler9 = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler9.AppendLiteral("   - Times Looped: ");
				defaultInterpolatedStringHandler9.AppendFormatted<int>(musicSequenceWorker.timesLooped);
				this.PrintLabel(defaultInterpolatedStringHandler9.ToStringAndClear(), inRect, ref y);
			}
			Rect rect3 = inRect;
			rect3.yMin = rect3.yMax - 30f - 2f;
			rect3.yMax -= 2f;
			rect3.xMin = 2f;
			rect3.xMax = inRect.width / 2f - 2f;
			Rect rect4 = inRect;
			rect4.yMin = rect4.yMax - 30f - 2f;
			rect4.yMax -= 2f;
			rect4.xMin = inRect.width / 2f + 2f;
			rect4.xMax -= 2f;
			if (DevGUI.ButtonText(rect3, musicManagerPlay.IsPlaying ? "Stop" : "Start", new TextAnchor?(TextAnchor.MiddleCenter)))
			{
				if (musicManagerPlay.IsPlaying)
				{
					musicManagerPlay.Stop();
				}
				else
				{
					musicManagerPlay.ScheduleNewSong();
				}
			}
			if (musicManagerPlay.MusicSequenceWorker == null)
			{
				if (DevGUI.ButtonText(rect4, "Trigger Transition", new TextAnchor?(TextAnchor.MiddleCenter)))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (MusicTransitionDef local2 in DefDatabase<MusicTransitionDef>.AllDefs)
					{
						MusicTransitionDef local = local2;
						list.Add(new FloatMenuOption(local.defName, delegate()
						{
							Find.MusicManagerPlay.ForceTriggerTransition(local);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					FloatMenu window = new FloatMenu(list, "Select transition", false);
					Find.WindowStack.Add(window);
					return;
				}
			}
			else
			{
				bool flag = musicManagerPlay.MusicSequenceWorker.def.nextSequence != null;
				if (DevGUI.ButtonText(rect4, flag ? "Next Sequence" : "Next Song", new TextAnchor?(TextAnchor.MiddleCenter)))
				{
					musicManagerPlay.ForceTriggerNextSongOrSequence();
				}
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000DB48 File Offset: 0x0000BD48
		private void PrintLabel(string text, Rect container, ref float y)
		{
			DevGUI.Label(new Rect(container.x, y, container.width, 20f), text);
			y += 20f;
		}

		// Token: 0x040000E3 RID: 227
		private Vector2 windowPosition;

		// Token: 0x040000E4 RID: 228
		private const string Title = "Music Debugger";

		// Token: 0x040000E5 RID: 229
		private const float ButtonHeight = 30f;
	}
}
