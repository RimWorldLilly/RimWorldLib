using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F72 RID: 3954
	public class IdeoIconDef : IdeoSymbolPartDef
	{
		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x06006564 RID: 25956 RVA: 0x00241C86 File Offset: 0x0023FE86
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x04003BC3 RID: 15299
		[NoTranslate]
		public string iconPath;

		// Token: 0x04003BC4 RID: 15300
		[Unsaved(false)]
		private Texture2D cachedIcon;
	}
}
