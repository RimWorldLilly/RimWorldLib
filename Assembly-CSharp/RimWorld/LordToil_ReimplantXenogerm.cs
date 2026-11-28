using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CF0 RID: 3312
	public class LordToil_ReimplantXenogerm : LordToil
	{
		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06005D7C RID: 23932 RVA: 0x00223E78 File Offset: 0x00222078
		public override bool ShouldFail
		{
			get
			{
				return this.Data.target == null;
			}
		}

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06005D7D RID: 23933 RVA: 0x00223E88 File Offset: 0x00222088
		public LordToilData_ReimplantXenogerm Data
		{
			get
			{
				return (LordToilData_ReimplantXenogerm)this.data;
			}
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x00223E95 File Offset: 0x00222095
		public LordToil_ReimplantXenogerm(IntVec3 spot)
		{
			this.data = new LordToilData_ReimplantXenogerm
			{
				waitSpot = spot
			};
		}

		// Token: 0x06005D7F RID: 23935 RVA: 0x00223EAF File Offset: 0x002220AF
		public override void Init()
		{
			this.SetTarget();
		}

		// Token: 0x06005D80 RID: 23936 RVA: 0x00223EB8 File Offset: 0x002220B8
		private void SetTarget()
		{
			LordToilData_ReimplantXenogerm data = this.Data;
			if (!this.lord.ownedPawns.TryRandomElementByWeight((Pawn x) => (float)((!x.Destroyed && !x.Downed && !x.health.hediffSet.HasHediff(HediffDefOf.XenogermReplicating, false)) ? 1 : 0), out data.target))
			{
				this.lord.ownedPawns.TryRandomElementByWeight((Pawn x) => (float)((!x.Destroyed && !x.Downed) ? 1 : 0), out data.target);
			}
			if (data.target != null)
			{
				Hediff firstHediffOfDef = data.target.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.XenogermReplicating, false);
				if (firstHediffOfDef != null)
				{
					data.target.health.RemoveHediff(firstHediffOfDef);
				}
				Find.LetterStack.ReceiveLetter("LetterLabelSanguophageWaitingToReimplant".Translate(), "LetterTextSanguophageWaitingToReimplant".Translate(data.target.Named("PAWN"), 15000.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")), LetterDefOf.PositiveEvent, data.target, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06005D81 RID: 23937 RVA: 0x00223FD4 File Offset: 0x002221D4
		public override void UpdateAllDuties()
		{
			LordToilData_ReimplantXenogerm data = this.Data;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i] == data.target)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Idle, data.waitSpot, -1f);
				}
				else
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.WanderClose_NoNeeds, data.waitSpot, 10f);
				}
			}
		}

		// Token: 0x06005D82 RID: 23938 RVA: 0x0022408E File Offset: 0x0022228E
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			if (pawn == this.Data.target)
			{
				pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
			}
		}

		// Token: 0x06005D83 RID: 23939 RVA: 0x002240B1 File Offset: 0x002222B1
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (p == this.Data.target)
			{
				this.SetTarget();
			}
		}

		// Token: 0x06005D84 RID: 23940 RVA: 0x002240C7 File Offset: 0x002222C7
		public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn requester, Pawn current)
		{
			LordToil_ReimplantXenogerm.<ExtraFloatMenuOptions>d__11 <ExtraFloatMenuOptions>d__ = new LordToil_ReimplantXenogerm.<ExtraFloatMenuOptions>d__11(-2);
			<ExtraFloatMenuOptions>d__.<>4__this = this;
			<ExtraFloatMenuOptions>d__.<>3__requester = requester;
			<ExtraFloatMenuOptions>d__.<>3__current = current;
			return <ExtraFloatMenuOptions>d__;
		}

		// Token: 0x04003515 RID: 13589
		private const float WanderRadius = 10f;
	}
}
