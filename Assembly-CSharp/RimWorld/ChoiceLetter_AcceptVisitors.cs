using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D50 RID: 7504
	public class ChoiceLetter_AcceptVisitors : ChoiceLetter
	{
		// Token: 0x170023D4 RID: 9172
		// (get) Token: 0x0600C235 RID: 49717 RVA: 0x0046C6E6 File Offset: 0x0046A8E6
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023D5 RID: 9173
		// (get) Token: 0x0600C236 RID: 49718 RVA: 0x0046C6EC File Offset: 0x0046A8EC
		public override bool CanShowInLetterStack
		{
			get
			{
				if (!base.CanShowInLetterStack)
				{
					return false;
				}
				if (this.quest == null || this.quest.State != QuestState.Ongoing)
				{
					return false;
				}
				bool result = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.CanStillAccept(this.pawns[i]))
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x170023D6 RID: 9174
		// (get) Token: 0x0600C237 RID: 49719 RVA: 0x0046C74C File Offset: 0x0046A94C
		private DiaOption Option_Accept
		{
			get
			{
				DiaOption diaOption = new DiaOption("AcceptButton".Translate());
				diaOption.action = delegate()
				{
					this.pawns.RemoveAll((Pawn x) => !this.CanStillAccept(x));
					if (!this.acceptedSignal.NullOrEmpty())
					{
						foreach (Pawn pawn in this.pawns)
						{
							QuestUtility.SendQuestTargetSignals(pawn.questTags, "Recruited", pawn.Named("SUBJECT"));
						}
						object arg = (this.pawns.Count == 1) ? this.pawns[0] : this.pawns;
						Find.SignalManager.SendSignal(new Signal(this.acceptedSignal, arg.Named("SUBJECT")));
					}
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				bool flag = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.CanStillAccept(this.pawns[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x170023D7 RID: 9175
		// (get) Token: 0x0600C238 RID: 49720 RVA: 0x0046C7C2 File Offset: 0x0046A9C2
		private DiaOption Option_RejectWithCharityConfirmation
		{
			get
			{
				return new DiaOption("RejectLetter".Translate())
				{
					action = delegate()
					{
						Action action = delegate()
						{
							if (!this.rejectedSignal.NullOrEmpty())
							{
								object arg = (this.pawns.Count == 1) ? this.pawns[0] : this.pawns;
								Find.SignalManager.SendSignal(new Signal(this.rejectedSignal, arg.Named("SUBJECT")));
							}
							Find.LetterStack.RemoveLetter(this);
						};
						if (!ModsConfig.IdeologyActive || !this.charity)
						{
							action();
							return;
						}
						IEnumerable<Pawn> source = IdeoUtility.AllColonistsWithCharityPrecept();
						if (source.Any<Pawn>())
						{
							string text = "";
							foreach (IGrouping<Ideo, Pawn> grouping in from c in source
							group c by c.Ideo)
							{
								text = text + "\n- " + "BelieversIn".Translate(grouping.Key.name.Colorize(grouping.Key.TextColor), grouping.Select((Pawn f) => f.NameShortColored.Resolve()).ToCommaList(false, false)).Resolve();
							}
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmationCharityJoiner".Translate(text), action, false, null, WindowLayer.Dialog));
							return;
						}
						action();
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x170023D8 RID: 9176
		// (get) Token: 0x0600C239 RID: 49721 RVA: 0x0046C7F1 File Offset: 0x0046A9F1
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_AcceptVisitors.<get_Choices>d__13 <get_Choices>d__ = new ChoiceLetter_AcceptVisitors.<get_Choices>d__13(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x0600C23A RID: 49722 RVA: 0x0046C804 File Offset: 0x0046AA04
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.acceptedSignal, "acceptedSignal", null, false);
			Scribe_Values.Look<string>(ref this.rejectedSignal, "rejectedSignal", null, false);
			Scribe_Values.Look<bool>(ref this.charity, "charity", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600C23B RID: 49723 RVA: 0x0046C898 File Offset: 0x0046AA98
		private bool CanStillAccept(Pawn p)
		{
			if (p.DestroyedOrNull() || !p.SpawnedOrAnyParentSpawned)
			{
				return false;
			}
			if (p.CurJob != null && p.CurJob.exitMapOnArrival)
			{
				return false;
			}
			if (p.Faction != null && p.Faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			Lord lord = p.GetLord();
			if (lord != null)
			{
				if (lord.CurLordToil is LordToil_ExitMap || lord.CurLordToil is LordToil_ExitMapRandom)
				{
					return false;
				}
				LordJob_VisitColony lordJob_VisitColony = lord.LordJob as LordJob_VisitColony;
				if (lordJob_VisitColony != null && lordJob_VisitColony.exitSubgraph.lordToils.Contains(lord.CurLordToil))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04006F0D RID: 28429
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006F0E RID: 28430
		public string acceptedSignal;

		// Token: 0x04006F0F RID: 28431
		public string rejectedSignal;

		// Token: 0x04006F10 RID: 28432
		public bool charity;
	}
}
