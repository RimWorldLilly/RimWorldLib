using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001954 RID: 6484
	public class CompReportWorkSpeed : ThingComp
	{
		// Token: 0x17001C33 RID: 7219
		// (get) Token: 0x0600A2C8 RID: 41672 RVA: 0x003A25EA File Offset: 0x003A07EA
		public CompProperties_ReportWorkSpeed Props
		{
			get
			{
				return (CompProperties_ReportWorkSpeed)this.props;
			}
		}

		// Token: 0x0600A2C9 RID: 41673 RVA: 0x003A25F8 File Offset: 0x003A07F8
		public override string CompInspectStringExtra()
		{
			if (this.parent.def.statBases == null)
			{
				return null;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			foreach (StatDef statDef in DefDatabase<StatDef>.AllDefsListForReading)
			{
				if (((statDef != null) ? statDef.parts : null) != null && !statDef.Worker.IsDisabledFor(this.parent))
				{
					foreach (StatPart statPart in statDef.parts)
					{
						if (statPart is StatPart_WorkTableOutdoors || statPart is StatPart_Outdoors)
						{
							flag = true;
						}
						else if (statPart is StatPart_WorkTableTemperature)
						{
							flag2 = true;
						}
						else if (statPart is StatPart_WorkTableUnpowered)
						{
							flag3 = true;
						}
						else if (statPart is StatPart_WorkTableRoomRole)
						{
							flag4 = true;
						}
					}
				}
			}
			StatDef statDef2 = this.Props.workSpeedStat ?? StatDefOf.WorkTableWorkSpeedFactor;
			float statValue = this.parent.GetStatValue(statDef2, true, -1);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(statDef2.LabelCap);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(statValue.ToStringPercent());
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			string text2 = string.Empty;
			bool flag5 = flag && StatPart_WorkTableOutdoors.Applies(this.parent.def, this.parent.Map, this.parent.Position);
			bool flag6 = flag2 && StatPart_WorkTableTemperature.Applies(this.parent);
			bool flag7 = flag3 && StatPart_WorkTableUnpowered.Applies(this.parent);
			bool flag8 = flag4 && StatPart_WorkTableRoomRole.Applies(this.parent);
			if (flag5)
			{
				text2 += "Outdoors".Translate();
			}
			if (flag6)
			{
				string text3 = "BadTemperature".Translate();
				if (!text2.NullOrEmpty())
				{
					text2 = text2 + ", " + text3;
				}
				else
				{
					text2 += text3;
				}
			}
			if (flag7)
			{
				string text4 = "NoPower".Translate();
				if (!text2.NullOrEmpty())
				{
					text2 = text2 + ", " + text4;
				}
				else
				{
					text2 += text4;
				}
			}
			if (flag8)
			{
				string text5 = "NotInRoomRole".Translate(this.parent.def.building.workTableRoomRole.label);
				if (!text2.NullOrEmpty())
				{
					text2 = text2 + ", " + text5;
				}
				else
				{
					text2 += text5;
				}
			}
			CompAffectedByFacilities comp = this.parent.GetComp<CompAffectedByFacilities>();
			if (comp != null)
			{
				foreach (Thing thing in comp.LinkedFacilitiesListForReading)
				{
					if (thing.def.GetCompProperties<CompProperties_Facility>().statOffsets.GetStatOffsetFromList(statDef2) != 0f)
					{
						string label = thing.def.label;
						if (!text2.NullOrEmpty())
						{
							text2 = text2 + ", " + label;
						}
						else
						{
							text2 += label;
						}
					}
				}
			}
			if (!text2.NullOrEmpty())
			{
				text = text + " (" + text2 + ")";
			}
			return text;
		}
	}
}
