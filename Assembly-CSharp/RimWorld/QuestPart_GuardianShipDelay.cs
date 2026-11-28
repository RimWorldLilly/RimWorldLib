using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C5 RID: 4293
	public class QuestPart_GuardianShipDelay : QuestPartActivable
	{
		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06006B4D RID: 27469 RVA: 0x0025A566 File Offset: 0x00258766
		public int TicksLeft
		{
			get
			{
				if (base.State != QuestPartState.Enabled)
				{
					return 0;
				}
				return this.delayTicks - this.ticksPassed;
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06006B4E RID: 27470 RVA: 0x0025A580 File Offset: 0x00258780
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_GuardianShipDelay.<get_QuestLookTargets>d__7 <get_QuestLookTargets>d__ = new QuestPart_GuardianShipDelay.<get_QuestLookTargets>d__7(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006B4F RID: 27471 RVA: 0x0025A590 File Offset: 0x00258790
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.ticksPassed = 0;
		}

		// Token: 0x06006B50 RID: 27472 RVA: 0x0025A5A0 File Offset: 0x002587A0
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (!this.pawn.DestroyedOrNull() && !this.pawn.Suspended)
			{
				this.ticksPassed++;
				if (this.ticksPassed >= this.delayTicks)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x06006B51 RID: 27473 RVA: 0x0025A5EF File Offset: 0x002587EF
		public override IEnumerable<Gizmo> ExtraGizmos(ISelectable target)
		{
			QuestPart_GuardianShipDelay.<ExtraGizmos>d__10 <ExtraGizmos>d__ = new QuestPart_GuardianShipDelay.<ExtraGizmos>d__10(-2);
			<ExtraGizmos>d__.<>4__this = this;
			<ExtraGizmos>d__.<>3__target = target;
			return <ExtraGizmos>d__;
		}

		// Token: 0x06006B52 RID: 27474 RVA: 0x0025A606 File Offset: 0x00258806
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x06006B53 RID: 27475 RVA: 0x0025A643 File Offset: 0x00258843
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.pawn = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.RandomElement<Pawn>();
			this.delayTicks = Rand.RangeInclusive(833, 2500);
		}

		// Token: 0x04004437 RID: 17463
		public Pawn pawn;

		// Token: 0x04004438 RID: 17464
		public int delayTicks;

		// Token: 0x04004439 RID: 17465
		public int ticksPassed;

		// Token: 0x0400443A RID: 17466
		private Gizmo gizmo;
	}
}
