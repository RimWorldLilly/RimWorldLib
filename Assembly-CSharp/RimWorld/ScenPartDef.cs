using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE1 RID: 4065
	public class ScenPartDef : Def
	{
		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x060066D4 RID: 26324 RVA: 0x00247949 File Offset: 0x00245B49
		public bool PlayerAddRemovable
		{
			get
			{
				return this.category != ScenPartCategory.Fixed && this.canBePlayerAddedRemoved;
			}
		}

		// Token: 0x060066D5 RID: 26325 RVA: 0x0024795C File Offset: 0x00245B5C
		public override IEnumerable<string> ConfigErrors()
		{
			ScenPartDef.<ConfigErrors>d__16 <ConfigErrors>d__ = new ScenPartDef.<ConfigErrors>d__16(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004016 RID: 16406
		public ScenPartCategory category;

		// Token: 0x04004017 RID: 16407
		public Type scenPartClass;

		// Token: 0x04004018 RID: 16408
		public float summaryPriority = -1f;

		// Token: 0x04004019 RID: 16409
		public float selectionWeight = 1f;

		// Token: 0x0400401A RID: 16410
		public int maxUses = 999999;

		// Token: 0x0400401B RID: 16411
		public bool canBeRandomlyAdded = true;

		// Token: 0x0400401C RID: 16412
		public bool canBePlayerAddedRemoved = true;

		// Token: 0x0400401D RID: 16413
		public Type pageClass;

		// Token: 0x0400401E RID: 16414
		public GameConditionDef gameCondition;

		// Token: 0x0400401F RID: 16415
		public bool gameConditionTargetsWorld;

		// Token: 0x04004020 RID: 16416
		public FloatRange durationRandomRange = new FloatRange(30f, 100f);

		// Token: 0x04004021 RID: 16417
		public Type designatorType;

		// Token: 0x04004022 RID: 16418
		public GenStepDef genStep;

		// Token: 0x04004023 RID: 16419
		public FactionDef preventRemovalOfFaction;
	}
}
