using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200175B RID: 5979
	public class RoleRequirement_SupremeGender : RoleRequirement
	{
		// Token: 0x060094CA RID: 38090 RVA: 0x003636E6 File Offset: 0x003618E6
		private bool ActiveFor(Precept_Role role)
		{
			return role.restrictToSupremeGender && role.ideo.SupremeGender > Gender.None;
		}

		// Token: 0x060094CB RID: 38091 RVA: 0x00363700 File Offset: 0x00361900
		public override string GetLabel(Precept_Role role)
		{
			if (!this.ActiveFor(role))
			{
				return string.Empty;
			}
			return this.labelKey.Translate(role.ideo.SupremeGender.GetLabel(false));
		}

		// Token: 0x060094CC RID: 38092 RVA: 0x00363737 File Offset: 0x00361937
		public override bool Met(Pawn pawn, Precept_Role role)
		{
			return !this.ActiveFor(role) || pawn.gender == role.ideo.SupremeGender;
		}
	}
}
