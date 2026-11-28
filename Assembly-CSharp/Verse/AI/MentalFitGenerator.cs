using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x0200085E RID: 2142
	public class MentalFitGenerator : IExposable
	{
		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06004334 RID: 17204 RVA: 0x001A8371 File Offset: 0x001A6571
		public bool CanDoRandomMentalFits
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && !this.pawn.IsWorldPawn();
			}
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x001A8395 File Offset: 0x001A6595
		public MentalFitGenerator()
		{
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x001A839D File Offset: 0x001A659D
		public MentalFitGenerator(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x001A83AC File Offset: 0x001A65AC
		public void Reset()
		{
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x001A83AE File Offset: 0x001A65AE
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksUntilCanDoMentalFit, "ticksUntilCanDoMentalFit", 0, false);
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x001A83C4 File Offset: 0x001A65C4
		public void TickInterval(int delta)
		{
			if (this.ticksUntilCanDoMentalFit > 0)
			{
				if (this.pawn.Awake())
				{
					this.ticksUntilCanDoMentalFit -= delta;
				}
				return;
			}
			if (!this.pawn.Awake())
			{
				return;
			}
			if (!this.pawn.IsHashIntervalTick(150, delta))
			{
				return;
			}
			if (!this.CanDoRandomMentalFits)
			{
				return;
			}
			if (this.pawn.InMentalState)
			{
				return;
			}
			foreach (MentalFitDef mentalFitDef in DefDatabase<MentalFitDef>.AllDefsListForReading)
			{
				if (Rand.MTBEventOccurs(mentalFitDef.CalculateMTBDays(this.pawn), 60000f, 150f) && this.pawn.mindState.mentalStateHandler.TryStartMentalState(mentalFitDef.mentalState, null, false, false, false, null, true, false, false))
				{
					break;
				}
			}
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x001A84B4 File Offset: 0x001A66B4
		public void Notify_RecoveredFromMentalState()
		{
			this.ticksUntilCanDoMentalFit = 600;
		}

		// Token: 0x04002BEC RID: 11244
		private Pawn pawn;

		// Token: 0x04002BED RID: 11245
		private int ticksUntilCanDoMentalFit;

		// Token: 0x04002BEE RID: 11246
		private const int CheckInterval = 150;

		// Token: 0x04002BEF RID: 11247
		private const int MinTicksSinceRecoveryToHaveFit = 600;
	}
}
