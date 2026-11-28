using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001D4C RID: 7500
	public static class TimeControls
	{
		// Token: 0x0600C210 RID: 49680 RVA: 0x0046B654 File Offset: 0x00469854
		private static void PlaySoundOf(TimeSpeed speed)
		{
			SoundDef soundDef = null;
			switch (speed)
			{
			case TimeSpeed.Paused:
				soundDef = SoundDefOf.Clock_Stop;
				break;
			case TimeSpeed.Normal:
				soundDef = SoundDefOf.Clock_Normal;
				break;
			case TimeSpeed.Fast:
				soundDef = SoundDefOf.Clock_Fast;
				break;
			case TimeSpeed.Superfast:
				soundDef = SoundDefOf.Clock_Superfast;
				break;
			case TimeSpeed.Ultrafast:
				soundDef = SoundDefOf.Clock_Superfast;
				break;
			}
			if (soundDef != null)
			{
				soundDef.PlayOneShotOnCamera(null);
			}
			SteamDeck.Vibrate();
		}

		// Token: 0x0600C211 RID: 49681 RVA: 0x0046B6B4 File Offset: 0x004698B4
		private static KeyBindingDef GetBindingFor(TimeSpeed speed)
		{
			switch (speed)
			{
			case TimeSpeed.Paused:
				return KeyBindingDefOf.TogglePause;
			case TimeSpeed.Normal:
				return KeyBindingDefOf.TimeSpeed_Normal;
			case TimeSpeed.Fast:
				return KeyBindingDefOf.TimeSpeed_Fast;
			case TimeSpeed.Superfast:
				return KeyBindingDefOf.TimeSpeed_Superfast;
			case TimeSpeed.Ultrafast:
				return KeyBindingDefOf.TimeSpeed_Ultrafast;
			default:
				return null;
			}
		}

		// Token: 0x0600C212 RID: 49682 RVA: 0x0046B6F4 File Offset: 0x004698F4
		public static void DoTimeControlsGUI(Rect timerRect)
		{
			TickManager tickManager = Find.TickManager;
			Widgets.BeginGroup(timerRect);
			Rect rect = new Rect(0f, 0f, TimeControls.TimeButSize.x, TimeControls.TimeButSize.y);
			for (int i = 0; i < TimeControls.CachedTimeSpeedValues.Length; i++)
			{
				TimeSpeed timeSpeed = TimeControls.CachedTimeSpeedValues[i];
				if (timeSpeed != TimeSpeed.Ultrafast)
				{
					string value = KeyPrefs.KeyPrefsData.GetBoundKeyCode(TimeControls.GetBindingFor(timeSpeed), KeyPrefs.BindingSlot.A).ToStringReadable();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("HotKeyTip".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(value);
					string tooltip = defaultInterpolatedStringHandler.ToStringAndClear();
					if (Widgets.ButtonImage(rect, TexButton.SpeedButtonTextures[(int)timeSpeed], true, tooltip) && !tickManager.ForcePaused)
					{
						if (timeSpeed == TimeSpeed.Paused)
						{
							tickManager.TogglePaused();
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Pause, KnowledgeAmount.SpecificInteraction);
						}
						else
						{
							tickManager.CurTimeSpeed = timeSpeed;
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						}
						TimeControls.PlaySoundOf(tickManager.CurTimeSpeed);
					}
					if ((tickManager.ForcePaused ? TimeSpeed.Paused : tickManager.CurTimeSpeed) == timeSpeed)
					{
						GUI.DrawTexture(rect, TexUI.HighlightTex);
					}
					rect.x += rect.width;
				}
			}
			if (tickManager.slower.ForcedNormalSpeed)
			{
				Widgets.DrawLineHorizontal(rect.width * 2f, rect.height / 2f, rect.width * 2f);
			}
			if (tickManager.ForcePaused)
			{
				Widgets.DrawLineHorizontal(rect.width, rect.height / 2f, rect.width * 3f);
			}
			Widgets.EndGroup();
			GenUI.AbsorbClicksInRect(timerRect);
			UIHighlighter.HighlightOpportunity(timerRect, "TimeControls");
			if (Event.current.type == EventType.keyDown)
			{
				if (KeyBindingDefOf.TogglePause.KeyDownEvent)
				{
					Find.TickManager.TogglePaused();
					TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Pause, KnowledgeAmount.SpecificInteraction);
					Event.current.Use();
				}
				if (!Find.WindowStack.WindowsForcePause)
				{
					if (KeyBindingDefOf.TimeSpeed_Normal.KeyDownEvent)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
					if (KeyBindingDefOf.TimeSpeed_Fast.KeyDownEvent)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Fast;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
					if (KeyBindingDefOf.TimeSpeed_Superfast.KeyDownEvent)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Superfast;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
					if (KeyBindingDefOf.TimeSpeed_Slower.KeyDownEvent && Find.TickManager.CurTimeSpeed != TimeSpeed.Paused)
					{
						TickManager tickManager2 = Find.TickManager;
						TimeSpeed curTimeSpeed = tickManager2.CurTimeSpeed;
						tickManager2.CurTimeSpeed = (TimeSpeed)(curTimeSpeed - TimeSpeed.Normal);
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
					if (KeyBindingDefOf.TimeSpeed_Faster.KeyDownEvent && Find.TickManager.CurTimeSpeed < TimeSpeed.Superfast)
					{
						TickManager tickManager3 = Find.TickManager;
						TimeSpeed curTimeSpeed = tickManager3.CurTimeSpeed;
						tickManager3.CurTimeSpeed = curTimeSpeed + 1;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
				}
				if (Prefs.DevMode)
				{
					if (KeyBindingDefOf.TimeSpeed_Ultrafast.KeyDownEvent)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Ultrafast;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						Event.current.Use();
					}
					if (KeyBindingDefOf.Dev_TickOnce.KeyDownEvent && tickManager.CurTimeSpeed == TimeSpeed.Paused)
					{
						tickManager.DoSingleTick();
						SoundDefOf.Clock_Stop.PlayOneShotOnCamera(null);
					}
				}
			}
		}

		// Token: 0x04006EF1 RID: 28401
		public static readonly Vector2 TimeButSize = new Vector2(32f, 24f);

		// Token: 0x04006EF2 RID: 28402
		private static readonly TimeSpeed[] CachedTimeSpeedValues = (TimeSpeed[])Enum.GetValues(typeof(TimeSpeed));
	}
}
