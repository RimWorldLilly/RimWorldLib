using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7D RID: 7293
	public abstract class Designator_ZoneAdd : Designator_Zone
	{
		// Token: 0x170021C6 RID: 8646
		// (get) Token: 0x0600BA3E RID: 47678 RVA: 0x0041A7C7 File Offset: 0x004189C7
		// (set) Token: 0x0600BA3F RID: 47679 RVA: 0x0041A7D3 File Offset: 0x004189D3
		private Zone SelectedZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
			set
			{
				Find.Selector.ClearSelection();
				if (value != null)
				{
					Find.Selector.Select(value, false, false);
				}
			}
		}

		// Token: 0x170021C7 RID: 8647
		// (get) Token: 0x0600BA40 RID: 47680
		protected abstract string NewZoneLabel { get; }

		// Token: 0x0600BA41 RID: 47681
		protected abstract Zone MakeNewZone();

		// Token: 0x0600BA42 RID: 47682 RVA: 0x0041A7F0 File Offset: 0x004189F0
		public Designator_ZoneAdd()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc6;
		}

		// Token: 0x0600BA43 RID: 47683 RVA: 0x0041A844 File Offset: 0x00418A44
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			if (Find.Selector.SelectedZone != null && Find.Selector.SelectedZone.GetType() != this.zoneTypeToPlace)
			{
				Find.Selector.Deselect(Find.Selector.SelectedZone);
			}
		}

		// Token: 0x0600BA44 RID: 47684 RVA: 0x0041A894 File Offset: 0x00418A94
		public override void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				string text = "";
				if (!Input.GetKey(KeyCode.Mouse0))
				{
					Zone selectedZone = Find.Selector.SelectedZone;
					if (selectedZone != null)
					{
						text = "ExpandOrCreateZone".Translate(selectedZone.label, this.NewZoneLabel);
					}
					else
					{
						text = "CreateNewZone".Translate(this.NewZoneLabel);
					}
				}
				GenUI.DrawMouseAttachment(this.icon, text, 0f, default(Vector2), null, null, false, default(Color), null, null);
			}
		}

		// Token: 0x0600BA45 RID: 47685 RVA: 0x0041A954 File Offset: 0x00418B54
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			if (zone != null && zone.GetType() != this.zoneTypeToPlace)
			{
				return false;
			}
			return Designator_ZoneAdd.IsZoneableCell(c, base.Map);
		}

		// Token: 0x0600BA46 RID: 47686 RVA: 0x0041A9B4 File Offset: 0x00418BB4
		public static AcceptanceReport IsZoneableCell(IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			if (c.Fogged(map))
			{
				return false;
			}
			if (c.InNoZoneEdgeArea(map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			using (IEnumerator<Thing> enumerator = map.thingGrid.ThingsAt(c).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.def.CanOverlapZones)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600BA47 RID: 47687 RVA: 0x0041AA58 File Offset: 0x00418C58
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			this.unsetCells.Clear();
			this.unsetCells.AddRange(cells);
			if (this.unsetCells.Count == 1)
			{
				Zone zone = base.Map.zoneManager.ZoneAt(this.unsetCells[0]);
				if (zone != null)
				{
					if (zone.GetType() == this.zoneTypeToPlace)
					{
						this.SelectedZone = zone;
					}
					return;
				}
			}
			if (this.SelectedZone == null)
			{
				Zone zone2 = null;
				foreach (IntVec3 c3 in cells)
				{
					Zone zone3 = base.Map.zoneManager.ZoneAt(c3);
					if (zone3 != null && zone3.GetType() == this.zoneTypeToPlace)
					{
						if (zone2 == null)
						{
							zone2 = zone3;
						}
						else if (zone3 != zone2)
						{
							zone2 = null;
							break;
						}
					}
				}
				this.SelectedZone = zone2;
			}
			this.unsetCells.RemoveAll((IntVec3 c) => base.Map.zoneManager.ZoneAt(c) != null);
			if (this.unsetCells.Count == 0)
			{
				return;
			}
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, this.unsetCells)))
			{
				return;
			}
			if (this.SelectedZone == null)
			{
				this.SelectedZone = this.MakeNewZone();
				base.Map.zoneManager.RegisterZone(this.SelectedZone);
				this.SelectedZone.AddCell(this.unsetCells[0]);
				this.unsetCells.RemoveAt(0);
			}
			bool somethingSucceeded;
			for (;;)
			{
				somethingSucceeded = true;
				int count = this.unsetCells.Count;
				for (int i = this.unsetCells.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					for (int j = 0; j < 4; j++)
					{
						IntVec3 c2 = this.unsetCells[i] + GenAdj.CardinalDirections[j];
						if (c2.InBounds(base.Map) && base.Map.zoneManager.ZoneAt(c2) == this.SelectedZone)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						this.SelectedZone.AddCell(this.unsetCells[i]);
						this.unsetCells.RemoveAt(i);
					}
				}
				if (this.unsetCells.Count == 0)
				{
					break;
				}
				if (this.unsetCells.Count == count)
				{
					this.SelectedZone = this.MakeNewZone();
					base.Map.zoneManager.RegisterZone(this.SelectedZone);
					this.SelectedZone.AddCell(this.unsetCells[0]);
					this.unsetCells.RemoveAt(0);
				}
			}
			this.SelectedZone.CheckContiguous();
			base.Finalize(somethingSucceeded);
			Zone_Stockpile zone_Stockpile = this.SelectedZone as Zone_Stockpile;
			if (zone_Stockpile != null)
			{
				zone_Stockpile.slotGroup.RemoveHaulDesignationOnStoredThings();
			}
			TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, this.unsetCells));
		}

		// Token: 0x040069EF RID: 27119
		protected Type zoneTypeToPlace;

		// Token: 0x040069F0 RID: 27120
		private List<IntVec3> unsetCells = new List<IntVec3>();
	}
}
