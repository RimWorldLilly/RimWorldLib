using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C49 RID: 7241
	[StaticConstructorOnStartup]
	public class Designator_Eyedropper : Designator
	{
		// Token: 0x0600B8A0 RID: 47264 RVA: 0x004120E0 File Offset: 0x004102E0
		public Designator_Eyedropper(Action<ColorDef> selectAction, string rejectMessage, string desc)
		{
			this.selectAction = selectAction;
			this.rejectMessage = rejectMessage;
			this.defaultLabel = "DesignatorEyedropper".Translate();
			this.defaultDesc = desc;
			this.icon = Designator_Eyedropper.EyeDropperTex;
			this.useMouseIcon = true;
		}

		// Token: 0x0600B8A1 RID: 47265 RVA: 0x0041212F File Offset: 0x0041032F
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (this.ColorDefAt(c) != null)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (!this.rejectMessage.NullOrEmpty())
			{
				return this.rejectMessage;
			}
			return false;
		}

		// Token: 0x0600B8A2 RID: 47266 RVA: 0x00412160 File Offset: 0x00410360
		public override void DesignateSingleCell(IntVec3 cell)
		{
			ColorDef colorDef = this.ColorDefAt(cell);
			if (colorDef != null)
			{
				Action<ColorDef> action = this.selectAction;
				if (action != null)
				{
					action(colorDef);
				}
				Messages.Message("GrabbedColor".Translate() + ": " + colorDef.LabelCap, null, MessageTypeDefOf.NeutralEvent, false);
				return;
			}
			if (!this.rejectMessage.NullOrEmpty())
			{
				Messages.Message(this.rejectMessage, null, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x0600B8A3 RID: 47267 RVA: 0x004121DC File Offset: 0x004103DC
		protected virtual ColorDef ColorDefAt(IntVec3 cell)
		{
			if (!cell.InBounds(base.Map) || cell.Fogged(base.Map))
			{
				return null;
			}
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Building building = thingList[i] as Building;
				if (building != null && building.def.building.paintable && building.PaintColorDef != null)
				{
					return building.PaintColorDef;
				}
			}
			return base.Map.terrainGrid.ColorAt(cell) ?? cell.GetTerrain(base.Map).colorDef;
		}

		// Token: 0x0600B8A4 RID: 47268 RVA: 0x0041227C File Offset: 0x0041047C
		public override void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				string text = string.Empty;
				ColorDef colorDef = this.ColorDefAt(UI.MouseCell());
				if (colorDef != null)
				{
					text = "Grab".Translate() + ": " + colorDef.LabelCap;
				}
				else if (!this.rejectMessage.NullOrEmpty())
				{
					text = this.rejectMessage;
				}
				GenUI.DrawMouseAttachment(this.icon, text, this.iconAngle, this.iconOffset, null, null, false, default(Color), null, null);
			}
		}

		// Token: 0x040069AF RID: 27055
		private Action<ColorDef> selectAction;

		// Token: 0x040069B0 RID: 27056
		private string rejectMessage;

		// Token: 0x040069B1 RID: 27057
		public static readonly Texture2D EyeDropperTex = ContentFinder<Texture2D>.Get("UI/Icons/Eyedropper", true);
	}
}
