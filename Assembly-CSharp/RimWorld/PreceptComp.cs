using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200162E RID: 5678
	public abstract class PreceptComp
	{
		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x06008E6B RID: 36459 RVA: 0x00345E0C File Offset: 0x0034400C
		public virtual IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return Enumerable.Empty<TraitRequirement>();
			}
		}

		// Token: 0x06008E6C RID: 36460 RVA: 0x00345E13 File Offset: 0x00344013
		public virtual void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
		{
		}

		// Token: 0x06008E6D RID: 36461 RVA: 0x00345E15 File Offset: 0x00344015
		public virtual void Notify_MemberWitnessedAction(HistoryEvent ev, Precept precept, Pawn member)
		{
		}

		// Token: 0x06008E6E RID: 36462 RVA: 0x00345E17 File Offset: 0x00344017
		public virtual void Notify_HistoryEvent(HistoryEvent ev, Precept precept)
		{
		}

		// Token: 0x06008E6F RID: 36463 RVA: 0x00345E19 File Offset: 0x00344019
		public virtual void Notify_MemberGenerated(Pawn pawn, Precept precept, bool newborn, bool ignoreApparel = false)
		{
		}

		// Token: 0x06008E70 RID: 36464 RVA: 0x00345E1B File Offset: 0x0034401B
		public virtual void Notify_AddBedThoughts(Pawn pawn, Precept precept)
		{
		}

		// Token: 0x06008E71 RID: 36465 RVA: 0x00345E1D File Offset: 0x0034401D
		public virtual bool MemberWillingToDo(HistoryEvent ev)
		{
			return true;
		}

		// Token: 0x06008E72 RID: 36466 RVA: 0x00345E20 File Offset: 0x00344020
		public virtual IEnumerable<string> GetDescriptions()
		{
			PreceptComp.<GetDescriptions>d__10 <GetDescriptions>d__ = new PreceptComp.<GetDescriptions>d__10(-2);
			<GetDescriptions>d__.<>4__this = this;
			return <GetDescriptions>d__;
		}

		// Token: 0x06008E73 RID: 36467 RVA: 0x00345E30 File Offset: 0x00344030
		public virtual IEnumerable<string> ConfigErrors(PreceptDef parent)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x0400574E RID: 22350
		[MustTranslate]
		public string description;

		// Token: 0x0400574F RID: 22351
		public PreceptDef preceptDef;
	}
}
