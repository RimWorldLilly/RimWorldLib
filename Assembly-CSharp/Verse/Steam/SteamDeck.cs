using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000745 RID: 1861
	public static class SteamDeck
	{
		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06003AF7 RID: 15095 RVA: 0x001783E2 File Offset: 0x001765E2
		public static bool IsSteamDeck
		{
			get
			{
				return SteamDeck.isSteamDeck || DebugSettings.simulateUsingSteamDeck;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06003AF8 RID: 15096 RVA: 0x001783F2 File Offset: 0x001765F2
		public static bool IsSteamDeckInNonKeyboardMode
		{
			get
			{
				return SteamDeck.IsSteamDeck && !Prefs.SteamDeckKeyboardMode;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x00178405 File Offset: 0x00176605
		public static bool KeyboardShowing
		{
			get
			{
				return SteamDeck.keyboardShowing;
			}
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x0017840C File Offset: 0x0017660C
		public static void Init()
		{
			SteamDeck.isSteamDeck = SteamUtils.IsSteamRunningOnSteamDeck();
			if (!SteamDeck.isSteamDeck)
			{
				return;
			}
			SteamInput.Init(false);
			SteamDeck.keyboardDismissedCallback = Callback<FloatingGamepadTextInputDismissed_t>.Create(new Callback<FloatingGamepadTextInputDismissed_t>.DispatchDelegate(SteamDeck.KeyboardDismissedCallback));
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x00178440 File Offset: 0x00176640
		public static void ShowOnScreenKeyboard(string initialText, Rect textFieldRect, bool multiline)
		{
			if (!SteamDeck.isSteamDeck || SteamDeck.keyboardShowing)
			{
				return;
			}
			Rect rect = GUIUtility.GUIToScreenRect(textFieldRect);
			if (SteamUtils.ShowFloatingGamepadTextInput(multiline ? EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeMultipleLines : EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height))
			{
				SteamDeck.keyboardShowing = true;
			}
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x00178497 File Offset: 0x00176697
		public static void HideOnScreenKeyboard()
		{
			if (!SteamDeck.isSteamDeck || !SteamDeck.keyboardShowing)
			{
				return;
			}
			SteamUtils.DismissFloatingGamepadTextInput();
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x001784AE File Offset: 0x001766AE
		public static void ShowConfigPage()
		{
			if (!SteamDeck.isSteamDeck)
			{
				return;
			}
			SteamInput.ShowBindingPanel(new InputHandle_t(0UL));
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x001784C5 File Offset: 0x001766C5
		public static void Shutdown()
		{
			if (SteamDeck.isSteamDeck)
			{
				SteamInput.Shutdown();
			}
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x001784D4 File Offset: 0x001766D4
		public static void Update()
		{
			if (!SteamDeck.isSteamDeck)
			{
				return;
			}
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				if (textEditor != null && (textEditor.controlID != SteamDeck.lastFocusedTextFieldID || textEditor.cursorIndex != SteamDeck.lastFocusedTextFieldCursorIndex) && textEditor.position != default(Rect) && !UnityGUIBugsFixer.IsLeftMouseButtonPressed())
				{
					SteamDeck.lastFocusedTextFieldID = textEditor.controlID;
					SteamDeck.lastFocusedTextFieldCursorIndex = textEditor.cursorIndex;
					SteamDeck.ShowOnScreenKeyboard(textEditor.text, textEditor.position, textEditor.multiline);
					return;
				}
			}
			else
			{
				SteamDeck.keyboardShowing = false;
			}
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0017857C File Offset: 0x0017677C
		public static void OnGUI()
		{
			if (SteamDeck.keyboardShowing)
			{
				TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				string text = textEditor.text;
				if (!text.NullOrEmpty())
				{
					Text.Font = GameFont.Medium;
					Text.Anchor = TextAnchor.UpperCenter;
					float height = Text.CalcHeight(text, 464f) + 36f;
					float num = Mathf.Min(Text.CalcSize(text).x, 464f) + 36f;
					Text.Anchor = TextAnchor.UpperLeft;
					Text.Font = GameFont.Small;
					Rect rect = new Rect((float)UI.screenWidth / 2f - num / 2f, 30f, num, height);
					Find.WindowStack.ImmediateWindow(84906312, rect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Medium;
						Text.Anchor = TextAnchor.UpperCenter;
						Rect rect = rect.AtZero().ContractedBy(18f);
						rect.height += 10f;
						Widgets.Label(rect, text);
						Text.Anchor = TextAnchor.UpperLeft;
						Text.Font = GameFont.Small;
					}, true, false, 1f, null, false);
				}
			}
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x00178670 File Offset: 0x00176870
		public static void Vibrate()
		{
			if (!SteamDeck.isSteamDeck)
			{
				return;
			}
			SteamInput.TriggerVibration(new InputHandle_t(0UL), 10000, 10000);
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x00178690 File Offset: 0x00176890
		public static void RootOnGUI()
		{
			if (!SteamDeck.IsSteamDeck)
			{
				return;
			}
			SteamDeck.SimulateRightClickIfHoldingMiddleButton();
			if (SteamDeck.unfocusCurrentTextField && Event.current.type == EventType.repaint)
			{
				SteamDeck.unfocusCurrentTextField = false;
				UI.UnfocusCurrentTextField();
				TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				if (textEditor != null)
				{
					SteamDeck.lastFocusedTextFieldID = textEditor.controlID;
					SteamDeck.lastFocusedTextFieldCursorIndex = textEditor.cursorIndex;
				}
			}
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x001786FC File Offset: 0x001768FC
		public static void WindowOnGUI()
		{
			if (!SteamDeck.IsSteamDeck)
			{
				return;
			}
			SteamDeck.SimulateRightClickIfHoldingMiddleButton();
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x0017870C File Offset: 0x0017690C
		private static void SimulateRightClickIfHoldingMiddleButton()
		{
			if ((Event.current.type == EventType.mouseDown || Event.current.type == EventType.mouseUp) && Event.current.button == 0 && (Input.GetMouseButton(2) || Input.GetMouseButtonUp(2) || (SteamDeck.nextLeftMouseUpEventIsRightMouseUpEvent && Event.current.type == EventType.mouseUp)))
			{
				Event.current.button = 1;
				if (Event.current.type == EventType.mouseDown)
				{
					SteamDeck.nextLeftMouseUpEventIsRightMouseUpEvent = true;
					return;
				}
				if (Event.current.type == EventType.mouseUp)
				{
					SteamDeck.nextLeftMouseUpEventIsRightMouseUpEvent = false;
				}
			}
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x00178793 File Offset: 0x00176993
		private static void KeyboardDismissedCallback(FloatingGamepadTextInputDismissed_t data)
		{
			SteamDeck.keyboardShowing = false;
			SteamDeck.unfocusCurrentTextField = true;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x001787A4 File Offset: 0x001769A4
		public static void ShowSteamDeckGameControlsIfNotKnown()
		{
			if (SteamDeck.IsSteamDeckInNonKeyboardMode && Find.TickManager.TicksGame > 120 && !Find.TickManager.Paused && !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.SteamDeckControlsGame))
			{
				Dialog_MessageBox dialog_MessageBox = new Dialog_MessageBox(ConceptDefOf.SteamDeckControlsGame.HelpTextAdjusted, null, null, null, null, null, false, null, null, WindowLayer.Dialog);
				dialog_MessageBox.image = ContentFinder<Texture2D>.Get("UI/Misc/SteamDeck2", true);
				Find.WindowStack.Add(dialog_MessageBox);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.SteamDeckControlsGame, KnowledgeAmount.Total);
			}
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x00178824 File Offset: 0x00176A24
		public static void ShowSteamDeckMainMenuControlsIfNotKnown()
		{
			if (SteamDeck.IsSteamDeckInNonKeyboardMode && !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.SteamDeckControlsMainMenu))
			{
				Dialog_MessageBox dialog_MessageBox = new Dialog_MessageBox(ConceptDefOf.SteamDeckControlsMainMenu.HelpTextAdjusted, null, null, null, null, null, false, null, null, WindowLayer.Dialog);
				dialog_MessageBox.image = ContentFinder<Texture2D>.Get("UI/Misc/SteamDeck1", true);
				Find.WindowStack.Add(dialog_MessageBox);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.SteamDeckControlsMainMenu, KnowledgeAmount.Total);
			}
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x0017888C File Offset: 0x00176A8C
		public static void HandleTouchScreenScrollViewScroll(Rect outRect, ref Vector2 scrollPosition)
		{
			if (!SteamDeck.IsSteamDeck)
			{
				return;
			}
			if (DragAndDropWidget.Dragging || Widgets.Painting || ReorderableWidget.Dragging)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0) && Mouse.IsOver(outRect) && !Mouse.IsOver(new Rect(outRect.xMax - 16f, outRect.y, 16f, outRect.height)) && !Mouse.IsOver(new Rect(outRect.x, outRect.yMax - 16f, outRect.width, 16f)))
			{
				SteamDeck.currentScrollView = new Rect?(outRect);
				SteamDeck.currentScrollViewStartMousePos = UI.MousePositionOnUIInverted;
				SteamDeck.scrollMouseTraveledEnoughDist = false;
				SteamDeck.scrollVelocityRecords.Clear();
				if (SteamDeck.scrollViewWithVelocity == SteamDeck.currentScrollView)
				{
					SteamDeck.scrollViewWithVelocity = null;
					SteamDeck.scrollViewVelocity = default(Vector2);
				}
			}
			else if (Input.GetMouseButton(0) && SteamDeck.currentScrollView == outRect)
			{
				if (!SteamDeck.scrollMouseTraveledEnoughDist && (UI.MousePositionOnUIInverted - SteamDeck.currentScrollViewStartMousePos).sqrMagnitude > 100f)
				{
					SteamDeck.scrollMouseTraveledEnoughDist = true;
				}
				if (SteamDeck.scrollMouseTraveledEnoughDist)
				{
					Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
					scrollPosition -= UnityGUIBugsFixer.CurrentEventDelta;
					SteamDeck.scrollVelocityRecords.Add(new ValueTuple<float, Vector2>(Time.time, -UnityGUIBugsFixer.CurrentEventDelta));
					SteamDeck.<HandleTouchScreenScrollViewScroll>g__ClampScrollPosition|34_0(ref scrollPosition);
				}
			}
			else if (!Input.GetMouseButton(0) && SteamDeck.currentScrollView == outRect)
			{
				if (SteamDeck.scrollMouseTraveledEnoughDist)
				{
					SteamDeck.scrollViewWithVelocity = SteamDeck.currentScrollView;
					SteamDeck.scrollViewVelocity = default(Vector2);
					for (int i = 0; i < SteamDeck.scrollVelocityRecords.Count; i++)
					{
						if (Time.time - SteamDeck.scrollVelocityRecords[i].Item1 < 0.057f)
						{
							SteamDeck.scrollViewVelocity += SteamDeck.scrollVelocityRecords[i].Item2;
						}
					}
					SteamDeck.consumeAllMouseUpEventsOnFrame = Time.frameCount;
				}
				SteamDeck.currentScrollView = null;
			}
			if (Event.current.type == EventType.repaint && SteamDeck.scrollViewWithVelocity == outRect && SteamDeck.scrollViewVelocity != default(Vector2))
			{
				scrollPosition += SteamDeck.scrollViewVelocity;
				SteamDeck.scrollViewVelocity = Vector2.MoveTowards(SteamDeck.scrollViewVelocity, default(Vector2), Time.deltaTime * 90f);
				SteamDeck.<HandleTouchScreenScrollViewScroll>g__ClampScrollPosition|34_0(ref scrollPosition);
			}
			if (SteamDeck.consumeAllMouseUpEventsOnFrame == Time.frameCount && Event.current.type == EventType.mouseUp)
			{
				Event.current.Use();
			}
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x00178BB8 File Offset: 0x00176DB8
		public static string GetKeyBindingLabel(KeyBindingDef keyDef)
		{
			KeyCode mainKey = keyDef.MainKey;
			if (mainKey <= KeyCode.D)
			{
				if (mainKey <= KeyCode.Escape)
				{
					if (mainKey == KeyCode.Tab)
					{
						return "X";
					}
					if (mainKey == KeyCode.Return)
					{
						return "A";
					}
					if (mainKey == KeyCode.Escape)
					{
						return "B";
					}
				}
				else
				{
					if (mainKey == KeyCode.Space)
					{
						return "Y";
					}
					if (mainKey == KeyCode.A)
					{
						return "←";
					}
					if (mainKey == KeyCode.D)
					{
						return "→";
					}
				}
			}
			else if (mainKey <= KeyCode.PageDown)
			{
				if (mainKey == KeyCode.S)
				{
					return "↓";
				}
				if (mainKey == KeyCode.W)
				{
					return "↑";
				}
				switch (mainKey)
				{
				case KeyCode.Home:
					return "R4";
				case KeyCode.End:
					return "L4";
				case KeyCode.PageUp:
					return "L1";
				case KeyCode.PageDown:
					return "R1";
				}
			}
			else
			{
				if (mainKey == KeyCode.RightShift)
				{
					return "R5";
				}
				if (mainKey == KeyCode.LeftShift)
				{
					return "R5";
				}
				switch (mainKey)
				{
				case KeyCode.Mouse0:
					return "R2";
				case KeyCode.Mouse1:
					return "L2";
				case KeyCode.Mouse2:
					return "L5";
				}
			}
			if (keyDef == KeyBindingDefOf.MapZoom_In || keyDef == KeyBindingDefOf.MapZoom_Out || keyDef == KeyBindingDefOf.Accept || keyDef == KeyBindingDefOf.Cancel || keyDef == KeyBindingDefOf.TogglePause || keyDef == KeyBindingDefOf.TimeSpeed_Faster || keyDef == KeyBindingDefOf.TimeSpeed_Slower || keyDef == KeyBindingDefOf.QueueOrder)
			{
				return "?";
			}
			return keyDef.MainKeyLabel;
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x00178D34 File Offset: 0x00176F34
		[CompilerGenerated]
		internal static void <HandleTouchScreenScrollViewScroll>g__ClampScrollPosition|34_0(ref Vector2 pos)
		{
			if (pos.x < 0f)
			{
				pos.x = 0f;
			}
			if (pos.y < 0f)
			{
				pos.y = 0f;
			}
		}

		// Token: 0x040026B2 RID: 9906
		private static bool isSteamDeck;

		// Token: 0x040026B3 RID: 9907
		private static bool keyboardShowing;

		// Token: 0x040026B4 RID: 9908
		private static int lastFocusedTextFieldID;

		// Token: 0x040026B5 RID: 9909
		private static int lastFocusedTextFieldCursorIndex;

		// Token: 0x040026B6 RID: 9910
		private static bool nextLeftMouseUpEventIsRightMouseUpEvent;

		// Token: 0x040026B7 RID: 9911
		private static bool unfocusCurrentTextField;

		// Token: 0x040026B8 RID: 9912
		private static Callback<FloatingGamepadTextInputDismissed_t> keyboardDismissedCallback;

		// Token: 0x040026B9 RID: 9913
		private static Rect? currentScrollView;

		// Token: 0x040026BA RID: 9914
		private static Vector2 currentScrollViewStartMousePos;

		// Token: 0x040026BB RID: 9915
		private static bool scrollMouseTraveledEnoughDist;

		// Token: 0x040026BC RID: 9916
		private static int consumeAllMouseUpEventsOnFrame = -1;

		// Token: 0x040026BD RID: 9917
		private static Rect? scrollViewWithVelocity;

		// Token: 0x040026BE RID: 9918
		private static Vector2 scrollViewVelocity;

		// Token: 0x040026BF RID: 9919
		private static List<ValueTuple<float, Vector2>> scrollVelocityRecords = new List<ValueTuple<float, Vector2>>();
	}
}
