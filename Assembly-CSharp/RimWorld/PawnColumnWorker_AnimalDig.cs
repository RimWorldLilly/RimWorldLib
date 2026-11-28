using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA6 RID: 7590
	public class PawnColumnWorker_AnimalDig : PawnColumnWorker_Checkbox
	{
		// Token: 0x170024AA RID: 9386
		// (get) Token: 0x0600C551 RID: 50513 RVA: 0x0048867A File Offset: 0x0048687A
		public override bool VisibleCurrently
		{
			get
			{
				return this.anyAnimalWithDig;
			}
		}

		// Token: 0x0600C552 RID: 50514 RVA: 0x00488684 File Offset: 0x00486884
		public override void Recache()
		{
			this.anyAnimalWithDig = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.ColonyAnimals)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Dig))
				{
					this.anyAnimalWithDig = true;
					break;
				}
			}
		}

		// Token: 0x0600C553 RID: 50515 RVA: 0x00488700 File Offset: 0x00486900
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.animalDig;
		}

		// Token: 0x0600C554 RID: 50516 RVA: 0x0048870D File Offset: 0x0048690D
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.animalDig = value;
		}

		// Token: 0x0600C555 RID: 50517 RVA: 0x0048871B File Offset: 0x0048691B
		protected override bool HasCheckbox(Pawn pawn)
		{
			Pawn_TrainingTracker training = pawn.training;
			return training != null && training.HasLearned(TrainableDefOf.Dig);
		}

		// Token: 0x040070D9 RID: 28889
		private bool anyAnimalWithDig;
	}
}
