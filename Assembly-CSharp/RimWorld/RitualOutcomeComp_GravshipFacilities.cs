using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D9 RID: 5849
	public class RitualOutcomeComp_GravshipFacilities : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x17001920 RID: 6432
		// (get) Token: 0x06009254 RID: 37460 RVA: 0x00356FAD File Offset: 0x003551AD
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x06009255 RID: 37461 RVA: 0x00356FB5 File Offset: 0x003551B5
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			QualityFactor qualityFactor = this.GetQualityFactor(ritual.Ritual, ritual.selectedTarget, ritual.obligation, ritual.assignments, data);
			if (qualityFactor == null)
			{
				return 0f;
			}
			return qualityFactor.quality;
		}

		// Token: 0x06009256 RID: 37462 RVA: 0x00356FE8 File Offset: 0x003551E8
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Thing thing = ritualTarget.Thing;
			Building_GravEngine building_GravEngine;
			if (thing == null)
			{
				building_GravEngine = null;
			}
			else
			{
				CompPilotConsole compPilotConsole = thing.TryGetComp<CompPilotConsole>();
				building_GravEngine = ((compPilotConsole != null) ? compPilotConsole.engine : null);
			}
			Building_GravEngine building_GravEngine2 = building_GravEngine;
			if (building_GravEngine2 == null)
			{
				return null;
			}
			float num = 0f;
			int num2 = 0;
			foreach (CompGravshipFacility compGravshipFacility in building_GravEngine2.GravshipComponents)
			{
				CompPowerTrader compPowerTrader = compGravshipFacility.parent.TryGetComp<CompPowerTrader>();
				if (compPowerTrader == null || compPowerTrader.PowerOn)
				{
					CompGravshipThruster compGravshipThruster = compGravshipFacility.parent.TryGetComp<CompGravshipThruster>();
					if (compGravshipThruster == null || compGravshipThruster.CanBeActive)
					{
						ThingDef def = compGravshipFacility.parent.def;
						float num3;
						if (this.facilityQualityOffsets.TryGetValue(def, out num3))
						{
							num += num3;
							num2++;
							this.tmpFacilityCount.TryAdd(def, 0);
							Dictionary<ThingDef, int> dictionary = this.tmpFacilityCount;
							ThingDef thingDef = def;
							int num4 = dictionary[thingDef];
							dictionary[thingDef] = num4 + 1;
						}
					}
				}
			}
			if (num2 == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ThingDef, int> keyValuePair in this.tmpFacilityCount)
			{
				ThingDef thingDef;
				int num4;
				keyValuePair.Deconstruct(out thingDef, out num4);
				ThingDef thingDef2 = thingDef;
				int num5 = num4;
				float num6;
				if (this.facilityQualityOffsets.TryGetValue(thingDef2, out num6))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 3);
					defaultInterpolatedStringHandler.AppendLiteral(" - ");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(thingDef2.LabelCap);
					defaultInterpolatedStringHandler.AppendLiteral(" x");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num5);
					defaultInterpolatedStringHandler.AppendLiteral(": +");
					defaultInterpolatedStringHandler.AppendFormatted((num6 * (float)num5).ToStringPercent());
					stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			this.tmpFacilityCount.Clear();
			int num7 = 0;
			foreach (KeyValuePair<ThingDef, float> keyValuePair2 in this.facilityQualityOffsets)
			{
				ThingDef thingDef;
				float num8;
				keyValuePair2.Deconstruct(out thingDef, out num8);
				ThingDef thingDef3 = thingDef;
				num7 += thingDef3.GetCompProperties<CompProperties_GravshipFacility>().maxSimultaneous;
			}
			string text = stringBuilder.ToString();
			return new QualityFactor
			{
				label = this.LabelForDesc.CapitalizeFirst(),
				qualityChange = "OutcomeBonusDesc_QualitySingleOffset".Translate(num.ToStringWithSign("0.#%")).Resolve(),
				count = num2.ToString() + " / " + num7.ToString(),
				quality = num,
				positive = true,
				priority = 4f,
				toolTip = (text.NullOrEmpty() ? null : text)
			};
		}

		// Token: 0x040058A2 RID: 22690
		public Dictionary<ThingDef, float> facilityQualityOffsets = new Dictionary<ThingDef, float>();

		// Token: 0x040058A3 RID: 22691
		private Dictionary<ThingDef, int> tmpFacilityCount = new Dictionary<ThingDef, int>();
	}
}
