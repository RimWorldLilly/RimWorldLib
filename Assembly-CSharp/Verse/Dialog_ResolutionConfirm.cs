using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000678 RID: 1656
	public class Dialog_ResolutionConfirm : Window
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x0600342B RID: 13355 RVA: 0x0015058A File Offset: 0x0014E78A
		private float TimeUntilRevert
		{
			get
			{
				return this.startTime + 10f - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600342C RID: 13356 RVA: 0x0015059E File Offset: 0x0014E79E
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 300f);
			}
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x001505AF File Offset: 0x0014E7AF
		private Dialog_ResolutionConfirm() : base(null)
		{
			this.startTime = Time.realtimeSinceStartup;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x001505D8 File Offset: 0x0014E7D8
		public Dialog_ResolutionConfirm(FullScreenMode oldFullScreenMode, IntVec2 oldRes) : this()
		{
			this.oldFullscreen = (oldFullScreenMode != FullScreenMode.Windowed);
			this.oldFullScreenMode = oldFullScreenMode;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScalePreference;
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x00150606 File Offset: 0x0014E806
		public Dialog_ResolutionConfirm(bool oldFullscreen) : this()
		{
			this.oldFullscreen = oldFullscreen;
			this.oldFullScreenMode = (oldFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = Prefs.UIScalePreference;
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x00150642 File Offset: 0x0014E842
		public Dialog_ResolutionConfirm(IntVec2 oldRes) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldFullScreenMode = Screen.fullScreenMode;
			this.oldRes = oldRes;
			this.oldUIScale = Prefs.UIScalePreference;
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x00150672 File Offset: 0x0014E872
		public Dialog_ResolutionConfirm(float oldUIScale) : this()
		{
			this.oldFullscreen = Screen.fullScreen;
			this.oldFullScreenMode = Screen.fullScreenMode;
			this.oldRes = new IntVec2(Screen.width, Screen.height);
			this.oldUIScale = oldUIScale;
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x001506AC File Offset: 0x0014E8AC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			string label = "ConfirmResolutionChange".Translate(Mathf.CeilToInt(this.TimeUntilRevert));
			Widgets.Label(new Rect(0f, 0f, inRect.width, inRect.height), label);
			if (Widgets.ButtonText(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionKeep".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "ResolutionRevert".Translate(), true, true, true, null))
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x001507C8 File Offset: 0x0014E9C8
		private void Revert()
		{
			if (Prefs.LogVerbose)
			{
				Log.Message(string.Concat(new string[]
				{
					"Reverting screen settings to ",
					this.oldRes.x.ToString(),
					"x",
					this.oldRes.z.ToString(),
					", fs=",
					this.oldFullscreen.ToString()
				}));
			}
			ResolutionUtility.SetResolutionRaw(this.oldRes.x, this.oldRes.z, this.oldFullScreenMode);
			Prefs.FullScreenMode = this.oldFullScreenMode;
			Prefs.ScreenWidth = this.oldRes.x;
			Prefs.ScreenHeight = this.oldRes.z;
			Prefs.UIScalePreference = this.oldUIScale;
			GenUI.ClearLabelWidthCache();
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x00150894 File Offset: 0x0014EA94
		public override void WindowUpdate()
		{
			if (this.TimeUntilRevert <= 0f)
			{
				this.Revert();
				this.Close(true);
			}
		}

		// Token: 0x04002215 RID: 8725
		private float startTime;

		// Token: 0x04002216 RID: 8726
		private IntVec2 oldRes;

		// Token: 0x04002217 RID: 8727
		private bool oldFullscreen;

		// Token: 0x04002218 RID: 8728
		private FullScreenMode oldFullScreenMode;

		// Token: 0x04002219 RID: 8729
		private float oldUIScale;

		// Token: 0x0400221A RID: 8730
		private const float RevertTime = 10f;
	}
}
