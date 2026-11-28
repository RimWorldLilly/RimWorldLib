using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001996 RID: 6550
	public class CompSpreadSludge : ThingComp
	{
		// Token: 0x17001C8F RID: 7311
		// (get) Token: 0x0600A469 RID: 42089 RVA: 0x003ABECB File Offset: 0x003AA0CB
		private CompProperties_SpreadSludge Props
		{
			get
			{
				return (CompProperties_SpreadSludge)this.props;
			}
		}

		// Token: 0x17001C90 RID: 7312
		// (get) Token: 0x0600A46A RID: 42090 RVA: 0x003ABED8 File Offset: 0x003AA0D8
		private Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x0600A46B RID: 42091 RVA: 0x003ABEE8 File Offset: 0x003AA0E8
		public override void CompTick()
		{
			CompSpreadSludge.<>c__DisplayClass4_0 CS$<>8__locals1 = new CompSpreadSludge.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			if (!this.Pawn.Spawned || this.Pawn.Downed)
			{
				return;
			}
			if (this.Props.abilityDef == AbilityDefOf.EggSpew)
			{
				Pawn_TrainingTracker training = this.Pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.EggSpew))
				{
					return;
				}
			}
			if (this.Props.abilityDef == AbilityDefOf.SludgeSpew)
			{
				Pawn_TrainingTracker training2 = this.Pawn.training;
				if (training2 != null && training2.HasLearned(TrainableDefOf.SludgeSpew))
				{
					return;
				}
			}
			if (!Rand.MTBEventOccurs((float)this.Props.mtbTicks, 1f, 1f))
			{
				return;
			}
			Pawn_AbilityTracker abilities = this.Pawn.abilities;
			Ability ability = (abilities != null) ? abilities.GetAbility(this.Props.abilityDef, false) : null;
			CS$<>8__locals1.range = this.Props.abilityDef.verbProperties.range;
			if (ability == null)
			{
				return;
			}
			IntVec3 c;
			if ((!this.Props.tryAvoidSludge || !CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, Mathf.CeilToInt(CS$<>8__locals1.range), (IntVec3 x) => base.<CompTick>g__ValidCell|0(x) && x.GetTerrain(CS$<>8__locals1.<>4__this.parent.Map) != TerrainDefOf.InsectSludge, out c, -1)) && !CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, Mathf.CeilToInt(CS$<>8__locals1.range), new Predicate<IntVec3>(CS$<>8__locals1.<CompTick>g__ValidCell|0), out c, -1))
			{
				return;
			}
			ability.verb.TryStartCastOn(c, c, false, true, true, false);
			return;
		}
	}
}
