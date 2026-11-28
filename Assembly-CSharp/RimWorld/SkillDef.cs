using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FE6 RID: 4070
	public class SkillDef : Def
	{
		// Token: 0x060066F2 RID: 26354 RVA: 0x00247D65 File Offset: 0x00245F65
		public override void PostLoad()
		{
			if (this.label == null)
			{
				this.label = this.skillLabel;
			}
		}

		// Token: 0x060066F3 RID: 26355 RVA: 0x00247D7B File Offset: 0x00245F7B
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.lessonInteraction == null)
			{
				this.lessonInteraction = InteractionDefOf.LessonGeneric;
			}
		}

		// Token: 0x060066F4 RID: 26356 RVA: 0x00247D98 File Offset: 0x00245F98
		public bool IsDisabled(WorkTags combinedDisabledWorkTags, IEnumerable<WorkTypeDef> disabledWorkTypes)
		{
			if ((combinedDisabledWorkTags & this.disablingWorkTags) != WorkTags.None)
			{
				return true;
			}
			if (this.neverDisabledBasedOnWorkTypes)
			{
				return false;
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			bool result = false;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				for (int j = 0; j < workTypeDef.relevantSkills.Count; j++)
				{
					if (workTypeDef.relevantSkills[j] == this)
					{
						if (!disabledWorkTypes.Contains(workTypeDef))
						{
							return false;
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0400404F RID: 16463
		[MustTranslate]
		public string skillLabel;

		// Token: 0x04004050 RID: 16464
		public RulePack generalRules;

		// Token: 0x04004051 RID: 16465
		public bool usuallyDefinedInBackstories = true;

		// Token: 0x04004052 RID: 16466
		public bool pawnCreatorSummaryVisible;

		// Token: 0x04004053 RID: 16467
		public WorkTags disablingWorkTags;

		// Token: 0x04004054 RID: 16468
		public float listOrder;

		// Token: 0x04004055 RID: 16469
		public bool neverDisabledBasedOnWorkTypes;

		// Token: 0x04004056 RID: 16470
		public InteractionDef lessonInteraction;
	}
}
