using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200067E RID: 1662
	public abstract class Window
	{
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x0600344E RID: 13390 RVA: 0x00150CA1 File Offset: 0x0014EEA1
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x00150CB2 File Offset: 0x0014EEB2
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x00150CB9 File Offset: 0x0014EEB9
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06003451 RID: 13393 RVA: 0x00150CBC File Offset: 0x0014EEBC
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x00150CC9 File Offset: 0x0014EEC9
		public virtual QuickSearchWidget CommonSearchWidget
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x00150CCC File Offset: 0x0014EECC
		public virtual string CloseButtonText
		{
			get
			{
				return "CloseButton".Translate();
			}
		}

		// Token: 0x06003454 RID: 13396 RVA: 0x00150CE0 File Offset: 0x0014EEE0
		public Window(IWindowDrawing customWindowDrawing = null)
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
			this.onGUIProfilerLabelCached = "WindowOnGUI: " + base.GetType().Name;
			this.extraOnGUIProfilerLabelCached = "ExtraOnGUI: " + base.GetType().Name;
			this.innerWindowOnGUICached = new GUI.WindowFunction(this.InnerWindowOnGUI);
			this.notify_CommonSearchChangedCached = new Action(this.Notify_CommonSearchChanged);
			this.windowDrawing = (customWindowDrawing ?? new DefaultWindowDrawing());
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x00150DED File Offset: 0x0014EFED
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		// Token: 0x06003456 RID: 13398
		public abstract void DoWindowContents(Rect inRect);

		// Token: 0x06003457 RID: 13399 RVA: 0x00150E02 File Offset: 0x0014F002
		public virtual void ExtraOnGUI()
		{
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x00150E04 File Offset: 0x0014F004
		public virtual void PreOpen()
		{
			this.SetInitialSizeAndPosition();
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget != null)
			{
				commonSearchWidget.Reset();
			}
			if (this.layer == WindowLayer.Dialog)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.DesignatorManager.Dragger.EndDrag();
					Find.DesignatorManager.Deselect();
					Find.Selector.Notify_DialogOpened();
				}
				if (Find.World != null)
				{
					Find.WorldSelector.Notify_DialogOpened();
				}
			}
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x00150E6D File Offset: 0x0014F06D
		public virtual void PostOpen()
		{
			if (this.soundAppear != null)
			{
				this.soundAppear.PlayOneShotOnCamera(null);
			}
			if (this.soundAmbient != null)
			{
				this.sustainerAmbient = this.soundAmbient.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
			}
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x00150EA2 File Offset: 0x0014F0A2
		public virtual bool OnCloseRequest()
		{
			return true;
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x00150EA5 File Offset: 0x0014F0A5
		public virtual void PreClose()
		{
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x00150EA7 File Offset: 0x0014F0A7
		public virtual void PostClose()
		{
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x00150EAC File Offset: 0x0014F0AC
		public virtual void WindowOnGUI()
		{
			if (!this.drawInScreenshotMode && Find.UIRoot.screenshotMode.Active)
			{
				return;
			}
			if (this.onlyDrawInDevMode && !Prefs.DevMode)
			{
				return;
			}
			if (this.resizeable)
			{
				if (this.resizer == null)
				{
					this.resizer = new WindowResizer();
				}
				if (this.resizeLater)
				{
					this.resizeLater = false;
					this.windowRect = this.resizeLaterRect;
				}
			}
			this.windowRect = this.windowRect.Rounded();
			this.windowRect = GUI.Window(this.ID, this.windowRect, this.innerWindowOnGUICached, "", this.windowDrawing.EmptyStyle);
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x00150F58 File Offset: 0x0014F158
		private void InnerWindowOnGUI(int x)
		{
			UnityGUIBugsFixer.OnGUI();
			SteamDeck.WindowOnGUI();
			OriginalEventUtility.RecordOriginalEvent(Event.current);
			Rect rect = this.windowRect.AtZero();
			Find.WindowStack.currentlyDrawnWindow = this;
			if (this.doWindowBackground)
			{
				this.windowDrawing.DoWindowBackground(rect);
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedAccept();
			}
			if (Event.current.type == EventType.mouseDown)
			{
				Find.WindowStack.Notify_ClickedInsideWindow(this);
			}
			if (Event.current.type == EventType.keyDown && !Find.WindowStack.GetsInput(this))
			{
				Event.current.Use();
			}
			if (!this.optionalTitle.NullOrEmpty())
			{
				GUI.Label(new Rect(this.Margin, this.Margin, this.windowRect.width, 25f), this.optionalTitle);
			}
			if (this.doCloseX && this.windowDrawing.DoClostButtonSmall(rect))
			{
				this.Close(true);
			}
			if (this.resizeable && Event.current.type != EventType.repaint)
			{
				Rect lhs = this.resizer.DoResizeControl(this.windowRect);
				if (lhs != this.windowRect)
				{
					this.resizeLater = true;
					this.resizeLaterRect = lhs;
				}
			}
			Rect rect2 = rect.ContractedBy(this.Margin);
			if (!this.optionalTitle.NullOrEmpty())
			{
				rect2.yMin += this.Margin + 25f;
			}
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget != null)
			{
				commonSearchWidget.OnGUI(this.QuickSearchWidgetRect(rect, rect2), this.notify_CommonSearchChangedCached, null);
			}
			this.windowDrawing.BeginGroup(rect2);
			try
			{
				this.DoWindowContents(rect2.AtZero());
			}
			catch (Exception ex)
			{
				string str = "Exception filling window for ";
				Type type = base.GetType();
				string str2 = (type != null) ? type.ToString() : null;
				string str3 = ": ";
				Exception ex2 = ex;
				Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
			}
			this.windowDrawing.EndGroup();
			this.LateWindowOnGUI(rect2);
			if (this.grayOutIfOtherDialogOpen)
			{
				IList<Window> windows = Find.WindowStack.Windows;
				for (int i = 0; i < windows.Count; i++)
				{
					if (windows[i].layer == WindowLayer.Dialog && !(windows[i] is Page) && windows[i] != this)
					{
						this.windowDrawing.DoGrayOut(rect);
						break;
					}
				}
			}
			if (this.resizeable && Event.current.type == EventType.repaint)
			{
				this.resizer.DoResizeControl(this.windowRect);
			}
			if (this.doCloseButton)
			{
				Text.Font = GameFont.Small;
				Rect rect3 = new Rect(rect.width / 2f - Window.CloseButSize.x / 2f, rect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y);
				if (this.windowDrawing.DoCloseButton(rect3, this.CloseButtonText))
				{
					this.Close(true);
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsOpen)
			{
				this.OnCancelKeyPressed();
			}
			if (this.draggable)
			{
				GUI.DragWindow();
			}
			else if (Event.current.type == EventType.mouseDown)
			{
				Event.current.Use();
			}
			if (!this.ignoreScreenFader)
			{
				ScreenFader.OverlayOnGUI(rect.size);
			}
			Find.WindowStack.currentlyDrawnWindow = null;
			OriginalEventUtility.Reset();
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x001512CC File Offset: 0x0014F4CC
		protected virtual Rect QuickSearchWidgetRect(Rect winRect, Rect inRect)
		{
			Vector2 vector = this.commonSearchWidgetOffset;
			return new Rect(winRect.x + vector.x, winRect.height - 55f + vector.y, Window.QuickSearchSize.x, Window.QuickSearchSize.y);
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x0015131B File Offset: 0x0014F51B
		protected virtual void LateWindowOnGUI(Rect inRect)
		{
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x0015131D File Offset: 0x0014F51D
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x0015132C File Offset: 0x0014F52C
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x00151330 File Offset: 0x0014F530
		protected virtual void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			this.windowRect = new Rect(((float)UI.screenWidth - initialSize.x) / 2f, ((float)UI.screenHeight - initialSize.y) / 2f, initialSize.x, initialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x00151392 File Offset: 0x0014F592
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
			if (this.openMenuOnCancel)
			{
				Find.MainTabsRoot.ToggleTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x001513C5 File Offset: 0x0014F5C5
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x001513E0 File Offset: 0x0014F5E0
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x001513E8 File Offset: 0x0014F5E8
		public virtual void Notify_CommonSearchChanged()
		{
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x001513EA File Offset: 0x0014F5EA
		public virtual void Notify_ClickOutsideWindow()
		{
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget == null)
			{
				return;
			}
			commonSearchWidget.Unfocus();
		}

		// Token: 0x0400222B RID: 8747
		public WindowLayer layer = WindowLayer.Dialog;

		// Token: 0x0400222C RID: 8748
		public string optionalTitle;

		// Token: 0x0400222D RID: 8749
		public bool doCloseX;

		// Token: 0x0400222E RID: 8750
		public bool doCloseButton;

		// Token: 0x0400222F RID: 8751
		public bool closeOnAccept = true;

		// Token: 0x04002230 RID: 8752
		public bool closeOnCancel = true;

		// Token: 0x04002231 RID: 8753
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		// Token: 0x04002232 RID: 8754
		public bool closeOnClickedOutside;

		// Token: 0x04002233 RID: 8755
		public bool forcePause;

		// Token: 0x04002234 RID: 8756
		public bool preventCameraMotion = true;

		// Token: 0x04002235 RID: 8757
		public bool preventDrawTutor;

		// Token: 0x04002236 RID: 8758
		public bool doWindowBackground = true;

		// Token: 0x04002237 RID: 8759
		public bool onlyOneOfTypeAllowed = true;

		// Token: 0x04002238 RID: 8760
		public bool absorbInputAroundWindow;

		// Token: 0x04002239 RID: 8761
		public bool resizeable;

		// Token: 0x0400223A RID: 8762
		public bool draggable;

		// Token: 0x0400223B RID: 8763
		public bool drawShadow = true;

		// Token: 0x0400223C RID: 8764
		public bool focusWhenOpened = true;

		// Token: 0x0400223D RID: 8765
		public float shadowAlpha = 1f;

		// Token: 0x0400223E RID: 8766
		public SoundDef soundAppear;

		// Token: 0x0400223F RID: 8767
		public SoundDef soundClose;

		// Token: 0x04002240 RID: 8768
		public SoundDef soundAmbient;

		// Token: 0x04002241 RID: 8769
		public bool silenceAmbientSound;

		// Token: 0x04002242 RID: 8770
		public bool grayOutIfOtherDialogOpen;

		// Token: 0x04002243 RID: 8771
		public Vector2 commonSearchWidgetOffset = new Vector2(0f, Window.CloseButSize.y - Window.QuickSearchSize.y) / 2f;

		// Token: 0x04002244 RID: 8772
		public bool openMenuOnCancel;

		// Token: 0x04002245 RID: 8773
		public bool preventSave;

		// Token: 0x04002246 RID: 8774
		public bool drawInScreenshotMode = true;

		// Token: 0x04002247 RID: 8775
		public bool onlyDrawInDevMode;

		// Token: 0x04002248 RID: 8776
		public bool ignoreScreenFader;

		// Token: 0x04002249 RID: 8777
		public const float StandardMargin = 18f;

		// Token: 0x0400224A RID: 8778
		public const float FooterRowHeight = 55f;

		// Token: 0x0400224B RID: 8779
		public static readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		// Token: 0x0400224C RID: 8780
		public static readonly Vector2 QuickSearchSize = new Vector2(240f, 24f);

		// Token: 0x0400224D RID: 8781
		public int ID;

		// Token: 0x0400224E RID: 8782
		public Rect windowRect;

		// Token: 0x0400224F RID: 8783
		private Sustainer sustainerAmbient;

		// Token: 0x04002250 RID: 8784
		private WindowResizer resizer;

		// Token: 0x04002251 RID: 8785
		private bool resizeLater;

		// Token: 0x04002252 RID: 8786
		private Rect resizeLaterRect;

		// Token: 0x04002253 RID: 8787
		private IWindowDrawing windowDrawing;

		// Token: 0x04002254 RID: 8788
		private string onGUIProfilerLabelCached;

		// Token: 0x04002255 RID: 8789
		public string extraOnGUIProfilerLabelCached;

		// Token: 0x04002256 RID: 8790
		private GUI.WindowFunction innerWindowOnGUICached;

		// Token: 0x04002257 RID: 8791
		private Action notify_CommonSearchChangedCached;
	}
}
