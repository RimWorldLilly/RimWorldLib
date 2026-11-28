using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001973 RID: 6515
	public class CompShipLandingBeacon : ThingComp
	{
		// Token: 0x17001C5F RID: 7263
		// (get) Token: 0x0600A38A RID: 41866 RVA: 0x003A5D44 File Offset: 0x003A3F44
		public CompProperties_ShipLandingBeacon Props
		{
			get
			{
				return (CompProperties_ShipLandingBeacon)this.props;
			}
		}

		// Token: 0x17001C60 RID: 7264
		// (get) Token: 0x0600A38B RID: 41867 RVA: 0x003A5D51 File Offset: 0x003A3F51
		public List<ShipLandingArea> LandingAreas
		{
			get
			{
				return this.landingAreas;
			}
		}

		// Token: 0x17001C61 RID: 7265
		// (get) Token: 0x0600A38C RID: 41868 RVA: 0x003A5D5C File Offset: 0x003A3F5C
		public bool Active
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp == null || comp.PowerOn;
			}
		}

		// Token: 0x0600A38D RID: 41869 RVA: 0x003A5D80 File Offset: 0x003A3F80
		private bool CanLinkTo(CompShipLandingBeacon other)
		{
			return other != this && ShipLandingBeaconUtility.CanLinkTo(this.parent.Position, other);
		}

		// Token: 0x0600A38E RID: 41870 RVA: 0x003A5D9C File Offset: 0x003A3F9C
		public void EstablishConnections()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			List<CompShipLandingBeacon> list = new List<CompShipLandingBeacon>();
			List<CompShipLandingBeacon> list2 = new List<CompShipLandingBeacon>();
			List<Thing> list3 = this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon);
			foreach (Thing thing in list3)
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null && this.CanLinkTo(compShipLandingBeacon))
				{
					if (this.parent.Position.x == compShipLandingBeacon.parent.Position.x)
					{
						list2.Add(compShipLandingBeacon);
					}
					else if (this.parent.Position.z == compShipLandingBeacon.parent.Position.z)
					{
						list.Add(compShipLandingBeacon);
					}
				}
			}
			using (List<CompShipLandingBeacon>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CompShipLandingBeacon h = enumerator2.Current;
					using (List<CompShipLandingBeacon>.Enumerator enumerator3 = list2.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							CompShipLandingBeacon v = enumerator3.Current;
							Thing thing2 = list3.FirstOrDefault((Thing x) => x.Position.x == h.parent.Position.x && x.Position.z == v.parent.Position.z);
							if (thing2 != null)
							{
								this.TryAddArea(new ShipLandingArea(CellRect.FromLimits(thing2.Position, this.parent.Position).ContractedBy(1), this.parent.Map)
								{
									beacons = new List<CompShipLandingBeacon>
									{
										this,
										thing2.TryGetComp<CompShipLandingBeacon>(),
										v,
										h
									}
								});
							}
						}
					}
				}
			}
			for (int i = this.landingAreas.Count - 1; i >= 0; i--)
			{
				using (List<CompShipLandingBeacon>.Enumerator enumerator2 = this.landingAreas[i].beacons.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (!enumerator2.Current.TryAddArea(this.landingAreas[i]))
						{
							this.RemoveArea(this.landingAreas[i]);
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600A38F RID: 41871 RVA: 0x003A6074 File Offset: 0x003A4274
		private void RemoveArea(ShipLandingArea area)
		{
			foreach (CompShipLandingBeacon compShipLandingBeacon in area.beacons)
			{
				if (compShipLandingBeacon.landingAreas.Contains(area))
				{
					compShipLandingBeacon.landingAreas.Remove(area);
				}
			}
			this.landingAreas.Remove(area);
		}

		// Token: 0x0600A390 RID: 41872 RVA: 0x003A60E8 File Offset: 0x003A42E8
		public bool TryAddArea(ShipLandingArea newArea)
		{
			if (!this.landingAreas.Contains(newArea))
			{
				for (int i = this.landingAreas.Count - 1; i >= 0; i--)
				{
					if (this.landingAreas[i].MyRect.Overlaps(newArea.MyRect) && this.landingAreas[i].MyRect != newArea.MyRect)
					{
						if (this.landingAreas[i].MyRect.Area <= newArea.MyRect.Area)
						{
							return false;
						}
						this.RemoveArea(this.landingAreas[i]);
					}
				}
				this.landingAreas.Add(newArea);
			}
			return true;
		}

		// Token: 0x0600A391 RID: 41873 RVA: 0x003A61B0 File Offset: 0x003A43B0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
			if (compGlower != null)
			{
				this.fieldColor = compGlower.GlowColor.ToColor.ToOpaque();
			}
			this.EstablishConnections();
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				shipLandingArea.RecalculateBlockingThing();
			}
		}

		// Token: 0x0600A392 RID: 41874 RVA: 0x003A6230 File Offset: 0x003A4430
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			for (int i = this.landingAreas.Count - 1; i >= 0; i--)
			{
				this.RemoveArea(this.landingAreas[i]);
			}
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null)
				{
					compShipLandingBeacon.EstablishConnections();
				}
			}
		}

		// Token: 0x0600A393 RID: 41875 RVA: 0x003A62C0 File Offset: 0x003A44C0
		public override void CompTickRare()
		{
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				shipLandingArea.RecalculateBlockingThing();
			}
		}

		// Token: 0x0600A394 RID: 41876 RVA: 0x003A6310 File Offset: 0x003A4510
		public override void PostDrawExtraSelectionOverlays()
		{
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				if (shipLandingArea.Active)
				{
					Color color = shipLandingArea.Clear ? this.fieldColor : Color.red;
					color.a = Pulser.PulseBrightness(1f, 0.6f);
					GenDraw.DrawFieldEdges(shipLandingArea.MyRect.ToList<IntVec3>(), color, null, null, 2900);
				}
				foreach (CompShipLandingBeacon compShipLandingBeacon in shipLandingArea.beacons)
				{
					if (this.CanLinkTo(compShipLandingBeacon))
					{
						GenDraw.DrawLineBetween(this.parent.TrueCenter(), compShipLandingBeacon.parent.TrueCenter(), SimpleColor.White, 0.2f);
					}
				}
			}
		}

		// Token: 0x0600A395 RID: 41877 RVA: 0x003A6424 File Offset: 0x003A4624
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			string text = "";
			if (!this.Active)
			{
				text += "NotUsable".Translate() + ": " + "Unpowered".Translate().CapitalizeFirst();
			}
			int i = 0;
			while (i < this.landingAreas.Count)
			{
				if (!this.landingAreas[i].Clear)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "NotUsable".Translate() + ": ";
					if (this.landingAreas[i].BlockedByRoof)
					{
						text += "BlockedByRoof".Translate().CapitalizeFirst();
						break;
					}
					if (this.landingAreas[i].BlockedByTerrainAffordance)
					{
						text += "BlockedByTerrain".Translate(this.Props.landingAreaTerrainSupport);
						break;
					}
					text += "BlockedBy".Translate(this.landingAreas[i].FirstBlockingThing).CapitalizeFirst();
					break;
				}
				else
				{
					i++;
				}
			}
			foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				if (thing != this.parent && ShipLandingBeaconUtility.AlignedDistanceTooShort(this.parent.Position, thing.Position, this.Props.edgeLengthRange.min - 1f))
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "NotUsable".Translate() + ": " + "TooCloseToOtherBeacon".Translate().CapitalizeFirst();
					break;
				}
			}
			return text;
		}

		// Token: 0x04006094 RID: 24724
		private List<ShipLandingArea> landingAreas = new List<ShipLandingArea>();

		// Token: 0x04006095 RID: 24725
		private Color fieldColor = Color.white;
	}
}
