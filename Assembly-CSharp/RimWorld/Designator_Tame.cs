using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C65 RID: 7269
	public class Designator_Tame : Designator
	{
		// Token: 0x17002182 RID: 8578
		// (get) Token: 0x0600B96D RID: 47469 RVA: 0x00416132 File Offset: 0x00414332
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x17002183 RID: 8579
		// (get) Token: 0x0600B96E RID: 47470 RVA: 0x00416139 File Offset: 0x00414339
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B96F RID: 47471 RVA: 0x00416140 File Offset: 0x00414340
		public Designator_Tame()
		{
			this.defaultLabel = "DesignatorTame".Translate();
			this.defaultDesc = "DesignatorTameDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Tame", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Tame;
			this.hotKey = KeyBindingDefOf.Misc4;
			this.tutorTag = "Tame";
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x0600B970 RID: 47472 RVA: 0x004161DE File Offset: 0x004143DE
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.TameablesInCell(c).Any<Pawn>())
			{
				return "MessageMustDesignateTameable".Translate();
			}
			return true;
		}

		// Token: 0x0600B971 RID: 47473 RVA: 0x0041621C File Offset: 0x0041441C
		public override void DesignateSingleCell(IntVec3 loc)
		{
			foreach (Pawn t in this.TameablesInCell(loc))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x0600B972 RID: 47474 RVA: 0x0041626C File Offset: 0x0041446C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!TameUtility.CanTame(pawn))
			{
				return false;
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
			{
				return "CantTameScaria".Translate();
			}
			return base.Map.designationManager.DesignationOn(pawn, this.Designation) == null;
		}

		// Token: 0x0600B973 RID: 47475 RVA: 0x004162E0 File Offset: 0x004144E0
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			using (IEnumerator<PawnKindDef> enumerator = (from p in this.justDesignated
			select p.kindDef).Distinct<PawnKindDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef kind = enumerator.Current;
					TameUtility.ShowDesignationWarnings(this.justDesignated.First((Pawn x) => x.kindDef == kind), true);
				}
			}
			this.justDesignated.Clear();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTaming, KnowledgeAmount.Total);
		}

		// Token: 0x0600B974 RID: 47476 RVA: 0x00416394 File Offset: 0x00414594
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
			this.justDesignated.Add((Pawn)t);
		}

		// Token: 0x0600B975 RID: 47477 RVA: 0x004163E6 File Offset: 0x004145E6
		private IEnumerable<Pawn> TameablesInCell(IntVec3 c)
		{
			Designator_Tame.<TameablesInCell>d__11 <TameablesInCell>d__ = new Designator_Tame.<TameablesInCell>d__11(-2);
			<TameablesInCell>d__.<>4__this = this;
			<TameablesInCell>d__.<>3__c = c;
			return <TameablesInCell>d__;
		}

		// Token: 0x040069C6 RID: 27078
		private readonly List<Pawn> justDesignated = new List<Pawn>();
	}
}
