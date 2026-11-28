using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020005AE RID: 1454
	public abstract class Designator : Command
	{
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002C1B RID: 11291 RVA: 0x0010B6CF File Offset: 0x001098CF
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002C1C RID: 11292 RVA: 0x0010B6D6 File Offset: 0x001098D6
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002C1D RID: 11293 RVA: 0x0010B6D9 File Offset: 0x001098D9
		public virtual bool DrawHighlight
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002C1E RID: 11294 RVA: 0x0010B6DC File Offset: 0x001098DC
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002C1F RID: 11295 RVA: 0x0010B6DF File Offset: 0x001098DF
		public virtual bool AlwaysDoGuiControls
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002C20 RID: 11296 RVA: 0x0010B6E2 File Offset: 0x001098E2
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002C21 RID: 11297 RVA: 0x0010B6E5 File Offset: 0x001098E5
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002C22 RID: 11298 RVA: 0x0010B6EC File Offset: 0x001098EC
		public virtual DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06002C23 RID: 11299 RVA: 0x0010B6F0 File Offset: 0x001098F0
		public override string TutorTagSelect
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				string result;
				if ((result = this.cachedTutorTagSelect) == null)
				{
					result = (this.cachedTutorTagSelect = "SelectDesignator-" + this.tutorTag);
				}
				return result;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002C24 RID: 11300 RVA: 0x0010B72C File Offset: 0x0010992C
		public string TutorTagDesignate
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				string result;
				if ((result = this.cachedTutorTagDesignate) == null)
				{
					result = (this.cachedTutorTagDesignate = "Designate-" + this.tutorTag);
				}
				return result;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002C25 RID: 11301 RVA: 0x0010B766 File Offset: 0x00109966
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002C26 RID: 11302 RVA: 0x0010B794 File Offset: 0x00109994
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				Designator.<get_RightClickFloatMenuOptions>d__37 <get_RightClickFloatMenuOptions>d__ = new Designator.<get_RightClickFloatMenuOptions>d__37(-2);
				<get_RightClickFloatMenuOptions>d__.<>4__this = this;
				return <get_RightClickFloatMenuOptions>d__;
			}
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x0010B7A4 File Offset: 0x001099A4
		public Designator()
		{
			this.activateSound = SoundDefOf.Tick_Tiny;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x0010B7E2 File Offset: 0x001099E2
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x0010B800 File Offset: 0x00109A00
		public override void ProcessInput(Event ev)
		{
			if (!this.CheckCanInteract())
			{
				return;
			}
			base.ProcessInput(ev);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x0010B820 File Offset: 0x00109A20
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			if (DebugViewSettings.showArchitectMenuOrder)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Tiny;
				Widgets.Label(new Rect(topLeft.x, topLeft.y + 5f, this.GetWidth(maxWidth), 15f), this.Order.ToString());
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			return result;
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x0010B88C File Offset: 0x00109A8C
		public Command_Action CreateReverseDesignationGizmo(Thing t)
		{
			AcceptanceReport acceptanceReport = this.CanDesignateThing(t);
			if (acceptanceReport.Accepted || (this.showReverseDesignatorDisabledReason && !acceptanceReport.Reason.NullOrEmpty()))
			{
				float iconAngle;
				Vector2 iconOffset;
				return new Command_Action
				{
					defaultLabel = this.LabelCapReverseDesignating(t),
					icon = this.IconReverseDesignating(t, out iconAngle, out iconOffset),
					iconAngle = iconAngle,
					iconOffset = iconOffset,
					defaultDesc = (acceptanceReport.Reason.NullOrEmpty() ? this.DescReverseDesignating(t) : acceptanceReport.Reason),
					Order = ((this is Designator_Uninstall) ? -11f : -20f),
					Disabled = !acceptanceReport.Accepted,
					action = delegate()
					{
						if (!TutorSystem.AllowAction(this.TutorTagDesignate))
						{
							return;
						}
						if (this.Designation != null && this.Designation.targetType == TargetType.Thing && this.Map.designationManager.DesignationOn(t, this.Designation) != null)
						{
							return;
						}
						this.DesignateThing(t);
						this.Finalize(true);
					},
					hotKey = this.hotKey,
					groupKeyIgnoreContent = this.groupKeyIgnoreContent,
					groupKey = this.groupKey
				};
			}
			return null;
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x0010B9A5 File Offset: 0x00109BA5
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x0010B9AC File Offset: 0x00109BAC
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002C2E RID: 11310
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		// Token: 0x06002C2F RID: 11311 RVA: 0x0010B9B4 File Offset: 0x00109BB4
		public virtual void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(this.TutorTagDesignate, cells)))
			{
				return;
			}
			bool somethingSucceeded = false;
			bool flag = false;
			foreach (IntVec3 intVec in cells)
			{
				if (this.CanDesignateCell(intVec).Accepted)
				{
					this.DesignateSingleCell(intVec);
					somethingSucceeded = true;
					if (!flag)
					{
						flag = this.ShowWarningForCell(intVec);
					}
				}
			}
			this.Finalize(somethingSucceeded);
			if (TutorSystem.TutorialMode)
			{
				TutorSystem.Notify_Event(new EventPack(this.TutorTagDesignate, cells));
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x0010BA5C File Offset: 0x00109C5C
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x0010BA63 File Offset: 0x00109C63
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x0010BA66 File Offset: 0x00109C66
		public void Finalize(bool somethingSucceeded)
		{
			if (somethingSucceeded)
			{
				this.FinalizeDesignationSucceeded();
				return;
			}
			this.FinalizeDesignationFailed();
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x0010BA78 File Offset: 0x00109C78
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x0010BA90 File Offset: 0x00109C90
		protected virtual void FinalizeDesignationFailed()
		{
			if (this.soundFailed != null)
			{
				this.soundFailed.PlayOneShotOnCamera(null);
			}
			if (Find.DesignatorManager.Dragger.FailureReason != null)
			{
				Messages.Message(Find.DesignatorManager.Dragger.FailureReason, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x0010BADC File Offset: 0x00109CDC
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x0010BAE4 File Offset: 0x00109CE4
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x0010BAEC File Offset: 0x00109CEC
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return (Texture2D)this.icon;
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x0010BB0D File Offset: 0x00109D0D
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x0010BB10 File Offset: 0x00109D10
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.hideMouseIcon ? null : this.icon, this.mouseText, this.iconAngle, this.iconOffset, null, null, false, default(Color), null, null);
			}
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x0010BB73 File Offset: 0x00109D73
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x0010BB78 File Offset: 0x00109D78
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
			Rect winRect = new Rect(leftX, bottomY - 90f, 200f, 90f);
			DrawStyleDef style = Find.DesignatorManager.SelectedStyle;
			DrawStyleCategoryDef drawStyleCategory = this.DrawStyleCategory;
			List<DrawStyleDef> list = (drawStyleCategory != null) ? drawStyleCategory.styles : null;
			if (style == null || list == null || list.Count <= 1)
			{
				return;
			}
			Find.WindowStack.ImmediateWindow(415111, winRect, WindowLayer.GameUI, delegate
			{
				using (new TextBlock(GameFont.Medium, TextAnchor.MiddleCenter))
				{
					Rect winRect = winRect;
					winRect.x = 0f;
					winRect.y = 0f;
					Rect rect = winRect.MiddlePartPixels(48f, 48f);
					if (Widgets.ButtonImage(rect, style.uiIcon, true, null))
					{
						List<FloatMenuOption> list2 = new List<FloatMenuOption>();
						foreach (DrawStyleDef drawStyleDef in this.DrawStyleCategory.styles)
						{
							DrawStyleDef lDef = drawStyleDef;
							list2.Add(new FloatMenuOption(drawStyleDef.LabelCap, delegate()
							{
								Find.DesignatorManager.SelectedStyle = lDef;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list2));
					}
					Rect rect2 = winRect.MiddlePartPixels(60f, 60f);
					rect2.x = rect.xMin - 16f - rect2.width;
					Rect rect3 = rect;
					rect3.x = rect2.xMin + 27f;
					rect3.xMax = rect2.xMax + 27f;
					Rect butRect = rect2;
					butRect.x = rect.xMax + 16f;
					Rect rect4 = rect;
					rect4.x = butRect.xMin - 27f;
					rect4.xMax = butRect.xMax - 27f;
					if (Widgets.ButtonImage(rect2, TexUI.ConcaveArrowTexLeft, true, null))
					{
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						Find.DesignatorManager.PreviousDrawStyle();
						Event.current.Use();
					}
					if (!SteamDeck.IsSteamDeck)
					{
						Widgets.Label(rect3, KeyBindingDefOf.Designator_PreviousDrawStyle.MainKeyLabel);
					}
					if (Widgets.ButtonImage(butRect, TexUI.ConcaveArrowTexRight, true, null))
					{
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						Find.DesignatorManager.AdvanceDrawStyle();
						Event.current.Use();
					}
					if (!SteamDeck.IsSteamDeck)
					{
						Widgets.Label(rect4, KeyBindingDefOf.Designator_NextDrawStyle.MainKeyLabel);
					}
				}
			}, true, false, 1f, null, false);
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x0010BC12 File Offset: 0x00109E12
		public virtual void SelectedUpdate()
		{
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x0010BC14 File Offset: 0x00109E14
		public virtual void SelectedProcessInput(Event ev)
		{
			if (KeyBindingDefOf.Designator_NextDrawStyle.KeyDownEvent)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Find.DesignatorManager.AdvanceDrawStyle();
			}
			if (KeyBindingDefOf.Designator_PreviousDrawStyle.KeyDownEvent)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Find.DesignatorManager.PreviousDrawStyle();
			}
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x0010BC63 File Offset: 0x00109E63
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x0010BC66 File Offset: 0x00109E66
		public virtual void Selected()
		{
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x0010BC68 File Offset: 0x00109E68
		public virtual void Deselected()
		{
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x0010BC6A File Offset: 0x00109E6A
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		// Token: 0x04001DEA RID: 7658
		protected bool useMouseIcon;

		// Token: 0x04001DEB RID: 7659
		public bool isOrder;

		// Token: 0x04001DEC RID: 7660
		public SoundDef soundDragSustain;

		// Token: 0x04001DED RID: 7661
		public SoundDef soundDragChanged;

		// Token: 0x04001DEE RID: 7662
		public SoundDef soundSucceeded;

		// Token: 0x04001DEF RID: 7663
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		// Token: 0x04001DF0 RID: 7664
		protected bool hasDesignateAllFloatMenuOption;

		// Token: 0x04001DF1 RID: 7665
		protected string designateAllLabel;

		// Token: 0x04001DF2 RID: 7666
		protected bool showReverseDesignatorDisabledReason;

		// Token: 0x04001DF3 RID: 7667
		protected DesignationDef[] removeAllOtherDesignationDefs;

		// Token: 0x04001DF4 RID: 7668
		private string cachedTutorTagSelect;

		// Token: 0x04001DF5 RID: 7669
		private string cachedTutorTagDesignate;

		// Token: 0x04001DF6 RID: 7670
		protected string cachedHighlightTag;

		// Token: 0x04001DF7 RID: 7671
		private List<Designation> tmpAllDesignations = new List<Designation>();

		// Token: 0x04001DF8 RID: 7672
		private const float IconButtonSize = 48f;

		// Token: 0x04001DF9 RID: 7673
		private const float IconButtonGap = 16f;

		// Token: 0x04001DFA RID: 7674
		private const float StyleButtonSize = 60f;

		// Token: 0x04001DFB RID: 7675
		private const float StyleButtonLabelOffset = 27f;
	}
}
