using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001998 RID: 6552
	public abstract class CompStatOffsetBase : ThingComp
	{
		// Token: 0x17001C91 RID: 7313
		// (get) Token: 0x0600A470 RID: 42096 RVA: 0x003AC0DA File Offset: 0x003AA2DA
		public CompProperties_StatOffsetBase Props
		{
			get
			{
				return (CompProperties_StatOffsetBase)this.props;
			}
		}

		// Token: 0x17001C92 RID: 7314
		// (get) Token: 0x0600A471 RID: 42097 RVA: 0x003AC0E7 File Offset: 0x003AA2E7
		public virtual Pawn LastUser
		{
			get
			{
				return this.lastUser;
			}
		}

		// Token: 0x0600A472 RID: 42098
		public abstract float GetStatOffset(Pawn pawn = null);

		// Token: 0x0600A473 RID: 42099
		public abstract IEnumerable<string> GetExplanation();

		// Token: 0x0600A474 RID: 42100 RVA: 0x003AC0EF File Offset: 0x003AA2EF
		public virtual void Used(Pawn pawn)
		{
			this.lastUser = pawn;
		}

		// Token: 0x0600A475 RID: 42101 RVA: 0x003AC0F8 File Offset: 0x003AA2F8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Pawn>(ref this.lastUser, "lastUser", false);
		}

		// Token: 0x04006135 RID: 24885
		protected Pawn lastUser;
	}
}
