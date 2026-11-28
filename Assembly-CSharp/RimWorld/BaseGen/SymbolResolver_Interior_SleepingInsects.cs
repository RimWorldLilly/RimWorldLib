using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
	// Token: 0x02002149 RID: 8521
	public class SymbolResolver_Interior_SleepingInsects : SymbolResolver
	{
		// Token: 0x0600D520 RID: 54560 RVA: 0x004F00FE File Offset: 0x004EE2FE
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.threatPoints != null;
		}

		// Token: 0x0600D521 RID: 54561 RVA: 0x004F0118 File Offset: 0x004EE318
		public override void Resolve(ResolveParams rp)
		{
			Mathf.Min(rp.rect.Width, rp.rect.Height);
			LordJob_SleepThenAssaultColony lordJob_SleepThenAssaultColony = new LordJob_SleepThenAssaultColony(Faction.OfInsects, true);
			lordJob_SleepThenAssaultColony.awakeOnClamor = true;
			Lord lord = LordMaker.MakeNewLord(Faction.OfInsects, lordJob_SleepThenAssaultColony, BaseGen.globalSettings.map, null);
			foreach (PawnKindDef singlePawnKindDef in PawnUtility.GetCombatPawnKindsForPoints((PawnKindDef k) => k.RaceProps.Insect, rp.threatPoints.Value, null))
			{
				ResolveParams resolveParams = rp;
				resolveParams.faction = Faction.OfInsects;
				resolveParams.singlePawnKindDef = singlePawnKindDef;
				resolveParams.singlePawnLord = lord;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
			SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
			signalAction_DormancyWakeUp.signalTag = rp.sleepingInsectsWakeupSignalTag;
			signalAction_DormancyWakeUp.lord = lord;
			GenSpawn.Spawn(signalAction_DormancyWakeUp, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
