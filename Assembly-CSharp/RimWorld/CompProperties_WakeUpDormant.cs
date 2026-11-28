using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019D8 RID: 6616
	public class CompProperties_WakeUpDormant : CompProperties
	{
		// Token: 0x0600A696 RID: 42646 RVA: 0x003B60FC File Offset: 0x003B42FC
		public CompProperties_WakeUpDormant()
		{
			this.compClass = typeof(CompWakeUpDormant);
		}

		// Token: 0x04006227 RID: 25127
		[NoTranslate]
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x04006228 RID: 25128
		public float wakeUpCheckRadius = -1f;

		// Token: 0x04006229 RID: 25129
		public float wakeUpOnThingConstructedRadius = 3f;

		// Token: 0x0400622A RID: 25130
		public bool wakeUpOnDamage = true;

		// Token: 0x0400622B RID: 25131
		public bool onlyWakeUpSameFaction = true;

		// Token: 0x0400622C RID: 25132
		public bool onlySendSignalOnce = true;

		// Token: 0x0400622D RID: 25133
		public SoundDef wakeUpSound;

		// Token: 0x0400622E RID: 25134
		public bool wakeUpIfAnyTargetClose;

		// Token: 0x0400622F RID: 25135
		public TargetingParameters wakeUpTargetingParams = TargetingParameters.ForColonist();

		// Token: 0x04006230 RID: 25136
		public bool wakeUpWithDelay;

		// Token: 0x04006231 RID: 25137
		[NoTranslate]
		public string radiusCheckInspectPaneKey;

		// Token: 0x04006232 RID: 25138
		[NoTranslate]
		public string activateMessageKey;

		// Token: 0x04006233 RID: 25139
		[NoTranslate]
		public string activatePluralMessageKey;

		// Token: 0x04006234 RID: 25140
		public MessageTypeDef activateMessageType;
	}
}
