using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D85 RID: 7557
	public class ITab_StudyNotesVoidMonolith : ITab_StudyNotes
	{
		// Token: 0x17002458 RID: 9304
		// (get) Token: 0x0600C3DE RID: 50142 RVA: 0x0047AA01 File Offset: 0x00478C01
		public override bool IsVisible
		{
			get
			{
				return base.Studiable && base.StudiableThing is Building_VoidMonolith && !Find.Anomaly.QuestlineEnded;
			}
		}

		// Token: 0x17002459 RID: 9305
		// (get) Token: 0x0600C3DF RID: 50143 RVA: 0x0047AA27 File Offset: 0x00478C27
		protected override IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return Find.Anomaly.MonolithLetters;
			}
		}

		// Token: 0x1700245A RID: 9306
		// (get) Token: 0x0600C3E0 RID: 50144 RVA: 0x0047AA33 File Offset: 0x00478C33
		protected override bool StudyCompleted
		{
			get
			{
				return Find.Anomaly.MonolithStudyCompleted;
			}
		}
	}
}
