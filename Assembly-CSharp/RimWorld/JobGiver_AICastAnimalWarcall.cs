using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ACF RID: 2767
	public class JobGiver_AICastAnimalWarcall : JobGiver_AICastAbility
	{
		// Token: 0x060052F8 RID: 21240 RVA: 0x001EEA4C File Offset: 0x001ECC4C
		protected override LocalTargetInfo GetTarget(Pawn caster, Ability ability)
		{
			JobGiver_AICastAnimalWarcall.potentialTargets.Clear();
			IEnumerable<Thing> hostiles = from x in caster.Map.attackTargetsCache.GetPotentialTargetsFor(caster)
			select x.Thing;
			if (hostiles.EnumerableNullOrEmpty<Thing>())
			{
				return LocalTargetInfo.Invalid;
			}
			foreach (Pawn pawn in caster.Map.mapPawns.AllPawnsSpawned)
			{
				if ((!pawn.IsMutant || !pawn.mutant.Def.preventsMentalBreaks) && pawn.RaceProps.Animal && pawn.Faction == null && pawn.MentalStateDef != MentalStateDefOf.BerserkWarcall && pawn.Position.InHorDistOf(caster.Position, 30f) && ability.CanApplyOn(new LocalTargetInfo(pawn)))
				{
					JobGiver_AICastAnimalWarcall.potentialTargets.Add(pawn);
				}
			}
			Pawn thing;
			if (JobGiver_AICastAnimalWarcall.potentialTargets.TryRandomElementByWeight(delegate(Pawn x)
			{
				float num = 625f;
				foreach (Thing thing2 in hostiles)
				{
					if (thing2.Spawned)
					{
						float num2 = (float)thing2.Position.DistanceToSquared(x.Position);
						if (num2 < num)
						{
							num = num2;
						}
					}
				}
				return JobGiver_AICastAnimalWarcall.DistanceSquaredToTargetSelectionWeightCurve.Evaluate(num);
			}, out thing))
			{
				return new LocalTargetInfo(thing);
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x040031F2 RID: 12786
		private const float MaxDistanceFromCaster = 30f;

		// Token: 0x040031F3 RID: 12787
		private const float MaxSquareDistanceFromTarget = 625f;

		// Token: 0x040031F4 RID: 12788
		private static readonly SimpleCurve DistanceSquaredToTargetSelectionWeightCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(400f, 0.1f),
				true
			},
			{
				new CurvePoint(625f, 0f),
				true
			}
		};

		// Token: 0x040031F5 RID: 12789
		private static List<Pawn> potentialTargets = new List<Pawn>();
	}
}
