using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000127 RID: 295
	public class CreepJoinerFormKindDef : PawnKindDef, ICreepJoinerDef
	{
		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x000368CF File Offset: 0x00034ACF
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x000368D7 File Offset: 0x00034AD7
		public List<CreepJoinerBaseDef> Excludes
		{
			get
			{
				return this.excludes;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x000368DF File Offset: 0x00034ADF
		public List<CreepJoinerBaseDef> Requires
		{
			get
			{
				return this.requires;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x000368E7 File Offset: 0x00034AE7
		public float MinCombatPoints
		{
			get
			{
				return this.minCombatPoints;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x000368EF File Offset: 0x00034AEF
		public bool CanOccurRandomly
		{
			get
			{
				return this.canOccurRandomly;
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x000368F8 File Offset: 0x00034AF8
		public string GetBodyGraphicPath(Pawn pawn)
		{
			for (int i = 0; i < this.bodyTypeGraphicPaths.Count; i++)
			{
				if (this.bodyTypeGraphicPaths[i].bodyType == pawn.story.bodyType)
				{
					return this.bodyTypeGraphicPaths[i].texturePath;
				}
			}
			return null;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0003694C File Offset: 0x00034B4C
		public bool StyleItemAllowed(StyleItemDef styleItem)
		{
			if (!ModLister.AnomalyInstalled)
			{
				return true;
			}
			bool flag = styleItem is HairDef;
			bool flag2 = styleItem is BeardDef;
			if (!flag && !flag2)
			{
				return true;
			}
			if (flag)
			{
				if (this.hairTagFilter != null && !this.hairTagFilter.Allows(styleItem.styleTags))
				{
					return false;
				}
			}
			else if (flag2 && this.beardTagFilter != null && !this.beardTagFilter.Allows(styleItem.styleTags))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0400050D RID: 1293
		private float weight = 1f;

		// Token: 0x0400050E RID: 1294
		private float minCombatPoints;

		// Token: 0x0400050F RID: 1295
		private bool canOccurRandomly = true;

		// Token: 0x04000510 RID: 1296
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04000511 RID: 1297
		[MustTranslate]
		public string letterPrompt;

		// Token: 0x04000512 RID: 1298
		public List<BodyTypeGraphicData> bodyTypeGraphicPaths = new List<BodyTypeGraphicData>();

		// Token: 0x04000513 RID: 1299
		public List<HeadTypeDef> forcedHeadTypes;

		// Token: 0x04000514 RID: 1300
		public TagFilter hairTagFilter;

		// Token: 0x04000515 RID: 1301
		public TagFilter beardTagFilter;

		// Token: 0x04000516 RID: 1302
		public Color? hairColorOverride;

		// Token: 0x04000517 RID: 1303
		private List<CreepJoinerBaseDef> excludes = new List<CreepJoinerBaseDef>();

		// Token: 0x04000518 RID: 1304
		private List<CreepJoinerBaseDef> requires = new List<CreepJoinerBaseDef>();
	}
}
