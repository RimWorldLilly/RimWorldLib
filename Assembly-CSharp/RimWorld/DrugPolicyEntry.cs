using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015B3 RID: 5555
	public class DrugPolicyEntry : IExposable
	{
		// Token: 0x060089A3 RID: 35235 RVA: 0x0031D8AC File Offset: 0x0031BAAC
		public void CopyFrom(DrugPolicyEntry other)
		{
			this.drug = other.drug;
			this.allowedForAddiction = other.allowedForAddiction;
			this.allowedForJoy = other.allowedForJoy;
			this.allowScheduled = other.allowScheduled;
			this.daysFrequency = other.daysFrequency;
			this.onlyIfMoodBelow = other.onlyIfMoodBelow;
			this.onlyIfJoyBelow = other.onlyIfJoyBelow;
			this.takeToInventory = other.takeToInventory;
		}

		// Token: 0x060089A4 RID: 35236 RVA: 0x0031D91C File Offset: 0x0031BB1C
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<bool>(ref this.allowedForAddiction, "allowedForAddiction", false, false);
			Scribe_Values.Look<bool>(ref this.allowedForJoy, "allowedForJoy", false, false);
			Scribe_Values.Look<bool>(ref this.allowScheduled, "allowScheduled", false, false);
			Scribe_Values.Look<float>(ref this.daysFrequency, "daysFrequency", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfMoodBelow, "onlyIfMoodBelow", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfJoyBelow, "onlyIfJoyBelow", 1f, false);
			Scribe_Values.Look<int>(ref this.takeToInventory, "takeToInventory", 0, false);
		}

		// Token: 0x040054C6 RID: 21702
		public ThingDef drug;

		// Token: 0x040054C7 RID: 21703
		public bool allowedForAddiction;

		// Token: 0x040054C8 RID: 21704
		public bool allowedForJoy;

		// Token: 0x040054C9 RID: 21705
		public bool allowScheduled;

		// Token: 0x040054CA RID: 21706
		public float daysFrequency = 1f;

		// Token: 0x040054CB RID: 21707
		public float onlyIfMoodBelow = 1f;

		// Token: 0x040054CC RID: 21708
		public float onlyIfJoyBelow = 1f;

		// Token: 0x040054CD RID: 21709
		public int takeToInventory;

		// Token: 0x040054CE RID: 21710
		public string takeToInventoryTempBuffer;
	}
}
