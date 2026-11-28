using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020004C7 RID: 1223
	public class Hediff_Scaria : Hediff
	{
		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x000CA078 File Offset: 0x000C8278
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600220C RID: 8716 RVA: 0x000CA07C File Offset: 0x000C827C
		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.TicksLeft >= 0)
				{
					stringBuilder.Append("DeathIn".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)).Resolve().CapitalizeFirst());
				}
				string tipStringExtra = base.TipStringExtra;
				if (!tipStringExtra.NullOrEmpty())
				{
					stringBuilder.AppendLineIfNotEmpty();
					stringBuilder.AppendLineIfNotEmpty();
					stringBuilder.Append(tipStringExtra);
				}
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("  - " + "HarmedRevengeChance".Translate() + ": " + 0.5f.ToStringPercentSigned());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x000CA147 File Offset: 0x000C8347
		private int TicksLeft
		{
			get
			{
				return 300000 - (Find.TickManager.TicksGame - this.manhunterTick);
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x000CA160 File Offset: 0x000C8360
		private bool IsBerserk
		{
			get
			{
				return (this.pawn.RaceProps.Humanlike && this.pawn.mindState.mentalStateHandler.CurStateDef == MentalStateDefOf.BerserkPermanent) || (this.pawn.RaceProps.Animal && this.pawn.mindState.mentalStateHandler.CurStateDef == MentalStateDefOf.ManhunterPermanent);
			}
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000CA1CD File Offset: 0x000C83CD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.manhunterTick, "manhunterTick", 0, false);
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000CA1E8 File Offset: 0x000C83E8
		public override void TickInterval(int delta)
		{
			if (this.pawn.RaceProps.Humanlike && !this.pawn.mindState.mentalStateHandler.InMentalState && !this.pawn.Downed)
			{
				this.StartMentalState();
			}
			if (this.pawn.IsAnimal && this.pawn.Faction != null)
			{
				this.StartMentalState();
			}
			if (this.manhunterTick < 0 && this.IsBerserk)
			{
				this.manhunterTick = Find.TickManager.TicksGame;
			}
			if (this.manhunterTick > 0 && !this.IsBerserk)
			{
				this.manhunterTick = -999999;
			}
			if (this.manhunterTick > 0 && Find.TickManager.TicksGame - this.manhunterTick > 300000)
			{
				this.pawn.Kill(null, this);
			}
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x000CA2C4 File Offset: 0x000C84C4
		private void StartMentalState()
		{
			this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.BerserkPermanent, "CausedByHediff".Translate(this.def.LabelCap), true, true, false, null, true, false, false);
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x000CA312 File Offset: 0x000C8512
		public override void PostRemoved()
		{
			if (this.IsBerserk && !this.pawn.mindState.mentalStateHandler.CurState.causedByMood)
			{
				this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x000CA354 File Offset: 0x000C8554
		public override void Notify_PawnDamagedThing(Thing thing, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			if (result.hediffs.NullOrEmpty<Hediff>())
			{
				return;
			}
			foreach (Hediff hd in result.hediffs)
			{
				if (dinfo.Def == DamageDefOf.Bite || dinfo.Def == DamageDefOf.Scratch || dinfo.Def == DamageDefOf.ScratchToxic)
				{
					HediffComp_Infecter hediffComp_Infecter = hd.TryGetComp<HediffComp_Infecter>();
					if (hediffComp_Infecter != null)
					{
						hediffComp_Infecter.fromScaria = true;
					}
				}
			}
		}

		// Token: 0x040019A4 RID: 6564
		public const float ScariaManhunterChanceOffset = 0.5f;

		// Token: 0x040019A5 RID: 6565
		private const int KillAfterDaysManhunter = 5;

		// Token: 0x040019A6 RID: 6566
		private int manhunterTick = -999999;
	}
}
