using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A38 RID: 6712
	public class CompUseEffect_FixWorstHealthCondition : CompUseEffect
	{
		// Token: 0x0600A865 RID: 43109 RVA: 0x003BD4C8 File Offset: 0x003BB6C8
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			Hediff hediff;
			BodyPartRecord bodyPartRecord;
			if (!HealthUtility.TryGetWorstHealthCondition(p, out hediff, out bodyPartRecord, Array.Empty<HediffDef>()))
			{
				return "AbilityCannotCastNoHealableInjury".Translate(p.Named("PAWN")).Resolve().StripTags() ?? "";
			}
			return true;
		}

		// Token: 0x0600A866 RID: 43110 RVA: 0x003BD520 File Offset: 0x003BB720
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			TaggedString taggedString = HealthUtility.FixWorstHealthCondition(usedBy, Array.Empty<HediffDef>());
			if (PawnUtility.ShouldSendNotificationAbout(usedBy))
			{
				Messages.Message(taggedString, usedBy, MessageTypeDefOf.PositiveEvent, true);
			}
		}
	}
}
