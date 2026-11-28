using System;

namespace Verse
{
	// Token: 0x02000598 RID: 1432
	public class Projectile_SpawnsThing : Projectile
	{
		// Token: 0x06002A41 RID: 10817 RVA: 0x00102704 File Offset: 0x00100904
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Map map = base.Map;
			base.Impact(hitThing, blockedByShield);
			IntVec3 loc = base.Position;
			if (this.def.projectile.tryAdjacentFreeSpaces && base.Position.GetFirstBuilding(map) != null)
			{
				foreach (IntVec3 intVec in GenAdjFast.AdjacentCells8Way(base.Position))
				{
					if (intVec.GetFirstBuilding(map) == null && intVec.Standable(map))
					{
						loc = intVec;
						break;
					}
				}
			}
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.def.projectile.spawnsThingDef, null), loc, map, WipeMode.Vanish);
			if (thing.def.CanHaveFaction)
			{
				thing.SetFaction(base.Launcher.Faction, null);
			}
		}
	}
}
