using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200151C RID: 5404
	public class InteractionWorker_InterrogateIdentity : InteractionWorker
	{
		// Token: 0x06008453 RID: 33875 RVA: 0x002F9BDC File Offset: 0x002F7DDC
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			if (Rand.Chance(InterrogateUtility.GetChance(initiator)))
			{
				TaggedString taggedString = "MetalhorrorReasonInterrogation".Translate(initiator.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString value = "MetalhorrorNoticedDetailsAppended".Translate(initiator.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString taggedString2 = "InteractionDetectedDesc".Translate(initiator.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString t = taggedString2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("\n\n");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(value);
				taggedString2 = t + defaultInterpolatedStringHandler.ToStringAndClear();
				MetalhorrorUtility.Detect(recipient, taggedString, taggedString2, 0.5f);
			}
		}

		// Token: 0x040051B9 RID: 20921
		private const float MetalhorrorDetectionNoticed = 0.5f;
	}
}
