using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001972 RID: 6514
	public class ShipLandingArea
	{
		// Token: 0x17001C58 RID: 7256
		// (get) Token: 0x0600A381 RID: 41857 RVA: 0x003A5B7A File Offset: 0x003A3D7A
		public IntVec3 CenterCell
		{
			get
			{
				return this.rect.CenterCell;
			}
		}

		// Token: 0x17001C59 RID: 7257
		// (get) Token: 0x0600A382 RID: 41858 RVA: 0x003A5B87 File Offset: 0x003A3D87
		public CellRect MyRect
		{
			get
			{
				return this.rect;
			}
		}

		// Token: 0x17001C5A RID: 7258
		// (get) Token: 0x0600A383 RID: 41859 RVA: 0x003A5B8F File Offset: 0x003A3D8F
		public bool Clear
		{
			get
			{
				return this.firstBlockingThing == null && !this.blockedByRoof && !this.blockedByTerrainAffordance;
			}
		}

		// Token: 0x17001C5B RID: 7259
		// (get) Token: 0x0600A384 RID: 41860 RVA: 0x003A5BAC File Offset: 0x003A3DAC
		public bool BlockedByRoof
		{
			get
			{
				return this.blockedByRoof;
			}
		}

		// Token: 0x17001C5C RID: 7260
		// (get) Token: 0x0600A385 RID: 41861 RVA: 0x003A5BB4 File Offset: 0x003A3DB4
		public bool BlockedByTerrainAffordance
		{
			get
			{
				return this.blockedByTerrainAffordance;
			}
		}

		// Token: 0x17001C5D RID: 7261
		// (get) Token: 0x0600A386 RID: 41862 RVA: 0x003A5BBC File Offset: 0x003A3DBC
		public Thing FirstBlockingThing
		{
			get
			{
				return this.firstBlockingThing;
			}
		}

		// Token: 0x17001C5E RID: 7262
		// (get) Token: 0x0600A387 RID: 41863 RVA: 0x003A5BC4 File Offset: 0x003A3DC4
		public bool Active
		{
			get
			{
				for (int i = 0; i < this.beacons.Count; i++)
				{
					if (!this.beacons[i].Active)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600A388 RID: 41864 RVA: 0x003A5BFD File Offset: 0x003A3DFD
		public ShipLandingArea(CellRect rect, Map map)
		{
			this.rect = rect;
			this.map = map;
		}

		// Token: 0x0600A389 RID: 41865 RVA: 0x003A5C20 File Offset: 0x003A3E20
		public void RecalculateBlockingThing()
		{
			this.blockedByRoof = false;
			foreach (IntVec3 c in this.rect)
			{
				if (c.Roofed(this.map))
				{
					this.blockedByRoof = true;
					break;
				}
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!(thingList[i] is Pawn) && (thingList[i].def.Fillage != FillCategory.None || thingList[i].def.IsEdifice() || thingList[i] is Skyfaller))
					{
						this.firstBlockingThing = thingList[i];
						return;
					}
				}
				TerrainAffordanceDef landingAreaTerrainSupport = this.beacons[0].Props.landingAreaTerrainSupport;
				if (landingAreaTerrainSupport != null && !c.SupportsStructureType(this.map, landingAreaTerrainSupport))
				{
					this.blockedByTerrainAffordance = true;
					return;
				}
			}
			this.firstBlockingThing = null;
		}

		// Token: 0x0400608E RID: 24718
		private CellRect rect;

		// Token: 0x0400608F RID: 24719
		private Map map;

		// Token: 0x04006090 RID: 24720
		private Thing firstBlockingThing;

		// Token: 0x04006091 RID: 24721
		private bool blockedByRoof;

		// Token: 0x04006092 RID: 24722
		private bool blockedByTerrainAffordance;

		// Token: 0x04006093 RID: 24723
		public List<CompShipLandingBeacon> beacons = new List<CompShipLandingBeacon>();
	}
}
