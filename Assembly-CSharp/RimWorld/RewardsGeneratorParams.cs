using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F34 RID: 7988
	public struct RewardsGeneratorParams
	{
		// Token: 0x0600CBBD RID: 52157 RVA: 0x004B2311 File Offset: 0x004B0511
		public string ConfigError()
		{
			if (this.rewardValue <= 0f)
			{
				return "rewardValue is " + this.rewardValue.ToString();
			}
			if (this.thingRewardDisallowed && this.thingRewardRequired)
			{
				return "thing reward is both disallowed and required";
			}
			return null;
		}

		// Token: 0x0600CBBE RID: 52158 RVA: 0x004B234D File Offset: 0x004B054D
		public override string ToString()
		{
			return GenText.FieldsToString<RewardsGeneratorParams>(this);
		}

		// Token: 0x040081E3 RID: 33251
		public float rewardValue;

		// Token: 0x040081E4 RID: 33252
		public Faction giverFaction;

		// Token: 0x040081E5 RID: 33253
		public string chosenPawnSignal;

		// Token: 0x040081E6 RID: 33254
		public bool giveToCaravan;

		// Token: 0x040081E7 RID: 33255
		public float minGeneratedRewardValue;

		// Token: 0x040081E8 RID: 33256
		public bool thingRewardDisallowed;

		// Token: 0x040081E9 RID: 33257
		public bool thingRewardRequired;

		// Token: 0x040081EA RID: 33258
		public bool thingRewardItemsOnly;

		// Token: 0x040081EB RID: 33259
		public List<ThingDef> disallowedThingDefs;

		// Token: 0x040081EC RID: 33260
		public bool allowRoyalFavor;

		// Token: 0x040081ED RID: 33261
		public bool allowGoodwill;

		// Token: 0x040081EE RID: 33262
		public bool allowDevelopmentPoints;

		// Token: 0x040081EF RID: 33263
		public bool allowXenogermReimplantation;

		// Token: 0x040081F0 RID: 33264
		public float populationIntent;
	}
}
