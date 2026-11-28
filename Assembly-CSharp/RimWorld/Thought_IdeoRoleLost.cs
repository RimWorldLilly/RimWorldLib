using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D6 RID: 5590
	public class Thought_IdeoRoleLost : Thought_Memory
	{
		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x06008AE4 RID: 35556 RVA: 0x00325FF2 File Offset: 0x003241F2
		public Precept_Role Role
		{
			get
			{
				return (Precept_Role)this.sourcePrecept;
			}
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x06008AE5 RID: 35557 RVA: 0x00325FFF File Offset: 0x003241FF
		public override bool ShouldDiscard
		{
			get
			{
				return this.Role == null || this.pawn.Ideo != this.Role.ideo || this.Role.IsAssigned(this.pawn) || base.ShouldDiscard;
			}
		}

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x06008AE6 RID: 35558 RVA: 0x0032603C File Offset: 0x0032423C
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.Role.Named("ROLE"));
			}
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x06008AE7 RID: 35559 RVA: 0x00326063 File Offset: 0x00324263
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Role.Named("ROLE"));
			}
		}
	}
}
