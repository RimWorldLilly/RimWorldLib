using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAD RID: 7341
	public class Dialog_FactionDuringLanding : Window
	{
		// Token: 0x1700222D RID: 8749
		// (get) Token: 0x0600BC1C RID: 48156 RVA: 0x00431EC2 File Offset: 0x004300C2
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, 684f);
			}
		}

		// Token: 0x0600BC1D RID: 48157 RVA: 0x00431ED3 File Offset: 0x004300D3
		public Dialog_FactionDuringLanding() : base(null)
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BC1E RID: 48158 RVA: 0x00431EFC File Offset: 0x004300FC
		public override void DoWindowContents(Rect inRect)
		{
			FactionUIUtility.DoWindowContents(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - Window.CloseButSize.y), ref this.scrollPosition, ref this.scrollViewHeight, null);
		}

		// Token: 0x04006B70 RID: 27504
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04006B71 RID: 27505
		private float scrollViewHeight;
	}
}
