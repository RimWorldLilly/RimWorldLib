using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001758 RID: 5976
	public abstract class RoleRequirement
	{
		// Token: 0x060094BF RID: 38079 RVA: 0x00363642 File Offset: 0x00361842
		public virtual string GetLabel(Precept_Role role)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x060094C0 RID: 38080 RVA: 0x00363654 File Offset: 0x00361854
		public string GetLabelCap(Precept_Role role)
		{
			return this.GetLabel(role).CapitalizeFirst();
		}

		// Token: 0x060094C1 RID: 38081
		public abstract bool Met(Pawn p, Precept_Role role);

		// Token: 0x040059A6 RID: 22950
		[NoTranslate]
		public string labelKey;
	}
}
