using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024A RID: 586
	public sealed class DebugCellDrawer
	{
		// Token: 0x0600113F RID: 4415 RVA: 0x00062FC4 File Offset: 0x000611C4
		public void FlashCell(IntVec3 c, float colorPct = 0f, string text = null, int duration = 50)
		{
			DebugCell debugCell = new DebugCell();
			debugCell.c = c;
			debugCell.displayString = text;
			debugCell.colorPct = colorPct;
			debugCell.ticksLeft = duration;
			this.debugCells.Add(debugCell);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00063000 File Offset: 0x00061200
		public void FlashCell(IntVec3 c, Material mat, string text = null, int duration = 50)
		{
			DebugCell debugCell = new DebugCell();
			debugCell.c = c;
			debugCell.displayString = text;
			debugCell.customMat = mat;
			debugCell.ticksLeft = duration;
			this.debugCells.Add(debugCell);
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0006303C File Offset: 0x0006123C
		public void FlashLine(IntVec3 a, IntVec3 b, int duration = 50, SimpleColor color = SimpleColor.White)
		{
			this.debugLines.Add(new DebugLine(a.ToVector3Shifted(), b.ToVector3Shifted(), duration, color));
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00063060 File Offset: 0x00061260
		public void DebugDrawerUpdate()
		{
			for (int i = 0; i < this.debugCells.Count; i++)
			{
				this.debugCells[i].Draw();
			}
			for (int j = 0; j < this.debugLines.Count; j++)
			{
				this.debugLines[j].Draw();
			}
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x000630C0 File Offset: 0x000612C0
		public void DebugDrawerTick()
		{
			for (int i = this.debugCells.Count - 1; i >= 0; i--)
			{
				DebugCell debugCell = this.debugCells[i];
				debugCell.ticksLeft--;
				if (debugCell.ticksLeft <= 0)
				{
					this.debugCells.RemoveAt(i);
				}
			}
			this.debugLines.RemoveAll((DebugLine dl) => dl.Done);
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00063140 File Offset: 0x00061340
		public void DebugDrawerOnGUI()
		{
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.MiddleCenter;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				for (int i = 0; i < this.debugCells.Count; i++)
				{
					this.debugCells[i].OnGUI();
				}
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x04000FC5 RID: 4037
		private List<DebugCell> debugCells = new List<DebugCell>();

		// Token: 0x04000FC6 RID: 4038
		private List<DebugLine> debugLines = new List<DebugLine>();

		// Token: 0x04000FC7 RID: 4039
		private const int DefaultLifespanTicks = 50;
	}
}
