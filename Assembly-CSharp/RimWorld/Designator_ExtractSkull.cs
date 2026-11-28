using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C47 RID: 7239
	public class Designator_ExtractSkull : Designator
	{
		// Token: 0x17002153 RID: 8531
		// (get) Token: 0x0600B893 RID: 47251 RVA: 0x00411BE0 File Offset: 0x0040FDE0
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x17002154 RID: 8532
		// (get) Token: 0x0600B894 RID: 47252 RVA: 0x00411BE7 File Offset: 0x0040FDE7
		public override bool Visible
		{
			get
			{
				return ModsConfig.IdeologyActive && WorkGiver_ExtractSkull.CanPlayerExtractSkull();
			}
		}

		// Token: 0x0600B895 RID: 47253 RVA: 0x00411BFC File Offset: 0x0040FDFC
		public Designator_ExtractSkull()
		{
			this.defaultLabel = "DesignatorExtractSkull".Translate();
			this.defaultDesc = "DesignatorExtractSkullDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ExtractSkull", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ExtractSkull;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.tutorTag = "ExtractSkull";
		}

		// Token: 0x0600B896 RID: 47254 RVA: 0x00411C88 File Offset: 0x0040FE88
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

		// Token: 0x0600B897 RID: 47255 RVA: 0x00411D14 File Offset: 0x0040FF14
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!this.Visible)
			{
				return false;
			}
			Corpse corpse = t as Corpse;
			if (corpse == null)
			{
				return false;
			}
			if (!corpse.InnerPawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (!corpse.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord p) => p.def == BodyPartDefOf.Head))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, DesignationDefOf.ExtractSkull) != null)
			{
				return false;
			}
			if (t.SpawnedParentOrMe is Building)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B898 RID: 47256 RVA: 0x00411DDC File Offset: 0x0040FFDC
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

		// Token: 0x0600B899 RID: 47257 RVA: 0x00411E48 File Offset: 0x00410048
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.ExtractSkull, null));
		}
	}
}
