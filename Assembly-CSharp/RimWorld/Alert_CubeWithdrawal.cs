using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BAB RID: 7083
	public class Alert_CubeWithdrawal : Alert_Critical
	{
		// Token: 0x170020E3 RID: 8419
		// (get) Token: 0x0600B5CD RID: 46541 RVA: 0x00400787 File Offset: 0x003FE987
		protected override bool DoMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170020E4 RID: 8420
		// (get) Token: 0x0600B5CE RID: 46542 RVA: 0x0040078C File Offset: 0x003FE98C
		private List<Pawn> Withdrawal
		{
			get
			{
				this.inWithdrawal.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep)
				{
					if ((!ModsConfig.BiotechActive || !pawn.Deathresting) && Alert_CubeWithdrawal.InWithdrawal(pawn))
					{
						this.inWithdrawal.Add(pawn);
					}
				}
				return this.inWithdrawal;
			}
		}

		// Token: 0x0600B5CF RID: 46543 RVA: 0x0040080C File Offset: 0x003FEA0C
		public Alert_CubeWithdrawal()
		{
			this.defaultLabel = "AlertCubeWithdrawal".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0600B5D0 RID: 46544 RVA: 0x0040083C File Offset: 0x003FEA3C
		private static bool InWithdrawal(ThingWithComps thing)
		{
			Pawn pawn = thing as Pawn;
			Hediff hediff;
			return pawn != null && pawn.health != null && pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeWithdrawal, out hediff) && hediff.Severity >= 0.8f;
		}

		// Token: 0x0600B5D1 RID: 46545 RVA: 0x00400888 File Offset: 0x003FEA88
		public override string GetLabel()
		{
			if (this.inWithdrawal.Count == 1)
			{
				return this.inWithdrawal[0].Label + ": " + Alert_CubeWithdrawal.GetSeverity(this.inWithdrawal[0]).ToStringPercent("0");
			}
			return this.defaultLabel;
		}

		// Token: 0x0600B5D2 RID: 46546 RVA: 0x004008E0 File Offset: 0x003FEAE0
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.inWithdrawal)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve() + ": " + Alert_CubeWithdrawal.GetSeverity(pawn).ToStringPercent("0"));
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("AlertCubeWithdrawalDesc".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(":\n");
			defaultInterpolatedStringHandler.AppendFormatted(stringBuilder.ToString().TrimEndNewlines());
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("AlertCubeWithdrawalDescAppended".Translate());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600B5D3 RID: 46547 RVA: 0x004009C8 File Offset: 0x003FEBC8
		private static float GetSeverity(Pawn pawn)
		{
			return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.CubeWithdrawal, false).Severity;
		}

		// Token: 0x0600B5D4 RID: 46548 RVA: 0x004009E5 File Offset: 0x003FEBE5
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Withdrawal);
		}

		// Token: 0x0400688C RID: 26764
		private readonly List<Pawn> inWithdrawal = new List<Pawn>();

		// Token: 0x0400688D RID: 26765
		private const float AlertPercentage = 0.8f;
	}
}
