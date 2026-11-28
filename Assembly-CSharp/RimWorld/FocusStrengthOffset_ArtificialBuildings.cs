using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001901 RID: 6401
	public class FocusStrengthOffset_ArtificialBuildings : FocusStrengthOffset_Curve
	{
		// Token: 0x17001BA8 RID: 7080
		// (get) Token: 0x0600A076 RID: 41078 RVA: 0x003969A3 File Offset: 0x00394BA3
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_NearbyArtificialStructures";
			}
		}

		// Token: 0x0600A077 RID: 41079 RVA: 0x003969AA File Offset: 0x00394BAA
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return parent.Spawned;
		}

		// Token: 0x0600A078 RID: 41080 RVA: 0x003969B2 File Offset: 0x00394BB2
		protected override float SourceValue(Thing parent)
		{
			return (float)(parent.Spawned ? parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius).Count : 0);
		}

		// Token: 0x0600A079 RID: 41081 RVA: 0x003969E1 File Offset: 0x00394BE1
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			MeditationUtility.DrawArtificialBuildingOverlay(parent.Position, parent.def, parent.Map, this.radius);
		}

		// Token: 0x0600A07A RID: 41082 RVA: 0x00396A08 File Offset: 0x00394C08
		public override string InspectStringExtra(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				List<Thing> forCell = parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius);
				if (forCell.Count > 0)
				{
					IEnumerable<string> source = (from c in forCell
					select GenLabel.ThingLabel(c, 1, false, true)).Distinct<string>();
					TaggedString taggedString = "MeditationFocusImpacted".Translate() + ": " + source.Take(3).ToCommaList(false, false).CapitalizeFirst();
					if (source.Count<string>() > 3)
					{
						taggedString += " " + "Etc".Translate();
					}
					return taggedString;
				}
			}
			return "";
		}

		// Token: 0x04005EC8 RID: 24264
		public float radius = 10f;
	}
}
