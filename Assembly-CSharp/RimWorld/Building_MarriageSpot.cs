using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ABC RID: 6844
	public class Building_MarriageSpot : Building
	{
		// Token: 0x0600AE05 RID: 44549 RVA: 0x003D5578 File Offset: 0x003D3778
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append(this.UsableNowStatus());
			return stringBuilder.ToString();
		}

		// Token: 0x0600AE06 RID: 44550 RVA: 0x003D55BC File Offset: 0x003D37BC
		private string UsableNowStatus()
		{
			if (!this.AnyCoupleForWhichIsValid())
			{
				StringBuilder stringBuilder = new StringBuilder();
				Pair<Pawn, Pawn> pair;
				if (this.TryFindAnyFiancesCouple(out pair))
				{
					if (!MarriageSpotUtility.IsValidMarriageSpotFor(base.Position, pair.First, pair.Second, stringBuilder))
					{
						return "MarriageSpotNotUsable".Translate(stringBuilder);
					}
				}
				else if (!MarriageSpotUtility.IsValidMarriageSpot(base.Position, base.Map, stringBuilder))
				{
					return "MarriageSpotNotUsable".Translate(stringBuilder);
				}
			}
			return "MarriageSpotUsable".Translate();
		}

		// Token: 0x0600AE07 RID: 44551 RVA: 0x003D564C File Offset: 0x003D384C
		private bool AnyCoupleForWhichIsValid()
		{
			return base.Map.mapPawns.FreeColonistsSpawned.Any(delegate(Pawn p)
			{
				Pawn firstDirectRelationPawn = p.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => x.Spawned);
				return firstDirectRelationPawn != null && MarriageSpotUtility.IsValidMarriageSpotFor(base.Position, p, firstDirectRelationPawn, null);
			});
		}

		// Token: 0x0600AE08 RID: 44552 RVA: 0x003D5670 File Offset: 0x003D3870
		private bool TryFindAnyFiancesCouple(out Pair<Pawn, Pawn> fiances)
		{
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => x.Spawned);
				if (firstDirectRelationPawn != null)
				{
					fiances = new Pair<Pawn, Pawn>(pawn, firstDirectRelationPawn);
					return true;
				}
			}
			fiances = default(Pair<Pawn, Pawn>);
			return false;
		}
	}
}
