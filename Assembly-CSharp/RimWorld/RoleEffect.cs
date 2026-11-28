using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200174F RID: 5967
	public abstract class RoleEffect
	{
		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x060094A8 RID: 38056 RVA: 0x003633ED File Offset: 0x003615ED
		public virtual bool IsBad
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060094A9 RID: 38057 RVA: 0x003633F0 File Offset: 0x003615F0
		public virtual string Label(Pawn pawn, Precept_Role role)
		{
			if (!this.labelKey.NullOrEmpty())
			{
				return this.labelKey.Translate(pawn.Named("PAWN"), role.LabelCap);
			}
			return this.label.Formatted(pawn.Named("PAWN"), role.LabelCap);
		}

		// Token: 0x060094AA RID: 38058 RVA: 0x00363457 File Offset: 0x00361657
		public virtual bool CanEquip(Pawn pawn, Thing thing)
		{
			return true;
		}

		// Token: 0x060094AB RID: 38059 RVA: 0x0036345A File Offset: 0x0036165A
		public virtual void Notify_Tended(Pawn doctor, Pawn target)
		{
		}

		// Token: 0x0400599F RID: 22943
		[MustTranslate]
		public string label;

		// Token: 0x040059A0 RID: 22944
		[NoTranslate]
		public string labelKey;
	}
}
