using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200230E RID: 8974
	public class WorldDebugDrawer
	{
		// Token: 0x170026BE RID: 9918
		// (get) Token: 0x0600DD9B RID: 56731 RVA: 0x005283ED File Offset: 0x005265ED
		private static float MaxDistToCameraToDisplayLabel
		{
			get
			{
				return WorldCameraDriver.MinAltitude - 100f + 14f;
			}
		}

		// Token: 0x0600DD9C RID: 56732 RVA: 0x00528400 File Offset: 0x00526600
		public void FlashTile(PlanetTile tile, float colorPct = 0f, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.colorPct = colorPct;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x0600DD9D RID: 56733 RVA: 0x0052843C File Offset: 0x0052663C
		public void FlashTile(PlanetTile tile, Material mat, string text = null, int duration = 50)
		{
			DebugTile debugTile = new DebugTile();
			debugTile.tile = tile;
			debugTile.displayString = text;
			debugTile.customMat = mat;
			debugTile.ticksLeft = duration;
			this.debugTiles.Add(debugTile);
		}

		// Token: 0x0600DD9E RID: 56734 RVA: 0x00528478 File Offset: 0x00526678
		public void FlashLine(Vector3 a, Vector3 b, bool onPlanetSurface = false, int duration = 50)
		{
			DebugWorldLine debugWorldLine = new DebugWorldLine(a, b, onPlanetSurface);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x0600DD9F RID: 56735 RVA: 0x005284A4 File Offset: 0x005266A4
		public void FlashLine(PlanetTile tileA, PlanetTile tileB, int duration = 50)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(tileA);
			Vector3 tileCenter2 = worldGrid.GetTileCenter(tileB);
			DebugWorldLine debugWorldLine = new DebugWorldLine(tileCenter, tileCenter2, true);
			debugWorldLine.TicksLeft = duration;
			this.debugLines.Add(debugWorldLine);
		}

		// Token: 0x0600DDA0 RID: 56736 RVA: 0x005284E4 File Offset: 0x005266E4
		public void WorldDebugDrawerUpdate()
		{
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				this.debugTiles[i].Draw();
			}
			for (int j = 0; j < this.debugLines.Count; j++)
			{
				this.debugLines[j].Draw();
			}
		}

		// Token: 0x0600DDA1 RID: 56737 RVA: 0x00528540 File Offset: 0x00526740
		public void WorldDebugDrawerTick()
		{
			for (int i = this.debugTiles.Count - 1; i >= 0; i--)
			{
				DebugTile debugTile = this.debugTiles[i];
				debugTile.ticksLeft--;
				if (debugTile.ticksLeft <= 0)
				{
					this.debugTiles.RemoveAt(i);
				}
			}
			for (int j = this.debugLines.Count - 1; j >= 0; j--)
			{
				DebugWorldLine debugWorldLine = this.debugLines[j];
				debugWorldLine.ticksLeft--;
				if (debugWorldLine.ticksLeft <= 0)
				{
					this.debugLines.RemoveAt(j);
				}
			}
		}

		// Token: 0x0600DDA2 RID: 56738 RVA: 0x005285DC File Offset: 0x005267DC
		public void WorldDebugDrawerOnGUI()
		{
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			for (int i = 0; i < this.debugTiles.Count; i++)
			{
				if (this.debugTiles[i].DistanceToCamera <= WorldDebugDrawer.MaxDistToCameraToDisplayLabel)
				{
					this.debugTiles[i].OnGUI();
				}
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x04008E54 RID: 36436
		private List<DebugTile> debugTiles = new List<DebugTile>();

		// Token: 0x04008E55 RID: 36437
		private List<DebugWorldLine> debugLines = new List<DebugWorldLine>();

		// Token: 0x04008E56 RID: 36438
		private const int DefaultLifespanTicks = 50;
	}
}
