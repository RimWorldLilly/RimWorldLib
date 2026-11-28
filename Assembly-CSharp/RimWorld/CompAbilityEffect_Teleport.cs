using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145C RID: 5212
	public class CompAbilityEffect_Teleport : CompAbilityEffect_WithDest
	{
		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x06008011 RID: 32785 RVA: 0x002DABC7 File Offset: 0x002D8DC7
		public new CompProperties_AbilityTeleport Props
		{
			get
			{
				return (CompProperties_AbilityTeleport)this.props;
			}
		}

		// Token: 0x06008012 RID: 32786 RVA: 0x002DABD4 File Offset: 0x002D8DD4
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			CompAbilityEffect_Teleport.<GetPreCastActions>d__3 <GetPreCastActions>d__ = new CompAbilityEffect_Teleport.<GetPreCastActions>d__3(-2);
			<GetPreCastActions>d__.<>4__this = this;
			return <GetPreCastActions>d__;
		}

		// Token: 0x06008013 RID: 32787 RVA: 0x002DABE4 File Offset: 0x002D8DE4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.HasThing)
			{
				base.Apply(target, dest);
				LocalTargetInfo destination = base.GetDestination(dest.IsValid ? dest : target);
				if (destination.IsValid)
				{
					Pawn pawn = this.parent.pawn;
					if (!this.parent.def.HasAreaOfEffect)
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(target.Thing, pawn.Map, 1f), target.Thing.Position, 60, null);
					}
					else
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(target.Thing, pawn.Map, 1f), target.Thing.Position, 60, null);
					}
					if (this.Props.destination == AbilityEffectDestination.Selected)
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Exit.Spawn(destination.Cell, pawn.Map, 1f), destination.Cell, 60, null);
					}
					else
					{
						this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(destination.Cell, pawn.Map, 1f), destination.Cell, 60, null);
					}
					CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
					target.Thing.Position = destination.Cell;
					Pawn pawn2 = target.Thing as Pawn;
					if (pawn2 != null)
					{
						if ((pawn2.Faction == Faction.OfPlayer || pawn2.IsPlayerControlled) && pawn2.Position.Fogged(pawn2.Map))
						{
							FloodFillerFog.FloodUnfog(pawn2.Position, pawn2.Map);
						}
						pawn2.stances.stunner.StunFor(this.Props.stunTicks.RandomInRange, this.parent.pawn, false, false, false);
						pawn2.Notify_Teleported(true, true);
						CompAbilityEffect_Teleport.SendSkipUsedSignal(pawn2.Position, pawn2);
					}
					if (this.Props.destClamorType != null)
					{
						GenClamor.DoClamor(pawn, target.Cell, (float)this.Props.destClamorRadius, this.Props.destClamorType);
					}
				}
			}
		}

		// Token: 0x06008014 RID: 32788 RVA: 0x002DAE12 File Offset: 0x002D9012
		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return base.CanPlaceSelectedTargetAt(target) && base.CanHitTarget(target);
		}

		// Token: 0x06008015 RID: 32789 RVA: 0x002DAE28 File Offset: 0x002D9028
		public override bool Valid(LocalTargetInfo target, bool showMessages = true)
		{
			AcceptanceReport report = this.CanSkipTarget(target);
			if (!report)
			{
				if (showMessages && !report.Reason.NullOrEmpty())
				{
					Pawn pawn = target.Thing as Pawn;
					if (pawn != null)
					{
						Messages.Message("CannotSkipTarget".Translate(pawn.Named("PAWN")) + ": " + report.Reason, pawn, MessageTypeDefOf.RejectInput, false);
					}
				}
				return false;
			}
			return base.Valid(target, showMessages);
		}

		// Token: 0x06008016 RID: 32790 RVA: 0x002DAEB4 File Offset: 0x002D90B4
		private AcceptanceReport CanSkipTarget(LocalTargetInfo target)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.BodySize > this.Props.maxBodySize)
				{
					return "CannotSkipTargetTooLarge".Translate();
				}
				if (pawn.kindDef.skipResistant)
				{
					return "CannotSkipTargetPsychicResistant".Translate();
				}
			}
			return true;
		}

		// Token: 0x06008017 RID: 32791 RVA: 0x002DAF18 File Offset: 0x002D9118
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			return this.CanSkipTarget(target).Reason;
		}

		// Token: 0x06008018 RID: 32792 RVA: 0x002DAF34 File Offset: 0x002D9134
		public static void SendSkipUsedSignal(LocalTargetInfo target, Thing initiator)
		{
			Find.SignalManager.SendSignal(new Signal(CompAbilityEffect_Teleport.SkipUsedSignalTag, target.Named("POSITION"), initiator.Named("SUBJECT")));
		}

		// Token: 0x04005005 RID: 20485
		public static string SkipUsedSignalTag = "CompAbilityEffect.SkipUsed";
	}
}
