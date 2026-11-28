using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200170C RID: 5900
	public class RitualRoleAnimal : RitualRole
	{
		// Token: 0x17001954 RID: 6484
		// (get) Token: 0x0600937D RID: 37757 RVA: 0x0035D5AA File Offset: 0x0035B7AA
		public override bool Animal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600937E RID: 37758 RVA: 0x0035D5B0 File Offset: 0x0035B7B0
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			reason = null;
			if (!p.IsAnimal)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeAnimal".Translate(base.LabelCap);
				}
				return false;
			}
			if (p.BodySize < this.minBodySize)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustHaveLargerBodySize".Translate(base.LabelCap, this.minBodySize.ToString("0.00"));
				}
				return false;
			}
			if (!p.Faction.IsPlayerSafe())
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600937F RID: 37759 RVA: 0x0035D65F File Offset: 0x0035B85F
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}

		// Token: 0x06009380 RID: 37760 RVA: 0x0035D665 File Offset: 0x0035B865
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.minBodySize, "minBodySize", 0f, false);
		}

		// Token: 0x0400593C RID: 22844
		private float minBodySize;
	}
}
