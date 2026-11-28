using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5D RID: 7261
	public class Designator_Slaughter : Designator
	{
		// Token: 0x1700217C RID: 8572
		// (get) Token: 0x0600B943 RID: 47427 RVA: 0x004155D4 File Offset: 0x004137D4
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x1700217D RID: 8573
		// (get) Token: 0x0600B944 RID: 47428 RVA: 0x004155DB File Offset: 0x004137DB
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B945 RID: 47429 RVA: 0x004155E4 File Offset: 0x004137E4
		public Designator_Slaughter()
		{
			this.defaultLabel = "DesignatorSlaughter".Translate();
			this.defaultDesc = "DesignatorSlaughterDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Slaughter", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Slaughter;
			this.hotKey = KeyBindingDefOf.Misc7;
		}

		// Token: 0x0600B946 RID: 47430 RVA: 0x00415670 File Offset: 0x00413870
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.SlaughterablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateSlaughterable".Translate();
			}
			return true;
		}

		// Token: 0x0600B947 RID: 47431 RVA: 0x004156AC File Offset: 0x004138AC
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.SlaughterablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0600B948 RID: 47432 RVA: 0x004156FC File Offset: 0x004138FC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.IsAnimal && pawn.Faction == Faction.OfPlayer && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null && !pawn.InAggroMentalState)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600B949 RID: 47433 RVA: 0x00415758 File Offset: 0x00413958
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			this.justDesignated.Add((Pawn)t);
			Designation designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
		}

		// Token: 0x0600B94A RID: 47434 RVA: 0x004157C4 File Offset: 0x004139C4
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.ShowDesignationWarnings(this.justDesignated[i]);
			}
			this.justDesignated.Clear();
		}

		// Token: 0x0600B94B RID: 47435 RVA: 0x0041580A File Offset: 0x00413A0A
		private IEnumerable<Pawn> SlaughterablesInCell(IntVec3 c)
		{
			Designator_Slaughter.<SlaughterablesInCell>d__11 <SlaughterablesInCell>d__ = new Designator_Slaughter.<SlaughterablesInCell>d__11(-2);
			<SlaughterablesInCell>d__.<>4__this = this;
			<SlaughterablesInCell>d__.<>3__c = c;
			return <SlaughterablesInCell>d__;
		}

		// Token: 0x0600B94C RID: 47436 RVA: 0x00415821 File Offset: 0x00413A21
		private void ShowDesignationWarnings(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
			SlaughterDesignatorUtility.CheckWarnAboutVeneratedAnimal(pawn);
		}

		// Token: 0x040069C5 RID: 27077
		private readonly List<Pawn> justDesignated = new List<Pawn>();
	}
}
