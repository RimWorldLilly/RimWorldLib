using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019CF RID: 6607
	public class CompProperties_UniqueWeapon : CompProperties
	{
		// Token: 0x0600A635 RID: 42549 RVA: 0x003B45A5 File Offset: 0x003B27A5
		public CompProperties_UniqueWeapon()
		{
			this.compClass = typeof(CompUniqueWeapon);
		}

		// Token: 0x04006211 RID: 25105
		public List<WeaponCategoryDef> weaponCategories = new List<WeaponCategoryDef>();

		// Token: 0x04006212 RID: 25106
		[MustTranslate]
		public List<string> namerLabels = new List<string>();
	}
}
