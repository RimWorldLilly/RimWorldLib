using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000056 RID: 86
	[StaticConstructorOnStartup]
	public class EditWindow_Log : EditWindow
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00012433 File Offset: 0x00010633
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00012452 File Offset: 0x00010652
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00012455 File Offset: 0x00010655
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0001245C File Offset: 0x0001065C
		private static LogMessage SelectedMessage
		{
			get
			{
				return EditWindow_Log.selectedMessage;
			}
			set
			{
				if (EditWindow_Log.selectedMessage == value)
				{
					return;
				}
				EditWindow_Log.selectedMessage = value;
				if (UnityData.IsInMainThread && GUI.GetNameOfFocusedControl() == EditWindow_Log.MessageDetailsControlName)
				{
					UI.UnfocusCurrentControl();
				}
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0001248A File Offset: 0x0001068A
		public EditWindow_Log()
		{
			this.optionalTitle = "Debug log";
			this.closeOnAccept = false;
			this.closeOnCancel = Prefs.CloseLogWindowOnEscape;
			this.drawInScreenshotMode = true;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000124B6 File Offset: 0x000106B6
		public static void TryAutoOpen()
		{
			if (EditWindow_Log.canAutoOpen)
			{
				EditWindow_Log.wantsToOpen = true;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000124C5 File Offset: 0x000106C5
		public static void ClearSelectedMessage()
		{
			EditWindow_Log.SelectedMessage = null;
			EditWindow_Log.detailsScrollPosition = Vector2.zero;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x000124D7 File Offset: 0x000106D7
		public static void SelectLastMessage(bool expandDetailsPane = false)
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.SelectedMessage = Log.Messages.LastOrDefault<LogMessage>();
			EditWindow_Log.messagesScrollPosition.y = (float)Log.Messages.Count<LogMessage>() * 30f;
			if (expandDetailsPane)
			{
				EditWindow_Log.detailsPaneHeight = 9999f;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00012515 File Offset: 0x00010715
		public static void ClearAll()
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.messagesScrollPosition = Vector2.zero;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00012526 File Offset: 0x00010726
		public override void PostClose()
		{
			base.PostClose();
			EditWindow_Log.wantsToOpen = false;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00012534 File Offset: 0x00010734
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			float x = inRect.x;
			float y = inRect.y;
			base.DoRowButton(ref x, y, "Clear", "Clear all log messages.", delegate
			{
				Log.Clear();
				EditWindow_Log.ClearAll();
			});
			base.DoRowButton(ref x, y, "Trace big", "Set the stack trace to be large on screen.", delegate
			{
				EditWindow_Log.detailsPaneHeight = 700f;
			});
			base.DoRowButton(ref x, y, "Trace medium", "Set the stack trace to be medium-sized on screen.", delegate
			{
				EditWindow_Log.detailsPaneHeight = 300f;
			});
			base.DoRowButton(ref x, y, "Trace small", "Set the stack trace to be small on screen.", delegate
			{
				EditWindow_Log.detailsPaneHeight = 100f;
			});
			if (EditWindow_Log.canAutoOpen)
			{
				base.DoRowButton(ref x, y, "Auto-open is ON", null, delegate
				{
					EditWindow_Log.canAutoOpen = false;
				});
			}
			else
			{
				base.DoRowButton(ref x, y, "Auto-open is OFF", null, delegate
				{
					EditWindow_Log.canAutoOpen = true;
				});
			}
			base.DoRowButton(ref x, y, "Copy to clipboard", "Copy all messages to the clipboard.", delegate
			{
				this.CopyAllMessagesToClipboard();
			});
			if (DebugSettings.pauseOnError)
			{
				base.DoRowButton(ref x, y, "Pause on error is ON", null, delegate
				{
					DebugSettings.pauseOnError = false;
				});
			}
			else
			{
				base.DoRowButton(ref x, y, "Pause on error is OFF", null, delegate
				{
					DebugSettings.pauseOnError = true;
				});
			}
			base.DoImageToggle(ref x, y, EditWindow_Log.InfoIcon, "Show info messages", ref EditWindow_Log.showMessages);
			base.DoImageToggle(ref x, y, EditWindow_Log.WarningIcon, "Show warning messages", ref EditWindow_Log.showWarnings);
			base.DoImageToggle(ref x, y, EditWindow_Log.ErrorIcon, "Show error messages", ref EditWindow_Log.showErrors);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect);
			rect.yMin += 26f;
			rect.yMax = inRect.height;
			if (EditWindow_Log.selectedMessage != null)
			{
				rect.yMax -= EditWindow_Log.detailsPaneHeight;
			}
			Rect detailsRect = new Rect(inRect);
			detailsRect.yMin = rect.yMax;
			this.DoMessagesListing(rect);
			this.DoMessageDetails(detailsRect, inRect);
			if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				EditWindow_Log.ClearSelectedMessage();
			}
			EditWindow_Log.detailsPaneHeight = Mathf.Max(EditWindow_Log.detailsPaneHeight, 10f);
			EditWindow_Log.detailsPaneHeight = Mathf.Min(EditWindow_Log.detailsPaneHeight, inRect.height - 80f);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00012813 File Offset: 0x00010A13
		public static void Notify_MessageDequeued(LogMessage oldMessage)
		{
			if (EditWindow_Log.SelectedMessage == oldMessage)
			{
				EditWindow_Log.SelectedMessage = null;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00012824 File Offset: 0x00010A24
		private void DoMessagesListing(Rect listingRect)
		{
			Rect viewRect = new Rect(0f, 0f, listingRect.width - 16f, this.listingViewHeight + 100f);
			DevGUI.BeginScrollView(listingRect, ref EditWindow_Log.messagesScrollPosition, viewRect, true);
			float width = viewRect.width - 28f;
			Text.Font = GameFont.Tiny;
			float num = 0f;
			bool flag = false;
			foreach (LogMessage logMessage in Log.Messages)
			{
				if ((!EditWindow_Log.showMessages && logMessage.type == LogMessageType.Message) || (!EditWindow_Log.showWarnings && logMessage.type == LogMessageType.Warning) || (!EditWindow_Log.showErrors && logMessage.type == LogMessageType.Error))
				{
					if (EditWindow_Log.selectedMessage == logMessage)
					{
						EditWindow_Log.ClearSelectedMessage();
					}
				}
				else
				{
					string text = logMessage.ToString();
					if (text.Length > 1000)
					{
						text = text.Substring(0, 1000);
					}
					float num2 = Math.Min(Text.TinyFontSupported ? 30f : Text.LineHeight, Text.CalcHeight(text, width));
					GUI.color = new Color(1f, 1f, 1f, 0.7f);
					DevGUI.Label(new Rect(4f, num, 28f, num2), logMessage.repeats.ToStringCached());
					Rect rect = new Rect(28f, num, width, num2);
					if (EditWindow_Log.selectedMessage == logMessage)
					{
						GUI.DrawTexture(rect, EditWindow_Log.SelectedMessageTex);
					}
					else if (flag)
					{
						GUI.DrawTexture(rect, EditWindow_Log.AltMessageTex);
					}
					if (DevGUI.ButtonInvisible(rect))
					{
						EditWindow_Log.ClearSelectedMessage();
						EditWindow_Log.SelectedMessage = logMessage;
					}
					GUI.color = logMessage.Color;
					DevGUI.Label(rect, text);
					num += num2;
					flag = !flag;
				}
			}
			if (Event.current.type == EventType.layout)
			{
				this.listingViewHeight = num;
			}
			DevGUI.EndScrollView();
			GUI.color = Color.white;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00012A34 File Offset: 0x00010C34
		private void DoMessageDetails(Rect detailsRect, Rect outRect)
		{
			if (EditWindow_Log.selectedMessage == null)
			{
				return;
			}
			Rect rect = detailsRect;
			rect.height = 7f;
			Rect rect2 = detailsRect;
			rect2.yMin = rect.yMax;
			GUI.DrawTexture(rect, EditWindow_Log.StackTraceBorderTex);
			if (Mouse.IsOver(rect))
			{
				DevGUI.DrawHighlight(rect);
			}
			if (Event.current.type == EventType.mouseDown && Mouse.IsOver(rect))
			{
				this.borderDragging = true;
				Event.current.Use();
			}
			if (this.borderDragging)
			{
				EditWindow_Log.detailsPaneHeight = outRect.height + Mathf.Round(3.5f) - Event.current.mousePosition.y;
			}
			if (Event.current.rawType == EventType.mouseUp)
			{
				this.borderDragging = false;
			}
			GUI.DrawTexture(rect2, EditWindow_Log.StackTraceAreaTex);
			string text = EditWindow_Log.selectedMessage.text + "\n" + EditWindow_Log.selectedMessage.StackTrace;
			GUI.SetNextControlName(EditWindow_Log.MessageDetailsControlName);
			DevGUI.TextAreaScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, true);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00012B2C File Offset: 0x00010D2C
		private void CopyAllMessagesToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LogMessage logMessage in Log.Messages)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(logMessage.text);
				stringBuilder.Append(logMessage.StackTrace);
				if (stringBuilder[stringBuilder.Length - 1] != '\n')
				{
					stringBuilder.AppendLine();
				}
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x04000139 RID: 313
		private static LogMessage selectedMessage = null;

		// Token: 0x0400013A RID: 314
		private static Vector2 messagesScrollPosition;

		// Token: 0x0400013B RID: 315
		private static Vector2 detailsScrollPosition;

		// Token: 0x0400013C RID: 316
		private static float detailsPaneHeight = 100f;

		// Token: 0x0400013D RID: 317
		private static bool canAutoOpen = true;

		// Token: 0x0400013E RID: 318
		private static bool showErrors = true;

		// Token: 0x0400013F RID: 319
		private static bool showWarnings = true;

		// Token: 0x04000140 RID: 320
		private static bool showMessages = true;

		// Token: 0x04000141 RID: 321
		public static bool wantsToOpen = false;

		// Token: 0x04000142 RID: 322
		private float listingViewHeight;

		// Token: 0x04000143 RID: 323
		private bool borderDragging;

		// Token: 0x04000144 RID: 324
		private const float CountWidth = 28f;

		// Token: 0x04000145 RID: 325
		private const float Yinc = 25f;

		// Token: 0x04000146 RID: 326
		private const float DetailsPaneBorderHeight = 7f;

		// Token: 0x04000147 RID: 327
		private const float DetailsPaneMinHeight = 10f;

		// Token: 0x04000148 RID: 328
		private const float ListingMinHeight = 80f;

		// Token: 0x04000149 RID: 329
		private const float TopAreaHeight = 26f;

		// Token: 0x0400014A RID: 330
		private const float MessageMaxHeight = 30f;

		// Token: 0x0400014B RID: 331
		private static readonly Texture2D AltMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.17f, 0.17f, 0.17f, 0.85f));

		// Token: 0x0400014C RID: 332
		private static readonly Texture2D SelectedMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.25f, 0.25f, 0.17f, 0.85f));

		// Token: 0x0400014D RID: 333
		private static readonly Texture2D StackTraceAreaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.5f));

		// Token: 0x0400014E RID: 334
		private static readonly Texture2D StackTraceBorderTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 1f));

		// Token: 0x0400014F RID: 335
		private static readonly Texture2D WarningIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/YellowWarning");

		// Token: 0x04000150 RID: 336
		private static readonly Texture2D ErrorIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/Error");

		// Token: 0x04000151 RID: 337
		private static readonly Texture2D InfoIcon = Resources.Load<Texture2D>("Textures/UI/Buttons/InfoButton");

		// Token: 0x04000152 RID: 338
		private static readonly string MessageDetailsControlName = "MessageDetailsTextArea";
	}
}
