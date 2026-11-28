using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AF RID: 4271
	public class QuestPart_CameraJump : QuestPart
	{
		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06006AD9 RID: 27353 RVA: 0x0025869D File Offset: 0x0025689D
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_CameraJump.<get_QuestLookTargets>d__5 <get_QuestLookTargets>d__ = new QuestPart_CameraJump.<get_QuestLookTargets>d__5(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006ADA RID: 27354 RVA: 0x002586B0 File Offset: 0x002568B0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				LookTargets lookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !lookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets);
				}
				if (lookTargets.IsValid())
				{
					if (this.select)
					{
						CameraJumper.TryJumpAndSelect(lookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
						return;
					}
					CameraJumper.TryJump(lookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
				}
			}
		}

		// Token: 0x06006ADB RID: 27355 RVA: 0x0025872C File Offset: 0x0025692C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_Values.Look<bool>(ref this.select, "select", true, false);
		}

		// Token: 0x06006ADC RID: 27356 RVA: 0x0025878C File Offset: 0x0025698C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.lookTargets = Find.Maps.SelectMany((Map x) => x.mapPawns.FreeColonistsSpawned).RandomElementWithFallback(null);
		}

		// Token: 0x040043ED RID: 17389
		public string inSignal;

		// Token: 0x040043EE RID: 17390
		public LookTargets lookTargets;

		// Token: 0x040043EF RID: 17391
		public bool getLookTargetsFromSignal = true;

		// Token: 0x040043F0 RID: 17392
		public bool select = true;
	}
}
