using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B57 RID: 6999
	public static class SkyfallerMaker
	{
		// Token: 0x0600B3C4 RID: 46020 RVA: 0x003F6167 File Offset: 0x003F4367
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller)
		{
			return (Skyfaller)ThingMaker.MakeThing(skyfaller, null);
		}

		// Token: 0x0600B3C5 RID: 46021 RVA: 0x003F6178 File Offset: 0x003F4378
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, ThingDef innerThing)
		{
			Thing innerThing2 = ThingMaker.MakeThing(innerThing, null);
			return SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing2);
		}

		// Token: 0x0600B3C6 RID: 46022 RVA: 0x003F6194 File Offset: 0x003F4394
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, Thing innerThing)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (innerThing != null && !skyfaller2.innerContainer.TryAdd(innerThing, true))
			{
				Log.Error("Could not add " + innerThing.ToStringSafe<Thing>() + " to a skyfaller.");
				innerThing.Destroy(DestroyMode.Vanish);
			}
			return skyfaller2;
		}

		// Token: 0x0600B3C7 RID: 46023 RVA: 0x003F61DC File Offset: 0x003F43DC
		public static Skyfaller MakeSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things)
		{
			Skyfaller skyfaller2 = SkyfallerMaker.MakeSkyfaller(skyfaller);
			if (things != null)
			{
				skyfaller2.innerContainer.TryAddRangeOrTransfer(things, false, true);
			}
			return skyfaller2;
		}

		// Token: 0x0600B3C8 RID: 46024 RVA: 0x003F6202 File Offset: 0x003F4402
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller), pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600B3C9 RID: 46025 RVA: 0x003F6217 File Offset: 0x003F4417
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, ThingDef innerThing, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing), pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600B3CA RID: 46026 RVA: 0x003F622D File Offset: 0x003F442D
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, Thing innerThing, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, innerThing), pos, map, WipeMode.Vanish);
		}

		// Token: 0x0600B3CB RID: 46027 RVA: 0x003F6243 File Offset: 0x003F4443
		public static Skyfaller SpawnSkyfaller(ThingDef skyfaller, IEnumerable<Thing> things, IntVec3 pos, Map map)
		{
			return (Skyfaller)GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(skyfaller, things), pos, map, WipeMode.Vanish);
		}
	}
}
