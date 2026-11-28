using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1C RID: 6684
	public class CompTargetEffect_MutateIntoFleshbeast : CompTargetEffect
	{
		// Token: 0x0600A7F5 RID: 42997 RVA: 0x003BBDFC File Offset: 0x003B9FFC
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				FleshbeastUtility.MeatSplatter(CompTargetEffect_MutateIntoFleshbeast.BloodFilth.RandomInRange, pawn.PositionHeld, pawn.MapHeld, FleshbeastUtility.MeatExplosionSize.Large);
				FleshbeastUtility.SpawnFleshbeastFromPawn(pawn, false, true, Array.Empty<PawnKindDef>());
			}
		}

		// Token: 0x040062BE RID: 25278
		private static readonly IntRange BloodFilth = new IntRange(3, 4);
	}
}
