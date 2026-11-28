using System;

namespace Verse
{
	// Token: 0x02000184 RID: 388
	public class ManeuverDef : Def
	{
		// Token: 0x0400096A RID: 2410
		public ToolCapacityDef requiredCapacity;

		// Token: 0x0400096B RID: 2411
		public VerbProperties verb;

		// Token: 0x0400096C RID: 2412
		public RulePackDef combatLogRulesHit;

		// Token: 0x0400096D RID: 2413
		public RulePackDef combatLogRulesDeflect;

		// Token: 0x0400096E RID: 2414
		public RulePackDef combatLogRulesMiss;

		// Token: 0x0400096F RID: 2415
		public RulePackDef combatLogRulesDodge;

		// Token: 0x04000970 RID: 2416
		public LogEntryDef logEntryDef;
	}
}
