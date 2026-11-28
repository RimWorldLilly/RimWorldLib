using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002118 RID: 8472
	public class SymbolResolver_Ambush : SymbolResolver
	{
		// Token: 0x0600D48D RID: 54413 RVA: 0x004E94E0 File Offset: 0x004E76E0
		public override bool CanResolve(ResolveParams rp)
		{
			return !rp.ambushSignalTag.NullOrEmpty();
		}

		// Token: 0x0600D48E RID: 54414 RVA: 0x004E94F0 File Offset: 0x004E76F0
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			signalAction_Ambush.signalTag = rp.ambushSignalTag;
			signalAction_Ambush.points = (rp.ambushPoints ?? this.DefaultAmbushPoints);
			signalAction_Ambush.ambushType = rp.ambushType.GetValueOrDefault();
			signalAction_Ambush.spawnNear = (rp.spawnNear ?? IntVec3.Invalid);
			signalAction_Ambush.spawnAround = rp.spawnAround.GetValueOrDefault();
			signalAction_Ambush.spawnPawnsOnEdge = rp.spawnPawnsOnEdge.GetValueOrDefault();
			GenSpawn.Spawn(signalAction_Ambush, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x040086D8 RID: 34520
		private float DefaultAmbushPoints = 200f;
	}
}
