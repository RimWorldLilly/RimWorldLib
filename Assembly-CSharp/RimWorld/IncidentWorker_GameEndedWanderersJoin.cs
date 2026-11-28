using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001179 RID: 4473
	public class IncidentWorker_GameEndedWanderersJoin : IncidentWorker
	{
		// Token: 0x06007040 RID: 28736 RVA: 0x00274CCC File Offset: 0x00272ECC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return this.CanSpawnJoiner(map);
		}

		// Token: 0x06007041 RID: 28737 RVA: 0x00274CF8 File Offset: 0x00272EF8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!this.CanSpawnJoiner(map))
			{
				return false;
			}
			IntVec3 loc;
			this.TryFindEntryCell(map, out loc);
			List<Pawn> startingAndOptionalPawns = Find.GameInitData.startingAndOptionalPawns;
			foreach (Pawn pawn in startingAndOptionalPawns)
			{
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				foreach (ThingDefCount t in Find.GameInitData.startingPossessions[pawn])
				{
					ThingWithComps thingWithComps = StartingPawnUtility.GenerateStartingPossession(t) as ThingWithComps;
					if (thingWithComps.HasComp<CompEquippable>())
					{
						pawn.equipment.AddEquipment(thingWithComps);
					}
					else
					{
						Apparel apparel = thingWithComps as Apparel;
						if (apparel != null)
						{
							pawn.apparel.Wear(apparel, true, false);
						}
						else
						{
							pawn.inventory.innerContainer.TryAdd(thingWithComps, true);
						}
					}
				}
			}
			base.SendStandardLetter(this.def.letterLabel, this.def.letterText, LetterDefOf.PositiveEvent, parms, startingAndOptionalPawns, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06007042 RID: 28738 RVA: 0x00274E54 File Offset: 0x00273054
		public virtual bool CanSpawnJoiner(Map map)
		{
			IntVec3 intVec;
			return this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x06007043 RID: 28739 RVA: 0x00274E6C File Offset: 0x0027306C
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}
	}
}
