using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D9 RID: 5593
	public class Thought_IdeoRoleApparelRequirementNotMet : Thought_Situational
	{
		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x06008AF3 RID: 35571 RVA: 0x00326243 File Offset: 0x00324443
		public Precept_Role Role
		{
			get
			{
				return (Precept_Role)this.sourcePrecept;
			}
		}

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x06008AF4 RID: 35572 RVA: 0x00326250 File Offset: 0x00324450
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.Role.Named("ROLE"));
			}
		}

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x06008AF5 RID: 35573 RVA: 0x00326278 File Offset: 0x00324478
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Role.Named("ROLE")) + ":\n\n" + this.GetAllRequiredApparel(this.pawn).ToLineList(" -  ");
			}
		}

		// Token: 0x06008AF6 RID: 35574 RVA: 0x003262D0 File Offset: 0x003244D0
		protected override ThoughtState CurrentStateInternal()
		{
			using (IEnumerator<Pawn> enumerator = this.Role.ChosenPawns().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == this.pawn && this.GetAllRequiredApparel(this.pawn).Count > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008AF7 RID: 35575 RVA: 0x00326348 File Offset: 0x00324548
		private List<string> GetAllRequiredApparel(Pawn p)
		{
			this.unmetReqsListTmp.Clear();
			if (ModsConfig.IdeologyActive)
			{
				Ideo ideo = p.Ideo;
				Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
				if (precept_Role != null && !precept_Role.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					for (int i = 0; i < precept_Role.apparelRequirements.Count; i++)
					{
						ApparelRequirement requirement = precept_Role.apparelRequirements[i].requirement;
						if (requirement.IsActive(p) && !requirement.IsMet(p))
						{
							if (!requirement.groupLabel.NullOrEmpty())
							{
								this.unmetReqsListTmp.Add(requirement.groupLabel);
							}
							else
							{
								foreach (ThingDef thingDef in requirement.AllRequiredApparelForPawn(p, false, false))
								{
									this.unmetReqsListTmp.Add(thingDef.LabelCap);
								}
							}
						}
					}
				}
			}
			return this.unmetReqsListTmp;
		}

		// Token: 0x0400557B RID: 21883
		private List<string> unmetReqsListTmp = new List<string>();
	}
}
