using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121B RID: 4635
	public class ComplexThreatWorker_SecurityCrate : ComplexThreatWorker
	{
		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x060072FE RID: 29438 RVA: 0x00283A74 File Offset: 0x00281C74
		public IEnumerable<ComplexThreatDef> SubThreats
		{
			get
			{
				return new ComplexThreatWorker_SecurityCrate.<get_SubThreats>d__1(-2);
			}
		}

		// Token: 0x060072FF RID: 29439 RVA: 0x00283A80 File Offset: 0x00281C80
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			IntVec3 intVec;
			return base.CanResolveInt(parms) && ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientSecurityCrate, parms.room, parms.map, out intVec, 1, null) && this.SubThreats.Any((ComplexThreatDef st) => st.Worker.CanResolve(parms));
		}

		// Token: 0x06007300 RID: 29440 RVA: 0x00283AF0 File Offset: 0x00281CF0
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			IntVec3 loc;
			ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientSecurityCrate, parms.room, parms.map, out loc, 1, null);
			Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingDefOf.AncientSecurityCrate, loc, parms.map, WipeMode.Vanish);
			ThingSetMakerParams parms2 = default(ThingSetMakerParams);
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientComplex_SecurityCrate.root.Generate(parms2);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (!building_Crate.TryAcceptThing(list[i], false))
				{
					list[i].Destroy(DestroyMode.Vanish);
				}
			}
			outSpawnedThings.Add(building_Crate);
			parms.spawnedThings.Add(building_Crate);
			if (building_Crate.openedSignal.NullOrEmpty())
			{
				building_Crate.openedSignal = "CrateOpened" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			}
			parms.triggerSignal = building_Crate.openedSignal;
			ComplexThreatDef complexThreatDef;
			if ((from st in this.SubThreats
			where st.Worker.CanResolve(parms)
			select st).TryRandomElement(out complexThreatDef))
			{
				float num = 0f;
				complexThreatDef.Worker.Resolve(parms, ref num, outSpawnedThings, null);
				threatPointsUsed += num;
				return;
			}
			Log.Warning("Failed to find a viable subthreat when placing a security crate threat.");
		}
	}
}
