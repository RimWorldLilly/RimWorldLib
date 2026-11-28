using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200061D RID: 1565
	public class NewQuestLetter : ChoiceLetter
	{
		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x001395E4 File Offset: 0x001377E4
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				NewQuestLetter.<get_Choices>d__1 <get_Choices>d__ = new NewQuestLetter.<get_Choices>d__1(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x001395F4 File Offset: 0x001377F4
		public override void OpenLetter()
		{
			if (this.quest != null && !base.ArchivedOnly)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
				Find.LetterStack.RemoveLetter(this);
				return;
			}
			base.OpenLetter();
		}
	}
}
