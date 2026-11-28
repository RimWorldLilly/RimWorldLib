using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015C4 RID: 5572
	public class Pawn_GuiltTracker : IExposable
	{
		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x06008A25 RID: 35365 RVA: 0x0031FE6D File Offset: 0x0031E06D
		public bool IsGuilty
		{
			get
			{
				return this.guiltyTicksLeft > 0 || (this.pawn.InAggroMentalState && this.pawn.MentalStateDef.allowGuilty);
			}
		}

		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x06008A26 RID: 35366 RVA: 0x0031FE99 File Offset: 0x0031E099
		public int TicksUntilInnocent
		{
			get
			{
				return this.guiltyTicksLeft;
			}
		}

		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x06008A27 RID: 35367 RVA: 0x0031FEA1 File Offset: 0x0031E0A1
		public string Tip
		{
			get
			{
				return "GuiltyDesc".Translate() + ": " + this.TicksUntilInnocent.ToStringTicksToPeriod(true, false, true, true, false);
			}
		}

		// Token: 0x06008A28 RID: 35368 RVA: 0x0031FED1 File Offset: 0x0031E0D1
		public Pawn_GuiltTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008A29 RID: 35369 RVA: 0x0031FEE0 File Offset: 0x0031E0E0
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.guiltyTicksLeft, "guiltyTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.awaitingExecution, "awaitingExecution", false, false);
		}

		// Token: 0x06008A2A RID: 35370 RVA: 0x0031FF06 File Offset: 0x0031E106
		public void Notify_Guilty(int durationTicks = 60000)
		{
			this.guiltyTicksLeft = durationTicks;
		}

		// Token: 0x06008A2B RID: 35371 RVA: 0x0031FF0F File Offset: 0x0031E10F
		public void GuiltTrackerTickInterval(int delta)
		{
			if (this.guiltyTicksLeft > 0)
			{
				this.guiltyTicksLeft -= delta;
				return;
			}
			if (!this.IsGuilty)
			{
				this.awaitingExecution = false;
			}
		}

		// Token: 0x0400551C RID: 21788
		private Pawn pawn;

		// Token: 0x0400551D RID: 21789
		public bool awaitingExecution;

		// Token: 0x0400551E RID: 21790
		private int guiltyTicksLeft;

		// Token: 0x0400551F RID: 21791
		private const int DefaultGuiltyDuration = 60000;
	}
}
