using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE2 RID: 6882
	public static class BillDialogUtility
	{
		// Token: 0x0600AFCA RID: 45002 RVA: 0x003DF2A0 File Offset: 0x003DD4A0
		public static IEnumerable<Widgets.DropdownMenuElement<Pawn>> GetPawnRestrictionOptionsForBill(Bill bill, Func<Pawn, bool> pawnValidator = null)
		{
			BillDialogUtility.<GetPawnRestrictionOptionsForBill>d__0 <GetPawnRestrictionOptionsForBill>d__ = new BillDialogUtility.<GetPawnRestrictionOptionsForBill>d__0(-2);
			<GetPawnRestrictionOptionsForBill>d__.<>3__bill = bill;
			<GetPawnRestrictionOptionsForBill>d__.<>3__pawnValidator = pawnValidator;
			return <GetPawnRestrictionOptionsForBill>d__;
		}
	}
}
