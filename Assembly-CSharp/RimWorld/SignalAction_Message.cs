using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B08 RID: 6920
	public class SignalAction_Message : SignalAction
	{
		// Token: 0x0600B0E9 RID: 45289 RVA: 0x003E54A8 File Offset: 0x003E36A8
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag != this.signalTag)
			{
				return;
			}
			if (this.allMustBeAsleep)
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.lookTargets.targets)
				{
					if (!globalTargetInfo.Pawn.DestroyedOrNull() && globalTargetInfo.Pawn.Spawned && !globalTargetInfo.Pawn.Dead && globalTargetInfo.Pawn.Awake())
					{
						return;
					}
				}
			}
			base.Notify_SignalReceived(signal);
		}

		// Token: 0x0600B0EA RID: 45290 RVA: 0x003E5558 File Offset: 0x003E3758
		protected override void DoAction(SignalArgs args)
		{
			Messages.Message(this.message, this.lookTargets, this.messageType ?? MessageTypeDefOf.NeutralEvent, this.historical);
		}

		// Token: 0x0600B0EB RID: 45291 RVA: 0x003E5580 File Offset: 0x003E3780
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
			Scribe_Values.Look<bool>(ref this.historical, "historical", false, false);
			Scribe_Values.Look<bool>(ref this.allMustBeAsleep, "allMustBeAsleep", false, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageType, "messageType");
		}

		// Token: 0x04006625 RID: 26149
		public string message;

		// Token: 0x04006626 RID: 26150
		public bool historical = true;

		// Token: 0x04006627 RID: 26151
		public MessageTypeDef messageType;

		// Token: 0x04006628 RID: 26152
		public LookTargets lookTargets;

		// Token: 0x04006629 RID: 26153
		public bool allMustBeAsleep;
	}
}
