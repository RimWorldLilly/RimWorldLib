using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B52 RID: 6994
	public class LavaRockIncoming : Skyfaller
	{
		// Token: 0x0600B389 RID: 45961 RVA: 0x003F488C File Offset: 0x003F2A8C
		protected override void Impact()
		{
			int randomInRange = LavaRockIncoming.LavaRadiusRange.RandomInRange;
			IntVec3 position = base.Position;
			Map map = base.Map;
			base.Impact();
			LavaEmergenceImmediate lavaEmergenceImmediate = (LavaEmergenceImmediate)ThingMaker.MakeThing(ThingDefOf.LavaEmergenceImmediate, null);
			lavaEmergenceImmediate.forceCoolDelay = LavaRockIncoming.CoolTicksRange.RandomInRange;
			lavaEmergenceImmediate.forcePoolSize = randomInRange * randomInRange * 3;
			GenSpawn.Spawn(lavaEmergenceImmediate, position, map, WipeMode.Vanish);
		}

		// Token: 0x04006792 RID: 26514
		private static readonly IntRange LavaRadiusRange = new IntRange(3, 5);

		// Token: 0x04006793 RID: 26515
		private static readonly IntRange CoolTicksRange = new IntRange(48000, 72000);
	}
}
