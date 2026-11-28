using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200090E RID: 2318
	public class DutyDef : Def
	{
		// Token: 0x04002DDF RID: 11743
		public ThinkNode thinkNode;

		// Token: 0x04002DE0 RID: 11744
		public ThinkNode constantThinkNode;

		// Token: 0x04002DE1 RID: 11745
		public bool alwaysShowWeapon;

		// Token: 0x04002DE2 RID: 11746
		public ThinkTreeDutyHook hook = ThinkTreeDutyHook.HighPriority;

		// Token: 0x04002DE3 RID: 11747
		public RandomSocialMode socialModeMax = RandomSocialMode.SuperActive;

		// Token: 0x04002DE4 RID: 11748
		public bool threatDisabled;

		// Token: 0x04002DE5 RID: 11749
		public bool ritualSpectateTarget;

		// Token: 0x04002DE6 RID: 11750
		public bool forceFaceUpPosture;

		// Token: 0x04002DE7 RID: 11751
		public bool? drawBodyOverride;
	}
}
