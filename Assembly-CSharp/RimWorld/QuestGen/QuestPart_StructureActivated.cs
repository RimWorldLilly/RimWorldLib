using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022A3 RID: 8867
	public class QuestPart_StructureActivated : QuestPart
	{
		// Token: 0x0600DAE5 RID: 56037 RVA: 0x0051892E File Offset: 0x00516B2E
		public QuestPart_StructureActivated()
		{
		}

		// Token: 0x0600DAE6 RID: 56038 RVA: 0x00518936 File Offset: 0x00516B36
		public QuestPart_StructureActivated(int structureCount, string activatedSignal, string destroyedSignal, string outSignal, int stage)
		{
			this.structureCount = structureCount;
			this.activatedSignal = activatedSignal;
			this.destroyedSignal = destroyedSignal;
			this.outSignal = outSignal;
			this.stage = stage;
		}

		// Token: 0x0600DAE7 RID: 56039 RVA: 0x00518964 File Offset: 0x00516B64
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Thing thing;
			if ((signal.tag == this.activatedSignal || signal.tag == this.destroyedSignal) && signal.args.TryGetArg<Thing>("SUBJECT", out thing))
			{
				Pawn arg;
				signal.args.TryGetArg<Pawn>("ACTIVATOR", out arg);
				this.activatedStructureCount++;
				TaggedString t = "StructureActivatedMessage".Translate(arg.Named("PAWN"), thing.Named("STRUCTURE"));
				string t2;
				if (this.structureCount <= 1)
				{
					t2 = ".";
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.activatedStructureCount);
					defaultInterpolatedStringHandler.AppendLiteral("/");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.structureCount);
					defaultInterpolatedStringHandler.AppendLiteral(").");
					t2 = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				Messages.Message(t + t2, thing, MessageTypeDefOf.PositiveEvent, true);
				if (this.activatedStructureCount == this.structureCount)
				{
					Signal signal2 = new Signal(this.outSignal, false);
					signal2.args.Add(arg.Named("ACTIVATOR"));
					Find.SignalManager.SendSignal(signal2);
					switch (this.stage)
					{
					case 1:
						Find.LetterStack.ReceiveLetter("VoidAwakeningStageOneStructuresActivatedLabel".Translate(), "VoidAwakeningStageOneStructuresActivatedText".Translate(), LetterDefOf.NegativeEvent, null, 0, true);
						return;
					case 2:
						Find.LetterStack.ReceiveLetter("VoidAwakeningStageTwoStructuresActivatedLabel".Translate(), "VoidAwakeningStageTwoStructuresActivatedText".Translate(), LetterDefOf.NegativeEvent, null, 0, true);
						break;
					case 3:
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x0600DAE8 RID: 56040 RVA: 0x00518B1C File Offset: 0x00516D1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.activatedSignal, "studiedSignal", null, false);
			Scribe_Values.Look<string>(ref this.destroyedSignal, "destroyedSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<int>(ref this.structureCount, "structureCount", 0, false);
			Scribe_Values.Look<int>(ref this.activatedStructureCount, "studiedStructureCount", 0, false);
			Scribe_Values.Look<int>(ref this.stage, "wave", 0, false);
		}

		// Token: 0x04008C1D RID: 35869
		private string activatedSignal;

		// Token: 0x04008C1E RID: 35870
		private string destroyedSignal;

		// Token: 0x04008C1F RID: 35871
		private string outSignal;

		// Token: 0x04008C20 RID: 35872
		private int structureCount;

		// Token: 0x04008C21 RID: 35873
		private int activatedStructureCount;

		// Token: 0x04008C22 RID: 35874
		private int stage;
	}
}
