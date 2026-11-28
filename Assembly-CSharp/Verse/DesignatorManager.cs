using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005B1 RID: 1457
	public class DesignatorManager
	{
		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06002C4B RID: 11339 RVA: 0x0010BFAB File Offset: 0x0010A1AB
		public Designator SelectedDesignator
		{
			get
			{
				return this.selectedDesignator;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x0010BFB3 File Offset: 0x0010A1B3
		// (set) Token: 0x06002C4D RID: 11341 RVA: 0x0010BFBC File Offset: 0x0010A1BC
		public DrawStyleDef SelectedStyle
		{
			get
			{
				return this.selectedStyle;
			}
			set
			{
				this.selectedStyle = value;
				this.dragger.UpdateDragCellsIfNeeded();
				DrawStyleCategoryDef drawStyleCategory = this.selectedDesignator.DrawStyleCategory;
				if (drawStyleCategory != null && drawStyleCategory.styles.Contains(value))
				{
					this.previouslySelected[drawStyleCategory] = value;
				}
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002C4E RID: 11342 RVA: 0x0010C005 File Offset: 0x0010A205
		public DesignationDragger Dragger
		{
			get
			{
				return this.dragger;
			}
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x0010C010 File Offset: 0x0010A210
		public void Select(Designator des)
		{
			this.Deselect();
			this.selectedDesignator = des;
			DrawStyleCategoryDef drawStyleCategory = des.DrawStyleCategory;
			if (drawStyleCategory != null && !drawStyleCategory.styles.NullOrEmpty<DrawStyleDef>())
			{
				DrawStyleDef drawStyleDef;
				if (Prefs.RememberDrawStlyes && this.previouslySelected.TryGetValue(drawStyleCategory, out drawStyleDef))
				{
					this.selectedStyle = drawStyleDef;
				}
				else
				{
					this.selectedStyle = drawStyleCategory.styles[0];
				}
			}
			else
			{
				this.selectedStyle = null;
			}
			this.selectedDesignator.Selected();
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x0010C087 File Offset: 0x0010A287
		public void Deselect()
		{
			if (this.selectedDesignator == null)
			{
				return;
			}
			this.selectedDesignator.Deselected();
			this.selectedDesignator = null;
			this.dragger.EndDrag();
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x0010C0AF File Offset: 0x0010A2AF
		private bool CheckSelectedDesignatorValid()
		{
			if (this.selectedDesignator == null)
			{
				return false;
			}
			if (!this.selectedDesignator.CanRemainSelected())
			{
				this.Deselect();
				return false;
			}
			return true;
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x0010C0D4 File Offset: 0x0010A2D4
		public void ProcessInputEvents()
		{
			if (!this.CheckSelectedDesignatorValid())
			{
				return;
			}
			if (Event.current.type == EventType.mouseDown && Event.current.button == 0)
			{
				if (this.SelectedStyle == null)
				{
					Designator designator = this.selectedDesignator;
					AcceptanceReport acceptanceReport = this.selectedDesignator.CanDesignateCell(UI.MouseCell());
					if (acceptanceReport.Accepted)
					{
						designator.DesignateSingleCell(UI.MouseCell());
						designator.Finalize(true);
					}
					else
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.SilentInput, false);
						this.selectedDesignator.Finalize(false);
					}
				}
				else
				{
					this.dragger.StartDrag();
				}
				Event.current.Use();
			}
			if ((Event.current.type == EventType.mouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.Deselect();
				this.dragger.EndDrag();
				Event.current.Use();
				TutorSystem.Notify_Event("ClearDesignatorSelection");
			}
			if (Event.current.type == EventType.mouseUp && Event.current.button == 0 && (this.dragger.Dragging || (this.selectedStyle != null && this.selectedStyle.DrawStyleWorker.SingleCell)))
			{
				this.selectedDesignator.DesignateMultiCell(this.dragger.DragCells);
				this.dragger.EndDrag();
				Event.current.Use();
			}
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x0010C239 File Offset: 0x0010A439
		public void DesignationManagerOnGUI()
		{
			this.dragger.DraggerOnGUI();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.DrawMouseAttachments();
				DrawStyleDef drawStyleDef = this.selectedStyle;
				if (drawStyleDef == null)
				{
					return;
				}
				drawStyleDef.DrawStyleWorker.Draw();
			}
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x0010C26E File Offset: 0x0010A46E
		public void DesignatorManagerUpdate()
		{
			this.dragger.DraggerUpdate();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.SelectedUpdate();
			}
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x0010C28E File Offset: 0x0010A48E
		public void AdvanceDrawStyle()
		{
			this.ChangeDrawStyle(1);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x0010C297 File Offset: 0x0010A497
		public void PreviousDrawStyle()
		{
			this.ChangeDrawStyle(-1);
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x0010C2A0 File Offset: 0x0010A4A0
		private void ChangeDrawStyle(int delta)
		{
			if (this.SelectedStyle == null || this.selectedDesignator.DrawStyleCategory == null || this.selectedDesignator.DrawStyleCategory.styles.NullOrEmpty<DrawStyleDef>())
			{
				return;
			}
			List<DrawStyleDef> styles = this.selectedDesignator.DrawStyleCategory.styles;
			int index = GenMath.PositiveMod(styles.IndexOf(this.SelectedStyle) + delta, styles.Count);
			this.SelectedStyle = styles[index];
		}

		// Token: 0x04001E02 RID: 7682
		private Designator selectedDesignator;

		// Token: 0x04001E03 RID: 7683
		private DrawStyleDef selectedStyle;

		// Token: 0x04001E04 RID: 7684
		private Dictionary<DrawStyleCategoryDef, DrawStyleDef> previouslySelected = new Dictionary<DrawStyleCategoryDef, DrawStyleDef>();

		// Token: 0x04001E05 RID: 7685
		private readonly DesignationDragger dragger = new DesignationDragger();
	}
}
