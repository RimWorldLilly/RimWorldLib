using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1E RID: 3614
	public class ThoughtWorker_Expectations : ThoughtWorker
	{
		// Token: 0x06006128 RID: 24872 RVA: 0x00231F00 File Offset: 0x00230100
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
			if (expectationDef == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(expectationDef.thoughtStage);
		}

		// Token: 0x06006129 RID: 24873 RVA: 0x00231F38 File Offset: 0x00230138
		public override string PostProcessDescription(Pawn p, string description)
		{
			string text = base.PostProcessDescription(p, description);
			if (ModsConfig.IdeologyActive)
			{
				Ideo ideo = p.Ideo;
				Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
				if (precept_Role != null && ExpectationsUtility.OffsetByRole(p))
				{
					if (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive)
					{
						return text + "\n\n" + "RoleRaisedExpectationGracePeriod".Translate(precept_Role.LabelCap, MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
					}
					return text + "\n\n" + "RoleRaisedExpectation".Translate(precept_Role.LabelCap);
				}
			}
			return text;
		}
	}
}
