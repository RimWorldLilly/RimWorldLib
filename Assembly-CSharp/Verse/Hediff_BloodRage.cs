using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004A4 RID: 1188
	public class Hediff_BloodRage : Hediff
	{
		// Token: 0x060020DF RID: 8415 RVA: 0x000C45AC File Offset: 0x000C27AC
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Blood rage"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
			if (this.pawn.RaceProps.Animal)
			{
				this.adjustedSeverityRaisePerTick = 8.8E-05f * Hediff_BloodRage.SeverityRaiseRangeAnimal.RandomInRange;
			}
			else if (this.pawn.RaceProps.Humanlike)
			{
				this.adjustedSeverityRaisePerTick = 8.8E-05f * Hediff_BloodRage.SeverityRaiseRange.RandomInRange;
				if (this.pawn.story.traits.HasTrait(TraitDefOf.Wimp))
				{
					this.adjustedSeverityRaisePerTick *= 0.5f;
				}
				if (this.pawn.story.traits.HasTrait(TraitDefOf.Kind))
				{
					this.adjustedSeverityRaisePerTick *= 0.5f;
				}
			}
			this.adjustedSeverityRaisePerTick *= this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000C46B4 File Offset: 0x000C28B4
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.Severity >= 1f)
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					BloodRainUtility.TryTriggerBerserkShort(this.pawn);
				}
				if (this.pawn.RaceProps.Animal && this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterBloodRain, null, false, false, false, null, false, false, false) && MessagesRepeatAvoider.MessageShowAllowed("MessageManhunterBloodrain-" + this.pawn.LabelShort, 240f))
				{
					Messages.Message("Manhunter_BloodRain".Translate(this.pawn), this.pawn, MessageTypeDefOf.NeutralEvent, false);
				}
			}
			if (BloodRainUtility.ExposedToBloodRain(this.pawn))
			{
				this.Severity += this.adjustedSeverityRaisePerTick * (float)delta;
				return;
			}
			this.Severity -= 0.000132f * (float)delta;
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000C47B6 File Offset: 0x000C29B6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.adjustedSeverityRaisePerTick, "adjustedSeverityRaisePerTick", 0f, false);
		}

		// Token: 0x04001935 RID: 6453
		private float adjustedSeverityRaisePerTick;

		// Token: 0x04001936 RID: 6454
		private const float SeverityRaisePerTick = 8.8E-05f;

		// Token: 0x04001937 RID: 6455
		private const float SeverityRaisePerTickAnimals = 8.8E-05f;

		// Token: 0x04001938 RID: 6456
		private const float SeverityFallPerTick = 0.000132f;

		// Token: 0x04001939 RID: 6457
		private static readonly FloatRange SeverityRaiseRange = new FloatRange(0.75f, 1.25f);

		// Token: 0x0400193A RID: 6458
		private static readonly FloatRange SeverityRaiseRangeAnimal = new FloatRange(0.1f, 0.8f);
	}
}
