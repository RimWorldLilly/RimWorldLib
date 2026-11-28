using System;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200238F RID: 9103
	public static class CaravanRideableUtility
	{
		// Token: 0x0600E269 RID: 57961 RVA: 0x00540DAD File Offset: 0x0053EFAD
		public static bool IsCaravanRideable(this Pawn pawn)
		{
			return !pawn.Downed && pawn.ageTracker.CurLifeStage.caravanRideable && pawn.GetStatValue(StatDefOf.CaravanRidingSpeedFactor, true, -1) > 1f;
		}

		// Token: 0x0600E26A RID: 57962 RVA: 0x00540DDF File Offset: 0x0053EFDF
		public static bool IsCaravanRideable(this ThingDef def)
		{
			return def.StatBaseDefined(StatDefOf.CaravanRidingSpeedFactor);
		}

		// Token: 0x0600E26B RID: 57963 RVA: 0x00540DEC File Offset: 0x0053EFEC
		public static string RideableLifeStagesDesc(RaceProperties raceProps)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LifeStageAge lifeStageAge in raceProps.lifeStageAges)
			{
				if (lifeStageAge.def.caravanRideable)
				{
					stringBuilder.AppendWithComma(lifeStageAge.def.label);
				}
			}
			return stringBuilder.ToString().CapitalizeFirst();
		}

		// Token: 0x0600E26C RID: 57964 RVA: 0x00540E68 File Offset: 0x0053F068
		public static string GetIconTooltipText(Pawn pawn)
		{
			float statValue = pawn.GetStatValue(StatDefOf.CaravanRidingSpeedFactor, true, -1);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("RideableAnimalTip".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(StatDefOf.CaravanRidingSpeedFactor.LabelCap);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(statValue.ToStringPercent());
			stringBuilder.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			return stringBuilder.ToString();
		}
	}
}
