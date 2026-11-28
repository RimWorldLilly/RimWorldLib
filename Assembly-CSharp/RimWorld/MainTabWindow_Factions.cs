using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001D98 RID: 7576
	public class MainTabWindow_Factions : MainTabWindow
	{
		// Token: 0x0600C48F RID: 50319 RVA: 0x0047EE8E File Offset: 0x0047D08E
		public override void PreOpen()
		{
			base.PreOpen();
			this.scrollToFaction = null;
		}

		// Token: 0x0600C490 RID: 50320 RVA: 0x0047EE9D File Offset: 0x0047D09D
		public void ScrollToFaction(Faction faction)
		{
			this.scrollToFaction = faction;
		}

		// Token: 0x0600C491 RID: 50321 RVA: 0x0047EEA6 File Offset: 0x0047D0A6
		public override void DoWindowContents(Rect fillRect)
		{
			FactionUIUtility.DoWindowContents(fillRect, ref this.scrollPosition, ref this.scrollViewHeight, this.scrollToFaction);
			if (this.scrollToFaction != null)
			{
				this.scrollToFaction = null;
			}
		}

		// Token: 0x04007041 RID: 28737
		private Vector2 scrollPosition;

		// Token: 0x04007042 RID: 28738
		private float scrollViewHeight;

		// Token: 0x04007043 RID: 28739
		private Faction scrollToFaction;
	}
}
