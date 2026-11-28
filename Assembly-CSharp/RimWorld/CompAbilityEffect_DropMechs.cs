using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F0 RID: 5104
	public class CompAbilityEffect_DropMechs : CompAbilityEffect_WithDest
	{
		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x06007EB8 RID: 32440 RVA: 0x002D4B6D File Offset: 0x002D2D6D
		public new CompProperties_DropMechs Props
		{
			get
			{
				return (CompProperties_DropMechs)this.props;
			}
		}

		// Token: 0x06007EB9 RID: 32441 RVA: 0x002D4B7C File Offset: 0x002D2D7C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			PawnGroupMakerParms parms = new PawnGroupMakerParms
			{
				faction = Faction.OfMechanoids,
				groupKind = PawnGroupKindDefOf.Combat,
				points = this.Props.points.RandomInRange,
				tile = this.parent.pawn.MapHeld.Tile
			};
			DropPodUtility.DropThingsNear(target.Cell, this.parent.pawn.MapHeld, PawnGroupMakerUtility.GeneratePawns(parms, true).ToList<Pawn>(), 140, false, false, true, true, true, Faction.OfMechanoids);
		}

		// Token: 0x06007EBA RID: 32442 RVA: 0x002D4C15 File Offset: 0x002D2E15
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return ModsConfig.OdysseyActive && Faction.OfMechanoids != null && !Faction.OfMechanoids.deactivated && base.CanApplyOn(target, dest);
		}

		// Token: 0x04004F76 RID: 20342
		private const int PodOpenDelay = 140;
	}
}
