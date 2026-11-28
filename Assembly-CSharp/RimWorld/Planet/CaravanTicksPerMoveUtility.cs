using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002392 RID: 9106
	public static class CaravanTicksPerMoveUtility
	{
		// Token: 0x0600E27C RID: 57980 RVA: 0x0054179A File Offset: 0x0053F99A
		public static int GetTicksPerMove(Caravan caravan, StringBuilder explanation = null)
		{
			if (caravan == null)
			{
				if (explanation != null)
				{
					CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
				}
				return 3300;
			}
			return CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(caravan), explanation);
		}

		// Token: 0x0600E27D RID: 57981 RVA: 0x005417BA File Offset: 0x0053F9BA
		public static int GetTicksPerMove(CaravanTicksPerMoveUtility.CaravanInfo caravanInfo, StringBuilder explanation = null)
		{
			return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.pawns, caravanInfo.massUsage, caravanInfo.massCapacity, caravanInfo.isShuttle, explanation);
		}

		// Token: 0x0600E27E RID: 57982 RVA: 0x005417DC File Offset: 0x0053F9DC
		public static int GetTicksPerMove(List<Pawn> pawns, float massUsage, float massCapacity, bool isShuttle = false, StringBuilder explanation = null)
		{
			CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Clear();
			if (!pawns.Any<Pawn>())
			{
				if (explanation != null)
				{
					CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
				}
				return 3300;
			}
			if (isShuttle)
			{
				if (explanation != null)
				{
					explanation.Append("CaravanMovementSpeedShuttle".Translate());
				}
				return 0;
			}
			int num = 0;
			foreach (Pawn pawn in pawns)
			{
				if (pawn.RaceProps.Humanlike)
				{
					num++;
				}
				else if (pawn.IsCaravanRideable())
				{
					CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Add(pawn.GetStatValue(StatDefOf.CaravanRidingSpeedFactor, true, -1));
				}
			}
			float num2 = 1f;
			int num3 = 0;
			int count = CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count;
			if (count > 0 && num > 0)
			{
				CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Sort();
				CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Reverse();
				if (CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count > num)
				{
					CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.RemoveRange(num, CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count - num);
				}
				num3 = CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count;
				while (CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count < num)
				{
					CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Add(1f);
				}
				num2 = CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Average();
			}
			float num4 = 0f;
			int num5 = 0;
			foreach (Pawn pawn2 in pawns)
			{
				if (CaravanBonusUtility.HasCaravanBonus(pawn2))
				{
					num4 += pawn2.GetStatValue(StatDefOf.CaravanBonusSpeedFactor, true, -1);
					num5++;
				}
			}
			if (num5 != 0)
			{
				num4 /= (float)num5;
			}
			else
			{
				num4 = 1f;
			}
			float num6 = (float)CaravanTicksPerMoveUtility.BaseHumanlikeTicksPerCell() * 340f;
			float moveSpeedFactorFromMass = CaravanTicksPerMoveUtility.GetMoveSpeedFactorFromMass(massUsage, massCapacity);
			int num7 = Mathf.Max(Mathf.RoundToInt(num6 / (moveSpeedFactorFromMass * num2 * num4)), 1);
			bool flag = massUsage > massCapacity;
			if (explanation != null)
			{
				float num8 = 60000f / num6;
				explanation.Append("CaravanMovementSpeedFull".Translate() + ":");
				explanation.AppendLine();
				explanation.Append("  " + "StatsReport_BaseValue".Translate() + ": " + num8.ToString("0.#") + " " + "TilesPerDay".Translate());
				explanation.AppendLine();
				TaggedString t = "  " + "RideableAnimalsPerPeople".Translate();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(count);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				explanation.Append(t + defaultInterpolatedStringHandler.ToStringAndClear());
				if (num3 > 0)
				{
					explanation.AppendLine();
					explanation.Append("  " + "MultiplierFromRiddenAnimals".Translate() + ": " + num2.ToStringPercent());
				}
				if (!Mathf.Approximately(num4, 1f))
				{
					explanation.AppendLine();
					explanation.Append("  " + "MultiplierFromMoveSpeedBonus".Translate() + ": " + num4.ToStringPercent());
				}
				if (!flag)
				{
					explanation.AppendLine();
					explanation.Append("  " + "MultiplierForCarriedMass".Translate(moveSpeedFactorFromMass.ToStringPercent()));
				}
				float num9 = 60000f / (float)num7;
				explanation.AppendLine();
				explanation.Append("  " + "FinalCaravanPawnsMovementSpeed".Translate() + ": " + num9.ToString("0.#") + " " + "TilesPerDay".Translate());
			}
			return num7;
		}

		// Token: 0x0600E27F RID: 57983 RVA: 0x00541BFC File Offset: 0x0053FDFC
		private static int BaseHumanlikeTicksPerCell()
		{
			float num = ThingDefOf.Human.GetStatValueAbstract(StatDefOf.MoveSpeed, null) / 60f;
			return Mathf.RoundToInt(1f / num);
		}

		// Token: 0x0600E280 RID: 57984 RVA: 0x00541C2C File Offset: 0x0053FE2C
		private static float GetMoveSpeedFactorFromMass(float massUsage, float massCapacity)
		{
			if (massCapacity <= 0f)
			{
				return 1f;
			}
			float t = massUsage / massCapacity;
			return Mathf.Lerp(2f, 1f, t);
		}

		// Token: 0x0600E281 RID: 57985 RVA: 0x00541C5C File Offset: 0x0053FE5C
		private static void AppendUsingDefaultTicksPerMoveInfo(StringBuilder sb)
		{
			sb.Append("CaravanMovementSpeedFull".Translate() + ":");
			float num = 18.181818f;
			sb.AppendLine();
			sb.Append("  " + "Default".Translate() + ": " + num.ToString("0.#") + " " + "TilesPerDay".Translate());
		}

		// Token: 0x0400908D RID: 37005
		public const float CellToTilesConversionRatio = 340f;

		// Token: 0x0400908E RID: 37006
		public const int DefaultTicksPerMove = 3300;

		// Token: 0x0400908F RID: 37007
		private const float MoveSpeedFactorAtZeroMass = 2f;

		// Token: 0x04009090 RID: 37008
		private static readonly List<float> caravanAnimalSpeedFactors = new List<float>();

		// Token: 0x04009091 RID: 37009
		private static readonly List<float> nomadSpeedFactors = new List<float>();

		// Token: 0x02003E82 RID: 16002
		public struct CaravanInfo
		{
			// Token: 0x06016222 RID: 90658 RVA: 0x006E5072 File Offset: 0x006E3272
			public CaravanInfo(Caravan caravan)
			{
				this.pawns = caravan.PawnsListForReading;
				this.massUsage = caravan.MassUsage;
				this.massCapacity = caravan.MassCapacity;
				this.isShuttle = (caravan.Shuttle != null);
			}

			// Token: 0x06016223 RID: 90659 RVA: 0x006E50A7 File Offset: 0x006E32A7
			public CaravanInfo(Dialog_FormCaravan formCaravanDialog)
			{
				this.pawns = TransferableUtility.GetPawnsFromTransferables(formCaravanDialog.transferables);
				this.massUsage = formCaravanDialog.MassUsage;
				this.massCapacity = formCaravanDialog.MassCapacity;
				this.isShuttle = false;
			}

			// Token: 0x0400F665 RID: 63077
			public List<Pawn> pawns;

			// Token: 0x0400F666 RID: 63078
			public float massUsage;

			// Token: 0x0400F667 RID: 63079
			public float massCapacity;

			// Token: 0x0400F668 RID: 63080
			public bool isShuttle;
		}
	}
}
