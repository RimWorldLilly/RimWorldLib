using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002110 RID: 8464
	public class SymbolResolver_SoundOneShotAction : SymbolResolver
	{
		// Token: 0x0600D47A RID: 54394 RVA: 0x004E8FE4 File Offset: 0x004E71E4
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_SoundOneShot signalAction_SoundOneShot = (SignalAction_SoundOneShot)ThingMaker.MakeThing(ThingDefOf.SignalAction_SoundOneShot, null);
			signalAction_SoundOneShot.signalTag = rp.soundOneShotActionSignalTag;
			signalAction_SoundOneShot.sound = rp.sound;
			GenSpawn.Spawn(signalAction_SoundOneShot, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x040086D5 RID: 34517
		private const string SoundSignalPrefix = "SoundTrigger";
	}
}
