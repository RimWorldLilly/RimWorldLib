using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004A1 RID: 1185
	public class Hediff_Alcohol : Hediff_High
	{
		// Token: 0x060020D2 RID: 8402 RVA: 0x000C4328 File Offset: 0x000C2528
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.CurStageIndex >= 3 && this.pawn.IsHashIntervalTick(300, delta) && this.HangoverSusceptible(this.pawn))
			{
				Hediff hediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hangover, false);
				if (hediff != null)
				{
					hediff.Severity = 1f;
					return;
				}
				hediff = HediffMaker.MakeHediff(HediffDefOf.Hangover, this.pawn, null);
				hediff.Severity = 1f;
				this.pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x000C43CD File Offset: 0x000C25CD
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}

		// Token: 0x04001931 RID: 6449
		private const int HangoverCheckInterval = 300;
	}
}
