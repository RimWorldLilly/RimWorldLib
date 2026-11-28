using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000680 RID: 1664
	public class WindowStack
	{
		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x0600346C RID: 13420 RVA: 0x001515E1 File Offset: 0x0014F7E1
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x170009D2 RID: 2514
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x0600346E RID: 13422 RVA: 0x001515FC File Offset: 0x0014F7FC
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x0600346F RID: 13423 RVA: 0x00151609 File Offset: 0x0014F809
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06003470 RID: 13424 RVA: 0x00151614 File Offset: 0x0014F814
		public bool WindowsForcePause
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].forcePause)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06003471 RID: 13425 RVA: 0x00151650 File Offset: 0x0014F850
		public bool WindowsPreventCameraMotion
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventCameraMotion)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06003472 RID: 13426 RVA: 0x0015168C File Offset: 0x0014F88C
		public bool WindowsPreventDrawTutor
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventDrawTutor)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06003473 RID: 13427 RVA: 0x001516C5 File Offset: 0x0014F8C5
		public float SecondsSinceClosedGameStartDialog
		{
			get
			{
				if (this.gameStartDialogOpen)
				{
					return 0f;
				}
				if (this.timeGameStartDialogClosed < 0f)
				{
					return 9999999f;
				}
				return Time.time - this.timeGameStartDialogClosed;
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06003474 RID: 13428 RVA: 0x001516F4 File Offset: 0x0014F8F4
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06003475 RID: 13429 RVA: 0x0015170C File Offset: 0x0014F90C
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06003476 RID: 13430 RVA: 0x0015171C File Offset: 0x0014F91C
		public bool NonImmediateDialogWindowOpen
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (!(this.windows[i] is ImmediateWindow) && this.windows[i].layer == WindowLayer.Dialog)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06003477 RID: 13431 RVA: 0x0015176C File Offset: 0x0014F96C
		public bool WindowsPreventSave
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventSave)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06003478 RID: 13432 RVA: 0x001517A8 File Offset: 0x0014F9A8
		public bool AnyWindowAbsorbingAllInput
		{
			get
			{
				using (List<Window>.Enumerator enumerator = this.windows.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.absorbInputAroundWindow)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x00151804 File Offset: 0x0014FA04
		public bool AnySearchWidgetFocused
		{
			get
			{
				foreach (Window window in this.windows)
				{
					QuickSearchWidget commonSearchWidget = window.CommonSearchWidget;
					if (commonSearchWidget != null && commonSearchWidget.CurrentlyFocused())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x00151868 File Offset: 0x0014FA68
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x001518A4 File Offset: 0x0014FAA4
		public void HandleEventsHighPriority()
		{
			if (Event.current.type == EventType.mouseDown && this.GetWindowAt(UI.GUIToScreenPoint(Event.current.mousePosition)) == null)
			{
				bool flag = this.CloseWindowsBecauseClicked(null);
				this.NotifyOutsideClicks(null);
				if (flag)
				{
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				this.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				this.Notify_PressedAccept();
			}
			if ((Event.current.type == EventType.mouseDown || Event.current.type == EventType.keyDown) && !this.GetsInput(null))
			{
				Event.current.Use();
			}
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x00151940 File Offset: 0x0014FB40
		public void WindowStackOnGUI()
		{
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int i = this.windowStackOnGUITmpList.Count - 1; i >= 0; i--)
			{
				this.windowStackOnGUITmpList[i].ExtraOnGUI();
			}
			this.UpdateImmediateWindowsList();
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			int j = 0;
			while (j < this.windowStackOnGUITmpList.Count)
			{
				if (!this.windowStackOnGUITmpList[j].drawShadow)
				{
					goto IL_EF;
				}
				if (this.windowStackOnGUITmpList[j].drawInScreenshotMode || !Find.UIRoot.screenshotMode.Active)
				{
					GUI.color = new Color(1f, 1f, 1f, this.windowStackOnGUITmpList[j].shadowAlpha);
					Widgets.DrawShadowAround(this.windowStackOnGUITmpList[j].windowRect);
					GUI.color = Color.white;
					goto IL_EF;
				}
				IL_100:
				j++;
				continue;
				IL_EF:
				this.windowStackOnGUITmpList[j].WindowOnGUI();
				goto IL_100;
			}
			if (this.updateInternalWindowsOrderLater)
			{
				this.updateInternalWindowsOrderLater = false;
				this.UpdateInternalWindowsOrder();
			}
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x00151A78 File Offset: 0x0014FC78
		public void Notify_ClickedInsideWindow(Window window)
		{
			if (this.GetsInput(window))
			{
				this.windows.Remove(window);
				this.InsertAtCorrectPositionInList(window);
				this.focusedWindow = window;
			}
			else
			{
				Event.current.Use();
			}
			this.CloseWindowsBecauseClicked(window);
			this.NotifyOutsideClicks(window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x00151ACB File Offset: 0x0014FCCB
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x00151ADC File Offset: 0x0014FCDC
		public void Notify_PressedCancel()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnCancel || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnCancelKeyPressed();
					return;
				}
			}
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x00151B50 File Offset: 0x0014FD50
		public void Notify_PressedAccept()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnAccept || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnAcceptKeyPressed();
					return;
				}
			}
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x00151BC1 File Offset: 0x0014FDC1
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x00151BCA File Offset: 0x0014FDCA
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x00151BE0 File Offset: 0x0014FDE0
		public bool IsOpen<WindowType>()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x00151C1C File Offset: 0x0014FE1C
		public bool IsOpen(Type type)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x00151C5B File Offset: 0x0014FE5B
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x00151C69 File Offset: 0x0014FE69
		public bool TryGetWindow<T>(out T window) where T : class
		{
			window = this.WindowOfType<T>();
			return window != null;
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x00151C88 File Offset: 0x0014FE88
		public WindowType WindowOfType<WindowType>() where WindowType : class
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return this.windows[i] as WindowType;
				}
			}
			return default(WindowType);
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x00151CE0 File Offset: 0x0014FEE0
		public bool GetsInput(Window window)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					return true;
				}
				if (this.windows[i].absorbInputAroundWindow)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x00151D2C File Offset: 0x0014FF2C
		public void Add(Window window)
		{
			this.RemoveWindowsOfType(window.GetType());
			window.ID = WindowStack.uniqueWindowID++;
			window.PreOpen();
			this.InsertAtCorrectPositionInList(window);
			this.FocusAfterInsertIfShould(window);
			this.updateInternalWindowsOrderLater = true;
			window.PostOpen();
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x00151D7C File Offset: 0x0014FF7C
		public void ImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground = true, bool absorbInputAroundWindow = false, float shadowAlpha = 1f, Action doClickOutsideFunc = null, bool ignoreScreenFader = false)
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			if (ID == 0)
			{
				Log.Warning("Used 0 as immediate window ID.");
				return;
			}
			ID = -Math.Abs(ID);
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].ID == ID)
				{
					ImmediateWindow immediateWindow = (ImmediateWindow)this.windows[i];
					immediateWindow.windowRect = rect;
					immediateWindow.doWindowFunc = doWindowFunc;
					immediateWindow.doClickOutsideFunc = doClickOutsideFunc;
					immediateWindow.layer = layer;
					immediateWindow.doWindowBackground = doBackground;
					immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
					immediateWindow.shadowAlpha = shadowAlpha;
					immediateWindow.ignoreScreenFader = ignoreScreenFader;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.AddNewImmediateWindow(ID, rect, layer, doWindowFunc, doBackground, absorbInputAroundWindow, shadowAlpha, doClickOutsideFunc, ignoreScreenFader);
			}
			this.immediateWindowsRequests.Add(ID);
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x00151E50 File Offset: 0x00150050
		public bool TryRemove(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == windowType)
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x00151EA4 File Offset: 0x001500A4
		public bool TryRemoveAssignableFromType(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (windowType.IsAssignableFrom(this.windows[i].GetType()))
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x00151EF8 File Offset: 0x001500F8
		public bool TryRemove(Window window, bool doCloseSound = true)
		{
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] == window)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (!window.OnCloseRequest())
			{
				return false;
			}
			if (doCloseSound && window.soundClose != null)
			{
				window.soundClose.PlayOneShotOnCamera(null);
			}
			window.PreClose();
			this.windows.Remove(window);
			window.PostClose();
			if (this.focusedWindow == window)
			{
				if (this.windows.Count > 0)
				{
					this.focusedWindow = this.windows[this.windows.Count - 1];
				}
				else
				{
					this.focusedWindow = null;
				}
				this.updateInternalWindowsOrderLater = true;
			}
			return true;
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x00151FB8 File Offset: 0x001501B8
		public Window GetWindowAt(Vector2 pos)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i].windowRect.Contains(pos))
				{
					return this.windows[i];
				}
			}
			return null;
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x00152004 File Offset: 0x00150204
		private void AddNewImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground, bool absorbInputAroundWindow, float shadowAlpha, Action doClickOutsideFunc, bool ignoreScreenFader)
		{
			if (ID >= 0)
			{
				Log.Error("Invalid immediate window ID.");
				return;
			}
			ImmediateWindow immediateWindow = new ImmediateWindow();
			immediateWindow.ID = ID;
			immediateWindow.layer = layer;
			immediateWindow.doWindowFunc = doWindowFunc;
			immediateWindow.doClickOutsideFunc = doClickOutsideFunc;
			immediateWindow.doWindowBackground = doBackground;
			immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
			immediateWindow.shadowAlpha = shadowAlpha;
			immediateWindow.ignoreScreenFader = ignoreScreenFader;
			immediateWindow.PreOpen();
			immediateWindow.windowRect = rect;
			this.InsertAtCorrectPositionInList(immediateWindow);
			this.FocusAfterInsertIfShould(immediateWindow);
			this.updateInternalWindowsOrderLater = true;
			immediateWindow.PostOpen();
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x0015208C File Offset: 0x0015028C
		private void UpdateImmediateWindowsList()
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			this.updateImmediateWindowsListTmpList.Clear();
			this.updateImmediateWindowsListTmpList.AddRange(this.windows);
			for (int i = 0; i < this.updateImmediateWindowsListTmpList.Count; i++)
			{
				if (this.IsImmediateWindow(this.updateImmediateWindowsListTmpList[i]))
				{
					bool flag = false;
					for (int j = 0; j < this.immediateWindowsRequests.Count; j++)
					{
						if (this.immediateWindowsRequests[j] == this.updateImmediateWindowsListTmpList[i].ID)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.TryRemove(this.updateImmediateWindowsListTmpList[i], true);
					}
				}
			}
			this.immediateWindowsRequests.Clear();
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x0015214C File Offset: 0x0015034C
		private void InsertAtCorrectPositionInList(Window window)
		{
			int index = 0;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (window.layer >= this.windows[i].layer)
				{
					index = i + 1;
				}
			}
			this.windows.Insert(index, window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x001521A4 File Offset: 0x001503A4
		private void FocusAfterInsertIfShould(Window window)
		{
			if (!window.focusWhenOpened)
			{
				return;
			}
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					this.focusedWindow = this.windows[i];
					this.updateInternalWindowsOrderLater = true;
					return;
				}
				if (this.windows[i] == this.focusedWindow)
				{
					break;
				}
			}
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x00152210 File Offset: 0x00150410
		private void AdjustWindowsIfResolutionChanged()
		{
			IntVec2 a = new IntVec2(UI.screenWidth, UI.screenHeight);
			if (!UnityGUIBugsFixer.ResolutionsEqual(a, this.prevResolution))
			{
				this.prevResolution = a;
				for (int i = 0; i < this.windows.Count; i++)
				{
					this.windows[i].Notify_ResolutionChanged();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
			}
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x0015227C File Offset: 0x0015047C
		private void RemoveWindowsOfType(Type type)
		{
			this.removeWindowsOfTypeTmpList.Clear();
			this.removeWindowsOfTypeTmpList.AddRange(this.windows);
			for (int i = 0; i < this.removeWindowsOfTypeTmpList.Count; i++)
			{
				if (this.removeWindowsOfTypeTmpList[i].onlyOneOfTypeAllowed && this.removeWindowsOfTypeTmpList[i].GetType() == type)
				{
					this.TryRemove(this.removeWindowsOfTypeTmpList[i], true);
				}
			}
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x001522FC File Offset: 0x001504FC
		private void NotifyOutsideClicks(Window clickedWindow)
		{
			foreach (Window window in this.windows)
			{
				if (window != clickedWindow)
				{
					window.Notify_ClickOutsideWindow();
				}
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x00152354 File Offset: 0x00150554
		private bool CloseWindowsBecauseClicked(Window clickedWindow)
		{
			this.closeWindowsTmpList.Clear();
			this.closeWindowsTmpList.AddRange(this.windows);
			bool result = false;
			int num = this.closeWindowsTmpList.Count - 1;
			while (num >= 0 && this.closeWindowsTmpList[num] != clickedWindow)
			{
				if (this.closeWindowsTmpList[num].closeOnClickedOutside)
				{
					result = true;
					this.TryRemove(this.closeWindowsTmpList[num], true);
				}
				num--;
			}
			return result;
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x001523D0 File Offset: 0x001505D0
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x001523DC File Offset: 0x001505DC
		private void UpdateInternalWindowsOrder()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				GUI.BringWindowToFront(this.windows[i].ID);
			}
			if (this.focusedWindow != null)
			{
				GUI.FocusWindow(this.focusedWindow.ID);
			}
		}

		// Token: 0x0400225C RID: 8796
		public Window currentlyDrawnWindow;

		// Token: 0x0400225D RID: 8797
		private List<Window> windows = new List<Window>();

		// Token: 0x0400225E RID: 8798
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x0400225F RID: 8799
		private bool updateInternalWindowsOrderLater;

		// Token: 0x04002260 RID: 8800
		private Window focusedWindow;

		// Token: 0x04002261 RID: 8801
		private static int uniqueWindowID;

		// Token: 0x04002262 RID: 8802
		private bool gameStartDialogOpen;

		// Token: 0x04002263 RID: 8803
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x04002264 RID: 8804
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x04002265 RID: 8805
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x04002266 RID: 8806
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x04002267 RID: 8807
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x04002268 RID: 8808
		private List<Window> closeWindowsTmpList = new List<Window>();
	}
}
