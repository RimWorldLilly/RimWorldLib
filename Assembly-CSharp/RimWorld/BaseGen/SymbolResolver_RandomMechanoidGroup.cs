using System;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x02002134 RID: 8500
	public class SymbolResolver_RandomMechanoidGroup : SymbolResolver
	{
		// Token: 0x0600D4E2 RID: 54498 RVA: 0x004ED540 File Offset: 0x004EB740
		public override void Resolve(ResolveParams rp)
		{
			int num = rp.mechanoidsCount ?? SymbolResolver_RandomMechanoidGroup.DefaultMechanoidCountRange.RandomInRange;
			Lord lord = rp.singlePawnLord;
			if (lord == null && num > 0)
			{
				Map map = BaseGen.globalSettings.map;
				LordJob lordJob;
				IntVec3 point;
				if (rp.disableSinglePawn.GetValueOrDefault())
				{
					lordJob = new LordJob_SleepThenAssaultColony(Faction.OfMechanoids, rp.sendWokenUpMessage.GetValueOrDefault(true))
					{
						wakeOnPawnUnfogged = rp.wakeSleepingMechanoidsOnUnfogged.GetValueOrDefault()
					};
				}
				else if (Rand.Bool && (from x in rp.rect.Cells
				where !x.Impassable(map)
				select x).TryRandomElement(out point))
				{
					lordJob = new LordJob_DefendPoint(point, null, null, false, true);
				}
				else
				{
					lordJob = new LordJob_AssaultColony(Faction.OfMechanoids, false, false, false, false, false, false, false);
				}
				lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob, map, null);
			}
			for (int i = 0; i < num; i++)
			{
				PawnKindDef pawnKindDef = rp.singlePawnKindDef;
				if (pawnKindDef == null)
				{
					pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(new Func<PawnKindDef, bool>(MechClusterGenerator.MechKindSuitableForCluster)).RandomElementByWeight((PawnKindDef kind) => 1f / kind.combatPower);
				}
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnKindDef = pawnKindDef;
				resolveParams.singlePawnLord = lord;
				resolveParams.faction = Faction.OfMechanoids;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
		}

		// Token: 0x04008700 RID: 34560
		private static readonly IntRange DefaultMechanoidCountRange = new IntRange(1, 5);
	}
}
