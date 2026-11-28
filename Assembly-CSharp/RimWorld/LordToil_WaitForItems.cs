using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D03 RID: 3331
	public class LordToil_WaitForItems : LordToil, IWaitForItemsLordToil
	{
		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x06005DD1 RID: 24017 RVA: 0x00225ABD File Offset: 0x00223CBD
		public bool HasAllRequestedItems
		{
			get
			{
				return this.CountRemaining <= 0;
			}
		}

		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x06005DD2 RID: 24018 RVA: 0x00225ACB File Offset: 0x00223CCB
		public int CountRemaining
		{
			get
			{
				return GiveItemsToPawnUtility.GetCountRemaining(this.target, this.requestedThingDef, this.requestedThingCount);
			}
		}

		// Token: 0x06005DD3 RID: 24019 RVA: 0x00225AE4 File Offset: 0x00223CE4
		public LordToil_WaitForItems(Pawn target, ThingDef thingDef, int amount, IntVec3 waitSpot)
		{
			this.waitSpot = waitSpot;
			this.target = target;
			this.requestedThingDef = thingDef;
			this.requestedThingCount = amount;
		}

		// Token: 0x06005DD4 RID: 24020 RVA: 0x00225B0C File Offset: 0x00223D0C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.WanderClose_NoNeeds, this.waitSpot, 10f);
			}
		}

		// Token: 0x06005DD5 RID: 24021 RVA: 0x00225B69 File Offset: 0x00223D69
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			if (pawn == this.target)
			{
				pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
			}
		}

		// Token: 0x06005DD6 RID: 24022 RVA: 0x00225B87 File Offset: 0x00223D87
		public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn requester, Pawn current)
		{
			LordToil_WaitForItems.<ExtraFloatMenuOptions>d__12 <ExtraFloatMenuOptions>d__ = new LordToil_WaitForItems.<ExtraFloatMenuOptions>d__12(-2);
			<ExtraFloatMenuOptions>d__.<>4__this = this;
			<ExtraFloatMenuOptions>d__.<>3__requester = requester;
			<ExtraFloatMenuOptions>d__.<>3__current = current;
			return <ExtraFloatMenuOptions>d__;
		}

		// Token: 0x0400353C RID: 13628
		public IntVec3 waitSpot;

		// Token: 0x0400353D RID: 13629
		public Pawn target;

		// Token: 0x0400353E RID: 13630
		public ThingDef requestedThingDef;

		// Token: 0x0400353F RID: 13631
		public int requestedThingCount;

		// Token: 0x04003540 RID: 13632
		private const float WanderRadius = 10f;
	}
}
