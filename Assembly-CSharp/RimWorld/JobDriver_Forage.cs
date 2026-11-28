using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A3B RID: 2619
	public class JobDriver_Forage : JobDriver
	{
		// Token: 0x06004F04 RID: 20228 RVA: 0x001DFA37 File Offset: 0x001DDC37
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x001DFA5B File Offset: 0x001DDC5B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Forage.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_Forage.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x001DFA6B File Offset: 0x001DDC6B
		private Toil CompleteForageToil()
		{
			Toil toil = ToilMaker.MakeToil("CompleteForageToil");
			toil.initAction = delegate()
			{
				Thing thing = ThingMaker.MakeThing(this.pawn.Map.Biome.foragedFood, null);
				thing.stackCount = JobDriver_Forage.StackCountRange.RandomInRange;
				GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
				Messages.Message("MessageAnimalForagedFood".Translate(this.pawn.Named("ANIMAL"), thing.Named("FOOD")), thing, MessageTypeDefOf.PositiveEvent, true);
			};
			return toil;
		}

		// Token: 0x040030DD RID: 12509
		private const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x040030DE RID: 12510
		private const int ForageTicks = 240;

		// Token: 0x040030DF RID: 12511
		private static readonly IntRange StackCountRange = new IntRange(10, 25);
	}
}
