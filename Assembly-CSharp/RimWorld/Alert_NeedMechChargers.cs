using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C11 RID: 7185
	public class Alert_NeedMechChargers : Alert
	{
		// Token: 0x0600B772 RID: 46962 RVA: 0x0040A9DC File Offset: 0x00408BDC
		public Alert_NeedMechChargers()
		{
			this.defaultLabel = "AlertNeedMechChargers".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0600B773 RID: 46963 RVA: 0x0040AA37 File Offset: 0x00408C37
		public override AlertReport GetReport()
		{
			this.RecacheMechsAndChargers();
			return this.requiredChargerDefs.Count > 0;
		}

		// Token: 0x0600B774 RID: 46964 RVA: 0x0040AA54 File Offset: 0x00408C54
		private void RecacheMechsAndChargers()
		{
			this.chargers.Clear();
			this.mechs.Clear();
			List<Map> maps = Find.Maps;
			if (this.allChargerDefs == null)
			{
				this.allChargerDefs = new List<ThingDef>();
				this.allChargerDefs.AddRange(from t in DefDatabase<ThingDef>.AllDefs
				where ThingRequestGroup.MechCharger.Includes(t)
				select t);
			}
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.MechCharger);
				for (int j = 0; j < list.Count; j++)
				{
					Building_MechCharger building_MechCharger = (Building_MechCharger)list[j];
					if (building_MechCharger.Faction == Faction.OfPlayer)
					{
						this.chargers.Add(building_MechCharger);
					}
				}
			}
			for (int k = 0; k < maps.Count; k++)
			{
				List<Pawn> list2 = maps[k].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int l = 0; l < list2.Count; l++)
				{
					if (list2[l].IsColonyMech)
					{
						this.mechs.Add(list2[l]);
					}
				}
			}
			this.requiredChargerDefs.Clear();
			for (int m = 0; m < this.mechs.Count; m++)
			{
				Pawn mech = this.mechs[m];
				if (!this.chargers.Any((Building_MechCharger c) => c.IsCompatibleWithCharger(mech.kindDef)))
				{
					ThingDef thingDef = this.allChargerDefs.FirstOrDefault((ThingDef c) => Building_MechCharger.IsCompatibleWithCharger(c, mech.kindDef));
					if (thingDef != null && !this.requiredChargerDefs.Contains(thingDef))
					{
						this.requiredChargerDefs.Add(thingDef);
					}
				}
			}
		}

		// Token: 0x0600B775 RID: 46965 RVA: 0x0040AC24 File Offset: 0x00408E24
		public override TaggedString GetExplanation()
		{
			TaggedString taggedString = "AlertNeedMechChargersDesc".Translate((from c in this.requiredChargerDefs
			select c.LabelCap.Resolve()).ToLineList("  - ", false));
			if (!ResearchProjectDefOf.BasicMechtech.IsFinished)
			{
				taggedString += "\n\n" + "AlertNeedMechChargerBasicMechtech".Translate();
			}
			return taggedString;
		}

		// Token: 0x04006936 RID: 26934
		private List<Pawn> mechs = new List<Pawn>();

		// Token: 0x04006937 RID: 26935
		private List<Building_MechCharger> chargers = new List<Building_MechCharger>();

		// Token: 0x04006938 RID: 26936
		private List<ThingDef> allChargerDefs = new List<ThingDef>();

		// Token: 0x04006939 RID: 26937
		private List<ThingDef> requiredChargerDefs = new List<ThingDef>();
	}
}
