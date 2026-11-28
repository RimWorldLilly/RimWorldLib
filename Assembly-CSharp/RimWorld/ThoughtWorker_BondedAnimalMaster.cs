using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFA RID: 3578
	public class ThoughtWorker_BondedAnimalMaster : ThoughtWorker
	{
		// Token: 0x060060DB RID: 24795 RVA: 0x00230C94 File Offset: 0x0022EE94
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtWorker_BondedAnimalMaster.tmpAnimals.Clear();
			this.GetAnimals(p, ThoughtWorker_BondedAnimalMaster.tmpAnimals);
			if (!ThoughtWorker_BondedAnimalMaster.tmpAnimals.Any<string>())
			{
				return false;
			}
			if (ThoughtWorker_BondedAnimalMaster.tmpAnimals.Count == 1)
			{
				return ThoughtState.ActiveAtStage(0, ThoughtWorker_BondedAnimalMaster.tmpAnimals[0]);
			}
			return ThoughtState.ActiveAtStage(1, ThoughtWorker_BondedAnimalMaster.tmpAnimals.ToCommaList(true, false));
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x00230CFB File Offset: 0x0022EEFB
		protected virtual bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster == p;
		}

		// Token: 0x060060DD RID: 24797 RVA: 0x00230D0C File Offset: 0x0022EF0C
		public void GetAnimals(Pawn p, List<string> outAnimals)
		{
			outAnimals.Clear();
			List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = directRelations[i];
				Pawn otherPawn = directPawnRelation.otherPawn;
				if (directPawnRelation.def == PawnRelationDefOf.Bond && !otherPawn.Dead && otherPawn.Spawned && otherPawn.Faction == Faction.OfPlayer && otherPawn.training.HasLearned(TrainableDefOf.Obedience) && this.AnimalMasterCheck(p, otherPawn))
				{
					outAnimals.Add(otherPawn.LabelShort);
				}
			}
		}

		// Token: 0x060060DE RID: 24798 RVA: 0x00230D9B File Offset: 0x0022EF9B
		public int GetAnimalsCount(Pawn p)
		{
			ThoughtWorker_BondedAnimalMaster.tmpAnimals.Clear();
			this.GetAnimals(p, ThoughtWorker_BondedAnimalMaster.tmpAnimals);
			return ThoughtWorker_BondedAnimalMaster.tmpAnimals.Count;
		}

		// Token: 0x040035FD RID: 13821
		private static List<string> tmpAnimals = new List<string>();
	}
}
