using System;

namespace RimWorld
{
	// Token: 0x02001B32 RID: 6962
	public class MoteProgressBarAlwaysVisible : MoteProgressBar
	{
		// Token: 0x17002036 RID: 8246
		// (get) Token: 0x0600B29A RID: 45722 RVA: 0x003EE29D File Offset: 0x003EC49D
		protected override bool OnlyShowForClosestZoom
		{
			get
			{
				return false;
			}
		}
	}
}
