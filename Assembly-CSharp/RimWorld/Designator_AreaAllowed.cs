using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C30 RID: 7216
	public abstract class Designator_AreaAllowed : Designator_Cells
	{
		// Token: 0x17002138 RID: 8504
		// (get) Token: 0x0600B815 RID: 47125 RVA: 0x0040FB50 File Offset: 0x0040DD50
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002139 RID: 8505
		// (get) Token: 0x0600B816 RID: 47126 RVA: 0x0040FB53 File Offset: 0x0040DD53
		public static Area SelectedArea
		{
			get
			{
				return Designator_AreaAllowed.selectedArea;
			}
		}

		// Token: 0x1700213A RID: 8506
		// (get) Token: 0x0600B817 RID: 47127 RVA: 0x0040FB5A File Offset: 0x0040DD5A
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Areas;
			}
		}

		// Token: 0x0600B818 RID: 47128 RVA: 0x0040FB61 File Offset: 0x0040DD61
		public Designator_AreaAllowed(DesignateMode mode)
		{
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
		}

		// Token: 0x0600B819 RID: 47129 RVA: 0x0040FB86 File Offset: 0x0040DD86
		public static void ClearSelectedArea()
		{
			Designator_AreaAllowed.selectedArea = null;
		}

		// Token: 0x0600B81A RID: 47130 RVA: 0x0040FB8E File Offset: 0x0040DD8E
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
			if (Designator_AreaAllowed.selectedArea != null && Find.WindowStack.FloatMenu == null)
			{
				Designator_AreaAllowed.selectedArea.MarkForDraw();
			}
		}

		// Token: 0x0600B81B RID: 47131 RVA: 0x0040FBB4 File Offset: 0x0040DDB4
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			if (Designator_AreaAllowed.selectedArea != null)
			{
				base.ProcessInput(ev);
			}
			AreaUtility.MakeAllowedAreaListFloatMenu(delegate(Area a)
			{
				Designator_AreaAllowed.selectedArea = a;
				this.<>n__0(ev);
			}, false, true, base.Map);
		}

		// Token: 0x0600B81C RID: 47132 RVA: 0x0040FC0A File Offset: 0x0040DE0A
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AllowedAreas, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x040069A0 RID: 27040
		public static Area selectedArea;
	}
}
