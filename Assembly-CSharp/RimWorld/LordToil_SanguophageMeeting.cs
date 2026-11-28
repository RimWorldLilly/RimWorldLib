using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C7E RID: 3198
	public class LordToil_SanguophageMeeting : LordToil
	{
		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x06005AAC RID: 23212 RVA: 0x00215BCC File Offset: 0x00213DCC
		private LordToilData_SanguophageMeeting Data
		{
			get
			{
				return (LordToilData_SanguophageMeeting)this.data;
			}
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x00215BD9 File Offset: 0x00213DD9
		public LordToil_SanguophageMeeting(IntVec3 meetingSpot, int meetingDurationTicks)
		{
			this.meetingSpot = meetingSpot;
			this.meetingDurationTicks = meetingDurationTicks;
			this.data = new LordToilData_SanguophageMeeting();
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x00215BFA File Offset: 0x00213DFA
		public override void Init()
		{
			base.Init();
			Messages.Message("SanguophagesBegunMeeting".Translate(), this.lord.ownedPawns, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x00215C2C File Offset: 0x00213E2C
		public override void LordToilTick()
		{
			this.Data.ticksInMeeting++;
			TargetInfo targetInfo = new TargetInfo(this.meetingSpot + new IntVec3(0, 0, -2), base.Map, false);
			if (this.progressBar == null)
			{
				this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
			}
			this.progressBar.EffectTick(targetInfo, TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
			if (mote != null)
			{
				mote.progress = Mathf.Clamp01((float)this.Data.ticksInMeeting / (float)this.meetingDurationTicks);
			}
			if (this.torchCached == null)
			{
				this.torchCached = GenClosest.ClosestThing_Regionwise_ReachablePrioritized(targetInfo.Cell, base.Map, ThingRequest.ForDef(ThingDefOf.SanguphageMeetingTorch), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), 3f, null, null, 24, 30, false);
			}
			TargetInfo a = (this.torchCached != null) ? new TargetInfo(this.torchCached.Position, base.Map, false) : targetInfo;
			if (this.meetingEffect == null)
			{
				this.meetingEffect = EffecterDefOf.SanguophageMeeting.Spawn(a.Cell, base.Map, 1f);
			}
			this.meetingEffect.EffectTick(a, TargetInfo.Invalid);
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x00215D76 File Offset: 0x00213F76
		public override void Cleanup()
		{
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			Effecter effecter2 = this.meetingEffect;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.progressBar = null;
			this.meetingEffect = null;
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x00215DA8 File Offset: 0x00213FA8
		public override void UpdateAllDuties()
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.SocialMeeting, this.meetingSpot, -1f);
				pawnDuty.spectateRect = CellRect.SingleCell(this.meetingSpot);
				pawnDuty.spectateDistance = new IntRange(1, 2);
				pawnDuty.locomotion = LocomotionUrgency.Walk;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04003403 RID: 13315
		private IntVec3 meetingSpot;

		// Token: 0x04003404 RID: 13316
		private int meetingDurationTicks;

		// Token: 0x04003405 RID: 13317
		private Effecter progressBar;

		// Token: 0x04003406 RID: 13318
		private Effecter meetingEffect;

		// Token: 0x04003407 RID: 13319
		private Thing torchCached;
	}
}
