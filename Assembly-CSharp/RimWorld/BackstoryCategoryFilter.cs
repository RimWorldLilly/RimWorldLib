using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F32 RID: 3890
	public class BackstoryCategoryFilter
	{
		// Token: 0x06006479 RID: 25721 RVA: 0x0023E344 File Offset: 0x0023C544
		public bool Matches(PawnBio bio)
		{
			if (this.exclude != null && this.exclude.Any((string e) => bio.adulthood.spawnCategories.Contains(e) || bio.childhood.spawnCategories.Contains(e)))
			{
				return false;
			}
			if (this.excludeChildhood != null && bio.childhood.spawnCategories.Any((string e) => this.excludeChildhood.Contains(e)))
			{
				return false;
			}
			if (this.excludeAdulthood != null && bio.adulthood.spawnCategories.Any((string e) => this.excludeAdulthood.Contains(e)))
			{
				return false;
			}
			bool flag = true;
			if (this.categoriesChildhood != null)
			{
				flag &= bio.childhood.spawnCategories.Any((string e) => this.categoriesChildhood.Contains(e));
			}
			if (this.categoriesAdulthood != null)
			{
				flag &= bio.adulthood.spawnCategories.Any((string e) => this.categoriesAdulthood.Contains(e));
			}
			if (this.categoriesChildhood != null || this.categoriesAdulthood != null)
			{
				return flag;
			}
			return this.categories == null || this.categories.Any((string c) => bio.adulthood.spawnCategories.Contains(c) || bio.childhood.spawnCategories.Contains(c));
		}

		// Token: 0x0600647A RID: 25722 RVA: 0x0023E470 File Offset: 0x0023C670
		public bool Matches(BackstoryDef backstory)
		{
			if (this.exclude != null && backstory.spawnCategories.Any((string e) => this.exclude.Contains(e)))
			{
				return false;
			}
			if (!this.excludeChildhood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Childhood && backstory.spawnCategories.Any((string e) => this.excludeChildhood.Contains(e)))
			{
				return false;
			}
			if (!this.excludeAdulthood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Adulthood && backstory.spawnCategories.Any((string e) => this.excludeAdulthood.Contains(e)))
			{
				return false;
			}
			if (!this.categoriesChildhood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Childhood && !backstory.spawnCategories.Any((string c) => this.categoriesChildhood.Contains(c)))
			{
				return false;
			}
			if (!this.categoriesAdulthood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Adulthood && !backstory.spawnCategories.Any((string c) => this.categoriesAdulthood.Contains(c)))
			{
				return false;
			}
			if (backstory.requiresSpawnCategory)
			{
				if (this.categories.NullOrEmpty<string>() && this.categoriesChildhood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Childhood)
				{
					return false;
				}
				if (this.categories.NullOrEmpty<string>() && this.categoriesAdulthood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Adulthood)
				{
					return false;
				}
			}
			return this.categories == null || backstory.spawnCategories.Any((string c) => this.categories.Contains(c));
		}

		// Token: 0x040039B4 RID: 14772
		public List<string> categories;

		// Token: 0x040039B5 RID: 14773
		public List<string> exclude;

		// Token: 0x040039B6 RID: 14774
		public List<string> categoriesChildhood;

		// Token: 0x040039B7 RID: 14775
		public List<string> excludeChildhood;

		// Token: 0x040039B8 RID: 14776
		public List<string> categoriesAdulthood;

		// Token: 0x040039B9 RID: 14777
		public List<string> excludeAdulthood;

		// Token: 0x040039BA RID: 14778
		public float commonality = 1f;
	}
}
