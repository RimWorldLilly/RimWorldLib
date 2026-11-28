using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001360 RID: 4960
	public class CompPowerPlantSteam : CompPowerPlant
	{
		// Token: 0x06007ADA RID: 31450 RVA: 0x002BBB37 File Offset: 0x002B9D37
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this.parent);
		}

		// Token: 0x06007ADB RID: 31451 RVA: 0x002BBB54 File Offset: 0x002B9D54
		public override void CompTick()
		{
			base.CompTick();
			if (this.geyser == null)
			{
				this.geyser = (Building_SteamGeyser)this.parent.Map.thingGrid.ThingAt(this.parent.Position, ThingDefOf.SteamGeyser);
			}
			if (this.geyser != null)
			{
				this.geyser.harvester = (Building)this.parent;
				this.steamSprayer.SteamSprayerTick();
			}
		}

		// Token: 0x06007ADC RID: 31452 RVA: 0x002BBBC8 File Offset: 0x002B9DC8
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.geyser != null)
			{
				this.geyser.harvester = null;
			}
		}

		// Token: 0x04004D89 RID: 19849
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04004D8A RID: 19850
		private Building_SteamGeyser geyser;
	}
}
