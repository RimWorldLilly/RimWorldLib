using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C55 RID: 7253
	[StaticConstructorOnStartup]
	public class Designator_PaintBuilding : Designator_Paint
	{
		// Token: 0x17002172 RID: 8562
		// (get) Token: 0x0600B913 RID: 47379 RVA: 0x004145A0 File Offset: 0x004127A0
		protected override Texture2D IconTopTex
		{
			get
			{
				return ContentFinder<Texture2D>.Get("UI/Designators/Paint_Top", true);
			}
		}

		// Token: 0x17002173 RID: 8563
		// (get) Token: 0x0600B914 RID: 47380 RVA: 0x004145AD File Offset: 0x004127AD
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.PaintBuilding;
			}
		}

		// Token: 0x0600B915 RID: 47381 RVA: 0x004145B4 File Offset: 0x004127B4
		public Designator_PaintBuilding()
		{
			this.defaultLabel = "DesignatorPaintBuilding".Translate();
			this.defaultDesc = "DesignatorPaintBuildingDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Paint_Bottom", true);
			this.tutorTag = "PaintBuilding";
		}

		// Token: 0x0600B916 RID: 47382 RVA: 0x00414610 File Offset: 0x00412810
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				return this.eyedropper.CanDesignateCell(c);
			}
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]))
				{
					return true;
				}
			}
			return "MessageMustDesignatePaintableBuildings".Translate(this.colorDef);
		}

		// Token: 0x0600B917 RID: 47383 RVA: 0x004146A8 File Offset: 0x004128A8
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				this.eyedropper.DesignateSingleCell(c);
				return;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x0600B918 RID: 47384 RVA: 0x0041470C File Offset: 0x0041290C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.building == null || !t.def.building.paintable)
			{
				return false;
			}
			if (t.Faction != Faction.OfPlayer)
			{
				return false;
			}
			Building building = t as Building;
			if (building != null && building.PaintColorDef == this.colorDef)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.Uninstall) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B919 RID: 47385 RVA: 0x00414798 File Offset: 0x00412998
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.TryRemoveDesignationOn(t, this.Designation);
			base.Map.designationManager.TryRemoveDesignationOn(t, DesignationDefOf.RemovePaintBuilding);
			if (DebugSettings.godMode)
			{
				((Building)t).ChangePaint(this.colorDef);
				return;
			}
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, this.colorDef));
		}

		// Token: 0x0600B91A RID: 47386 RVA: 0x00414814 File Offset: 0x00412A14
		protected override int NumHighlightedCells()
		{
			Designator_PaintBuilding.tmpPaintThings.Clear();
			Find.DesignatorManager.Dragger.UpdateCellBuffer();
			foreach (IntVec3 c in Find.DesignatorManager.Dragger.CellBuffer)
			{
				if (c.InBounds(base.Map) && !c.Fogged(base.Map))
				{
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (!Designator_PaintBuilding.tmpPaintThings.Contains(thingList[i]) && this.CanDesignateThing(thingList[i]))
						{
							Designator_PaintBuilding.tmpPaintThings.Add(thingList[i]);
						}
					}
				}
			}
			return Designator_PaintBuilding.tmpPaintThings.Count;
		}

		// Token: 0x040069C4 RID: 27076
		private static readonly HashSet<Thing> tmpPaintThings = new HashSet<Thing>();
	}
}
