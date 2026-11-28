using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013EC RID: 5100
	public class CompAbilityEffect_DropMechCluster : CompAbilityEffect_WithDest
	{
		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x06007EAE RID: 32430 RVA: 0x002D49DF File Offset: 0x002D2BDF
		public new CompProperties_DropMechCluster Props
		{
			get
			{
				return (CompProperties_DropMechCluster)this.props;
			}
		}

		// Token: 0x06007EAF RID: 32431 RVA: 0x002D49EC File Offset: 0x002D2BEC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(this.Props.points, this.parent.pawn.Map, this.Props.startDormant, false);
			MechClusterUtility.SpawnCluster(target.Cell, this.parent.pawn.Map, sketch, true, false, null);
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x002D4A4F File Offset: 0x002D2C4F
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return ModsConfig.RoyaltyActive && Faction.OfMechanoids != null && !Faction.OfMechanoids.deactivated && base.CanApplyOn(target, dest);
		}
	}
}
