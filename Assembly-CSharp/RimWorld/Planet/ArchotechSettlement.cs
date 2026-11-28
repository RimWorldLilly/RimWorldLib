using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023B5 RID: 9141
	public class ArchotechSettlement : Settlement
	{
		// Token: 0x170027FF RID: 10239
		// (get) Token: 0x0600E3AB RID: 58283 RVA: 0x0054A741 File Offset: 0x00548941
		private IEnumerable<Building> AllStudiableArchotechBuildings
		{
			get
			{
				ArchotechSettlement.<get_AllStudiableArchotechBuildings>d__2 <get_AllStudiableArchotechBuildings>d__ = new ArchotechSettlement.<get_AllStudiableArchotechBuildings>d__2(-2);
				<get_AllStudiableArchotechBuildings>d__.<>4__this = this;
				return <get_AllStudiableArchotechBuildings>d__;
			}
		}

		// Token: 0x17002800 RID: 10240
		// (get) Token: 0x0600E3AC RID: 58284 RVA: 0x0054A754 File Offset: 0x00548954
		public bool AnyArchotechBuildingRequiresStudy
		{
			get
			{
				foreach (Building thing in this.AllStudiableArchotechBuildings)
				{
					CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
					if (compStudiable != null && !compStudiable.Completed)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600E3AD RID: 58285 RVA: 0x0054A7B4 File Offset: 0x005489B4
		public override void Abandon(bool wasGravshipLaunch)
		{
			AbandonedArchotechStructures abandonedArchotechStructures = (AbandonedArchotechStructures)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedArchotechStructures);
			abandonedArchotechStructures.Tile = base.Tile;
			abandonedArchotechStructures.worldObjectDef = this.def;
			abandonedArchotechStructures.archotechStructures.AddRange(this.AllStudiableArchotechBuildings);
			foreach (Building building in abandonedArchotechStructures.archotechStructures)
			{
				building.DeSpawn(DestroyMode.Vanish);
			}
			Find.WorldObjects.Add(abandonedArchotechStructures);
			Find.LetterStack.ReceiveLetter("LetterLabelArchotechStructuresAbandoned".Translate(), "LetterTextArchotechStructuresAbandoned".Translate(this), LetterDefOf.NeutralEvent, abandonedArchotechStructures, null, null, null, null, 0, true);
			this.Destroy();
		}

		// Token: 0x0600E3AE RID: 58286 RVA: 0x0054A884 File Offset: 0x00548A84
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			foreach (Building building in this.existingBuildings)
			{
				Building building2 = base.Map.listerBuildings.AllBuildingsNonColonistOfDef(building.def).FirstOrDefault((Building b) => !this.existingBuildings.Contains(b));
				if (building2 != null)
				{
					IntVec3 position = building2.Position;
					building2.DeSpawn(DestroyMode.Vanish);
					GenSpawn.Spawn(building, position, base.Map, WipeMode.Vanish);
				}
				else
				{
					building.Destroy(DestroyMode.Vanish);
				}
			}
			this.existingBuildings.Clear();
		}

		// Token: 0x0600E3AF RID: 58287 RVA: 0x0054A934 File Offset: 0x00548B34
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Building>(ref this.existingBuildings, "existingBuildings", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04009114 RID: 37140
		public List<Building> existingBuildings = new List<Building>();
	}
}
