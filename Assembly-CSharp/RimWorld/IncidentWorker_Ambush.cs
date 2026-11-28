using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200115E RID: 4446
	public abstract class IncidentWorker_Ambush : IncidentWorker
	{
		// Token: 0x06006FAE RID: 28590
		protected abstract List<Pawn> GeneratePawns(IncidentParms parms);

		// Token: 0x06006FAF RID: 28591 RVA: 0x00270BE1 File Offset: 0x0026EDE1
		protected virtual void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
		}

		// Token: 0x06006FB0 RID: 28592 RVA: 0x00270BE3 File Offset: 0x0026EDE3
		protected virtual LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return null;
		}

		// Token: 0x06006FB1 RID: 28593 RVA: 0x00270BE8 File Offset: 0x0026EDE8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = parms.target as Map;
			if (map != null)
			{
				IntVec3 intVec;
				return this.TryFindEntryCell(map, out intVec);
			}
			return CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile);
		}

		// Token: 0x06006FB2 RID: 28594 RVA: 0x00270C20 File Offset: 0x0026EE20
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			IntVec3 existingMapEdgeCell = IntVec3.Invalid;
			if (map != null && !this.TryFindEntryCell(map, out existingMapEdgeCell))
			{
				return false;
			}
			List<Pawn> generatedEnemies = this.GeneratePawns(parms);
			if (!generatedEnemies.Any<Pawn>())
			{
				return false;
			}
			if (map != null)
			{
				return this.DoExecute(parms, generatedEnemies, existingMapEdgeCell);
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				this.DoExecute(parms, generatedEnemies, existingMapEdgeCell);
			}, "GeneratingMapForNewEncounter", false, null, true, false, null);
			return true;
		}

		// Token: 0x06006FB3 RID: 28595 RVA: 0x00270CCC File Offset: 0x0026EECC
		private bool DoExecute(IncidentParms parms, List<Pawn> generatedEnemies, IntVec3 existingMapEdgeCell)
		{
			Map map = parms.target as Map;
			bool flag = false;
			if (map == null)
			{
				map = CaravanIncidentUtility.SetupCaravanAttackMap((Caravan)parms.target, generatedEnemies, false);
				flag = true;
			}
			else
			{
				for (int i = 0; i < generatedEnemies.Count; i++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(existingMapEdgeCell, map, 4);
					GenSpawn.Spawn(generatedEnemies[i], loc, map, Rot4.Random, WipeMode.Vanish, false, false);
				}
			}
			this.PostProcessGeneratedPawnsAfterSpawning(generatedEnemies);
			LordJob lordJob = this.CreateLordJob(generatedEnemies, parms);
			if (lordJob != null)
			{
				LordMaker.MakeNewLord(parms.faction, lordJob, map, generatedEnemies);
			}
			TaggedString baseLetterLabel = this.GetLetterLabel(generatedEnemies[0], parms);
			TaggedString baseLetterText = this.GetLetterText(generatedEnemies[0], parms);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(generatedEnemies, ref baseLetterLabel, ref baseLetterText, this.GetRelatedPawnsInfoLetterText(parms), true, true);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.GetLetterDef(generatedEnemies[0], parms), parms, generatedEnemies[0], Array.Empty<NamedArgument>());
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
			}
			return true;
		}

		// Token: 0x06006FB4 RID: 28596 RVA: 0x00270DD0 File Offset: 0x0026EFD0
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && map.reachability.CanReachColony(x), map, CellFinder.EdgeRoadChance_Hostile, out cell);
		}

		// Token: 0x06006FB5 RID: 28597 RVA: 0x00270E07 File Offset: 0x0026F007
		protected virtual string GetLetterLabel(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterLabel;
		}

		// Token: 0x06006FB6 RID: 28598 RVA: 0x00270E14 File Offset: 0x0026F014
		protected virtual string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterText;
		}

		// Token: 0x06006FB7 RID: 28599 RVA: 0x00270E21 File Offset: 0x0026F021
		protected virtual LetterDef GetLetterDef(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterDef;
		}

		// Token: 0x06006FB8 RID: 28600 RVA: 0x00270E2E File Offset: 0x0026F02E
		protected virtual string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural);
		}
	}
}
