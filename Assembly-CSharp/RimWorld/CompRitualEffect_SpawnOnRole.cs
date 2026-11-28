using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200174A RID: 5962
	public class CompRitualEffect_SpawnOnRole : CompRitualEffect_SpawnOnPawn
	{
		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x06009457 RID: 37975 RVA: 0x00360F90 File Offset: 0x0035F190
		protected new CompProperties_RitualEffectSpawnOnRole Props
		{
			get
			{
				return (CompProperties_RitualEffectSpawnOnRole)this.props;
			}
		}

		// Token: 0x06009458 RID: 37976 RVA: 0x00360F9D File Offset: 0x0035F19D
		protected override Pawn GetPawn(LordJob_Ritual ritual)
		{
			return ritual.PawnWithRole(this.Props.roleId);
		}
	}
}
