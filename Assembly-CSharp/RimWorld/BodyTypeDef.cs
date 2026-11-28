using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F41 RID: 3905
	public class BodyTypeDef : Def
	{
		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x060064BD RID: 25789 RVA: 0x0023F989 File Offset: 0x0023DB89
		public virtual Texture2D Icon
		{
			get
			{
				return ContentFinder<Texture2D>.Get(this.bodyNakedGraphicPath + "_south", true);
			}
		}

		// Token: 0x04003A43 RID: 14915
		[NoTranslate]
		public string bodyNakedGraphicPath;

		// Token: 0x04003A44 RID: 14916
		[NoTranslate]
		public string bodyDessicatedGraphicPath;

		// Token: 0x04003A45 RID: 14917
		public List<BodyTypeDef.WoundAnchor> woundAnchors;

		// Token: 0x04003A46 RID: 14918
		public float woundScale = 1f;

		// Token: 0x04003A47 RID: 14919
		public Vector2 headOffset;

		// Token: 0x04003A48 RID: 14920
		public Vector2 bodyGraphicScale = Vector2.one;

		// Token: 0x04003A49 RID: 14921
		public float bedOffset;

		// Token: 0x04003A4A RID: 14922
		public List<AttachPoint> attachPoints;

		// Token: 0x04003A4B RID: 14923
		public List<AttachPoint> attachPointsDessicated;

		// Token: 0x02002F51 RID: 12113
		public class WoundAnchor
		{
			// Token: 0x0400C03D RID: 49213
			[NoTranslate]
			public string tag;

			// Token: 0x0400C03E RID: 49214
			public BodyPartGroupDef group;

			// Token: 0x0400C03F RID: 49215
			public bool? narrowCrown;

			// Token: 0x0400C040 RID: 49216
			public Rot4? rotation;

			// Token: 0x0400C041 RID: 49217
			public bool canMirror = true;

			// Token: 0x0400C042 RID: 49218
			public Vector3 offset;

			// Token: 0x0400C043 RID: 49219
			public PawnOverlayDrawer.OverlayLayer layer;

			// Token: 0x0400C044 RID: 49220
			public Color debugColor;

			// Token: 0x0400C045 RID: 49221
			public float range;
		}
	}
}
