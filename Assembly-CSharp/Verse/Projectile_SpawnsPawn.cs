using System;

namespace Verse
{
	// Token: 0x02000596 RID: 1430
	public class Projectile_SpawnsPawn : Projectile
	{
		// Token: 0x06002A3D RID: 10813 RVA: 0x001024C8 File Offset: 0x001006C8
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
			GenSpawn.Spawn(PawnGenerator.GeneratePawn(this.def.projectile.spawnsPawnKind, base.Launcher.Faction, null), loc, map, WipeMode.Vanish);
		}
	}
}
