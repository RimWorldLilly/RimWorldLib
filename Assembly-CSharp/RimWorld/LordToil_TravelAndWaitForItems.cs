using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D04 RID: 3332
	public class LordToil_TravelAndWaitForItems : LordToil_Travel, IWaitForItemsLordToil
	{
		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06005DD7 RID: 24023 RVA: 0x00225BA5 File Offset: 0x00223DA5
		public bool HasAllRequestedItems
		{
			get
			{
				return this.CountRemaining <= 0;
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x06005DD8 RID: 24024 RVA: 0x00225BB3 File Offset: 0x00223DB3
		public int CountRemaining
		{
			get
			{
				return GiveItemsToPawnUtility.GetCountRemaining(this.target, this.requestedThingDef, this.requestedThingCount);
			}
		}

		// Token: 0x06005DD9 RID: 24025 RVA: 0x00225BCC File Offset: 0x00223DCC
		public LordToil_TravelAndWaitForItems(IntVec3 dest, Pawn target, ThingDef thingDef, int amount) : base(dest)
		{
			this.target = target;
			this.requestedThingDef = thingDef;
			this.requestedThingCount = amount;
		}

		// Token: 0x06005DDA RID: 24026 RVA: 0x00225BEB File Offset: 0x00223DEB
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			if (pawn == this.target)
			{
				pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
			}
		}

		// Token: 0x06005DDB RID: 24027 RVA: 0x00225C09 File Offset: 0x00223E09
		public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn requester, Pawn current)
		{
			LordToil_TravelAndWaitForItems.<ExtraFloatMenuOptions>d__9 <ExtraFloatMenuOptions>d__ = new LordToil_TravelAndWaitForItems.<ExtraFloatMenuOptions>d__9(-2);
			<ExtraFloatMenuOptions>d__.<>4__this = this;
			<ExtraFloatMenuOptions>d__.<>3__requester = requester;
			<ExtraFloatMenuOptions>d__.<>3__current = current;
			return <ExtraFloatMenuOptions>d__;
		}

		// Token: 0x04003541 RID: 13633
		public Pawn target;

		// Token: 0x04003542 RID: 13634
		public ThingDef requestedThingDef;

		// Token: 0x04003543 RID: 13635
		public int requestedThingCount;
	}
}
