using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200027C RID: 636
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x0006A188 File Offset: 0x00068388
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0006A190 File Offset: 0x00068390
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 3640, 1f);
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0006A204 File Offset: 0x00068404
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.defGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.defGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "defGrid");
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.countGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.countGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "countGrid");
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0006A261 File Offset: 0x00068461
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0006A280 File Offset: 0x00068480
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0006A29C File Offset: 0x0006849C
		public void SetAt(IntVec3 c, ThingDef def, int count)
		{
			if (count == 0)
			{
				def = null;
			}
			ushort num;
			if (def == null)
			{
				num = 0;
			}
			else
			{
				num = def.shortHash;
			}
			ushort num2 = (ushort)count;
			if (count > 65535)
			{
				Log.Error("Cannot store count " + count.ToString() + " in DeepResourceGrid: out of ushort range.");
				num2 = ushort.MaxValue;
			}
			if (count < 0)
			{
				Log.Error("Cannot store count " + count.ToString() + " in DeepResourceGrid: out of ushort range.");
				num2 = 0;
			}
			int num3 = this.map.cellIndices.CellToIndex(c);
			if (this.defGrid[num3] == num && this.countGrid[num3] == num2)
			{
				return;
			}
			this.defGrid[num3] = num;
			this.countGrid[num3] = num2;
			this.drawer.SetDirty();
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0006A350 File Offset: 0x00068550
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0006A36A File Offset: 0x0006856A
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap && !Find.ScreenshotModeHandler.Active)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0006A390 File Offset: 0x00068590
		public void DrawPlacingMouseAttachments(BuildableDef placingDef)
		{
			ThingDef thingDef = placingDef as ThingDef;
			if (thingDef != null && thingDef.CompDefFor<CompDeepDrill>() != null && this.AnyActiveDeepScannersOnMap())
			{
				this.RenderMouseAttachments();
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0006A3C0 File Offset: 0x000685C0
		public void DeepResourcesOnGUI()
		{
			Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
			if (singleSelectedThing == null)
			{
				return;
			}
			CompDeepScanner compDeepScanner = singleSelectedThing.TryGetComp<CompDeepScanner>();
			CompDeepDrill compDeepDrill = singleSelectedThing.TryGetComp<CompDeepDrill>();
			if (compDeepScanner == null && compDeepDrill == null)
			{
				return;
			}
			if (!this.AnyActiveDeepScannersOnMap())
			{
				return;
			}
			this.RenderMouseAttachments();
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0006A400 File Offset: 0x00068600
		public bool AnyActiveDeepScannersOnMap()
		{
			foreach (Building thing in this.map.listerBuildings.allBuildingsColonist)
			{
				CompDeepScanner compDeepScanner = thing.TryGetComp<CompDeepScanner>();
				if (compDeepScanner != null && compDeepScanner.ShouldShowDeepResourceOverlay())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0006A470 File Offset: 0x00068670
		private void RenderMouseAttachments()
		{
			IntVec3 c = UI.MouseCell();
			if (!c.InBounds(this.map))
			{
				return;
			}
			ThingDef thingDef = this.map.deepResourceGrid.ThingDefAt(c);
			if (thingDef == null)
			{
				return;
			}
			int num = this.map.deepResourceGrid.CountAt(c);
			if (num <= 0)
			{
				return;
			}
			Vector2 vector = c.ToVector3().MapToUIPosition();
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			float num2 = (UI.CurUICellSize() - 27f) / 2f;
			Rect rect = new Rect(vector.x + num2, vector.y - UI.CurUICellSize() + num2, 27f, 27f);
			Widgets.ThingIcon(rect, thingDef, null, null, 1f, null, null, 1f);
			Widgets.Label(new Rect(rect.xMax + 4f, rect.y, 999f, 29f), "DeepResourceRemaining".Translate(thingDef.Named("RESOURCE"), num.Named("COUNT")));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0006A599 File Offset: 0x00068799
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0006A5B8 File Offset: 0x000687B8
		public Color GetCellExtraColor(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			float num = (float)this.CountAt(c);
			ThingDef thingDef = this.ThingDefAt(c);
			return DebugMatsSpectrum.Mat(Mathf.RoundToInt(num / (float)thingDef.deepCountPerCell / 2f * 100f) % 100, true).color;
		}

		// Token: 0x040010AA RID: 4266
		private const float LineSpacing = 29f;

		// Token: 0x040010AB RID: 4267
		private const float IconPaddingRight = 4f;

		// Token: 0x040010AC RID: 4268
		private const float IconSize = 27f;

		// Token: 0x040010AD RID: 4269
		private Map map;

		// Token: 0x040010AE RID: 4270
		private CellBoolDrawer drawer;

		// Token: 0x040010AF RID: 4271
		private ushort[] defGrid;

		// Token: 0x040010B0 RID: 4272
		private ushort[] countGrid;
	}
}
