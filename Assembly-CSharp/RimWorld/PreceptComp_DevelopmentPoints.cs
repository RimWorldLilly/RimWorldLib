using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001634 RID: 5684
	public class PreceptComp_DevelopmentPoints : PreceptComp
	{
		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x06008E83 RID: 36483 RVA: 0x0034630C File Offset: 0x0034450C
		private string Label
		{
			get
			{
				if (this.eventLabel.NullOrEmpty())
				{
					return this.eventDef.label;
				}
				return this.eventLabel.Formatted(Array.Empty<NamedArgument>()).Resolve();
			}
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x06008E84 RID: 36484 RVA: 0x0034634C File Offset: 0x0034454C
		private string LabelCap
		{
			get
			{
				return (!this.eventLabel.NullOrEmpty()) ? this.eventLabel.Formatted(Array.Empty<NamedArgument>()).CapitalizeFirst() : this.eventDef.LabelCap;
			}
		}

		// Token: 0x06008E85 RID: 36485 RVA: 0x00346390 File Offset: 0x00344590
		public override void Notify_HistoryEvent(HistoryEvent ev, Precept precept)
		{
			if (ev.def != this.eventDef || precept.ideo != Faction.OfPlayer.ideos.FluidIdeo || !precept.ideo.development.CanBeDevelopedNow)
			{
				return;
			}
			Ideo ideo;
			ev.args.TryGetArg<Ideo>(HistoryEventArgsNames.Ideo, out ideo);
			if (ideo == null || ideo == precept.ideo)
			{
				int value = precept.ideo.development.Points;
				if (precept.ideo.development.TryAddDevelopmentPoints(this.points))
				{
					Pawn t;
					ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out t);
					Quest quest;
					ev.args.TryGetArg<Quest>(HistoryEventArgsNames.Quest, out quest);
					Messages.Message("MessageDevelopmentPointsEarned".Translate(value, precept.ideo.development.Points, this.Label), t, MessageTypeDefOf.PositiveEvent, quest, true);
				}
			}
		}

		// Token: 0x06008E86 RID: 36486 RVA: 0x00346490 File Offset: 0x00344690
		public override IEnumerable<string> GetDescriptions()
		{
			PreceptComp_DevelopmentPoints.<GetDescriptions>d__8 <GetDescriptions>d__ = new PreceptComp_DevelopmentPoints.<GetDescriptions>d__8(-2);
			<GetDescriptions>d__.<>4__this = this;
			return <GetDescriptions>d__;
		}

		// Token: 0x04005756 RID: 22358
		public HistoryEventDef eventDef;

		// Token: 0x04005757 RID: 22359
		public int points;

		// Token: 0x04005758 RID: 22360
		[MustTranslate]
		public string eventLabel;
	}
}
