using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001759 RID: 5977
	public class RoleRequirement_NotChild : RoleRequirement
	{
		// Token: 0x060094C3 RID: 38083 RVA: 0x0036366A File Offset: 0x0036186A
		public override string GetLabel(Precept_Role role)
		{
			return "RoleRequirementCannotBe".Translate();
		}

		// Token: 0x060094C4 RID: 38084 RVA: 0x0036367B File Offset: 0x0036187B
		public override bool Met(Pawn p, Precept_Role role)
		{
			return !p.DevelopmentalStage.Juvenile();
		}

		// Token: 0x040059A7 RID: 22951
		public static readonly RoleRequirement_NotChild Requirement = new RoleRequirement_NotChild();
	}
}
