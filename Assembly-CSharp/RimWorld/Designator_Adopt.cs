using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2F RID: 7215
	public class Designator_Adopt : Designator
	{
		// Token: 0x17002136 RID: 8502
		// (get) Token: 0x0600B80C RID: 47116 RVA: 0x0040F82B File Offset: 0x0040DA2B
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002137 RID: 8503
		// (get) Token: 0x0600B80D RID: 47117 RVA: 0x0040F82E File Offset: 0x0040DA2E
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x0600B80E RID: 47118 RVA: 0x0040F838 File Offset: 0x0040DA38
		public Designator_Adopt()
		{
			this.defaultLabel = "DesignatorAdopt".Translate();
			this.defaultDesc = "DesignatorAdoptDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Adopt", true);
			this.soundSucceeded = SoundDefOf.Designate_Adopt;
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x0600B80F RID: 47119 RVA: 0x0040F8A0 File Offset: 0x0040DAA0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600B810 RID: 47120 RVA: 0x0040F914 File Offset: 0x0040DB14
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x0600B811 RID: 47121 RVA: 0x0040F964 File Offset: 0x0040DB64
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn == null || pawn.IsShambler)
			{
				return false;
			}
			Designator_Adopt._tmpBuilder.Clear();
			if (t.Faction != Faction.OfPlayer && t.AdoptableBy(Faction.OfPlayer, Designator_Adopt._tmpBuilder))
			{
				return true;
			}
			return Designator_Adopt._tmpBuilder.ToString();
		}

		// Token: 0x0600B812 RID: 47122 RVA: 0x0040F9CC File Offset: 0x0040DBCC
		public override void DesignateThing(Thing t)
		{
			t.SetFaction(Faction.OfPlayer, null);
			Pawn pawn = (Pawn)t;
			Building_Bed building_Bed = pawn.CurrentBed();
			if (RestUtility.CanUseBedNow(building_Bed, pawn, false, false, null))
			{
				building_Bed.TryGetComp<CompAssignableToPawn_Bed>().TryAssignPawn(pawn);
			}
			if (this.lastDesignateFrame < RealTime.frameCount)
			{
				TaggedString taggedString = "LetterTextAdopted".Translate(pawn.Named("BABY"));
				if (pawn.babyNamingDeadline >= Find.TickManager.TicksGame)
				{
					taggedString += "\n\n" + "LetterPartNameBabyAdopt".Translate(pawn, 60000.ToStringTicksToPeriod(true, false, true, true, false));
				}
				ChoiceLetter_BabyBirth choiceLetter_BabyBirth = (ChoiceLetter_BabyBirth)LetterMaker.MakeLetter("LetterLabelAdopted".Translate(pawn.Named("BABY")), taggedString, LetterDefOf.BabyBirth, pawn, null, null, null);
				choiceLetter_BabyBirth.Start();
				Find.LetterStack.ReceiveLetter(choiceLetter_BabyBirth, null, 0, true);
			}
			this.lastDesignateFrame = RealTime.frameCount;
			foreach (IntVec3 cell in t.OccupiedRect())
			{
				FleckMaker.ThrowMetaPuffs(new TargetInfo(cell, base.Map, false));
			}
		}

		// Token: 0x0400699E RID: 27038
		private int lastDesignateFrame = -1;

		// Token: 0x0400699F RID: 27039
		private static StringBuilder _tmpBuilder = new StringBuilder();
	}
}
