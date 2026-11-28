using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000597 RID: 1431
	public class Projectile_SpawnsPawnZeroAge : Projectile
	{
		// Token: 0x06002A3F RID: 10815 RVA: 0x001025A0 File Offset: 0x001007A0
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
			PawnKindDef spawnsPawnKind = this.def.projectile.spawnsPawnKind;
			Faction faction = base.Launcher.Faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			float? fixedChronologicalAge = new float?(0f);
			float? fixedBiologicalAge = new float?(0f);
			GenSpawn.Spawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(spawnsPawnKind, faction, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, fixedChronologicalAge, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), loc, map, WipeMode.Vanish);
		}
	}
}
