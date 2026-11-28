using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200175A RID: 5978
	public class RoleRequirement_SameIdeo : RoleRequirement
	{
		// Token: 0x060094C7 RID: 38087 RVA: 0x0036369F File Offset: 0x0036189F
		public override string GetLabel(Precept_Role role)
		{
			return this.labelKey.Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle(role.ideo.memberName, Gender.None, false, false));
		}

		// Token: 0x060094C8 RID: 38088 RVA: 0x003636CE File Offset: 0x003618CE
		public override bool Met(Pawn p, Precept_Role role)
		{
			return p.Ideo == role.ideo;
		}
	}
}
