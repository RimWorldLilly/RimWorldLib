using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB1 RID: 7345
	public class Dialog_Ideo : Window
	{
		// Token: 0x17002248 RID: 8776
		// (get) Token: 0x0600BC73 RID: 48243 RVA: 0x00435E37 File Offset: 0x00434037
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x0600BC74 RID: 48244 RVA: 0x00435E53 File Offset: 0x00434053
		public Dialog_Ideo(Ideo ideo) : base(null)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.ideo = ideo;
		}

		// Token: 0x0600BC75 RID: 48245 RVA: 0x00435E86 File Offset: 0x00434086
		public override void PostClose()
		{
			base.PostClose();
			IdeoUIUtility.UnselectCurrent();
		}

		// Token: 0x0600BC76 RID: 48246 RVA: 0x00435E94 File Offset: 0x00434094
		public override void DoWindowContents(Rect inRect)
		{
			inRect.height -= Window.CloseButSize.y;
			IdeoUIUtility.DoIdeoDetails(inRect, this.ideo, ref this.scrollPosition, ref this.viewHeight, false, null, true, true, false, false);
		}

		// Token: 0x04006BB6 RID: 27574
		private Ideo ideo;

		// Token: 0x04006BB7 RID: 27575
		private float viewHeight;

		// Token: 0x04006BB8 RID: 27576
		private Vector2 scrollPosition;
	}
}
