using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C57 RID: 7255
	public class Designator_ReleaseAnimalToWild : Designator
	{
		// Token: 0x0600B924 RID: 47396 RVA: 0x00414B20 File Offset: 0x00412D20
		public Designator_ReleaseAnimalToWild()
		{
			this.defaultLabel = "DesignatorReleaseAnimalToWild".Translate();
			this.defaultDesc = "DesignatorReleaseAnimalToWildDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ReleaseToTheWild", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ReleaseToWild;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.tutorTag = "ReleaseAnimalToWild";
		}

		// Token: 0x0600B925 RID: 47397 RVA: 0x00414BAC File Offset: 0x00412DAC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B926 RID: 47398 RVA: 0x00414C38 File Offset: 0x00412E38
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.IsAnimal && pawn.Faction == Faction.OfPlayer && base.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild) == null && !pawn.Dead && pawn.RaceProps.canReleaseToWild;
		}

		// Token: 0x0600B927 RID: 47399 RVA: 0x00414C98 File Offset: 0x00412E98
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in c.GetThingList(base.Map))
			{
				if (this.CanDesignateThing(t).Accepted)
				{
					this.DesignateThing(t);
				}
			}
		}

		// Token: 0x0600B928 RID: 47400 RVA: 0x00414D04 File Offset: 0x00412F04
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation((Pawn)t, DesignationDefOf.ReleaseAnimalToWild, null));
			Designation designation = base.Map.designationManager.DesignationOn(t, DesignationDefOf.Slaughter);
			if (designation != null)
			{
				base.Map.designationManager.RemoveDesignation(designation);
			}
			ReleaseAnimalToWildUtility.CheckWarnAboutBondedAnimal((Pawn)t);
		}
	}
}
