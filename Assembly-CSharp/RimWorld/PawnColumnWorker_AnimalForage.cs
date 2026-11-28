using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA7 RID: 7591
	public class PawnColumnWorker_AnimalForage : PawnColumnWorker_Checkbox
	{
		// Token: 0x170024AB RID: 9387
		// (get) Token: 0x0600C557 RID: 50519 RVA: 0x0048873B File Offset: 0x0048693B
		public override bool VisibleCurrently
		{
			get
			{
				return this.anyAnimalWithForage;
			}
		}

		// Token: 0x0600C558 RID: 50520 RVA: 0x00488744 File Offset: 0x00486944
		public override void Recache()
		{
			this.anyAnimalWithForage = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.ColonyAnimals)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Forage))
				{
					this.anyAnimalWithForage = true;
					break;
				}
			}
		}

		// Token: 0x0600C559 RID: 50521 RVA: 0x004887C0 File Offset: 0x004869C0
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.animalForage;
		}

		// Token: 0x0600C55A RID: 50522 RVA: 0x004887CD File Offset: 0x004869CD
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.animalForage = value;
		}

		// Token: 0x0600C55B RID: 50523 RVA: 0x004887DB File Offset: 0x004869DB
		protected override bool HasCheckbox(Pawn pawn)
		{
			Pawn_TrainingTracker training = pawn.training;
			return training != null && training.HasLearned(TrainableDefOf.Forage);
		}

		// Token: 0x040070DA RID: 28890
		private bool anyAnimalWithForage;
	}
}
