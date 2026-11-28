using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001003 RID: 4099
	public class TimeAssignmentDef : Def
	{
		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x06006764 RID: 26468 RVA: 0x0024957A File Offset: 0x0024777A
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x002495A1 File Offset: 0x002477A1
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightNotSelectedTag = "TimeAssignmentButton-" + this.defName + "-NotSelected";
		}

		// Token: 0x04004156 RID: 16726
		public Color color;

		// Token: 0x04004157 RID: 16727
		public bool allowRest = true;

		// Token: 0x04004158 RID: 16728
		public bool allowJoy = true;

		// Token: 0x04004159 RID: 16729
		[Unsaved(false)]
		public string cachedHighlightNotSelectedTag;

		// Token: 0x0400415A RID: 16730
		private Texture2D colorTextureInt;
	}
}
