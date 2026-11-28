using System;

namespace Verse
{
	// Token: 0x02000121 RID: 289
	public class IconForStuffAppearance
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x00036281 File Offset: 0x00034481
		public StuffAppearanceDef Appearance
		{
			get
			{
				return this.appearance;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x00036289 File Offset: 0x00034489
		public string IconPath
		{
			get
			{
				return this.iconPath;
			}
		}

		// Token: 0x040004C4 RID: 1220
		private StuffAppearanceDef appearance;

		// Token: 0x040004C5 RID: 1221
		[NoTranslate]
		private string iconPath;
	}
}
