using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF9 RID: 7161
	public class Alert_ShuttleLandingBeaconUnusable : Alert
	{
		// Token: 0x17002118 RID: 8472
		// (get) Token: 0x0600B708 RID: 46856 RVA: 0x00407E90 File Offset: 0x00406090
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon);
					for (int j = 0; j < list.Count; j++)
					{
						CompShipLandingBeacon compShipLandingBeacon = list[j].TryGetComp<CompShipLandingBeacon>();
						if (compShipLandingBeacon != null && compShipLandingBeacon.Active && !compShipLandingBeacon.LandingAreas.Any<ShipLandingArea>())
						{
							this.targets.Add(list[j]);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0600B709 RID: 46857 RVA: 0x00407F30 File Offset: 0x00406130
		public Alert_ShuttleLandingBeaconUnusable()
		{
			this.defaultLabel = "ShipLandingBeaconUnusable".Translate();
			this.defaultExplanation = "ShipLandingBeaconUnusableDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0600B70A RID: 46858 RVA: 0x00407F7F File Offset: 0x0040617F
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04006901 RID: 26881
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
