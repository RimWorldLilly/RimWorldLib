using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200199B RID: 6555
	public class CompProperties_Studiable : CompProperties
	{
		// Token: 0x17001C96 RID: 7318
		// (get) Token: 0x0600A48E RID: 42126 RVA: 0x003ACF49 File Offset: 0x003AB149
		public bool Completable
		{
			get
			{
				return this.studyAmountToComplete > 0f;
			}
		}

		// Token: 0x0600A48F RID: 42127 RVA: 0x003ACF58 File Offset: 0x003AB158
		public CompProperties_Studiable()
		{
			this.compClass = typeof(CompStudiable);
		}

		// Token: 0x0600A490 RID: 42128 RVA: 0x003ACF94 File Offset: 0x003AB194
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Studiable.<ConfigErrors>d__18 <ConfigErrors>d__ = new CompProperties_Studiable.<ConfigErrors>d__18(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04006149 RID: 24905
		public int frequencyTicks = -1;

		// Token: 0x0400614A RID: 24906
		public float studyAmountToComplete = -1f;

		// Token: 0x0400614B RID: 24907
		public bool showToggleGizmo;

		// Token: 0x0400614C RID: 24908
		public bool studyEnabledByDefault = true;

		// Token: 0x0400614D RID: 24909
		public bool canBeActivityDeactivated;

		// Token: 0x0400614E RID: 24910
		internal float? anomalyKnowledge;

		// Token: 0x0400614F RID: 24911
		internal KnowledgeCategoryDef knowledgeCategory;

		// Token: 0x04006150 RID: 24912
		public float knowledgeFactorOutdoors = 1f;

		// Token: 0x04006151 RID: 24913
		public int minMonolithLevelForStudy;

		// Token: 0x04006152 RID: 24914
		internal bool requiresHoldingPlatform;

		// Token: 0x04006153 RID: 24915
		internal bool requiresImprisonment;

		// Token: 0x04006154 RID: 24916
		[MustTranslate]
		public string completedLetterTitle;

		// Token: 0x04006155 RID: 24917
		[MustTranslate]
		public string completedLetterText;

		// Token: 0x04006156 RID: 24918
		[MustTranslate]
		public string completedMessage;

		// Token: 0x04006157 RID: 24919
		public LetterDef completedLetterDef;
	}
}
