using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA9 RID: 7593
	public abstract class PawnColumnWorker_Designator : PawnColumnWorker_Checkbox
	{
		// Token: 0x170024AC RID: 9388
		// (get) Token: 0x0600C56B RID: 50539
		protected abstract DesignationDef DesignationType { get; }

		// Token: 0x0600C56C RID: 50540 RVA: 0x00488A69 File Offset: 0x00486C69
		protected virtual void Notify_DesignationAdded(Pawn pawn)
		{
		}

		// Token: 0x0600C56D RID: 50541 RVA: 0x00488A6B File Offset: 0x00486C6B
		protected override bool GetValue(Pawn pawn)
		{
			return this.GetDesignation(pawn) != null;
		}

		// Token: 0x0600C56E RID: 50542 RVA: 0x00488A78 File Offset: 0x00486C78
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			if (value == this.GetValue(pawn))
			{
				return;
			}
			if (table.SortingBy == this.def)
			{
				table.SetDirty();
			}
			if (!value)
			{
				Designation designation = this.GetDesignation(pawn);
				if (designation != null)
				{
					pawn.MapHeld.designationManager.RemoveDesignation(designation);
				}
				return;
			}
			string title;
			if (this.ShouldConfirmDesignation(pawn, out title))
			{
				Find.WindowStack.Add(new Dialog_Confirm(title, delegate()
				{
					this.DesignationConfirmed(pawn);
				}));
				return;
			}
			this.DesignationConfirmed(pawn);
		}

		// Token: 0x0600C56F RID: 50543 RVA: 0x00488B20 File Offset: 0x00486D20
		private void DesignationConfirmed(Pawn pawn)
		{
			pawn.MapHeld.designationManager.AddDesignation(new Designation(pawn, this.DesignationType, null));
			this.Notify_DesignationAdded(pawn);
		}

		// Token: 0x0600C570 RID: 50544 RVA: 0x00488B4B File Offset: 0x00486D4B
		protected virtual bool ShouldConfirmDesignation(Pawn pawn, out string title)
		{
			title = "";
			return false;
		}

		// Token: 0x0600C571 RID: 50545 RVA: 0x00488B58 File Offset: 0x00486D58
		private Designation GetDesignation(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return null;
			}
			return mapHeld.designationManager.DesignationOn(pawn, this.DesignationType);
		}
	}
}
