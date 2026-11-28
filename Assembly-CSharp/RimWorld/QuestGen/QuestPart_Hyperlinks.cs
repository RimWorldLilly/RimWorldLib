using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022CD RID: 8909
	public class QuestPart_Hyperlinks : QuestPart
	{
		// Token: 0x17002635 RID: 9781
		// (get) Token: 0x0600DBBD RID: 56253 RVA: 0x0051E18F File Offset: 0x0051C38F
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				if (this.cachedHyperlinks == null)
				{
					this.cachedHyperlinks = this.GetHyperlinks();
				}
				return this.cachedHyperlinks;
			}
		}

		// Token: 0x0600DBBE RID: 56254 RVA: 0x0051E1AB File Offset: 0x0051C3AB
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			QuestPart_Hyperlinks.<GetHyperlinks>d__7 <GetHyperlinks>d__ = new QuestPart_Hyperlinks.<GetHyperlinks>d__7(-2);
			<GetHyperlinks>d__.<>4__this = this;
			return <GetHyperlinks>d__;
		}

		// Token: 0x0600DBBF RID: 56255 RVA: 0x0051E1BC File Offset: 0x0051C3BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.thingDefs, "thingDefs", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingDefs == null)
				{
					this.thingDefs = new List<ThingDef>();
				}
				this.thingDefs.RemoveAll((ThingDef x) => x == null);
				if (this.pawns == null)
				{
					this.pawns = new List<Pawn>();
				}
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600DBC0 RID: 56256 RVA: 0x0051E27F File Offset: 0x0051C47F
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04008D07 RID: 36103
		public List<ThingDef> thingDefs = new List<ThingDef>();

		// Token: 0x04008D08 RID: 36104
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04008D09 RID: 36105
		public List<Faction> factions = new List<Faction>();

		// Token: 0x04008D0A RID: 36106
		public List<ResearchProjectDef> researchProjects = new List<ResearchProjectDef>();

		// Token: 0x04008D0B RID: 36107
		private IEnumerable<Dialog_InfoCard.Hyperlink> cachedHyperlinks;
	}
}
