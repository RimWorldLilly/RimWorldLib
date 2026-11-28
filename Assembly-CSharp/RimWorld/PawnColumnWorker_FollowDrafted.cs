using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAA RID: 7594
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x170024AD RID: 9389
		// (get) Token: 0x0600C573 RID: 50547 RVA: 0x00488B8B File Offset: 0x00486D8B
		public override bool VisibleCurrently
		{
			get
			{
				return this.anyAnimalWithObedience;
			}
		}

		// Token: 0x0600C574 RID: 50548 RVA: 0x00488B94 File Offset: 0x00486D94
		public override void Recache()
		{
			this.anyAnimalWithObedience = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.ColonyAnimals)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Obedience))
				{
					this.anyAnimalWithObedience = true;
					break;
				}
			}
		}

		// Token: 0x0600C575 RID: 50549 RVA: 0x00488C10 File Offset: 0x00486E10
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600C576 RID: 50550 RVA: 0x00488C3E File Offset: 0x00486E3E
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x0600C577 RID: 50551 RVA: 0x00488C4B File Offset: 0x00486E4B
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.followDrafted = value;
		}

		// Token: 0x040070DC RID: 28892
		private bool anyAnimalWithObedience;
	}
}
