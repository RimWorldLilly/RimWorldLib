using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200044A RID: 1098
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x000BF133 File Offset: 0x000BD333
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x000BF140 File Offset: 0x000BD340
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x000BF14E File Offset: 0x000BD34E
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x000BF165 File Offset: 0x000BD365
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			this.ticksToHeal -= delta;
			if (this.ticksToHeal <= 0)
			{
				HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(base.Pawn, this.parent.LabelCap);
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000BF19C File Offset: 0x000BD39C
		public static void TryHealRandomPermanentWound(Pawn pawn, string cause)
		{
			Hediff hediff;
			if (!(from hd in pawn.health.hediffSet.hediffs
			where hd.IsPermanent() || hd.def.chronic
			select hd).TryRandomElement(out hediff))
			{
				return;
			}
			HealthUtility.Cure(hediff);
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Messages.Message("MessagePermanentWoundHealed".Translate(cause, pawn.LabelShort, hediff.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x000BF23D File Offset: 0x000BD43D
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x000BF251 File Offset: 0x000BD451
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal.ToString();
		}

		// Token: 0x04001888 RID: 6280
		private int ticksToHeal;
	}
}
