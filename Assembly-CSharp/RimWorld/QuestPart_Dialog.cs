using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BA RID: 4282
	public class QuestPart_Dialog : QuestPart
	{
		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06006B17 RID: 27415 RVA: 0x00259641 File Offset: 0x00257841
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_Dialog.<get_QuestLookTargets>d__12 <get_QuestLookTargets>d__ = new QuestPart_Dialog.<get_QuestLookTargets>d__12(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06006B18 RID: 27416 RVA: 0x00259651 File Offset: 0x00257851
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_Dialog.<get_InvolvedFactions>d__14 <get_InvolvedFactions>d__ = new QuestPart_Dialog.<get_InvolvedFactions>d__14(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006B19 RID: 27417 RVA: 0x00259664 File Offset: 0x00257864
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				DiaNode diaNode = new DiaNode(signal.args.GetFormattedText(this.text));
				LookTargets resolvedLookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !resolvedLookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out resolvedLookTargets);
				}
				if (resolvedLookTargets.IsValid())
				{
					DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
					diaOption.action = delegate()
					{
						CameraJumper.TryJumpAndSelect(resolvedLookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
					};
					diaOption.resolveTree = true;
					diaNode.options.Add(diaOption);
				}
				if (this.options.Any<QuestPart_Dialog.Option>())
				{
					for (int i = 0; i < this.options.Count; i++)
					{
						int localIndex = i;
						DiaOption diaOption2 = new DiaOption(signal.args.GetFormattedText(this.options[i].text));
						diaOption2.action = delegate()
						{
							Find.SignalManager.SendSignal(new Signal(this.options[localIndex].outSignal, false));
						};
						diaOption2.resolveTree = true;
						diaNode.options.Add(diaOption2);
					}
				}
				else
				{
					DiaOption diaOption3 = new DiaOption("OK".Translate());
					diaOption3.resolveTree = true;
					diaNode.options.Add(diaOption3);
				}
				TaggedString formattedText = signal.args.GetFormattedText(this.title);
				Dialog_NodeTreeWithFactionInfo dialog_NodeTreeWithFactionInfo = new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, true, this.radioMode, formattedText);
				QuestPartDialogCloseAction questPartDialogCloseAction = QuestPartDialogCloseActionRegistry.CloseActionOf(this.closeAction);
				if (questPartDialogCloseAction != null)
				{
					dialog_NodeTreeWithFactionInfo.soundClose = questPartDialogCloseAction.dialogCloseSound;
					dialog_NodeTreeWithFactionInfo.closeAction = questPartDialogCloseAction.dialogCloseAction;
				}
				Find.WindowStack.Add(dialog_NodeTreeWithFactionInfo);
				if (this.addToArchive)
				{
					Find.Archive.Add(new ArchivedDialog(diaNode.text, formattedText, this.relatedFaction));
				}
			}
		}

		// Token: 0x06006B1A RID: 27418 RVA: 0x0025989C File Offset: 0x00257A9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Collections.Look<QuestPart_Dialog.Option>(ref this.options, "options", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<bool>(ref this.addToArchive, "addToArchive", true, false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", false, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<QuestPartDialogCloseAction.CloseActionKey>(ref this.closeAction, "closeAction", QuestPartDialogCloseAction.CloseActionKey.None, false);
		}

		// Token: 0x06006B1B RID: 27419 RVA: 0x0025996C File Offset: 0x00257B6C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.title = "Title";
			this.text = "DEV: Test";
			this.relatedFaction = Faction.OfMechanoids;
			this.addToArchive = false;
			QuestPart_Dialog.Option option = new QuestPart_Dialog.Option();
			option.text = "Option 1";
			option.outSignal = "DebugSignal" + Rand.Int.ToString();
			this.options.Add(option);
			QuestPart_Dialog.Option option2 = new QuestPart_Dialog.Option();
			option2.text = "Option 2";
			option2.outSignal = "DebugSignal" + Rand.Int.ToString();
			this.options.Add(option2);
		}

		// Token: 0x04004410 RID: 17424
		public string inSignal;

		// Token: 0x04004411 RID: 17425
		public string text;

		// Token: 0x04004412 RID: 17426
		public string title;

		// Token: 0x04004413 RID: 17427
		public List<QuestPart_Dialog.Option> options = new List<QuestPart_Dialog.Option>();

		// Token: 0x04004414 RID: 17428
		public Faction relatedFaction;

		// Token: 0x04004415 RID: 17429
		public bool addToArchive = true;

		// Token: 0x04004416 RID: 17430
		public bool radioMode;

		// Token: 0x04004417 RID: 17431
		public bool getLookTargetsFromSignal;

		// Token: 0x04004418 RID: 17432
		public LookTargets lookTargets;

		// Token: 0x04004419 RID: 17433
		public QuestPartDialogCloseAction.CloseActionKey closeAction;

		// Token: 0x02002FFF RID: 12287
		public class Option : IExposable
		{
			// Token: 0x0601221C RID: 74268 RVA: 0x00627A7D File Offset: 0x00625C7D
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.text, "text", null, false);
				Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			}

			// Token: 0x0400C2F5 RID: 49909
			public string text;

			// Token: 0x0400C2F6 RID: 49910
			public string outSignal;
		}
	}
}
