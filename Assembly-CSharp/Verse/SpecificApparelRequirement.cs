using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001A4 RID: 420
	public class SpecificApparelRequirement
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0003E339 File Offset: 0x0003C539
		public BodyPartGroupDef BodyPartGroup
		{
			get
			{
				return this.bodyPartGroup;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0003E341 File Offset: 0x0003C541
		public ApparelLayerDef ApparelLayer
		{
			get
			{
				return this.apparelLayer;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x0003E349 File Offset: 0x0003C549
		public ThingDef ApparelDef
		{
			get
			{
				return this.apparelDef;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0003E351 File Offset: 0x0003C551
		public string RequiredTag
		{
			get
			{
				return this.requiredTag;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x0003E359 File Offset: 0x0003C559
		public List<SpecificApparelRequirement.TagChance> AlternateTagChoices
		{
			get
			{
				return this.alternateTagChoices;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0003E361 File Offset: 0x0003C561
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x0003E369 File Offset: 0x0003C569
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0003E371 File Offset: 0x0003C571
		public ColorGenerator ColorGenerator
		{
			get
			{
				return this.colorGenerator;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000B9B RID: 2971 RVA: 0x0003E379 File Offset: 0x0003C579
		public bool Locked
		{
			get
			{
				return this.locked;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x0003E381 File Offset: 0x0003C581
		public bool Biocode
		{
			get
			{
				return this.biocode;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x0003E389 File Offset: 0x0003C589
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x0003E391 File Offset: 0x0003C591
		public QualityCategory? Quality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x0003E399 File Offset: 0x0003C599
		public bool UseRandomStyleDef
		{
			get
			{
				return this.useRandomStyleDef;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x0003E3A1 File Offset: 0x0003C5A1
		public bool IgnoreNaked
		{
			get
			{
				return this.ignoreNaked;
			}
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0003E3AC File Offset: 0x0003C5AC
		public Color GetColor()
		{
			if (this.color != default(Color))
			{
				return this.color;
			}
			if (this.colorGenerator != null)
			{
				return this.colorGenerator.NewRandomizedColor();
			}
			return default(Color);
		}

		// Token: 0x04000AC5 RID: 2757
		private BodyPartGroupDef bodyPartGroup;

		// Token: 0x04000AC6 RID: 2758
		private ApparelLayerDef apparelLayer;

		// Token: 0x04000AC7 RID: 2759
		private ThingDef apparelDef;

		// Token: 0x04000AC8 RID: 2760
		private string requiredTag;

		// Token: 0x04000AC9 RID: 2761
		private List<SpecificApparelRequirement.TagChance> alternateTagChoices;

		// Token: 0x04000ACA RID: 2762
		private ThingDef stuff;

		// Token: 0x04000ACB RID: 2763
		private ThingStyleDef styleDef;

		// Token: 0x04000ACC RID: 2764
		private Color color;

		// Token: 0x04000ACD RID: 2765
		private ColorGenerator colorGenerator;

		// Token: 0x04000ACE RID: 2766
		private bool locked;

		// Token: 0x04000ACF RID: 2767
		private bool biocode;

		// Token: 0x04000AD0 RID: 2768
		private QualityCategory? quality;

		// Token: 0x04000AD1 RID: 2769
		private bool useRandomStyleDef;

		// Token: 0x04000AD2 RID: 2770
		private bool ignoreNaked;

		// Token: 0x0200254F RID: 9551
		public struct TagChance
		{
			// Token: 0x040098D0 RID: 39120
			public string tag;

			// Token: 0x040098D1 RID: 39121
			public float chance;
		}
	}
}
