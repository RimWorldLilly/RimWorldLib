using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200174B RID: 5963
	public class IdeoRoleInstance : IExposable
	{
		// Token: 0x0600945A RID: 37978 RVA: 0x00360FB8 File Offset: 0x0035F1B8
		public IdeoRoleInstance(Precept_Role sourceRole)
		{
			this.sourceRole = sourceRole;
		}

		// Token: 0x0600945B RID: 37979 RVA: 0x00360FC8 File Offset: 0x0035F1C8
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.abilities != null)
			{
				foreach (Ability ability in this.abilities)
				{
					ability.pawn = this.pawn;
					ability.verb.caster = this.pawn;
					ability.sourcePrecept = this.sourceRole;
				}
			}
		}

		// Token: 0x04005993 RID: 22931
		public Precept sourceRole;

		// Token: 0x04005994 RID: 22932
		public Pawn pawn;

		// Token: 0x04005995 RID: 22933
		public List<Ability> abilities;
	}
}
