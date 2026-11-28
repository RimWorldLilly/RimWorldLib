using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6C RID: 7020
	public class ThingSetMaker_Meteorite : ThingSetMaker
	{
		// Token: 0x0600B430 RID: 46128 RVA: 0x003F83D8 File Offset: 0x003F65D8
		public static void Reset()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Clear();
			ThingSetMaker_Meteorite.nonSmoothedMineables.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && !x.building.mineablePreventMeteorite && !x.IsSmoothed
			select x);
		}

		// Token: 0x0600B431 RID: 46129 RVA: 0x003F8418 File Offset: 0x003F6618
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int randomInRange = (parms.countRange ?? ThingSetMaker_Meteorite.MineablesCountRange).RandomInRange;
			ThingDef def = this.FindRandomMineableDef();
			for (int i = 0; i < randomInRange; i++)
			{
				Building building = (Building)ThingMaker.MakeThing(def, null);
				building.canChangeTerrainOnDestroyed = false;
				outThings.Add(building);
			}
		}

		// Token: 0x0600B432 RID: 46130 RVA: 0x003F8484 File Offset: 0x003F6684
		private ThingDef FindRandomMineableDef()
		{
			float value = Rand.Value;
			if (value < 0.4f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where !x.building.isResourceRock
				select x).RandomElement<ThingDef>();
			}
			if (value < 0.75f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue < 5f
				select x).RandomElement<ThingDef>();
			}
			return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
			where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue >= 5f
			select x).RandomElement<ThingDef>();
		}

		// Token: 0x0600B433 RID: 46131 RVA: 0x003F8533 File Offset: 0x003F6733
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_Meteorite.nonSmoothedMineables;
		}

		// Token: 0x040067D1 RID: 26577
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		// Token: 0x040067D2 RID: 26578
		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		// Token: 0x040067D3 RID: 26579
		private const float PreciousMineableMarketValue = 5f;
	}
}
