using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D54 RID: 7508
	public class ChoiceLetter_ChoosePawn : ChoiceLetter
	{
		// Token: 0x170023E1 RID: 9185
		// (get) Token: 0x0600C254 RID: 49748 RVA: 0x0046D112 File Offset: 0x0046B312
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023E2 RID: 9186
		// (get) Token: 0x0600C255 RID: 49749 RVA: 0x0046D118 File Offset: 0x0046B318
		public override bool CanShowInLetterStack
		{
			get
			{
				if (!base.CanShowInLetterStack)
				{
					return false;
				}
				if (this.chosenPawnSignal.NullOrEmpty())
				{
					return false;
				}
				bool result = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull())
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x170023E3 RID: 9187
		// (get) Token: 0x0600C256 RID: 49750 RVA: 0x0046D16E File Offset: 0x0046B36E
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_ChoosePawn.<get_Choices>d__7 <get_Choices>d__ = new ChoiceLetter_ChoosePawn.<get_Choices>d__7(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x0600C257 RID: 49751 RVA: 0x0046D180 File Offset: 0x0046B380
		private DiaOption Option_ChoosePawn(Pawn p)
		{
			return new DiaOption(p.LabelCap)
			{
				action = delegate()
				{
					if (!this.chosenPawnSignal.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.chosenPawnSignal, p.Named("CHOSEN")));
					}
					Find.LetterStack.RemoveLetter(this);
				},
				resolveTree = true
			};
		}

		// Token: 0x0600C258 RID: 49752 RVA: 0x0046D1CC File Offset: 0x0046B3CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.chosenPawnSignal, "chosenPawnSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006F1A RID: 28442
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006F1B RID: 28443
		public string chosenPawnSignal;
	}
}
