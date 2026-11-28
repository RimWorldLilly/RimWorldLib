using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200210A RID: 8458
	public class SymbolResolver_PawnGroup : SymbolResolver
	{
		// Token: 0x0600D463 RID: 54371 RVA: 0x004E80B0 File Offset: 0x004E62B0
		public static PawnGroupMakerParms GetGroupMakerParms(Map map, ResolveParams rp)
		{
			PawnGroupMakerParms pawnGroupMakerParms;
			if ((pawnGroupMakerParms = rp.pawnGroupMakerParams) == null)
			{
				PawnGroupMakerParms pawnGroupMakerParms2 = new PawnGroupMakerParms();
				pawnGroupMakerParms2.tile = map.Tile;
				pawnGroupMakerParms2.faction = Find.FactionManager.RandomRaidableEnemyFaction(false, false, true, TechLevel.Undefined);
				pawnGroupMakerParms = pawnGroupMakerParms2;
				pawnGroupMakerParms2.points = 250f;
			}
			PawnGroupMakerParms pawnGroupMakerParms3 = pawnGroupMakerParms;
			pawnGroupMakerParms3.groupKind = (rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Combat);
			return pawnGroupMakerParms3;
		}

		// Token: 0x0600D464 RID: 54372 RVA: 0x004E810C File Offset: 0x004E630C
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			return rp.rect.Cells.Any((IntVec3 x) => x.Standable(BaseGen.globalSettings.map));
		}

		// Token: 0x0600D465 RID: 54373 RVA: 0x004E815C File Offset: 0x004E635C
		public override void Resolve(ResolveParams rp)
		{
			PawnGroupMakerParms groupMakerParms = SymbolResolver_PawnGroup.GetGroupMakerParms(BaseGen.globalSettings.map, rp);
			List<PawnKindDef> list = new List<PawnKindDef>();
			foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(groupMakerParms, true))
			{
				list.Add(pawn.kindDef);
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = pawn;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
			}
		}

		// Token: 0x040086D3 RID: 34515
		private const float DefaultPoints = 250f;
	}
}
