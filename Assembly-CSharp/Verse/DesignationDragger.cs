using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005AD RID: 1453
	[StaticConstructorOnStartup]
	public class DesignationDragger
	{
		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002C0B RID: 11275 RVA: 0x0010AE7C File Offset: 0x0010907C
		public bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002C0C RID: 11276 RVA: 0x0010AE84 File Offset: 0x00109084
		private Designator SelDes
		{
			get
			{
				return Find.DesignatorManager.SelectedDesignator;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002C0D RID: 11277 RVA: 0x0010AE90 File Offset: 0x00109090
		private DrawStyleDef SelStyle
		{
			get
			{
				return Find.DesignatorManager.SelectedStyle;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002C0E RID: 11278 RVA: 0x0010AE9C File Offset: 0x0010909C
		public List<IntVec3> CellBuffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002C0F RID: 11279 RVA: 0x0010AEA4 File Offset: 0x001090A4
		public List<IntVec3> DragCells
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.dragCells;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002C10 RID: 11280 RVA: 0x0010AEB2 File Offset: 0x001090B2
		public string FailureReason
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.failureReasonInt;
			}
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x0010AEC0 File Offset: 0x001090C0
		public void StartDrag()
		{
			this.dragging = !this.SelStyle.DrawStyleWorker.SingleCell;
			this.startDragCell = UI.MouseCell();
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x0010AEE6 File Offset: 0x001090E6
		public void EndDrag()
		{
			this.dragging = false;
			this.lastDragRealTime = -99999f;
			this.lastFrameDragCellsDrawn = 0;
			if (this.sustainer != null)
			{
				this.sustainer.End();
				this.sustainer = null;
			}
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x0010AF1C File Offset: 0x0010911C
		public void DraggerUpdate()
		{
			if (!this.dragging)
			{
				DrawStyleDef selStyle = this.SelStyle;
				if (((selStyle != null) ? selStyle.DrawStyleWorker : null) == null || !this.SelStyle.DrawStyleWorker.SingleCell)
				{
					return;
				}
			}
			this.tmpHighlightCells.Clear();
			this.numSelectedCells = 0;
			this.UpdateCellBuffer();
			CellRect cellRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(3).ClipInsideMap(this.SelDes.Map);
			for (int i = this.buffer.Count - 1; i >= 0; i--)
			{
				IntVec3 intVec = this.buffer[i];
				if (cellRect.Contains(intVec) && this.SelDes.CanDesignateCell(intVec))
				{
					if (cellRect.Contains(intVec))
					{
						this.tmpHighlightCells.Add(intVec);
					}
					this.numSelectedCells++;
				}
			}
			if (this.SelDes.DrawHighlight)
			{
				this.SelDes.RenderHighlight(this.tmpHighlightCells);
			}
			if (this.numSelectedCells != this.lastFrameDragCellsDrawn)
			{
				if (this.SelDes.soundDragChanged != null)
				{
					SoundInfo info = SoundInfo.OnCamera(MaintenanceType.None);
					info.SetParameter("TimeSinceDrag", Time.realtimeSinceStartup - this.lastDragRealTime);
					this.SelDes.soundDragChanged.PlayOneShot(info);
				}
				this.lastDragRealTime = Time.realtimeSinceStartup;
				this.lastFrameDragCellsDrawn = this.numSelectedCells;
			}
			if (this.sustainer == null || this.sustainer.Ended)
			{
				if (this.SelDes.soundDragSustain != null)
				{
					this.sustainer = this.SelDes.soundDragSustain.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
					return;
				}
			}
			else
			{
				this.sustainer.externalParams["TimeSinceDrag"] = Time.realtimeSinceStartup - this.lastDragRealTime;
				this.sustainer.Maintain();
			}
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x0010B0EC File Offset: 0x001092EC
		public void DraggerOnGUI()
		{
			if (this.dragging)
			{
				DrawStyleDef selStyle = this.SelStyle;
				if (((selStyle != null) ? selStyle.DrawStyleWorker : null) != null && !this.SelStyle.DrawStyleWorker.SingleCell)
				{
					ValueTuple<IntVec3, IntVec3> currentBoundary = this.GetCurrentBoundary();
					IntVec3 item = currentBoundary.Item1;
					IntVec3 item2 = currentBoundary.Item2;
					IntVec3 intVec = item - item2;
					intVec.x = Mathf.Abs(intVec.x) + 1;
					intVec.z = Mathf.Abs(intVec.z) + 1;
					if (this.SelStyle.drawOutline && (intVec.x > 1 || intVec.z > 1))
					{
						Vector3 v = new Vector3((float)Mathf.Min(item.x, item2.x), 0f, (float)Mathf.Min(item.z, item2.z));
						Vector3 v2 = new Vector3((float)(Mathf.Max(item.x, item2.x) + 1), 0f, (float)(Mathf.Max(item.z, item2.z) + 1));
						Vector2 vector = v.MapToUIPosition();
						Vector2 vector2 = v2.MapToUIPosition();
						Widgets.DrawBox(Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y), 1, DesignationDragger.OutlineTex);
					}
					if (this.SelDes.DragDrawMeasurements)
					{
						bool flag = intVec.x >= intVec.z;
						if (intVec.x >= 5 && (this.SelStyle.drawShortSideMeasurement || flag))
						{
							Vector2 screenPos = (item.ToUIPosition() + item2.ToUIPosition()) / 2f;
							screenPos.y = item.ToUIPosition().y;
							Widgets.DrawNumberOnMap(screenPos, intVec.x, Color.white);
						}
						if (intVec.z >= 5 && (this.SelStyle.drawShortSideMeasurement || !flag))
						{
							Vector2 screenPos2 = (item.ToUIPosition() + item2.ToUIPosition()) / 2f;
							screenPos2.x = item.ToUIPosition().x;
							Widgets.DrawNumberOnMap(screenPos2, intVec.z, Color.white);
						}
					}
					if (intVec.SqrMagnitude >= 9f && intVec.x >= 3 && intVec.z >= 3 && this.buffer.Count > 0 && this.SelStyle.drawArea)
					{
						Widgets.DrawNumberOnMap((item.ToUIPosition() + item2.ToUIPosition()) / 2f, this.buffer.Count, Color.white);
					}
					return;
				}
			}
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x0010B378 File Offset: 0x00109578
		public void UpdateCellBuffer()
		{
			ValueTuple<IntVec3, IntVec3> currentBoundary = this.GetCurrentBoundary();
			IntVec3 item = currentBoundary.Item1;
			IntVec3 item2 = currentBoundary.Item2;
			if (this.lastStart == item && this.lastEnd == item2 && this.lastStyleDef == this.SelStyle && this.lastTick == GenTicks.TicksGame)
			{
				return;
			}
			this.buffer.Clear();
			this.SelStyle.DrawStyleWorker.Update(item, item2, this.buffer);
			this.lastStart = item;
			this.lastEnd = item2;
			this.lastStyleDef = this.SelStyle;
			this.lastTick = GenTicks.TicksGame;
			if (!this.SelStyle.DrawStyleWorker.CanHaveDuplicates)
			{
				return;
			}
			for (int i = this.buffer.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < i; j++)
				{
					if (this.buffer[i] == this.buffer[j])
					{
						this.buffer.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x0010B47C File Offset: 0x0010967C
		[return: TupleElementNames(new string[]
		{
			"start",
			"end"
		})]
		private ValueTuple<IntVec3, IntVec3> GetCurrentBoundary()
		{
			IntVec3 intVec = this.startDragCell;
			IntVec3 intVec2 = UI.MouseCell();
			if (this.SelStyle.DrawStyleWorker.SingleCell)
			{
				return new ValueTuple<IntVec3, IntVec3>(intVec2, intVec2);
			}
			if (KeyBindingDefOf.Designator_ShapeSnap.IsDown && this.SelStyle.canSnap)
			{
				IntVec3 intVec3 = intVec2 - intVec;
				int num = (Mathf.Abs(intVec3.x) >= Mathf.Abs(intVec3.z)) ? intVec3.x : intVec3.z;
				intVec2.x = intVec.x + ((!Mathf.Approximately(Mathf.Sign((float)intVec3.x), Mathf.Sign((float)num))) ? (-num) : num);
				intVec2.z = intVec.z + ((!Mathf.Approximately(Mathf.Sign((float)intVec3.z), Mathf.Sign((float)num))) ? (-num) : num);
			}
			return new ValueTuple<IntVec3, IntVec3>(intVec, intVec2);
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x0010B564 File Offset: 0x00109764
		public void UpdateDragCellsIfNeeded()
		{
			if (Time.frameCount == this.lastUpdateFrame && this.lastStyle == this.SelStyle)
			{
				return;
			}
			this.lastUpdateFrame = Time.frameCount;
			this.dragCells.Clear();
			this.failureReasonInt = null;
			this.lastStyle = this.SelStyle;
			if (this.SelStyle == null)
			{
				return;
			}
			this.UpdateCellBuffer();
			foreach (IntVec3 intVec in this.buffer)
			{
				this.TryAddDragCell(new IntVec3(intVec.x, this.startDragCell.y, intVec.z));
			}
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x0010B628 File Offset: 0x00109828
		private void TryAddDragCell(IntVec3 c)
		{
			AcceptanceReport acceptanceReport = this.SelDes.CanDesignateCell(c);
			if (acceptanceReport.Accepted)
			{
				this.dragCells.Add(c);
				return;
			}
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				this.failureReasonInt = acceptanceReport.Reason;
			}
		}

		// Token: 0x04001DD8 RID: 7640
		private bool dragging;

		// Token: 0x04001DD9 RID: 7641
		private IntVec3 startDragCell;

		// Token: 0x04001DDA RID: 7642
		private int lastFrameDragCellsDrawn;

		// Token: 0x04001DDB RID: 7643
		private Sustainer sustainer;

		// Token: 0x04001DDC RID: 7644
		private float lastDragRealTime = -1000f;

		// Token: 0x04001DDD RID: 7645
		private readonly List<IntVec3> buffer = new List<IntVec3>();

		// Token: 0x04001DDE RID: 7646
		private readonly List<IntVec3> dragCells = new List<IntVec3>();

		// Token: 0x04001DDF RID: 7647
		private string failureReasonInt;

		// Token: 0x04001DE0 RID: 7648
		private int lastUpdateFrame = -1;

		// Token: 0x04001DE1 RID: 7649
		private static readonly Texture2D OutlineTex = SolidColorMaterials.NewSolidColorTexture(new Color32(109, 139, 79, 100));

		// Token: 0x04001DE2 RID: 7650
		private const string TimeSinceDragParam = "TimeSinceDrag";

		// Token: 0x04001DE3 RID: 7651
		private readonly List<IntVec3> tmpHighlightCells = new List<IntVec3>();

		// Token: 0x04001DE4 RID: 7652
		private int numSelectedCells;

		// Token: 0x04001DE5 RID: 7653
		private IntVec3 lastStart;

		// Token: 0x04001DE6 RID: 7654
		private IntVec3 lastEnd;

		// Token: 0x04001DE7 RID: 7655
		private DrawStyleDef lastStyleDef;

		// Token: 0x04001DE8 RID: 7656
		private int lastTick;

		// Token: 0x04001DE9 RID: 7657
		private DrawStyleDef lastStyle;
	}
}
