using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001844 RID: 6212
	public class CompProperties_CanBeDormant : CompProperties
	{
		// Token: 0x06009B82 RID: 39810 RVA: 0x00380D18 File Offset: 0x0037EF18
		public CompProperties_CanBeDormant()
		{
			this.compClass = typeof(CompCanBeDormant);
		}

		// Token: 0x06009B83 RID: 39811 RVA: 0x00380D99 File Offset: 0x0037EF99
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_CanBeDormant.<ConfigErrors>d__16 <ConfigErrors>d__ = new CompProperties_CanBeDormant.<ConfigErrors>d__16(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005C6D RID: 23661
		public bool startsDormant;

		// Token: 0x04005C6E RID: 23662
		[NoTranslate]
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x04005C6F RID: 23663
		public float maxDistAwakenByOther = 40f;

		// Token: 0x04005C70 RID: 23664
		public bool canWakeUpFogged = true;

		// Token: 0x04005C71 RID: 23665
		public bool jobDormancy;

		// Token: 0x04005C72 RID: 23666
		public IntRange wakeUpDelayRange = new IntRange(60, 300);

		// Token: 0x04005C73 RID: 23667
		[NoTranslate]
		public string awakeStateLabelKey = "AwokeDaysAgo";

		// Token: 0x04005C74 RID: 23668
		[NoTranslate]
		public string dormantStateLabelKey = "DormantCompInactive";

		// Token: 0x04005C75 RID: 23669
		[NoTranslate]
		public string wakeUpDelayStateLabelKey;

		// Token: 0x04005C76 RID: 23670
		public EffecterDef wakeUpEffect;

		// Token: 0x04005C77 RID: 23671
		public IntRange? wakeUpRepeatSignalDelayRange;

		// Token: 0x04005C78 RID: 23672
		public bool showSleepingZs = true;

		// Token: 0x04005C79 RID: 23673
		public bool delayedWakeUpDoesZs = true;

		// Token: 0x04005C7A RID: 23674
		public bool dontShowDevGizmos;

		// Token: 0x04005C7B RID: 23675
		public List<NeedDef> freezeNeeds = new List<NeedDef>();
	}
}
