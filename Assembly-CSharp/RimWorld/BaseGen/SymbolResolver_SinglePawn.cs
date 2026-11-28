using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200210E RID: 8462
	public class SymbolResolver_SinglePawn : SymbolResolver
	{
		// Token: 0x0600D46D RID: 54381 RVA: 0x004E836C File Offset: 0x004E656C
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && ((rp.singlePawnToSpawn != null && rp.singlePawnToSpawn.Spawned) || SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out intVec));
		}

		// Token: 0x0600D46E RID: 54382 RVA: 0x004E83A8 File Offset: 0x004E65A8
		public override void Resolve(ResolveParams rp)
		{
			if (rp.singlePawnToSpawn != null && rp.singlePawnToSpawn.Spawned)
			{
				return;
			}
			Map map = BaseGen.globalSettings.map;
			IntVec3 loc;
			if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out loc))
			{
				if (rp.singlePawnToSpawn != null)
				{
					Find.WorldPawns.PassToWorld(rp.singlePawnToSpawn, PawnDiscardDecideMode.Decide);
				}
				return;
			}
			Pawn pawn;
			if (rp.singlePawnToSpawn == null)
			{
				PawnGenerationRequest value;
				if (rp.singlePawnGenerationRequest != null)
				{
					value = rp.singlePawnGenerationRequest.Value;
				}
				else
				{
					PawnKindDef pawnKindDef;
					if ((pawnKindDef = rp.singlePawnKindDef) == null)
					{
						pawnKindDef = (from x in DefDatabase<PawnKindDef>.AllDefsListForReading
						where x.defaultFactionDef == null || !x.defaultFactionDef.isPlayer
						select x).RandomElement<PawnKindDef>();
					}
					PawnKindDef pawnKindDef2 = pawnKindDef;
					Faction faction = rp.faction;
					if (faction == null && pawnKindDef2.RaceProps.Humanlike)
					{
						if (pawnKindDef2.defaultFactionDef != null)
						{
							faction = FactionUtility.DefaultFactionFrom(pawnKindDef2.defaultFactionDef);
							if (faction == null)
							{
								return;
							}
						}
						else if (!(from x in Find.FactionManager.AllFactions
						where !x.IsPlayer
						select x).TryRandomElement(out faction))
						{
							return;
						}
					}
					value = new PawnGenerationRequest(pawnKindDef2, faction, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, faction.deactivated, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
				}
				pawn = PawnGenerator.GeneratePawn(value);
				if (rp.postThingGenerate != null)
				{
					rp.postThingGenerate(pawn);
				}
			}
			else
			{
				pawn = rp.singlePawnToSpawn;
			}
			if (!pawn.Dead && rp.disableSinglePawn != null && rp.disableSinglePawn.Value)
			{
				CompCanBeDormant compCanBeDormant;
				if (pawn.TryGetComp(out compCanBeDormant))
				{
					compCanBeDormant.ToSleep();
				}
				pawn.mindState.Active = false;
			}
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			if (rp.singlePawnLord != null)
			{
				rp.singlePawnLord.AddPawn(pawn);
			}
			if (rp.postThingSpawn != null)
			{
				rp.postThingSpawn(pawn);
			}
		}

		// Token: 0x0600D46F RID: 54383 RVA: 0x004E8600 File Offset: 0x004E6800
		public static bool TryFindSpawnCell(ResolveParams rp, out IntVec3 cell)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rp.rect, (IntVec3 x) => x.Standable(map) && (rp.singlePawnSpawnCellExtraPredicate == null || rp.singlePawnSpawnCellExtraPredicate(x)), out cell);
		}
	}
}
