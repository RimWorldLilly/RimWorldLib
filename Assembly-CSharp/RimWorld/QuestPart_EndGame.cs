using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BE RID: 4286
	public class QuestPart_EndGame : QuestPart
	{
		// Token: 0x06006B31 RID: 27441 RVA: 0x00259E8C File Offset: 0x0025808C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && !ShipCountdown.CountingDown)
			{
				if (!Find.TickManager.Paused)
				{
					Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
				}
				List<Pawn> list;
				if (!signal.args.TryGetArg<List<Pawn>>("SENTCOLONISTS", out list))
				{
					list = null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						stringBuilder.AppendLine("   " + list[i].LabelCap);
					}
					Find.StoryWatcher.statsRecord.colonistsLaunched += list.Count;
				}
				ShipCountdown.InitiateCountdown(GameVictoryUtility.MakeEndCredits(this.introText, this.endingText, stringBuilder.ToString(), "GameOverColonistsEscaped", null));
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (!list[j].Destroyed)
						{
							list[j].Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x06006B32 RID: 27442 RVA: 0x00259F92 File Offset: 0x00258192
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.introText, "introText", null, false);
			Scribe_Values.Look<string>(ref this.endingText, "endingText", null, false);
		}

		// Token: 0x06006B33 RID: 27443 RVA: 0x00259FD0 File Offset: 0x002581D0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04004422 RID: 17442
		public string inSignal;

		// Token: 0x04004423 RID: 17443
		public string introText;

		// Token: 0x04004424 RID: 17444
		public string endingText;
	}
}
