using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000811 RID: 2065
	public class Verb_CastTargetEffectBiomutationLance : Verb_CastTargetEffect
	{
		// Token: 0x06004107 RID: 16647 RVA: 0x0019D1F8 File Offset: 0x0019B3F8
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!pawn.RaceProps.Humanlike && !pawn.IsAnimal)
				{
					if (showMessages)
					{
						Messages.Message("MessageBiomutationLanceInvalidTargetRace".Translate(pawn), this.caster, MessageTypeDefOf.RejectInput, null, false);
					}
					return false;
				}
				if (pawn.BodySize > 2.5f)
				{
					if (showMessages)
					{
						Messages.Message("MessageBiomutationLanceTargetTooBig".Translate(pawn), this.caster, MessageTypeDefOf.RejectInput, null, false);
					}
					return false;
				}
			}
			return base.ValidateTarget(target, showMessages);
		}
	}
}
