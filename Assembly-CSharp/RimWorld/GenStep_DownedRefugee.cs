using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012EE RID: 4846
	public class GenStep_DownedRefugee : GenStep_Scatterer
	{
		// Token: 0x17001491 RID: 5265
		// (get) Token: 0x06007844 RID: 30788 RVA: 0x002A843C File Offset: 0x002A663C
		public override int SeedPart
		{
			get
			{
				return 931842770;
			}
		}

		// Token: 0x06007845 RID: 30789 RVA: 0x002A8443 File Offset: 0x002A6643
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return base.CanScatterAt(c, map) && c.Standable(map) && !c.Fogged(map);
		}

		// Token: 0x06007846 RID: 30790 RVA: 0x002A8464 File Offset: 0x002A6664
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Pawn pawn;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				pawn = (Pawn)parms.sitePart.things.Take(parms.sitePart.things[0]);
			}
			else
			{
				DownedRefugeeComp component = map.Parent.GetComponent<DownedRefugeeComp>();
				if (component != null && component.pawn.Any)
				{
					pawn = component.pawn.Take(component.pawn[0]);
				}
				else
				{
					pawn = DownedRefugeeQuestUtility.GenerateRefugee(map.Tile, null, 0.6f);
				}
			}
			HealthUtility.DamageUntilDowned(pawn, false, null, null, null);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			pawn.mindState.WillJoinColonyIfRescued = true;
			MapGenerator.SetVar<CellRect>("RectOfInterest", CellRect.CenteredOn(loc, 1, 1));
		}
	}
}
