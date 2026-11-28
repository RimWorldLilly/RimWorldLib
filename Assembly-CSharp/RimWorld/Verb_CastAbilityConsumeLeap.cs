using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001478 RID: 5240
	public class Verb_CastAbilityConsumeLeap : Verb_CastAbilityJump
	{
		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x06008099 RID: 32921 RVA: 0x002DDE03 File Offset: 0x002DC003
		public override ThingDef JumpFlyerDef
		{
			get
			{
				return ThingDefOf.PawnFlyer_ConsumeLeap;
			}
		}
	}
}
