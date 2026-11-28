using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001449 RID: 5193
	public class CompAbilityEffect_Spawn : CompAbilityEffect
	{
		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x06007FD1 RID: 32721 RVA: 0x002D993B File Offset: 0x002D7B3B
		public new CompProperties_AbilitySpawn Props
		{
			get
			{
				return (CompProperties_AbilitySpawn)this.props;
			}
		}

		// Token: 0x06007FD2 RID: 32722 RVA: 0x002D9948 File Offset: 0x002D7B48
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			GenSpawn.Spawn(this.Props.thingDef, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			if (this.Props.sendSkipSignal)
			{
				CompAbilityEffect_Teleport.SendSkipUsedSignal(target, this.parent.pawn);
			}
		}

		// Token: 0x06007FD3 RID: 32723 RVA: 0x002D99A4 File Offset: 0x002D7BA4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Filled(this.parent.pawn.Map) || (!this.Props.allowOnBuildings && target.Cell.GetEdifice(this.parent.pawn.Map) != null))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityOccupiedCells".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
