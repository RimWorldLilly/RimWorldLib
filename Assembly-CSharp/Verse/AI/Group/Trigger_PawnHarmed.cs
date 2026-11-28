using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000974 RID: 2420
	public class Trigger_PawnHarmed : Trigger
	{
		// Token: 0x06004990 RID: 18832 RVA: 0x001CAA1F File Offset: 0x001C8C1F
		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null, DutyDef skipDuty = null, int? minTicks = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
			this.skipDuty = skipDuty;
			this.minTicks = minTicks;
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x001CAA57 File Offset: 0x001C8C57
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			base.SourceToilBecameActive(transition, previousToil);
			if (this.minTicks != null)
			{
				this.minTick = new int?(GenTicks.TicksGame + this.minTicks.Value);
			}
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x001CAA8C File Offset: 0x001C8C8C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (!Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				return false;
			}
			if (this.requireInstigatorWithFaction && (signal.dinfo.Instigator == null || signal.dinfo.Instigator.Faction == null))
			{
				return false;
			}
			if (this.requireInstigatorWithSpecificFaction != null && (signal.dinfo.Instigator == null || signal.dinfo.Instigator.Faction != this.requireInstigatorWithSpecificFaction))
			{
				return false;
			}
			Pawn pawn = signal.dinfo.IntendedTarget as Pawn;
			if (pawn != null)
			{
				Pawn_MindState mindState = pawn.mindState;
				DutyDef dutyDef;
				if (mindState == null)
				{
					dutyDef = null;
				}
				else
				{
					PawnDuty duty = mindState.duty;
					dutyDef = ((duty != null) ? duty.def : null);
				}
				if (dutyDef == this.skipDuty)
				{
					return false;
				}
			}
			return (this.minTick == null || GenTicks.TicksGame >= this.minTick.Value) && Rand.Value < this.chance;
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x001CAB6C File Offset: 0x001C8D6C
		public static bool SignalIsHarm(TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnDamaged)
			{
				return signal.dinfo.Def.ExternalViolenceFor(signal.Pawn);
			}
			if (signal.type == TriggerSignalType.PawnLost)
			{
				return signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.Incapped || signal.condition == PawnLostCondition.Killed;
			}
			return signal.type == TriggerSignalType.PawnArrestAttempted;
		}

		// Token: 0x04002F36 RID: 12086
		public float chance = 1f;

		// Token: 0x04002F37 RID: 12087
		public bool requireInstigatorWithFaction;

		// Token: 0x04002F38 RID: 12088
		public Faction requireInstigatorWithSpecificFaction;

		// Token: 0x04002F39 RID: 12089
		public DutyDef skipDuty;

		// Token: 0x04002F3A RID: 12090
		public int? minTicks;

		// Token: 0x04002F3B RID: 12091
		private int? minTick;
	}
}
