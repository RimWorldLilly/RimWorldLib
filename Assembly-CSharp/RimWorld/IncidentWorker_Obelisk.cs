using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A6 RID: 4518
	public abstract class IncidentWorker_Obelisk : IncidentWorker
	{
		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x060070F3 RID: 28915
		public abstract ThingDef ObeliskDef { get; }

		// Token: 0x060070F4 RID: 28916 RVA: 0x0027866C File Offset: 0x0027686C
		public override float ChanceFactorNow(IIncidentTarget target)
		{
			Map map = target as Map;
			if (map == null)
			{
				return base.ChanceFactorNow(target);
			}
			int num = map.listerBuildings.allBuildingsNonColonist.Count((Building b) => b.def.GetCompProperties<CompProperties_Obelisk>() != null);
			return ((num > 0) ? ((float)num * 0.7f) : 1f) * base.ChanceFactorNow(target);
		}

		// Token: 0x060070F5 RID: 28917 RVA: 0x002786D8 File Offset: 0x002768D8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return this.TryFindCell(out intVec, map, this.ObeliskDef);
		}

		// Token: 0x060070F6 RID: 28918 RVA: 0x00278700 File Offset: 0x00276900
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Skyfaller skyfaller = this.SpawnObeliskIncoming(map);
			if (skyfaller == null)
			{
				return false;
			}
			skyfaller.impactLetter = LetterMaker.MakeLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef ?? LetterDefOf.NeutralEvent, new TargetInfo(skyfaller.Position, map, false), null, null, null);
			return true;
		}

		// Token: 0x060070F7 RID: 28919 RVA: 0x0027877C File Offset: 0x0027697C
		private Skyfaller SpawnObeliskIncoming(Map map)
		{
			IntVec3 pos;
			if (!this.TryFindCell(out pos, map, this.ObeliskDef))
			{
				return null;
			}
			return SkyfallerMaker.SpawnSkyfaller(ThingDefOf.WarpedObeliskIncoming, ThingMaker.MakeThing(this.ObeliskDef, null), pos, map);
		}

		// Token: 0x060070F8 RID: 28920 RVA: 0x002787B4 File Offset: 0x002769B4
		private bool TryFindCell(out IntVec3 cell, Map map, ThingDef obeliskDef)
		{
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.WarpedObeliskIncoming, map, this.ObeliskDef.terrainAffordanceNeeded, out cell, 10, default(IntVec3), -1, true, false, false, false, true, true, delegate(IntVec3 x)
			{
				if ((float)x.DistanceToEdge(map) < 20f + (float)map.Size.x * 0.1f)
				{
					return false;
				}
				foreach (IntVec3 c in CellRect.CenteredOn(x, obeliskDef.Size.x, obeliskDef.Size.z))
				{
					if (!c.InBounds(map) || !c.Standable(map) || !c.GetAffordances(map).Contains(obeliskDef.terrainAffordanceNeeded))
					{
						return false;
					}
				}
				return true;
			});
		}
	}
}
