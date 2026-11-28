using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9A RID: 7578
	public class MainTabWindow_Ideos : MainTabWindow
	{
		// Token: 0x17002485 RID: 9349
		// (get) Token: 0x0600C49D RID: 50333 RVA: 0x00480097 File Offset: 0x0047E297
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(base.InitialSize.x, (float)(UI.screenHeight - 35));
			}
		}

		// Token: 0x0600C49E RID: 50334 RVA: 0x004800B2 File Offset: 0x0047E2B2
		public override void PreOpen()
		{
			base.PreOpen();
			this.scrollPosition_ideoDetails = Vector2.zero;
		}

		// Token: 0x0600C49F RID: 50335 RVA: 0x004800C5 File Offset: 0x0047E2C5
		public override void PostClose()
		{
			base.PostClose();
			IdeoUIUtility.UnselectCurrent();
		}

		// Token: 0x0600C4A0 RID: 50336 RVA: 0x004800D4 File Offset: 0x0047E2D4
		public override void DoWindowContents(Rect rect)
		{
			IdeoUIUtility.DoIdeoListAndDetails(rect, ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, false, false, null, null, null, false, null, null, false, false, null);
		}

		// Token: 0x0400705A RID: 28762
		private Vector2 scrollPosition_ideoList;

		// Token: 0x0400705B RID: 28763
		private float scrollViewHeight_ideoList;

		// Token: 0x0400705C RID: 28764
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x0400705D RID: 28765
		private float scrollViewHeight_ideoDetails;
	}
}
