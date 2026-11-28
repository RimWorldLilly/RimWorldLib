using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001694 RID: 5780
	public abstract class RitualObligationTargetFilter : IExposable
	{
		// Token: 0x060090E0 RID: 37088 RVA: 0x0035249B File Offset: 0x0035069B
		public RitualObligationTargetFilter()
		{
		}

		// Token: 0x060090E1 RID: 37089 RVA: 0x003524A3 File Offset: 0x003506A3
		public RitualObligationTargetFilter(RitualObligationTargetFilterDef def)
		{
			this.def = def;
		}

		// Token: 0x060090E2 RID: 37090 RVA: 0x003524B2 File Offset: 0x003506B2
		public RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
		{
			if ((target.Thing != null && !target.Thing.Spawned) || Find.IdeoManager.GetActiveRitualOn(target.Thing) != null)
			{
				return false;
			}
			return this.CanUseTargetInternal(target, obligation);
		}

		// Token: 0x060090E3 RID: 37091
		public abstract IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map);

		// Token: 0x060090E4 RID: 37092
		protected abstract RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation);

		// Token: 0x060090E5 RID: 37093 RVA: 0x003524F2 File Offset: 0x003506F2
		public virtual List<string> MissingTargetBuilding(Ideo ideo)
		{
			return null;
		}

		// Token: 0x060090E6 RID: 37094 RVA: 0x003524F5 File Offset: 0x003506F5
		public virtual bool ObligationTargetsValid(RitualObligation obligation)
		{
			return true;
		}

		// Token: 0x060090E7 RID: 37095
		public abstract IEnumerable<string> GetTargetInfos(RitualObligation obligation);

		// Token: 0x060090E8 RID: 37096 RVA: 0x003524F8 File Offset: 0x003506F8
		public virtual IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
		{
			RitualObligationTargetFilter.<GetBlockingIssues>d__10 <GetBlockingIssues>d__ = new RitualObligationTargetFilter.<GetBlockingIssues>d__10(-2);
			<GetBlockingIssues>d__.<>3__target = target;
			<GetBlockingIssues>d__.<>3__assignments = assignments;
			return <GetBlockingIssues>d__;
		}

		// Token: 0x060090E9 RID: 37097 RVA: 0x0035250F File Offset: 0x0035070F
		public virtual string LabelExtraPart(RitualObligation obligation)
		{
			return "";
		}

		// Token: 0x060090EA RID: 37098 RVA: 0x00352516 File Offset: 0x00350716
		public virtual bool ShouldGrayOut(Pawn pawn, ILordJobAssignmentsManager<RitualRole> assignments, out TaggedString reason)
		{
			reason = TaggedString.Empty;
			return false;
		}

		// Token: 0x060090EB RID: 37099 RVA: 0x00352524 File Offset: 0x00350724
		public virtual void CopyTo(RitualObligationTargetFilter other)
		{
			other.parent = this.parent;
			other.def = this.def;
		}

		// Token: 0x060090EC RID: 37100 RVA: 0x0035253E File Offset: 0x0035073E
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualObligationTargetFilterDef>(ref this.def, "def");
			Scribe_References.Look<Precept_Ritual>(ref this.parent, "parent", false);
		}

		// Token: 0x04005858 RID: 22616
		public Precept_Ritual parent;

		// Token: 0x04005859 RID: 22617
		public RitualObligationTargetFilterDef def;
	}
}
