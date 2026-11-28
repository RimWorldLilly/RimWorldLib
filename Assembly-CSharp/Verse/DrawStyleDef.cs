using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000166 RID: 358
	public class DrawStyleDef : Def
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0003B8A8 File Offset: 0x00039AA8
		public DrawStyle DrawStyleWorker
		{
			get
			{
				return GenWorker<DrawStyle>.Get(this.drawStyleType);
			}
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0003B8B5 File Offset: 0x00039AB5
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
			});
		}

		// Token: 0x040007FA RID: 2042
		[NoTranslate]
		private string uiIconPath;

		// Token: 0x040007FB RID: 2043
		public bool drawOutline;

		// Token: 0x040007FC RID: 2044
		public bool drawArea = true;

		// Token: 0x040007FD RID: 2045
		public bool drawShortSideMeasurement = true;

		// Token: 0x040007FE RID: 2046
		public bool canSnap = true;

		// Token: 0x040007FF RID: 2047
		public Type drawStyleType;

		// Token: 0x04000800 RID: 2048
		public Texture2D uiIcon = BaseContent.BadTex;
	}
}
