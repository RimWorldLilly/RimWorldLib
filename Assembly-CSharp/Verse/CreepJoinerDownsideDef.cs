using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000129 RID: 297
	public class CreepJoinerDownsideDef : CreepJoinerBaseDef
	{
		// Token: 0x060009D0 RID: 2512 RVA: 0x00036A2E File Offset: 0x00034C2E
		public override IEnumerable<string> ConfigErrors()
		{
			CreepJoinerDownsideDef.<ConfigErrors>d__15 <ConfigErrors>d__ = new CreepJoinerDownsideDef.<ConfigErrors>d__15(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400051E RID: 1310
		public bool repeats;

		// Token: 0x0400051F RID: 1311
		public bool hasLetter;

		// Token: 0x04000520 RID: 1312
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04000521 RID: 1313
		[MustTranslate]
		public string letterDesc;

		// Token: 0x04000522 RID: 1314
		public LetterDef letterDef;

		// Token: 0x04000523 RID: 1315
		public List<BackstoryTrait> traits = new List<BackstoryTrait>();

		// Token: 0x04000524 RID: 1316
		public List<HediffDef> hediffs = new List<HediffDef>();

		// Token: 0x04000525 RID: 1317
		public FloatRange triggersAfterDays = FloatRange.Zero;

		// Token: 0x04000526 RID: 1318
		public List<AbilityDef> abilities = new List<AbilityDef>();

		// Token: 0x04000527 RID: 1319
		public float triggerMtbDays;

		// Token: 0x04000528 RID: 1320
		public FloatRange triggerMinDays = FloatRange.Zero;

		// Token: 0x04000529 RID: 1321
		public bool canOccurWhenImprisoned;

		// Token: 0x0400052A RID: 1322
		public bool canOccurWhileDowned = true;

		// Token: 0x0400052B RID: 1323
		public bool mustBeConscious;

		// Token: 0x0400052C RID: 1324
		public Type workerType;
	}
}
