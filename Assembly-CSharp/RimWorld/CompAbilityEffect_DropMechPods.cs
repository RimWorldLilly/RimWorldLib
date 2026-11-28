using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013EE RID: 5102
	public class CompAbilityEffect_DropMechPods : CompAbilityEffect_WithDest
	{
		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x06007EB3 RID: 32435 RVA: 0x002D4A95 File Offset: 0x002D2C95
		public new CompProperties_DropMechPods Props
		{
			get
			{
				return (CompProperties_DropMechPods)this.props;
			}
		}

		// Token: 0x06007EB4 RID: 32436 RVA: 0x002D4AA4 File Offset: 0x002D2CA4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			List<Thing> list = new List<Thing>();
			int randomInRange = this.Props.numPods.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.MechanoidDropPod, null);
				thing.SetFaction(Faction.OfPlayer, null);
				list.Add(thing);
			}
			DropPodUtility.DropThingsNear(target.Cell, this.parent.pawn.MapHeld, list, 140, false, false, true, true, true, Faction.OfMechanoids);
		}

		// Token: 0x06007EB5 RID: 32437 RVA: 0x002D4B27 File Offset: 0x002D2D27
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return ModsConfig.OdysseyActive && Faction.OfMechanoids != null && !Faction.OfMechanoids.deactivated && base.CanApplyOn(target, dest);
		}

		// Token: 0x04004F74 RID: 20340
		private const int PodOpenDelay = 140;
	}
}
