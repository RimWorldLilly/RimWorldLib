using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C54 RID: 7252
	public abstract class Designator_Paint : DesignatorWithEyedropper
	{
		// Token: 0x1700216C RID: 8556
		// (get) Token: 0x0600B905 RID: 47365
		protected abstract Texture2D IconTopTex { get; }

		// Token: 0x1700216D RID: 8557
		// (get) Token: 0x0600B906 RID: 47366 RVA: 0x004140C0 File Offset: 0x004122C0
		public override Color IconDrawColor
		{
			get
			{
				return this.colorDef.color;
			}
		}

		// Token: 0x1700216E RID: 8558
		// (get) Token: 0x0600B907 RID: 47367 RVA: 0x004140CD File Offset: 0x004122CD
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700216F RID: 8559
		// (get) Token: 0x0600B908 RID: 47368 RVA: 0x004140D0 File Offset: 0x004122D0
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Paint;
			}
		}

		// Token: 0x17002170 RID: 8560
		// (get) Token: 0x0600B909 RID: 47369 RVA: 0x004140D8 File Offset: 0x004122D8
		protected virtual IEnumerable<ColorDef> Colors
		{
			get
			{
				return from x in DefDatabase<ColorDef>.AllDefs
				where x.colorType == ColorType.Structure
				orderby x.displayOrder
				select x;
			}
		}

		// Token: 0x17002171 RID: 8561
		// (get) Token: 0x0600B90A RID: 47370 RVA: 0x00414134 File Offset: 0x00412334
		private string AttachmentString
		{
			get
			{
				if (this.cachedAttachmentString == null)
				{
					this.cachedAttachmentString = "Paint".Translate() + ": " + this.colorDef.LabelCap + "\n" + KeyBindingDefOf.ShowEyedropper.MainKeyLabel + ": " + "GrabExistingColor".Translate();
				}
				return this.cachedAttachmentString;
			}
		}

		// Token: 0x0600B90B RID: 47371 RVA: 0x004141B0 File Offset: 0x004123B0
		public Designator_Paint()
		{
			this.colorDef = this.Colors.FirstOrDefault<ColorDef>();
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Paint;
			this.hotKey = KeyBindingDefOf.Misc6;
			this.eyedropper = new Designator_Eyedropper(delegate(ColorDef newCol)
			{
				this.colorDef = newCol;
				this.cachedAttachmentString = null;
				if (!this.eyedropMode)
				{
					Find.DesignatorManager.Select(this);
				}
			}, "SelectAPaintedBuilding".Translate(), "DesignatorEyeDropperDesc_Paint".Translate());
		}

		// Token: 0x0600B90C RID: 47372 RVA: 0x0041423C File Offset: 0x0041243C
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			List<FloatMenuGridOption> list = new List<FloatMenuGridOption>();
			List<FloatMenuGridOption> list2 = list;
			Texture2D eyeDropperTex = Designator_Eyedropper.EyeDropperTex;
			Action action = delegate()
			{
				this.<>n__0(ev);
				Find.DesignatorManager.Select(this.eyedropper);
			};
			TipSignal? tooltip = new TipSignal?("DesignatorEyeDropperDesc_Paint".Translate());
			list2.Add(new FloatMenuGridOption(eyeDropperTex, action, null, tooltip));
			foreach (ColorDef newCol2 in this.Colors)
			{
				ColorDef newCol = newCol2;
				list.Add(new FloatMenuGridOption(BaseContent.WhiteTex, delegate()
				{
					this.<>n__0(ev);
					Find.DesignatorManager.Select(this);
					this.colorDef = newCol;
					this.cachedAttachmentString = null;
				}, new Color?(newCol.color), new TipSignal?(newCol.LabelCap)));
			}
			Find.WindowStack.Add(new FloatMenuGrid(list));
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x0600B90D RID: 47373 RVA: 0x00414360 File Offset: 0x00412560
		public override void DrawMouseAttachments()
		{
			this.eyedropMode = KeyBindingDefOf.ShowEyedropper.IsDown;
			if (this.eyedropMode)
			{
				this.eyedropper.DrawMouseAttachments();
				return;
			}
			if (this.useMouseIcon)
			{
				Texture icon = this.icon;
				string attachmentString = this.AttachmentString;
				float iconAngle = this.iconAngle;
				Vector2 iconOffset = this.iconOffset;
				Color? color = new Color?(this.colorDef.color);
				GenUI.DrawMouseAttachment(icon, attachmentString, iconAngle, iconOffset, null, null, false, default(Color), color, delegate(Rect r)
				{
					GUI.DrawTexture(r, this.IconTopTex);
				});
			}
			if (Find.DesignatorManager.Dragger.Dragging)
			{
				Vector2 vector = Event.current.mousePosition + Designator_Place.PlaceMouseAttachmentDrawOffset;
				if (this.useMouseIcon)
				{
					vector.y += 32f + Text.LineHeight * 2f;
				}
				Rect rect = new Rect(vector.x, vector.y, 27f, 27f);
				ThingDef dye = ThingDefOf.Dye;
				ThingDef stuffDef = null;
				ThingStyleDef thingStyleDef = null;
				float scale = 1f;
				Color? color = null;
				Widgets.ThingIcon(rect, dye, stuffDef, thingStyleDef, scale, color, null, 1f);
				int num = this.NumHighlightedCells();
				string text = num.ToStringCached();
				if (base.Map.resourceCounter.GetCount(ThingDefOf.Dye) < num)
				{
					GUI.color = Color.red;
					text += " (" + "NotEnoughStoredLower".Translate() + ")";
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(new Rect(vector.x + 29f, vector.y, 999f, 29f), text);
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600B90E RID: 47374 RVA: 0x0041452B File Offset: 0x0041272B
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			base.DrawIcon(rect, buttonMat, parms);
			Widgets.DrawTextureFitted(rect, this.IconTopTex, this.iconDrawScale * 0.85f, this.iconProportions, this.iconTexCoords, this.iconAngle, buttonMat, 1f);
		}

		// Token: 0x0600B90F RID: 47375
		protected abstract int NumHighlightedCells();

		// Token: 0x040069C2 RID: 27074
		protected ColorDef colorDef;

		// Token: 0x040069C3 RID: 27075
		private string cachedAttachmentString;
	}
}
