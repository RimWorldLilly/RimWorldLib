using System;

namespace RimWorld
{
	// Token: 0x02001591 RID: 5521
	public struct ApparelRequirementWithSource
	{
		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x06008781 RID: 34689 RVA: 0x0030FA2B File Offset: 0x0030DC2B
		public ApparelRequirementSource Source
		{
			get
			{
				if (this.sourceRole != null)
				{
					return ApparelRequirementSource.Role;
				}
				if (this.sourceTitle != null)
				{
					return ApparelRequirementSource.Title;
				}
				return ApparelRequirementSource.Invalid;
			}
		}

		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x06008782 RID: 34690 RVA: 0x0030FA42 File Offset: 0x0030DC42
		public string SourceLabelCap
		{
			get
			{
				if (this.sourceTitle != null)
				{
					return this.sourceTitle.def.GetLabelCapFor(this.sourceTitle.pawn);
				}
				return this.sourceRole.LabelCap;
			}
		}

		// Token: 0x06008783 RID: 34691 RVA: 0x0030FA73 File Offset: 0x0030DC73
		public ApparelRequirementWithSource(ApparelRequirement requirement, Precept_Role role)
		{
			this.requirement = requirement;
			this.sourceRole = role;
			this.sourceTitle = null;
		}

		// Token: 0x06008784 RID: 34692 RVA: 0x0030FA8A File Offset: 0x0030DC8A
		public ApparelRequirementWithSource(ApparelRequirement requirement, RoyalTitle title)
		{
			this.requirement = requirement;
			this.sourceTitle = title;
			this.sourceRole = null;
		}

		// Token: 0x040053B2 RID: 21426
		public ApparelRequirement requirement;

		// Token: 0x040053B3 RID: 21427
		public Precept_Role sourceRole;

		// Token: 0x040053B4 RID: 21428
		public RoyalTitle sourceTitle;
	}
}
