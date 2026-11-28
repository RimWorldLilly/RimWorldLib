using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4B RID: 3915
	public class CultureDef : Def
	{
		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x060064D8 RID: 25816 RVA: 0x0023FE9D File Offset: 0x0023E09D
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x060064D9 RID: 25817 RVA: 0x0023FEC5 File Offset: 0x0023E0C5
		public RulePackDef GetPawnNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.pawnNameMakerFemale != null)
			{
				return this.pawnNameMakerFemale;
			}
			return this.pawnNameMaker;
		}

		// Token: 0x060064DA RID: 25818 RVA: 0x0023FEE0 File Offset: 0x0023E0E0
		public override IEnumerable<string> ConfigErrors()
		{
			CultureDef.<ConfigErrors>d__17 <ConfigErrors>d__ = new CultureDef.<ConfigErrors>d__17(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003A77 RID: 14967
		public RulePackDef pawnNameMaker;

		// Token: 0x04003A78 RID: 14968
		public RulePackDef pawnNameMakerFemale;

		// Token: 0x04003A79 RID: 14969
		public RulePackDef ideoNameMaker;

		// Token: 0x04003A7A RID: 14970
		public RulePackDef deityNameMaker;

		// Token: 0x04003A7B RID: 14971
		public RulePackDef deityTypeMaker;

		// Token: 0x04003A7C RID: 14972
		public RulePackDef leaderTitleMaker;

		// Token: 0x04003A7D RID: 14973
		public RulePackDef festivalNameMaker;

		// Token: 0x04003A7E RID: 14974
		public List<ThingStyleCategoryWithPriority> thingStyleCategories;

		// Token: 0x04003A7F RID: 14975
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x04003A80 RID: 14976
		public IdeoWeaponClassPair preferredWeaponClasses;

		// Token: 0x04003A81 RID: 14977
		[NoTranslate]
		public List<string> allowedPlaceTags = new List<string>();

		// Token: 0x04003A82 RID: 14978
		public string iconPath;

		// Token: 0x04003A83 RID: 14979
		public Color iconColor = Color.white;

		// Token: 0x04003A84 RID: 14980
		private Texture2D icon;
	}
}
