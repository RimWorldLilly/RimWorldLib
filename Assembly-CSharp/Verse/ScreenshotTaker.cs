using System;
using System.IO;
using System.Runtime.CompilerServices;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000724 RID: 1828
	public static class ScreenshotTaker
	{
		// Token: 0x060039C8 RID: 14792 RVA: 0x001700D8 File Offset: 0x0016E2D8
		public static void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (KeyBindingDefOf.TakeScreenshot.JustPressed || ScreenshotTaker.takeScreenshot)
			{
				ScreenshotTaker.TakeShot();
				ScreenshotTaker.takeScreenshot = false;
			}
			if (Time.frameCount == ScreenshotTaker.lastShotFrame + 1)
			{
				if (ScreenshotTaker.suppressMessage)
				{
					ScreenshotTaker.suppressMessage = false;
					return;
				}
				Messages.Message("MessageScreenshotSavedAs".Translate(ScreenshotTaker.lastShotFilePath), MessageTypeDefOf.TaskCompletion, false);
			}
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x0017014B File Offset: 0x0016E34B
		public static void QueueSilentScreenshot()
		{
			ScreenshotTaker.takeScreenshot = (ScreenshotTaker.suppressMessage = true);
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x0017015C File Offset: 0x0016E35C
		private static void TakeShot()
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				try
				{
					SteamScreenshots.TriggerScreenshot();
					return;
				}
				catch (Exception ex)
				{
					string str = "Could not take Steam screenshot. Steam offline? Taking normal screenshot. Exception: ";
					Exception ex2 = ex;
					Log.Warning(str + ((ex2 != null) ? ex2.ToString() : null));
					ScreenshotTaker.TakeNonSteamShot(null);
					return;
				}
			}
			ScreenshotTaker.TakeNonSteamShot(null);
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x001701BC File Offset: 0x0016E3BC
		public static void TakeNonSteamShot(string fileName = null)
		{
			string screenshotFolderPath = GenFilePaths.ScreenshotFolderPath;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(screenshotFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				string text;
				if (fileName != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
					defaultInterpolatedStringHandler.AppendFormatted(screenshotFolderPath);
					defaultInterpolatedStringHandler.AppendFormatted<char>(Path.DirectorySeparatorChar);
					defaultInterpolatedStringHandler.AppendFormatted(fileName);
					defaultInterpolatedStringHandler.AppendLiteral(".png");
					text = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					do
					{
						ScreenshotTaker.screenshotCount++;
						text = string.Concat(new string[]
						{
							screenshotFolderPath,
							Path.DirectorySeparatorChar.ToString(),
							"screenshot",
							ScreenshotTaker.screenshotCount.ToString(),
							".png"
						});
					}
					while (File.Exists(text));
				}
				ScreenCapture.CaptureScreenshot(text);
				ScreenshotTaker.lastShotFrame = Time.frameCount;
				ScreenshotTaker.lastShotFilePath = text;
			}
			catch (Exception ex)
			{
				Log.Error("Failed to save screenshot in " + screenshotFolderPath + "\nException follows:\n" + ex.ToString());
			}
		}

		// Token: 0x04002613 RID: 9747
		private static int lastShotFrame = -999;

		// Token: 0x04002614 RID: 9748
		private static int screenshotCount = 0;

		// Token: 0x04002615 RID: 9749
		private static string lastShotFilePath;

		// Token: 0x04002616 RID: 9750
		private static bool suppressMessage;

		// Token: 0x04002617 RID: 9751
		private static bool takeScreenshot;
	}
}
