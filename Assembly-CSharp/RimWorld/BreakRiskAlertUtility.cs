using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C24 RID: 7204
	public static class BreakRiskAlertUtility
	{
		// Token: 0x17002127 RID: 8487
		// (get) Token: 0x0600B7B7 RID: 47031 RVA: 0x0040C05C File Offset: 0x0040A25C
		public static List<Pawn> PawnsAtRiskExtreme
		{
			get
			{
				BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (!pawn.Downed && pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
					{
						BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Add(pawn);
					}
				}
				return BreakRiskAlertUtility.pawnsAtRiskExtremeResult;
			}
		}

		// Token: 0x17002128 RID: 8488
		// (get) Token: 0x0600B7B8 RID: 47032 RVA: 0x0040C0DC File Offset: 0x0040A2DC
		public static List<Pawn> PawnsAtRiskMajor
		{
			get
			{
				BreakRiskAlertUtility.pawnsAtRiskMajorResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (!pawn.Downed && pawn.mindState.mentalBreaker.BreakMajorIsImminent)
					{
						BreakRiskAlertUtility.pawnsAtRiskMajorResult.Add(pawn);
					}
				}
				return BreakRiskAlertUtility.pawnsAtRiskMajorResult;
			}
		}

		// Token: 0x17002129 RID: 8489
		// (get) Token: 0x0600B7B9 RID: 47033 RVA: 0x0040C15C File Offset: 0x0040A35C
		public static List<Pawn> PawnsAtRiskMinor
		{
			get
			{
				BreakRiskAlertUtility.pawnsAtRiskMinorResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (!pawn.Downed && pawn.mindState.mentalBreaker.BreakMinorIsImminent)
					{
						BreakRiskAlertUtility.pawnsAtRiskMinorResult.Add(pawn);
					}
				}
				return BreakRiskAlertUtility.pawnsAtRiskMinorResult;
			}
		}

		// Token: 0x1700212A RID: 8490
		// (get) Token: 0x0600B7BA RID: 47034 RVA: 0x0040C1DC File Offset: 0x0040A3DC
		public static string AlertLabel
		{
			get
			{
				int num = BreakRiskAlertUtility.PawnsAtRiskExtreme.Count<Pawn>();
				string text;
				if (num > 0)
				{
					text = "BreakRiskExtreme".Translate();
				}
				else
				{
					num = BreakRiskAlertUtility.PawnsAtRiskMajor.Count<Pawn>();
					if (num > 0)
					{
						text = "BreakRiskMajor".Translate();
					}
					else
					{
						num = BreakRiskAlertUtility.PawnsAtRiskMinor.Count<Pawn>();
						text = "BreakRiskMinor".Translate();
					}
				}
				if (num > 1)
				{
					text = text + " x" + num.ToStringCached();
				}
				return text;
			}
		}

		// Token: 0x1700212B RID: 8491
		// (get) Token: 0x0600B7BB RID: 47035 RVA: 0x0040C260 File Offset: 0x0040A460
		public static string AlertExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (BreakRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>())
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (Pawn pawn in BreakRiskAlertUtility.PawnsAtRiskExtreme)
					{
						stringBuilder2.AppendLine("  - " + pawn.NameShortColored.Resolve());
					}
					stringBuilder.Append("BreakRiskExtremeDesc".Translate(stringBuilder2).Resolve());
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder3 = new StringBuilder();
					foreach (Pawn pawn2 in BreakRiskAlertUtility.PawnsAtRiskMajor)
					{
						stringBuilder3.AppendLine("  - " + pawn2.NameShortColored.Resolve());
					}
					stringBuilder.Append("BreakRiskMajorDesc".Translate(stringBuilder3).Resolve());
				}
				if (BreakRiskAlertUtility.PawnsAtRiskMinor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder4 = new StringBuilder();
					foreach (Pawn pawn3 in BreakRiskAlertUtility.PawnsAtRiskMinor)
					{
						stringBuilder4.AppendLine("  - " + pawn3.NameShortColored.Resolve());
					}
					stringBuilder.Append("BreakRiskMinorDesc".Translate(stringBuilder4).Resolve());
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("BreakRiskDescEnding".Translate());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600B7BC RID: 47036 RVA: 0x0040C46C File Offset: 0x0040A66C
		public static void Clear()
		{
			BreakRiskAlertUtility.pawnsAtRiskExtremeResult.Clear();
			BreakRiskAlertUtility.pawnsAtRiskMajorResult.Clear();
			BreakRiskAlertUtility.pawnsAtRiskMinorResult.Clear();
		}

		// Token: 0x0400694B RID: 26955
		private static List<Pawn> pawnsAtRiskExtremeResult = new List<Pawn>();

		// Token: 0x0400694C RID: 26956
		private static List<Pawn> pawnsAtRiskMajorResult = new List<Pawn>();

		// Token: 0x0400694D RID: 26957
		private static List<Pawn> pawnsAtRiskMinorResult = new List<Pawn>();
	}
}
