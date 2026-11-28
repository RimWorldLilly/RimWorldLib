using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C50 RID: 7248
	public class Designator_Mine : Designator_Cells
	{
		// Token: 0x1700215E RID: 8542
		// (get) Token: 0x0600B8D2 RID: 47314 RVA: 0x00413015 File Offset: 0x00411215
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700215F RID: 8543
		// (get) Token: 0x0600B8D3 RID: 47315 RVA: 0x00413018 File Offset: 0x00411218
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Mine;
			}
		}

		// Token: 0x17002160 RID: 8544
		// (get) Token: 0x0600B8D4 RID: 47316 RVA: 0x0041301F File Offset: 0x0041121F
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Mine;
			}
		}

		// Token: 0x0600B8D5 RID: 47317 RVA: 0x00413028 File Offset: 0x00411228
		public Designator_Mine()
		{
			this.defaultLabel = "DesignatorMine".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine", true);
			this.defaultDesc = "DesignatorMineDesc".Translate();
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Mine;
			this.removeAllOtherDesignationDefs = new DesignationDef[]
			{
				DesignationDefOf.MineVein
			};
			this.hotKey = KeyBindingDefOf.Misc10;
			this.tutorTag = "Mine";
		}

		// Token: 0x0600B8D6 RID: 47318 RVA: 0x004130C8 File Offset: 0x004112C8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			if (c.Fogged(base.Map))
			{
				return true;
			}
			Mineable firstMineable = c.GetFirstMineable(base.Map);
			if (firstMineable == null)
			{
				return "MessageMustDesignateMineable".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(firstMineable);
			if (!result.Accepted)
			{
				return result;
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600B8D7 RID: 47319 RVA: 0x00413158 File Offset: 0x00411358
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.mineable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(t.Position, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			if (base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.MineVein) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			return true;
		}

		// Token: 0x0600B8D8 RID: 47320 RVA: 0x004131C8 File Offset: 0x004113C8
		public override void DesignateSingleCell(IntVec3 loc)
		{
			base.Map.designationManager.AddDesignation(new Designation(loc, this.Designation, null));
			base.Map.designationManager.TryRemoveDesignation(loc, DesignationDefOf.SmoothWall);
			Designator_Mine.PossiblyWarnPlayerOnDesignatingMining();
			if (DebugSettings.godMode)
			{
				Mineable firstMineable = loc.GetFirstMineable(base.Map);
				if (firstMineable != null)
				{
					firstMineable.DestroyMined(null);
				}
			}
		}

		// Token: 0x0600B8D9 RID: 47321 RVA: 0x00413230 File Offset: 0x00411430
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x0600B8DA RID: 47322 RVA: 0x0041323E File Offset: 0x0041143E
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Mining, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x0600B8DB RID: 47323 RVA: 0x00413251 File Offset: 0x00411451
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x0600B8DC RID: 47324 RVA: 0x00413258 File Offset: 0x00411458
		protected static void PossiblyWarnPlayerOnDesignatingMining()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			Designator_Mine.tmpIdeoMemberNames.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				if (ideo.WarnPlayerOnDesignateMine)
				{
					Designator_Mine.tmpIdeoMemberNames.Add(Find.ActiveLanguageWorker.Pluralize(ideo.memberName, -1));
				}
			}
			if (Designator_Mine.tmpIdeoMemberNames.Any<string>())
			{
				Messages.Message("MessageWarningPlayerDesignatedMining".Translate(Designator_Mine.tmpIdeoMemberNames.ToCommaList(true, false)), MessageTypeDefOf.CautionInput, false);
			}
			Designator_Mine.tmpIdeoMemberNames.Clear();
		}

		// Token: 0x040069B7 RID: 27063
		private static readonly List<string> tmpIdeoMemberNames = new List<string>();
	}
}
