using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011AA RID: 4522
	public class IncidentWorker_WastepackInfestation : IncidentWorker
	{
		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x06007100 RID: 28928 RVA: 0x00278844 File Offset: 0x00276A44
		public override float BaseChanceThisGame
		{
			get
			{
				return base.BaseChanceThisGame * Find.Storyteller.difficulty.wastepackInfestationChanceFactor;
			}
		}

		// Token: 0x06007101 RID: 28929 RVA: 0x0027885C File Offset: 0x00276A5C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			return base.CanFireNowSub(parms) && Faction.OfInsects != null && this.GetSpawnCenter(parms, map).IsValid && CocoonInfestationUtility.GetCocoonsToSpawn(parms.points).Any<ThingDef>();
		}

		// Token: 0x06007102 RID: 28930 RVA: 0x002788B4 File Offset: 0x00276AB4
		private IntVec3 GetSpawnCenter(IncidentParms parms, Map map)
		{
			if (CocoonInfestationUtility.CanSpawnCocoonAt(parms.spawnCenter, map))
			{
				return parms.spawnCenter;
			}
			IntVec3 result;
			if (RCellFinder.TryFindRandomCellNearWith(parms.spawnCenter, (IntVec3 c) => CocoonInfestationUtility.CanSpawnCocoonAt(c, map), map, out result, 5, 2147483647))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06007103 RID: 28931 RVA: 0x00278918 File Offset: 0x00276B18
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Thing> targets = CocoonInfestationUtility.SpawnCocoonInfestation(this.GetSpawnCenter(parms, map), (Map)parms.target, parms.points);
			base.SendStandardLetter(parms, new LookTargets(targets), Array.Empty<NamedArgument>());
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}
	}
}
