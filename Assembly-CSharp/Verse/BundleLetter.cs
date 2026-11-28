using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000618 RID: 1560
	public class BundleLetter : Letter
	{
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060030C8 RID: 12488 RVA: 0x001387E2 File Offset: 0x001369E2
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x001387E8 File Offset: 0x001369E8
		public void SetLetters(List<Letter> letters)
		{
			if (GenCollection.ListsEqual<Letter>(letters, this.bundledLetters))
			{
				return;
			}
			this.bundledLetters.Clear();
			this.bundledLetters.AddRange(letters);
			this.floatMenuOptions.Clear();
			using (List<Letter>.Enumerator enumerator = letters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Letter letter = enumerator.Current;
					FloatMenuOption item = new FloatMenuOption(letter.Label, delegate()
					{
						letter.OpenLetter();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					this.floatMenuOptions.Add(item);
				}
			}
			base.Label = this.bundledLetters.Count.ToString() + " " + "MoreLower".Translate() + "...";
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x001388E4 File Offset: 0x00136AE4
		public override void OpenLetter()
		{
			if (Event.current.button == 0)
			{
				Find.WindowStack.Add(new FloatMenu(this.floatMenuOptions));
			}
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x00138908 File Offset: 0x00136B08
		protected override string GetMouseoverText()
		{
			return "MoreLetters".Translate(this.bundledLetters.Count) + ":\n\n" + (from l in this.bundledLetters
			select l.Label.ToString()).ToLineList(" - ", false);
		}

		// Token: 0x04001F6A RID: 8042
		private List<Letter> bundledLetters = new List<Letter>();

		// Token: 0x04001F6B RID: 8043
		private List<FloatMenuOption> floatMenuOptions = new List<FloatMenuOption>();
	}
}
