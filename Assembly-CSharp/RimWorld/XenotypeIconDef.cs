using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001019 RID: 4121
	public class XenotypeIconDef : Def
	{
		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x060067D1 RID: 26577 RVA: 0x0024AB44 File Offset: 0x00248D44
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.texPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x060067D2 RID: 26578 RVA: 0x0024AB6C File Offset: 0x00248D6C
		public override IEnumerable<string> ConfigErrors()
		{
			XenotypeIconDef.<ConfigErrors>d__4 <ConfigErrors>d__ = new XenotypeIconDef.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004238 RID: 16952
		[NoTranslate]
		public string texPath;

		// Token: 0x04004239 RID: 16953
		[Unsaved(false)]
		private Texture2D cachedIcon;
	}
}
