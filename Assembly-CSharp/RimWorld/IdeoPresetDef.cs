using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F70 RID: 3952
	public class IdeoPresetDef : Def
	{
		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x06006560 RID: 25952 RVA: 0x00241BBE File Offset: 0x0023FDBE
		public Texture2D Icon
		{
			get
			{
				if (this.iconPath.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x04003BBC RID: 15292
		public IdeoPresetCategoryDef categoryDef;

		// Token: 0x04003BBD RID: 15293
		public List<MemeDef> memes = new List<MemeDef>();

		// Token: 0x04003BBE RID: 15294
		[NoTranslate]
		public string iconPath;

		// Token: 0x04003BBF RID: 15295
		public bool classicPlus;

		// Token: 0x04003BC0 RID: 15296
		private Texture2D cachedIcon;
	}
}
