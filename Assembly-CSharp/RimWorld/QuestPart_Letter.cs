using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CE RID: 4302
	public class QuestPart_Letter : QuestPart
	{
		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06006B8B RID: 27531 RVA: 0x0025B863 File Offset: 0x00259A63
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_Letter.<get_QuestLookTargets>d__12 <get_QuestLookTargets>d__ = new QuestPart_Letter.<get_QuestLookTargets>d__12(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06006B8C RID: 27532 RVA: 0x0025B873 File Offset: 0x00259A73
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_Letter.<get_InvolvedFactions>d__14 <get_InvolvedFactions>d__ = new QuestPart_Letter.<get_InvolvedFactions>d__14(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006B8D RID: 27533 RVA: 0x0025B883 File Offset: 0x00259A83
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.letter.relatedFaction == faction)
			{
				this.letter.relatedFaction = null;
			}
		}

		// Token: 0x06006B8E RID: 27534 RVA: 0x0025B8A0 File Offset: 0x00259AA0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!string.IsNullOrEmpty(this.getColonistsFromSignal) && signal.tag == this.getColonistsFromSignal)
			{
				NamedArgument namedArgument;
				if (signal.args.TryGetArg("SUBJECT", out namedArgument))
				{
					this.<Notify_QuestSignalReceived>g__ReadPawns|16_0(namedArgument.arg);
				}
				NamedArgument namedArgument2;
				if (signal.args.TryGetArg("SENT", out namedArgument2))
				{
					this.<Notify_QuestSignalReceived>g__ReadPawns|16_0(namedArgument2.arg);
				}
			}
			if (signal.tag == this.inSignal)
			{
				Letter letter = Gen.MemberwiseClone<Letter>(this.letter);
				letter.ID = Find.UniqueIDsManager.GetNextLetterID();
				ChoiceLetter choiceLetter = letter as ChoiceLetter;
				if (choiceLetter != null)
				{
					choiceLetter.quest = this.quest;
				}
				ChoiceLetter_ChoosePawn choiceLetter_ChoosePawn = letter as ChoiceLetter_ChoosePawn;
				if (choiceLetter_ChoosePawn != null)
				{
					if (this.useColonistsOnMap != null && this.useColonistsOnMap.HasMap)
					{
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(this.useColonistsOnMap.Map.mapPawns.FreeColonists);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
					Caravan caravan;
					if (this.useColonistsFromCaravanArg && signal.args.TryGetArg<Caravan>("CARAVAN", out caravan) && caravan != null)
					{
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(from x in caravan.PawnsListForReading
						where x.IsFreeColonist
						select x);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
					if (!string.IsNullOrEmpty(this.getColonistsFromSignal))
					{
						this.colonistsFromSignal.RemoveAll((Pawn x) => x.Dead);
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(this.colonistsFromSignal);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
				}
				ChoiceLetter_AcceptVisitors choiceLetter_AcceptVisitors = letter as ChoiceLetter_AcceptVisitors;
				if (choiceLetter_AcceptVisitors != null)
				{
					choiceLetter_AcceptVisitors.acceptedSignal = this.acceptedVisitorsSignal;
					if (this.visitors != null)
					{
						choiceLetter_AcceptVisitors.pawns.AddRange(this.visitors);
					}
				}
				LookTargets lookTargets;
				if (this.getLookTargetsFromSignal && !letter.lookTargets.IsValid() && SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets))
				{
					letter.lookTargets = lookTargets;
				}
				letter.Label = signal.args.GetFormattedText(letter.Label);
				ChoiceLetter choiceLetter2 = letter as ChoiceLetter;
				bool flag = true;
				if (choiceLetter2 != null)
				{
					choiceLetter2.title = signal.args.GetFormattedText(choiceLetter2.title);
					choiceLetter2.Text = signal.args.GetFormattedText(choiceLetter2.Text);
					if (choiceLetter2.Text.NullOrEmpty())
					{
						flag = false;
					}
				}
				if (this.filterDeadPawnsFromLookTargets)
				{
					for (int i = letter.lookTargets.targets.Count - 1; i >= 0; i--)
					{
						Thing thing = letter.lookTargets.targets[i].Thing;
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.Dead)
						{
							letter.lookTargets.targets.Remove(thing);
						}
					}
				}
				if (flag)
				{
					Find.LetterStack.ReceiveLetter(letter, null, 0, true);
				}
			}
		}

		// Token: 0x06006B8F RID: 27535 RVA: 0x0025BBF4 File Offset: 0x00259DF4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_References.Look<MapParent>(ref this.useColonistsOnMap, "useColonistsOnMap", false);
			Scribe_Values.Look<bool>(ref this.useColonistsFromCaravanArg, "useColonistsFromCaravanArg", false, false);
			Scribe_Values.Look<string>(ref this.chosenPawnSignal, "chosenPawnSignal", null, false);
			Scribe_Values.Look<bool>(ref this.filterDeadPawnsFromLookTargets, "filterDeadPawnsFromLookTargets", false, false);
			Scribe_Values.Look<string>(ref this.getColonistsFromSignal, "getColonistsFromSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.colonistsFromSignal, "colonistsFromSignal", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.colonistsFromSignal.RemoveAll((Pawn x) => x == null);
			}
			Scribe_Values.Look<string>(ref this.acceptedVisitorsSignal, "acceptedVisitorsSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.visitors, "visitors", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Pawn> list = this.visitors;
				if (list == null)
				{
					return;
				}
				list.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006B90 RID: 27536 RVA: 0x0025BD44 File Offset: 0x00259F44
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.letter = LetterMaker.MakeLetter("DEV: Test", "Test text", LetterDefOf.PositiveEvent, null, null);
		}

		// Token: 0x06006B94 RID: 27540 RVA: 0x0025BDC4 File Offset: 0x00259FC4
		[CompilerGenerated]
		private void <Notify_QuestSignalReceived>g__ReadPawns|16_0(object obj)
		{
			Pawn pawn = obj as Pawn;
			if (pawn != null && !this.colonistsFromSignal.Contains(pawn))
			{
				this.colonistsFromSignal.Add(pawn);
			}
			List<Pawn> list = obj as List<Pawn>;
			if (list != null)
			{
				this.colonistsFromSignal.AddRange(from p in list
				where !this.colonistsFromSignal.Contains(p)
				select p);
			}
			List<Thing> list2 = obj as List<Thing>;
			if (list2 != null)
			{
				this.colonistsFromSignal.AddRange(from Pawn p in 
					from t in list2
					where t is Pawn
					select t
				where !this.colonistsFromSignal.Contains(p)
				select p);
			}
		}

		// Token: 0x04004458 RID: 17496
		public string inSignal;

		// Token: 0x04004459 RID: 17497
		public Letter letter;

		// Token: 0x0400445A RID: 17498
		public bool getLookTargetsFromSignal = true;

		// Token: 0x0400445B RID: 17499
		public MapParent useColonistsOnMap;

		// Token: 0x0400445C RID: 17500
		public string getColonistsFromSignal;

		// Token: 0x0400445D RID: 17501
		public bool useColonistsFromCaravanArg;

		// Token: 0x0400445E RID: 17502
		public string chosenPawnSignal;

		// Token: 0x0400445F RID: 17503
		public string acceptedVisitorsSignal;

		// Token: 0x04004460 RID: 17504
		public List<Pawn> visitors;

		// Token: 0x04004461 RID: 17505
		public bool filterDeadPawnsFromLookTargets;

		// Token: 0x04004462 RID: 17506
		private List<Pawn> colonistsFromSignal = new List<Pawn>();
	}
}
