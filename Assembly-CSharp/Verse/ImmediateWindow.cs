using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200067A RID: 1658
	public class ImmediateWindow : Window
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x0600343A RID: 13370 RVA: 0x00150BA1 File Offset: 0x0014EDA1
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowRect.size;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600343B RID: 13371 RVA: 0x00150BAE File Offset: 0x0014EDAE
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x00150BB8 File Offset: 0x0014EDB8
		public ImmediateWindow() : base(null)
		{
			this.doCloseButton = false;
			this.doCloseX = false;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.focusWhenOpened = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x00150C0B File Offset: 0x0014EE0B
		public override void DoWindowContents(Rect inRect)
		{
			this.doWindowFunc();
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x00150C18 File Offset: 0x0014EE18
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			Action action = this.doClickOutsideFunc;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04002224 RID: 8740
		public Action doWindowFunc;

		// Token: 0x04002225 RID: 8741
		public Action doClickOutsideFunc;
	}
}
