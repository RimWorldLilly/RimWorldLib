using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001443 RID: 5187
	public class CompAbilityEffect_PsychicSlaughter : CompAbilityEffect
	{
		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x06007FBC RID: 32700 RVA: 0x002D941C File Offset: 0x002D761C
		public new CompProperties_PsychicSlaughter Props
		{
			get
			{
				return (CompProperties_PsychicSlaughter)this.props;
			}
		}

		// Token: 0x06007FBD RID: 32701 RVA: 0x002D942C File Offset: 0x002D762C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			Pawn pawn = target.Pawn;
			Map map = pawn.MapHeld;
			IntVec3 positionHeld = pawn.PositionHeld;
			base.Apply(target, dest);
			int num = Mathf.Max(GenMath.RoundRandom(pawn.GetStatValue(StatDefOf.MeatAmount, true, -1)), 3);
			pawn.Kill(new DamageInfo?(new DamageInfo(DamageDefOf.Psychic, 99999f, 0f, -1f, this.parent.pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false)), null);
			if (pawn.Faction != null && pawn.HomeFaction != null && pawn.HomeFaction != Faction.OfPlayer && pawn.HomeFaction.def.humanlikeFaction && !pawn.Faction.def.PermanentlyHostileTo(FactionDefOf.PlayerColony))
			{
				pawn.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -100, true, true, HistoryEventDefOf.MemberKilled, null);
			}
			Corpse corpse = pawn.Corpse;
			if (corpse != null)
			{
				corpse.Destroy(DestroyMode.Vanish);
			}
			EffecterDefOf.MeatExplosion.Spawn(positionHeld, map, 1f).Cleanup();
			FleshbeastUtility.MeatSplatter(CompAbilityEffect_PsychicSlaughter.BloodFilth.RandomInRange, positionHeld, map, FleshbeastUtility.MeatExplosionSize.Large);
			int num2 = CompAbilityEffect_PsychicSlaughter.MeatPieces.RandomInRange;
			int num3 = Mathf.CeilToInt((float)num / (float)ThingDefOf.Meat_Twisted.stackLimit);
			if (num2 < num3)
			{
				num2 = num3;
			}
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < num2; i++)
			{
				IntVec3 near = positionHeld;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 x) => x.Walkable(map)));
				}
				IntVec3 loc;
				if (RCellFinder.TryFindRandomCellNearWith(near, validator, map, out loc, 1, 4))
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Meat_Twisted, null);
					thing.stackCount = num / num2;
					GenSpawn.Spawn(thing, loc, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x06007FBE RID: 32702 RVA: 0x002D95FD File Offset: 0x002D77FD
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}

		// Token: 0x06007FBF RID: 32703 RVA: 0x002D9600 File Offset: 0x002D7800
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x06007FC0 RID: 32704 RVA: 0x002D960C File Offset: 0x002D780C
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!pawn.RaceProps.IsFlesh)
			{
				if (throwMessages)
				{
					Messages.Message("MessageSlaughterNoFlesh".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			if ((double)pawn.BodySize > 2.5)
			{
				if (throwMessages)
				{
					Messages.Message("MessageSlaughterTooBig".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x04004FE9 RID: 20457
		private static readonly IntRange MeatPieces = new IntRange(3, 4);

		// Token: 0x04004FEA RID: 20458
		private static readonly IntRange BloodFilth = new IntRange(3, 4);
	}
}
