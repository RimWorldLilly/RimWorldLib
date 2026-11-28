using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001B0E RID: 6926
	[StaticConstructorOnStartup]
	public class TunnelHiveSpawner : GroundSpawner
	{
		// Token: 0x0600B111 RID: 45329 RVA: 0x003E6474 File Offset: 0x003E4674
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.spawnHive, "spawnHive", true, false);
			Scribe_Values.Look<float>(ref this.insectsPoints, "insectsPoints", 0f, false);
			Scribe_Values.Look<bool>(ref this.spawnedByInfestationThingComp, "spawnedByInfestationThingComp", false, false);
		}

		// Token: 0x0600B112 RID: 45330 RVA: 0x003E64C4 File Offset: 0x003E46C4
		protected override void Spawn(Map map, IntVec3 loc)
		{
			if (this.spawnHive)
			{
				HiveUtility.SpawnHive(loc, map, WipeMode.FullRefund, false, true, true, false, true, true, false).questTags = this.questTags;
			}
			if (this.insectsPoints > 0f)
			{
				this.insectsPoints = Mathf.Max(this.insectsPoints, Hive.spawnablePawnKinds.Min((PawnKindDef x) => x.combatPower));
				float pointsLeft = this.insectsPoints;
				List<Pawn> list = new List<Pawn>();
				int num = 0;
				Func<PawnKindDef, bool> <>9__1;
				while (pointsLeft > 0f)
				{
					num++;
					if (num > 1000)
					{
						Log.Error("Too many iterations.");
						break;
					}
					IEnumerable<PawnKindDef> spawnablePawnKinds = Hive.spawnablePawnKinds;
					Func<PawnKindDef, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((PawnKindDef x) => x.combatPower <= pointsLeft));
					}
					PawnKindDef pawnKindDef;
					if (!spawnablePawnKinds.Where(predicate).TryRandomElement(out pawnKindDef))
					{
						break;
					}
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, Faction.OfInsects, null);
					GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(loc, map, 2, null), map, WipeMode.Vanish);
					pawn.mindState.spawnedByInfestationThingComp = this.spawnedByInfestationThingComp;
					list.Add(pawn);
					pointsLeft -= pawnKindDef.combatPower;
					if (ModsConfig.BiotechActive)
					{
						PollutionUtility.Notify_TunnelHiveSpawnedInsect(pawn);
					}
				}
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfInsects, new LordJob_AssaultColony(Faction.OfInsects, true, false, false, false, true, false, false), map, list);
				}
			}
		}

		// Token: 0x04006654 RID: 26196
		public bool spawnHive = true;

		// Token: 0x04006655 RID: 26197
		public float insectsPoints;

		// Token: 0x04006656 RID: 26198
		public bool spawnedByInfestationThingComp;
	}
}
