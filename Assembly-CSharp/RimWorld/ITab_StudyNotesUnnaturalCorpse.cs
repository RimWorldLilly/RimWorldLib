using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D84 RID: 7556
	public class ITab_StudyNotesUnnaturalCorpse : ITab_StudyNotes
	{
		// Token: 0x17002452 RID: 9298
		// (get) Token: 0x0600C3D7 RID: 50135 RVA: 0x0047A9A8 File Offset: 0x00478BA8
		public override bool IsVisible
		{
			get
			{
				return base.Studiable && base.StudiableThing is UnnaturalCorpse;
			}
		}

		// Token: 0x17002453 RID: 9299
		// (get) Token: 0x0600C3D8 RID: 50136 RVA: 0x0047A9C2 File Offset: 0x00478BC2
		private UnnaturalCorpse UnnaturalCorpse
		{
			get
			{
				return (UnnaturalCorpse)base.StudiableThing;
			}
		}

		// Token: 0x17002454 RID: 9300
		// (get) Token: 0x0600C3D9 RID: 50137 RVA: 0x0047A9CF File Offset: 0x00478BCF
		private UnnaturalCorpseTracker Tracker
		{
			get
			{
				return this.UnnaturalCorpse.Tracker;
			}
		}

		// Token: 0x17002455 RID: 9301
		// (get) Token: 0x0600C3DA RID: 50138 RVA: 0x0047A9DC File Offset: 0x00478BDC
		protected override IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return this.Tracker.Letters;
			}
		}

		// Token: 0x17002456 RID: 9302
		// (get) Token: 0x0600C3DB RID: 50139 RVA: 0x0047A9E9 File Offset: 0x00478BE9
		protected override bool StudyCompleted
		{
			get
			{
				return this.Tracker.CanDestroyViaResearch;
			}
		}

		// Token: 0x17002457 RID: 9303
		// (get) Token: 0x0600C3DC RID: 50140 RVA: 0x0047A9F6 File Offset: 0x00478BF6
		protected override bool DrawThingIcon
		{
			get
			{
				return false;
			}
		}
	}
}
