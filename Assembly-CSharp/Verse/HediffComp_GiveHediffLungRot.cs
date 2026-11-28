using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200043A RID: 1082
	public class HediffComp_GiveHediffLungRot : HediffComp
	{
		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x000BE655 File Offset: 0x000BC855
		private HediffCompProperties_GiveHediffLungRot Props
		{
			get
			{
				return (HediffCompProperties_GiveHediffLungRot)this.props;
			}
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x000BE662 File Offset: 0x000BC862
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x000BE664 File Offset: 0x000BC864
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned && this.parent.Severity >= this.Props.minSeverity && pawn.Position.AnyGas(pawn.Map, GasType.RotStink) && !pawn.health.hediffSet.HasHediff(HediffDefOf.LungRot, false) && pawn.IsHashIntervalTick(this.Props.mtbCheckDuration, delta) && !pawn.health.immunity.AnyGeneMakesFullyImmuneTo(HediffDefOf.LungRot) && Rand.MTBEventOccurs(this.Props.mtbOverRotGasExposureCurve.Evaluate(this.parent.Severity), 2500f, (float)this.Props.mtbCheckDuration))
			{
				IEnumerable<BodyPartRecord> lungRotAffectedBodyParts = GasUtility.GetLungRotAffectedBodyParts(pawn);
				if (lungRotAffectedBodyParts.Any<BodyPartRecord>())
				{
					foreach (BodyPartRecord part in lungRotAffectedBodyParts)
					{
						pawn.health.AddHediff(HediffDefOf.LungRot, part, null, null);
					}
					if (PawnUtility.ShouldSendNotificationAbout(pawn))
					{
						TaggedString label = "LetterLabelNewDisease".Translate() + ": " + HediffDefOf.LungRot.LabelCap;
						TaggedString text = "LetterTextLungRot".Translate(pawn.Named("PAWN"), HediffDefOf.LungRot.label, pawn.LabelDefinite()).AdjustedFor(pawn, "PAWN", true).CapitalizeFirst();
						Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
					}
				}
			}
		}
	}
}
