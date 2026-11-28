using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5A RID: 3930
	public class FeatureDef : Def
	{
		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x0600651E RID: 25886 RVA: 0x00240ED0 File Offset: 0x0023F0D0
		public FeatureWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (FeatureWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04003B6B RID: 15211
		public Type workerClass = typeof(FeatureWorker);

		// Token: 0x04003B6C RID: 15212
		public float order;

		// Token: 0x04003B6D RID: 15213
		public int minSize = 50;

		// Token: 0x04003B6E RID: 15214
		public int maxSize = int.MaxValue;

		// Token: 0x04003B6F RID: 15215
		public bool canTouchWorldEdge = true;

		// Token: 0x04003B70 RID: 15216
		public RulePackDef nameMaker;

		// Token: 0x04003B71 RID: 15217
		public int maxPossiblyAllowedSizeToTake = 30;

		// Token: 0x04003B72 RID: 15218
		public float maxPossiblyAllowedSizePctOfMeToTake = 0.5f;

		// Token: 0x04003B73 RID: 15219
		public List<BiomeDef> rootBiomes = new List<BiomeDef>();

		// Token: 0x04003B74 RID: 15220
		public List<BiomeDef> acceptableBiomes = new List<BiomeDef>();

		// Token: 0x04003B75 RID: 15221
		public int maxSpaceBetweenRootGroups = 5;

		// Token: 0x04003B76 RID: 15222
		public int minRootGroupsInCluster = 3;

		// Token: 0x04003B77 RID: 15223
		public int minRootGroupSize = 10;

		// Token: 0x04003B78 RID: 15224
		public int maxRootGroupSize = int.MaxValue;

		// Token: 0x04003B79 RID: 15225
		public int maxPassageWidth = 3;

		// Token: 0x04003B7A RID: 15226
		public float maxPctOfWholeArea = 0.1f;

		// Token: 0x04003B7B RID: 15227
		[Unsaved(false)]
		private FeatureWorker workerInt;
	}
}
