using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAB RID: 7595
	public class PawnColumnWorker_FollowFieldwork : PawnColumnWorker_Checkbox
	{
		// Token: 0x170024AE RID: 9390
		// (get) Token: 0x0600C579 RID: 50553 RVA: 0x00488C61 File Offset: 0x00486E61
		public override bool VisibleCurrently
		{
			get
			{
				return this.anyAnimalWithObedience;
			}
		}

		// Token: 0x0600C57A RID: 50554 RVA: 0x00488C6C File Offset: 0x00486E6C
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

		// Token: 0x0600C57B RID: 50555 RVA: 0x00488CE8 File Offset: 0x00486EE8
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600C57C RID: 50556 RVA: 0x00488D16 File Offset: 0x00486F16
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followFieldwork;
		}

		// Token: 0x0600C57D RID: 50557 RVA: 0x00488D23 File Offset: 0x00486F23
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.followFieldwork = value;
		}

		// Token: 0x040070DD RID: 28893
		private bool anyAnimalWithObedience;
	}
}
