using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002023 RID: 8227
	public class WorkGiverDef : Def
	{
		// Token: 0x17002568 RID: 9576
		// (get) Token: 0x0600D019 RID: 53273 RVA: 0x004C7CEA File Offset: 0x004C5EEA
		public WorkGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WorkGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0600D01A RID: 53274 RVA: 0x004C7D1C File Offset: 0x004C5F1C
		public override IEnumerable<string> ConfigErrors()
		{
			WorkGiverDef.<ConfigErrors>d__34 <ConfigErrors>d__ = new WorkGiverDef.<ConfigErrors>d__34(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400844F RID: 33871
		public Type giverClass;

		// Token: 0x04008450 RID: 33872
		public WorkTypeDef workType;

		// Token: 0x04008451 RID: 33873
		public WorkTags workTags;

		// Token: 0x04008452 RID: 33874
		public int priorityInType;

		// Token: 0x04008453 RID: 33875
		[MustTranslate]
		public string verb;

		// Token: 0x04008454 RID: 33876
		[MustTranslate]
		public string gerund;

		// Token: 0x04008455 RID: 33877
		public bool scanThings = true;

		// Token: 0x04008456 RID: 33878
		public bool scanCells;

		// Token: 0x04008457 RID: 33879
		public bool emergency;

		// Token: 0x04008458 RID: 33880
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04008459 RID: 33881
		public bool directOrderable = true;

		// Token: 0x0400845A RID: 33882
		public bool prioritizeSustains;

		// Token: 0x0400845B RID: 33883
		public bool nonColonistsCanDo;

		// Token: 0x0400845C RID: 33884
		public JobTag tagToGive = JobTag.MiscWork;

		// Token: 0x0400845D RID: 33885
		public WorkGiverEquivalenceGroupDef equivalenceGroup;

		// Token: 0x0400845E RID: 33886
		public bool canBeDoneWhileDrafted;

		// Token: 0x0400845F RID: 33887
		public int autoTakeablePriorityDrafted = -1;

		// Token: 0x04008460 RID: 33888
		public ThingDef forceMote;

		// Token: 0x04008461 RID: 33889
		public FleckDef forceFleck;

		// Token: 0x04008462 RID: 33890
		public bool canBeDoneByMechs = true;

		// Token: 0x04008463 RID: 33891
		public List<ThingDef> fixedBillGiverDefs;

		// Token: 0x04008464 RID: 33892
		public bool billGiversAllHumanlikes;

		// Token: 0x04008465 RID: 33893
		public bool billGiversAllHumanlikesCorpses;

		// Token: 0x04008466 RID: 33894
		public bool billGiversAllMechanoids;

		// Token: 0x04008467 RID: 33895
		public bool billGiversAllMechanoidsCorpses;

		// Token: 0x04008468 RID: 33896
		public bool billGiversAllAnimals;

		// Token: 0x04008469 RID: 33897
		public bool billGiversAllAnimalsCorpses;

		// Token: 0x0400846A RID: 33898
		public bool feedHumanlikesOnly;

		// Token: 0x0400846B RID: 33899
		public bool feedAnimalsOnly;

		// Token: 0x0400846C RID: 33900
		public bool doesSmoothing;

		// Token: 0x0400846D RID: 33901
		public ThingDef scannerDef;

		// Token: 0x0400846E RID: 33902
		[Unsaved(false)]
		private WorkGiver workerInt;
	}
}
