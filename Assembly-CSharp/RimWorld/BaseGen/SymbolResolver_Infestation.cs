using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200212B RID: 8491
	public class SymbolResolver_Infestation : SymbolResolver
	{
		// Token: 0x0600D4C5 RID: 54469 RVA: 0x004EC651 File Offset: 0x004EA851
		public override bool CanResolve(ResolveParams rp)
		{
			return Faction.OfInsects != null && !rp.infestationSignalTag.NullOrEmpty();
		}

		// Token: 0x0600D4C6 RID: 54470 RVA: 0x004EC66C File Offset: 0x004EA86C
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_Infestation signalAction_Infestation = (SignalAction_Infestation)ThingMaker.MakeThing(ThingDefOf.SignalAction_Infestation, null);
			signalAction_Infestation.signalTag = rp.infestationSignalTag;
			signalAction_Infestation.hivesCount = rp.hivesCount.GetValueOrDefault(1);
			signalAction_Infestation.insectsPoints = rp.insectsPoints;
			signalAction_Infestation.spawnAnywhereIfNoGoodCell = rp.spawnAnywhereIfNoGoodCell.GetValueOrDefault();
			signalAction_Infestation.ignoreRoofedRequirement = rp.ignoreRoofedRequirement.GetValueOrDefault();
			signalAction_Infestation.overrideLoc = rp.overrideLoc;
			signalAction_Infestation.sendStandardLetter = rp.sendStandardLetter.GetValueOrDefault(true);
			GenSpawn.Spawn(signalAction_Infestation, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
