using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001791 RID: 6033
	public static class PawnGenerationContextUtility
	{
		// Token: 0x0600964B RID: 38475 RVA: 0x00369614 File Offset: 0x00367814
		public static string ToStringHuman(this PawnGenerationContext context)
		{
			switch (context)
			{
			case PawnGenerationContext.All:
				return "PawnGenerationContext_All".Translate();
			case PawnGenerationContext.PlayerStarter:
				return "PawnGenerationContext_PlayerStarter".Translate();
			case PawnGenerationContext.NonPlayer:
				return "PawnGenerationContext_NonPlayer".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600964C RID: 38476 RVA: 0x0036966A File Offset: 0x0036786A
		public static bool Includes(this PawnGenerationContext includer, PawnGenerationContext other)
		{
			return includer == PawnGenerationContext.All || includer == other;
		}

		// Token: 0x0600964D RID: 38477 RVA: 0x00369678 File Offset: 0x00367878
		public static PawnGenerationContext GetRandom()
		{
			Array values = Enum.GetValues(typeof(PawnGenerationContext));
			return (PawnGenerationContext)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x0600964E RID: 38478 RVA: 0x003696AC File Offset: 0x003678AC
		public static bool OverlapsWith(this PawnGenerationContext a, PawnGenerationContext b)
		{
			return a == PawnGenerationContext.All || b == PawnGenerationContext.All || a == b;
		}
	}
}
