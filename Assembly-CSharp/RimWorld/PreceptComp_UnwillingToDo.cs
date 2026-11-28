using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200163B RID: 5691
	public class PreceptComp_UnwillingToDo : PreceptComp
	{
		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x06008E98 RID: 36504 RVA: 0x00346B04 File Offset: 0x00344D04
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				PreceptComp_UnwillingToDo.<get_TraitsAffecting>d__4 <get_TraitsAffecting>d__ = new PreceptComp_UnwillingToDo.<get_TraitsAffecting>d__4(-2);
				<get_TraitsAffecting>d__.<>4__this = this;
				return <get_TraitsAffecting>d__;
			}
		}

		// Token: 0x06008E99 RID: 36505 RVA: 0x00346B14 File Offset: 0x00344D14
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			if (this.eventDef != null && ev.def != this.eventDef)
			{
				return true;
			}
			Pawn pawn;
			if (!ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn))
			{
				return false;
			}
			if (!this.preceptDef.enabledForNPCFactions && pawn != null && !pawn.Faction.IsPlayer)
			{
				return true;
			}
			if (this.nullifyingTraits != null && ((pawn != null) ? pawn.story : null) != null)
			{
				for (int i = 0; i < this.nullifyingTraits.Count; i++)
				{
					if (this.nullifyingTraits[i].HasTrait(pawn))
					{
						return true;
					}
				}
			}
			if (this.nullifyingHediffs != null)
			{
				bool flag;
				if (pawn == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_HealthTracker health = pawn.health;
					flag = (((health != null) ? health.hediffSet : null) != null);
				}
				if (flag)
				{
					for (int j = 0; j < this.nullifyingHediffs.Count; j++)
					{
						if (!pawn.health.hediffSet.HasHediff(this.nullifyingHediffs[j], false))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06008E9A RID: 36506 RVA: 0x00346C0B File Offset: 0x00344E0B
		public virtual string GetProhibitionText()
		{
			return this.description ?? this.eventDef.LabelCap;
		}

		// Token: 0x06008E9B RID: 36507 RVA: 0x00346C27 File Offset: 0x00344E27
		public override IEnumerable<string> GetDescriptions()
		{
			PreceptComp_UnwillingToDo.<GetDescriptions>d__7 <GetDescriptions>d__ = new PreceptComp_UnwillingToDo.<GetDescriptions>d__7(-2);
			<GetDescriptions>d__.<>4__this = this;
			return <GetDescriptions>d__;
		}

		// Token: 0x04005765 RID: 22373
		public HistoryEventDef eventDef;

		// Token: 0x04005766 RID: 22374
		public List<TraitRequirement> nullifyingTraits;

		// Token: 0x04005767 RID: 22375
		public List<HediffDef> nullifyingHediffs;
	}
}
