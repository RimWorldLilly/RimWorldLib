using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9C RID: 7580
	public class MainTabWindow_Mechs : MainTabWindow_PawnTable
	{
		// Token: 0x17002493 RID: 9363
		// (get) Token: 0x0600C4BC RID: 50364 RVA: 0x004809B9 File Offset: 0x0047EBB9
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Mechs;
			}
		}

		// Token: 0x17002494 RID: 9364
		// (get) Token: 0x0600C4BD RID: 50365 RVA: 0x004809C0 File Offset: 0x0047EBC0
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
				where p.RaceProps.IsMechanoid && p.OverseerSubject != null
				select p;
			}
		}

		// Token: 0x0600C4BE RID: 50366 RVA: 0x004809FC File Offset: 0x0047EBFC
		public override void PostOpen()
		{
			base.PostOpen();
			this.cachedColors = (from c in DefDatabase<ColorDef>.AllDefsListForReading
			select c.color).Concat(from f in Find.FactionManager.AllFactionsVisible
			select f.Color).Distinct<Color>().ToList<Color>();
			this.cachedColors.SortByColor((Color c) => c);
		}

		// Token: 0x0600C4BF RID: 50367 RVA: 0x00480AA8 File Offset: 0x0047ECA8
		public override void DoWindowContents(Rect rect)
		{
			if (!ModLister.BiotechInstalled)
			{
				return;
			}
			base.DoWindowContents(rect);
			Rect rect2 = new Rect(rect.x, rect.y, 240f, 32f);
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(rect2, "ChooseMechColor".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ChooseColor("ChooseMechAccentColor".Translate(), Find.FactionManager.OfPlayer.AllegianceColor, this.cachedColors, delegate(Color color)
				{
					Find.FactionManager.OfPlayer.AllegianceColor = color;
					foreach (Pawn dirty in MechanitorUtility.MechsInPlayerFaction())
					{
						PortraitsCache.SetDirty(dirty);
					}
				}));
			}
		}

		// Token: 0x04007064 RID: 28772
		private List<Color> cachedColors = new List<Color>();

		// Token: 0x04007065 RID: 28773
		private const int SetColorButtonHeight = 32;

		// Token: 0x04007066 RID: 28774
		private const int SetColorButtonWidth = 240;
	}
}
