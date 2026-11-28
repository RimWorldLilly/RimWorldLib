using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002FF RID: 767
	public class RegionMaker
	{
		// Token: 0x06001767 RID: 5991 RVA: 0x0008CAFC File Offset: 0x0008ACFC
		public RegionMaker(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0008CB50 File Offset: 0x0008AD50
		public Region TryGenerateRegionFrom(IntVec3 root)
		{
			RegionType expectedRegionType = root.GetExpectedRegionType(this.map);
			if (expectedRegionType == RegionType.None)
			{
				return null;
			}
			if (this.working)
			{
				Log.Error("Trying to generate a new region but we are currently generating one. Nested calls are not allowed.");
				return null;
			}
			this.working = true;
			Region result;
			try
			{
				this.regionGrid = this.map.regionGrid;
				this.newReg = Region.MakeNewUnfilled(root, this.map);
				this.newReg.type = expectedRegionType;
				if (this.newReg.type == RegionType.Portal)
				{
					this.newReg.door = root.GetDoor(this.map);
				}
				this.FloodFillAndAddCells(root);
				this.CreateLinks();
				this.RegisterThingsInRegionListers();
				result = this.newReg;
			}
			finally
			{
				this.working = false;
			}
			return result;
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x0008CC14 File Offset: 0x0008AE14
		private void FloodFillAndAddCells(IntVec3 root)
		{
			this.newRegCells.Clear();
			if (this.newReg.type.IsOneCellRegion())
			{
				this.AddCell(root);
				return;
			}
			this.map.floodFiller.FloodFill(root, (IntVec3 x) => this.newReg.extentsLimit.Contains(x) && x.GetExpectedRegionType(this.map) == this.newReg.type, new Action<IntVec3>(this.AddCell), int.MaxValue, false, null);
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0008CC78 File Offset: 0x0008AE78
		private void AddCell(IntVec3 c)
		{
			this.regionGrid.SetRegionAt(c, this.newReg);
			this.newRegCells.Add(c);
			if (this.newReg.extentsClose.minX > c.x)
			{
				this.newReg.extentsClose.minX = c.x;
			}
			if (this.newReg.extentsClose.maxX < c.x)
			{
				this.newReg.extentsClose.maxX = c.x;
			}
			if (this.newReg.extentsClose.minZ > c.z)
			{
				this.newReg.extentsClose.minZ = c.z;
			}
			if (this.newReg.extentsClose.maxZ < c.z)
			{
				this.newReg.extentsClose.maxZ = c.z;
			}
			if (c.x == 0 || c.x == this.map.Size.x - 1 || c.z == 0 || c.z == this.map.Size.z - 1)
			{
				this.newReg.touchesMapEdge = true;
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x0008CDAC File Offset: 0x0008AFAC
		private void CreateLinks()
		{
			for (int i = 0; i < this.linksProcessedAt.Length; i++)
			{
				this.linksProcessedAt[i].Clear();
			}
			for (int j = 0; j < this.newRegCells.Count; j++)
			{
				IntVec3 c = this.newRegCells[j];
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.North, c);
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.South, c);
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.East, c);
				this.SweepInTwoDirectionsAndTryToCreateLink(Rot4.West, c);
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0008CE2C File Offset: 0x0008B02C
		private void SweepInTwoDirectionsAndTryToCreateLink(Rot4 potentialOtherRegionDir, IntVec3 c)
		{
			if (!potentialOtherRegionDir.IsValid)
			{
				return;
			}
			HashSet<IntVec3> hashSet = this.linksProcessedAt[potentialOtherRegionDir.AsInt];
			if (hashSet.Contains(c))
			{
				return;
			}
			IntVec3 c2 = c + potentialOtherRegionDir.FacingCell;
			if (c2.InBounds(this.map) && this.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c2) == this.newReg)
			{
				return;
			}
			RegionType expectedRegionType = c2.GetExpectedRegionType(this.map);
			if (expectedRegionType == RegionType.None)
			{
				return;
			}
			Rot4 rot = potentialOtherRegionDir;
			rot.Rotate(RotationDirection.Clockwise);
			int num = 0;
			int num2 = 0;
			hashSet.Add(c);
			if (!expectedRegionType.IsOneCellRegion())
			{
				for (;;)
				{
					IntVec3 intVec = c + rot.FacingCell * (num + 1);
					if (!intVec.InBounds(this.map) || this.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec) != this.newReg || (intVec + potentialOtherRegionDir.FacingCell).GetExpectedRegionType(this.map) != expectedRegionType)
					{
						break;
					}
					if (!hashSet.Add(intVec))
					{
						Log.Error("We've processed the same cell twice.");
					}
					num++;
				}
				for (;;)
				{
					IntVec3 intVec2 = c - rot.FacingCell * (num2 + 1);
					if (!intVec2.InBounds(this.map) || this.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec2) != this.newReg || (intVec2 + potentialOtherRegionDir.FacingCell).GetExpectedRegionType(this.map) != expectedRegionType)
					{
						break;
					}
					if (!hashSet.Add(intVec2))
					{
						Log.Error("We've processed the same cell twice.");
					}
					num2++;
				}
			}
			int length = num + num2 + 1;
			SpanDirection dir;
			IntVec3 root;
			if (potentialOtherRegionDir == Rot4.North)
			{
				dir = SpanDirection.East;
				root = c - rot.FacingCell * num2;
				root.z++;
			}
			else if (potentialOtherRegionDir == Rot4.South)
			{
				dir = SpanDirection.East;
				root = c + rot.FacingCell * num;
			}
			else if (potentialOtherRegionDir == Rot4.East)
			{
				dir = SpanDirection.North;
				root = c + rot.FacingCell * num;
				root.x++;
			}
			else
			{
				dir = SpanDirection.North;
				root = c - rot.FacingCell * num2;
			}
			EdgeSpan span = new EdgeSpan(root, dir, length);
			RegionLink regionLink = this.map.regionLinkDatabase.LinkFrom(span);
			regionLink.Register(this.newReg);
			this.newReg.links.Add(regionLink);
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0008D09C File Offset: 0x0008B29C
		private void RegisterThingsInRegionListers()
		{
			CellRect cellRect = this.newReg.extentsClose;
			cellRect = cellRect.ExpandedBy(1);
			cellRect.ClipInsideMap(this.map);
			RegionMaker.tmpProcessedThings.Clear();
			foreach (IntVec3 intVec in cellRect)
			{
				bool flag = false;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = intVec + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.map) && this.regionGrid.GetValidRegionAt(c) == this.newReg)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					RegionListersUpdater.RegisterAllAt(intVec, this.map, RegionMaker.tmpProcessedThings);
				}
			}
			RegionMaker.tmpProcessedThings.Clear();
		}

		// Token: 0x040013DF RID: 5087
		private Map map;

		// Token: 0x040013E0 RID: 5088
		private Region newReg;

		// Token: 0x040013E1 RID: 5089
		private List<IntVec3> newRegCells = new List<IntVec3>();

		// Token: 0x040013E2 RID: 5090
		private bool working;

		// Token: 0x040013E3 RID: 5091
		private HashSet<IntVec3>[] linksProcessedAt = new HashSet<IntVec3>[]
		{
			new HashSet<IntVec3>(),
			new HashSet<IntVec3>(),
			new HashSet<IntVec3>(),
			new HashSet<IntVec3>()
		};

		// Token: 0x040013E4 RID: 5092
		private RegionGrid regionGrid;

		// Token: 0x040013E5 RID: 5093
		private static HashSet<Thing> tmpProcessedThings = new HashSet<Thing>();
	}
}
