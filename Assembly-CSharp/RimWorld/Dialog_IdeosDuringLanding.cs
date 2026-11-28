using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB2 RID: 7346
	public class Dialog_IdeosDuringLanding : Window
	{
		// Token: 0x17002249 RID: 8777
		// (get) Token: 0x0600BC77 RID: 48247 RVA: 0x00435ED7 File Offset: 0x004340D7
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1010f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x0600BC78 RID: 48248 RVA: 0x00435EF3 File Offset: 0x004340F3
		public Dialog_IdeosDuringLanding() : base(null)
		{
			this.doCloseButton = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BC79 RID: 48249 RVA: 0x00435F14 File Offset: 0x00434114
		public override void DoWindowContents(Rect inRect)
		{
			IdeoUIUtility.DoIdeoListAndDetails(new Rect(inRect.x, inRect.y, inRect.width, inRect.height - Window.CloseButSize.y), ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, false, false, null, null, null, false, null, null, false, false, null);
		}

		// Token: 0x04006BB9 RID: 27577
		private Vector2 scrollPosition_ideoList;

		// Token: 0x04006BBA RID: 27578
		private float scrollViewHeight_ideoList;

		// Token: 0x04006BBB RID: 27579
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x04006BBC RID: 27580
		private float scrollViewHeight_ideoDetails;
	}
}
