using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000659 RID: 1625
	public static class UnityGUIBugsFixer
	{
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060032D7 RID: 13015 RVA: 0x00145263 File Offset: 0x00143463
		public static bool IsSteamDeckOrLinuxBuild
		{
			get
			{
				return SteamDeck.IsSteamDeck || Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060032D8 RID: 13016 RVA: 0x00145280 File Offset: 0x00143480
		public static List<Resolution> ScreenResolutionsWithoutDuplicates
		{
			get
			{
				UnityGUIBugsFixer.resolutions.Clear();
				Resolution[] array = Screen.resolutions;
				for (int i = 0; i < array.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < UnityGUIBugsFixer.resolutions.Count; j++)
					{
						if (UnityGUIBugsFixer.resolutions[j].width == array[i].width && UnityGUIBugsFixer.resolutions[j].height == array[i].height)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						UnityGUIBugsFixer.resolutions.Add(array[i]);
					}
				}
				return UnityGUIBugsFixer.resolutions;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060032D9 RID: 13017 RVA: 0x00145323 File Offset: 0x00143523
		public static Vector2 CurrentEventDelta
		{
			get
			{
				UnityGUIBugsFixer.EnsureFrameMouseDelta();
				if (UnityGUIBugsFixer.frameDeltaConsumedFrame == Time.frameCount)
				{
					return Vector2.zero;
				}
				UnityGUIBugsFixer.frameDeltaConsumedFrame = Time.frameCount;
				return UnityGUIBugsFixer.currentEventDelta;
			}
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x0014534C File Offset: 0x0014354C
		private static void EnsureFrameMouseDelta()
		{
			int frameCount = Time.frameCount;
			if (frameCount == UnityGUIBugsFixer.frameDeltaComputedFrame)
			{
				return;
			}
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			UnityGUIBugsFixer.currentEventDelta = ((UnityGUIBugsFixer.frameDeltaComputedFrame < 0) ? Vector2.zero : (mousePositionOnUIInverted - UnityGUIBugsFixer.lastFrameMousePosition));
			UnityGUIBugsFixer.lastFrameMousePosition = mousePositionOnUIInverted;
			UnityGUIBugsFixer.frameDeltaComputedFrame = frameCount;
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x00145399 File Offset: 0x00143599
		public static bool MouseDrag(int button = 0)
		{
			if (!UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild)
			{
				return Event.current.type == EventType.mouseDrag && Event.current.button == button;
			}
			return Input.GetMouseButton(button);
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x001453C5 File Offset: 0x001435C5
		public static void OnGUI()
		{
			UnityGUIBugsFixer.EnsureSliderDragReset();
			UnityGUIBugsFixer.EnsureFrameMouseDelta();
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x001453D4 File Offset: 0x001435D4
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.scrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * -6f);
			}
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x0014542C File Offset: 0x0014362C
		private static void FixShift()
		{
			if ((Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer) && !Event.current.shift)
			{
				Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
			}
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x0014547A File Offset: 0x0014367A
		public static bool ResolutionsEqual(IntVec2 a, IntVec2 b)
		{
			return a == b;
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x00145484 File Offset: 0x00143684
		private static void FixDelta()
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild)
			{
				if (Event.current.rawType == EventType.mouseDown)
				{
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
				if (Event.current.type != EventType.repaint)
				{
					UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					return;
				}
				if (Time.frameCount != UnityGUIBugsFixer.lastMousePositionFrame)
				{
					if (UnityGUIBugsFixer.lastMousePosition != null)
					{
						UnityGUIBugsFixer.currentEventDelta = mousePositionOnUIInverted - UnityGUIBugsFixer.lastMousePosition.Value;
					}
					else
					{
						UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					}
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
			}
			else if (Event.current.rawType == EventType.mouseDrag)
			{
				if (mousePositionOnUIInverted != UnityGUIBugsFixer.lastMousePosition || Time.frameCount != UnityGUIBugsFixer.lastMousePositionFrame)
				{
					if (UnityGUIBugsFixer.lastMousePosition != null)
					{
						UnityGUIBugsFixer.currentEventDelta = mousePositionOnUIInverted - UnityGUIBugsFixer.lastMousePosition.Value;
					}
					else
					{
						UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					}
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
			}
			else
			{
				UnityGUIBugsFixer.currentEventDelta = Event.current.delta;
				if (Event.current.rawType == EventType.mouseDown)
				{
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
				if (Event.current.rawType == EventType.mouseUp)
				{
					UnityGUIBugsFixer.lastMousePosition = null;
				}
			}
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x001455FB File Offset: 0x001437FB
		private static void EnsureSliderDragReset()
		{
			if (Event.current.type == EventType.mouseUp)
			{
				Widgets.ResetSliderDraggingIDs();
			}
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x0014560F File Offset: 0x0014380F
		public static void Notify_BeginGroup()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x00145616 File Offset: 0x00143816
		public static void Notify_EndGroup()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x0014561D File Offset: 0x0014381D
		public static void Notify_BeginScrollView()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x00145624 File Offset: 0x00143824
		public static void Notify_EndScrollView()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x0014562B File Offset: 0x0014382B
		public static void Notify_GUIMatrixChanged()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x00145634 File Offset: 0x00143834
		private static void FixSteamDeckMousePositionNeverUpdating()
		{
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x00145641 File Offset: 0x00143841
		private static void RememberMouseStateForIsLeftMouseButtonPressed()
		{
			if (Event.current.type == EventType.mouseDown && Event.current.button == 0)
			{
				UnityGUIBugsFixer.leftMouseButtonPressed = true;
				return;
			}
			if (Event.current.rawType == EventType.mouseUp && Event.current.button == 0)
			{
				UnityGUIBugsFixer.leftMouseButtonPressed = false;
			}
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x00145681 File Offset: 0x00143881
		public static bool IsLeftMouseButtonPressed()
		{
			return Input.GetMouseButton(0) || UnityGUIBugsFixer.leftMouseButtonPressed;
		}

		// Token: 0x0400210B RID: 8459
		private static List<Resolution> resolutions = new List<Resolution>();

		// Token: 0x0400210C RID: 8460
		private static Vector2 currentEventDelta;

		// Token: 0x0400210D RID: 8461
		private static int lastMousePositionFrame;

		// Token: 0x0400210E RID: 8462
		private static bool leftMouseButtonPressed;

		// Token: 0x0400210F RID: 8463
		private static Vector2 lastFrameMousePosition;

		// Token: 0x04002110 RID: 8464
		private static int frameDeltaComputedFrame = -1;

		// Token: 0x04002111 RID: 8465
		private static int frameDeltaConsumedFrame = -1;

		// Token: 0x04002112 RID: 8466
		private const float ScrollFactor = -6f;

		// Token: 0x04002113 RID: 8467
		private static Vector2? lastMousePosition;
	}
}
