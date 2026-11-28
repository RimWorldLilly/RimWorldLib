using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4E RID: 7246
	public class Designator_Hunt : Designator
	{
		// Token: 0x1700215B RID: 8539
		// (get) Token: 0x0600B8BE RID: 47294 RVA: 0x0041282E File Offset: 0x00410A2E
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x1700215C RID: 8540
		// (get) Token: 0x0600B8BF RID: 47295 RVA: 0x00412835 File Offset: 0x00410A35
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B8C0 RID: 47296 RVA: 0x0041283C File Offset: 0x00410A3C
		public Designator_Hunt()
		{
			this.defaultLabel = "DesignatorHunt".Translate();
			this.defaultDesc = "DesignatorHuntDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Hunt", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Hunt;
			this.hotKey = KeyBindingDefOf.Misc11;
		}

		// Token: 0x0600B8C1 RID: 47297 RVA: 0x004128C8 File Offset: 0x00410AC8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.HuntablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateHuntable".Translate();
			}
			return true;
		}

		// Token: 0x0600B8C2 RID: 47298 RVA: 0x00412904 File Offset: 0x00410B04
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.HuntablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0600B8C3 RID: 47299 RVA: 0x00412954 File Offset: 0x00410B54
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.AnimalOrWildMan() && !pawn.IsPrisonerInPrisonCell() && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && base.Map.designationManager.DesignationOn(pawn, this.Designation) == null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600B8C4 RID: 47300 RVA: 0x004129BC File Offset: 0x00410BBC
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x0600B8C5 RID: 47301 RVA: 0x00412A10 File Offset: 0x00410C10
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					Designator_Hunt.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind));
				}
			}
			this.justDesignated.Clear();
		}

		// Token: 0x0600B8C6 RID: 47302 RVA: 0x00412AB8 File Offset: 0x00410CB8
		private IEnumerable<Pawn> HuntablesInCell(IntVec3 c)
		{
			Designator_Hunt.<HuntablesInCell>d__11 <HuntablesInCell>d__ = new Designator_Hunt.<HuntablesInCell>d__11(-2);
			<HuntablesInCell>d__.<>4__this = this;
			<HuntablesInCell>d__.<>3__c = c;
			return <HuntablesInCell>d__;
		}

		// Token: 0x0600B8C7 RID: 47303 RVA: 0x00412AD0 File Offset: 0x00410CD0
		public static void ShowDesignationWarnings(Pawn pawn)
		{
			bool flag;
			bool flag2;
			Designator_Hunt.CheckHunters(pawn.MapHeld, out flag, out flag2);
			if (!flag)
			{
				Messages.Message("MessageNoHuntersAvailable".Translate(), pawn, MessageTypeDefOf.CautionInput, false);
			}
			else if (!flag2)
			{
				Messages.Message("MessageNoHuntersWithProperWeapon".Translate(), pawn, MessageTypeDefOf.CautionInput, false);
			}
			float manhunterOnDamageChance = pawn.RaceProps.manhunterOnDamageChance;
			float manhunterOnDamageChance2 = PawnUtility.GetManhunterOnDamageChance(pawn, null, -1f);
			if (manhunterOnDamageChance >= 0.015f)
			{
				Messages.Message("MessageAnimalsGoPsychoHunted".Translate(pawn.kindDef.GetLabelPlural(-1).CapitalizeFirst(), manhunterOnDamageChance2.ToStringPercent(), pawn.Named("ANIMAL")).CapitalizeFirst(), pawn, MessageTypeDefOf.CautionInput, false);
			}
			SlaughterDesignatorUtility.CheckWarnAboutVeneratedAnimal(pawn);
		}

		// Token: 0x0600B8C8 RID: 47304 RVA: 0x00412BB0 File Offset: 0x00410DB0
		private static void CheckHunters(Map map, out bool anyAssignedHunting, out bool anyProperWeapon)
		{
			anyAssignedHunting = false;
			anyProperWeapon = false;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				bool flag = pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hunting);
				if (flag)
				{
					anyAssignedHunting = true;
				}
				if (!pawn.Downed && flag && WorkGiver_HunterHunt.HasHuntingWeapon(pawn))
				{
					anyProperWeapon = true;
					break;
				}
			}
		}

		// Token: 0x040069B4 RID: 27060
		private readonly List<Pawn> justDesignated = new List<Pawn>();
	}
}
