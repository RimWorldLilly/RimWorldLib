using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF4 RID: 4084
	public abstract class StyleItemDef : Def
	{
		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x06006724 RID: 26404 RVA: 0x00248B43 File Offset: 0x00246D43
		public virtual Texture2D Icon
		{
			get
			{
				if (this.noGraphic)
				{
					return BaseContent.ClearTex;
				}
				return ContentFinder<Texture2D>.Get(this.iconPath ?? (this.texPath + "_south"), true);
			}
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x06006725 RID: 26405 RVA: 0x00248B73 File Offset: 0x00246D73
		public StyleItemCategoryDef StyleItemCategory
		{
			get
			{
				if (this.category == null)
				{
					return StyleItemCategoryDefOf.Misc;
				}
				return this.category;
			}
		}

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x06006726 RID: 26406 RVA: 0x00248B89 File Offset: 0x00246D89
		public static IEnumerable<StyleItemDef> AllStyleItemDefs
		{
			get
			{
				return new StyleItemDef.<get_AllStyleItemDefs>d__14(-2);
			}
		}

		// Token: 0x06006727 RID: 26407
		public abstract Graphic GraphicFor(Pawn pawn, Color color);

		// Token: 0x06006728 RID: 26408 RVA: 0x00248B92 File Offset: 0x00246D92
		public override IEnumerable<string> ConfigErrors()
		{
			StyleItemDef.<ConfigErrors>d__16 <ConfigErrors>d__ = new StyleItemDef.<ConfigErrors>d__16(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040040D9 RID: 16601
		[LoadAlias("hairTags")]
		[NoTranslate]
		public List<string> styleTags = new List<string>();

		// Token: 0x040040DA RID: 16602
		[LoadAlias("hairGender")]
		public StyleGender styleGender = StyleGender.Any;

		// Token: 0x040040DB RID: 16603
		private StyleItemCategoryDef category;

		// Token: 0x040040DC RID: 16604
		public bool noGraphic;

		// Token: 0x040040DD RID: 16605
		public ShaderTypeDef overrideShaderTypeDef;

		// Token: 0x040040DE RID: 16606
		public GeneDef requiredGene;

		// Token: 0x040040DF RID: 16607
		public MutantDef requiredMutant;

		// Token: 0x040040E0 RID: 16608
		[NoTranslate]
		public string texPath;

		// Token: 0x040040E1 RID: 16609
		[NoTranslate]
		public string iconPath;
	}
}
